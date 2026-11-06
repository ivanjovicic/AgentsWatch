using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Core.Runs;

public static class EvidenceRules
{
    public const string ReceiptIncomplete = "AWE001";
    public const string MandatoryMissing = "AWE002";
    public const string ValidationFailed = "AWE003";
    public const string ValidationBlocked = "AWE004";
    public const string ExpectedEvidenceMissing = "AWE006";
    public const string AcceptanceUnsupported = "AWE007";
    public const string ApprovalRequired = "AWE008";
    public const string Satisfied = "AWE000";
}

/// <summary>Deterministic Evidence Gate: contract + receipt -> findings and decision. Never synthesizes Pass from prose.</summary>
public static class EvidenceGate
{
    public static RunReceipt Apply(RunReceipt receipt, RunContract? contract)
    {
        var findings = (receipt.Findings ?? []).Where(f => !f.RuleId.StartsWith("AWE", StringComparison.Ordinal)).ToList();
        var n = 0;
        void Add(string rule, FindingCategory category, FindingSeverity severity, FindingStatus status, string message, IReadOnlyList<string> paths, IReadOnlyList<string> evidenceRefs) =>
            findings.Add(new Finding($"e{++n}", rule, category, severity, status, message, paths, evidenceRefs, receipt.RunId, receipt.ContractId));

        if (receipt.SchemaVersion != RunReceipt.CurrentSchemaVersion
            || string.IsNullOrWhiteSpace(receipt.RunId)
            || string.IsNullOrWhiteSpace(receipt.ContractId)
            || string.IsNullOrWhiteSpace(receipt.TaskId)
            || receipt.RunDelta is null
            || receipt.StartRepositoryState is null
            || receipt.EndRepositoryState is null)
        {
            Add(EvidenceRules.ReceiptIncomplete, FindingCategory.ContractIncomplete, FindingSeverity.Error, FindingStatus.Unknown,
                "receipt is missing required identity or repository/delta fields.", [], ["receipt"]);
            return Finalize(receipt, findings, DecisionStatus.NeedsEvidence, [$"{EvidenceRules.ReceiptIncomplete}: required receipt fields are missing or unsupported [receipt]"]);
        }

        var validations = receipt.Validations ?? [];
        foreach (var v in validations)
        {
            var pref = $"validations[{v.ValidationId}]";
            switch (v.Status)
            {
                case ValidationStatus.Fail:
                case ValidationStatus.TimedOut:
                case ValidationStatus.Killed:
                    Add(EvidenceRules.ValidationFailed, FindingCategory.ValidationFailed, FindingSeverity.Error, FindingStatus.Unsupported,
                        $"validation '{v.ValidationId}' ({v.CommandDisplay}) {v.Status} (source: {v.Source}).", [], [pref]);
                    break;
                case ValidationStatus.BlockedByEnvironment:
                    Add(EvidenceRules.ValidationBlocked, FindingCategory.MissingValidation, FindingSeverity.Error, FindingStatus.Unknown,
                        $"validation '{v.ValidationId}' ({v.CommandDisplay}) is blocked by environment (source: {v.Source}); never treated as Pass.", [], [pref]);
                    break;
            }
        }

        if (contract is null)
        {
            Add(EvidenceRules.ReceiptIncomplete, FindingCategory.ContractIncomplete, FindingSeverity.Error, FindingStatus.Unknown,
                $"contract '{receipt.ContractId}' could not be loaded for evidence check.", [], ["contract"]);
        }
        else
        {
            foreach (var req in (contract.ValidationContract?.Requirements ?? []).Where(r => r.Mandatory))
            {
                var matches = validations.Where(v =>
                    string.Equals(v.ValidationId, req.Id, StringComparison.Ordinal)
                    || string.Equals(v.CommandDisplay, req.Command, StringComparison.Ordinal)).ToList();
                var pref = $"contract.validationContract.requirements[{req.Id}]";
                if (matches.Count == 0)
                {
                    Add(EvidenceRules.MandatoryMissing, FindingCategory.MissingValidation, FindingSeverity.Error, FindingStatus.Unsupported,
                        $"mandatory validation '{req.Id}' ({req.Command}) has no evidence; agent prose is not evidence.", [], [pref]);
                    continue;
                }

                var worst = matches.OrderByDescending(v => Rank(v.Status)).First();
                if (worst.Status is ValidationStatus.NotRun or ValidationStatus.Unknown)
                {
                    Add(EvidenceRules.MandatoryMissing, FindingCategory.MissingValidation, FindingSeverity.Error, FindingStatus.Unknown,
                        $"mandatory validation '{req.Id}' ({req.Command}) is {worst.Status} (source: {worst.Source}).", [], [$"validations[{worst.ValidationId}]", pref]);
                }
            }

            foreach (var item in (contract.ExpectedEvidence ?? []).Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                if (!validations.Any(v => BacksExpectedItem(v, item)))
                {
                    Add(EvidenceRules.ExpectedEvidenceMissing, FindingCategory.MissingValidation, FindingSeverity.Warning, FindingStatus.Unsupported,
                        $"expected evidence '{item}' is not backed by a passing validation whose outputSummary, evidenceDigest, sourceRef, command or id references it.", [], [$"contract.expectedEvidence[{item}]"]);
                }
            }

            if (contract.PermissionMode == PermissionMode.ApprovalRequired && receipt.Decision.Override is null)
            {
                Add(EvidenceRules.ApprovalRequired, FindingCategory.RiskApprovalRequired, FindingSeverity.Error, FindingStatus.NeedsReview,
                    "contract permissionMode is approval-required and no auditable override is recorded.", [], ["contract.permissionMode"]);
            }
        }

        foreach (var ac in receipt.AcceptanceCriteria ?? [])
        {
            if (ac.Status == CriterionStatus.Unknown)
            {
                Add(EvidenceRules.AcceptanceUnsupported, FindingCategory.AcceptanceCriterionUnsupported, FindingSeverity.Warning, FindingStatus.Unknown,
                    $"acceptance criterion '{ac.CriterionId}' is unknown and cannot support completion.", [], [$"acceptanceCriteria[{ac.CriterionId}]"]);
            }
            else if (ac.Status == CriterionStatus.Unsupported)
            {
                Add(EvidenceRules.AcceptanceUnsupported, FindingCategory.AcceptanceCriterionUnsupported, FindingSeverity.Error, FindingStatus.Unsupported,
                    $"acceptance criterion '{ac.CriterionId}' is unsupported.", [], [$"acceptanceCriteria[{ac.CriterionId}]"]);
            }
        }

        var failed = findings.Any(f => f.RuleId == EvidenceRules.ValidationFailed);
        var blocked = findings.Any(f => f.RuleId == EvidenceRules.ValidationBlocked);
        var approval = findings.Any(f => f.RuleId == EvidenceRules.ApprovalRequired);
        var unsupportedAc = findings.Any(f => f.RuleId == EvidenceRules.AcceptanceUnsupported && f.Status == FindingStatus.Unsupported);
        var needsEvidence = findings.Any(f => f.RuleId is EvidenceRules.ReceiptIncomplete or EvidenceRules.MandatoryMissing or EvidenceRules.ExpectedEvidenceMissing
            || (f.RuleId == EvidenceRules.AcceptanceUnsupported && f.Status == FindingStatus.Unknown));

        var (status, fallback) = (failed, blocked, approval, unsupportedAc, needsEvidence) switch
        {
            (true, _, _, _, _) => (DecisionStatus.Failed, $"{EvidenceRules.ValidationFailed}: one or more validations failed, timed out or were killed"),
            (_, true, _, _, _) => (DecisionStatus.Blocked, $"{EvidenceRules.ValidationBlocked}: validation blocked by environment"),
            (_, _, true, _, _) => (DecisionStatus.NeedsApproval, $"{EvidenceRules.ApprovalRequired}: approval required"),
            (_, _, _, true, _) => (DecisionStatus.NeedsReview, $"{EvidenceRules.AcceptanceUnsupported}: an acceptance criterion is unsupported"),
            (_, _, _, _, true) => (DecisionStatus.NeedsEvidence, $"{EvidenceRules.MandatoryMissing}: required evidence is missing or unknown"),
            _ => (DecisionStatus.Done, $"{EvidenceRules.Satisfied}: mandatory validations passed; no blocking evidence findings")
        };

        if (status == DecisionStatus.Done && findings.Any(f => f.RuleId is ScopeDriftRules.OutsideOwned or ScopeDriftRules.AvoidTouched or ScopeDriftRules.RenameCrosses or ClaimRules.Unsupported))
        {
            status = DecisionStatus.NeedsReview;
        }

        if (status == DecisionStatus.Done && findings.Any(f => f.RuleId == ClaimRules.Unknown))
        {
            status = DecisionStatus.NeedsEvidence;
        }

        var reasons = findings.Where(f => f.RuleId.StartsWith("AWE", StringComparison.Ordinal) || f.RuleId.StartsWith("AWD", StringComparison.Ordinal) || f.RuleId.StartsWith("AWK", StringComparison.Ordinal))
            .Select(f => $"{f.RuleId}: {f.Message} [{string.Join(", ", f.EvidenceRefs)}]").ToList();
        var ambiguous = findings.Count(f => f.Category == FindingCategory.AttributionAmbiguous);
        if (ambiguous > 0)
        {
            reasons.Add($"AWR002: {ambiguous} ambiguous change(s) need review [runDelta.attributionAmbiguities]");
            if (status == DecisionStatus.Done)
            {
                status = DecisionStatus.NeedsReview;
            }
        }

        if (reasons.Count == 0)
        {
            reasons.Add(fallback);
        }

        return Finalize(receipt, findings, status, reasons);
    }

    public static bool SatisfiesMandatory(RunReceipt receipt, RunContract contract)
    {
        return (contract.ValidationContract?.Requirements ?? []).Where(r => r.Mandatory).All(req =>
            (receipt.Validations ?? []).Any(v =>
                (string.Equals(v.ValidationId, req.Id, StringComparison.Ordinal) || string.Equals(v.CommandDisplay, req.Command, StringComparison.Ordinal))
                && v.Status == ValidationStatus.Pass));
    }

    private static bool BacksExpectedItem(ValidationEvidence v, string item) =>
        v.Status == ValidationStatus.Pass
        && new[] { v.OutputSummary, v.EvidenceDigest, v.SourceRef, v.CommandDisplay, v.ValidationId }
            .Any(field => field is not null && field.Contains(item, StringComparison.OrdinalIgnoreCase));

    private static int Rank(ValidationStatus s) => s switch
    {
        ValidationStatus.Fail or ValidationStatus.TimedOut or ValidationStatus.Killed => 4,
        ValidationStatus.BlockedByEnvironment => 3,
        ValidationStatus.NotRun or ValidationStatus.Unknown => 2,
        ValidationStatus.Pass => 1,
        _ => 0
    };

    private static RunReceipt Finalize(RunReceipt receipt, List<Finding> findings, DecisionStatus status, IReadOnlyList<string> reasons)
    {
        var updated = receipt with
        {
            Findings = findings,
            Decision = receipt.Decision with { Status = status, Reasons = reasons },
            EvidenceDigest = null
        };
        return RunReceiptJson.WithDigest(updated);
    }
}

public sealed record EvidenceCommandResult(bool Ok, RunReceipt? Receipt, string? Path, string? Error, ValidationEvidence? Recorded = null);

/// <summary>Application use case: import labeled validation evidence and evaluate the Evidence Gate.</summary>
public sealed class EvidenceService(
    string workspaceRoot,
    IRunReceiptStore receipts,
    IRunContractStore? contracts = null,
    IReceiptProjectionWriter? projections = null)
{
    public EvidenceCommandResult Record(string runId, ValidationEvidence evidence, bool replace = false)
    {
        var loaded = Load(runId);
        if (!loaded.Ok)
        {
            return loaded;
        }

        var list = loaded.Receipt!.Validations.ToList();
        if (string.IsNullOrWhiteSpace(evidence.ValidationId))
        {
            evidence = evidence with { ValidationId = NextValidationId(list) };
        }
        else
        {
            var i = list.FindIndex(v => v.ValidationId == evidence.ValidationId);
            if (i >= 0)
            {
                if (!replace)
                {
                    return new EvidenceCommandResult(false, loaded.Receipt, loaded.Path,
                        $"validation '{evidence.ValidationId}' already exists; use --replace to overwrite.");
                }

                list[i] = evidence;
                return Persist(loaded.Receipt with { Validations = list, EvidenceDigest = null }, evidence);
            }
        }

        list.Add(evidence);
        return Persist(loaded.Receipt with { Validations = list, EvidenceDigest = null }, evidence);
    }

    internal static string NextValidationId(IReadOnlyList<ValidationEvidence> existing)
    {
        var used = existing.Select(v => v.ValidationId).ToHashSet(StringComparer.Ordinal);
        for (var n = 1; n < int.MaxValue; n++)
        {
            var id = "v" + n;
            if (!used.Contains(id))
            {
                return id;
            }
        }

        throw new InvalidOperationException("could not allocate a validation id.");
    }

    public EvidenceCommandResult Check(string runId)
    {
        var loaded = Load(runId);
        return loaded.Ok ? Persist(loaded.Receipt) : loaded;
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

    private EvidenceCommandResult Persist(RunReceipt? receipt, ValidationEvidence? recorded = null)
    {
        if (receipt is null)
        {
            return new EvidenceCommandResult(false, null, null, "no receipt.");
        }

        RunContract? contract = null;
        try
        {
            if (contracts?.Exists(receipt.ContractId) == true)
            {
                contract = RunContractJson.Deserialize(contracts.ReadRaw(receipt.ContractId));
            }
        }
        catch (Exception ex) when (ex is ContractFormatException or System.Text.Json.JsonException)
        {
            contract = null;
        }

        var updated = EvidenceGate.Apply(receipt, contract);
        try
        {
            receipts.Replace(updated);
        }
        catch (IOException ex)
        {
            return new EvidenceCommandResult(false, updated, receipts.PathFor(updated.RunId), "could not persist receipt: " + ex.Message, recorded);
        }

        if (projections is not null)
        {
            try
            {
                projections.Write(workspaceRoot, updated);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return new EvidenceCommandResult(true, updated, receipts.PathFor(updated.RunId), "warning: projection failed (receipt updated): " + ex.Message, recorded);
            }
        }

        return new EvidenceCommandResult(true, updated, receipts.PathFor(updated.RunId), null, recorded);
    }
}