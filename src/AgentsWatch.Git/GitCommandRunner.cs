using System.Diagnostics;
using AgentsWatch.Core;

namespace AgentsWatch.Git;

public interface IGitCommandRunner
{
    Task<string> RunAsync(string workingDirectory, string arguments, CancellationToken cancellationToken = default);
}

public sealed class GitCommandRunner : IGitCommandRunner
{
    public async Task<string> RunAsync(string workingDirectory, string arguments, CancellationToken cancellationToken = default)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = string.IsNullOrWhiteSpace(workingDirectory)
                ? Directory.GetCurrentDirectory()
                : workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start git process.");

        var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        var output = await outputTask;
        var error = await errorTask;

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"git {arguments} failed: {error}".Trim());
        }

        return output.TrimEnd();
    }
}

public sealed class GitSnapshotReader
{
    private readonly IGitCommandRunner _git;

    public GitSnapshotReader(IGitCommandRunner git)
    {
        _git = git;
    }

    public async Task<GitSnapshot> ReadAsync(string workingDirectory, CancellationToken cancellationToken = default)
    {
        var branch = await _git.RunAsync(workingDirectory, "rev-parse --abbrev-ref HEAD", cancellationToken);
        var commit = await _git.RunAsync(workingDirectory, "rev-parse HEAD", cancellationToken);
        var status = await _git.RunAsync(workingDirectory, "status --short -uall", cancellationToken);
        var changedFiles = GitStatusParser.Parse(status);

        return new GitSnapshot(branch, commit, changedFiles, status);
    }
}

public static class GitStatusParser
{
    public static IReadOnlyList<ChangedFile> Parse(string statusOutput)
    {
        if (string.IsNullOrWhiteSpace(statusOutput))
        {
            return [];
        }

        return statusOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseLine)
            .ToArray();
    }

    private static ChangedFile ParseLine(string line)
    {
        if (line.Length < 4)
        {
            return new ChangedFile(line, "unknown");
        }

        var status = line[..2].Trim();
        var path = line[3..].Trim();
        return new ChangedFile(path, string.IsNullOrWhiteSpace(status) ? "modified" : status);
    }
}
