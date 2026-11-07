using AgentsWatch.Core.Runs;
using AgentsWatch.Core.Contracts;
using AgentsWatch.Git;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

internal static class FinishCommand
{
    public static async Task<int> RunAsync(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length != 1 || args[0].StartsWith("--"))
        {
            e.WriteLine("Usage: agentswatch finish <run-id>");
            return 2;
        }

        var git = new GitRepositoryStateReader(new GitCommandRunner());
        var service = new RunFinishService(new FileActiveRunStore(root), new FileRunRecordStore(root), git, git,
            receipts: new FileRunReceiptStore(root), contracts: new FileRunContractStore(root), projections: new FileReceiptProjectionWriter());
        var result = await service.FinishAsync(root, args[0]);
        if (!result.Finished)
        {
            e.WriteLine("error: " + SafeText(result.Error));
            return 1;
        }

        // Render from the already-redacted receipt. Do not print result.Record (raw) or recompute the digest.
        var receipt = result.Receipt!;
        var d = receipt.RunDelta;
        var t = receipt.HeadTransition;
        o.WriteLine($"Run finished: {receipt.RunId}");
        o.WriteLine($"Receipt: {DisplayArtifactPath(root, result.ReceiptPath)}");
        foreach (var p in result.ProjectionPaths ?? [])
        {
            o.WriteLine($"Projection: {DisplayArtifactPath(root, p)}");
        }

        if (result.ProjectionError is not null)
        {
            e.WriteLine("warning: " + SafeText(result.ProjectionError));
        }

        o.WriteLine($"Decision: {receipt.Decision.Status}");
        o.WriteLine($"HEAD: {t.Kind}{(t.BranchChanged ? $" (branch {SafeText(t.FromBranch)} -> {SafeText(t.ToBranch)})" : string.Empty)}");
        o.WriteLine($"Run-interval changes: {d.RunIntervalChanges.Count}");
        o.WriteLine($"Pre-existing unchanged: {d.PreExistingUnchangedChanges.Count}");
        o.WriteLine($"Pre-existing changed further: {d.PreExistingChangedFurther.Count}");
        o.WriteLine($"Ambiguous: {d.AttributionAmbiguities.Count}");
        foreach (var c in d.RunIntervalChanges.Concat(d.PreExistingChangedFurther).Concat(d.AttributionAmbiguities))
        {
            var from = c.OldPath is null ? "" : $" (from {SafeText(c.OldPath)})";
            o.WriteLine($"- [{c.Classification}] {c.Status} {SafeText(c.Path)}{from}: {SafeText(c.ClassificationReason)}");
        }

        o.WriteLine("Note: interval changes are observations, not proof of executor authorship.");
        return 0;
    }

    private static string SafeText(string? value) => ReceiptRedaction.RedactText(value) ?? string.Empty;

    private static string DisplayArtifactPath(string root, string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        var display = path;
        try
        {
            var fullRoot = Path.GetFullPath(root);
            var fullPath = Path.GetFullPath(path);
            var prefix = fullRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                         + Path.DirectorySeparatorChar;
            if (fullPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                || string.Equals(fullPath, fullRoot, StringComparison.OrdinalIgnoreCase))
            {
                display = Path.GetRelativePath(fullRoot, fullPath);
            }
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
        {
            display = path;
        }

        display = display.Replace('\\', '/');
        if (display.StartsWith("./", StringComparison.Ordinal))
        {
            display = display[2..];
        }

        return SafeText(display);
    }
}
