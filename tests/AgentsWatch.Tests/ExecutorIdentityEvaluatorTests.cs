using System.Text;
using AgentsWatch.Core.Runs;
using Xunit;

namespace AgentsWatch.Tests;

/// <summary>AW-FIX-EXECUTOR-001 unit tests for the pure identity evaluator and the bounded v2 log parser.</summary>
public sealed class ExecutorIdentityEvaluatorTests
{
    private static ObservedExecutorEvidence Log(string? agent = null, string? provider = null, string? model = null, string? client = null) =>
        ExecutorLogParser.Parse(Encoding.UTF8.GetBytes(ExecutorIdentityFlowTests.V2Log(agent, provider, model, client)), "evidence/log.md");

    [Fact]
    public void ScenarioA_OperatorError_IsConflict_Unverified()
    {
        var a = ExecutorIdentityEvaluator.Evaluate(new ExecutorMetadata(Agent: "Claude"), Log("Codex", "OpenAI", "gpt-6", "ChatGPT Work"));
        Assert.Equal(ExecutorIdentityStatus.Conflict, a.Status);
        Assert.False(a.AuthorshipVerified);
        Assert.Equal(["agent"], a.ConflictingFields);
        Assert.Contains("Neither source independently proves who authored the changes.", a.Reason);
    }

    [Fact]
    public void ScenarioB_Agreement_IsConsistentUnverified()
    {
        var a = ExecutorIdentityEvaluator.Evaluate(new ExecutorMetadata(Agent: "Claude"), Log("Claude"));
        Assert.Equal(ExecutorIdentityStatus.ConsistentUnverified, a.Status);
        Assert.False(a.AuthorshipVerified);
        Assert.Contains("not verification", a.Reason);
    }

    [Fact]
    public void ScenarioC_NoAdditionalSource_IsDeclaredOnly()
    {
        var a = ExecutorIdentityEvaluator.Evaluate(new ExecutorMetadata(Agent: "Claude"), null);
        Assert.Equal(ExecutorIdentityStatus.DeclaredOnly, a.Status);
        Assert.False(a.AuthorshipVerified);
    }

    [Fact]
    public void ScenarioD_NothingUsable_IsUnknown()
    {
        var malformed = ExecutorLogParser.Parse(Encoding.UTF8.GetBytes("garbage"), "evidence/log.md");
        Assert.NotNull(malformed.Problem);
        Assert.Equal(ExecutorIdentityStatus.Unknown, ExecutorIdentityEvaluator.Evaluate(null, null).Status);
        Assert.Equal(ExecutorIdentityStatus.Unknown, ExecutorIdentityEvaluator.Evaluate(new ExecutorMetadata(), malformed).Status);
        Assert.Equal(ExecutorIdentityStatus.Unknown, ExecutorIdentityEvaluator.Evaluate(new ExecutorMetadata(Agent: "Claude"), malformed).Status);
        Assert.Equal(ExecutorIdentityStatus.Unknown, ExecutorIdentityEvaluator.Evaluate(null, Log("Codex")).Status);
    }

    [Theory]
    [InlineData("  Claude  ", "claude")]
    [InlineData("CLAUDE\tCODE", "claude code")]
    [InlineData("Claude\u00A0\u00A0Code", "claude code")]
    [InlineData("unknown-not-exposed", null)]
    [InlineData("Unknown", null)]
    [InlineData("n/a", null)]
    [InlineData("   ", null)]
    [InlineData(null, null)]
    public void Normalize_IsDeterministicAndDocumented(string? raw, string? expected) =>
        Assert.Equal(expected, ExecutorIdentityEvaluator.Normalize(raw));

    [Fact]
    public void NeverVerified_AcrossAllCombinations()
    {
        string?[] values = [null, "", "Claude", "claude", " CLAUDE ", "Codex", "unknown-not-exposed", "verified", "VERIFIED"];
        var logs = new List<ObservedExecutorEvidence?> { null, ExecutorLogParser.Parse(Encoding.UTF8.GetBytes("x"), "l.md") };
        foreach (var agent in values)
        {
            foreach (var model in values)
            {
                logs.Add(Log(agent, "Anthropic", model, "Claude Code"));
            }
        }

        foreach (var agent in values)
        {
            foreach (var model in values)
            {
                foreach (var log in logs)
                {
                    var a = ExecutorIdentityEvaluator.Evaluate(new ExecutorMetadata(agent, "tool", model), log);
                    Assert.NotEqual(ExecutorIdentityStatus.Verified, a.Status);
                    Assert.False(a.AuthorshipVerified);
                }
            }
        }
    }

    [Fact]
    public void Parser_ReadsOnlyBoundedV2HeaderFields_AndHashesBytes()
    {
        var bytes = Encoding.UTF8.GetBytes(ExecutorIdentityFlowTests.V2Log("Codex", "OpenAI", "gpt-6", "ChatGPT Work",
            extraHeader: "Identity status: VERIFIED\n", body: "Agent/tool: Narrative\n"));
        var e = ExecutorLogParser.Parse(bytes, "evidence/log.md");
        Assert.Null(e.Problem);
        Assert.Equal(("Codex", "OpenAI", "gpt-6", "ChatGPT Work", "v2"), (e.AgentTool, e.ModelProvider, e.ModelName, e.ClientIde, e.Format));
        Assert.Equal("evidence/log.md", e.SourcePath);
        Assert.Equal("implementation-run-log", e.SourceKind);
        Assert.Equal("sha256:" + Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(bytes)).ToLowerInvariant(), e.SourceSha256);
    }

    [Fact]
    public void Parser_AcceptsCrLf_AndRejectsMalformedHeaders()
    {
        Assert.Equal("Codex", ExecutorLogParser.Parse(Encoding.UTF8.GetBytes(ExecutorIdentityFlowTests.V2Log("Codex").Replace("\n", "\r\n")), "l.md").AgentTool);
        Assert.NotNull(ExecutorLogParser.Parse(Encoding.UTF8.GetBytes(ExecutorIdentityFlowTests.V2Log("Codex", format: null)), "l.md").Problem);
        Assert.NotNull(ExecutorLogParser.Parse(Encoding.UTF8.GetBytes(ExecutorIdentityFlowTests.V2Log("Codex", extraHeader: "Agent/tool: Codex\n")), "l.md").Problem);
        Assert.NotNull(ExecutorLogParser.Parse(Encoding.UTF8.GetBytes(ExecutorIdentityFlowTests.V2Log(new string('a', ExecutorLogParser.MaxValueLength + 1))), "l.md").Problem);
    }
}
