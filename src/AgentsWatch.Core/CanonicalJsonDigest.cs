using System.Security.Cryptography;
using System.Text;

namespace AgentsWatch.Core;

/// <summary>Which accepted form a stored digest matched.</summary>
public enum DigestForm
{
    /// <summary>The stored digest matches no accepted form of the current content.</summary>
    None,

    /// <summary>Canonical form (AW-FIX-DIGEST-001): LF line endings. Identical to digests written by pre-fix builds on Linux/macOS.</summary>
    CanonicalLf,

    /// <summary>Legacy form written by pre-fix builds on Windows: the same JSON with CRLF line endings. Accepted for verification only; never written.</summary>
    LegacyCrlf
}

/// <summary>
/// Platform-independent digests over AgentsWatch's indented JSON serialization (AW-FIX-DIGEST-001).
/// <para>
/// Canonical form: the existing <c>RunContractJson.Options</c> serialization (indented, camelCase, nulls omitted),
/// with every line break written as a single LF (U+000A), encoded as UTF-8 without a BOM, no trailing newline;
/// digest = <c>"sha256:" + lowercase hex(SHA-256(bytes))</c>.
/// </para>
/// <para>
/// Why normalizing line breaks cannot hide a content change: System.Text.Json escapes every control character
/// inside string values (a CR or LF in data is written as <c>\r</c> / <c>\n</c>), so raw CR/LF bytes in the
/// serialized text are only the indentation layout chosen by <c>Environment.NewLine</c>. Mapping CRLF to LF is
/// therefore a bijection between the two layouts of the same content.
/// </para>
/// <para>
/// Verification accepts exactly two forms of the CURRENT content: canonical LF, and legacy CRLF (pre-fix Windows).
/// Both are recomputed from the record being verified, so any change to its content changes both digests.
/// </para>
/// </summary>
public static class CanonicalJsonDigest
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    /// <summary>Normalizes the indentation line breaks of serializer output to LF.</summary>
    public static string ToCanonicalText(string serializedJson) => serializedJson.Replace("\r\n", "\n", StringComparison.Ordinal);

    /// <summary>Digest of the canonical (LF) form. The only form written for new or re-signed records.</summary>
    public static string Compute(string serializedJson) => Hash(ToCanonicalText(serializedJson));

    /// <summary>Digest that a pre-fix Windows build wrote for the same content (CRLF layout).</summary>
    public static string ComputeLegacyCrlf(string serializedJson) => Hash(ToCanonicalText(serializedJson).Replace("\n", "\r\n", StringComparison.Ordinal));

    /// <summary>Identifies which accepted form <paramref name="storedDigest"/> matches for this content.</summary>
    public static DigestForm Match(string? storedDigest, string serializedJson)
    {
        if (string.IsNullOrWhiteSpace(storedDigest))
        {
            return DigestForm.None;
        }

        if (string.Equals(storedDigest, Compute(serializedJson), StringComparison.Ordinal))
        {
            return DigestForm.CanonicalLf;
        }

        return string.Equals(storedDigest, ComputeLegacyCrlf(serializedJson), StringComparison.Ordinal)
            ? DigestForm.LegacyCrlf
            : DigestForm.None;
    }

    private static string Hash(string text) =>
        "sha256:" + Convert.ToHexString(SHA256.HashData(Utf8NoBom.GetBytes(text))).ToLowerInvariant();
}
