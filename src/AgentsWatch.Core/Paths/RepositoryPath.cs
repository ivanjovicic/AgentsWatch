using System.Text;
using System.Text.RegularExpressions;

namespace AgentsWatch.Core.Paths;

/// <summary>
/// Repository-relative path matching. One policy used by contract lint consumers and scope drift.
/// Normalization: backslash to slash, collapse duplicate slashes, strip leading ./ and surrounding slashes.
/// Matching is ordinal (case-sensitive). Globs: ** any path including slashes, * any segment chars except /, ? one non-slash char.
/// No brace expansion or character classes.
/// </summary>
public static class RepositoryPath
{
    public static string Normalize(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var p = path.Replace('\\', '/');
        while (p.Contains("//", StringComparison.Ordinal))
        {
            p = p.Replace("//", "/", StringComparison.Ordinal);
        }

        while (p.StartsWith("./", StringComparison.Ordinal))
        {
            p = p[2..];
        }

        return p.Trim('/');
    }

    public static bool Matches(string pattern, string path)
    {
        var nPattern = Normalize(pattern);
        var nPath = Normalize(path);
        if (nPattern.Length == 0)
        {
            return false;
        }

        return ToRegex(nPattern).IsMatch(nPath);
    }

    public static string? FirstMatchingPattern(IEnumerable<string>? patterns, string path)
    {
        foreach (var pattern in patterns ?? [])
        {
            if (Matches(pattern, path))
            {
                return pattern;
            }
        }

        return null;
    }

    internal static Regex ToRegex(string normalizedPattern)
    {
        var sb = new StringBuilder("^");
        for (var i = 0; i < normalizedPattern.Length; i++)
        {
            var c = normalizedPattern[i];
            if (c == '*' && i + 1 < normalizedPattern.Length && normalizedPattern[i + 1] == '*')
            {
                i++;
                if (i + 1 < normalizedPattern.Length && normalizedPattern[i + 1] == '/')
                {
                    sb.Append("(?:.*/)?");
                    i++;
                }
                else
                {
                    sb.Append(".*");
                }
            }
            else if (c == '*')
            {
                sb.Append("[^/]*");
            }
            else if (c == '?')
            {
                sb.Append("[^/]");
            }
            else
            {
                sb.Append(Regex.Escape(c.ToString()));
            }
        }

        sb.Append('$');
        return new Regex(sb.ToString(), RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
    }
}
