using System.Text.RegularExpressions;

namespace AgentsWatch.Core;

public enum RunLifecycleStatus
{
    InProgress,
    Completed
}

public sealed record RunManifest(
    string SchemaVersion,
    string TaskId,
    string Title,
    DateTimeOffset StartedAt,
    DateTimeOffset? FinishedAt,
    RunLifecycleStatus Status,
    string RepositoryRoot,
    string StartBranch,
    string StartCommitSha,
    string? EndBranch,
    string? EndCommitSha,
    IReadOnlyList<string> AllowedPaths,
    IReadOnlyList<ChangedFile> ChangedFiles,
    IReadOnlyList<ChangedFile> OutOfScopeFiles,
    IReadOnlyList<string> Warnings)
{
    public const string CurrentSchemaVersion = "1.0";

    public static RunManifest Start(
        string taskId,
        string title,
        string repositoryRoot,
        GitSnapshot snapshot,
        IReadOnlyList<string> allowedPaths,
        DateTimeOffset startedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(repositoryRoot);
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(allowedPaths);

        return new RunManifest(
            CurrentSchemaVersion,
            taskId,
            title,
            startedAt,
            null,
            RunLifecycleStatus.InProgress,
            repositoryRoot,
            snapshot.Branch,
            snapshot.CommitSha,
            null,
            null,
            allowedPaths,
            [],
            [],
            []);
    }

    public RunManifest Complete(
        GitSnapshot endSnapshot,
        IReadOnlyList<ChangedFile> changedFiles,
        IReadOnlyList<ChangedFile> outOfScopeFiles,
        IReadOnlyList<string> warnings,
        DateTimeOffset finishedAt)
    {
        ArgumentNullException.ThrowIfNull(endSnapshot);
        ArgumentNullException.ThrowIfNull(changedFiles);
        ArgumentNullException.ThrowIfNull(outOfScopeFiles);
        ArgumentNullException.ThrowIfNull(warnings);

        return this with
        {
            FinishedAt = finishedAt,
            Status = RunLifecycleStatus.Completed,
            EndBranch = endSnapshot.Branch,
            EndCommitSha = endSnapshot.CommitSha,
            ChangedFiles = changedFiles,
            OutOfScopeFiles = outOfScopeFiles,
            Warnings = warnings
        };
    }
}

public static class RunId
{
    private static readonly Regex ValidPattern = new(
        "^[A-Za-z0-9][A-Za-z0-9._-]{0,99}$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !ValidPattern.IsMatch(value))
        {
            throw new ArgumentException(
                "Task ID must be 1-100 characters and contain only letters, numbers, '.', '_' or '-'.",
                nameof(value));
        }

        return value;
    }
}

public static class ScopeMatcher
{
    public static bool IsAllowed(string path, IReadOnlyList<string> allowedPatterns)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(allowedPatterns);

        if (allowedPatterns.Count == 0)
        {
            return true;
        }

        var normalizedPath = Normalize(path);
        return allowedPatterns.Any(pattern => Matches(normalizedPath, pattern));
    }

    private static bool Matches(string normalizedPath, string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            return false;
        }

        var normalizedPattern = Normalize(pattern.Trim());
        if (normalizedPattern.EndsWith("/", StringComparison.Ordinal))
        {
            normalizedPattern += "**";
        }

        var regexPattern = "^" + Regex.Escape(normalizedPattern)
            .Replace("\\*\\*", ".*", StringComparison.Ordinal)
            .Replace("\\*", "[^/]*", StringComparison.Ordinal)
            .Replace("\\?", "[^/]", StringComparison.Ordinal) + "$";

        return Regex.IsMatch(
            normalizedPath,
            regexPattern,
            RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));
    }

    private static string Normalize(string value) => value.Replace('\\', '/').TrimStart('/');
}
