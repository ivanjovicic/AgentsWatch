using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AgentsWatch.Core.Runs;

/// <summary>
/// Deterministic trust level of the executor identity recorded in a receipt (AW-FIX-EXECUTOR-001).
/// <see cref="Verified"/> is reserved for a future provenance mechanism: nothing in this version assigns it,
/// and receipts that claim it are rejected on load.
/// </summary>
public enum ExecutorIdentityStatus { Unknown, DeclaredOnly, ConsistentUnverified, Conflict, Verified }

/// <summary>
/// Bounded metadata read from an operator-selected implementation run log, with its workspace-relative path and SHA-256.
/// A byte-verified log is still only a declarative source: it is never proof of authorship.
/// </summary>
public sealed record ObservedExecutorEvidence(
    string SourceKind,
    string SourcePath,
    string SourceSha256,
    string? Format,
    string? AgentTool = null,
    string? ModelProvider = null,
    string? ModelName = null,
    string? ClientIde = null,
    string? Problem = null);

/// <summary>Result of comparing the operator declaration (receipt.executorMetadata) with an additional source.</summary>
public sealed record ExecutorIdentityAssessment(
    ExecutorIdentityStatus Status,
    bool AuthorshipVerified,
    string Reason,
    IReadOnlyList<string> ComparedFields,
    IReadOnlyList<string> ConflictingFields,
    IReadOnlyList<string> SourceRefs);

public static class ExecutorIdentityRules
{
    /// <summary>Executor identity conflict. Informational for the decision: it never changes Done/NeedsReview ordering
    /// and is distinct from AWR002 (ambiguous Git change attribution).</summary>
    public const string Conflict = "AWI001";
}

/// <summary>
/// Pure, deterministic comparison of declared vs. additional-source executor identity.
/// Compared pairs (exact match after <see cref="Normalize"/>):
///   declared agent  &lt;-&gt; log "Agent/tool"
///   declared model  &lt;-&gt; log "Model name/id"
/// Not compared (no comparable counterpart): declared tool, log "Model provider", log "Client/IDE".
/// Absent or placeholder values are skipped, never treated as a conflict.
/// Never returns <see cref="ExecutorIdentityStatus.Verified"/> and never sets AuthorshipVerified.
/// </summary>
public static class ExecutorIdentityEvaluator
{
    public const string SourceKindImplementationLog = "implementation-run-log";

    private const string NotProof = "Neither source independently proves who authored the changes.";

    private static readonly HashSet<string> Placeholders = new(StringComparer.Ordinal) { "unknown", "n/a", "na", "none", "null", "-", "?" };

    /// <summary>NFKC, collapse every whitespace run to one space, trim, lower-case (invariant). Placeholders and
    /// values starting with "unknown-" (e.g. unknown-not-exposed) normalize to null (absent).</summary>
    public static string? Normalize(string? value)
    {
        if (value is null)
        {
            return null;
        }

        var collapsed = Regex.Replace(value.Normalize(NormalizationForm.FormKC), @"\s+", " ").Trim().ToLowerInvariant();
        if (collapsed.Length == 0 || Placeholders.Contains(collapsed) || collapsed.StartsWith("unknown-", StringComparison.Ordinal))
        {
            return null;
        }

        return collapsed;
    }

    public static ExecutorIdentityAssessment Evaluate(ExecutorMetadata? declared, ObservedExecutorEvidence? observed)
    {
        var agent = Normalize(declared?.Agent);
        var tool = Normalize(declared?.Tool);
        var model = Normalize(declared?.Model);
        var hasDeclared = agent is not null || tool is not null || model is not null;
        string[] declaredRef = ["executorMetadata"];
        string[] bothRefs = ["executorMetadata", "observedExecutorEvidence"];

        if (observed is null)
        {
            return hasDeclared
                ? Result(ExecutorIdentityStatus.DeclaredOnly, "Only the operator declaration is available; no additional source was provided. A declaration is a claim, not proof of authorship.", [], [], declaredRef)
                : Result(ExecutorIdentityStatus.Unknown, "No operator declaration and no additional source; executor identity cannot be compared.", [], [], []);
        }

        if (observed.Problem is not null)
        {
            return Result(ExecutorIdentityStatus.Unknown, $"Additional source is unusable ({observed.Problem}); identities were not compared and no attribution is made.", [], [], bothRefs);
        }

        if (!hasDeclared)
        {
            return Result(ExecutorIdentityStatus.Unknown, "No operator declaration to compare with the additional source; a log alone does not identify the executor.", [], [], ["observedExecutorEvidence"]);
        }

        var pairs = new (string Field, string? Declared, string? Observed)[]
        {
            ("agent", agent, Normalize(observed.AgentTool)),
            ("model", model, Normalize(observed.ModelName))
        };
        var compared = pairs.Where(p => p.Declared is not null && p.Observed is not null).ToArray();
        if (compared.Length == 0)
        {
            return Result(ExecutorIdentityStatus.DeclaredOnly,
                "The additional source has no field comparable with the operator declaration (agent vs Agent/tool, model vs Model name/id); only the declaration is usable. A declaration is a claim, not proof of authorship.",
                [], [], bothRefs);
        }

        var comparedFields = compared.Select(p => p.Field).ToArray();
        var conflicting = compared.Where(p => !string.Equals(p.Declared, p.Observed, StringComparison.Ordinal)).Select(p => p.Field).ToArray();
        if (conflicting.Length > 0)
        {
            return Result(ExecutorIdentityStatus.Conflict,
                $"Operator declaration conflicts with implementation log metadata. {NotProof} Conflicting field(s): {string.Join(", ", conflicting)}.",
                comparedFields, conflicting, bothRefs);
        }

        return Result(ExecutorIdentityStatus.ConsistentUnverified,
            $"Operator declaration matches implementation log metadata on: {string.Join(", ", comparedFields)}. Matching declarative sources are not verification. {NotProof}",
            comparedFields, [], bothRefs);
    }

    // The only construction path: AuthorshipVerified is always false in this version.
    private static ExecutorIdentityAssessment Result(ExecutorIdentityStatus status, string reason, IReadOnlyList<string> compared, IReadOnlyList<string> conflicting, IReadOnlyList<string> refs) =>
        new(status, false, reason, compared, conflicting, refs);
}

/// <summary>
/// Deterministic extraction of the four identity fields from an "Evidence format: v2" run log header.
/// Only "Key: value" lines before the first "## " section are read; narrative is never interpreted.
/// Nothing in the log is executed or sent anywhere.
/// </summary>
public static class ExecutorLogParser
{
    public const int MaxValueLength = 200;

    private const string FormatKey = "Evidence format";
    private static readonly string[] Keys = [FormatKey, "Agent/tool", "Model provider", "Model name/id", "Client/IDE"];

    public static ObservedExecutorEvidence Parse(byte[] content, string sourcePath)
    {
        var sha = "sha256:" + Convert.ToHexString(SHA256.HashData(content)).ToLowerInvariant();
        ObservedExecutorEvidence Problem(string reason) => new(ExecutorIdentityEvaluator.SourceKindImplementationLog, sourcePath, sha, null, Problem: reason);

        if (Array.IndexOf(content, (byte)0) >= 0)
        {
            return Problem("log contains NUL bytes");
        }

        string text;
        try
        {
            text = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(content);
        }
        catch (DecoderFallbackException)
        {
            return Problem("log is not valid UTF-8");
        }

        if (text.Length > 0 && text[0] == '\uFEFF')
        {
            text = text[1..];
        }

        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var raw in text.Split('\n'))
        {
            var line = raw.EndsWith('\r') ? raw[..^1] : raw;
            if (line.StartsWith("## ", StringComparison.Ordinal))
            {
                break; // header ends at the first section; the body is narrative and is never parsed
            }

            var key = Keys.FirstOrDefault(k => line.StartsWith(k + ":", StringComparison.Ordinal));
            if (key is null)
            {
                continue;
            }

            var value = line[(key.Length + 1)..].Trim(' ', '\t');
            if (fields.ContainsKey(key))
            {
                return Problem($"duplicate '{key}' header field");
            }

            if (value.Any(c => char.IsControl(c) && c != '\t'))
            {
                return Problem($"'{key}' contains control characters");
            }

            if (value.Length > MaxValueLength)
            {
                return Problem($"'{key}' exceeds {MaxValueLength} characters");
            }

            fields[key] = value;
        }

        if (!fields.TryGetValue(FormatKey, out var format))
        {
            return Problem("missing 'Evidence format: v2' header; only Evidence format: v2 logs are supported");
        }

        if (!string.Equals(format, "v2", StringComparison.Ordinal))
        {
            return Problem("unsupported evidence format; only Evidence format: v2 logs are supported");
        }

        string? Field(string k) => fields.TryGetValue(k, out var v) && ExecutorIdentityEvaluator.Normalize(v) is not null ? v : null;
        return new ObservedExecutorEvidence(ExecutorIdentityEvaluator.SourceKindImplementationLog, sourcePath, sha, "v2",
            Field("Agent/tool"), Field("Model provider"), Field("Model name/id"), Field("Client/IDE"));
    }
}

public sealed record ExecutorLogReadResult(ObservedExecutorEvidence? Evidence, string? RefusalReason)
{
    public bool Refused => RefusalReason is not null;
}

/// <summary>
/// Read-only, workspace-confined access to the operator-selected implementation log for `finish --executor-log`.
/// Refuses absolute paths, '..' segments, any symbolic link or junction on the path, AgentsWatch state and
/// generated projections, non-regular files and files larger than <see cref="MaxBytes"/>.
/// Refusal reasons never echo the supplied path.
/// </summary>
public static class ExecutorLogReader
{
    public const int MaxBytes = 64 * 1024;

    public static ExecutorLogReadResult Read(string workspaceRoot, string path)
    {
        static ExecutorLogReadResult Refuse(string reason) => new(null, reason);

        if (string.IsNullOrWhiteSpace(path) || path.Contains('\0'))
        {
            return Refuse("path is empty or invalid");
        }

        var rel = OperatingSystem.IsWindows() ? path.Replace('\\', '/') : path;
        if (Path.IsPathRooted(path) || Path.IsPathRooted(rel) || rel.StartsWith('/') || (rel.Length >= 2 && rel[1] == ':'))
        {
            return Refuse("path must be workspace-relative");
        }

        var segments = rel.Split('/', StringSplitOptions.RemoveEmptyEntries).Where(s => s != ".").ToArray();
        if (segments.Any(s => s == ".."))
        {
            return Refuse("path escapes the workspace ('..' segments are not allowed)");
        }

        if (segments.Length == 0)
        {
            return Refuse("path is empty or invalid");
        }

        if (string.Equals(segments[0], ".agentwatch", StringComparison.OrdinalIgnoreCase))
        {
            return Refuse("AgentsWatch internal state (.agentwatch/) is not an implementation log");
        }

        if (segments.Length >= 2 && string.Equals(segments[0], ".ai", StringComparison.OrdinalIgnoreCase) && string.Equals(segments[1], "handoffs", StringComparison.OrdinalIgnoreCase))
        {
            return Refuse("AgentsWatch-generated projection (.ai/handoffs/) is not an implementation log");
        }

        var fullRoot = Path.GetFullPath(workspaceRoot);
        var current = fullRoot;
        try
        {
            foreach (var segment in segments)
            {
                current = Path.Combine(current, segment);
                FileSystemInfo info = Directory.Exists(current) ? new DirectoryInfo(current) : new FileInfo(current);
                if (info.LinkTarget is not null || (info.Exists && info.Attributes.HasFlag(FileAttributes.ReparsePoint)))
                {
                    return Refuse("path traverses a symbolic link or junction");
                }
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            return Refuse("path cannot be inspected safely");
        }

        var full = Path.GetFullPath(Path.Combine(fullRoot, string.Join(Path.DirectorySeparatorChar, segments)));
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        if (!full.StartsWith(fullRoot.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, comparison))
        {
            return Refuse("path escapes the workspace");
        }

        if (Directory.Exists(full))
        {
            return Refuse("not a regular file");
        }

        if (!File.Exists(full))
        {
            return Refuse("file not found");
        }

        byte[] bytes;
        try
        {
            if (new FileInfo(full).Length > MaxBytes)
            {
                return Refuse($"file exceeds {MaxBytes} bytes");
            }

            using var stream = new FileStream(full, FileMode.Open, FileAccess.Read, FileShare.Read);
            var buffer = new byte[MaxBytes + 1];
            var total = 0;
            int read;
            while (total < buffer.Length && (read = stream.Read(buffer, total, buffer.Length - total)) > 0)
            {
                total += read;
            }

            if (total > MaxBytes)
            {
                return Refuse($"file exceeds {MaxBytes} bytes");
            }

            bytes = buffer[..total];
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return Refuse("file cannot be read");
        }

        var head = Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 4096));
        if (head.StartsWith("# AgentsWatch Run Receipt", StringComparison.Ordinal)
            || head.StartsWith("# Handoff - ", StringComparison.Ordinal)
            || head.Contains("> Generated from `.agentwatch/runs/", StringComparison.Ordinal))
        {
            return Refuse("AgentsWatch-generated projection is not an implementation log");
        }

        return new ExecutorLogReadResult(ExecutorLogParser.Parse(bytes, string.Join('/', segments)), null);
    }
}
