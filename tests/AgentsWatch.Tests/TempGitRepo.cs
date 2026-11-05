using System.Diagnostics;
using Xunit;

namespace AgentsWatch.Tests;

internal sealed class TempGitRepo : IDisposable
{
    public string Root { get; } = Path.Combine(Path.GetTempPath(), "aw-repo-" + Guid.NewGuid().ToString("N"));

    public TempGitRepo(bool commit = true)
    {
        Directory.CreateDirectory(Root);
        Git("init -q -b main");
        Git("config user.email t@example.com");
        Git("config user.name t");
        Git("config commit.gpgsign false");
        Git("config core.autocrlf false");
        if (commit)
        {
            Write("a.txt", "a");
            Write("b file.txt", "b");
            Git("add -A");
            Git("commit -q -m init");
        }
    }

    public void Write(string path, string content)
    {
        var full = Path.Combine(Root, path);
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        File.WriteAllText(full, content);
    }

    public string Git(string args)
    {
        using var p = Process.Start(new ProcessStartInfo("git", args)
        {
            WorkingDirectory = Root,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })!;
        var stdout = p.StandardOutput.ReadToEnd();
        var stderr = p.StandardError.ReadToEnd();
        p.WaitForExit();
        Assert.True(p.ExitCode == 0, $"git {args} failed: {stderr}");
        return stdout.Trim();
    }

    public void Dispose()
    {
        try
        {
            foreach (var f in Directory.GetFiles(Root, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(f, FileAttributes.Normal);
            }

            Directory.Delete(Root, true);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }
}