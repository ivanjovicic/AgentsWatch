using AgentsWatch.Core.Contracts;
using AgentsWatch.Core.Runs;
using AgentsWatch.Git;

namespace AgentsWatch.Cli;

internal static class StartCommand
{
    public static async Task<int> RunAsync(string root, string[] args, TextWriter? output = null, TextWriter? error = null)
    {
        var o = output ?? Console.Out;
        var e = error ?? Console.Error;
        if (args.Length < 1 || args[0].StartsWith("--"))
        {
            e.WriteLine("Usage: agentswatch start <contract-id> [--agent <name>] [--tool <name>] [--model <name>]");
            return 2;
        }

        string? agent = null, tool = null, model = null;
        for (var i = 1; i < args.Length; i++)
        {
            if (i + 1 >= args.Length)
            {
                e.WriteLine($"Missing value for {args[i]}");
                return 2;
            }

            switch (args[i])
            {
                case "--agent": agent = args[++i]; break;
                case "--tool": tool = args[++i]; break;
                case "--model": model = args[++i]; break;
                default:
                    e.WriteLine($"Unknown option: {args[i]}");
                    return 2;
            }
        }

        var service = new RunStartService(new FileRunContractStore(root), new FileActiveRunStore(root),
            new GitRepositoryStateReader(new GitCommandRunner()));
        var result = await service.StartAsync(root, args[0], new ExecutorMetadata(agent, tool, model));
        if (!result.Started)
        {
            e.WriteLine("error: " + result.Error);
            foreach (var f in result.ContractCheck is { IsValid: false } check ? check.Findings : [])
            {
                e.WriteLine($"- {f.RuleId}: {f.Reason}");
            }

            return 1;
        }

        var b = result.Baseline!;
        o.WriteLine($"Run started: {b.RunId}");
        o.WriteLine($"Baseline: {result.Path}");
        o.WriteLine($"Branch: {b.Branch}  HEAD: {b.HeadCommitSha ?? "(unborn)"}");
        var w = b.WorktreeState;
        o.WriteLine(w.IsDirty
            ? $"Dirty at start: staged {w.Staged.Count}, unstaged {w.Unstaged.Count}, untracked {w.Untracked.Count} (recorded as pre-existing, not attributed to this run)"
            : "Worktree clean at start.");
        return 0;
    }
}