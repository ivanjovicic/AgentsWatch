using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RunFinishTests
{
    private static async Task<string> Start(TempGitRepo repo)
    {
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["ac"], ["**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["evidence"], null));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var r = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
        Assert.True(r.Started, r.Error);
        return r.Baseline!.RunId;
    }

    private static RunFinishService Service(string root)
    {
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        return new RunFinishService(new FileActiveRunStore(root), new FileRunRecordStore(root), git, git);
    }

    private static async Task<RunFinishRecord> Finish(TempGitRepo repo, string runId)
    {
        var r = await Service(repo.Root).FinishAsync(repo.Root, runId);
        Assert.True(r.Finished, r.Error);
        return r.Record!;
    }

    private static ClassifiedChange Only(IReadOnlyList<ClassifiedChange> list, string path) => Assert.Single(list, c => c.Path == path);

    [Fact]
    public async Task CleanStart_ModifiedFile_IsRunIntervalChange()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        repo.Write("a.txt", "changed");
        var d = (await Finish(repo, run)).Delta;
        var c = Assert.Single(d.RunIntervalChanges);
        Assert.Equal(("a.txt", "M"), (c.Path, c.Status));
        Assert.Contains("authorship not proven", c.ClassificationReason);
        Assert.Empty(d.AttributionAmbiguities);
    }

    [Fact]
    public async Task CleanStart_AddAndDelete_AreRunIntervalChanges()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        repo.Write("added.txt", "n");
        repo.Git("add added.txt");
        File.Delete(Path.Combine(repo.Root, "a.txt"));
        var d = (await Finish(repo, run)).Delta;
        Assert.Equal("A", Only(d.RunIntervalChanges, "added.txt").Status);
        Assert.Equal("D", Only(d.RunIntervalChanges, "a.txt").Status);
    }

    [Fact]
    public async Task CleanStart_Rename_KeepsOldPath()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        repo.Git("mv a.txt moved.txt");
        var c = Assert.Single((await Finish(repo, run)).Delta.RunIntervalChanges);
        Assert.Equal(("moved.txt", "R", "a.txt"), (c.Path, c.Status, c.OldPath));
    }

    [Fact]
    public async Task CleanStart_UntrackedWithSpaces_IsRunIntervalChange()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        repo.Write("new dir/my file.txt", "x");
        var c = Assert.Single((await Finish(repo, run)).Delta.RunIntervalChanges);
        Assert.Equal(("new dir/my file.txt", "??"), (c.Path, c.Status));
    }

    [Fact]
    public async Task DirtyAtStart_Unchanged_IsNotIntervalChange()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "pre-existing");
        repo.Write("pre untracked.txt", "u");
        var run = await Start(repo);
        var d = (await Finish(repo, run)).Delta;
        Assert.Empty(d.RunIntervalChanges);
        Assert.Empty(d.PreExistingChangedFurther);
        Assert.Equal(["a.txt", "pre untracked.txt"], d.PreExistingUnchangedChanges.Select(c => c.Path));
    }

    [Fact]
    public async Task DirtyAtStart_ChangedFurther_IsDetected_AndNewFileStillInterval()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "pre-existing");
        repo.Write("pre untracked.txt", "u1");
        var run = await Start(repo);
        repo.Write("a.txt", "pre-existing + agent edit");
        repo.Write("pre untracked.txt", "u2");
        repo.Write("b file.txt", "edited");
        var d = (await Finish(repo, run)).Delta;
        Assert.Contains("unstaged changed", Only(d.PreExistingChangedFurther, "a.txt").ClassificationReason);
        Assert.Contains("untracked changed", Only(d.PreExistingChangedFurther, "pre untracked.txt").ClassificationReason);
        Assert.Equal("b file.txt", Assert.Single(d.RunIntervalChanges).Path);
    }

    [Fact]
    public async Task DirtyAtStart_Reverted_IsChangedFurther()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "pre-existing");
        var run = await Start(repo);
        repo.Git("checkout -- a.txt");
        Assert.Contains("reverted", Only((await Finish(repo, run)).Delta.PreExistingChangedFurther, "a.txt").ClassificationReason);
    }

    [Fact]
    public async Task PartialStagedAndUnstaged_SamePath_StagingChangeDetected()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "staged part");
        repo.Git("add a.txt");
        repo.Write("a.txt", "staged part + unstaged part");
        var run = await Start(repo);
        repo.Git("add a.txt");
        var c = Only((await Finish(repo, run)).Delta.PreExistingChangedFurther, "a.txt");
        Assert.Contains("staged", c.ClassificationReason);
        Assert.Contains("unstaged", c.ClassificationReason);
    }

    [Fact]
    public async Task FastForwardCommit_OfCleanFile_IsIntervalChange_DirtyCommitted_IsAmbiguous()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "pre-existing");
        var run = await Start(repo);
        repo.Write("b file.txt", "edited");
        repo.Git("commit -q -am during");
        var rec = await Finish(repo, run);
        Assert.Equal(HeadTransitionKind.FastForward, rec.HeadTransition.Kind);
        Assert.Contains("committed during interval", Only(rec.Delta.RunIntervalChanges, "b file.txt").ClassificationReason);
        Assert.Contains("included in commit", Only(rec.Delta.AttributionAmbiguities, "a.txt").ClassificationReason);
    }

    [Fact]
    public async Task NonFastForwardHead_MarksEverythingAmbiguous()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "v2");
        repo.Git("commit -q -am second");
        var run = await Start(repo);
        repo.Git("reset -q --hard HEAD~1");
        repo.Git("checkout -q -b other");
        repo.Write("x.txt", "x");
        var rec = await Finish(repo, run);
        Assert.Equal(HeadTransitionKind.NonFastForward, rec.HeadTransition.Kind);
        Assert.True(rec.HeadTransition.BranchChanged);
        Assert.Empty(rec.Delta.RunIntervalChanges);
        Assert.Contains(rec.Delta.AttributionAmbiguities, c => c.Path == "x.txt" && c.ClassificationReason.Contains("non-fast-forward"));
    }

    [Fact]
    public async Task RenameOfPreExistingDirtyPath_IsAmbiguous()
    {
        using var repo = new TempGitRepo();
        var body = string.Join("\n", Enumerable.Range(0, 40).Select(i => $"line {i}"));
        repo.Write("a.txt", body);
        repo.Git("commit -q -am long");
        repo.Write("a.txt", body + "\npre-existing edit");
        var run = await Start(repo);
        repo.Git("add a.txt");
        repo.Git("mv a.txt renamed.txt");
        var d = (await Finish(repo, run)).Delta;
        Assert.Contains(d.AttributionAmbiguities, c => c.Path == "renamed.txt" && c.OldPath == "a.txt");
        Assert.Contains(d.AttributionAmbiguities, c => c.Path == "a.txt");
        Assert.Empty(d.RunIntervalChanges);
    }

    [Fact]
    public async Task MissingOrInvalidRun_FailsClearly_AndRecordPersistsBeforeBaselineRemoval()
    {
        using var repo = new TempGitRepo();
        Assert.Contains("no active run", (await Service(repo.Root).FinishAsync(repo.Root, "nope")).Error);
        Assert.Contains("invalid run id", (await Service(repo.Root).FinishAsync(repo.Root, "../x")).Error);

        var run = await Start(repo);
        var rec = await Finish(repo, run);
        Assert.Empty(new FileActiveRunStore(repo.Root).ActiveRunIds());
        var loaded = new FileRunRecordStore(repo.Root).Read(run);
        Assert.Equal(RunFinishJson.Serialize(rec), RunFinishJson.Serialize(loaded));
        Assert.Contains("not proven authored", loaded.AttributionPolicy);
        Assert.Contains("no active run", (await Service(repo.Root).FinishAsync(repo.Root, run)).Error);
    }

    [Fact]
    public async Task ExistingRecord_KeepsActiveBaseline()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        var store = new FileRunRecordStore(repo.Root);
        Directory.CreateDirectory(Path.GetDirectoryName(store.PathFor(run))!);
        File.WriteAllText(store.PathFor(run), "{}");
        var r = await Service(repo.Root).FinishAsync(repo.Root, run);
        Assert.False(r.Finished);
        Assert.Single(new FileActiveRunStore(repo.Root).ActiveRunIds());
    }

    [Fact]
    public async Task Cli_StartMutateFinish()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "pre");
        var run = await Start(repo);
        repo.Write("a.txt", "pre+more");
        repo.Write("c.txt", "c");
        var o = new StringWriter();
        var e = new StringWriter();
        Assert.True(await FinishCommand.RunAsync(repo.Root, [run], o, e) == 0, e.ToString());
        var text = o.ToString();
        Assert.Contains("Run-interval changes: 1", text);
        Assert.Contains("Pre-existing changed further: 1", text);
        Assert.Equal(1, await FinishCommand.RunAsync(repo.Root, [run], new StringWriter(), new StringWriter()));
    }
}