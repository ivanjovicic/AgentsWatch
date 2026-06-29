namespace AgentsWatch.Core;

public sealed class PromptRiskAnalyzer
{
    private static readonly string[] BroadScopeSignals =
    [
        "whole repo",
        "whole app",
        "entire repo",
        "entire app",
        "analyze everything",
        "fix everything",
        "refactor broadly"
    ];

    private static readonly string[] MultiModeSignals =
    [
        "investigate",
        "implement",
        "add tests",
        "review",
        "update docs"
    ];

    public PromptOptimizationResult Optimize(PromptOptimizationRequest request)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.RawPrompt);

        var prompt = request.RawPrompt.Trim();
        var lower = prompt.ToLowerInvariant();
        var causes = new List<string>();

        if (BroadScopeSignals.Any(lower.Contains))
        {
            causes.Add("broad scope");
        }

        var modeCount = MultiModeSignals.Count(lower.Contains);
        if (modeCount >= 3)
        {
            causes.Add("multiple task modes in one run");
        }

        if (!lower.Contains("scope") && !lower.Contains("inspect only"))
        {
            causes.Add("missing scope limiter");
        }

        if (!lower.Contains("stop"))
        {
            causes.Add("missing stop rules");
        }

        if (!lower.Contains("test") && !lower.Contains("validation"))
        {
            causes.Add("missing validation rule");
        }

        var risk = causes.Count switch
        {
            >= 3 => RiskLevel.High,
            1 or 2 => RiskLevel.Medium,
            _ => RiskLevel.Low
        };

        var budget = risk == RiskLevel.High ? TokenBudget.Low : request.Budget;
        var split = BuildSuggestedSplit(risk);
        var scopeLimiter = new ScopeLimiter(
            InspectOnly: ["<fill exact relevant folders/files>"],
            DoNotInspectUnlessRequired: ["auth/session", "migrations", "unrelated features"],
            DoNotEdit: ["generated files", "unrelated providers/services"]);

        return new PromptOptimizationResult(
            risk,
            budget,
            causes,
            split,
            scopeLimiter,
            BuildOptimizedPrompt(prompt, budget, risk));
    }

    private static IReadOnlyList<string> BuildSuggestedSplit(RiskLevel risk)
    {
        if (risk == RiskLevel.Low)
        {
            return ["single focused implementation run with targeted validation"];
        }

        return
        [
            "001-investigate-only.md",
            "002-implement-minimal-fix.md",
            "003-add-targeted-tests.md",
            "004-diff-only-review.md"
        ];
    }

    private static string BuildOptimizedPrompt(string rawPrompt, TokenBudget budget, RiskLevel risk)
    {
        var mode = risk == RiskLevel.High ? "investigation-only" : "implementation";
        return $"""
Run mode: {mode}
Token budget: {budget.ToString().ToLowerInvariant()}

Task:
{rawPrompt}

Scope limiter:
Inspect only:
- <fill exact relevant folders/files>

Do not inspect unless required:
- unrelated features
- auth/session
- migrations

Do not edit:
- generated files
- unrelated providers/services

Stop rules:
- stop if root cause is unknown after initial scan
- stop if file budget is exceeded
- stop if another subsystem becomes involved

Return:
- root cause or change summary
- exact files involved
- validation to run
- missed work and follow-up
""";
    }
}
