using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AgentsWatch.Core.Runs;

/// <summary>
/// Redacts sentinel-like secrets from stored JSON string values after evaluation and before digest.
/// Walks a JSON DOM so assignment patterns cannot consume structural quotes or commas.
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

    /// <summary>Redact string values in a JSON document. Property names and punctuation are left intact.</summary>
    public static string RedactJsonDocument(string json)
    {
        using var doc = JsonDocument.Parse(json);
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
        {
            WriteRedacted(doc.RootElement, writer);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static RunReceipt Apply(RunReceipt receipt)
    {
        var json = RunReceiptJson.Serialize(receipt with { EvidenceDigest = null });
        return RunReceiptJson.Deserialize(RedactJsonDocument(json));
    }

    private static void WriteRedacted(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    writer.WritePropertyName(property.Name);
                    WriteRedacted(property.Value, writer);
                }

                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteRedacted(item, writer);
                }

                writer.WriteEndArray();
                break;
            case JsonValueKind.String:
                writer.WriteStringValue(RedactText(element.GetString()));
                break;
            default:
                element.WriteTo(writer);
                break;
        }
    }
}
