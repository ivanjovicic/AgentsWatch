using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Core.Runs;

public enum ValidationStatus { Pass, Fail, NotRun, BlockedByEnvironment, TimedOut, Killed, Unknown }

public enum ValidationSource { AgentsWatchCommand, ImportedAgentResult, UserDeclared, CI, Unknown }

public enum CriterionStatus { Supported, Unsupported, Unknown }

public enum FindingCategory
{
    ContractIncomplete,
    AttributionAmbiguous,
    MissingValidation,
    ValidationFailed,
    ScopeOutsideOwnedPaths,
    AvoidPathTouched,
    UnsupportedClaim,
    AcceptanceCriterionUnsupported,
    RiskApprovalRequired,
    ExecutorIdentityConflict
}

public enum FindingStatus { Supported, Unsupported, Unknown, NeedsReview }

public enum DecisionStatus { Done, NeedsEvidence, NeedsReview, NeedsApproval, Blocked, Failed }

public sealed record RepositoryStateSnapshot(string Branch, string? HeadCommitSha, WorktreeState WorktreeState);

/// <summary>ValidationEvidence v1. Populated by AW-VFY-007; never synthesized from prose.</summary>
public sealed record ValidationEvidence(
    string ValidationId,
    string RunId,
    string CommandDisplay,
    ValidationStatus Status,
    ValidationSource Source,
    DateTimeOffset? StartedAtUtc = null,
    DateTimeOffset? FinishedAtUtc = null,
    long? DurationMs = null,
    int? ExitCode = null,
    string? OutputSummary = null,
    string? FirstErrorLine = null,
    string? SourceRef = null,
    string? EvidenceDigest = null);

/// <summary>AgentClaim v1. Populated by AW-VFY-009.</summary>
public sealed record AgentClaim(string ClaimId, string RunId, string ClaimType, string Source, string Value, string? RawText = null, CriterionStatus? Status = null, string? Reason = null, IReadOnlyList<string>? EvidenceRefs = null, IReadOnlyList<string>? Paths = null);

public sealed record AcceptanceCriterionResult(string CriterionId, string Text, CriterionStatus Status, IReadOnlyList<string> EvidenceRefs, string Reason);

public sealed record Finding(
    string FindingId,
    string RuleId,
    FindingCategory Category,
    FindingSeverity Severity,
    FindingStatus Status,
    string Message,
    IReadOnlyList<string> Paths,
    IReadOnlyList<string> EvidenceRefs,
    string? RunId = null,
    string? ContractId = null);

public sealed record DecisionOverride(string OverriddenBy, string OverrideReason, DateTimeOffset OverriddenAtUtc);

public sealed record RunDecision(DecisionStatus Status, IReadOnlyList<string> Reasons, DecisionOverride? Override = null);

/// <summary>
/// RunReceipt v1: canonical compact verification evidence at .agentwatch/runs/&lt;run-id&gt;.json.
/// No learning/nextPrompt/routing/narrative fields; Markdown is a projection.
/// ExecutorMetadata is the operator-declared executor (declaredExecutor). ObservedExecutorEvidence and
/// ExecutorIdentity are additive, optional (omitted when null) fields from AW-FIX-EXECUTOR-001, so historical
/// v1 receipts keep their exact JSON and digest. Neither field proves authorship.
/// </summary>
public sealed record RunReceipt(
    int SchemaVersion,
    string RunId,
    string ContractId,
    string TaskId,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset FinishedAtUtc,
    ExecutorMetadata? ExecutorMetadata,
    RepositoryStateSnapshot StartRepositoryState,
    RepositoryStateSnapshot EndRepositoryState,
    HeadTransition HeadTransition,
    RunDelta RunDelta,
    IReadOnlyList<ValidationEvidence> Validations,
    IReadOnlyList<AgentClaim> Claims,
    IReadOnlyList<AcceptanceCriterionResult> AcceptanceCriteria,
    IReadOnlyList<Finding> Findings,
    RunDecision Decision,
    IReadOnlyList<string> MissedWork,
    string AttributionPolicy,
    string? EvidenceDigest = null,
    string? ContractDigest = null,
    ObservedExecutorEvidence? ObservedExecutorEvidence = null,
    ExecutorIdentityAssessment? ExecutorIdentity = null)
{
    public const int CurrentSchemaVersion = 1;
}

public static class RunReceiptJson
{
    public static string Serialize(RunReceipt r) => JsonSerializer.Serialize(r, RunContractJson.Options);

    public static RunReceipt Deserialize(string json)
    {
        using (var doc = JsonDocument.Parse(json))
        {
            if (!doc.RootElement.TryGetProperty("schemaVersion", out var v) || v.ValueKind != JsonValueKind.Number || !v.TryGetInt32(out var version))
            {
                throw new InvalidDataException("receipt schemaVersion is missing or not an integer.");
            }

            if (version != RunReceipt.CurrentSchemaVersion)
            {
                throw new InvalidDataException($"receipt schemaVersion {version} is not supported (supported: {RunReceipt.CurrentSchemaVersion}).");
            }

            ValidateRequiredShape(doc.RootElement);
        }

        return JsonSerializer.Deserialize<RunReceipt>(json, RunContractJson.Options) ?? throw new InvalidDataException("receipt JSON is null.");
    }

    /// <summary>sha256 over the canonical JSON with evidenceDigest omitted.</summary>
    public static string ComputeDigest(RunReceipt r) =>
        "sha256:" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Serialize(r with { EvidenceDigest = null })))).ToLowerInvariant();

    public static RunReceipt WithDigest(RunReceipt r) => r with { EvidenceDigest = ComputeDigest(r) };

    public static bool VerifyDigest(RunReceipt r) => r.EvidenceDigest is not null && r.EvidenceDigest == ComputeDigest(r);

    public static void EnsureStoredDigest(RunReceipt r)
    {
        // Do not permit a missing digest to downgrade an existing receipt into an
        // unchecked record that the next mutation could silently re-sign.
        if (string.IsNullOrWhiteSpace(r.EvidenceDigest))
        {
            throw new InvalidDataException("receipt evidence digest is missing; refusing to rewrite an unverified receipt.");
        }

        if (!VerifyDigest(r))
        {
            throw new InvalidDataException("receipt evidence digest does not match its contents.");
        }
    }

    internal static void ValidateRequiredShape(JsonElement root)
    {
        if (root.TryGetProperty("executorIdentity", out var identity) && identity.ValueKind == JsonValueKind.Object)
        {
            // VERIFIED is reserved for a future provenance mechanism. A checksum is not a signature, so a receipt
            // edited to claim it (or to claim verified authorship) is rejected rather than displayed as fact.
            if (identity.TryGetProperty("status", out var s) && s.ValueKind == JsonValueKind.String
                && string.Equals(s.GetString(), "verified", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("executorIdentity.status VERIFIED is reserved and is never recorded by this version; refusing a receipt that claims it.");
            }

            if (identity.TryGetProperty("authorshipVerified", out var v) && v.ValueKind == JsonValueKind.True)
            {
                throw new InvalidDataException("executorIdentity.authorshipVerified=true is not supported: no mechanism verifies authorship (VERIFIED is reserved); refusing the receipt.");
            }
        }

        if (root.TryGetProperty("acceptanceCriteria", out var criteria) && criteria.ValueKind == JsonValueKind.Array)
        {
            var i = 0;
            foreach (var item in criteria.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object
                    || !item.TryGetProperty("status", out var status)
                    || status.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                {
                    throw new InvalidDataException($"acceptanceCriteria[{i}].status is missing; a missing status is not treated as Supported.");
                }

                i++;
            }
        }

        if (root.TryGetProperty("runDelta", out var delta))
        {
            if (delta.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidDataException("runDelta must be an object.");
            }

            foreach (var name in new[] { "runIntervalChanges", "preExistingUnchangedChanges", "preExistingChangedFurther", "attributionAmbiguities" })
            {
                if (!delta.TryGetProperty(name, out var arr) || arr.ValueKind != JsonValueKind.Array)
                {
                    throw new InvalidDataException($"runDelta.{name} is missing or not an array.");
                }
            }
        }
    }
}

public static class RunReceiptBuilder
{
    /// <summary>Builds a receipt from the finish record, then applies the Evidence Gate. Never invents validation results.
    /// The executor identity is always assessed (declaration vs optional additional source); it never affects the decision.</summary>
    public static RunReceipt Build(RunFinishRecord record, RunContract? contract, ObservedExecutorEvidence? observedExecutor = null)
    {
        var findings = new List<Finding>();
        var n = 0;
        string NextId() => $"f{++n}";

        if (contract is null)
        {
            findings.Add(new Finding(NextId(), "AWR001", FindingCategory.ContractIncomplete, FindingSeverity.Error, FindingStatus.Unknown,
                $"contract '{record.ContractId}' could not be loaded at finish.", [], [], record.RunId, record.ContractId));
        }

        foreach (var a in record.Delta.AttributionAmbiguities)
        {
            findings.Add(new Finding(NextId(), "AWR002", FindingCategory.AttributionAmbiguous, FindingSeverity.Warning, FindingStatus.NeedsReview,
                a.ClassificationReason, a.OldPath is null ? [a.Path] : [a.Path, a.OldPath], ["runDelta.attributionAmbiguities"], record.RunId, record.ContractId));
        }

        var identity = ExecutorIdentityEvaluator.Evaluate(record.StartBaseline.ExecutorMetadata, observedExecutor);
        if (identity.Status == ExecutorIdentityStatus.Conflict)
        {
            findings.Add(new Finding(NextId(), ExecutorIdentityRules.Conflict, FindingCategory.ExecutorIdentityConflict, FindingSeverity.Warning, FindingStatus.NeedsReview,
                $"operator-declared executor conflicts with implementation log metadata on: {string.Join(", ", identity.ConflictingFields)}; neither source proves authorship, and this finding alone does not invalidate the run or its evidence.",
                observedExecutor is null ? [] : [observedExecutor.SourcePath], ["executorMetadata", "observedExecutorEvidence"], record.RunId, record.ContractId));
        }

        var criteria = (contract?.AcceptanceCriteria ?? [])
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select((c, i) => new AcceptanceCriterionResult($"ac{i + 1}", c, CriterionStatus.Unknown, [], "not evaluated; mark with agentswatch accept record and link recorded validation evidence"))
            .ToArray();

        var baseline = record.StartBaseline;
        var receipt = new RunReceipt(
            RunReceipt.CurrentSchemaVersion,
            record.RunId,
            record.ContractId,
            record.TaskId,
            record.StartedAtUtc,
            record.FinishedAtUtc,
            baseline.ExecutorMetadata,
            new RepositoryStateSnapshot(baseline.Branch, baseline.HeadCommitSha, baseline.WorktreeState),
            new RepositoryStateSnapshot(record.Delta.EndBranch, record.Delta.EndHeadCommitSha, record.EndWorktreeState),
            record.HeadTransition,
            record.Delta,
            [],
            [],
            criteria,
            findings,
            new RunDecision(DecisionStatus.NeedsEvidence, ["pending evidence check"]),
            [],
            record.AttributionPolicy,
            // Preserve the start-time contract identity: a contract edited during the run must not be re-baselined at finish.
            ContractDigest: baseline.ContractDigest,
            ObservedExecutorEvidence: observedExecutor,
            ExecutorIdentity: identity);
        return EvidenceGate.Apply(receipt, contract);
    }
}

public interface IRunReceiptStore
{
    bool Exists(string runId);
    string PathFor(string runId);
    void WriteNew(RunReceipt receipt);
    void Replace(RunReceipt receipt);
    RunReceipt Read(string runId);
}

public sealed class FileRunReceiptStore(string workspaceRoot) : IRunReceiptStore
{
    private string Dir => Path.Combine(workspaceRoot, ".agentwatch", "runs");

    public string PathFor(string runId)
    {
        if (!ContractIds.IsValid(runId))
        {
            throw new ArgumentException($"Invalid run id '{runId}'.", nameof(runId));
        }

        return Path.Combine(Dir, runId + ".json");
    }

    public bool Exists(string runId) => File.Exists(PathFor(runId));

    public void WriteNew(RunReceipt receipt)
    {
        var path = PathFor(receipt.RunId);
        Directory.CreateDirectory(Dir);
        var temp = Path.Combine(Dir, $".{receipt.RunId}.{Guid.NewGuid():N}.tmp");
        File.WriteAllText(temp, RunReceiptJson.Serialize(receipt) + "\n");
        try
        {
            File.Move(temp, path, overwrite: false); // WriteNew
        }
        finally
        {
            if (File.Exists(temp))
            {
                File.Delete(temp);
            }
        }
    }

    public void Replace(RunReceipt receipt)
    {
        var path = PathFor(receipt.RunId);
        Directory.CreateDirectory(Dir);
        var temp = Path.Combine(Dir, $".{receipt.RunId}.{Guid.NewGuid():N}.tmp");
        File.WriteAllText(temp, RunReceiptJson.Serialize(receipt) + "\n");
        try
        {
            File.Move(temp, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temp))
            {
                File.Delete(temp);
            }
        }
    }

    public RunReceipt Read(string runId) => RunReceiptJson.Deserialize(File.ReadAllText(PathFor(runId)));
}

/// <summary>Port for generated Markdown projections (implemented in Reports).</summary>
public interface IReceiptProjectionWriter
{
    /// <summary>Writes projections; returns written paths.</summary>
    IReadOnlyList<string> Write(string workspaceRoot, RunReceipt receipt);
}