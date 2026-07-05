using AgentsWatch.Core;

namespace AgentsWatch.Git;

public sealed class GitRepositoryLocator
{
    private readonly IGitCommandRunner _git;

    public GitRepositoryLocator(IGitCommandRunner git)
    {
        _git = git;
    }

    public async Task<string> FindRootAsync(
        string workingDirectory,
        CancellationToken cancellationToken = default)
    {
        var root = await _git.RunAsync(
            workingDirectory,
            "rev-parse --show-toplevel",
            cancellationToken);

        if (string.IsNullOrWhiteSpace(root))
        {
            throw new InvalidOperationException("Git repository root could not be determined.");
        }

        return Path.GetFullPath(root);
    }
}

public sealed class GitChangeSetReader
{
    private readonly IGitCommandRunner _git;

    public GitChangeSetReader(IGitCommandRunner git)
    {
        _git = git;
    }

    public async Task<IReadOnlyList<ChangedFile>> ReadSinceAsync(
        string workingDirectory,
        string baseCommitSha,
        GitSnapshot currentSnapshot,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workingDirectory);
        ArgumentNullException.ThrowIfNull(currentSnapshot);
        var validatedBase = GitObjectId.Validate(baseCommitSha);

        var trackedOutput = await _git.RunAsync(
            workingDirectory,
            $"diff --name-status --find-renames {validatedBase} --",
            cancellationToken);

        var tracked = GitNameStatusParser.Parse(trackedOutput);
        var combined = new Dictionary<string, ChangedFile>(StringComparer.Ordinal);

        foreach (var file in tracked)
        {
            combined[file.Path] = file;
        }

        foreach (var file in currentSnapshot.ChangedFiles.Where(static file => file.Status == "??"))
        {
            combined[file.Path] = new ChangedFile(file.Path, "untracked");
        }

        return combined.Values
            .OrderBy(static file => file.Path, StringComparer.Ordinal)
            .ToArray();
    }
}

public static class GitNameStatusParser
{
    public static IReadOnlyList<ChangedFile> Parse(string output)
    {
        if (string.IsNullOrWhiteSpace(output))
        {
            return [];
        }

        return output
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(static line => ParseLine(line.TrimEnd('\r')))
            .ToArray();
    }

    private static ChangedFile ParseLine(string line)
    {
        var columns = line.Split('\t');
        if (columns.Length < 2)
        {
            return new ChangedFile(line.Trim(), "unknown");
        }

        var status = columns[0].Trim();
        var path = status.StartsWith('R') || status.StartsWith('C')
            ? columns[^1].Trim()
            : columns[1].Trim();

        return new ChangedFile(path, NormalizeStatus(status));
    }

    private static string NormalizeStatus(string status) => status switch
    {
        "A" => "added",
        "M" => "modified",
        "D" => "deleted",
        "T" => "type-changed",
        "U" => "unmerged",
        _ when status.StartsWith('R') => "renamed",
        _ when status.StartsWith('C') => "copied",
        _ => string.IsNullOrWhiteSpace(status) ? "unknown" : status
    };
}
