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
        var status = await _git.RunAsync(workingDirectory, "status --porcelain=v1 -z -uall", cancellationToken);
        var changedFiles = GitStatusParser.ParsePorcelainZ(status);

        return new GitSnapshot(branch, commit, changedFiles, status);
    }
}

/// <summary>
/// Parses git porcelain v1 status output.
/// Canonical contract: <c>git status --porcelain=v1 -z -uall</c> (NUL-delimited, unquoted paths,
/// rename/copy entries followed by an extra NUL-terminated original path).
/// The fixed-width XY prefix is never trimmed before slicing the path.
/// </summary>
public static class GitStatusParser
{
    public static IReadOnlyList<ChangedFile> ParsePorcelainZ(string statusOutput)
    {
        if (string.IsNullOrEmpty(statusOutput))
        {
            return [];
        }

        var tokens = statusOutput.Split('\0');
        var result = new List<ChangedFile>();
        for (var i = 0; i < tokens.Length; i++)
        {
            var entry = tokens[i];
            if (entry.Length == 0)
            {
                continue;
            }

            if (entry.Length < 4 || entry[2] != ' ')
            {
                result.Add(new ChangedFile(entry, "unknown"));
                continue;
            }

            var xy = entry[..2];
            var path = entry[3..];
            string? originalPath = null;
            if ((xy[0] is 'R' or 'C' || xy[1] is 'R' or 'C') && i + 1 < tokens.Length)
            {
                originalPath = tokens[++i];
            }

            result.Add(new ChangedFile(path, NormalizeStatus(xy), OriginalPath: originalPath));
        }

        return result;
    }

    /// <summary>
    /// Line-based porcelain v1 (non -z) parser kept for compatibility. Prefer <see cref="ParsePorcelainZ"/>;
    /// this form cannot represent quoted/escaped paths losslessly.
    /// </summary>
    public static IReadOnlyList<ChangedFile> Parse(string statusOutput)
    {
        if (string.IsNullOrWhiteSpace(statusOutput))
        {
            return [];
        }

        return statusOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.TrimEnd('\r'))
            .Where(line => line.Length > 0)
            .Select(ParseLine)
            .ToArray();
    }

    private static ChangedFile ParseLine(string line)
    {
        if (line.Length < 4 || line[2] != ' ')
        {
            return new ChangedFile(line, "unknown");
        }

        var xy = line[..2];
        var path = line[3..];
        string? originalPath = null;
        if (xy[0] is 'R' or 'C' || xy[1] is 'R' or 'C')
        {
            var arrow = path.IndexOf(" -> ", StringComparison.Ordinal);
            if (arrow >= 0)
            {
                originalPath = path[..arrow];
                path = path[(arrow + 4)..];
            }
        }

        return new ChangedFile(path, NormalizeStatus(xy), OriginalPath: originalPath);
    }

    private static string NormalizeStatus(string xy)
    {
        var status = xy.Trim();
        return status.Length == 0 ? "modified" : status;
    }
}