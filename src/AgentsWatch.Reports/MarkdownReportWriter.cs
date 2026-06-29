using System.Text;
using AgentsWatch.Core;

namespace AgentsWatch.Reports;

public sealed class MarkdownReportWriter
{
    public string Write(RunReport report)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"# AgentsWatch Run Report — {report.TaskId}");
        builder.AppendLine();
        builder.AppendLine($"Started: {report.StartedAt:O}");
        builder.AppendLine($"Finished: {report.FinishedAt:O}");
        builder.AppendLine($"Risk: {report.Risk}");
        builder.AppendLine();

        AppendSnapshot(builder, "Start", report.StartSnapshot);
        AppendSnapshot(builder, "End", report.EndSnapshot);

        AppendList(builder, "Validation", report.Validation);
        AppendList(builder, "Missed", report.Missed);
        AppendList(builder, "Follow-up", report.FollowUps);

        return builder.ToString();
    }

    private static void AppendSnapshot(StringBuilder builder, string title, GitSnapshot? snapshot)
    {
        builder.AppendLine($"## {title} snapshot");
        builder.AppendLine();

        if (snapshot is null)
        {
            builder.AppendLine("Not recorded.");
            builder.AppendLine();
            return;
        }

        builder.AppendLine($"Branch: `{snapshot.Branch}`");
        builder.AppendLine($"Commit: `{snapshot.CommitSha}`");
        builder.AppendLine();
        builder.AppendLine("Changed files:");

        if (snapshot.ChangedFiles.Count == 0)
        {
            builder.AppendLine("- none");
        }
        else
        {
            foreach (var file in snapshot.ChangedFiles)
            {
                builder.AppendLine($"- `{file.Status}` {file.Path}");
            }
        }

        builder.AppendLine();
    }

    private static void AppendList(StringBuilder builder, string title, IReadOnlyList<string> values)
    {
        builder.AppendLine($"## {title}");
        builder.AppendLine();

        if (values.Count == 0)
        {
            builder.AppendLine("- none");
        }
        else
        {
            foreach (var value in values)
            {
                builder.AppendLine($"- {value}");
            }
        }

        builder.AppendLine();
    }
}

public sealed class HandoffSummaryWriter
{
    public string Write(RunReport report)
    {
        var changed = report.EndSnapshot?.ChangedFiles.Select(file => file.Path).Distinct().ToArray() ?? [];

        return $"""
Task: {report.TaskId}
Status: {(report.Missed.Count == 0 ? "completed" : "needs follow-up")}
Relevant files: {string.Join(", ", changed.Take(10))}
Files changed: {changed.Length}
Validation run: {string.Join("; ", report.Validation.DefaultIfEmpty("none"))}
Validation blocked: none recorded
Do not inspect next: unrelated features unless follow-up requires it
Next minimal prompt: {string.Join("; ", report.FollowUps.DefaultIfEmpty("none"))}
Residual risk: {string.Join("; ", report.Missed.DefaultIfEmpty("none"))}
""";
    }
}
