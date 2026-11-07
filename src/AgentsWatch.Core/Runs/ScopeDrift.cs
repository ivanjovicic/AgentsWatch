using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Paths;

namespace AgentsWatch.Core.Runs;

public static class ScopeDriftRules
{
    public const string OutsideOwned = "AWD001";
    public const string AvoidTouched = "AWD002";
    public const string RenameCrosses = "AWD003";
    public const string AmbiguousScope = "AWD004";
}

/// <summary>Deterministic scope check against owned/avoid patterns using classified run-interval evidence only.</summary>
public static class ScopeDriftChecker
{
    public static IReadOnlyList<Finding> Check(RunReceipt receipt, RunContract contract)
    {
        var findings = new List<Finding>();
        var n = 0;
        var owned = (contract.OwnedPaths ?? []).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
        var avoid = (contract.AvoidPaths ?? []).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

        void Add(string rule, FindingCategory category, FindingSeverity severity, FindingStatus status, string message, IReadOnlyList<string> paths, IReadOnlyList<string> refs) =>
            findings.Add(new Finding($"d{++n}", rule, category, severity, status, message, paths, refs, receipt.RunId, receipt.ContractId));

        foreach (var change in receipt.RunDelta.PreExistingUnchangedChanges ?? [])
        {
            _ = change; // never drift: dirty-before-start and unchanged during the run
        }

        foreach (var change in (receipt.RunDelta.AttributionAmbiguities ?? []).OrderBy(c => c.Path, StringComparer.Ordinal))
        {
            if (!AffectsScope(change, owned, avoid))
            {
                continue;
            }

            var paths = PathsOf(change);
            Add(ScopeDriftRules.AmbiguousScope, FindingCategory.AttributionAmbiguous, FindingSeverity.Warning, FindingStatus.NeedsReview,
                $"attribution of '{change.Path}'{(change.OldPath is null ? "" : $" (from {change.OldPath})")} is ambiguous, so scope drift was not inferred [{Context(change)}].",
                paths, ["runDelta.attributionAmbiguities"]);
        }

        foreach (var change in (receipt.RunDelta.RunIntervalChanges ?? []).Concat(receipt.RunDelta.PreExistingChangedFurther ?? [])
                     .OrderBy(c => c.Path, StringComparer.Ordinal))
        {
            Evaluate(change, owned, avoid, Add);
        }

        foreach (var change in (receipt.RunDelta.RunIntervalChanges ?? [])
                     .Concat(receipt.RunDelta.PreExistingChangedFurther ?? [])
                     .Concat(receipt.RunDelta.AttributionAmbiguities ?? [])
                     .OrderBy(c => c.Path, StringComparer.Ordinal))
        {
            if (!ContainsRedactedSegment(change.Path) && !ContainsRedactedSegment(change.OldPath))
            {
                continue;
            }

            Add(ScopeDriftRules.AmbiguousScope, FindingCategory.AttributionAmbiguous, FindingSeverity.Warning, FindingStatus.NeedsReview,
                "stored path contains redacted segments; original filename cannot be replayed against owned/avoid patterns.",
                PathsOf(change), ["runDelta"]);
        }

        return findings;
    }

    private static void Evaluate(ClassifiedChange change, string[] owned, string[] avoid, Action<string, FindingCategory, FindingSeverity, FindingStatus, string, IReadOnlyList<string>, IReadOnlyList<string>> add)
    {
        var path = change.Path;
        var oldPath = change.OldPath;
        var paths = PathsOf(change);
        var ctx = Context(change);
        var evref = change.Classification == ChangeClassification.PreExistingChangedFurther
            ? "runDelta.preExistingChangedFurther"
            : "runDelta.runIntervalChanges";

        var avoidHit = RepositoryPath.FirstMatchingPattern(avoid, path);
        var avoidOld = oldPath is null ? null : RepositoryPath.FirstMatchingPattern(avoid, oldPath);
        if (avoidHit is not null || avoidOld is not null)
        {
            var hit = avoidHit ?? avoidOld!;
            var which = avoidHit is not null ? path : oldPath!;
            add(ScopeDriftRules.AvoidTouched, FindingCategory.AvoidPathTouched, FindingSeverity.Error, FindingStatus.Unsupported,
                $"path '{which}' matches avoidPaths pattern '{hit}' [{ctx}].", paths, [evref, $"contract.avoidPaths[{hit}]"]);
        }

        if (oldPath is not null && owned.Length > 0)
        {
            var oldOwned = RepositoryPath.FirstMatchingPattern(owned, oldPath);
            var newOwned = RepositoryPath.FirstMatchingPattern(owned, path);
            if ((oldOwned is null) != (newOwned is null))
            {
                add(ScopeDriftRules.RenameCrosses, FindingCategory.ScopeOutsideOwnedPaths, FindingSeverity.Error, FindingStatus.Unsupported,
                    $"rename '{oldPath}' -> '{path}' crosses ownedPaths boundary (old matched {(oldOwned is null ? "none" : $"'{oldOwned}'")}; new matched {(newOwned is null ? "none" : $"'{newOwned}'")}) [{ctx}].",
                    paths, [evref, "contract.ownedPaths"]);
                return;
            }
        }

        if (owned.Length == 0)
        {
            return;
        }

        if (RepositoryPath.FirstMatchingPattern(owned, path) is null)
        {
            add(ScopeDriftRules.OutsideOwned, FindingCategory.ScopeOutsideOwnedPaths, FindingSeverity.Error, FindingStatus.Unsupported,
                $"path '{path}' is outside ownedPaths (no pattern matched; ownedPaths: {string.Join(", ", owned)}) [{ctx}].",
                paths, [evref, "contract.ownedPaths"]);
        }
        else if (oldPath is not null && RepositoryPath.FirstMatchingPattern(owned, oldPath) is null)
        {
            add(ScopeDriftRules.OutsideOwned, FindingCategory.ScopeOutsideOwnedPaths, FindingSeverity.Error, FindingStatus.Unsupported,
                $"renamed-from path '{oldPath}' is outside ownedPaths (no pattern matched; ownedPaths: {string.Join(", ", owned)}) [{ctx}].",
                paths, [evref, "contract.ownedPaths"]);
        }
    }

    internal static bool AffectsScope(ClassifiedChange change, string[] owned, string[] avoid)
    {
        if (RepositoryPath.FirstMatchingPattern(avoid, change.Path) is not null)
        {
            return true;
        }

        if (change.OldPath is not null && RepositoryPath.FirstMatchingPattern(avoid, change.OldPath) is not null)
        {
            return true;
        }

        if (owned.Length == 0)
        {
            return false;
        }

        var newOwned = RepositoryPath.FirstMatchingPattern(owned, change.Path) is not null;
        var oldOwned = change.OldPath is null || RepositoryPath.FirstMatchingPattern(owned, change.OldPath) is not null;
        return !newOwned || !oldOwned;
    }

    private static bool ContainsRedactedSegment(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        // First finish still sees raw paths; treat a path as ambiguous if it already
        // contains the placeholder or will after redaction, so AWD004 is stable.
        return path.Contains(ReceiptRedaction.Placeholder, StringComparison.Ordinal)
            || !string.Equals(path, ReceiptRedaction.RedactText(path), StringComparison.Ordinal);
    }

    private static string Context(ClassifiedChange change) =>
        $"attribution: {change.Classification}; status: {change.Status}";

    private static IReadOnlyList<string> PathsOf(ClassifiedChange change) =>
        change.OldPath is null ? [change.Path] : [change.Path, change.OldPath];
}

public sealed class DriftService(
    string workspaceRoot,
    IRunReceiptStore receipts,
    IRunContractStore contracts,
    IReceiptProjectionWriter? projections = null)
{
    public EvidenceCommandResult Check(string runId)
    {
        if (!ContractIds.IsValid(runId))
        {
            return new EvidenceCommandResult(false, null, null, $"invalid run id '{runId}'.");
        }

        if (!receipts.Exists(runId))
        {
            return new EvidenceCommandResult(false, null, receipts.PathFor(runId), $"no receipt for run '{runId}'.");
        }

        RunReceipt receipt;
        try
        {
            receipt = receipts.Read(runId);
            RunReceiptJson.EnsureStoredDigest(receipt);
        }
        catch (Exception ex) when (ex is InvalidDataException or System.Text.Json.JsonException)
        {
            return new EvidenceCommandResult(false, null, receipts.PathFor(runId), "receipt is invalid: " + ex.Message);
        }

        if (contracts.Exists(receipt.ContractId) != true)
        {
            return new EvidenceCommandResult(false, receipt, receipts.PathFor(runId), $"contract '{receipt.ContractId}' not found; cannot check scope.");
        }

        RunContract contract;
        try
        {
            contract = RunContractJson.Deserialize(contracts.ReadRaw(receipt.ContractId));
        }
        catch (Exception ex) when (ex is ContractFormatException or System.Text.Json.JsonException)
        {
            return new EvidenceCommandResult(false, receipt, receipts.PathFor(runId), "contract is invalid: " + ex.Message);
        }

        var kept = (receipt.Findings ?? []).Where(f => !f.RuleId.StartsWith("AWD", StringComparison.Ordinal)).ToList();
        var drift = ScopeDriftChecker.Check(receipt, contract);
        var merged = receipt with { Findings = kept.Concat(drift).ToArray(), EvidenceDigest = null };
        var updated = EvidenceGate.Apply(merged, contract);
        try
        {
            receipts.Replace(updated);
        }
        catch (IOException ex)
        {
            return new EvidenceCommandResult(false, updated, receipts.PathFor(runId), "could not persist receipt: " + ex.Message);
        }

        if (projections is not null)
        {
            try
            {
                projections.Write(workspaceRoot, updated);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return new EvidenceCommandResult(true, updated, receipts.PathFor(runId), "warning: projection failed (receipt updated): " + ex.Message);
            }
        }

        return new EvidenceCommandResult(true, updated, receipts.PathFor(runId), null);
    }
}
