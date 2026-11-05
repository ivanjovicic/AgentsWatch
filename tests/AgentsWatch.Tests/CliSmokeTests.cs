using System.Diagnostics;
using AgentsWatch.Cli;
using Xunit;

namespace AgentsWatch.Tests;

[Collection("Console")]
public sealed class CliSmokeTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "aw-test-" + Guid.NewGuid().ToString("N"));

    public CliSmokeTests() => Directory.CreateDirectory(_root);

    public void Dispose()
    {
        try
        {
            foreach (var file in Directory.GetFiles(_root, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            Directory.Delete(_root, true);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }

    [Fact]
    public void Init_CreatesExpectedLayout_AndIsIdempotentWithoutOverwriting()
    {
        Assert.Equal(0, InitCommand.Run(_root));

        foreach (var dir in new[] { ".ai/tasks", ".ai/runs", ".ai/generated", ".ai/handoffs", ".agentwatch/contracts", ".agentwatch/active-runs", ".agentwatch/runs" })
        {
            Assert.True(Directory.Exists(Path.Combine(_root, dir)), dir);
        }

        var files = new[] { ".ai/config.yml", ".ai/STATUS.md", ".ai/CHANGELOG_AI.md", ".ai/REVIEW_CHECKLIST.md" };
        foreach (var file in files)
        {
            Assert.True(File.Exists(Path.Combine(_root, file)), file);
        }

        var written = Directory.GetFiles(_root, "*", SearchOption.AllDirectories)
            .Select(p => Path.GetRelativePath(_root, p).Replace('\\', '/'))
            .OrderBy(p => p, StringComparer.Ordinal);
        Assert.Equal(files.OrderBy(p => p, StringComparer.Ordinal), written);

        foreach (var file in files)
        {
            File.WriteAllText(Path.Combine(_root, file), "user content");
        }

        Assert.Equal(0, InitCommand.Run(_root));

        foreach (var file in files)
        {
            Assert.Equal("user content", File.ReadAllText(Path.Combine(_root, file)));
        }
    }

    [Fact]
    public async Task Status_NonGitDirectory_ReportsClearlyAndSucceeds()
    {
        var output = new StringWriter();

        var exit = await StatusCommand.RunAsync(_root, output);

        Assert.Equal(0, exit);
        Assert.Contains("not a git repository", output.ToString());
    }

    [Fact]
    public async Task Status_CleanAndDirtyGitRepo_ReportsChangedFileCount()
    {
        Git("init -q");
        Git("config user.email t@example.com");
        Git("config user.name t");
        Git("config commit.gpgsign false");
        File.WriteAllText(Path.Combine(_root, "a.txt"), "a");
        Git("add a.txt");
        Git("commit -q -m init");

        var clean = new StringWriter();
        Assert.Equal(0, await StatusCommand.RunAsync(_root, clean));
        Assert.Contains("Changed files: 0", clean.ToString());

        File.WriteAllText(Path.Combine(_root, "a.txt"), "b");
        File.WriteAllText(Path.Combine(_root, "new file.txt"), "n");

        var dirty = new StringWriter();
        Assert.Equal(0, await StatusCommand.RunAsync(_root, dirty));
        Assert.Contains("Changed files: 2", dirty.ToString());
    }

    [Fact]
    public async Task Main_HelpVersionAndUnknown_ReturnExpectedExitCodes()
    {
        var original = Console.Out;
        var originalErr = Console.Error;
        try
        {
            var sink = new StringWriter();
            Console.SetOut(sink);
            Console.SetError(sink);
            Assert.Equal(0, await Program.Main(["--help"]));
            Assert.Equal(0, await Program.Main(["--version"]));
            Assert.Equal(0, await Program.Main(["optimize", "fix", "the", "login", "bug"]));
            Assert.Equal(2, await Program.Main(["optimize"]));
            Assert.Equal(2, await Program.Main(["bogus"]));
            Assert.Contains("AgentsWatch 0.1.0", sink.ToString());
        }
        finally
        {
            Console.SetOut(original);
            Console.SetError(originalErr);
        }
    }

    private void Git(string args)
    {
        using var p = Process.Start(new ProcessStartInfo("git", args)
        {
            WorkingDirectory = _root,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })!;
        p.WaitForExit();
        Assert.True(p.ExitCode == 0, $"git {args} failed: {p.StandardError.ReadToEnd()}");
    }
}