using AgentsWatch.Core.Paths;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class RepositoryPathTests
{
    [Theory]
    [InlineData(@"src\foo.cs", "src/foo.cs")]
    [InlineData("src//foo.cs", "src/foo.cs")]
    [InlineData("./src/foo.cs", "src/foo.cs")]
    [InlineData("src/foo.cs/", "src/foo.cs")]
    [InlineData(@"src\**", "src/**")]
    public void Normalize_UnifiesSeparatorsAndDots(string input, string expected) =>
        Assert.Equal(expected, RepositoryPath.Normalize(input));

    [Theory]
    [InlineData("src/**", "src/foo.cs")]
    [InlineData("src/**", "src/a/b.cs")]
    [InlineData(@"src\**", @"src\foo.cs")]
    [InlineData("src/**", "src\\nested\\x.cs")]
    [InlineData("**", "anything/here.txt")]
    [InlineData("docs/**/*.md", "docs/a.md")]
    [InlineData("docs/**/*.md", "docs/guide/a.md")]
    [InlineData("**/Program.cs", "Program.cs")]
    [InlineData("**/Program.cs", "src/AgentsWatch.Cli/Program.cs")]
    [InlineData("src/*.cs", "src/Foo.cs")]
    [InlineData("src/?.cs", "src/a.cs")]
    public void Matches_ExpectedGlobs(string pattern, string path) =>
        Assert.True(RepositoryPath.Matches(pattern, path), pattern + " vs " + path);

    [Theory]
    [InlineData("src/*.cs", "src/a/b.cs")]
    [InlineData("src/**", "tests/foo.cs")]
    [InlineData("src/?.cs", "src/ab.cs")]
    [InlineData("docs/**/*.md", "docs/a.txt")]
    [InlineData("Src/**", "src/foo.cs")]
    public void Matches_RejectsNonMatches(string pattern, string path) =>
        Assert.False(RepositoryPath.Matches(pattern, path), pattern + " vs " + path);

    [Fact]
    public void FirstMatchingPattern_ReturnsFirstOwnedHit()
    {
        Assert.Equal("src/**", RepositoryPath.FirstMatchingPattern(["docs/**", "src/**", "src/foo.cs"], "src/foo.cs"));
        Assert.Null(RepositoryPath.FirstMatchingPattern(["docs/**"], "src/foo.cs"));
        Assert.Null(RepositoryPath.FirstMatchingPattern([], "src/foo.cs"));
    }
}
