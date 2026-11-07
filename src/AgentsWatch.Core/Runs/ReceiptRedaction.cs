using System.Text.RegularExpressions;

namespace AgentsWatch.Core.Runs;

/// <summary>
/// Redacts sentinel-like secrets from a receipt after evaluation and before digest.
/// The stored canonical JSON (and therefore the evidence digest) covers the redacted form.
/// Evaluation sees in-memory values first; projections and CLI print the stored receipt.
/// </summary>
public static class ReceiptRedaction
{
    public const string Placeholder = "[redacted]";

    private static readonly Regex[] Patterns =
    [
        new(@"AWSENTINEL-TOKEN-[A-Za-z0-9_-]+", RegexOptions.Compiled),
        new(@"AKIA[0-9A-Z]{16}", RegexOptions.Compiled),
        new(@"(?i)\bBearer\s+[A-Za-z0-9._\-+=/]+", RegexOptions.Compiled),
        new(@"(?i)\b(password|passwd|pwd|secret|token|api[_-]?key)\s*[=:]\s*\S+", RegexOptions.Compiled)
    ];

    public static string? RedactText(string? value)
    {
        if (value is null)
        {
            return null;
        }

        var result = value;
        foreach (var pattern in Patterns)
        {
            result = pattern.Replace(result, Placeholder);
        }

        return result;
    }

    public static bool CommandsMatch(string? left, string? right) =>
        string.Equals(left, right, StringComparison.Ordinal)
        || string.Equals(RedactText(left), RedactText(right), StringComparison.Ordinal);

    public static RunReceipt Apply(RunReceipt receipt)
    {
        // Serialize the whole receipt so nested worktree entries, deltas, findings,
        // and future string fields cannot bypass redaction. Patterns do not match JSON syntax.
        var json = RunReceiptJson.Serialize(receipt with { EvidenceDigest = null });
        return RunReceiptJson.Deserialize(RedactText(json)!);
    }
}
