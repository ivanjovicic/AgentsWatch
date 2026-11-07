using System.Text.Json;
using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Core.Runs;

public enum ChangeClassification
{
    RunIntervalChange,
    PreExistingUnchanged,
    PreExistingChangedFurther,
    Ambiguous
}

public enum HeadTransitionKind
{
    Unchanged,
    FastForward,
    UnbornToCommit,
    NonFastForward
}

/// <summary>A path changed by commits between start HEAD and end HEAD (no rename detection).</summary>
public sealed record CommittedChange(string Path, string Status);

public sealed record HeadTransition(
    HeadTransitionKind Kind,
    string? FromCommitSha,
    string? ToCommitSha,
    string FromBranch,
    string ToBranch,
    IReadOnlyList<CommittedChange> CommittedChanges)
{
    public bool BranchChanged => !string.Equals(FromBranch, ToBranch, StringComparison.Ordinal);
}

public sealed record ClassifiedChange(
    string Path,
    string Status,
    ChangeClassification Classification,
    string ClassificationReason,
    string? OldPath = null);

/// <summary>RunDelta v1. Interval observation only; never proof of agent authorship.</summary>
public sealed record RunDelta(
    IReadOnlyList<ClassifiedChange> RunIntervalChanges,
    IReadOnlyList<ClassifiedChange> PreExistingUnchangedChanges,
    IReadOnlyList<ClassifiedChange> PreExistingChangedFurther,
    IReadOnlyList<ClassifiedChange> AttributionAmbiguities,
    string EndBranch,
    string? EndHeadCommitSha);

public sealed record RunFinishRecord(
    int SchemaVersion,
    string RunId,
    string ContractId,
    string TaskId,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset FinishedAtUtc,
    RunBaseline StartBaseline,
    WorktreeState EndWorktreeState,
    HeadTransition HeadTransition,
    RunDelta Delta,
    string AttributionPolicy)
{
    public const int CurrentSchemaVersion = 1;

    public const string Policy =
        "Changes are classified by comparing start baseline fingerprints with end state. RunIntervalChange means observed during the recorded interval, " +
        "not proven authored by the selected executor: Git cannot distinguish concurrent writers (humans, other agents, tools). Unprovable cases are Ambiguous.";
}

/// <summary>Port for history questions needed when HEAD moves during a run.</summary>
public interface IRepositoryHistoryReader
{
    Task<HeadTransition> GetTransitionAsync(string workspaceRoot, string fromBranch, string? fromSha, string toBranch, string? toSha, CancellationToken cancellationToken = default);
}

/// <summary>Pure, deterministic interval-delta classifier.</summary>
public static class RunDeltaCalculator
{
    private const string NotProven = "authorship not proven";

    public static RunDelta Compute(RunBaseline start, RepositoryState end, HeadTransition transition)
    {
        var startMap = Index(start.WorktreeState);
        var endMap = Index(end.Worktree);
        var committed = transition.CommittedChanges.ToDictionary(c => c.Path, StringComparer.Ordinal);

        var renamedFromStartDirty = new Dictionary<string, string>(StringComparer.Ordinal); // old -> new
        foreach (var (path, entries) in endMap)
        {
            foreach (var e in entries)
            {
                if (e.OldPath is { } old && !startMap.ContainsKey(path) && startMap.ContainsKey(old))
                {
                    renamedFromStartDirty[old] = path;
                }
            }
        }

        var paths = startMap.Keys.Concat(endMap.Keys).Concat(committed.Keys).Distinct(StringComparer.Ordinal).OrderBy(p => p, StringComparer.Ordinal);
        var results = new List<ClassifiedChange>();
        foreach (var path in paths)
        {
            var startEntries = startMap.GetValueOrDefault(path) ?? [];
            var endEntries = endMap.GetValueOrDefault(path) ?? [];
            committed.TryGetValue(path, out var commit);
            var status = StatusOf(endEntries) ?? commit?.Status ?? StatusOf(startEntries) ?? "?";
            var oldPath = endEntries.Select(e => e.OldPath).FirstOrDefault(o => o is not null);

            ClassifiedChange C(ChangeClassification c, string reason) => new(path, status, c, reason, oldPath);

            if (transition.Kind == HeadTransitionKind.NonFastForward)
            {
                results.Add(C(ChangeClassification.Ambiguous,
                    $"HEAD moved non-fast-forward ({Short(transition.FromCommitSha)} -> {Short(transition.ToCommitSha)}); baseline comparison is unreliable"));
                continue;
            }

            var startDirty = startEntries.Count > 0;
            var endDirty = endEntries.Count > 0;

            if (!startDirty)
            {
                if (oldPath is not null && startMap.ContainsKey(oldPath))
                {
                    results.Add(C(ChangeClassification.Ambiguous, $"renamed from '{oldPath}', which was already dirty at start"));
                }
                else if (endDirty || commit is not null)
                {
                    var where = (endDirty, commit is not null) switch
                    {
                        (true, true) => "committed and still dirty",
                        (false, true) => $"committed during interval ({Short(transition.FromCommitSha)} -> {Short(transition.ToCommitSha)})",
                        _ => "worktree/index"
                    };
                    results.Add(C(ChangeClassification.RunIntervalChange, $"clean at start, changed during interval ({where}); {NotProven}"));
                }

                continue;
            }

            if (renamedFromStartDirty.TryGetValue(path, out var renamedTo))
            {
                results.Add(C(ChangeClassification.Ambiguous, $"pre-existing dirty path renamed to '{renamedTo}' during interval"));
            }
            else if (commit is not null)
            {
                results.Add(C(ChangeClassification.Ambiguous, "pre-existing dirty path was included in commit(s) made during interval; pre-existing vs interval content cannot be separated"));
            }
            else if (!endDirty)
            {
                results.Add(C(ChangeClassification.PreExistingChangedFurther, "dirty at start, clean at end (pre-existing change reverted/discarded during interval)"));
            }
            else
            {
                var changedAreas = ChangedAreas(startEntries, endEntries);
                results.Add(changedAreas.Count == 0
                    ? C(ChangeClassification.PreExistingUnchanged, "dirty at start; identical staged/unstaged/untracked fingerprints at end")
                    : C(ChangeClassification.PreExistingChangedFurther, $"dirty at start; {string.Join(", ", changedAreas)} changed during interval; {NotProven}"));
            }
        }

        IReadOnlyList<ClassifiedChange> Of(ChangeClassification c) => results.Where(r => r.Classification == c).ToArray();
        return new RunDelta(Of(ChangeClassification.RunIntervalChange), Of(ChangeClassification.PreExistingUnchanged),
            Of(ChangeClassification.PreExistingChangedFurther), Of(ChangeClassification.Ambiguous), end.Branch, end.HeadCommitSha);
    }

    private sealed record AreaEntry(string Area, WorktreeEntry Entry);

    private static Dictionary<string, List<WorktreeEntry>> Index(WorktreeState state) => Index2(state).ToDictionary(
        g => g.Key, g => g.Select(a => a.Entry with { Status = a.Area + ":" + a.Entry.Status }).ToList(), StringComparer.Ordinal);

    private static IEnumerable<IGrouping<string, AreaEntry>> Index2(WorktreeState s) =>
        s.Staged.Select(e => new AreaEntry("staged", e))
            .Concat(s.Unstaged.Select(e => new AreaEntry("unstaged", e)))
            .Concat(s.Untracked.Select(e => new AreaEntry("untracked", e)))
            .GroupBy(a => a.Entry.Path, StringComparer.Ordinal);

    private static string? StatusOf(List<WorktreeEntry> entries)
    {
        var staged = entries.FirstOrDefault(e => e.Status.StartsWith("staged:"));
        var other = entries.FirstOrDefault(e => !e.Status.StartsWith("staged:"));
        return (staged ?? other)?.Status.Split(':', 2)[1];
    }

    private static List<string> ChangedAreas(List<WorktreeEntry> start, List<WorktreeEntry> end)
    {
        var areas = new List<string>();
        foreach (var area in new[] { "staged", "unstaged", "untracked" })
        {
            var s = start.FirstOrDefault(e => e.Status.StartsWith(area + ":"));
            var e = end.FirstOrDefault(x => x.Status.StartsWith(area + ":"));
            if (s?.Status != e?.Status || s?.Fingerprint != e?.Fingerprint || s?.OldPath != e?.OldPath)
            {
                areas.Add(area);
            }
        }

        return areas;
    }

    private static string Short(string? sha) => sha is null ? "(unborn)" : sha.Length > 7 ? sha[..7] : sha;
}

public interface IRunRecordStore
{
    bool Exists(string runId);
    string PathFor(string runId);
    void WriteNew(RunFinishRecord record);
    RunFinishRecord Read(string runId);
    void Remove(string runId);
}

public static class RunFinishJson
{
    public static string Serialize(RunFinishRecord r) => JsonSerializer.Serialize(r, RunContractJson.Options);

    public static RunFinishRecord Deserialize(string json) =>
        JsonSerializer.Deserialize<RunFinishRecord>(json, RunContractJson.Options) ?? throw new InvalidDataException("run record JSON is null.");
}

/// <summary>.agentwatch/runs/&lt;run-id&gt;.delta.json (receipt in AW-VFY-006 builds on this).</summary>
public sealed class FileRunRecordStore(string workspaceRoot) : IRunRecordStore
{
    private string Dir => Path.Combine(workspaceRoot, ".agentwatch", "runs");

    public string PathFor(string runId)
    {
        if (!ContractIds.IsValid(runId))
        {
            throw new ArgumentException($"Invalid run id '{runId}'.", nameof(runId));
        }

        return Path.Combine(Dir, runId + ".delta.json");
    }

    public bool Exists(string runId) => File.Exists(PathFor(runId));

    public void WriteNew(RunFinishRecord record)
    {
        var path = PathFor(record.RunId);
        Directory.CreateDirectory(Dir);
        var temp = Path.Combine(Dir, $".{record.RunId}.{Guid.NewGuid():N}.tmp");
        File.WriteAllText(temp, ReceiptRedaction.RedactJsonDocument(RunFinishJson.Serialize(record)) + "\n");
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

    public RunFinishRecord Read(string runId) => RunFinishJson.Deserialize(File.ReadAllText(PathFor(runId)));

    public void Remove(string runId)
    {
        var path = PathFor(runId);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}

public sealed record RunFinishResult(
    bool Finished,
    RunFinishRecord? Record,
    string? Path,
    string? Error,
    RunReceipt? Receipt = null,
    string? ReceiptPath = null,
    IReadOnlyList<string>? ProjectionPaths = null,
    string? ProjectionError = null);

/// <summary>Application use case for `agentswatch finish`.</summary>
public sealed class RunFinishService(
    IActiveRunStore activeRuns,
    IRunRecordStore records,
    IRepositoryStateReader repository,
    IRepositoryHistoryReader history,
    TimeProvider? clock = null,
    IRunReceiptStore? receipts = null,
    IRunContractStore? contracts = null,
    IReceiptProjectionWriter? projections = null)
{
    private readonly TimeProvider _clock = clock ?? TimeProvider.System;

    public async Task<RunFinishResult> FinishAsync(string workspaceRoot, string runId, CancellationToken cancellationToken = default)
    {
        if (!ContractIds.IsValid(runId))
        {
            return new RunFinishResult(false, null, null, $"invalid run id '{runId}'.");
        }

        if (!activeRuns.ActiveRunIds().Contains(runId, StringComparer.Ordinal))
        {
            return new RunFinishResult(false, null, null, $"no active run '{runId}' (expected {activeRuns.PathFor(runId)}).");
        }

        if (receipts?.Exists(runId) == true)
        {
            return new RunFinishResult(false, null, receipts.PathFor(runId), $"receipt for '{runId}' already exists; refusing to overwrite.");
        }

        if (records.Exists(runId))
        {
            // Delta without a receipt is a partial previous attempt and is retryable.
            var orphanDelta = receipts is not null && !receipts.Exists(runId);
            if (!orphanDelta)
            {
                return new RunFinishResult(false, null, records.PathFor(runId), $"run record for '{runId}' already exists; refusing to overwrite.");
            }

            records.Remove(runId);
        }

        RunBaseline baseline;
        try
        {
            baseline = activeRuns.Read(runId);
        }
        catch (Exception ex) when (ex is InvalidDataException or JsonException)
        {
            return new RunFinishResult(false, null, null, $"active run '{runId}' baseline is invalid: {ex.Message}");
        }

        var end = await repository.ReadAsync(workspaceRoot, cancellationToken);
        var transition = await history.GetTransitionAsync(workspaceRoot, baseline.Branch, baseline.HeadCommitSha, end.Branch, end.HeadCommitSha, cancellationToken);
        var delta = RunDeltaCalculator.Compute(baseline, end, transition);
        var record = new RunFinishRecord(RunFinishRecord.CurrentSchemaVersion, baseline.RunId, baseline.ContractId, baseline.TaskId,
            baseline.StartedAtUtc, _clock.GetUtcNow().ToUniversalTime(), baseline, end.Worktree, transition, delta, RunFinishRecord.Policy);

        try
        {
            records.WriteNew(record);
        }
        catch (IOException ex)
        {
            return new RunFinishResult(false, null, null, "could not persist run record; active baseline kept: " + ex.Message);
        }

        RunReceipt? receipt = null;
        if (receipts is not null)
        {
            RunContract? contract = null;
            try
            {
                if (contracts?.Exists(record.ContractId) == true)
                {
                    contract = RunContractJson.Deserialize(contracts.ReadRaw(record.ContractId));
                }
            }
            catch (Exception ex) when (ex is ContractFormatException or JsonException)
            {
                contract = null;
            }

            receipt = RunReceiptBuilder.Build(record, contract);
            try
            {
                receipts.WriteNew(receipt);
            }
            catch (IOException ex)
            {
                try
                {
                    records.Remove(runId);
                }
                catch (IOException)
                {
                    // Retry can still resume: orphan delta + missing receipt is accepted above.
                }

                return new RunFinishResult(false, record, null, "could not persist receipt; active baseline kept and delta record removed so finish can be retried: " + ex.Message);
            }
        }

        activeRuns.Remove(runId); // only after canonical JSON is durably written

        IReadOnlyList<string> projectionPaths = [];
        string? projectionError = null;
        if (receipt is not null && projections is not null)
        {
            try
            {
                projectionPaths = projections.Write(workspaceRoot, receipt);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
            {
                projectionError = "Markdown projection failed (canonical receipt is intact): " + ex.Message;
            }
        }

        return new RunFinishResult(true, record, records.PathFor(runId), null, receipt,
            receipt is null ? null : receipts!.PathFor(runId), projectionPaths, projectionError);
    }
}