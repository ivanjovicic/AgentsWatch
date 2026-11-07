using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class TrustBoundaryAndRedactionTests
{
    internal const string InventedSentinel = "AWSENTINEL-TOKEN-c0ffee99deadbeef41";

    private static IReadOnlyList<CriterionValidationBinding>? BindAc1(string command = "dotnet test") =>
        [new CriterionValidationBinding("ac1", [command])];

    private static async Task<(TempGitRepo Repo, string RunId)> StartedAndFinished(
        IReadOnlyList<CriterionValidationBinding>? bindings = null,
        string validateCommand = "dotnet test",
        string acceptText = "feature works")
    {
        var repo = new TempGitRepo();
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft(
            "T", null, "intent", [acceptText], ["**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation,
            [new ValidationRequirement("t", validateCommand)], ["stop"], ["test log"], null,
            bindings));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t");
        Assert.True(started.Started, started.Error);
        repo.Write("notes/" + InventedSentinel + ".txt", "payload");
        var git2 = new GitRepositoryStateReader(new GitCommandRunner());
        var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
            receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root), projections: new FileReceiptProjectionWriter())
            .FinishAsync(repo.Root, started.Baseline!.RunId);
        Assert.True(finished.Finished, finished.Error);
        return (repo, started.Baseline.RunId);
    }

    private static EvidenceService Evidence(string root) =>
        new(root, new FileRunReceiptStore(root), new FileRunContractStore(root), new FileReceiptProjectionWriter());

    private static ValidationEvidence Ev(string runId, string command, ValidationStatus status = ValidationStatus.Pass, string id = "t", string? summary = "test log") =>
        new(id, runId, command, status, ValidationSource.UserDeclared, ExitCode: status == ValidationStatus.Pass ? 0 : 1, OutputSummary: summary);

    [Fact]
    public void UnverifiedAcceptance_IsNeedsReview_NotDone()
    {
        Assert.Equal(DecisionStatus.NeedsReview, DecisionOrdering.Classify([
            new Finding("e1", EvidenceRules.AcceptanceUnverified, FindingCategory.AcceptanceCriterionUnsupported,
                FindingSeverity.Error, FindingStatus.NeedsReview, "unverified", [], [EvidenceRules.AcceptanceUnverified])
        ]));
    }

    [Fact]
    public async Task ManualSupported_LinkedToUnrelatedPassingValidation_IsUnverifiedNeedsReview()
    {
        var (repo, run) = await StartedAndFinished(bindings: null, validateCommand: "dotnet --info");
        using (repo)
        {
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet --info")).Ok);
            var o = new StringWriter();
            var err = new StringWriter();
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "environment probe passed"], o, err));
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(CriterionStatus.Supported, Assert.Single(receipt.AcceptanceCriteria).Status);
            Assert.Equal(DecisionStatus.NeedsReview, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified && f.Message.Contains("unverified", StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(receipt.Decision.Reasons, r => r.StartsWith(EvidenceRules.Satisfied));
        }
    }

    [Fact]
    public async Task ManualSupported_WithDeclaredValidationForCriterion_CanReachDone()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1(), validateCommand: "dotnet test");
        using (repo)
        {
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet test")).Ok);
            var o = new StringWriter();
            var err = new StringWriter();
            Assert.Equal(0, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "recorded test log backs the criterion"], o, err));
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.Done, receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task ManualSupported_LinkedValidationNotDeclaredForCriterion_IsUnverified()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1("dotnet test"), validateCommand: "dotnet --info");
        using (repo)
        {
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet --info")).Ok);
            var o = new StringWriter();
            var err = new StringWriter();
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "unrelated probe"], o, err));
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.NeedsReview, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
        }
    }

    [Fact]
    public async Task ExtraFailValidation_PreventsDone_EvenWhenMandatoryPassedAndSupported()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1(), validateCommand: "dotnet test");
        using (repo)
        {
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet test", id: "t")).Ok);
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "extra-check", ValidationStatus.Fail, id: "extra", summary: "Failed")).Ok);
            var o = new StringWriter();
            var err = new StringWriter();
            AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "mandatory test passed"], o, err);
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.Failed, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.ValidationFailed);
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);
        }
    }

    [Fact]
    public async Task SentinelLikeValues_AreRedactedFromJsonMarkdownHandoffAndCli_AndDigestMatchesStoredForm()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1(), validateCommand: "dotnet test");
        using (repo)
        {
            var leakyCommand = "echo " + InventedSentinel;
            var result = Evidence(repo.Root).Record(run, Ev(run, leakyCommand, summary: "log " + InventedSentinel, id: "t"));
            Assert.True(result.Ok, result.Error);
            var receipt = result.Receipt!;
            Assert.DoesNotContain(InventedSentinel, receipt.Validations.Single().CommandDisplay);
            Assert.Contains(ReceiptRedaction.Placeholder, receipt.Validations.Single().CommandDisplay);
            Assert.True(RunReceiptJson.VerifyDigest(receipt));

            var jsonPath = new FileRunReceiptStore(repo.Root).PathFor(run);
            var json = File.ReadAllText(jsonPath);
            var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
            var handoff = File.ReadAllText(Path.Combine(repo.Root, ".ai", "handoffs", run + ".md"));
            Assert.DoesNotContain(InventedSentinel, json);
            Assert.DoesNotContain(InventedSentinel, md);
            Assert.DoesNotContain(InventedSentinel, handoff);

            var o = new StringWriter();
            var err = new StringWriter();
            EvidenceCommand.Run(repo.Root, ["check", run], o, err);
            ReceiptCommand.Show(repo.Root, ["show", run], o, err);
            Assert.DoesNotContain(InventedSentinel, o.ToString());
            Assert.DoesNotContain(InventedSentinel, err.ToString());
            Assert.Contains(ReceiptRedaction.Placeholder, json);
            Assert.True(RunReceiptJson.VerifyDigest(new FileRunReceiptStore(repo.Root).Read(run)));
        }
    }

    [Fact]
    public void Redaction_IsIdempotent_AndDoesNotBreakDigestOfAlreadyRedactedReceipt()
    {
        var sample = "cmd " + InventedSentinel;
        var once = ReceiptRedaction.RedactText(sample);
        var twice = ReceiptRedaction.RedactText(once);
        Assert.Equal(once, twice);
        Assert.Equal("cmd " + ReceiptRedaction.Placeholder, once);
        Assert.True(ReceiptRedaction.CommandsMatch(sample, once));
    }

    [Fact]
    public void ContractCreate_AcceptValidation_BindsToPrecedingCriterion()
    {
        using var repo = new TempGitRepo();
        var o = new StringWriter();
        var err = new StringWriter();
        var code = ContractCommand.Run(repo.Root, [
            "create", "T", "--intent", "intent", "--accept", "feature works", "--accept-validation", "dotnet test",
            "--validate", "dotnet test", "--owned", "**", "--stop", "stop", "--evidence", "test log"
        ], o, err);
        Assert.Equal(0, code);
        var contract = RunContractJson.Deserialize(new FileRunContractStore(repo.Root).ReadRaw("t"));
        var binding = Assert.Single(contract.CriterionValidationBindings!);
        Assert.Equal("ac1", binding.CriterionId);
        Assert.Equal("dotnet test", Assert.Single(binding.Commands));
    }
}
