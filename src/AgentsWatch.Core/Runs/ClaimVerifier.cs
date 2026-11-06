using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Paths;

namespace AgentsWatch.Core.Runs;

public static class ClaimTypes
{
    public const string TestsAdded = "TestsAdded";
    public const string DocsOnly = "DocsOnly";
    public const string BackendUnchanged = "BackendUnchanged";
    public const string MigrationAdded = "MigrationAdded";
    public const string ValidationPassed = "ValidationPassed";
    public const string NoUnrelatedChanges = "NoUnrelatedChanges";
}

public static class ClaimRules
{
    public const string Unsupported = "AWK001";
    public const string Unknown = "AWK002";
}

public static class ClaimVerifier
{
    public static (IReadOnlyList<AgentClaim> Claims, IReadOnlyList<Finding> Findings) Evaluate(RunReceipt receipt, RunContract? contract)
    {
        var claims = new List<AgentClaim>();
        var findings = new List<Finding>();
        var n = 0;
        foreach (var claim in receipt.Claims ?? [])
        {
            var evaluated = EvaluateOne(receipt, contract, claim);
            claims.Add(evaluated);
            if (evaluated.Status is CriterionStatus.Unsupported)
            {
                findings.Add(new Finding($"k{++n}", ClaimRules.Unsupported, FindingCategory.UnsupportedClaim, FindingSeverity.Error, FindingStatus.Unsupported,
                    $"claim {evaluated.ClaimType} is unsupported: {evaluated.Reason}", evaluated.Paths ?? [], evaluated.EvidenceRefs ?? [], receipt.RunId, receipt.ContractId));
            }
            else if (evaluated.Status is CriterionStatus.Unknown)
            {
                findings.Add(new Finding($"k{++n}", ClaimRules.Unknown, FindingCategory.UnsupportedClaim, FindingSeverity.Warning, FindingStatus.Unknown,
                    $"claim {evaluated.ClaimType} is unknown: {evaluated.Reason}", evaluated.Paths ?? [], evaluated.EvidenceRefs ?? [], receipt.RunId, receipt.ContractId));
            }
        }

        return (claims, findings);
    }

    public static bool TryParseBooleanValue(string? value, out bool asserted)
    {
        asserted = false;
        switch (value?.Trim().ToLowerInvariant())
        {
            case "true":
            case "yes":
            case "1":
                asserted = true;
                return true;
            case "false":
            case "no":
            case "0":
                return true;
            default:
                return false;
        }
    }

    public static AgentClaim EvaluateOne(RunReceipt receipt, RunContract? contract, AgentClaim claim)
    {
        if (!TryParseBooleanValue(claim.Value, out var asserted))
        {
            return Finish(claim, CriterionStatus.Unknown, $"claim value '{claim.Value}' is not a boolean (true/yes/1 or false/no/0).", ["receipt.claims"], []);
        }

        var affirmative = claim.ClaimType switch
        {
            ClaimTypes.TestsAdded => TestsAdded(receipt, claim),
            ClaimTypes.DocsOnly => DocsOnly(receipt, claim),
            ClaimTypes.BackendUnchanged => BackendUnchanged(receipt, claim),
            ClaimTypes.MigrationAdded => MigrationAdded(receipt, claim),
            ClaimTypes.ValidationPassed => ValidationPassed(receipt, claim),
            ClaimTypes.NoUnrelatedChanges => NoUnrelatedChanges(receipt, contract, claim),
            _ => Finish(claim, CriterionStatus.Unknown, $"claim type '{claim.ClaimType}' is not in the v1 set.", ["receipt.claims"], [])
        };
        return asserted ? affirmative : Negate(affirmative);
    }

    private static AgentClaim Negate(AgentClaim affirmative)
    {
        var status = affirmative.Status switch
        {
            CriterionStatus.Supported => CriterionStatus.Unsupported,
            CriterionStatus.Unsupported => CriterionStatus.Supported,
            _ => CriterionStatus.Unknown
        };
        var prefix = status switch
        {
            CriterionStatus.Supported => $"negated claim ({affirmative.ClaimType}=false) holds: ",
            CriterionStatus.Unsupported => $"negated claim ({affirmative.ClaimType}=false) does not hold: ",
            _ => $"negated claim ({affirmative.ClaimType}=false) remains unknown: "
        };
        return affirmative with { Status = status, Reason = prefix + affirmative.Reason };
    }

    internal static IReadOnlyList<ClassifiedChange> Attributable(RunDelta delta) =>
        delta.RunIntervalChanges.Concat(delta.PreExistingChangedFurther).ToArray();

    private static AgentClaim TestsAdded(RunReceipt receipt, AgentClaim claim)
    {
        var files = Attributable(receipt.RunDelta);
        if (files.Count == 0)
        {
            return Finish(claim, CriterionStatus.Unknown, "no high-confidence attributable files; test-location support cannot be determined.", ["runDelta"], []);
        }

        var tests = files.Where(f => PathClassifier.Classify(f.Path) == PathKind.Test || (f.OldPath is not null && PathClassifier.Classify(f.OldPath) == PathKind.Test)).ToArray();
        return tests.Length > 0
            ? Finish(claim, CriterionStatus.Supported, "attributable changes include test files.", ["runDelta.runIntervalChanges"], tests.Select(t => t.Path).ToArray())
            : Finish(claim, CriterionStatus.Unsupported, "attributable changes do not include files matching test path heuristics.", ["runDelta.runIntervalChanges"], files.Select(f => f.Path).ToArray());
    }

    private static AgentClaim DocsOnly(RunReceipt receipt, AgentClaim claim)
    {
        var files = Attributable(receipt.RunDelta);
        if (files.Count == 0)
        {
            return Finish(claim, CriterionStatus.Unknown, "no high-confidence attributable files; cannot confirm a docs-only run.", ["runDelta"], []);
        }

        var nonDocs = files.Where(f => PathClassifier.Classify(f.Path) is not PathKind.Docs).ToArray();
        return nonDocs.Length == 0
            ? Finish(claim, CriterionStatus.Supported, "all attributable files match documentation patterns.", ["runDelta.runIntervalChanges"], files.Select(f => f.Path).ToArray())
            : Finish(claim, CriterionStatus.Unsupported, "attributable non-documentation files exist.", ["runDelta.runIntervalChanges"], nonDocs.Select(f => f.Path).ToArray());
    }

    private static AgentClaim BackendUnchanged(RunReceipt receipt, AgentClaim claim)
    {
        var files = Attributable(receipt.RunDelta);
        if (files.Count == 0)
        {
            return Finish(claim, CriterionStatus.Supported, "no high-confidence attributable files, so no backend change was observed.", ["runDelta"], []);
        }

        var kinds = files.Select(f => (f.Path, Kind: PathClassifier.Classify(f.Path))).ToArray();
        var backend = kinds.Where(k => k.Kind == PathKind.Backend).ToArray();
        if (backend.Length > 0)
        {
            return Finish(claim, CriterionStatus.Unsupported, "attributable backend files were changed.", ["runDelta.runIntervalChanges"], backend.Select(b => b.Path).ToArray());
        }

        if (kinds.Any(k => k.Kind == PathKind.Other))
        {
            return Finish(claim, CriterionStatus.Unknown, "some attributable paths have no recognized stack classification.", ["runDelta.runIntervalChanges"], kinds.Where(k => k.Kind == PathKind.Other).Select(k => k.Path).ToArray());
        }

        return Finish(claim, CriterionStatus.Supported, "attributable files are classified as non-backend.", ["runDelta.runIntervalChanges"], files.Select(f => f.Path).ToArray());
    }

    private static AgentClaim MigrationAdded(RunReceipt receipt, AgentClaim claim)
    {
        var files = Attributable(receipt.RunDelta);
        if (files.Count == 0)
        {
            return Finish(claim, CriterionStatus.Unknown, "no high-confidence attributable files; cannot confirm a migration was added.", ["runDelta"], []);
        }

        var added = files.Where(f => PathClassifier.Classify(f.Path) == PathKind.Migration && IsAdd(f.Status)).ToArray();
        return added.Length > 0
            ? Finish(claim, CriterionStatus.Supported, "attributable added files match migration path heuristics.", ["runDelta.runIntervalChanges"], added.Select(a => a.Path).ToArray())
            : Finish(claim, CriterionStatus.Unsupported, "no attributable added migration files were observed.", ["runDelta.runIntervalChanges"], files.Select(f => f.Path).ToArray());
    }

    private static AgentClaim ValidationPassed(RunReceipt receipt, AgentClaim claim)
    {
        var validations = receipt.Validations ?? [];
        if (validations.Count == 0)
        {
            return Finish(claim, CriterionStatus.Unsupported, "no structured validation evidence; agent prose is not sufficient.", ["receipt.validations"], []);
        }

        if (validations.Any(v => v.Status is ValidationStatus.Fail or ValidationStatus.TimedOut or ValidationStatus.Killed))
        {
            var bad = validations.First(v => v.Status is ValidationStatus.Fail or ValidationStatus.TimedOut or ValidationStatus.Killed);
            return Finish(claim, CriterionStatus.Unsupported, $"validation '{bad.ValidationId}' is {bad.Status} (source: {bad.Source}).", [$"validations[{bad.ValidationId}]"], []);
        }

        var pass = validations.Where(v => v.Status == ValidationStatus.Pass).ToArray();
        if (pass.Length > 0)
        {
            if (pass.All(v => v.Source == ValidationSource.Unknown))
            {
                return Finish(claim, CriterionStatus.Unknown, "passing validation evidence has an unknown source.", pass.Select(p => $"validations[{p.ValidationId}]").ToArray(), []);
            }

            return Finish(claim, CriterionStatus.Supported, "structured validation evidence has status Pass.", pass.Select(p => $"validations[{p.ValidationId}]").ToArray(), []);
        }

        return Finish(claim, CriterionStatus.Unknown, "validation evidence exists but none has status Pass.", ["receipt.validations"], []);
    }

    private static AgentClaim NoUnrelatedChanges(RunReceipt receipt, RunContract? contract, AgentClaim claim)
    {
        if (contract is null)
        {
            return Finish(claim, CriterionStatus.Unknown, "contract is unavailable; ownership cannot be evaluated.", ["contract"], []);
        }

        var drift = ScopeDriftChecker.Check(receipt, contract);
        var blocking = drift.Where(f => f.RuleId is ScopeDriftRules.OutsideOwned or ScopeDriftRules.AvoidTouched or ScopeDriftRules.RenameCrosses).ToArray();
        if (blocking.Length > 0)
        {
            return Finish(claim, CriterionStatus.Unsupported, "scope drift findings show unrelated attributable changes.", blocking.Select(f => f.RuleId).ToArray(), blocking.SelectMany(f => f.Paths).Distinct().ToArray());
        }

        if (drift.Any(f => f.RuleId == ScopeDriftRules.AmbiguousScope))
        {
            return Finish(claim, CriterionStatus.Unknown, "scope attribution is ambiguous; NoUnrelatedChanges is not supported.", [ScopeDriftRules.AmbiguousScope], drift.Where(f => f.RuleId == ScopeDriftRules.AmbiguousScope).SelectMany(f => f.Paths).ToArray());
        }

        return Finish(claim, CriterionStatus.Supported, "no unrelated attributable changes against owned/avoid paths.", ["runDelta", "contract.ownedPaths"], []);
    }

    private static bool IsAdd(string status) =>
        status.Equals("A", StringComparison.OrdinalIgnoreCase) || status.Equals("??", StringComparison.Ordinal) || status.Equals("R", StringComparison.OrdinalIgnoreCase);

    private static AgentClaim Finish(AgentClaim claim, CriterionStatus status, string reason, IReadOnlyList<string> refs, IReadOnlyList<string> paths) =>
        claim with { Status = status, Reason = reason, EvidenceRefs = refs, Paths = paths };
}

public sealed class ClaimsService(
    string workspaceRoot,
    IRunReceiptStore receipts,
    IRunContractStore contracts,
    IReceiptProjectionWriter? projections = null)
{
    public EvidenceCommandResult Record(string runId, AgentClaim claim, bool replace = false)
    {
        var loaded = Load(runId);
        if (!loaded.Ok)
        {
            return loaded;
        }

        if (!ClaimVerifier.TryParseBooleanValue(claim.Value, out _))
        {
            return new EvidenceCommandResult(false, loaded.Receipt, loaded.Path, $"claim value '{claim.Value}' is not a boolean (true/yes/1 or false/no/0).");
        }

        var list = loaded.Receipt!.Claims.ToList();
        if (string.IsNullOrWhiteSpace(claim.ClaimId))
        {
            claim = claim with { ClaimId = NextId(list) };
        }
        else
        {
            var i = list.FindIndex(c => c.ClaimId == claim.ClaimId);
            if (i >= 0)
            {
                if (!replace)
                {
                    return new EvidenceCommandResult(false, loaded.Receipt, loaded.Path, $"claim '{claim.ClaimId}' already exists; use --replace to overwrite.");
                }

                list[i] = claim;
                return WithRecordedClaim(Persist(loaded.Receipt with { Claims = list, EvidenceDigest = null }), claim.ClaimId);
            }
        }

        list.Add(claim);
        return WithRecordedClaim(Persist(loaded.Receipt with { Claims = list, EvidenceDigest = null }), claim.ClaimId);
    }

    public EvidenceCommandResult Check(string runId)
    {
        var loaded = Load(runId);
        return loaded.Ok ? Persist(loaded.Receipt!) : loaded;
    }

    private static EvidenceCommandResult WithRecordedClaim(EvidenceCommandResult result, string claimId)
    {
        if (!result.Ok || result.Receipt is null)
        {
            return result;
        }

        var recorded = result.Receipt.Claims.FirstOrDefault(c => c.ClaimId == claimId);
        return result with { RecordedClaim = recorded };
    }

    private EvidenceCommandResult Load(string runId)
    {
        if (!ContractIds.IsValid(runId))
        {
            return new EvidenceCommandResult(false, null, null, $"invalid run id '{runId}'.");
        }

        if (!receipts.Exists(runId))
        {
            return new EvidenceCommandResult(false, null, receipts.PathFor(runId), $"no receipt for run '{runId}'.");
        }

        try
        {
            return new EvidenceCommandResult(true, receipts.Read(runId), receipts.PathFor(runId), null);
        }
        catch (Exception ex) when (ex is InvalidDataException or System.Text.Json.JsonException)
        {
            return new EvidenceCommandResult(false, null, receipts.PathFor(runId), "receipt is invalid: " + ex.Message);
        }
    }

    private EvidenceCommandResult Persist(RunReceipt receipt)
    {
        RunContract? contract = null;
        try
        {
            if (contracts.Exists(receipt.ContractId))
            {
                contract = RunContractJson.Deserialize(contracts.ReadRaw(receipt.ContractId));
            }
        }
        catch (Exception ex) when (ex is ContractFormatException or System.Text.Json.JsonException)
        {
            contract = null;
        }

        var (claims, claimFindings) = ClaimVerifier.Evaluate(receipt, contract);
        var kept = (receipt.Findings ?? []).Where(f => !f.RuleId.StartsWith("AWK", StringComparison.Ordinal)).ToList();
        var merged = receipt with { Claims = claims, Findings = kept.Concat(claimFindings).ToArray(), EvidenceDigest = null };
        var updated = EvidenceGate.Apply(merged, contract);
        try
        {
            receipts.Replace(updated);
        }
        catch (IOException ex)
        {
            return new EvidenceCommandResult(false, updated, receipts.PathFor(updated.RunId), "could not persist receipt: " + ex.Message);
        }

        if (projections is not null)
        {
            try
            {
                projections.Write(workspaceRoot, updated);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return new EvidenceCommandResult(true, updated, receipts.PathFor(updated.RunId), "warning: projection failed (receipt updated): " + ex.Message);
            }
        }

        return new EvidenceCommandResult(true, updated, receipts.PathFor(updated.RunId), null);
    }

    internal static string NextId(IReadOnlyList<AgentClaim> existing)
    {
        var used = existing.Select(c => c.ClaimId).ToHashSet(StringComparer.Ordinal);
        for (var n = 1; n < int.MaxValue; n++)
        {
            var id = "c" + n;
            if (!used.Contains(id))
            {
                return id;
            }
        }

        throw new InvalidOperationException("could not allocate a claim id.");
    }
}
