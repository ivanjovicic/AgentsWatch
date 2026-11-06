using AgentsWatch.Core.Paths;

namespace AgentsWatch.Core.Paths;

public enum PathKind
{
    Test,
    Docs,
    Migration,
    Frontend,
    Backend,
    Config,
    Other
}

/// <summary>Explicit stack/path heuristics for claim verification. Unknown structure is represented as Other, never guessed as backend.</summary>
public static class PathClassifier
{
    private static readonly string[] TestSegments = ["test", "tests", "spec", "specs", "__tests__"];
    private static readonly string[] DocsSegments = ["docs", "doc", "documentation"];
    private static readonly string[] MigrationSegments = ["migrations", "migration"];
    private static readonly string[] FrontendSegments = ["wwwroot", "clientapp", "frontend", "ui", "app-ui"];
    private static readonly string[] BackendSegments = ["api", "server", "backend", "controllers", "services", "repositories"];
    private static readonly string[] ConfigFiles = [".gitignore", "dockerfile", "docker-compose.yml", "docker-compose.yaml", "appsettings.json", "appsettings.development.json"];

    public static PathKind Classify(string path)
    {
        var n = RepositoryPath.Normalize(path);
        if (n.Length == 0)
        {
            return PathKind.Other;
        }

        var file = n[(n.LastIndexOf('/') + 1)..];
        var segs = n.Split('/');
        var lowerFile = file.ToLowerInvariant();

        if (HasSegment(segs, MigrationSegments) || LooksLikeMigrationFile(n, lowerFile))
        {
            return PathKind.Migration;
        }

        if (HasSegment(segs, TestSegments) || LooksLikeTestFile(file, lowerFile))
        {
            return PathKind.Test;
        }

        if (HasSegment(segs, DocsSegments) || LooksLikeDocsFile(lowerFile))
        {
            return PathKind.Docs;
        }

        if (HasSegment(segs, FrontendSegments) || LooksLikeFrontendFile(lowerFile))
        {
            return PathKind.Frontend;
        }

        if (LooksLikeConfigFile(n, lowerFile))
        {
            return PathKind.Config;
        }

        if (HasSegment(segs, BackendSegments) || LooksLikeBackendFile(n, lowerFile))
        {
            return PathKind.Backend;
        }

        return PathKind.Other;
    }

    private static bool HasSegment(string[] segs, string[] names) =>
        segs.Any(s => names.Contains(s, StringComparer.OrdinalIgnoreCase));

    private static bool LooksLikeTestFile(string file, string lowerFile)
    {
        var stem = file.Contains('.') ? file[..file.LastIndexOf('.')] : file;
        return stem.EndsWith("Test", StringComparison.Ordinal) || stem.EndsWith("Tests", StringComparison.Ordinal)
            || lowerFile.EndsWith("_test.go") || lowerFile.EndsWith("_test.py") || lowerFile.EndsWith("_test.rb")
            || (lowerFile.StartsWith("test_") && lowerFile.EndsWith(".py"))
            || lowerFile.Contains(".spec.") || lowerFile.Contains(".test.")
            || lowerFile.EndsWith("_spec.rb");
    }

    private static bool LooksLikeDocsFile(string file) =>
        file.EndsWith(".md") || file.EndsWith(".mdx") || file.EndsWith(".rst") || file.EndsWith(".adoc")
        || file.StartsWith("readme") || file.StartsWith("changelog") || file is "license" or "license.txt" or "license.md";

    private static bool LooksLikeMigrationFile(string path, string file) =>
        path.Contains("/prisma/migrations/", StringComparison.OrdinalIgnoreCase)
        || path.Contains("/supabase/migrations/", StringComparison.OrdinalIgnoreCase)
        || path.Contains("/alembic/versions/", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith("migration.cs") || (file.EndsWith(".sql") && path.Contains("migration", StringComparison.OrdinalIgnoreCase));

    private static bool LooksLikeFrontendFile(string file) =>
        file.EndsWith(".tsx") || file.EndsWith(".jsx") || file.EndsWith(".vue") || file.EndsWith(".css") || file.EndsWith(".scss");

    private static bool LooksLikeConfigFile(string path, string file) =>
        ConfigFiles.Contains(file) || file.EndsWith(".csproj") || file.EndsWith(".sln")
        || file is "package.json" or "pubspec.yaml" or "pyproject.toml" || file.StartsWith("appsettings.");

    private static bool LooksLikeBackendFile(string path, string file) =>
        file.EndsWith(".cs") || file.EndsWith(".fs") || file.EndsWith(".java") || file.EndsWith(".go")
        || file.EndsWith(".py") || file.EndsWith(".rb") || file.EndsWith(".kt");
}
