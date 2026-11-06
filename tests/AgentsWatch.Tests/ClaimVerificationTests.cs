using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Paths;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class PathClassifierTests
{
    [Theory]
    [InlineData("tests/FooTests.cs", PathKind.Test)]
    [InlineData("src/Foo.spec.ts", PathKind.Test)]
    [InlineData("lib/foo_test.py", PathKind.Test)]
    [InlineData("docs/guide.md", PathKind.Docs)]
    [InlineData("README.md", PathKind.Docs)]
    [InlineData("src/Data/Migrations/20260101_Init.cs", PathKind.Migration)]
    [InlineData("prisma/migrations/001_init/migration.sql", PathKind.Migration)]
    [InlineData("src/ClientApp/App.tsx", PathKind.Frontend)]
    [InlineData("src/Services/Billing.cs", PathKind.Backend)]
    [InlineData("src/owned.cs", PathKind.Backend)]
    [InlineData("appsettings.json", PathKind.Config)]
    [InlineData("mystery.dat", PathKind.Other)]
    [InlineData("contest.cs", PathKind.Backend)]
    [InlineData("tests\\FooTests.cs", PathKind.Test)]
    [InlineData("src\\Services\\Billing.cs", PathKind.Backend)]
    [InlineData("docs\\guide.md", PathKind.Docs)]
    public void Classify_UsesExplicitHeuristics(string path, PathKind kind) =>
        Assert.Equal(kind, PathClassifier.Classify(path));
}

public sealed class ClaimVerificationTests
{
    private static async Task<(TempGitRepo Repo, string RunId)> RunAsync(Action<TempGitRepo>? beforeStart = null, Action<TempGitRepo>? during = null, string[]? owned = null)
    {
        var repo = new TempGitRepo();
        Directory.CreateDirectory(Path.Combine(repo.Root, "src"));
        repo.Write("src/owned.cs", "owned");
        repo.Git("add src/owned.cs");
        repo.Git("commit -q -m seed");
        beforeStart?.Invoke(repo);
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["ac"], owned ?? ["src/**"], [],
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

    private static ClaimsService Claims(string root) =>
        new(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());

    private static AgentClaim C(string run, string type, string source = "user-declared", string value = "true") =>
        new("", run, type, source, value);

    private static AgentClaim Eval(TempGitRepo repo, string run, string type, string source = "user-declared", string value = "true") =>
        Assert.Single(Claims(repo.Root).Record(run, C(run, type, source, value)).Receipt!.Claims, c => c.ClaimType == type);

    [Fact]
    public async Task TestsAdded_SupportedUnsupportedUnknown()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("tests/FooTests.cs", "class FooTests {}"));
        using (repo)
        {
            var c = Eval(repo, run, ClaimTypes.TestsAdded);
            Assert.Equal(CriterionStatus.Supported, c.Status);
            Assert.Contains("tests/FooTests.cs", c.Paths!);
        }

        (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "changed"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unsupported, Eval(repo, run, ClaimTypes.TestsAdded).Status);
        }

        (repo, run) = await RunAsync();
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unknown, Eval(repo, run, ClaimTypes.TestsAdded).Status);
        }
    }

    [Fact]
    public async Task DocsOnly_SupportedUnsupportedUnknown()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("docs/guide.md", "hi"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Supported, Eval(repo, run, ClaimTypes.DocsOnly).Status);
        }

        (repo, run) = await RunAsync(during: r => { r.Write("docs/guide.md", "hi"); r.Write("src/owned.cs", "x"); });
        using (repo)
        {
            var c = Eval(repo, run, ClaimTypes.DocsOnly);
            Assert.Equal(CriterionStatus.Unsupported, c.Status);
            Assert.Contains("src/owned.cs", c.Paths!);
        }

        (repo, run) = await RunAsync();
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unknown, Eval(repo, run, ClaimTypes.DocsOnly).Status);
        }
    }

    [Fact]
    public async Task BackendUnchanged_SupportedUnsupportedUnknown()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("docs/guide.md", "n"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Supported, Eval(repo, run, ClaimTypes.BackendUnchanged).Status);
        }

        (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "backend"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unsupported, Eval(repo, run, ClaimTypes.BackendUnchanged).Status);
        }

        (repo, run) = await RunAsync(during: r => r.Write("mystery.dat", "x"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unknown, Eval(repo, run, ClaimTypes.BackendUnchanged).Status);
        }
    }

    [Fact]
    public async Task MigrationAdded_SupportedUnsupportedUnknown()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("src/Data/Migrations/20260101_Init.cs", "class Init {}"));
        using (repo)
        {
            var c = Eval(repo, run, ClaimTypes.MigrationAdded);
            Assert.Equal(CriterionStatus.Supported, c.Status);
            Assert.Contains("Migrations", string.Join(" ", c.Paths!));
        }

        (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "x"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unsupported, Eval(repo, run, ClaimTypes.MigrationAdded).Status);
        }

        (repo, run) = await RunAsync();
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unknown, Eval(repo, run, ClaimTypes.MigrationAdded).Status);
        }
    }

    [Fact]
    public async Task ValidationPassed_StructuredPass_ProseRejected_UnknownSource()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "x"));
        using (repo)
        {
            var prose = Eval(repo, run, ClaimTypes.ValidationPassed, source: "agent-prose");
            Assert.Equal(CriterionStatus.Unsupported, prose.Status);
            Assert.Contains("prose", prose.Reason, StringComparison.OrdinalIgnoreCase);

            new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root))
                .Record(run, new ValidationEvidence("t", run, "dotnet test", ValidationStatus.Pass, ValidationSource.CI, OutputSummary: "test log"));
            var ok = Claims(repo.Root).Check(run).Receipt!.Claims.Single(c => c.ClaimType == ClaimTypes.ValidationPassed);
            Assert.Equal(CriterionStatus.Supported, ok.Status);
        }

        (repo, run) = await RunAsync();
        using (repo)
        {
            new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root))
                .Record(run, new ValidationEvidence("t", run, "dotnet test", ValidationStatus.Unknown, ValidationSource.CI));
            Assert.Equal(CriterionStatus.Unknown, Eval(repo, run, ClaimTypes.ValidationPassed).Status);
        }

        (repo, run) = await RunAsync();
        using (repo)
        {
            new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root))
                .Record(run, new ValidationEvidence("t", run, "dotnet test", ValidationStatus.Fail, ValidationSource.CI, OutputSummary: "Failed"));
            Assert.Equal(CriterionStatus.Unsupported, Eval(repo, run, ClaimTypes.ValidationPassed).Status);
        }
    }

    [Fact]
    public async Task NoUnrelatedChanges_UsesScopeAndIgnoresPreExistingUnchanged()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "ok"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Supported, Eval(repo, run, ClaimTypes.NoUnrelatedChanges).Status);
        }

        (repo, run) = await RunAsync(during: r => r.Write("a.txt", "unrelated"));
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unsupported, Eval(repo, run, ClaimTypes.NoUnrelatedChanges).Status);
        }

        (repo, run) = await RunAsync(beforeStart: r => r.Write("a.txt", "already dirty"), during: r => r.Write("src/owned.cs", "ok"));
        using (repo)
        {
            var raw = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Contains(raw.RunDelta.PreExistingUnchangedChanges, c => c.Path == "a.txt");
            Assert.Equal(CriterionStatus.Supported, Eval(repo, run, ClaimTypes.NoUnrelatedChanges).Status);
        }

        (repo, run) = await RunAsync(beforeStart: r => r.Write("a.txt", "pre"), during: r => { r.Git("add a.txt"); r.Git("commit -q -m during"); });
        using (repo)
        {
            var c = Eval(repo, run, ClaimTypes.NoUnrelatedChanges);
            Assert.Equal(CriterionStatus.Unknown, c.Status);
            Assert.Contains("ambiguous", c.Reason, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task Results_PersistAndProject_AndUnknownTypeIsUnknown()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("docs/guide.md", "d"));
        using (repo)
        {
            var result = Claims(repo.Root).Record(run, C(run, ClaimTypes.DocsOnly));
            Assert.True(result.Ok, result.Error);
            var loaded = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(CriterionStatus.Supported, Assert.Single(loaded.Claims).Status);
            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            Assert.Contains("DocsOnly", md);
            Assert.Contains("Supported", md);

            var unknown = Claims(repo.Root).Record(run, C(run, "BugFixed")).Receipt!.Claims.Single(c => c.ClaimType == "BugFixed");
            Assert.Equal(CriterionStatus.Unknown, unknown.Status);
            Assert.Contains(result.Receipt!.Decision.Reasons.Concat(unknown.Reason is null ? [] : [unknown.Reason]), _ => true);
            Assert.Contains(new FileRunReceiptStore(repo.Root).Read(run).Findings, f => f.RuleId == ClaimRules.Unknown);
        }
    }

    [Fact]
    public async Task FalseValue_IsEvaluatedAsNegatedClaim()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "no tests"));
        using (repo)
        {
            var denied = Eval(repo, run, ClaimTypes.TestsAdded, value: "false");
            Assert.Equal(CriterionStatus.Supported, denied.Status);
            Assert.Contains("negated claim", denied.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("=false", denied.Reason);
        }

        (repo, run) = await RunAsync(during: r => r.Write("tests/FooTests.cs", "class FooTests {}"));
        using (repo)
        {
            var contradicted = Claims(repo.Root).Record(run, C(run, ClaimTypes.TestsAdded, value: "false")).Receipt!.Claims.Single(c => c.ClaimType == ClaimTypes.TestsAdded);
            Assert.Equal(CriterionStatus.Unsupported, contradicted.Status);
            Assert.Contains("does not hold", contradicted.Reason);
        }

        (repo, run) = await RunAsync();
        using (repo)
        {
            Assert.Equal(CriterionStatus.Unknown, Eval(repo, run, ClaimTypes.TestsAdded, value: "NO").Status);
        }
    }

    [Fact]
    public async Task NonBooleanValue_IsRejectedAtRecordAndUnknownAtEvaluate()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "x"));
        using (repo)
        {
            var recorded = Claims(repo.Root).Record(run, C(run, ClaimTypes.TestsAdded, value: "maybe"));
            Assert.False(recorded.Ok);
            Assert.Contains("not a boolean", recorded.Error);

            var evaluated = ClaimVerifier.EvaluateOne(new FileRunReceiptStore(repo.Root).Read(run), null, C(run, ClaimTypes.TestsAdded, value: "maybe"));
            Assert.Equal(CriterionStatus.Unknown, evaluated.Status);
            Assert.Contains("not a boolean", evaluated.Reason);
        }
    }

    [Fact]
    public async Task EvidenceCheck_AfterClaimsCheck_PreservesAwkFindings()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("src/owned.cs", "x"));
        using (repo)
        {
            var afterClaims = Claims(repo.Root).Record(run, C(run, ClaimTypes.DocsOnly));
            Assert.Contains(afterClaims.Receipt!.Findings, f => f.RuleId == ClaimRules.Unsupported);
            var afterEvidence = new EvidenceService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root), new FileReceiptProjectionWriter()).Check(run);
            Assert.True(afterEvidence.Ok, afterEvidence.Error);
            Assert.Contains(afterEvidence.Receipt!.Findings, f => f.RuleId == ClaimRules.Unsupported && f.Message.Contains("DocsOnly"));
            Assert.Contains(afterEvidence.Receipt.Decision.Reasons, r => r.StartsWith(ClaimRules.Unsupported));
        }
    }

    [Fact]
    public async Task DriftCheck_AfterClaimsCheck_PreservesAwkAndClaimsCheckPreservesAwd()
    {
        var (repo, run) = await RunAsync(during: r => r.Write("a.txt", "outside"));
        using (repo)
        {
            var afterClaims = Claims(repo.Root).Record(run, C(run, ClaimTypes.DocsOnly));
            Assert.Contains(afterClaims.Receipt!.Findings, f => f.RuleId == ClaimRules.Unsupported);
            var afterDrift = new DriftService(repo.Root, new FileRunReceiptStore(repo.Root), new FileRunContractStore(repo.Root), new FileReceiptProjectionWriter()).Check(run);
            Assert.True(afterDrift.Ok, afterDrift.Error);
            Assert.Contains(afterDrift.Receipt!.Findings, f => f.RuleId == ClaimRules.Unsupported && f.Message.Contains("DocsOnly"));
            Assert.Contains(afterDrift.Receipt.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned && f.Paths.Contains("a.txt"));

            var afterClaimsAgain = Claims(repo.Root).Check(run);
            Assert.True(afterClaimsAgain.Ok, afterClaimsAgain.Error);
            Assert.Contains(afterClaimsAgain.Receipt!.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned && f.Paths.Contains("a.txt"));
            Assert.Contains(afterClaimsAgain.Receipt.Findings, f => f.RuleId == ClaimRules.Unsupported);
        }
    }

    [Fact]
    public async Task Record_AllocatesIds_RefusesOverwrite_AndCliChecksFalseClaims()
    {
        var (repo, run) = await RunAsync(during: r => { r.Write("docs/guide.md", "d"); r.Write("src/owned.cs", "x"); });
        using (repo)
        {
            var svc = Claims(repo.Root);
            var first = svc.Record(run, C(run, ClaimTypes.DocsOnly));
            Assert.Equal("c1", first.Receipt!.Claims[0].ClaimId);
            var second = svc.Record(run, C(run, ClaimTypes.TestsAdded));
            Assert.Equal(["c1", "c2"], second.Receipt!.Claims.Select(c => c.ClaimId).ToArray());
            Assert.Contains("already exists", svc.Record(run, first.Receipt.Claims[0] with { ClaimId = "c1", Value = "false" }).Error);

            var replaced = svc.Record(run, first.Receipt.Claims[0] with { ClaimId = "c1", ClaimType = ClaimTypes.TestsAdded, Value = "false" }, replace: true);
            Assert.True(replaced.Ok, replaced.Error);
            Assert.Equal("c1", replaced.RecordedClaim!.ClaimId);
            Assert.Equal(ClaimTypes.TestsAdded, replaced.RecordedClaim.ClaimType);
            Assert.Equal("false", replaced.RecordedClaim.Value);
            Assert.Equal(CriterionStatus.Supported, replaced.RecordedClaim.Status);

            var oReplace = new StringWriter();
            var eReplace = new StringWriter();
            Assert.Equal(1, ClaimsCommand.Run(repo.Root, ["record", run, "--type", ClaimTypes.DocsOnly, "--value", "true", "--source", "user-declared", "--id", "c1", "--replace"], oReplace, eReplace));
            Assert.Contains("Claim recorded: c1 DocsOnly", oReplace.ToString());

            var o = new StringWriter();
            var e = new StringWriter();
            Assert.Equal(1, ClaimsCommand.Run(repo.Root, ["check", run], o, e));
            Assert.Contains("DocsOnly", o.ToString());
            Assert.Contains("Unsupported", o.ToString());
            Assert.Equal(2, ClaimsCommand.Run(repo.Root, ["nope"], new StringWriter(), new StringWriter()));
            Assert.Contains("no receipt", Claims(repo.Root).Check("nope").Error);
        }
    }
}
