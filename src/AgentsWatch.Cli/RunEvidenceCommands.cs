using AgentsWatch.Core;
using AgentsWatch.Git;
using AgentsWatch.Reports;

namespace AgentsWatch.Cli;

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
        var store = new RunManifestStore();
        var jsonPath = store.GetJsonPath(repositoryRoot, options.TaskId);
        if (File.Exists(jsonPath))
        {
            Console.Error.WriteLine($"Run '{options.TaskId}' already exists. Existing evidence will not be overwritten.");
            return 3;
        }

        var activeRuns = await store.FindActiveAsync(repositoryRoot);
        if (activeRuns.Count > 0)
        {
            Console.Error.WriteLine("Another run is already active. Finish it before starting a new run.");
            foreach (var activeRun in activeRuns)
            {
                Console.Error.WriteLine($"- {activeRun.TaskId} started {activeRun.StartedAt:O}");
            }

            return 3;
        }

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
            snapshot,
            options.AllowedPaths,
            DateTimeOffset.UtcNow);

        await store.CreateAsync(repositoryRoot, manifest);
        var formatter = new MarkdownRunEvidenceFormatter();
        var markdownPath = await formatter.WriteAsync(
            store.GetMarkdownPath(repositoryRoot, options.TaskId),
            manifest);

        Console.WriteLine("Run started");
        Console.WriteLine($"Task: {options.TaskId}");
        Console.WriteLine($"Repository: {repositoryRoot}");
        Console.WriteLine($"Base commit: {snapshot.CommitSha}");
        Console.WriteLine($"Declared scope: {(options.AllowedPaths.Count == 0 ? "unrestricted" : string.Join(", ", options.AllowedPaths))}");
        Console.WriteLine($"Manifest: {jsonPath}");
        Console.WriteLine($"Run report: {markdownPath}");
        Console.WriteLine("Validation: NotRun");
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

        if (!CliInput.TryValidateTaskId(args[0], out var taskId))
        {
            return false;
        }

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

        options = new StartOptions(taskId, title, scopes.Distinct(StringComparer.Ordinal).ToArray());
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

        if (!CliInput.TryValidateTaskId(args[0], out var taskId))
        {
            return 2;
        }

        var gitRunner = new GitCommandRunner();
        var repositoryRoot = await new GitRepositoryLocator(gitRunner).FindRootAsync(workingDirectory);
        var store = new RunManifestStore();
        if (!File.Exists(store.GetJsonPath(repositoryRoot, taskId)))
        {
            Console.Error.WriteLine($"Run '{taskId}' does not exist.");
            return 3;
        }

        var manifest = await store.LoadAsync(repositoryRoot, taskId);
        if (manifest.Status == RunLifecycleStatus.Finished)
        {
            Console.Error.WriteLine($"Run '{taskId}' is already finished. Existing evidence will not be rewritten by finish.");
            return 3;
        }

        var endSnapshot = await new GitSnapshotReader(gitRunner).ReadAsync(repositoryRoot);
        var allChangedFiles = await new GitChangeSetReader(gitRunner).ReadSinceAsync(
            repositoryRoot,
            manifest.StartCommitSha,
            endSnapshot);
        var changedFiles = allChangedFiles
            .Where(file => !RunArtifactPaths.IsCurrentRunArtifact(file.Path, taskId))
            .ToArray();
        var outOfScopeFiles = changedFiles
            .Where(file => !ScopeMatcher.IsAllowed(file.Path, manifest.AllowedPaths))
            .ToArray();
        var uncommittedUserFiles = endSnapshot.ChangedFiles
            .Where(file => !RunArtifactPaths.IsCurrentRunArtifact(file.Path, taskId))
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

        if (uncommittedUserFiles.Length > 0)
        {
            warnings.Add("The repository contains uncommitted non-AgentsWatch changes at finish time.");
        }

        warnings.Add("Validation was not run or captured by this implementation slice.");

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

        Console.WriteLine("Run recorded");
        Console.WriteLine($"Task: {taskId}");
        Console.WriteLine($"Run report: {markdownPath}");
        Console.WriteLine($"Start commit: {manifest.StartCommitSha}");
        Console.WriteLine($"End commit: {endSnapshot.CommitSha}");
        Console.WriteLine($"Changed files: {changedFiles.Length}");
        Console.WriteLine($"Outside declared scope: {outOfScopeFiles.Length}");
        Console.WriteLine("Validation: NotRun");
        Console.WriteLine("Evidence boundary: build, test, CI, runtime, and agent-claim evidence are not captured yet.");
        return 0;
    }
}

internal static class ReportCommand
{
    public static async Task<int> RunAsync(string[] args, string workingDirectory)
    {
        if (args.Length > 1)
        {
            Console.Error.WriteLine("Usage: agentswatch report [task-id]");
            return 2;
        }

        var gitRunner = new GitCommandRunner();
        var repositoryRoot = await new GitRepositoryLocator(gitRunner).FindRootAsync(workingDirectory);
        var store = new RunManifestStore();
        RunManifest? manifest;

        if (args.Length == 0)
        {
            manifest = await store.FindLatestAsync(repositoryRoot);
            if (manifest is null)
            {
                Console.Error.WriteLine("No run evidence exists yet.");
                return 3;
            }
        }
        else
        {
            if (!CliInput.TryValidateTaskId(args[0], out var taskId))
            {
                return 2;
            }

            if (!File.Exists(store.GetJsonPath(repositoryRoot, taskId)))
            {
                Console.Error.WriteLine($"Run '{taskId}' does not exist.");
                return 3;
            }

            manifest = await store.LoadAsync(repositoryRoot, taskId);
        }

        Console.Write(new MarkdownRunEvidenceFormatter().Format(manifest));
        return 0;
    }
}

internal static class CliInput
{
    public static bool TryValidateTaskId(string value, out string taskId)
    {
        try
        {
            taskId = RunId.Validate(value);
            return true;
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine(ex.Message);
            taskId = string.Empty;
            return false;
        }
    }
}
