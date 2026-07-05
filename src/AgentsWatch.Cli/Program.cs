using AgentsWatch.Core;
using AgentsWatch.Git;
using AgentsWatch.LanguageAdapters;
using AgentsWatch.Reports;

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
            var workingDirectory = Directory.GetCurrentDirectory();
            return args[0] switch
            {
                "init" => InitCommand.Run(workingDirectory),
                "optimize" => OptimizeCommand.Run(args.Skip(1).ToArray()),
                "status" => await StatusCommand.RunAsync(workingDirectory),
                "start" => await StartCommand.RunAsync(args.Skip(1).ToArray(), workingDirectory),
                "finish" => await FinishCommand.RunAsync(args.Skip(1).ToArray(), workingDirectory),
                "report" => await ReportCommand.RunAsync(args.Skip(1).ToArray(), workingDirectory),
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
AgentsWatch — local evidence and control layer for AI coding-agent work

Usage:
  agentswatch init
  agentswatch optimize <prompt text or prompt file>
  agentswatch status
  agentswatch start <task-id> [--title <text>] [--scope <glob>]...
  agentswatch finish <task-id>
  agentswatch report <task-id>
  agentswatch --version

Current run-evidence boundary:
  start requires a clean Git working tree.
  finish records Git changes since the start commit and scope findings.
  build, test, CI, runtime, and agent-claim evidence are not captured yet.

Planned:
  agentswatch task split <prompt-file>
  agentswatch run -- <command>
  agentswatch pr evidence --run <task-id> --base <branch>
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

internal static class StartCommand
{
    public static async Task<int> RunAsync(string[] args, string workingDirectory)
    {
        if (!TryParse(args, out var options))
        {
            return 2;
        }

        var gitRunner = new GitCommandRunner();
        var repositoryRoot = await new GitRepositoryLocator(gitRunner).FindRootAsync(workingDirectory);
        var snapshot = await new GitSnapshotReader(gitRunner).ReadAsync(repositoryRoot);

        if (snapshot.ChangedFiles.Count > 0)
        {
            Console.Error.WriteLine("Cannot start run evidence from a dirty working tree.");
            Console.Error.WriteLine("Commit, stash, or remove existing changes first so later changes can be attributed to this run.");
            foreach (var file in snapshot.ChangedFiles)
            {
                Console.Error.WriteLine($"- {file.Status} {file.Path}");
            }

            return 3;
        }

        var manifest = RunManifest.Start(
            options.TaskId,
            options.Title,
            repositoryRoot,
            snapshot,
            options.AllowedPaths,
            DateTimeOffset.UtcNow);

        var store = new RunManifestStore();
        var jsonPath = store.GetJsonPath(repositoryRoot, options.TaskId);
        if (File.Exists(jsonPath))
        {
            Console.Error.WriteLine($"Run '{options.TaskId}' already exists. Existing evidence will not be overwritten.");
            return 3;
        }

        await store.CreateAsync(repositoryRoot, manifest);
        var formatter = new MarkdownRunEvidenceFormatter();
        var markdownPath = await formatter.WriteAsync(
            store.GetMarkdownPath(repositoryRoot, options.TaskId),
            manifest);

        Console.WriteLine($"Started run: {options.TaskId}");
        Console.WriteLine($"Repository: {repositoryRoot}");
        Console.WriteLine($"Base commit: {snapshot.CommitSha}");
        Console.WriteLine($"Declared scope: {(options.AllowedPaths.Count == 0 ? "unrestricted" : string.Join(", ", options.AllowedPaths))}");
        Console.WriteLine($"Manifest: {jsonPath}");
        Console.WriteLine($"Report: {markdownPath}");
        return 0;
    }

    private static bool TryParse(string[] args, out StartOptions options)
    {
        options = default!;
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: agentswatch start <task-id> [--title <text>] [--scope <glob>]...");
            return false;
        }

        var taskId = RunId.Validate(args[0]);
        var title = taskId;
        var scopes = new List<string>();

        for (var index = 1; index < args.Length; index++)
        {
            switch (args[index])
            {
                case "--title" when index + 1 < args.Length:
                    title = args[++index];
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.Error.WriteLine("--title must not be empty.");
                        return false;
                    }

                    break;
                case "--scope" when index + 1 < args.Length:
                    var scope = args[++index].Trim();
                    if (string.IsNullOrWhiteSpace(scope))
                    {
                        Console.Error.WriteLine("--scope must not be empty.");
                        return false;
                    }

                    scopes.Add(scope);
                    break;
                default:
                    Console.Error.WriteLine($"Unknown or incomplete start option: {args[index]}");
                    Console.Error.WriteLine("Usage: agentswatch start <task-id> [--title <text>] [--scope <glob>]...");
                    return false;
            }
        }

        options = new StartOptions(taskId, title, scopes);
        return true;
    }

    private sealed record StartOptions(string TaskId, string Title, IReadOnlyList<string> AllowedPaths);
}

internal static class FinishCommand
{
    public static async Task<int> RunAsync(string[] args, string workingDirectory)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: agentswatch finish <task-id>");
            return 2;
        }

        var taskId = RunId.Validate(args[0]);
        var gitRunner = new GitCommandRunner();
        var repositoryRoot = await new GitRepositoryLocator(gitRunner).FindRootAsync(workingDirectory);
        var store = new RunManifestStore();
        var manifest = await store.LoadAsync(repositoryRoot, taskId);

        if (manifest.Status == RunLifecycleStatus.Completed)
        {
            Console.Error.WriteLine($"Run '{taskId}' is already completed. Existing evidence will not be rewritten by finish.");
            return 3;
        }

        if (!string.Equals(
                Path.GetFullPath(manifest.RepositoryRoot),
                Path.GetFullPath(repositoryRoot),
                OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
        {
            Console.Error.WriteLine("The run manifest belongs to a different repository root.");
            return 3;
        }

        var endSnapshot = await new GitSnapshotReader(gitRunner).ReadAsync(repositoryRoot);
        var changedFiles = await new GitChangeSetReader(gitRunner).ReadSinceAsync(
            repositoryRoot,
            manifest.StartCommitSha,
            endSnapshot);
        var outOfScopeFiles = changedFiles
            .Where(file => !ScopeMatcher.IsAllowed(file.Path, manifest.AllowedPaths))
            .ToArray();

        var warnings = new List<string>();
        if (manifest.AllowedPaths.Count == 0)
        {
            warnings.Add("No allowed scope was declared; out-of-scope detection is unavailable for this run.");
        }

        if (!string.Equals(manifest.StartBranch, endSnapshot.Branch, StringComparison.Ordinal))
        {
            warnings.Add($"Branch changed during the run: {manifest.StartBranch} -> {endSnapshot.Branch}.");
        }

        if (endSnapshot.ChangedFiles.Count > 0)
        {
            warnings.Add("The repository contains uncommitted changes at finish time.");
        }

        var completed = manifest.Complete(
            endSnapshot,
            changedFiles,
            outOfScopeFiles,
            warnings,
            DateTimeOffset.UtcNow);

        await store.SaveAsync(repositoryRoot, completed);
        var formatter = new MarkdownRunEvidenceFormatter();
        var markdownPath = await formatter.WriteAsync(
            store.GetMarkdownPath(repositoryRoot, taskId),
            completed);

        Console.WriteLine($"Finished run: {taskId}");
        Console.WriteLine($"Head commit: {endSnapshot.CommitSha}");
        Console.WriteLine($"Changed files: {changedFiles.Count}");
        Console.WriteLine($"Outside declared scope: {outOfScopeFiles.Length}");
        Console.WriteLine($"Report: {markdownPath}");
        Console.WriteLine("Evidence boundary: build, test, CI, runtime, and agent-claim evidence are not captured yet.");
        return 0;
    }
}

internal static class ReportCommand
{
    public static async Task<int> RunAsync(string[] args, string workingDirectory)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: agentswatch report <task-id>");
            return 2;
        }

        var taskId = RunId.Validate(args[0]);
        var gitRunner = new GitCommandRunner();
        var repositoryRoot = await new GitRepositoryLocator(gitRunner).FindRootAsync(workingDirectory);
        var store = new RunManifestStore();
        var manifest = await store.LoadAsync(repositoryRoot, taskId);
        var formatter = new MarkdownRunEvidenceFormatter();
        Console.Write(formatter.Format(manifest));
        return 0;
    }
}
