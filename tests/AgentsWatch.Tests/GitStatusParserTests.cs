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

    [Fact]
    public void ParsePorcelainZ_ReturnsEmpty_ForCleanStatus()
    {
        Assert.Empty(GitStatusParser.ParsePorcelainZ(string.Empty));
    }

    [Fact]
    public void ParsePorcelainZ_ParsesAllEntryKinds_WithoutCorruptingPaths()
    {
        var status = string.Join('\0',
            " M README.md",
            "M  src/staged.cs",
            "A  docs/new file.md",
            " D removed.txt",
            "D  staged-removed.txt",
            "?? my notes/todo list.txt",
            "R  new name.cs",
            "old name.cs",
            "MM both.cs") + "\0";

        var result = GitStatusParser.ParsePorcelainZ(status);

        Assert.Equal(8, result.Count);
        Assert.Equal(("README.md", "M"), (result[0].Path, result[0].Status));
        Assert.Equal(("src/staged.cs", "M"), (result[1].Path, result[1].Status));
        Assert.Equal(("docs/new file.md", "A"), (result[2].Path, result[2].Status));
        Assert.Equal(("removed.txt", "D"), (result[3].Path, result[3].Status));
        Assert.Equal(("staged-removed.txt", "D"), (result[4].Path, result[4].Status));
        Assert.Equal(("my notes/todo list.txt", "??"), (result[5].Path, result[5].Status));
        Assert.Equal(("new name.cs", "R", "old name.cs"), (result[6].Path, result[6].Status, result[6].OriginalPath));
        Assert.Equal(("both.cs", "MM"), (result[7].Path, result[7].Status));
        Assert.All(result.Where(r => r.Status != "R"), r => Assert.Null(r.OriginalPath));
    }

    [Fact]
    public void ParsePorcelainZ_PreservesLeadingAndTrailingSpacesInPath()
    {
        var result = GitStatusParser.ParsePorcelainZ("??  spaced .txt \0");

        Assert.Equal(" spaced .txt ", Assert.Single(result).Path);
    }

    [Fact]
    public void Parse_LineFormat_HandlesRenameArrowAndCrLf()
    {
        var result = GitStatusParser.Parse("R  old.cs -> new.cs\r\n M a b.txt\r\n");

        Assert.Equal(("new.cs", "R", "old.cs"), (result[0].Path, result[0].Status, result[0].OriginalPath));
        Assert.Equal(("a b.txt", "M"), (result[1].Path, result[1].Status));
    }
}