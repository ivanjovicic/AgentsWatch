using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AgentsWatch.Core.Contracts;

/// <summary>
/// RunContract v1: verification normalization layer (not a task-management object).
/// Canonical JSON: .agentwatch/contracts/&lt;contract-id&gt;.json
/// </summary>
public sealed record RunContract(
    int SchemaVersion,
    string ContractId,
    string TaskId,
    string Intent,
    IReadOnlyList<string> AcceptanceCriteria,
    IReadOnlyList<string> OwnedPaths,
    IReadOnlyList<string> AvoidPaths,
    PermissionMode PermissionMode,
    RunMode RunMode,
    ValidationContract ValidationContract,
    IReadOnlyList<string> StopRules,
    IReadOnlyList<string> ExpectedEvidence,
    SourceTaskRef? SourceTaskRef,
    DateTimeOffset CreatedAtUtc)
{
    public const int CurrentSchemaVersion = 1;
}

public enum PermissionMode
{
    ReadOnly,
    WorkspaceWrite,
    ApprovalRequired
}

public enum RunMode
{
    Implementation,
    Investigation,
    Validation,
    Docs
}

public sealed record ValidationContract(IReadOnlyList<ValidationRequirement> Requirements);

public sealed record ValidationRequirement(string Id, string Command, bool Mandatory = true);

/// <summary>Opaque pointer to an external issue/prompt/roadmap item. Core never interprets the system.</summary>
public sealed record SourceTaskRef(string System, string Reference, string? Url = null);

public enum FindingSeverity
{
    Error,
    Warning
}

public sealed record ContractFinding(string RuleId, FindingSeverity Severity, string Reason);

public sealed record ContractCheckResult(IReadOnlyList<ContractFinding> Findings)
{
    public bool IsValid => Findings.All(f => f.Severity != FindingSeverity.Error);
}

public static class ContractRules
{
    public const string ParseError = "AWC000";
    public const string UnsupportedSchema = "AWC001";
    public const string InvalidContractId = "AWC002";
    public const string MissingTaskId = "AWC003";
    public const string MissingIntent = "AWC004";
    public const string MissingAcceptanceCriteria = "AWC005";
    public const string InvalidPathPattern = "AWC006";
    public const string MissingValidation = "AWC007";
    public const string MissingStopRules = "AWC008";
    public const string MissingExpectedEvidence = "AWC009";
    public const string NoOwnedPaths = "AWC010";
    public const string OwnedAvoidOverlap = "AWC011";
    public const string InvalidValidationRequirement = "AWC012";
}

public static partial class ContractIds
{
    [GeneratedRegex("^[a-z0-9][a-z0-9._-]{0,127}$")]
    private static partial Regex IdRegex();

    [GeneratedRegex("[^a-z0-9._-]+")]
    private static partial Regex InvalidChars();

    public static bool IsValid(string? id) => id is not null && IdRegex().IsMatch(id) && !id.Contains("..");

    /// <summary>Deterministic ID from task ID: lowercase, unsafe chars collapsed to '-'.</summary>
    public static string FromTaskId(string taskId)
    {
        var slug = InvalidChars().Replace(taskId.Trim().ToLowerInvariant(), "-").Trim('-', '.');
        while (slug.Contains(".."))
        {
            slug = slug.Replace("..", ".");
        }

        return slug.Length > 128 ? slug[..128] : slug;
    }

    public static string RelativePath(string contractId)
    {
        if (!IsValid(contractId))
        {
            throw new ArgumentException($"Invalid contract id '{contractId}'.", nameof(contractId));
        }

        return Path.Combine(".agentwatch", "contracts", contractId + ".json");
    }
}

public static class RunContractLinter
{
    private static readonly char[] ForbiddenPathChars = ['<', '>', '"', '|', '\0', ':'];

    public static ContractCheckResult Check(RunContract contract)
    {
        var findings = new List<ContractFinding>();
        void Error(string rule, string reason) => findings.Add(new ContractFinding(rule, FindingSeverity.Error, reason));
        void Warn(string rule, string reason) => findings.Add(new ContractFinding(rule, FindingSeverity.Warning, reason));

        if (contract.SchemaVersion != RunContract.CurrentSchemaVersion)
        {
            Error(ContractRules.UnsupportedSchema, $"schemaVersion {contract.SchemaVersion} is not supported (supported: {RunContract.CurrentSchemaVersion}).");
            return new ContractCheckResult(findings);
        }

        if (!ContractIds.IsValid(contract.ContractId))
        {
            Error(ContractRules.InvalidContractId, "contractId must match ^[a-z0-9][a-z0-9._-]{0,127}$ and not contain '..'.");
        }

        if (string.IsNullOrWhiteSpace(contract.TaskId))
        {
            Error(ContractRules.MissingTaskId, "taskId is required.");
        }

        var impl = contract.RunMode == RunMode.Implementation;
        if (string.IsNullOrWhiteSpace(contract.Intent))
        {
            (impl ? (Action<string, string>)Error : Warn)(ContractRules.MissingIntent, "intent is required.");
        }

        if (!NonEmpty(contract.AcceptanceCriteria))
        {
            (impl ? (Action<string, string>)Error : Warn)(ContractRules.MissingAcceptanceCriteria, "at least one non-empty acceptance criterion is required.");
        }

        foreach (var (pattern, kind) in (contract.OwnedPaths ?? []).Select(p => (p, "ownedPaths"))
                     .Concat((contract.AvoidPaths ?? []).Select(p => (p, "avoidPaths"))))
        {
            var problem = PathPatternProblem(pattern);
            if (problem is not null)
            {
                Error(ContractRules.InvalidPathPattern, $"{kind} entry '{pattern}': {problem}.");
            }
        }

        if (impl && !NonEmpty(contract.OwnedPaths))
        {
            Warn(ContractRules.NoOwnedPaths, "implementation contract has no ownedPaths; scope checks will be unbounded.");
        }

        foreach (var overlap in (contract.OwnedPaths ?? []).Intersect(contract.AvoidPaths ?? [], StringComparer.Ordinal))
        {
            Error(ContractRules.OwnedAvoidOverlap, $"'{overlap}' is listed in both ownedPaths and avoidPaths.");
        }

        var requirements = contract.ValidationContract?.Requirements ?? [];
        foreach (var r in requirements.Where(r => string.IsNullOrWhiteSpace(r.Id) || string.IsNullOrWhiteSpace(r.Command)))
        {
            Error(ContractRules.InvalidValidationRequirement, "validation requirement needs non-empty id and command.");
        }

        if (impl && !requirements.Any(r => r.Mandatory && !string.IsNullOrWhiteSpace(r.Command)))
        {
            Error(ContractRules.MissingValidation, "implementation contract requires at least one mandatory validation requirement.");
        }

        if (!NonEmpty(contract.StopRules))
        {
            (impl ? (Action<string, string>)Error : Warn)(ContractRules.MissingStopRules, "at least one stop rule is required.");
        }

        if (!NonEmpty(contract.ExpectedEvidence))
        {
            (impl ? (Action<string, string>)Error : Warn)(ContractRules.MissingExpectedEvidence, "at least one expected evidence item is required.");
        }

        return new ContractCheckResult(findings);
    }

    private static bool NonEmpty(IReadOnlyList<string>? items) => items is not null && items.Any(i => !string.IsNullOrWhiteSpace(i));

    /// <summary>Minimal safety check; not a glob engine.</summary>
    public static string? PathPatternProblem(string? pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern)) return "empty pattern";
        if (pattern != pattern.Trim()) return "leading/trailing whitespace";
        if (pattern.StartsWith('/') || pattern.StartsWith('\\') || (pattern.Length > 1 && pattern[1] == ':')) return "absolute paths are not allowed";
        if (pattern.IndexOfAny(ForbiddenPathChars) >= 0) return "contains forbidden characters";
        if (pattern.Split('/', '\\').Any(seg => seg == "..")) return "parent-directory traversal is not allowed";
        return null;
    }
}

public static class RunContractJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower, allowIntegerValues: false) }
    };

    public static string Serialize(RunContract contract) => JsonSerializer.Serialize(contract, Options);

    /// <summary>Reads schemaVersion first so unsupported versions fail clearly before shape binding.</summary>
    public static RunContract Deserialize(string json)
    {
        using (var doc = JsonDocument.Parse(json))
        {
            if (!doc.RootElement.TryGetProperty("schemaVersion", out var v) || v.ValueKind != JsonValueKind.Number || !v.TryGetInt32(out var version))
            {
                throw new ContractFormatException(ContractRules.UnsupportedSchema, "schemaVersion is missing or not an integer.");
            }

            if (version != RunContract.CurrentSchemaVersion)
            {
                throw new ContractFormatException(ContractRules.UnsupportedSchema, $"schemaVersion {version} is not supported (supported: {RunContract.CurrentSchemaVersion}).");
            }
        }

        try
        {
            return JsonSerializer.Deserialize<RunContract>(json, Options)
                ?? throw new ContractFormatException(ContractRules.ParseError, "contract JSON is null.");
        }
        catch (JsonException ex)
        {
            throw new ContractFormatException(ContractRules.ParseError, ex.Message);
        }
    }
}

public sealed class ContractFormatException(string ruleId, string reason) : Exception($"{ruleId}: {reason}")
{
    public string RuleId { get; } = ruleId;
    public string Reason { get; } = reason;
}