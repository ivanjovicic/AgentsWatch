using System.Security.Cryptography;
using AgentsWatch.Core.Runs;

namespace AgentsWatch.Git;

/// <summary>
/// Reads branch/HEAD and lossless worktree state. Fingerprints:
/// staged = index blob id (git ls-files -s), unstaged/untracked = sha256 of worktree bytes,
/// "deleted" when absent. Tool-owned ".agentwatch/" paths are excluded.
/// </summary>
public sealed class GitRepositoryStateReader(IGitCommandRunner git) : IRepositoryStateReader
{
    public const string Deleted = "deleted";
    private const string ToolDir = ".agentwatch/";

    public async Task<RepositoryState> ReadAsync(string workspaceRoot, CancellationToken cancellationToken = default)
    {
        string branch;
        try
        {
            branch = await git.RunAsync(workspaceRoot, "rev-parse --abbrev-ref HEAD", cancellationToken);
        }
        catch (InvalidOperationException)
        {
            branch = await git.RunAsync(workspaceRoot, "symbolic-ref --short HEAD", cancellationToken); // unborn branch
        }
        string? head;
        try
        {
            head = await git.RunAsync(workspaceRoot, "rev-parse --verify -q HEAD", cancellationToken);
        }
        catch (InvalidOperationException)
        {
            head = null; // unborn branch
        }

        var top = await git.RunAsync(workspaceRoot, "rev-parse --show-toplevel", cancellationToken);
        var status = await git.RunAsync(workspaceRoot, "status --porcelain=v1 -z -uall", cancellationToken);
        var index = ParseIndex(await git.RunAsync(workspaceRoot, "ls-files -s -z", cancellationToken));

        var staged = new List<WorktreeEntry>();
        var unstaged = new List<WorktreeEntry>();
        var untracked = new List<WorktreeEntry>();
        foreach (var e in ParseStatusZ(status))
        {
            if (e.Path.StartsWith(ToolDir, StringComparison.Ordinal))
            {
                continue;
            }

            if (e.X == '?' )
            {
                untracked.Add(new WorktreeEntry(e.Path, "??", WorktreeFingerprint(top, e.Path)));
                continue;
            }

            if (e.X == '!')
            {
                continue;
            }

            if (e.X != ' ')
            {
                var fp = e.X == 'D' ? Deleted : index.GetValueOrDefault(e.Path, "unknown");
                staged.Add(new WorktreeEntry(e.Path, e.X.ToString(), fp, e.OldPath));
            }

            if (e.Y != ' ')
            {
                var fp = e.Y == 'D' ? Deleted : WorktreeFingerprint(top, e.Path);
                unstaged.Add(new WorktreeEntry(e.Path, e.Y.ToString(), fp, e.X == ' ' ? e.OldPath : null));
            }
        }

        return new RepositoryState(branch, head, WorktreeState.Create(staged, unstaged, untracked));
    }

    public sealed record StatusEntry(char X, char Y, string Path, string? OldPath);

    public static IReadOnlyList<StatusEntry> ParseStatusZ(string output)
    {
        var tokens = output.Split('\0');
        var result = new List<StatusEntry>();
        for (var i = 0; i < tokens.Length; i++)
        {
            var t = tokens[i];
            if (t.Length < 4 || t[2] != ' ')
            {
                continue;
            }

            string? old = null;
            if ((t[0] is 'R' or 'C' || t[1] is 'R' or 'C') && i + 1 < tokens.Length)
            {
                old = tokens[++i];
            }

            result.Add(new StatusEntry(t[0], t[1], t[3..], old));
        }

        return result;
    }

    private static Dictionary<string, string> ParseIndex(string output)
    {
        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var line in output.Split('\0', StringSplitOptions.RemoveEmptyEntries))
        {
            // "<mode> <sha> <stage>\t<path>"
            var tab = line.IndexOf('\t');
            if (tab < 0)
            {
                continue;
            }

            var meta = line[..tab].Split(' ');
            if (meta.Length >= 3)
            {
                map[line[(tab + 1)..]] = $"blob:{meta[1]}:{meta[0]}";
            }
        }

        return map;
    }

    public static string WorktreeFingerprint(string root, string relativePath)
    {
        var full = Path.Combine(root, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (Directory.Exists(full))
        {
            return "dir";
        }

        if (!File.Exists(full))
        {
            return Deleted;
        }

        using var stream = File.OpenRead(full);
        return "sha256:" + Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }
}