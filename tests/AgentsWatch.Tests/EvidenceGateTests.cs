using System.Text.Json.Nodes;
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
        // Test-only: simulate a locally edited receipt whose checksum is also recomputed.
        // This does not authenticate the manual acceptance claim.
        store.Replace(RunReceiptJson.WithDigest(receipt with
        {
            AcceptanceCriteria = receipt.AcceptanceCriteria.Select(a => a with { Status = CriterionStatus.Supported, Reason = "marked supported in test" }).ToArray(),
            EvidenceDigest = null
        }));
    }


    [Fact]
    public async Task RemovedAcceptanceCriteria_CannotTurnEvidenceGateGreen()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var store = new FileRunReceiptStore(repo.Root);
            store.Replace(RunReceiptJson.WithDigest(store.Read(run) with { AcceptanceCriteria = [], EvidenceDigest = null }));

            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete
                && f.Message.Contains("acceptance criteria"));
        }
    }

    [Fact]
    public async Task SubstitutedAcceptanceCriteria_CannotTurnEvidenceGateGreen()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var store = new FileRunReceiptStore(repo.Root);
            var receipt = store.Read(run);
            store.Replace(RunReceiptJson.WithDigest(receipt with
            {
                AcceptanceCriteria = receipt.AcceptanceCriteria.Select(c => c with { Text = "different condition" }).ToArray(),
                EvidenceDigest = null
            }));

            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete);
        }
    }

    [Fact]
    public async Task CrossRunEvidence_IsRejectedAtImport_AndIgnoredIfAlreadyStored()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var wrongRun = Ev("unrelated-run", ValidationStatus.Pass, ValidationSource.CI);
            var imported = Service(repo.Root).Record(run, wrongRun);
            Assert.False(imported.Ok);
            Assert.Contains("does not match", imported.Error);
            Assert.Empty(new FileRunReceiptStore(repo.Root).Read(run).Validations);

            SupportAcceptance(repo.Root, run);
            var store = new FileRunReceiptStore(repo.Root);
            store.Replace(RunReceiptJson.WithDigest(store.Read(run) with { Validations = [wrongRun], EvidenceDigest = null }));
            var checkedReceipt = Service(repo.Root).Check(run).Receipt!;
            Assert.Equal(DecisionStatus.NeedsEvidence, checkedReceipt.Decision.Status);
            Assert.Contains(checkedReceipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete && f.Message.Contains("belongs to run"));
            Assert.Contains(checkedReceipt.Findings, f => f.RuleId == EvidenceRules.MandatoryMissing);
            var contract = RunContractJson.Deserialize(new FileRunContractStore(repo.Root).ReadRaw("t"));
            Assert.False(EvidenceGate.SatisfiesMandatory(checkedReceipt, contract));
        }
    }

    [Fact]
    public async Task MatchingIdWithDifferentCommand_DoesNotSatisfyMandatoryValidation()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, command: "echo skipped tests"));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.MandatoryMissing);
            var contract = RunContractJson.Deserialize(new FileRunContractStore(repo.Root).ReadRaw("t"));
            Assert.False(EvidenceGate.SatisfiesMandatory(result.Receipt, contract));
        }
    }

    [Fact]
    public async Task PassingStatusWithNonzeroExitCode_IsContradictoryEvidence()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var inconsistent = Ev(run, ValidationStatus.Pass, ValidationSource.CI) with { ExitCode = 7 };
            var result = Service(repo.Root).Record(run, inconsistent);
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete && f.Message.Contains("nonzero exit code"));
            var contract = RunContractJson.Deserialize(new FileRunContractStore(repo.Root).ReadRaw("t"));
            Assert.False(EvidenceGate.SatisfiesMandatory(result.Receipt, contract));
        }
    }

    [Fact]
    public async Task InvalidContractModifiedAfterFinish_CannotProduceDone()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var store = new FileRunContractStore(repo.Root);
            var original = RunContractJson.Deserialize(store.ReadRaw("t"));
            store.Write(original with { ValidationContract = new ValidationContract([]) }, overwrite: true);

            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete
                && f.Message.Contains("mandatory contract fields"));
        }
    }

    [Fact]
    public async Task ValidContractChangedDuringRun_MustNotBeCertified()
    {
        var repo = new TempGitRepo();
        using (repo)
        {
            var contracts = new FileRunContractStore(repo.Root);
            var draft = new ContractDraft("T", null, "intent", ["feature works"], ["src/**"], [],
                PermissionMode.WorkspaceWrite, RunMode.Implementation,
                [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null);
            var created = new ContractService(contracts).Create(draft);
            Assert.True(created.Created, created.Error);

            var git = new GitRepositoryStateReader(new GitCommandRunner());
            var active = new FileActiveRunStore(repo.Root);
            var started = await new RunStartService(contracts, active, git).StartAsync(repo.Root, "t");
            Assert.True(started.Started, started.Error);
            var run = started.Baseline!.RunId;

            var original = RunContractJson.Deserialize(contracts.ReadRaw("t"));
            var altered = original with { ValidationContract = new ValidationContract([new ValidationRequirement("t", "echo ok")]) };
            Assert.True(RunContractLinter.Check(altered).IsValid);
            contracts.Write(altered, overwrite: true);

            var finished = await new RunFinishService(active, new FileRunRecordStore(repo.Root), git, git,
                receipts: new FileRunReceiptStore(repo.Root), contracts: contracts, projections: new FileReceiptProjectionWriter())
                .FinishAsync(repo.Root, run);
            Assert.True(finished.Finished, finished.Error);

            SupportAcceptance(repo.Root, run);
            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, command: "echo ok"));
            Assert.True(result.Ok, result.Error);
            Assert.NotEqual(DecisionStatus.Done, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete
                && f.Message.Contains("contract", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public async Task LegacyActiveRunWithoutStartContractDigest_CannotBeCertified()
    {
        var repo = new TempGitRepo();
        using (repo)
        {
            var contracts = new FileRunContractStore(repo.Root);
            var created = new ContractService(contracts).Create(new ContractDraft("T", null, "intent",
                ["feature works"], ["**"], [], PermissionMode.WorkspaceWrite, RunMode.Implementation,
                [new ValidationRequirement("t", "dotnet test")], ["stop"], ["test log"], null));
            Assert.True(created.Created, created.Error);

            var git = new GitRepositoryStateReader(new GitCommandRunner());
            var active = new FileActiveRunStore(repo.Root);
            var started = await new RunStartService(contracts, active, git).StartAsync(repo.Root, "t");
            Assert.True(started.Started, started.Error);
            var run = started.Baseline!.RunId;
            File.WriteAllText(active.PathFor(run), RunBaselineJson.Serialize(started.Baseline with { ContractDigest = null }));

            var finished = await new RunFinishService(active, new FileRunRecordStore(repo.Root), git, git,
                receipts: new FileRunReceiptStore(repo.Root), contracts: contracts, projections: new FileReceiptProjectionWriter())
                .FinishAsync(repo.Root, run);
            Assert.True(finished.Finished, finished.Error);

            SupportAcceptance(repo.Root, run);
            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI));
            Assert.True(result.Ok, result.Error);
            Assert.Equal(DecisionStatus.NeedsEvidence, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete);
        }
    }

    [Fact]
    public async Task ValidContractChangedAfterFinish_IsRejected()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            SupportAcceptance(repo.Root, run);
            var store = new FileRunContractStore(repo.Root);
            var original = RunContractJson.Deserialize(store.ReadRaw("t"));
            Assert.Contains(original.ValidationContract.Requirements, r => r.Mandatory && r.Command == "dotnet test");
            store.Write(original with
            {
                ValidationContract = new ValidationContract([new ValidationRequirement("t", "echo ok")])
            }, overwrite: true);
            var edited = RunContractJson.Deserialize(store.ReadRaw("t"));
            Assert.True(RunContractLinter.Check(edited).IsValid);
            Assert.Equal(original.ContractId, edited.ContractId);
            Assert.Equal(original.TaskId, edited.TaskId);
            Assert.Equal(original.AcceptanceCriteria, edited.AcceptanceCriteria);

            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI, command: "echo ok", summary: "test log"));
            Assert.True(result.Ok, result.Error);
            Assert.NotEqual(DecisionStatus.Done, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == EvidenceRules.ReceiptIncomplete);
        }
    }

    [Fact]
    public async Task EvidenceCheck_WithoutDriftCheck_DoesNotDone_WhenOutsideOwnedAndAvoidTouched()
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
            repo.Write("docs/note.md", "avoided and outside owned");
            var git2 = new GitRepositoryStateReader(new GitCommandRunner());
            var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
                receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
                .FinishAsync(repo.Root, started.Baseline!.RunId);
            Assert.True(finished.Finished, finished.Error);
            var run = started.Baseline.RunId;

            SupportAcceptance(repo.Root, run);
            var result = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.CI));
            Assert.True(result.Ok, result.Error);
            Assert.NotEqual(DecisionStatus.Done, result.Receipt!.Decision.Status);
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == ScopeDriftRules.OutsideOwned && f.Paths.Contains("extra.txt"));
            Assert.Contains(result.Receipt.Findings, f => f.RuleId == ScopeDriftRules.AvoidTouched && f.Paths.Contains("docs/note.md"));
        }
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
            store.Replace(RunReceiptJson.WithDigest(receipt with { Claims = [new AgentClaim("c1", run, "ValidationPassed", "agent-prose", "all tests pass")], EvidenceDigest = null }));
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

    [Fact]
    public async Task MissingAcceptanceStatus_WithStaleDigest_CannotProduceDoneOrRewrite()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var path = new FileRunReceiptStore(repo.Root).PathFor(run);
            var node = JsonNode.Parse(File.ReadAllText(path))!;
            Assert.False(string.IsNullOrWhiteSpace(node["evidenceDigest"]?.GetValue<string>()));
            Assert.True(node["acceptanceCriteria"]![0]!.AsObject().Remove("status"));
            File.WriteAllText(path, node.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            var tampered = File.ReadAllText(path);
            Assert.DoesNotContain("status", node["acceptanceCriteria"]![0]!.ToJsonString());

            var o = new StringWriter();
            var e = new StringWriter();
            var code = EvidenceCommand.Run(repo.Root, ["record", run, "--command", "dotnet test", "--status", "pass", "--source", "user-declared", "--summary", "test log"], o, e);
            Assert.NotEqual(0, code);
            Assert.NotEqual(DecisionStatus.Done, Service(repo.Root).Check(run).Receipt?.Decision.Status);
            Assert.Equal(tampered, File.ReadAllText(path));
            Assert.DoesNotContain("\"done\"", File.ReadAllText(path), StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task MissingReceiptEvidenceDigest_CannotReblessTamperedAcceptanceOrRewrite(bool explicitNull)
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var store = new FileRunReceiptStore(repo.Root);
            var path = store.PathFor(run);
            var node = JsonNode.Parse(File.ReadAllText(path))!;
            node["acceptanceCriteria"]![0]!["status"] = "supported";
            node["acceptanceCriteria"]![0]!["reason"] = "manually marked supported";
            if (explicitNull)
            {
                node["evidenceDigest"] = null;
            }
            else
            {
                Assert.True(node.AsObject().Remove("evidenceDigest"));
            }

            File.WriteAllText(path, node.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            var tampered = File.ReadAllText(path);

            var evidence = Service(repo.Root).Record(run, Ev(run, ValidationStatus.Pass, ValidationSource.UserDeclared));
            Assert.False(evidence.Ok);
            Assert.Contains("digest", evidence.Error!, StringComparison.OrdinalIgnoreCase);
            Assert.False(new ClaimsService(repo.Root, store, new FileRunContractStore(repo.Root)).Check(run).Ok);
            Assert.False(new DriftService(repo.Root, store, new FileRunContractStore(repo.Root)).Check(run).Ok);
            Assert.Equal(tampered, File.ReadAllText(path));
        }
    }

    [Fact]
    public async Task EmptyRunDeltaObject_DoesNotThrow_AndIsRejectedWithoutRewrite()
    {
        var (repo, run) = await StartedAndFinished();
        using (repo)
        {
            var path = new FileRunReceiptStore(repo.Root).PathFor(run);
            var node = JsonNode.Parse(File.ReadAllText(path))!;
            node["runDelta"] = new JsonObject();
            File.WriteAllText(path, node.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            var tampered = File.ReadAllText(path);

            var o = new StringWriter();
            var err = new StringWriter();
            var thrown = Record.Exception(() => EvidenceCommand.Run(repo.Root, ["check", run], o, err));
            Assert.Null(thrown);
            Assert.Contains("invalid", err.ToString(), StringComparison.OrdinalIgnoreCase);
            Assert.Equal(tampered, File.ReadAllText(path));
        }
    }
}
