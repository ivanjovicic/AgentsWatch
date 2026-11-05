using AgentsWatch.Core.Runs;
using AgentsWatch.Git;

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
        var service = new RunFinishService(new FileActiveRunStore(root), new FileRunRecordStore(root), git, git);
        var result = await service.FinishAsync(root, args[0]);
        if (!result.Finished)
        {
            e.WriteLine("error: " + result.Error);
            return 1;
        }

        var r = result.Record!;
        var d = r.Delta;
        o.WriteLine($"Run finished: {r.RunId}");
        o.WriteLine($"Record: {result.Path}");
        o.WriteLine($"HEAD: {r.HeadTransition.Kind}{(r.HeadTransition.BranchChanged ? $" (branch {r.HeadTransition.FromBranch} -> {r.HeadTransition.ToBranch})" : string.Empty)}");
        o.WriteLine($"Run-interval changes: {d.RunIntervalChanges.Count}");
        o.WriteLine($"Pre-existing unchanged: {d.PreExistingUnchangedChanges.Count}");
        o.WriteLine($"Pre-existing changed further: {d.PreExistingChangedFurther.Count}");
        o.WriteLine($"Ambiguous: {d.AttributionAmbiguities.Count}");
        foreach (var c in d.RunIntervalChanges.Concat(d.PreExistingChangedFurther).Concat(d.AttributionAmbiguities))
        {
            o.WriteLine($"- [{c.Classification}] {c.Status} {c.Path}{(c.OldPath is null ? "" : $" (from {c.OldPath})")}: {c.ClassificationReason}");
        }

        o.WriteLine("Note: interval changes are observations, not proof of executor authorship.");
        return 0;
    }
}