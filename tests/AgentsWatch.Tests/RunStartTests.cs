using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RunStartTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 5, 17, 30, 0, TimeSpan.Zero);

    private static void CreateContract(string root, string taskId = "AW-T-1")
    {
        var r = new ContractService(new FileRunContractStore(root)).Create(new ContractDraft(taskId, null, "intent", ["ac"], ["src/**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["evidence"], null));
        Assert.True(r.Created, r.Error);
    }

    private static RunStartService Service(string root) => new(new FileRunContractStore(root), new FileActiveRunStore(root),
        new GitRepositoryStateReader(new GitCommandRunner()), new FixedClock(Now));

    private static async Task<RunBaseline> Start(TempGitRepo repo)
    {
        CreateContract(repo.Root);
        var result = await Service(repo.Root).StartAsync(repo.Root, "aw-t-1", new ExecutorMetadata(Agent: "test-agent"));
        Assert.True(result.Started, result.Error);
        return result.Baseline!;
    }

    [Fact]
    public async Task CleanRepo_CapturesBranchHeadAndCleanState()
    {
        using var repo = new TempGitRepo();
        var b = await Start(repo);

        Assert.Equal("main", b.Branch);
        Assert.Equal(repo.Git("rev-parse HEAD"), b.HeadCommitSha);
        Assert.False(b.WorktreeState.IsDirty);
        Assert.Equal("aw-t-1", b.ContractId);
        Assert.Equal("AW-T-1", b.TaskId);
        Assert.Equal(Now, b.StartedAtUtc);
        Assert.StartsWith("aw-t-1-20261105t173000z-", b.RunId);
        Assert.True(ContractIds.IsValid(b.RunId));
        Assert.Equal("test-agent", b.ExecutorMetadata?.Agent);
    }

    [Fact]
    public async Task DirtyRepo_SeparatesStagedUnstagedUntracked_WithFingerprints()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "unstaged change");
        repo.Write("b file.txt", "staged change");
        repo.Git("add \"b file.txt\"");
        repo.Write("b file.txt", "staged then edited");
        repo.Write("new dir/untracked file.txt", "u");

        var w = (await Start(repo)).WorktreeState;

        var staged = Assert.Single(w.Staged);
        Assert.Equal(("b file.txt", "M"), (staged.Path, staged.Status));
        Assert.StartsWith("blob:", staged.Fingerprint);

        Assert.Equal(["a.txt", "b file.txt"], w.Unstaged.Select(e => e.Path));
        Assert.All(w.Unstaged, e => Assert.StartsWith("sha256:", e.Fingerprint));
        Assert.NotEqual(w.Unstaged[0].Fingerprint, w.Unstaged[1].Fingerprint);

        var untracked = Assert.Single(w.Untracked);
        Assert.Equal("new dir/untracked file.txt", untracked.Path);
        Assert.DoesNotContain(w.Untracked, e => e.Path.StartsWith(".agentwatch/"));
        Assert.StartsWith("sha256:", w.StateFingerprint);
    }

    [Fact]
    public async Task Fingerprint_ChangesWhenPreExistingDirtyFileChangesFurther()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "dirty v1");
        var reader = new GitRepositoryStateReader(new GitCommandRunner());
        var first = await reader.ReadAsync(repo.Root);
        var same = await reader.ReadAsync(repo.Root);
        repo.Write("a.txt", "dirty v2");
        var further = await reader.ReadAsync(repo.Root);

        Assert.Equal(first.Worktree.StateFingerprint, same.Worktree.StateFingerprint);
        Assert.NotEqual(first.Worktree.Unstaged[0].Fingerprint, further.Worktree.Unstaged[0].Fingerprint);
        Assert.NotEqual(first.Worktree.StateFingerprint, further.Worktree.StateFingerprint);
    }

    [Fact]
    public async Task StagedDeleteAndRename_AreCaptured()
    {
        using var repo = new TempGitRepo();
        repo.Git("mv a.txt renamed.txt");
        repo.Git("rm -q \"b file.txt\"");

        var w = (await new GitRepositoryStateReader(new GitCommandRunner()).ReadAsync(repo.Root)).Worktree;

        Assert.Contains(w.Staged, e => e.Path == "b file.txt" && e.Status == "D" && e.Fingerprint == GitRepositoryStateReader.Deleted);
        Assert.Contains(w.Staged, e => e.Path == "renamed.txt" && e.Status == "R" && e.OldPath == "a.txt");
    }

    [Fact]
    public async Task UnbornRepo_HasNullHead()
    {
        using var repo = new TempGitRepo(commit: false);
        var b = await Start(repo);
        Assert.Null(b.HeadCommitSha);
        Assert.Equal("main", b.Branch);
    }

    [Fact]
    public async Task MissingOrInvalidContract_IsRefused()
    {
        using var repo = new TempGitRepo();
        var missing = await Service(repo.Root).StartAsync(repo.Root, "nope");
        Assert.False(missing.Started);

        new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("BAD", null, "", [], [], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [], [], [], null));
        var invalid = await Service(repo.Root).StartAsync(repo.Root, "bad");
        Assert.False(invalid.Started);
        Assert.False(invalid.ContractCheck!.IsValid);
        Assert.Empty(new FileActiveRunStore(repo.Root).ActiveRunIds());
    }

    [Fact]
    public async Task SecondActiveRun_IsRefused()
    {
        using var repo = new TempGitRepo();
        await Start(repo);
        var second = await Service(repo.Root).StartAsync(repo.Root, "aw-t-1");
        Assert.False(second.Started);
        Assert.Contains("active run already exists", second.Error);
        Assert.Single(new FileActiveRunStore(repo.Root).ActiveRunIds());
    }

    [Fact]
    public async Task Baseline_RoundTripsFromDisk_AndLeavesNoTempFiles()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "dirty");
        var b = await Start(repo);
        var store = new FileActiveRunStore(repo.Root);

        var loaded = store.Read(b.RunId);

        Assert.Equal(RunBaselineJson.Serialize(b), RunBaselineJson.Serialize(loaded));
        Assert.Empty(Directory.GetFiles(Path.Combine(repo.Root, ".agentwatch", "active-runs"), "*.tmp"));
        var json = File.ReadAllText(store.PathFor(b.RunId));
        Assert.Contains("\"worktreeState\"", json);
        Assert.Contains("\"porcelainVersion\": \"v1-z\"", json);
        Assert.DoesNotContain("dirty\"", json);
        Assert.DoesNotContain("isDirty", json);
    }

    [Fact]
    public async Task Cli_Start_ReportsDirtyBaseline()
    {
        using var repo = new TempGitRepo();
        CreateContract(repo.Root);
        repo.Write("x y.txt", "u");
        var o = new StringWriter();
        var e = new StringWriter();

        Assert.True(await StartCommand.RunAsync(repo.Root, ["aw-t-1", "--agent", "cli"], o, e) == 0, e.ToString());
        Assert.Contains("Dirty at start: staged 0, unstaged 0, untracked 1", o.ToString());
        Assert.Equal(1, await StartCommand.RunAsync(repo.Root, ["aw-t-1"], new StringWriter(), new StringWriter()));
    }

    private sealed class FixedClock(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }
}