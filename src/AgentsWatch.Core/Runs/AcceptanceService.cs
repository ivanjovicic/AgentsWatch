using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Core.Runs;

/// <summary>Marks acceptance criteria only when the requested status is linked to recorded validation evidence (Supported cannot be evidence-free).</summary>
public sealed class AcceptanceService(
    string workspaceRoot,
    IRunReceiptStore receipts,
    IRunContractStore contracts,
    IReceiptProjectionWriter? projections = null)
{
    public EvidenceCommandResult Record(string runId, string criterionId, CriterionStatus status, IReadOnlyList<string> validationIds, string? reason, bool replace = false)
    {
        var loaded = Load(runId);
        if (!loaded.Ok)
        {
            return loaded;
        }

        var receipt = loaded.Receipt!;
        var criteria = (receipt.AcceptanceCriteria ?? []).ToList();
        var index = criteria.FindIndex(c => string.Equals(c.CriterionId, criterionId, StringComparison.Ordinal));
        if (index < 0)
        {
            return new EvidenceCommandResult(false, receipt, loaded.Path,
                $"acceptance criterion '{criterionId}' is not on this receipt.");
        }

        var current = criteria[index];
        if (current.Status != CriterionStatus.Unknown && current.Status != status && !replace)
        {
            return new EvidenceCommandResult(false, receipt, loaded.Path,
                $"acceptance criterion '{criterionId}' is already {current.Status}; use --replace to overwrite.");
        }

        var ids = NormalizeIds(validationIds);
        var check = ValidateLinkage(receipt, status, ids);
        if (check is not null)
        {
            return new EvidenceCommandResult(false, receipt, loaded.Path, check);
        }

        var refs = ids.Select(id => $"validations[{id}]").ToArray();
        var storedReason = string.IsNullOrWhiteSpace(reason)
            ? DefaultReason(status, ids)
            : reason.Trim();
        criteria[index] = current with { Status = status, EvidenceRefs = refs, Reason = storedReason };
        return Persist(receipt with { AcceptanceCriteria = criteria, EvidenceDigest = null });
    }

    internal static string? ValidateLinkage(RunReceipt receipt, CriterionStatus status, IReadOnlyList<string> validationIds)
    {
        if (status == CriterionStatus.Supported && validationIds.Count == 0)
        {
            return "Supported acceptance requires --evidence <validation-id> linking to a recorded passing validation; evidence-free Supported is rejected.";
        }

        if (status == CriterionStatus.Unsupported && validationIds.Count == 0)
        {
            return "NotSupported acceptance requires --evidence <validation-id> linking to a recorded validation.";
        }

        var validations = receipt.Validations ?? [];
        foreach (var id in validationIds)
        {
            var match = validations.FirstOrDefault(v => string.Equals(v.ValidationId, id, StringComparison.Ordinal));
            if (match is null)
            {
                return $"validation '{id}' is not recorded on this receipt.";
            }

            if (!string.Equals(match.RunId, receipt.RunId, StringComparison.Ordinal))
            {
                return $"validation '{id}' belongs to run '{match.RunId}', not '{receipt.RunId}'.";
            }

            if (status == CriterionStatus.Supported)
            {
                if (match.Status != ValidationStatus.Pass || match.ExitCode is not null and not 0)
                {
                    return $"validation '{id}' cannot support acceptance (status {match.Status}, exit {match.ExitCode?.ToString() ?? "none"}); Supported requires a passing validation with exit 0 or no exit code.";
                }
            }
        }

        return null;
    }

    internal static IReadOnlyList<string> NormalizeIds(IReadOnlyList<string>? validationIds) =>
        (validationIds ?? [])
            .SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(v => v.StartsWith("validations[", StringComparison.Ordinal) && v.EndsWith(']')
                ? v["validations[".Length..^1]
                : v)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

    private static string DefaultReason(CriterionStatus status, IReadOnlyList<string> ids) => status switch
    {
        CriterionStatus.Supported => "supported by recorded validation " + string.Join(", ", ids),
        CriterionStatus.Unsupported => "marked unsupported using recorded validation " + string.Join(", ", ids),
        _ => ids.Count == 0
            ? "reset to unknown"
            : "marked unknown; referenced validation " + string.Join(", ", ids)
    };

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
            var receipt = receipts.Read(runId);
            RunReceiptJson.EnsureStoredDigest(receipt);
            return new EvidenceCommandResult(true, receipt, receipts.PathFor(runId), null);
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

        var updated = EvidenceGate.Apply(receipt, contract);
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
}
