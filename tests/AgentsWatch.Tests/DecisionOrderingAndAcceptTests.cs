using System.Text.Json.Nodes;
using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class DecisionOrderingAndAcceptTests
{
    private static Finding F(string id, string rule, FindingSeverity severity, FindingStatus status, FindingCategory category = FindingCategory.MissingValidation) =>
        new(id, rule, category, severity, status, rule + " message", [], [rule]);

    [Fact]
    public void Classify_IsIndependentOfFindingOrder()
    {
        var findings = new[]
        {
            F("e1", EvidenceRules.AcceptanceUnsupported, FindingSeverity.Warning, FindingStatus.Unknown, FindingCategory.AcceptanceCriterionUnsupported),
            F("e2", ScopeDriftRules.AvoidTouched, FindingSeverity.Error, FindingStatus.Unsupported, FindingCategory.AvoidPathTouched),
            F("e3", ClaimRules.Unsupported, FindingSeverity.Error, FindingStatus.Unsupported, FindingCategory.UnsupportedClaim),
            F("e4", EvidenceRules.ValidationFailed, FindingSeverity.Error, FindingStatus.Unsupported, FindingCategory.ValidationFailed)
        };

        var forward = DecisionOrdering.Classify(findings);
        var reverse = DecisionOrdering.Classify(Enumerable.Reverse(findings).ToArray());
        Assert.Equal(DecisionStatus.Failed, forward);
        Assert.Equal(forward, reverse);
        Assert.Equal(DecisionOrdering.TotalOrder, "Failed > Blocked > NeedsApproval > NeedsReview > NeedsEvidence > Done");
    }

    [Fact]
    public void UnknownAcceptance_DoesNotHideScopeDriftOrUnsupportedClaim()
    {
        var findings = new[]
        {
            F("e1", EvidenceRules.AcceptanceUnsupported, FindingSeverity.Warning, FindingStatus.Unknown, FindingCategory.AcceptanceCriterionUnsupported),
            F("e2", ScopeDriftRules.OutsideOwned, FindingSeverity.Error, FindingStatus.Unsupported, FindingCategory.ScopeOutsideOwnedPaths),
            F("e3", ClaimRules.Unsupported, FindingSeverity.Error, FindingStatus.Unsupported, FindingCategory.UnsupportedClaim)
        };

        Assert.Equal(DecisionStatus.NeedsReview, DecisionOrdering.Classify(findings));
        Assert.Equal(DecisionStatus.NeedsReview, DecisionOrdering.Classify(Enumerable.Reverse(findings).ToArray()));
    }

    [Fact]
    public void SafeNonDoneDecisions_KeepPriority()
    {
        Assert.Equal(DecisionStatus.Failed, DecisionOrdering.Classify([
            F("a", EvidenceRules.ValidationFailed, FindingSeverity.Error, FindingStatus.Unsupported, FindingCategory.ValidationFailed),
            F("b", EvidenceRules.ValidationBlocked, FindingSeverity.Error, FindingStatus.Unknown, FindingCategory.MissingValidation),
            F("c", EvidenceRules.ApprovalRequired, FindingSeverity.Error, FindingStatus.NeedsReview, FindingCategory.RiskApprovalRequired)
        ]));
        Assert.Equal(DecisionStatus.Blocked, DecisionOrdering.Classify([
            F("b", EvidenceRules.ValidationBlocked, FindingSeverity.Error, FindingStatus.Unknown, FindingCategory.MissingValidation),
            F("c", EvidenceRules.ApprovalRequired, FindingSeverity.Error, FindingStatus.NeedsReview, FindingCategory.RiskApprovalRequired)
        ]));
        Assert.Equal(DecisionStatus.NeedsApproval, DecisionOrdering.Classify([
            F("c", EvidenceRules.ApprovalRequired, FindingSeverity.Error, FindingStatus.NeedsReview, FindingCategory.RiskApprovalRequired),
            F("d", EvidenceRules.AcceptanceUnsupported, FindingSeverity.Warning, FindingStatus.Unknown, FindingCategory.AcceptanceCriterionUnsupported)
        ]));
        Assert.Equal(DecisionStatus.NeedsEvidence, DecisionOrdering.Classify([
            F("d", EvidenceRules.AcceptanceUnsupported, FindingSeverity.Warning, FindingStatus.Unknown, FindingCategory.AcceptanceCriterionUnsupported)
        ]));
        Assert.Equal(DecisionStatus.Done, DecisionOrdering.Classify([]));
    }

    private static async Task<(TempGitRepo Repo, string RunId)> StartedAndFinished()
    {
        var repo = new TempGitRepo();
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["feature works"], ["**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null,
            [new CriterionValidationBinding("ac1", ["dotnet test"])]));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
        Assert.True(started.Started, started.Error);
        var git2 = new GitRepositoryStateReader(new GitCommandRunner());
        var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
            receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
            .FinishAsync(repo.Root, started.Baseline!.RunId);
        Assert.True(finished.Finished, finished.Error);
        return (repo, started.Baseline.RunId);
    }

    private static EvidenceService Evidence(string root) =>
        new(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());

    private static AcceptanceService Accept(string root) =>
        new(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());

    private static ValidationEvidence Ev(string runId, ValidationStatus status, string id = "t", int? exit = null) =>
        new(id, runId, "dotnet test", status, ValidationSource.CI, ExitCode: exit ?? (status == ValidationStatus.Pass ? 0 : 1), OutputSummary: "test log");

    [Fact]
    public async Task TimedOut_CannotProduceDone()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var receipt = Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.TimedOut)).Receipt!;
            Assert.Equal(DecisionStatus.Failed, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.ValidationFailed && f.Message.Contains("TimedOut"));
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task HistoricalNeedsEvidence_DoesNotAutoConvertToDone_WhenAcceptanceStaysUnknown()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var recorded = Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.Pass));
            Assert.True(recorded.Ok, recorded.Error);
            var historical = recorded.Receipt!;
            Assert.Equal(DecisionStatus.NeedsEvidence, historical.Decision.Status);
            Assert.Equal(CriterionStatus.Unknown, Assert.Single(historical.AcceptanceCriteria).Status);

            var store = new FileRunReceiptStore(repo.Root);
            var reloaded = store.Read(run);
            Assert.Equal(DecisionStatus.NeedsEvidence, reloaded.Decision.Status);
            var contract = RunContractJson.Deserialize(new FileRunContractStore(repo.Root).ReadRaw("t"));
            var replayed = EvidenceGate.Apply(reloaded, contract);
            Assert.Equal(DecisionStatus.NeedsEvidence, replayed.Decision.Status);
            Assert.NotEqual(DecisionStatus.Done, replayed.Decision.Status);
            Assert.Equal(CriterionStatus.Unknown, Assert.Single(replayed.AcceptanceCriteria).Status);
        }
    }

    [Fact]
    public async Task UnknownAcceptance_WithScopeDrift_IsNeedsReview()
    {
        var repo = new TempGitRepo();
        using (repo)
        {
            var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["feature works"], ["src/**"], ["docs/**"],
                PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null));
            Assert.True(created.Created, created.Error);
            var git = new GitRepositoryStateReader(new GitCommandRunner());
            var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
            Assert.True(started.Started, started.Error);
            repo.Write("extra.txt", "outside owned");
            repo.Write("docs/note.md", "avoided");
            var git2 = new GitRepositoryStateReader(new GitCommandRunner());
            var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
                receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
                .FinishAsync(repo.Root, started.Baseline!.RunId);
            Assert.True(finished.Finished, finished.Error);
            var run = started.Baseline.RunId;
            var receipt = Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.Pass)).Receipt!;
            Assert.Equal(DecisionStatus.NeedsReview, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned);
            Assert.Contains(receipt.Findings, f => f.RuleId == ScopeDriftRules.AvoidTouched);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnsupported && f.Status == FindingStatus.Unknown);
            Assert.NotEqual(DecisionStatus.NeedsEvidence, receipt.Decision.Status);
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);

            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            Assert.Contains("## Decision: NeedsReview", md);
            Assert.Contains("- Blocking:", md);
            Assert.Contains("- Findings by severity:", md);
            Assert.Contains("error=", md);
        }
    }

    [Fact]
    public async Task AcceptRecord_SupportedRequiresPassingValidation_AndCanReachDone()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var empty = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Supported, [], null);
            Assert.False(empty.Ok);
            Assert.Contains("evidence-free", empty.Error, StringComparison.OrdinalIgnoreCase);

            Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.Pass));
            var o = new StringWriter();
            var err = new StringWriter();
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t"], o, err));
            Assert.Contains("non-empty --reason", err.ToString(), StringComparison.OrdinalIgnoreCase);

            o = new StringWriter();
            err = new StringWriter();
            const string reason = "the recorded test log shows the feature works";
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", reason], o, err));
            Assert.Contains("Acceptance recorded: ac1 [Supported]", o.ToString());
            Assert.Contains("manual assessment", o.ToString(), StringComparison.OrdinalIgnoreCase);
            Assert.Contains("declared", o.ToString(), StringComparison.OrdinalIgnoreCase);
            Assert.Contains(reason, o.ToString());
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            var ac = Assert.Single(receipt.AcceptanceCriteria);
            Assert.Equal(CriterionStatus.Supported, ac.Status);
            Assert.Contains("validations[t]", ac.EvidenceRefs);
            Assert.Contains("manual assessment", ac.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("declared, not attested", ac.Reason, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(reason, ac.Reason);
            Assert.Contains("t", ac.Reason);
            Assert.Equal(DecisionStatus.NeedsReview, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
            Assert.True(RunReceiptJson.VerifyDigest(receipt));
            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            Assert.Contains("manual assessment", md, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(reason, md);
        }
    }

    [Fact]
    public async Task AcceptRecord_RejectsMissingValidation_FailedValidation_AndTamperedReceipt()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var missing = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Supported, ["nope"], "looks good");
            Assert.False(missing.Ok);
            Assert.Contains("not recorded", missing.Error);

            Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.Fail));
            var failed = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Supported, ["t"], "looks good");
            Assert.False(failed.Ok);
            Assert.Contains("cannot support acceptance", failed.Error);
            Assert.Equal(DecisionStatus.Failed, new FileRunReceiptStore(repo.Root).Read(run).Decision.Status);

            var store = new FileRunReceiptStore(repo.Root);
            var path = store.PathFor(run);
            var node = JsonNode.Parse(File.ReadAllText(path))!;
            node["evidenceDigest"] = "sha256:deadbeef";
            var tampered = node.ToJsonString();
            File.WriteAllText(path, tampered);
            var rejected = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Unknown, [], null);
            Assert.False(rejected.Ok);
            Assert.Contains("digest", rejected.Error, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(tampered, File.ReadAllText(path));
        }
    }

    [Fact]
    public async Task AcceptRecord_NotSupportedRequiresEvidence_UnknownCanClear()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.Pass));
            var noLink = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Unsupported, [], null);
            Assert.False(noLink.Ok);

            var marked = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Unsupported, ["t"], "does not meet the criterion");
            Assert.True(marked.Ok, marked.Error);
            Assert.Equal(CriterionStatus.Unsupported, Assert.Single(marked.Receipt!.AcceptanceCriteria).Status);
            Assert.Equal(DecisionStatus.NeedsReview, marked.Receipt.Decision.Status);

            var cleared = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Unknown, [], null, replace: true);
            Assert.True(cleared.Ok, cleared.Error);
            Assert.Equal(CriterionStatus.Unknown, Assert.Single(cleared.Receipt!.AcceptanceCriteria).Status);
            Assert.Equal(DecisionStatus.NeedsEvidence, cleared.Receipt.Decision.Status);
        }
    }
    [Fact]
    public async Task AcceptRecord_SupportedWithoutReason_IsRejected()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            Evidence(repo.Root).Record(run, Ev(run, ValidationStatus.Pass));
            var blank = Accept(repo.Root).Record(run, "ac1", CriterionStatus.Supported, ["t"], "   ");
            Assert.False(blank.Ok);
            Assert.Contains("reason", blank.Error, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(CriterionStatus.Unknown, Assert.Single(new FileRunReceiptStore(repo.Root).Read(run).AcceptanceCriteria).Status);
            Assert.NotEqual(DecisionStatus.Done, new FileRunReceiptStore(repo.Root).Read(run).Decision.Status);
        }
    }

    [Fact]
    public async Task ClaimsBeforeEvidence_ThenAcceptAndEvidenceCheck_DoesNotKeepStaleUnsupportedClaim()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var o = new StringWriter();
            var err = new StringWriter();
            Assert.Equal(1, ClaimsCommand.Run(repo.Root, ["record", run, "--type", "ValidationPassed", "--value", "true", "--source", "agent-prose"], o, err));
            var afterClaim = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Contains(afterClaim.Findings, f => f.RuleId == ClaimRules.Unsupported);
            Assert.Equal(CriterionStatus.Unsupported, Assert.Single(afterClaim.Claims).Status);

            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["record", run, "--id", "t", "--command", "dotnet test", "--status", "pass", "--source", "user-declared", "--exit-code", "0", "--summary", "test log"], o, err));

            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "recorded test log backs the criterion"], o, err));

            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["check", run], o, err));
            var afterEvidenceCheck = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.NeedsReview, afterEvidenceCheck.Decision.Status);
            Assert.Contains(afterEvidenceCheck.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
            Assert.DoesNotContain(afterEvidenceCheck.Findings, f => f.RuleId == ClaimRules.Unsupported);
            Assert.Equal(CriterionStatus.Supported, Assert.Single(afterEvidenceCheck.Claims).Status);
            Assert.Contains("manual assessment", Assert.Single(afterEvidenceCheck.AcceptanceCriteria).Reason, StringComparison.OrdinalIgnoreCase);

            var decision = afterEvidenceCheck.Decision.Status;
            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, ClaimsCommand.Run(repo.Root, ["check", run], o, err));
            var afterClaimsCheck = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(decision, afterClaimsCheck.Decision.Status);
            Assert.Equal(DecisionStatus.NeedsReview, afterClaimsCheck.Decision.Status);
            Assert.Contains(afterClaimsCheck.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
            Assert.DoesNotContain(afterClaimsCheck.Findings, f => f.RuleId == ClaimRules.Unsupported);
            Assert.Equal(CriterionStatus.Supported, Assert.Single(afterClaimsCheck.Claims).Status);
        }
    }

    [Fact]
    public async Task EvidenceBeforeClaims_ThenAccept_YieldsSameDecisionAsOppositeOrder()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var o = new StringWriter();
            var err = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["record", run, "--id", "t", "--command", "dotnet test", "--status", "pass", "--source", "user-declared", "--exit-code", "0", "--summary", "test log"], o, err));
            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, ClaimsCommand.Run(repo.Root, ["record", run, "--type", "ValidationPassed", "--value", "true", "--source", "agent-prose"], o, err));
            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "recorded test log backs the criterion"], o, err));
            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["check", run], o, err));
            var afterEvidenceCheck = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.NeedsReview, afterEvidenceCheck.Decision.Status);
            Assert.Contains(afterEvidenceCheck.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
            Assert.DoesNotContain(afterEvidenceCheck.Findings, f => f.RuleId == ClaimRules.Unsupported);

            o = new StringWriter();
            err = new StringWriter();
            Assert.Equal(1, ClaimsCommand.Run(repo.Root, ["check", run], o, err));
            Assert.Equal(afterEvidenceCheck.Decision.Status, new FileRunReceiptStore(repo.Root).Read(run).Decision.Status);
        }
    }
}