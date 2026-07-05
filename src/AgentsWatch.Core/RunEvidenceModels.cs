using System.Text;
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
        GitSnapshot snapshot,
        IReadOnlyList<string> allowedPaths,
        DateTimeOffset startedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(allowedPaths);

        return new RunManifest(
            CurrentSchemaVersion,
            RunId.Validate(taskId),
            title,
            startedAt,
            null,
            RunLifecycleStatus.InProgress,
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

public static class RunArtifactPaths
{
    public static bool IsCurrentRunArtifact(string path, string taskId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        RunId.Validate(taskId);

        var normalizedPath = Normalize(path);
        var prefix = $".ai/runs/{taskId}";
        return string.Equals(normalizedPath, prefix + ".json", StringComparison.Ordinal)
            || string.Equals(normalizedPath, prefix + ".md", StringComparison.Ordinal);
    }

    private static string Normalize(string value) => value.Replace('\\', '/').TrimStart('/');
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

        return Regex.IsMatch(
            normalizedPath,
            BuildRegex(normalizedPattern),
            RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));
    }

    private static string BuildRegex(string pattern)
    {
        var builder = new StringBuilder("^");
        for (var index = 0; index < pattern.Length; index++)
        {
            var character = pattern[index];
            if (character == '*')
            {
                var isDoubleStar = index + 1 < pattern.Length && pattern[index + 1] == '*';
                if (isDoubleStar)
                {
                    var followedBySlash = index + 2 < pattern.Length && pattern[index + 2] == '/';
                    builder.Append(followedBySlash ? "(?:.*/)?" : ".*");
                    index += followedBySlash ? 2 : 1;
                }
                else
                {
                    builder.Append("[^/]*");
                }

                continue;
            }

            if (character == '?')
            {
                builder.Append("[^/]");
                continue;
            }

            builder.Append(Regex.Escape(character.ToString()));
        }

        builder.Append('$');
        return builder.ToString();
    }

    private static string Normalize(string value) => value.Replace('\\', '/').TrimStart('/');
}
