using AgentsWatch.Core;
using AgentsWatch.Git;
using AgentsWatch.LanguageAdapters;

namespace AgentsWatch.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0 || args[0] is "--help" or "-h" or "help")
        {
            Console.WriteLine(HelpText());
            return 0;
        }

        if (args[0] is "--version" or "version")
        {
            Console.WriteLine("AgentsWatch 0.1.0");
            return 0;
        }

        try
        {
            return args[0] switch
            {
                "init" => InitCommand.Run(Directory.GetCurrentDirectory()),
                "optimize" => OptimizeCommand.Run(args.Skip(1).ToArray()),
                "status" => await StatusCommand.RunAsync(Directory.GetCurrentDirectory()),
                _ => Unknown(args[0])
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"error: {ex.Message}");
            return 1;
        }
    }

    private static int Unknown(string command)
    {
        Console.Error.WriteLine($"Unknown command: {command}");
        Console.Error.WriteLine(HelpText());
        return 2;
    }

    private static string HelpText() => """
AgentsWatch — AI coding-agent supervisor and token optimizer

Usage:
  agentswatch init
  agentswatch optimize <prompt text or prompt file>
  agentswatch status
  agentswatch --version

Planned:
  agentswatch task split <prompt-file>
  agentswatch start <task-id>
  agentswatch finish <task-id>
  agentswatch report
  agentswatch handoff
  agentswatch review-diff <commit-or-range>
  agentswatch validate
""";
}

internal static class InitCommand
{
    public static int Run(string root)
    {
        Directory.CreateDirectory(Path.Combine(root, ".ai", "tasks"));
        Directory.CreateDirectory(Path.Combine(root, ".ai", "runs"));
        Directory.CreateDirectory(Path.Combine(root, ".ai", "generated"));
        Directory.CreateDirectory(Path.Combine(root, ".agentwatch"));

        WriteIfMissing(Path.Combine(root, ".ai", "config.yml"), DefaultConfig());
        WriteIfMissing(Path.Combine(root, ".ai", "STATUS.md"), "# AgentsWatch Status\n\nNo runs recorded yet.\n");
        WriteIfMissing(Path.Combine(root, ".ai", "CHANGELOG_AI.md"), "# AI Changelog\n\nNo agent runs recorded yet.\n");
        WriteIfMissing(Path.Combine(root, ".ai", "REVIEW_CHECKLIST.md"), DefaultReviewChecklist());

        Console.WriteLine("AgentsWatch initialized.");
        return 0;
    }

    private static void WriteIfMissing(string path, string content)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, content);
        }
    }

    private static string DefaultConfig() => """
project:
  name: Unknown Project
  types: []

validation: {}

risk:
  high:
    - "**/Auth/**"
    - "**/Security/**"
    - "**/Migrations/**"
  medium:
    - "src/**"
    - "lib/**"
""";

    private static string DefaultReviewChecklist() => """
# AgentsWatch Review Checklist

- [ ] Prompt had token budget and scope limiter.
- [ ] Changed files match claimed scope.
- [ ] Tests were added or missed tests were documented.
- [ ] Validation was run or blocked reason was recorded.
- [ ] Handoff summary exists for follow-up work.
""";
}

internal static class OptimizeCommand
{
    public static int Run(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: agentswatch optimize <prompt text or prompt file>");
            return 2;
        }

        var input = string.Join(' ', args);
        var prompt = File.Exists(input) ? File.ReadAllText(input) : input;
        var analyzer = new PromptRiskAnalyzer();
        var result = analyzer.Optimize(new PromptOptimizationRequest(prompt));

        Console.WriteLine($"Risk: {result.Risk}");
        Console.WriteLine($"Budget: {result.Budget}");
        Console.WriteLine();
        Console.WriteLine("Waste causes:");
        foreach (var cause in result.WasteCauses.DefaultIfEmpty("none detected"))
        {
            Console.WriteLine($"- {cause}");
        }

        Console.WriteLine();
        Console.WriteLine("Suggested split:");
        foreach (var item in result.SuggestedSplit)
        {
            Console.WriteLine($"- {item}");
        }

        Console.WriteLine();
        Console.WriteLine("Optimized prompt:");
        Console.WriteLine(result.OptimizedPrompt);
        return 0;
    }
}

internal static class StatusCommand
{
    public static async Task<int> RunAsync(string root)
    {
        var detector = new ProjectTypeDetector();
        var projectTypes = detector.Detect(root);
        Console.WriteLine("Detected project types: " + string.Join(", ", projectTypes));

        var provider = new ValidationCommandProvider();
        var commands = provider.GetSuggestedCommands(projectTypes);
        Console.WriteLine("Suggested validation:");
        foreach (var command in commands.DefaultIfEmpty("none"))
        {
            Console.WriteLine("- " + command);
        }

        var git = new GitSnapshotReader(new GitCommandRunner());
        var snapshot = await git.ReadAsync(root);
        Console.WriteLine($"Branch: {snapshot.Branch}");
        Console.WriteLine($"Commit: {snapshot.CommitSha}");
        Console.WriteLine($"Changed files: {snapshot.ChangedFiles.Count}");
        return 0;
    }
}
