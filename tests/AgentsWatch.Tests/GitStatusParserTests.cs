using AgentsWatch.Git;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class GitStatusParserTests
{
    [Fact]
    public void Parse_ReturnsEmptyList_ForCleanStatus()
    {
        var result = GitStatusParser.Parse(string.Empty);

        Assert.Empty(result);
    }

    [Fact]
    public void Parse_ParsesModifiedAndUntrackedFiles()
    {
        const string status = """
 M README.md
?? src/AgentsWatch.Cli/Program.cs
A  docs/PRODUCT_SPEC.md
""";

        var result = GitStatusParser.Parse(status);

        Assert.Collection(
            result,
            first =>
            {
                Assert.Equal("README.md", first.Path);
                Assert.Equal("M", first.Status);
            },
            second =>
            {
                Assert.Equal("src/AgentsWatch.Cli/Program.cs", second.Path);
                Assert.Equal("??", second.Status);
            },
            third =>
            {
                Assert.Equal("docs/PRODUCT_SPEC.md", third.Path);
                Assert.Equal("A", third.Status);
            });
    }
}
