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
    public async Task RunManifestStore_round_trips_and_refuses_overwrite()
    {
        var root = CreateTemporaryDirectory();
        try
        {
            var snapshot = new GitSnapshot("feature/test", new string('a', 40), [], string.Empty);
            var manifest = RunManifest.Start(
                "TASK-001",
                "Test run",
                root,
                snapshot,
                ["src/**"],
                DateTimeOffset.Parse("2026-07-05T12:00:00Z"));
            var store = new RunManifestStore();

            var path = await store.CreateAsync(root, manifest);
            var loaded = await store.LoadAsync(root, "TASK-001");

            Assert.True(File.Exists(path));
            Assert.Equal(manifest, loaded);
            await Assert.ThrowsAsync<IOException>(() => store.CreateAsync(root, manifest));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Markdown_report_makes_current_evidence_boundary_explicit()
    {
        var snapshot = new GitSnapshot("main", new string('b', 40), [], string.Empty);
        var manifest = RunManifest.Start(
                "TASK-002",
                "Evidence boundary",
                "/repo",
                snapshot,
                ["src/**"],
                DateTimeOffset.Parse("2026-07-05T12:00:00Z"))
            .Complete(
                snapshot,
                [new ChangedFile("src/Feature.cs", "modified")],
                [new ChangedFile("docs/Unexpected.md", "added")],
                ["Repository has uncommitted changes at finish time."],
                DateTimeOffset.Parse("2026-07-05T12:30:00Z"));

        var output = new MarkdownRunEvidenceFormatter().Format(manifest);

        Assert.Contains("`src/Feature.cs` — modified", output);
        Assert.Contains("`docs/Unexpected.md` — added", output);
        Assert.Contains("does not yet prove which build, test, UI, database, or runtime validation commands were executed", output);
    }

    private static string CreateTemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "agentswatch-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}
