namespace AgentsWatch.Core;

public enum RunMode
{
    InvestigationOnly,
    Implementation,
    Tests,
    DocsEvidence,
    DiffOnlyReview
}

public enum TokenBudget
{
    Low,
    Medium,
    High
}

public enum RiskLevel
{
    Low,
    Medium,
    High
}

public sealed record ScopeLimiter(
    IReadOnlyList<string> InspectOnly,
    IReadOnlyList<string> DoNotInspectUnlessRequired,
    IReadOnlyList<string> DoNotEdit);

public sealed record PromptOptimizationRequest(
    string RawPrompt,
    TokenBudget Budget = TokenBudget.Low,
    RunMode PreferredRunMode = RunMode.InvestigationOnly);

public sealed record PromptOptimizationResult(
    RiskLevel Risk,
    TokenBudget Budget,
    IReadOnlyList<string> WasteCauses,
    IReadOnlyList<string> SuggestedSplit,
    ScopeLimiter ScopeLimiter,
    string OptimizedPrompt);

public sealed record ChangedFile(
    string Path,
    string Status,
    int? AddedLines = null,
    int? DeletedLines = null);

public sealed record GitSnapshot(
    string Branch,
    string CommitSha,
    IReadOnlyList<ChangedFile> ChangedFiles,
    string RawStatus);

public sealed record RunReport(
    string TaskId,
    DateTimeOffset StartedAt,
    DateTimeOffset FinishedAt,
    GitSnapshot? StartSnapshot,
    GitSnapshot? EndSnapshot,
    RiskLevel Risk,
    IReadOnlyList<string> Validation,
    IReadOnlyList<string> Missed,
    IReadOnlyList<string> FollowUps);
