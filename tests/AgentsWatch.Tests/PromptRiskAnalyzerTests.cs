using AgentsWatch.Core;
using Xunit;

namespace AgentsWatch.Tests;

public sealed class PromptRiskAnalyzerTests
{
    [Fact]
    public void Optimize_ReturnsHighRisk_ForBroadMultiModePrompt()
    {
        var analyzer = new PromptRiskAnalyzer();

        var result = analyzer.Optimize(new PromptOptimizationRequest(
            "Analyze the whole repo, implement the fix, add tests, update docs, and review everything."));

        Assert.Equal(RiskLevel.High, result.Risk);
        Assert.Equal(TokenBudget.Low, result.Budget);
        Assert.Contains("broad scope", result.WasteCauses);
        Assert.Contains("multiple task modes in one run", result.WasteCauses);
        Assert.Contains("001-investigate-only.md", result.SuggestedSplit);
    }

    [Fact]
    public void Optimize_ReturnsLowRisk_ForScopedPromptWithValidation()
    {
        var analyzer = new PromptRiskAnalyzer();

        var result = analyzer.Optimize(new PromptOptimizationRequest(
            "Fix one widget overflow. Scope: inspect only lib/widgets/foo.dart. Stop if another subsystem is involved. Run widget test validation."));

        Assert.Equal(RiskLevel.Low, result.Risk);
        Assert.Contains("single focused implementation run with targeted validation", result.SuggestedSplit);
    }
}
