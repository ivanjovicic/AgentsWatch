using System.Runtime.InteropServices;
using Xunit;

namespace AgentsWatch.Tests;

/// <summary>A fact that needs a Unix filesystem feature; reported as Skipped (not passed) on Windows.</summary>
public sealed class UnixOnlyFactAttribute : FactAttribute
{
    public UnixOnlyFactAttribute()
    {
        if (OperatingSystem.IsWindows())
        {
            Skip = "Unix-only: needs a Unix filesystem feature (e.g. mkfifo).";
        }
    }
}

internal sealed class TempDir : IDisposable
{
    public TempDir()
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "aw-test-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path);
    }

    public string Path { get; }

    public void Dispose()
    {
        try
        {
            Directory.Delete(Path, true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}

internal static class PosixNative
{
    [DllImport("libc", SetLastError = true)]
    public static extern int mkfifo(string path, uint mode);

    [DllImport("libc", EntryPoint = "link", SetLastError = true)]
    private static extern int PosixLink(string oldPath, string newPath);

    [DllImport("kernel32.dll", EntryPoint = "CreateHardLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CreateHardLinkW(string newFileName, string existingFileName, IntPtr securityAttributes);

    public static bool TryCreateHardLink(string link, string target) =>
        OperatingSystem.IsWindows() ? CreateHardLinkW(link, target, IntPtr.Zero) : PosixLink(target, link) == 0;
}
