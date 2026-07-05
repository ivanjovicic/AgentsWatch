using AgentsWatch.Core;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RunArtifactPathTests
{
    [Theory]
    [InlineData(".agentwatch/runs/TASK-001.json", true)]
    [InlineData(".agentwatch\\runs\\OLDER.json", true)]
    [InlineData(".ai/runs/TASK-001.md", true)]
    [InlineData(".ai\\runs\\OLDER.md", true)]
    [InlineData(".agentwatch/runs/nested/TASK.json", false)]
    [InlineData(".agentwatch/runs/TASK.txt", false)]
    [InlineData(".ai/runs/TASK.json", false)]
    [InlineData("src/TASK.md", false)]
    public void IsManagedArtifact_recognizes_only_top_level_run_outputs(string path, bool expected)
    {
        Assert.Equal(expected, RunArtifactPaths.IsManagedArtifact(path));
    }
}
