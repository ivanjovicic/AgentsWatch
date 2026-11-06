using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class EvidenceGateTests
{
    private static async Task<(TempGitRepo Repo, string RunId)> StartedAndFinished(PermissionMode permission = PermissionMode.WorkspaceWrite)
    {
        var repo = new TempGitRepo();
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["feature works"], ["**"], [],
            permission, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
        Assert.True(started.Started, started.Error);
        repo.Write("a.txt", "changed");
        var git2 = new GitRepositoryStateReader(new GitCommandRunner());
        var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
            receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
            .FinishAsync(repo.Root, started.Baseline!.RunId);
        Assert.True(finished.Finished, finished.Error);
        return (repo, started.Baseline.RunId);
    }

    private static EvidenceService Service(string root) =>
        new(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());

    private static ValidationEvidence Ev(string runId, ValidationStatus status, ValidationSource source, string id = "t", string command = "dotnet test", string? summary = "test log") =>
        new(id, runId, command, status, source, ExitCode: status == ValidationStatus.Pass ? 0 : 1, OutputSummary: summary);

    private static void SupportAcceptance(string root, string runId)
    {
        var store = new FileRunReceiptStore(root);
        var receipt = store.Read(runId);
        store.Replace(receipt with
        {
            AcceptanceCriteria = receipt.AcceptanceCriteria.Select(a => a with { Status = CriterionStatus.Supported, Reason = "marked supported in test" }).ToArray(),
            EvidenceDigest = null
        });
    }

    [Fact]
    public async Task MissingValidation_CannotBeDone()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.NeedsEvidence, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.MandatoryMissing);
            Assert.Contains(receipt.Decision.Reasons, r => r.StartsWith(EvidenceRules.MandatoryMissing));
            Assert.Empty(receipt.Validations);
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);

            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            Assert.Contains("## Decision: NeedsEvidence", md);
            Assert.Contains(EvidenceRules.MandatoryMissing, md);
        }
    }

    [Fact]
    public async Task MandatoryPass_SatisfiesValidationRequirement_UserDeclaredStaysLabeled()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.UserDeclared));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(ValidationSource.UserDeclared, Assert.Single(result.Receipt!.Validations).Source);
            Assert.True(EvidenceGate.SatisfiesMandatory(result.Receipt, RunContractJson.Deserialize(new FileRunContractStore(repo.Root).ReadRaw("t"))));
            Assert.DoesNotContain(result.Receipt.Findings, f => f.RuleId == EvidenceRules.MandatoryMissing);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnsupported && f.Status == FindingStatus.Unknown);
        }
    }

    [Fact]
    public async Task UnknownAcceptance_BlocksDone_EvenWhenValidationPasses()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var receipt = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, summary: "ok")).Receipt!;
            Assert.Equal(CriterionStatus.Unknown, Assert.Single(receipt.AcceptanceCriteria).Status);
            Assert.Equal(DecisionStatus.NeedsEvidence, receipt.Decision.Status);
            Assert.Contains(receipt.Decision.Reasons, r => r.Contains(EvidenceRules.AcceptanceUnsupported));
        }
    }

    [Fact]
    public async Task PassingEvidence_WithSupportedAcceptance_IsDone_AndProjectsReasons()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.AgentsWatchCommand));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.Done, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Decision.Reasons, r => r.StartsWith(EvidenceRules.Satisfied));
            Assert.True(RunReceiptJson.VerifyDigest(result.Receipt));

            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            Assert.Contains("## Decision: Done", md);
            Assert.Contains(EvidenceRules.Satisfied, md);
            Assert.Contains("AgentsWatchCommand", md);
        }
    }

    [Fact]
    public async Task ValidationFail_IsFailed()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var receipt = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Fail, ValidationSource.CI, summary: "Failed 1")).Receipt!;
            Assert.Equal(DecisionStatus.Failed, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.ValidationFailed);
            Assert.Contains(receipt.Decision.Reasons, r => r.StartsWith(EvidenceRules.ValidationFailed));
        }
    }

    [Fact]
    public async Task BlockedByEnvironment_IsBlocked_NeverPass()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var receipt = Service(repo.Root).Record(run, Ev(run, ValidationStatus.BlockedByEnvironment, ValidationSource.AgentsWatchCommand, summary: "sdk missing")).Receipt!;
            Assert.Equal(DecisionStatus.Blocked, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.ValidationBlocked);
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task ExpectedEvidenceMissing_WhenPassHasNoBacking()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var receipt = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, summary: null)).Receipt!;
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.ExpectedEvidenceMissing);
            Assert.Equal(DecisionStatus.NeedsEvidence, receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task ProseClaim_IsNotValidationEvidence()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var store = new FileRunReceiptStore(repo.Root);
            var receipt = store.Read(run);
            store.Replace(receipt with { Claims = [new AgentClaim("c1", run, "ValidationPassed", "agent-prose", "all tests pass")], EvidenceDigest = null });
            var checkedReceipt = Service(repo.Root).Check(run).Receipt!;
            Assert.Contains(checkedReceipt.Findings, f => f.RuleId == EvidenceRules.MandatoryMissing);
            Assert.NotEqual(DecisionStatus.Done, checkedReceipt.Decision.Status);
            Assert.Empty(checkedReceipt.Validations);
        }
    }

    [Fact]
    public async Task ApprovalRequired_WithoutOverride_IsNeedsApproval()
    {
        var (repo, run) = await StartedAndFinished(PermissionMode.ApprovalRequired);
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var receipt = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI)).Receipt!;
            Assert.Equal(DecisionStatus.NeedsApproval, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.ApprovalRequired);
        }
    }

    [Fact]
    public async Task Cli_EvidenceRecordAndCheck()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var o = new StringWriter();
            var e = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["check", run], o, e));
            Assert.Contains("NeedsEvidence", o.ToString());

            o = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["record", run, "--id", "t", "--command", "dotnet test", "--status", "pass", "--source", "user-declared", "--summary", "Passed 1"], o, e));
            Assert.Contains("UserDeclared", o.ToString());
            Assert.Equal(ValidationSource.UserDeclared, Assert.Single(new FileRunReceiptStore(repo.Root).Read(run).Validations).Source);
        }
    }

    [Fact]
    public async Task EachExpectedEvidenceItem_MustBeBacked_OrNotDone()
    {
        var repo = new TempGitRepo();
        using (repo)
        {
            var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["feature works"], ["**"], [],
                PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"],
                ["unit test log", "coverage report"], null));
            Assert.True(created.Created, created.Error);
            var git = new GitRepositoryStateReader(new GitCommandRunner());
            var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
            Assert.True(started.Started, started.Error);
            var git2 = new GitRepositoryStateReader(new GitCommandRunner());
            var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
                receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
                .FinishAsync(repo.Root, started.Baseline!.RunId);
            Assert.True(finished.Finished, finished.Error);
            var run = started.Baseline.RunId;
            SupportAcceptance(repo.Root, run);

            var partial = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, summary: "unit test log"));
            Assert.True(partial.Ok, partial.Error);
            Assert.Contains(partial.Receipt!.Findings, f => f.RuleId == EvidenceRules.ExpectedEvidenceMissing && f.Message.Contains("coverage report"));
            Assert.DoesNotContain(partial.Receipt.Findings, f => f.RuleId == EvidenceRules.ExpectedEvidenceMissing && f.Message.Contains("unit test log"));
            Assert.NotEqual(DecisionStatus.Done, partial.Receipt.Decision.Status);

            var complete = Service(repo.Root).Record(run, new ValidationEvidence("cov", run, "coverage", ValidationStatus.Pass, ValidationSource.CI, OutputSummary: "coverage report"));
            Assert.True(complete.Ok, complete.Error);
            Assert.DoesNotContain(complete.Receipt!.Findings, f => f.RuleId == EvidenceRules.ExpectedEvidenceMissing);
            Assert.Equal(DecisionStatus.Done, complete.Receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task RecordWithoutId_AllocatesUniqueIds_AndRefusesSilentOverwrite()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var svc = Service(repo.Root);
            var first = svc.Record(run, Ev(run, ValidationStatus.Fail, ValidationSource.CI, id: "", summary: "Failed"));
            Assert.True(first.Ok, first.Error);
            Assert.Equal("v1", first.Recorded!.ValidationId);
            Assert.Equal(ValidationStatus.Fail, Assert.Single(first.Receipt!.Validations).Status);

            var second = svc.Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.UserDeclared, id: "", summary: "test log"));
            Assert.True(second.Ok, second.Error);
            Assert.Equal("v2", second.Recorded!.ValidationId);
            Assert.Equal(2, second.Receipt!.Validations.Count);
            Assert.Contains(second.Receipt.Validations, v => v.ValidationId == "v1" && v.Status == ValidationStatus.Fail);
            Assert.Contains(second.Receipt.Validations, v => v.ValidationId == "v2" && v.Status == ValidationStatus.Pass);

            var clash = svc.Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, id: "v1", summary: "test log"));
            Assert.False(clash.Ok);
            Assert.Contains("--replace", clash.Error);
            Assert.Equal(ValidationStatus.Fail, new FileRunReceiptStore(repo.Root).Read(run).Validations.Single(v => v.ValidationId == "v1").Status);

            var replaced = svc.Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, id: "v1", summary: "test log"), replace: true);
            Assert.True(replaced.Ok, replaced.Error);
            Assert.Equal(ValidationStatus.Pass, replaced.Receipt!.Validations.Single(v => v.ValidationId == "v1").Status);
        }
    }

    [Fact]
    public async Task Cli_RecordWithoutId_DoesNotOverwrite()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var e = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["record", run, "--command", "dotnet test", "--status", "fail", "--source", "ci", "--summary", "Failed"], new StringWriter(), e));
            var o = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["record", run, "--command", "dotnet test", "--status", "pass", "--source", "user-declared", "--summary", "test log"], o, e));
            Assert.Contains("Evidence recorded: v2", o.ToString());
            var ids = new FileRunReceiptStore(repo.Root).Read(run).Validations.Select(v => v.ValidationId + ":" + v.Status).OrderBy(x => x).ToArray();
            Assert.Equal(["v1:Fail", "v2:Pass"], ids);

            o = new StringWriter();
            e = new StringWriter();
            Assert.Equal(1, EvidenceCommand.Run(repo.Root, ["record", run, "--id", "v1", "--command", "dotnet test", "--status", "pass", "--source", "ci", "--summary", "test log"], o, e));
            Assert.Contains("already exists", e.ToString());
        }
    }
}