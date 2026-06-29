using AgentsWatch.LanguageAdapters;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class ProjectTypeDetectorTests
{
    [Fact]
    public void Detect_FindsDotNetProject()
    {
        using var temp = new TempDirectory();
        File.WriteAllText(Path.Combine(temp.Path, "Example.sln"), string.Empty);

        var result = new ProjectTypeDetector().Detect(temp.Path);

        Assert.Contains(ProjectType.DotNet, result);
    }

    [Fact]
    public void Detect_FindsFlutterProject()
    {
        using var temp = new TempDirectory();
        File.WriteAllText(Path.Combine(temp.Path, "pubspec.yaml"), "name: demo");

        var result = new ProjectTypeDetector().Detect(temp.Path);

        Assert.Contains(ProjectType.Flutter, result);
    }

    [Fact]
    public void ValidationCommands_ReturnExpectedCommands()
    {
        var provider = new ValidationCommandProvider();

        var commands = provider.GetSuggestedCommands(new HashSet<ProjectType>
        {
            ProjectType.DotNet,
            ProjectType.Flutter
        });

        Assert.Contains("dotnet build", commands);
        Assert.Contains("dotnet test", commands);
        Assert.Contains("flutter analyze", commands);
        Assert.Contains("flutter test", commands);
    }

    private sealed class TempDirectory : IDisposable
    {
        public TempDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"agentswatch-{Guid.NewGuid():N}");
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
