using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class ScopeDriftTests
{
    private static async Task<(TempGitRepo Repo, string RunId)> RunAsync(
        string[] owned, string[] avoid, Action<TempGitRepo>? beforeStart = null, Action<TempGitRepo>? during = null)
    {
        var repo = new TempGitRepo();
        Directory.CreateDirectory(Path.Combine(repo.Root, "src"));
        repo.Write("src/owned.cs", "owned");
        repo.Git("add src/owned.cs");
        repo.Git("commit -q -m seed");
        beforeStart?.Invoke(repo);
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["ac"], owned, avoid,
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
        Assert.True(started.Started, started.Error);
        during?.Invoke(repo);
        var git2 = new GitRepositoryStateReader(new GitCommandRunner());
        var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
            receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
            .FinishAsync(repo.Root, started.Baseline!.RunId);
        Assert.True(finished.Finished, finished.Error);
        return (repo, started.Baseline.RunId);
    }

    private static DriftService Service(string root) =>
        new(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());

    [Fact]
    public async Task InsideOwned_NoDrift()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Write("src/owned.cs", "changed"));
        using (repo)
        {
            var result = Service(repo.Root).Check(run);
            Assert.True(result.Ok, result.Error);
            Assert.DoesNotContain(result.Receipt!.Findings, f => f.RuleId.StartsWith("AWD"));
        }
    }

    [Fact]
    public async Task OutsideOwned_IsDrift()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Write("a.txt", "unrelated"));
        using (repo)
        {
            var receipt = Service(repo.Root).Check(run).Receipt!;
            var f = Assert.Single(receipt.Findings, x => x.RuleId == ScopeDriftRules.OutsideOwned);
            Assert.Equal(FindingCategory.ScopeOutsideOwnedPaths, f.Category);
            Assert.Contains("a.txt", f.Paths);
            Assert.Contains("outside ownedPaths", f.Message);
            Assert.Contains("RunIntervalChange", f.Message);
            Assert.Contains(receipt.Decision.Reasons, r => r.StartsWith(ScopeDriftRules.OutsideOwned));
        }
    }

    [Fact]
    public async Task AvoidPathTouched_IsFinding_EvenWhenOwned()
    {
        var (repo, run) = await RunAsync(["src/**"], ["src/secret/**"], during: r => r.Write("src/secret/key.txt", "k"));
        using (repo)
        {
            var receipt = Service(repo.Root).Check(run).Receipt!;
            var f = Assert.Single(receipt.Findings, x => x.RuleId.StartsWith("AWD"));
            Assert.Equal(ScopeDriftRules.AvoidTouched, f.RuleId);
            Assert.Equal(FindingCategory.AvoidPathTouched, f.Category);
            Assert.Contains("src/secret/key.txt", f.Paths);
            Assert.Contains("src/secret/**", f.Message);
            Assert.DoesNotContain(receipt.Findings, x => x.RuleId == ScopeDriftRules.OutsideOwned);
        }
    }

    [Fact]
    public async Task PreExistingUnchangedOutsideOwned_IsNotDrift()
    {
        var (repo, run) = await RunAsync(["src/**"], [],
            beforeStart: r => r.Write("a.txt", "already dirty"),
            during: r => r.Write("src/owned.cs", "changed"));
        using (repo)
        {
            var raw = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Contains(raw.RunDelta.PreExistingUnchangedChanges, c => c.Path == "a.txt");
            var receipt = Service(repo.Root).Check(run).Receipt!;
            Assert.DoesNotContain(receipt.Findings, f => f.Paths.Contains("a.txt") && f.RuleId.StartsWith("AWD"));
        }
    }

    [Fact]
    public async Task PreExistingChangedFurtherOutsideOwned_IsDriftWithContext()
    {
        var (repo, run) = await RunAsync(["src/**"], [],
            beforeStart: r => r.Write("a.txt", "dirty v1"),
            during: r => r.Write("a.txt", "dirty v2"));
        using (repo)
        {
            var raw = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Contains(raw.RunDelta.PreExistingChangedFurther, c => c.Path == "a.txt");
            var f = Assert.Single(Service(repo.Root).Check(run).Receipt!.Findings, x => x.RuleId == ScopeDriftRules.OutsideOwned);
            Assert.Contains("PreExistingChangedFurther", f.Message);
            Assert.Contains("a.txt", f.Paths);
        }
    }

    [Fact]
    public async Task AmbiguousOutsideOwned_IsReviewNotGuessedDrift()
    {
        var (repo, run) = await RunAsync(["src/**"], [],
            beforeStart: r => r.Write("a.txt", "pre"),
            during: r => { r.Git("add a.txt"); r.Git("commit -q -m during"); });
        using (repo)
        {
            var raw = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Contains(raw.RunDelta.AttributionAmbiguities, c => c.Path == "a.txt");
            var receipt = Service(repo.Root).Check(run).Receipt!;
            Assert.Contains(receipt.Findings, f => f.RuleId == ScopeDriftRules.AmbiguousScope && f.Paths.Contains("a.txt") && f.Status == FindingStatus.NeedsReview);
            Assert.DoesNotContain(receipt.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned);
            Assert.Contains(receipt.Decision.Reasons, r => r.StartsWith(ScopeDriftRules.AmbiguousScope));
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task AmbiguousInsideOwned_DoesNotInventDrift()
    {
        var (repo, run) = await RunAsync(["src/**"], [],
            beforeStart: r => r.Write("src/owned.cs", "pre"),
            during: r => { r.Git("add src/owned.cs"); r.Git("commit -q -m during"); });
        using (repo)
        {
            var raw = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Contains(raw.RunDelta.AttributionAmbiguities, c => c.Path == "src/owned.cs");
            var receipt = Service(repo.Root).Check(run).Receipt!;
            Assert.DoesNotContain(receipt.Findings, f => f.RuleId.StartsWith("AWD"));
        }
    }

    [Fact]
    public async Task RenameInsideToOutside_IsBoundaryFinding()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Git("mv src/owned.cs docs-owned.cs"));
        using (repo)
        {
            var f = Assert.Single(Service(repo.Root).Check(run).Receipt!.Findings, x => x.RuleId == ScopeDriftRules.RenameCrosses);
            Assert.Contains("src/owned.cs", f.Paths);
            Assert.Contains("docs-owned.cs", f.Paths);
            Assert.Contains("crosses ownedPaths", f.Message);
        }
    }

    [Fact]
    public async Task RenameOutsideToInside_IsBoundaryFinding()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Git("mv a.txt src/moved.txt"));
        using (repo)
        {
            var f = Assert.Single(Service(repo.Root).Check(run).Receipt!.Findings, x => x.RuleId == ScopeDriftRules.RenameCrosses);
            Assert.Equal(["src/moved.txt", "a.txt"], f.Paths);
        }
    }

    [Fact]
    public async Task SlashNormalization_DoesNotCreateFalseDrift()
    {
        var (repo, run) = await RunAsync([@"src\**"], [], during: r => r.Write("src/nested/file.cs", "n"));
        using (repo)
        {
            var result = Service(repo.Root).Check(run);
            Assert.DoesNotContain(result.Receipt!.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned);
        }
    }

    [Fact]
    public async Task EmptyOwnedPaths_AreUnbounded_ButAvoidStillApplies()
    {
        var (repo, run) = await RunAsync([], ["src/secret/**"], during: r =>
        {
            r.Write("a.txt", "outside anything");
            r.Write("src/secret/key.txt", "k");
        });
        using (repo)
        {
            var receipt = Service(repo.Root).Check(run).Receipt!;
            Assert.DoesNotContain(receipt.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned);
            Assert.Contains(receipt.Findings, f => f.RuleId == ScopeDriftRules.AvoidTouched && f.Paths.Contains("src/secret/key.txt"));
        }
    }

    [Fact]
    public async Task OverlappingOwnedPatterns_FirstMatchIsRecordedAndNoDrift()
    {
        var (repo, run) = await RunAsync(["src/**", "src/owned.cs"], [], during: r => r.Write("src/owned.cs", "x"));
        using (repo)
        {
            Assert.DoesNotContain(Service(repo.Root).Check(run).Receipt!.Findings, f => f.RuleId.StartsWith("AWD"));
        }
    }

    [Fact]
    public async Task OutsideOwnedAndAvoided_EmitsBothFindings()
    {
        var (repo, run) = await RunAsync(["src/**"], ["docs/**"], during: r => r.Write("docs/note.md", "n"));
        using (repo)
        {
            var receipt = Service(repo.Root).Check(run).Receipt!;
            Assert.Contains(receipt.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned && f.Paths.Contains("docs/note.md"));
            Assert.Contains(receipt.Findings, f => f.RuleId == ScopeDriftRules.AvoidTouched && f.Paths.Contains("docs/note.md") && f.Message.Contains("docs/**"));
        }
    }

    [Fact]
    public async Task EvidenceCheck_AfterDriftCheck_PreservesAwdFindings()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Write("a.txt", "unrelated"));
        using (repo)
        {
            var afterDrift = Service(repo.Root).Check(run).Receipt!;
            Assert.Contains(afterDrift.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned);
            var afterEvidence = new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root), new FileReceiptProjectionWriter()).Check(run);
            Assert.True(afterEvidence.Ok, afterEvidence.Error);
            Assert.Contains(afterEvidence.Receipt!.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned && f.Paths.Contains("a.txt"));
            Assert.Contains(afterEvidence.Receipt.Decision.Reasons, r => r.StartsWith(ScopeDriftRules.OutsideOwned));
        }
    }

    [Fact]
    public async Task Check_IsIdempotent_AndMissingIdsFail()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Write("a.txt", "x"));
        using (repo)
        {
            var svc = Service(repo.Root);
            var first = svc.Check(run).Receipt!;
            var second = svc.Check(run).Receipt!;
            Assert.Equal(
                first.Findings.Where(f => f.RuleId.StartsWith("AWD")).Select(f => f.RuleId + f.Message),
                second.Findings.Where(f => f.RuleId.StartsWith("AWD")).Select(f => f.RuleId + f.Message));
            Assert.Equal(1, first.Findings.Count(f => f.RuleId == ScopeDriftRules.OutsideOwned));

            Assert.Contains("invalid run id", svc.Check("../x").Error);
            Assert.Contains("no receipt", svc.Check("nope").Error);
        }
    }

    [Fact]
    public async Task MissingContract_FailsWithoutGuessing()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Write("src/owned.cs", "x"));
        using (repo)
        {
            File.Delete(Path.Combine(repo.Root, ".agentwatch", "contracts", "t.json"));
            var result = Service(repo.Root).Check(run);
            Assert.False(result.Ok);
            Assert.Contains("not found", result.Error);
        }
    }

    [Fact]
    public async Task Cli_DriftCheck_AndMalformedUsage()
    {
        var (repo, run) = await RunAsync(["src/**"], [], during: r => r.Write("readme extra.txt", "u"));
        using (repo)
        {
            var o = new StringWriter();
            var e = new StringWriter();
            Assert.Equal(1, DriftCommand.CheckFromArgs(repo.Root, ["check", run], o, e));
            Assert.Contains("AWD001", o.ToString());
            Assert.Contains("readme extra.txt", o.ToString());
            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            Assert.Contains("AWD001", md);

            Assert.Equal(2, DriftCommand.CheckFromArgs(repo.Root, ["check"], new StringWriter(), new StringWriter()));
            Assert.Equal(2, DriftCommand.CheckFromArgs(repo.Root, ["nope", run], new StringWriter(), new StringWriter()));
        }
    }
}
