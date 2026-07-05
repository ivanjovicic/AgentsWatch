using AgentsWatch.Core;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RunEvidenceFoundationTests
{
    [Theory]
    [InlineData("src/Payments/Retry.cs", "src/**", true)]
    [InlineData("tests/Payments/RetryTests.cs", "tests/**", true)]
    [InlineData("src/Auth/TokenService.cs", "src/Payments/**", false)]
    [InlineData("src\\Payments\\Retry.cs", "src/**", true)]
    [InlineData("README.md", "*.md", true)]
    [InlineData("Root.cs", "**/*.cs", true)]
    [InlineData("src/Nested.cs", "**/*.cs", true)]
    public void ScopeMatcher_supports_cross_platform_globs(string path, string pattern, bool expected)
    {
        var actual = ScopeMatcher.IsAllowed(path, [pattern]);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ScopeMatcher_treats_empty_scope_as_unrestricted()
    {
        Assert.True(ScopeMatcher.IsAllowed("any/path.txt", []));
    }

    [Theory]
    [InlineData("../escape")]
    [InlineData("task/name")]
    [InlineData("task name")]
    [InlineData("")]
    public void RunId_rejects_unsafe_values(string taskId)
    {
        Assert.Throws<ArgumentException>(() => RunId.Validate(taskId));
    }

    [Theory]
    [InlineData(".agentwatch/runs/TASK-001.json", true)]
    [InlineData(".ai\\runs\\TASK-001.md", true)]
    [InlineData(".agentwatch/runs/TASK-002.json", false)]
    [InlineData("src/TASK-001.json", false)]
    public void RunArtifactPaths_only_excludes_current_run_outputs(string path, bool expected)
    {
        Assert.Equal(expected, RunArtifactPaths.IsCurrentRunArtifact(path, "TASK-001"));
    }

    [Fact]
    public void GitNameStatusParser_parses_common_and_rename_statuses()
    {
        const string output = "M\tsrc/One.cs\nA\tsrc/Two.cs\nD\tsrc/Three.cs\nR100\tsrc/Old.cs\tsrc/New.cs\n";

        var files = GitNameStatusParser.Parse(output);

        Assert.Collection(
            files,
            file =>
            {
                Assert.Equal("src/One.cs", file.Path);
                Assert.Equal("modified", file.Status);
            },
            file =>
            {
                Assert.Equal("src/Two.cs", file.Path);
                Assert.Equal("added", file.Status);
            },
            file =>
            {
                Assert.Equal("src/Three.cs", file.Path);
                Assert.Equal("deleted", file.Status);
            },
            file =>
            {
                Assert.Equal("src/New.cs", file.Path);
                Assert.Equal("renamed", file.Status);
            });
    }

    [Fact]
    public async Task GitChangeSetReader_combines_tracked_and_untracked_files_without_duplicates()
    {
        var runner = new StubGitCommandRunner("M\tsrc/Tracked.cs\nA\tsrc/New.cs\n");
        var snapshot = new GitSnapshot(
            "feature/test",
            new string('b', 40),
            [
                new ChangedFile("src/Tracked.cs", " M"),
                new ChangedFile("notes.txt", "??")
            ],
            " M src/Tracked.cs\n?? notes.txt");

        var files = await new GitChangeSetReader(runner).ReadSinceAsync(
            "/repo",
            new string('a', 40),
            snapshot);

        Assert.Collection(
            files,
            file =>
            {
                Assert.Equal("notes.txt", file.Path);
                Assert.Equal("untracked", file.Status);
            },
            file =>
            {
                Assert.Equal("src/New.cs", file.Path);
                Assert.Equal("added", file.Status);
            },
            file =>
            {
                Assert.Equal("src/Tracked.cs", file.Path);
                Assert.Equal("modified", file.Status);
            });
        Assert.Contains("diff --name-status --find-renames", runner.LastArguments);
    }

    [Fact]
    public async Task GitChangeSetReader_rejects_non_object_revision_input()
    {
        var reader = new GitChangeSetReader(new StubGitCommandRunner(string.Empty));
        var snapshot = new GitSnapshot("main", new string('b', 40), [], string.Empty);

        await Assert.ThrowsAsync<ArgumentException>(
            () => reader.ReadSinceAsync("/repo", "--output=/tmp/file", snapshot));
    }

    [Fact]
    public async Task RunManifestStore_round_trips_refuses_overwrite_and_uses_private_sidecar_path()
    {
        var root = CreateTemporaryDirectory();
        try
        {
            var snapshot = new GitSnapshot("feature/test", new string('a', 40), [], string.Empty);
            var manifest = RunManifest.Start(
                "TASK-001",
                "Test run",
                snapshot,
                ["src/**"],
                DateTimeOffset.Parse("2026-07-05T12:00:00Z"));
            var store = new RunManifestStore();

            var path = await store.CreateAsync(root, manifest);
            var loaded = await store.LoadAsync(root, "TASK-001");

            Assert.Equal(Path.Combine(root, ".agentwatch", "runs", "TASK-001.json"), path);
            Assert.True(File.Exists(path));
            Assert.Equal(manifest.SchemaVersion, loaded.SchemaVersion);
            Assert.Equal(manifest.TaskId, loaded.TaskId);
            Assert.Equal(manifest.Title, loaded.Title);
            Assert.Equal(manifest.StartedAt, loaded.StartedAt);
            Assert.Equal(RunLifecycleStatus.InProgress, loaded.Status);
            Assert.Equal(ValidationEvidenceStatus.NotRun, loaded.ValidationStatus);
            Assert.Equal(manifest.StartBranch, loaded.StartBranch);
            Assert.Equal(manifest.StartCommitSha, loaded.StartCommitSha);
            Assert.Equal(manifest.AllowedPaths, loaded.AllowedPaths);
            Assert.Empty(loaded.ChangedFiles);
            Assert.Empty(loaded.OutOfScopeFiles);
            Assert.Empty(loaded.Warnings);
            await Assert.ThrowsAsync<IOException>(() => store.CreateAsync(root, manifest));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task RunManifestStore_finds_active_and_latest_runs()
    {
        var root = CreateTemporaryDirectory();
        try
        {
            var startSnapshot = new GitSnapshot("main", new string('a', 40), [], string.Empty);
            var endSnapshot = new GitSnapshot("main", new string('b', 40), [], string.Empty);
            var olderFinished = RunManifest.Start(
                    "TASK-OLD",
                    "Older finished run",
                    startSnapshot,
                    [],
                    DateTimeOffset.Parse("2026-07-05T10:00:00Z"))
                .Complete(
                    endSnapshot,
                    [],
                    [],
                    ["Validation was not run."],
                    DateTimeOffset.Parse("2026-07-05T10:30:00Z"));
            var newerActive = RunManifest.Start(
                "TASK-NEW",
                "Newer active run",
                endSnapshot,
                ["src/**"],
                DateTimeOffset.Parse("2026-07-05T11:00:00Z"));
            var store = new RunManifestStore();

            await store.CreateAsync(root, olderFinished);
            await store.CreateAsync(root, newerActive);

            var active = await store.FindActiveAsync(root);
            var latest = await store.FindLatestAsync(root);

            Assert.Single(active);
            Assert.Equal("TASK-NEW", active[0].TaskId);
            Assert.NotNull(latest);
            Assert.Equal("TASK-NEW", latest.TaskId);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task RunManifestStore_rejects_unknown_schema()
    {
        var root = CreateTemporaryDirectory();
        try
        {
            var manifest = new RunManifest(
                "99.0",
                "TASK-003",
                "Unsupported schema",
                DateTimeOffset.UtcNow,
                null,
                RunLifecycleStatus.InProgress,
                ValidationEvidenceStatus.NotRun,
                "main",
                new string('a', 40),
                null,
                null,
                [],
                [],
                [],
                []);

            await Assert.ThrowsAsync<InvalidDataException>(
                () => new RunManifestStore().CreateAsync(root, manifest));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Markdown_report_is_deterministic_honest_and_omits_local_root()
    {
        var startSnapshot = new GitSnapshot("main", new string('a', 40), [], string.Empty);
        var endSnapshot = new GitSnapshot("main", new string('b', 40), [], string.Empty);
        var manifest = RunManifest.Start(
                "TASK-002",
                "Evidence boundary",
                startSnapshot,
                ["src/**"],
                DateTimeOffset.Parse("2026-07-05T12:00:00Z"))
            .Complete(
                endSnapshot,
                [
                    new ChangedFile("src/Zeta.cs", "modified"),
                    new ChangedFile("src/Alpha.cs", "added")
                ],
                [new ChangedFile("docs/Unexpected.md", "added")],
                ["Repository has uncommitted changes at finish time."],
                DateTimeOffset.Parse("2026-07-05T12:30:00Z"));

        var output = new MarkdownRunEvidenceFormatter().Format(manifest);

        Assert.Contains("- Status: Finished", output);
        Assert.Contains("- Validation: NotRun", output);
        Assert.True(
            output.IndexOf("`src/Alpha.cs`", StringComparison.Ordinal)
            < output.IndexOf("`src/Zeta.cs`", StringComparison.Ordinal));
        Assert.Contains("`docs/Unexpected.md` — added", output);
        Assert.Contains("no build, test, CI, UI, database, runtime, or agent-claim evidence was captured", output);
        Assert.DoesNotContain("/repo", output);
    }

    private static string CreateTemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "agentswatch-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private sealed class StubGitCommandRunner : IGitCommandRunner
    {
        private readonly string _output;

        public StubGitCommandRunner(string output)
        {
            _output = output;
        }

        public string LastArguments { get; private set; } = string.Empty;

        public Task<string> RunAsync(
            string workingDirectory,
            string arguments,
            CancellationToken cancellationToken = default)
        {
            LastArguments = arguments;
            return Task.FromResult(_output);
        }
    }
}
