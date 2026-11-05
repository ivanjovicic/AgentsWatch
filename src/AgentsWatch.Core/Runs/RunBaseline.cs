using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Core.Runs;

/// <summary>One changed path in one worktree area. Fingerprint is a content/blob hash, never content.</summary>
public sealed record WorktreeEntry(string Path, string Status, string Fingerprint, string? OldPath = null);

/// <summary>Lossless staged/unstaged/untracked state from `git status --porcelain=v1 -z -uall`.</summary>
public sealed record WorktreeState(
    string PorcelainVersion,
    IReadOnlyList<WorktreeEntry> Staged,
    IReadOnlyList<WorktreeEntry> Unstaged,
    IReadOnlyList<WorktreeEntry> Untracked,
    string StateFingerprint)
{
    public const string PorcelainV1Z = "v1-z";

    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsDirty => Staged.Count + Unstaged.Count + Untracked.Count > 0;

    public static WorktreeState Create(IReadOnlyList<WorktreeEntry> staged, IReadOnlyList<WorktreeEntry> unstaged, IReadOnlyList<WorktreeEntry> untracked)
    {
        static IReadOnlyList<WorktreeEntry> Sort(IEnumerable<WorktreeEntry> e) => e.OrderBy(x => x.Path, StringComparer.Ordinal).ToArray();
        var s = Sort(staged);
        var u = Sort(unstaged);
        var n = Sort(untracked);
        var canonical = new StringBuilder();
        foreach (var (area, list) in new[] { ("S", s), ("U", u), ("N", n) })
        {
            foreach (var e in list)
            {
                canonical.Append(area).Append('\0').Append(e.Status).Append('\0').Append(e.Path).Append('\0')
                    .Append(e.OldPath).Append('\0').Append(e.Fingerprint).Append('\n');
            }
        }

        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical.ToString()))).ToLowerInvariant();
        return new WorktreeState(PorcelainV1Z, s, u, n, "sha256:" + hash);
    }
}

public sealed record ExecutorMetadata(string? Agent = null, string? Tool = null, string? Model = null);

public sealed record RepositoryState(string Branch, string? HeadCommitSha, WorktreeState Worktree);

/// <summary>RunBaseline v1, persisted at .agentwatch/active-runs/&lt;run-id&gt;.json by `start`.</summary>
public sealed record RunBaseline(
    int SchemaVersion,
    string RunId,
    string ContractId,
    string TaskId,
    DateTimeOffset StartedAtUtc,
    string Branch,
    string? HeadCommitSha,
    WorktreeState WorktreeState,
    ExecutorMetadata? ExecutorMetadata)
{
    public const int CurrentSchemaVersion = 1;
}

/// <summary>Port: reads branch/HEAD/worktree state. Implemented by the Git adapter.</summary>
public interface IRepositoryStateReader
{
    Task<RepositoryState> ReadAsync(string workspaceRoot, CancellationToken cancellationToken = default);
}

public interface IActiveRunStore
{
    IReadOnlyList<string> ActiveRunIds();
    string PathFor(string runId);
    void WriteNew(RunBaseline baseline);
    RunBaseline Read(string runId);
    void Remove(string runId);
}

public static class RunBaselineJson
{
    public static string Serialize(RunBaseline baseline) => JsonSerializer.Serialize(baseline, RunContractJson.Options);

    public static RunBaseline Deserialize(string json)
    {
        var baseline = JsonSerializer.Deserialize<RunBaseline>(json, RunContractJson.Options)
            ?? throw new InvalidDataException("baseline JSON is null.");
        if (baseline.SchemaVersion != RunBaseline.CurrentSchemaVersion)
        {
            throw new InvalidDataException($"baseline schemaVersion {baseline.SchemaVersion} is not supported.");
        }

        return baseline;
    }
}

/// <summary>File store; writes temp file then atomic rename so a partial baseline is never a valid *.json.</summary>
public sealed class FileActiveRunStore(string workspaceRoot) : IActiveRunStore
{
    private string Dir => Path.Combine(workspaceRoot, ".agentwatch", "active-runs");

    public IReadOnlyList<string> ActiveRunIds() => Directory.Exists(Dir)
        ? Directory.GetFiles(Dir, "*.json").Select(p => Path.GetFileNameWithoutExtension(p)!).OrderBy(x => x, StringComparer.Ordinal).ToArray()
        : [];

    public string PathFor(string runId)
    {
        if (!ContractIds.IsValid(runId))
        {
            throw new ArgumentException($"Invalid run id '{runId}'.", nameof(runId));
        }

        return Path.Combine(Dir, runId + ".json");
    }

    public void WriteNew(RunBaseline baseline)
    {
        var path = PathFor(baseline.RunId);
        Directory.CreateDirectory(Dir);
        var temp = Path.Combine(Dir, $".{baseline.RunId}.{Guid.NewGuid():N}.tmp");
        File.WriteAllText(temp, RunBaselineJson.Serialize(baseline) + "\n");
        try
        {
            File.Move(temp, path, overwrite: false);
        }
        finally
        {
            if (File.Exists(temp))
            {
                File.Delete(temp);
            }
        }
    }

    public RunBaseline Read(string runId) => RunBaselineJson.Deserialize(File.ReadAllText(PathFor(runId)));

    public void Remove(string runId) => File.Delete(PathFor(runId));
}

public sealed record RunStartResult(bool Started, RunBaseline? Baseline, string? Path, string? Error, ContractCheckResult? ContractCheck = null);

/// <summary>Application use case for `agentswatch start`.</summary>
public sealed class RunStartService(
    IRunContractStore contracts,
    IActiveRunStore activeRuns,
    IRepositoryStateReader repository,
    TimeProvider? clock = null)
{
    private readonly TimeProvider _clock = clock ?? TimeProvider.System;

    public async Task<RunStartResult> StartAsync(string workspaceRoot, string contractId, ExecutorMetadata? executor = null, CancellationToken cancellationToken = default)
    {
        var check = new ContractService(contracts).CheckById(contractId);
        if (!check.IsValid)
        {
            return new RunStartResult(false, null, null, $"contract '{contractId}' is missing or invalid; run `agentswatch contract check {contractId}`.", check);
        }

        var contract = RunContractJson.Deserialize(contracts.ReadRaw(contractId));
        var active = activeRuns.ActiveRunIds();
        if (active.Count > 0)
        {
            return new RunStartResult(false, null, null, $"an active run already exists ({string.Join(", ", active)}); finish it before starting another.", check);
        }

        var state = await repository.ReadAsync(workspaceRoot, cancellationToken);
        var now = _clock.GetUtcNow().ToUniversalTime();
        var runId = $"{contract.ContractId}-{now:yyyyMMdd't'HHmmss'z'}-{Convert.ToHexString(RandomNumberGenerator.GetBytes(3)).ToLowerInvariant()}";
        if (runId.Length > 128)
        {
            runId = runId[^128..].TrimStart('-', '.');
        }

        var executorOrNull = executor is { Agent: null, Tool: null, Model: null } ? null : executor;
        var baseline = new RunBaseline(RunBaseline.CurrentSchemaVersion, runId, contract.ContractId, contract.TaskId, now,
            state.Branch, state.HeadCommitSha, state.Worktree, executorOrNull);

        try
        {
            activeRuns.WriteNew(baseline);
        }
        catch (IOException ex)
        {
            return new RunStartResult(false, null, null, "could not persist baseline: " + ex.Message, check);
        }

        return new RunStartResult(true, baseline, activeRuns.PathFor(runId), null, check);
    }
}