using System.Text.Json;
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
        string acceptText = "feature works",
        IReadOnlyList<string>? avoidPaths = null)
    {
        var repo = new TempGitRepo();
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft(
            "T", null, "intent", [acceptText], ["**"], avoidPaths ?? [],
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
            Assert.Equal(1, AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "recorded test log backs the criterion"], o, err));
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(DecisionStatus.NeedsReview, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
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

    [Fact]
    public async Task P1_AssignmentSecrets_AsLastCommandToken_DoNotBreakJson_AndAreRedacted()
    {
        var secrets = new (string Command, string Secret)[]
        {
            ("echo token=demo123", "demo123"),
            ("echo password=demoPass1", "demoPass1"),
            ("echo api_key=demoKey1", "demoKey1")
        };
        foreach (var (command, secret) in secrets)
        {
            var (repo, run) = await StartedAndFinished(bindings: BindAc1(), validateCommand: "dotnet test");
            using (repo)
            {
                var result = Evidence(repo.Root).Record(run, Ev(run, command));
                Assert.True(result.Ok, result.Error);
                Assert.NotNull(result.Receipt);
                var jsonPath = new FileRunReceiptStore(repo.Root).PathFor(run);
                var json = File.ReadAllText(jsonPath);
                var parsed = JsonDocument.Parse(json);
                Assert.Equal(JsonValueKind.Object, parsed.RootElement.ValueKind);
                Assert.DoesNotContain(secret, json);
                Assert.DoesNotContain(secret, result.Receipt!.Validations.Single().CommandDisplay);
                Assert.Contains(ReceiptRedaction.Placeholder, result.Receipt.Validations.Single().CommandDisplay);
                Assert.True(RunReceiptJson.VerifyDigest(result.Receipt));
                var o = new StringWriter();
                var err = new StringWriter();
                EvidenceCommand.Run(repo.Root, ["check", run], o, err);
                Assert.DoesNotContain(secret, o.ToString());
                Assert.DoesNotContain(secret, err.ToString());
            }
        }
    }

    [Fact]
    public async Task P1_RedactedAvoidPath_ReplayKeepsNonDoneAndScopeFinding()
    {
        var avoided = "notes/" + InventedSentinel + ".txt";
        var (repo, run) = await StartedAndFinished(bindings: BindAc1(), avoidPaths: [avoided]);
        using (repo)
        {
            var first = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.NotEqual(DecisionStatus.Done, first.Decision.Status);
            Assert.Contains(first.Findings, f => f.RuleId == ScopeDriftRules.AvoidTouched);
            SupportAndPass(repo.Root, run);
            var after = Evidence(repo.Root).Check(run);
            Assert.True(after.Ok, after.Error);
            Assert.NotEqual(DecisionStatus.Done, after.Receipt!.Decision.Status);
            Assert.Contains(after.Receipt.Findings, f =>
                f.RuleId == ScopeDriftRules.AvoidTouched
                || f.RuleId == ScopeDriftRules.AmbiguousScope
                || f.RuleId == ScopeDriftRules.OutsideOwned
                || f.RuleId == ScopeDriftRules.RenameCrosses);
            Assert.DoesNotContain(InventedSentinel, File.ReadAllText(new FileRunReceiptStore(repo.Root).PathFor(run)));
        }
    }

    [Fact]
    public async Task P1_PreboundUnrelatedValidation_DoesNotYieldTrustworthyDone()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1("dotnet --info"), validateCommand: "dotnet --info");
        using (repo)
        {
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet --info")).Ok);
            var o = new StringWriter();
            var err = new StringWriter();
            AcceptCommand.Run(repo.Root, ["record", run, "--id", "ac1", "--status", "supported", "--evidence", "t", "--reason", "environment probe passed"], o, err);
            var receipt = new FileRunReceiptStore(repo.Root).Read(run);
            Assert.Equal(CriterionStatus.Supported, Assert.Single(receipt.AcceptanceCriteria).Status);
            Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);
            Assert.Equal(DecisionStatus.NeedsReview, receipt.Decision.Status);
            Assert.Contains(receipt.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified
                && f.Message.Contains("unverified", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public async Task P1_LinkedMissingOrFailedRef_IsUnverified_EvenIfAnotherDeclaredPassExists()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1("dotnet test"), validateCommand: "dotnet test");
        using (repo)
        {
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet test", id: "t")).Ok);
            Assert.True(Evidence(repo.Root).Record(run, Ev(run, "dotnet test", ValidationStatus.Fail, id: "bad", summary: "Failed")).Ok);
            StampSupported(repo.Root, run, ["validations[missing]"]);
            var missing = Evidence(repo.Root).Check(run).Receipt!;
            Assert.NotEqual(DecisionStatus.Done, missing.Decision.Status);
            Assert.Contains(missing.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);

            StampSupported(repo.Root, run, ["validations[bad]"]);
            var linkedFail = Evidence(repo.Root).Check(run).Receipt!;
            Assert.NotEqual(DecisionStatus.Done, linkedFail.Decision.Status);
            Assert.Contains(linkedFail.Findings, f => f.RuleId == EvidenceRules.AcceptanceUnverified);
        }
    }

    [Fact]
    public async Task P1_AllowedRedactedPath_FinishAndReplay_AreStableNeedsReview()
    {
        var repo = new TempGitRepo();
        using (repo)
        {
            var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft(
                "T", null, "docs audit", [], ["notes/**"], [],
                PermissionMode.WorkspaceWrite, RunMode.Docs,
                [], ["stop"], [], null));
            Assert.True(created.Created, created.Error);
            var git = new GitRepositoryStateReader(new GitCommandRunner());
            var started = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git)
                .StartAsync(repo.Root, "t");
            Assert.True(started.Started, started.Error);
            var run = started.Baseline!.RunId;
            repo.Write("notes/AWSENTINEL-TOKEN-fake123.txt", "payload");
            var git2 = new GitRepositoryStateReader(new GitCommandRunner());
            var finished = await new RunFinishService(new FileActiveRunStore(repo.Root), new FileRunRecordStore(repo.Root), git2, git2,
                receipts: new FileRunReceiptStore(repo.Root), contracts: new FileRunContractStore(repo.Root),
                projections: new FileReceiptProjectionWriter())
                .FinishAsync(repo.Root, run);
            Assert.True(finished.Finished, finished.Error);

            var store = new FileRunReceiptStore(repo.Root);
            var first = store.Read(run);
            Assert.Equal(DecisionStatus.NeedsReview, first.Decision.Status);
            Assert.Contains(first.Findings, f => f.RuleId == ScopeDriftRules.AmbiguousScope);
            Assert.DoesNotContain("AWSENTINEL-TOKEN-fake123", File.ReadAllText(store.PathFor(run)));
            Assert.True(RunReceiptJson.VerifyDigest(first));

            var replay1 = Evidence(repo.Root).Check(run);
            Assert.True(replay1.Ok, replay1.Error);
            var replay2 = Evidence(repo.Root).Check(run);
            Assert.True(replay2.Ok, replay2.Error);
            Assert.Equal(first.Decision.Status, replay1.Receipt!.Decision.Status);
            Assert.Equal(first.EvidenceDigest, replay1.Receipt.EvidenceDigest);
            Assert.Equal(replay1.Receipt.Decision.Status, replay2.Receipt!.Decision.Status);
            Assert.Equal(replay1.Receipt.EvidenceDigest, replay2.Receipt.EvidenceDigest);
            Assert.Equal(DecisionStatus.NeedsReview, replay1.Receipt.Decision.Status);
            Assert.Contains(replay1.Receipt.Findings, f => f.RuleId == ScopeDriftRules.AmbiguousScope);
            Assert.Contains(replay2.Receipt.Findings, f => f.RuleId == ScopeDriftRules.AmbiguousScope);
        }
    }

    [Fact]
    public async Task P2_DeltaSidecar_DoesNotPersistRawSentinelPaths()
    {
        var (repo, run) = await StartedAndFinished(bindings: BindAc1());
        using (repo)
        {
            var deltaPath = new FileRunRecordStore(repo.Root).PathFor(run);
            Assert.True(File.Exists(deltaPath));
            var delta = File.ReadAllText(deltaPath);
            JsonDocument.Parse(delta);
            Assert.DoesNotContain(InventedSentinel, delta);
            Assert.Contains(ReceiptRedaction.Placeholder, delta);
        }
    }

    private static void SupportAndPass(string root, string run)
    {
        StampSupported(root, run, []);
        Assert.True(Evidence(root).Record(run, Ev(run, "dotnet test")).Ok);
    }

    private static void StampSupported(string root, string run, IReadOnlyList<string> evidenceRefs)
    {
        var store = new FileRunReceiptStore(root);
        var receipt = store.Read(run);
        store.Replace(RunReceiptJson.WithDigest(receipt with
        {
            AcceptanceCriteria = receipt.AcceptanceCriteria.Select(a => a with
            {
                Status = CriterionStatus.Supported,
                Reason = "manual assessment (declared, not attested); marked in test",
                EvidenceRefs = evidenceRefs
            }).ToArray(),
            EvidenceDigest = null
        }));
    }
}
