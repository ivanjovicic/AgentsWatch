namespace AgentsWatch.LanguageAdapters;

public enum ProjectType
{
    Unknown,
    DotNet,
    Flutter,
    ReactTypeScript,
    Python,
    Node
}

public sealed class ProjectTypeDetector
{
    public IReadOnlySet<ProjectType> Detect(string repositoryRoot)
    {
        if (string.IsNullOrWhiteSpace(repositoryRoot))
        {
            repositoryRoot = Directory.GetCurrentDirectory();
        }

        var detected = new HashSet<ProjectType>();

        if (Directory.EnumerateFiles(repositoryRoot, "*.sln", SearchOption.TopDirectoryOnly).Any() ||
            Directory.EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories).Any())
        {
            detected.Add(ProjectType.DotNet);
        }

        if (File.Exists(Path.Combine(repositoryRoot, "pubspec.yaml")))
        {
            detected.Add(ProjectType.Flutter);
        }

        if (File.Exists(Path.Combine(repositoryRoot, "package.json")))
        {
            detected.Add(ProjectType.Node);

            if (Directory.EnumerateFiles(repositoryRoot, "*.tsx", SearchOption.AllDirectories).Any() ||
                Directory.EnumerateFiles(repositoryRoot, "*.ts", SearchOption.AllDirectories).Any())
            {
                detected.Add(ProjectType.ReactTypeScript);
            }
        }

        if (File.Exists(Path.Combine(repositoryRoot, "pyproject.toml")) ||
            File.Exists(Path.Combine(repositoryRoot, "requirements.txt")))
        {
            detected.Add(ProjectType.Python);
        }

        return detected.Count == 0 ? new HashSet<ProjectType> { ProjectType.Unknown } : detected;
    }
}

public sealed class ValidationCommandProvider
{
    public IReadOnlyList<string> GetSuggestedCommands(IReadOnlySet<ProjectType> projectTypes)
    {
        var commands = new List<string>();

        if (projectTypes.Contains(ProjectType.DotNet))
        {
            commands.Add("dotnet build");
            commands.Add("dotnet test");
        }

        if (projectTypes.Contains(ProjectType.Flutter))
        {
            commands.Add("flutter analyze");
            commands.Add("flutter test");
        }

        if (projectTypes.Contains(ProjectType.ReactTypeScript) || projectTypes.Contains(ProjectType.Node))
        {
            commands.Add("npm run build");
            commands.Add("npm test");
        }

        if (projectTypes.Contains(ProjectType.Python))
        {
            commands.Add("pytest");
            commands.Add("ruff check .");
        }

        return commands;
    }
}
