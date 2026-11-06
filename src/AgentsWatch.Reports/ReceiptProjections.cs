using System.Text;
using AgentsWatch.Core.Runs;

namespace AgentsWatch.Reports;

/// <summary>Generated evidence projection of a RunReceipt. Not authoritative; regenerate from JSON.</summary>
public static class ReceiptMarkdownWriter
{
    public static string Write(RunReceipt r)
    {
        var b = new StringBuilder();
        b.AppendLine($"# AgentsWatch Run Receipt - {r.RunId}");
        b.AppendLine();
        b.AppendLine("> Generated from `.agentwatch/runs/" + r.RunId + ".json` (canonical). Do not edit; edits here are not evidence.");
        b.AppendLine();
        b.AppendLine($"- Task: `{r.TaskId}`  Contract: `{r.ContractId}`");
        b.AppendLine($"- Started: {r.StartedAtUtc:O}  Finished: {r.FinishedAtUtc:O}");
        b.AppendLine($"- Executor: {Executor(r.ExecutorMetadata)}");
        b.AppendLine($"- Start: `{r.StartRepositoryState.Branch}` @ `{Short(r.StartRepositoryState.HeadCommitSha)}`; End: `{r.EndRepositoryState.Branch}` @ `{Short(r.EndRepositoryState.HeadCommitSha)}` ({r.HeadTransition.Kind})");
        b.AppendLine($"- Evidence digest: `{r.EvidenceDigest ?? "none"}`");
        b.AppendLine();
        b.AppendLine($"## Decision: {r.Decision.Status}");
        b.AppendLine();
        List(b, r.Decision.Reasons);
        b.AppendLine("## Run-interval changes (observed during interval; authorship not proven)");
        b.AppendLine();
        Changes(b, r.RunDelta.RunIntervalChanges);
        b.AppendLine("## Pre-existing changed further");
        b.AppendLine();
        Changes(b, r.RunDelta.PreExistingChangedFurther);
        b.AppendLine("## Pre-existing unchanged (not attributed to this run)");
        b.AppendLine();
        Changes(b, r.RunDelta.PreExistingUnchangedChanges);
        b.AppendLine("## Ambiguous");
        b.AppendLine();
        Changes(b, r.RunDelta.AttributionAmbiguities);
        b.AppendLine("## Validations");
        b.AppendLine();
        List(b, r.Validations.Select(v => $"`{v.CommandDisplay}`: {v.Status} (source: {v.Source}{(v.ExitCode is { } x ? $", exit {x}" : "")})").ToArray(),
            "none recorded - no validation result is claimed");
        b.AppendLine("## Claims");
        b.AppendLine();
        List(b, r.Claims.Select(c => $"{c.ClaimType} = {c.Value} [{c.Status?.ToString() ?? "pending"}] (source: {c.Source}){(c.Reason is null ? "" : " - " + c.Reason)}").ToArray(), "none recorded");
        b.AppendLine("## Acceptance criteria");
        b.AppendLine();
        List(b, r.AcceptanceCriteria.Select(a => $"{a.CriterionId} [{a.Status}] {a.Text} - {a.Reason}").ToArray());
        b.AppendLine("## Findings");
        b.AppendLine();
        List(b, r.Findings.Select(f => $"{f.RuleId} {f.Category} [{f.Severity}/{f.Status}] {f.Message}{(f.Paths.Count > 0 ? " (" + string.Join(", ", f.Paths) + ")" : "")}").ToArray());
        b.AppendLine("## Missed work");
        b.AppendLine();
        List(b, r.MissedWork);
        b.AppendLine("## Attribution policy");
        b.AppendLine();
        b.AppendLine(r.AttributionPolicy);
        return b.ToString();
    }

    internal static string Executor(ExecutorMetadata? m) => m is null
        ? "unknown"
        : $"agent={m.Agent ?? "unknown"}, tool={m.Tool ?? "unknown"}, model={m.Model ?? "unknown"}";

    internal static string Short(string? sha) => sha is null ? "(unborn)" : sha.Length > 7 ? sha[..7] : sha;

    private static void Changes(StringBuilder b, IReadOnlyList<ClassifiedChange> changes) =>
        List(b, changes.Select(c => $"`{c.Status}` {c.Path}{(c.OldPath is null ? "" : $" (from {c.OldPath})")} - {c.ClassificationReason}").ToArray());

    internal static void List(StringBuilder b, IReadOnlyList<string> items, string empty = "none")
    {
        if (items.Count == 0)
        {
            b.AppendLine("- " + empty);
        }

        foreach (var i in items)
        {
            b.AppendLine("- " + i);
        }

        b.AppendLine();
    }
}

/// <summary>Compact continuation handoff. Next-step text here is non-canonical and never affects the decision.</summary>
public static class ReceiptHandoffWriter
{
    public static string Write(RunReceipt r, string? nextStepNote = null)
    {
        var b = new StringBuilder();
        b.AppendLine($"# Handoff - {r.TaskId} ({r.RunId})");
        b.AppendLine();
        b.AppendLine($"- Decision: {r.Decision.Status} (from canonical receipt)");
        b.AppendLine($"- Executor: {ReceiptMarkdownWriter.Executor(r.ExecutorMetadata)}");
        b.AppendLine($"- Run-interval files: {Join(r.RunDelta.RunIntervalChanges.Select(c => c.Path))}");
        b.AppendLine($"- Pre-existing changed further: {Join(r.RunDelta.PreExistingChangedFurther.Select(c => c.Path))}");
        b.AppendLine($"- Ambiguous: {Join(r.RunDelta.AttributionAmbiguities.Select(c => c.Path))}");
        b.AppendLine($"- Validation: {(r.Validations.Count == 0 ? "none recorded" : Join(r.Validations.Select(v => $"{v.CommandDisplay}={v.Status}")))}");
        b.AppendLine($"- Open findings: {Join(r.Findings.Where(f => f.Status != FindingStatus.Supported).Select(f => $"{f.RuleId} {f.Category}"))}");
        b.AppendLine($"- Residual risk: {Join(r.Decision.Reasons.Concat(r.MissedWork))}");
        b.AppendLine();
        b.AppendLine("## Next step (non-canonical)");
        b.AppendLine();
        b.AppendLine(string.IsNullOrWhiteSpace(nextStepNote) ? "- none provided" : "- " + nextStepNote.Trim());
        return b.ToString();
    }

    private static string Join(IEnumerable<string> items)
    {
        var list = items.ToArray();
        return list.Length == 0 ? "none" : string.Join("; ", list);
    }
}

/// <summary>Writes .ai/runs/&lt;run-id&gt;.md and .ai/handoffs/&lt;run-id&gt;.md from the receipt.</summary>
public sealed class FileReceiptProjectionWriter : IReceiptProjectionWriter
{
    public IReadOnlyList<string> Write(string workspaceRoot, RunReceipt receipt)
    {
        var report = Path.Combine(workspaceRoot, ".ai", "runs", receipt.RunId + ".md");
        var handoff = Path.Combine(workspaceRoot, ".ai", "handoffs", receipt.RunId + ".md");
        WriteAtomic(report, ReceiptMarkdownWriter.Write(receipt));
        WriteAtomic(handoff, ReceiptHandoffWriter.Write(receipt));
        return [report, handoff];
    }

    private static void WriteAtomic(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temp = path + ".tmp";
        File.WriteAllText(temp, content);
        File.Move(temp, path, overwrite: true);
    }
}