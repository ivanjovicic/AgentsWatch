namespace AgentsWatch.Core.Contracts;

public interface IRunContractStore
{
    bool Exists(string contractId);
    void Write(RunContract contract, bool overwrite);
    string ReadRaw(string contractId);
    string PathFor(string contractId);
}

/// <summary>Local file store rooted at a workspace: .agentwatch/contracts/&lt;id&gt;.json</summary>
public sealed class FileRunContractStore(string workspaceRoot) : IRunContractStore
{
    public string PathFor(string contractId) => Path.Combine(workspaceRoot, ContractIds.RelativePath(contractId));

    public bool Exists(string contractId) => File.Exists(PathFor(contractId));

    public void Write(RunContract contract, bool overwrite)
    {
        var path = PathFor(contract.ContractId);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var mode = overwrite ? FileMode.Create : FileMode.CreateNew;
        using var stream = new FileStream(path, mode, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream);
        writer.Write(RunContractJson.Serialize(contract));
        writer.Write('\n');
    }

    public string ReadRaw(string contractId) => File.ReadAllText(PathFor(contractId));
}

public sealed record ContractDraft(
    string TaskId,
    string? ContractId,
    string? Intent,
    IReadOnlyList<string> AcceptanceCriteria,
    IReadOnlyList<string> OwnedPaths,
    IReadOnlyList<string> AvoidPaths,
    PermissionMode PermissionMode,
    RunMode RunMode,
    IReadOnlyList<ValidationRequirement> Validations,
    IReadOnlyList<string> StopRules,
    IReadOnlyList<string> ExpectedEvidence,
    SourceTaskRef? SourceTaskRef,
    IReadOnlyList<CriterionValidationBinding>? CriterionValidationBindings = null);

public sealed record ContractCreateResult(bool Created, RunContract? Contract, string? Path, ContractCheckResult Check, string? Error);

/// <summary>Application use case for contract create/check; reusable from CLI/MCP/importers.</summary>
public sealed class ContractService(IRunContractStore store, TimeProvider? clock = null)
{
    private readonly TimeProvider _clock = clock ?? TimeProvider.System;

    public ContractCreateResult Create(ContractDraft draft, bool overwrite = false)
    {
        var id = string.IsNullOrWhiteSpace(draft.ContractId) ? ContractIds.FromTaskId(draft.TaskId ?? string.Empty) : draft.ContractId!;
        var contract = new RunContract(
            RunContract.CurrentSchemaVersion,
            id,
            draft.TaskId?.Trim() ?? string.Empty,
            draft.Intent?.Trim() ?? string.Empty,
            draft.AcceptanceCriteria,
            draft.OwnedPaths,
            draft.AvoidPaths,
            draft.PermissionMode,
            draft.RunMode,
            new ValidationContract(draft.Validations),
            draft.StopRules,
            draft.ExpectedEvidence,
            draft.SourceTaskRef,
            _clock.GetUtcNow().ToUniversalTime());
        if (draft.CriterionValidationBindings is { Count: > 0 } bindings)
        {
            contract = contract with { CriterionValidationBindings = bindings };
        }

        var check = RunContractLinter.Check(contract);
        if (!ContractIds.IsValid(id))
        {
            return new ContractCreateResult(false, contract, null, check, $"{ContractRules.InvalidContractId}: cannot store contract with id '{id}'.");
        }

        if (!overwrite && store.Exists(id))
        {
            return new ContractCreateResult(false, contract, store.PathFor(id), check, $"contract '{id}' already exists; refusing to overwrite (use --force).");
        }

        try
        {
            store.Write(contract, overwrite);
        }
        catch (IOException) when (!overwrite)
        {
            return new ContractCreateResult(false, contract, store.PathFor(id), check, $"contract '{id}' already exists; refusing to overwrite (use --force).");
        }

        return new ContractCreateResult(true, contract, store.PathFor(id), check, null);
    }

    public ContractCheckResult CheckJson(string json)
    {
        try
        {
            return RunContractLinter.Check(RunContractJson.Deserialize(json));
        }
        catch (ContractFormatException ex)
        {
            return new ContractCheckResult([new ContractFinding(ex.RuleId, FindingSeverity.Error, ex.Reason)]);
        }
        catch (System.Text.Json.JsonException ex)
        {
            return new ContractCheckResult([new ContractFinding(ContractRules.ParseError, FindingSeverity.Error, ex.Message)]);
        }
    }

    public ContractCheckResult CheckById(string contractId)
    {
        if (!ContractIds.IsValid(contractId))
        {
            return new ContractCheckResult([new ContractFinding(ContractRules.InvalidContractId, FindingSeverity.Error, $"invalid contract id '{contractId}'.")]);
        }

        if (!store.Exists(contractId))
        {
            return new ContractCheckResult([new ContractFinding(ContractRules.ParseError, FindingSeverity.Error, $"contract '{contractId}' not found at {store.PathFor(contractId)}.")]);
        }

        return CheckJson(store.ReadRaw(contractId));
    }
}