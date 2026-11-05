using AgentsWatch.Cli;
using AgentsWatch.Core.Contracts;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RunContractTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "aw-contract-" + Guid.NewGuid().ToString("N"));
    private static readonly DateTimeOffset Now = new(2026, 11, 5, 17, 0, 0, TimeSpan.Zero);

    public RunContractTests() => Directory.CreateDirectory(_root);

    public void Dispose()
    {
        try { Directory.Delete(_root, true); } catch (IOException) { }
    }

    private ContractService Service() => new(new FileRunContractStore(_root), new FixedClock(Now));

    private static ContractDraft ValidDraft(SourceTaskRef? source = null) => new(
        "AW-VFY-003", null, "Implement RunContract v1", ["contract check returns findings"],
        ["src/AgentsWatch.Core/**"], ["src/AgentsWatch.Reports/**"], PermissionMode.WorkspaceWrite, RunMode.Implementation,
        [new ValidationRequirement("test", "dotnet test")], ["stop on scope expansion"], ["test output"], source);

    private static RunContract Valid() => new(1, "aw-vfy-003", "AW-VFY-003", "intent", ["ac"], ["src/**"], [],
        PermissionMode.WorkspaceWrite, RunMode.Implementation, new ValidationContract([new ValidationRequirement("t", "dotnet test")]),
        ["stop"], ["evidence"], null, Now);

    private static IEnumerable<string> Rules(RunContract c) => RunContractLinter.Check(c).Findings.Where(f => f.Severity == FindingSeverity.Error).Select(f => f.RuleId);

    [Fact]
    public void ValidImplementationContract_HasNoErrors()
    {
        var result = RunContractLinter.Check(Valid());
        Assert.True(result.IsValid);
        Assert.Empty(result.Findings);
    }

    [Fact]
    public void MissingIntent_IsError() => Assert.Contains(ContractRules.MissingIntent, Rules(Valid() with { Intent = " " }));

    [Fact]
    public void MissingAcceptanceCriteria_IsError() => Assert.Contains(ContractRules.MissingAcceptanceCriteria, Rules(Valid() with { AcceptanceCriteria = [] }));

    [Fact]
    public void MissingValidation_IsError()
    {
        Assert.Contains(ContractRules.MissingValidation, Rules(Valid() with { ValidationContract = new ValidationContract([]) }));
        Assert.Contains(ContractRules.MissingValidation, Rules(Valid() with { ValidationContract = new ValidationContract([new ValidationRequirement("t", "dotnet test", Mandatory: false)]) }));
    }

    [Fact]
    public void MissingStopAndEvidence_AreErrors()
    {
        var rules = Rules(Valid() with { StopRules = [], ExpectedEvidence = [""] }).ToList();
        Assert.Contains(ContractRules.MissingStopRules, rules);
        Assert.Contains(ContractRules.MissingExpectedEvidence, rules);
    }

    [Fact]
    public void InvestigationContract_MissingFields_AreWarningsOnly()
    {
        var result = RunContractLinter.Check(Valid() with { RunMode = RunMode.Investigation, AcceptanceCriteria = [], ValidationContract = new ValidationContract([]) });
        Assert.True(result.IsValid);
        Assert.Contains(result.Findings, f => f.RuleId == ContractRules.MissingAcceptanceCriteria && f.Severity == FindingSeverity.Warning);
    }

    [Theory]
    [InlineData("/etc/passwd")]
    [InlineData("C:/x")]
    [InlineData("../outside/**")]
    [InlineData("src/../../x")]
    [InlineData("")]
    [InlineData("a|b")]
    public void UnsafePathPatterns_AreRejected(string pattern) =>
        Assert.Contains(ContractRules.InvalidPathPattern, Rules(Valid() with { OwnedPaths = [pattern] }));

    [Fact]
    public void OwnedAvoidOverlap_IsError() => Assert.Contains(ContractRules.OwnedAvoidOverlap, Rules(Valid() with { AvoidPaths = ["src/**"] }));

    [Fact]
    public void UnsupportedSchemaVersion_FailsClearly()
    {
        Assert.Equal([ContractRules.UnsupportedSchema], Rules(Valid() with { SchemaVersion = 2 }));
        var json = RunContractJson.Serialize(Valid()).Replace("\"schemaVersion\": 1", "\"schemaVersion\": 99");
        var finding = Assert.Single(Service().CheckJson(json).Findings);
        Assert.Equal(ContractRules.UnsupportedSchema, finding.RuleId);
        Assert.Contains("99", finding.Reason);
        Assert.Equal(ContractRules.UnsupportedSchema, Assert.Single(Service().CheckJson("{\"contractId\":\"x\"}").Findings).RuleId);
        Assert.Equal(ContractRules.ParseError, Assert.Single(Service().CheckJson("{not json").Findings).RuleId);
    }

    [Fact]
    public void JsonRoundTrip_PreservesContract_WithCamelCaseAndStringEnums()
    {
        var c = Valid() with { SourceTaskRef = new SourceTaskRef("github", "ivanjovicic/AgentsWatch#12", "https://example.invalid/12") };
        var json = RunContractJson.Serialize(c);
        Assert.Contains("\"permissionMode\": \"workspace-write\"", json);
        Assert.Contains("\"runMode\": \"implementation\"", json);
        var back = RunContractJson.Deserialize(json);
        Assert.Equal(json, RunContractJson.Serialize(back));
        Assert.Equal(c.SourceTaskRef, back.SourceTaskRef);
        Assert.Equal(c.AcceptanceCriteria, back.AcceptanceCriteria);
    }

    [Fact]
    public void SourceTaskRef_IsOptional_AndOmittedFromJson()
    {
        var json = RunContractJson.Serialize(Valid());
        Assert.DoesNotContain("sourceTaskRef", json);
        Assert.Null(RunContractJson.Deserialize(json).SourceTaskRef);
    }

    [Fact]
    public void Create_WritesStablePath_AndRefusesOverwrite()
    {
        var service = Service();
        var first = service.Create(ValidDraft(new SourceTaskRef("prompt", "docs/prompts/AW-VFY-003-run-contract-v1.md")));
        Assert.True(first.Created, first.Error);
        Assert.Equal(Path.Combine(_root, ".agentwatch", "contracts", "aw-vfy-003.json"), first.Path);
        Assert.True(first.Check.IsValid);
        var original = File.ReadAllText(first.Path!);
        Assert.Contains("\"createdAtUtc\": \"2026-11-05T17:00:00+00:00\"", original);

        var second = service.Create(ValidDraft() with { Intent = "changed" });
        Assert.False(second.Created);
        Assert.Contains("already exists", second.Error);
        Assert.Equal(original, File.ReadAllText(first.Path!));

        Assert.True(service.Create(ValidDraft() with { Intent = "changed" }, overwrite: true).Created);
        Assert.Contains("changed", File.ReadAllText(first.Path!));
        Assert.True(service.CheckById("aw-vfy-003").IsValid);
    }

    [Theory]
    [InlineData("AW-VFY-003", "aw-vfy-003")]
    [InlineData("  Fix Login/Bug #12 ", "fix-login-bug-12")]
    [InlineData("a..b", "a.b")]
    public void ContractIds_AreDeterministicAndSafe(string taskId, string expected)
    {
        Assert.Equal(expected, ContractIds.FromTaskId(taskId));
        Assert.True(ContractIds.IsValid(expected));
    }

    [Fact]
    public void Create_RejectsUnsafeExplicitId()
    {
        var result = Service().Create(ValidDraft() with { ContractId = "../evil" });
        Assert.False(result.Created);
        Assert.False(Directory.Exists(Path.Combine(_root, ".agentwatch")));
    }

    [Fact]
    public void Create_DoesNotRequireUnrelatedPmMetadata()
    {
        var draft = new ContractDraft("T-1", null, "intent", ["ac"], [], [], PermissionMode.WorkspaceWrite, RunMode.Implementation,
            [new ValidationRequirement("v", "dotnet test")], ["stop"], ["evidence"], null);
        var result = Service().Create(draft);
        Assert.True(result.Created);
        Assert.True(result.Check.IsValid);
        var json = File.ReadAllText(result.Path!);
        foreach (var pm in new[] { "assignee", "priority", "labels", "sprint", "estimate", "status" })
        {
            Assert.DoesNotContain($"\"{pm}\"", json);
        }
    }

    [Fact]
    public void Cli_CreateAndCheck_EndToEnd()
    {
        var o = new StringWriter();
        var e = new StringWriter();
        var exit = ContractCommand.Run(_root, ["create", "AW-X-1", "--intent", "do x", "--accept", "x works", "--owned", "src/**",
            "--validate", "dotnet test", "--stop", "stop if unclear", "--evidence", "test log", "--source", "github:org/repo#1"], o, e);
        Assert.True(exit == 0, e.ToString());
        Assert.Contains("Valid: yes", o.ToString());

        Assert.Equal(0, ContractCommand.Run(_root, ["check", "aw-x-1"], new StringWriter(), e));
        Assert.Equal(1, ContractCommand.Run(_root, ["create", "AW-X-1", "--intent", "again"], new StringWriter(), e));

        var draftOut = new StringWriter();
        Assert.Equal(0, ContractCommand.Run(_root, ["create", "AW-X-2", "--intent", "draft"], draftOut, e));
        Assert.Contains("Valid: no", draftOut.ToString());
        var checkOut = new StringWriter();
        Assert.Equal(1, ContractCommand.Run(_root, ["check", ".agentwatch/contracts/aw-x-2.json"], checkOut, e));
        Assert.Contains(ContractRules.MissingAcceptanceCriteria, checkOut.ToString());
        Assert.Equal(2, ContractCommand.Run(_root, ["create", "AW-X-3", "--bogus", "1"], new StringWriter(), new StringWriter()));
    }

    private sealed class FixedClock(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }
}