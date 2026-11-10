using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;
using AgentsWatch.Reports;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RunReceiptTests
{
    private static async Task<string> Start(TempGitRepo repo, ExecutorMetadata? executor = null)
    {
        var created = new ContractService(new FileRunContractStore(repo.Root)).Create(new ContractDraft("T", null, "intent", ["feature works", "docs updated"], ["**"], [],
            PermissionMode.WorkspaceWrite, RunMode.Implementation, [new ValidationRequirement("t", "dotnet test")], ["stop"], ["evidence"], null));
        Assert.True(created.Created, created.Error);
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var r = await new RunStartService(new FileRunContractStore(repo.Root), new FileActiveRunStore(repo.Root), git).StartAsync(repo.Root, "t", executor);
        Assert.True(r.Started, r.Error);
        return r.Baseline!.RunId;
    }

    private static RunFinishService Service(string root, IReceiptProjectionWriter? projections = null, IRunReceiptStore? receipts = null)
    {
        var git = new GitRepositoryStateReader(new GitCommandRunner());
        return new RunFinishService(new FileActiveRunStore(root), new FileRunRecordStore(root), git, git,
            receipts: receipts ?? new FileRunReceiptStore(root), contracts: new FileRunContractStore(root), projections: projections ?? new FileReceiptProjectionWriter());
    }

    private static async Task<RunFinishResult> Finish(TempGitRepo repo, string run, IReceiptProjectionWriter? projections = null)
    {
        var r = await Service(repo.Root, projections).FinishAsync(repo.Root, run);
        Assert.True(r.Finished, r.Error);
        return r;
    }

    [Fact]
    public async Task Finish_WritesCanonicalReceiptAndProjections_WithPlaceholderDecision()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo, new ExecutorMetadata(Agent: "cursor"));
        repo.Write("a.txt", "edit");
        var r = await Finish(repo, run);

        Assert.Equal(Path.Combine(repo.Root, ".agentwatch", "runs", run + ".json"), r.ReceiptPath);
        Assert.Empty(new FileActiveRunStore(repo.Root).ActiveRunIds());
        var receipt = new FileRunReceiptStore(repo.Root).Read(run);
        Assert.Equal(DecisionStatus.NeedsEvidence, receipt.Decision.Status);
        Assert.NotEqual(DecisionStatus.Done, receipt.Decision.Status);
        Assert.Empty(receipt.Validations);
        Assert.Empty(receipt.Claims);
        Assert.Equal(["feature works", "docs updated"], receipt.AcceptanceCriteria.Select(a => a.Text));
        Assert.All(receipt.AcceptanceCriteria, a => Assert.Equal(CriterionStatus.Unknown, a.Status));
        Assert.True(RunReceiptJson.VerifyDigest(receipt));
        Assert.Equal("cursor", receipt.ExecutorMetadata!.Agent);

        Assert.Equal(2, r.ProjectionPaths!.Count);
        var md = File.ReadAllText(Path.Combine(repo.Root, ".ai", "runs", run + ".md"));
        Assert.Contains("Generated from `.agentwatch/runs/" + run + ".json`", md);
        Assert.Contains("## Decision: NeedsEvidence", md);
        Assert.Contains("`M` a.txt", md);
        Assert.Contains("none recorded - no validation result is claimed", md);
        Assert.DoesNotContain("Pass", md.Split("## Validations")[1].Split("## Claims")[0]);
        Assert.True(File.Exists(Path.Combine(repo.Root, ".ai", "handoffs", run + ".md")));
    }

    [Fact]
    public async Task Projection_SeparatesIntervalPreExistingAndAmbiguous()
    {
        using var repo = new TempGitRepo();
        repo.Write("a.txt", "pre");
        repo.Write("b file.txt", "pre b");
        var run = await Start(repo);
        repo.Write("a.txt", "pre+more");
        repo.Write("c.txt", "new");
        repo.Git("add \"b file.txt\"");
        repo.Git("commit -q -m during");
        var receipt = (await Finish(repo, run)).Receipt!;
        var md = ReceiptMarkdownWriter.Write(receipt);

        string Section(string title) => md.Split("## " + title)[1].Split("\n## ")[0];
        Assert.Contains("c.txt", Section("Run-interval changes"));
        Assert.Contains("a.txt", Section("Pre-existing changed further"));
        Assert.Contains("b file.txt", Section("Ambiguous"));
        Assert.DoesNotContain("b file.txt", Section("Run-interval changes"));
        Assert.Contains(receipt.Findings, f => f.Category == FindingCategory.AttributionAmbiguous && f.Paths.Contains("b file.txt"));
        Assert.Contains(receipt.Decision.Reasons, x => x.Contains("ambiguous"));
        Assert.DoesNotContain("agent-authored", md);
    }

    [Fact]
    public async Task Receipt_RoundTrips_AndSchemaVersionIsEnforced()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        var receipt = (await Finish(repo, run)).Receipt!;
        var json = RunReceiptJson.Serialize(receipt);

        Assert.Contains("\"schemaVersion\": 1", json);
        Assert.Equal(json, RunReceiptJson.Serialize(RunReceiptJson.Deserialize(json)));
        Assert.Throws<InvalidDataException>(() => RunReceiptJson.Deserialize(json.Replace("\"schemaVersion\": 1", "\"schemaVersion\": 2")));
        Assert.Throws<InvalidDataException>(() => RunReceiptJson.Deserialize("{\"runId\":\"x\"}"));
    }

    [Fact]
    public async Task UnknownExecutor_AndNoLearningFields()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        var receipt = (await Finish(repo, run)).Receipt!;
        Assert.Null(receipt.ExecutorMetadata);
        Assert.Contains("Executor: unknown", ReceiptMarkdownWriter.Write(receipt));
        var json = RunReceiptJson.Serialize(receipt);
        foreach (var field in new[] { "learningNote", "nextPrompt", "routeSuggestion", "optimizationAdvice", "verboseSessionNarrative", "executorMetadata" })
        {
            Assert.DoesNotContain(field, json);
        }
    }

    [Fact]
    public async Task HandoffNextStep_DoesNotAffectCanonicalReceipt()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        var receipt = (await Finish(repo, run)).Receipt!;
        var before = File.ReadAllText(new FileRunReceiptStore(repo.Root).PathFor(run));
        var handoff = ReceiptHandoffWriter.Write(receipt, "try AW-VFY-007 next");

        Assert.Contains("## Next step (non-canonical)", handoff);
        Assert.Contains("try AW-VFY-007 next", handoff);
        Assert.Contains("Decision: NeedsEvidence", handoff);
        Assert.Equal(before, File.ReadAllText(new FileRunReceiptStore(repo.Root).PathFor(run)));
        Assert.True(RunReceiptJson.VerifyDigest(receipt));
    }

    [Fact]
    public async Task ProjectionFailure_LeavesCanonicalReceiptIntact()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        var r = await Finish(repo, run, new ThrowingProjections());
        Assert.NotNull(r.ProjectionError);
        Assert.True(RunReceiptJson.VerifyDigest(new FileRunReceiptStore(repo.Root).Read(run)));
        Assert.False(File.Exists(Path.Combine(repo.Root, ".ai", "runs", run + ".md")));
    }

    [Fact]
    public async Task ExistingReceipt_RefusesAndKeepsBaseline()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        var path = new FileRunReceiptStore(repo.Root).PathFor(run);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "{}");
        var r = await Service(repo.Root).FinishAsync(repo.Root, run);
        Assert.False(r.Finished);
        Assert.Equal("{}", File.ReadAllText(path));
        Assert.Single(new FileActiveRunStore(repo.Root).ActiveRunIds());
    }

    [Fact]
    public async Task TamperedReceipt_FailsDigest_AndCliShowWarns()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        await Finish(repo, run);
        var path = new FileRunReceiptStore(repo.Root).PathFor(run);
        File.WriteAllText(path, File.ReadAllText(path).Replace("\"needs-evidence\"", "\"done\""));
        Assert.False(RunReceiptJson.VerifyDigest(new FileRunReceiptStore(repo.Root).Read(run)));

        var o = new StringWriter();
        var e = new StringWriter();
        // AW-FIX-DIGEST-001 (owner plan 2026-11-10): a digest mismatch now fails show (exit 1, UNTRUSTED)
        // and handoff (exit 1, nothing written) instead of warning with exit 0.
        Assert.Equal(1, ReceiptCommand.Show(repo.Root, ["show", run], o, e));
        Assert.Contains("digest mismatch", e.ToString());
        Assert.StartsWith("UNTRUSTED", o.ToString());
        Assert.Equal(1, ReceiptCommand.Show(repo.Root, ["show", "nope"], new StringWriter(), new StringWriter()));
        Assert.Equal(1, ReceiptCommand.Handoff(repo.Root, [run], new StringWriter(), new StringWriter()));
        Assert.False(File.Exists(Path.Combine(repo.Root, ".ai", "handoffs", run + ".md.tmp")));
    }

    [Fact]
    public async Task ReceiptWriteFailure_IsRetryable()
    {
        using var repo = new TempGitRepo();
        var run = await Start(repo);
        repo.Write("a.txt", "edit");
        var inner = new FileRunReceiptStore(repo.Root);
        var receipts = new FailOnceReceiptStore(inner);
        var r1 = await Service(repo.Root, receipts: receipts).FinishAsync(repo.Root, run);
        Assert.False(r1.Finished);
        Assert.Contains("retried", r1.Error);
        Assert.Single(new FileActiveRunStore(repo.Root).ActiveRunIds());
        Assert.False(new FileRunRecordStore(repo.Root).Exists(run));

        var r2 = await Service(repo.Root, receipts: receipts).FinishAsync(repo.Root, run);
        Assert.True(r2.Finished, r2.Error);
        Assert.True(inner.Exists(run));
        Assert.Empty(new FileActiveRunStore(repo.Root).ActiveRunIds());
        Assert.True(RunReceiptJson.VerifyDigest(inner.Read(run)));
    }

    private sealed class FailOnceReceiptStore(IRunReceiptStore inner) : IRunReceiptStore
    {
        private int _attempts;

        public bool Exists(string runId) => inner.Exists(runId);
        public string PathFor(string runId) => inner.PathFor(runId);
        public RunReceipt Read(string runId) => inner.Read(runId);
        public void Replace(RunReceipt receipt) => inner.Replace(receipt);

        public void WriteNew(RunReceipt receipt)
        {
            if (System.Threading.Interlocked.Increment(ref _attempts) == 1)
            {
                throw new IOException("disk full (simulated)");
            }

            inner.WriteNew(receipt);
        }
    }

    private sealed class ThrowingProjections : IReceiptProjectionWriter
    {
        public IReadOnlyList<string> Write(string workspaceRoot, RunReceipt receipt) => throw new IOException("disk full (simulated)");
    }
}