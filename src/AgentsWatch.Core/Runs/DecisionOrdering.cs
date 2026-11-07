using AgentsWatch.Core.Contracts;

namespace AgentsWatch.Core.Runs;

/// <summary>
/// Single total ordering for Evidence Gate decisions.
/// Classification inspects the finished finding set and is independent of the
/// order in which checks appended those findings.
/// </summary>
public static class DecisionOrdering
{
    /// <summary>
    /// First matching class wins:
    /// 1. Failed — any AWE003 (validation Fail, TimedOut, or Killed).
    /// 2. Blocked — any AWE004 (blocked by environment).
    /// 3. NeedsApproval — any AWE008.
    /// 4. NeedsReview — scope-drift errors (AWD001/AWD002/AWD003), unsupported claims (AWK001),
    ///    unsupported acceptance (AWE007 with Unsupported status), unverified Supported acceptance (AWE009),
    ///    or attribution ambiguity (AWR002 / AWD004).
    ///    This class outranks unknown acceptance (AWE007/Unknown) so drift is not hidden behind NeedsEvidence.
    /// 5. NeedsEvidence — incomplete receipt (AWE001), missing/unknown mandatory validation (AWE002),
    ///    missing expected evidence (AWE006), unknown acceptance (AWE007/Unknown), or unknown claims (AWK002).
    /// 6. Done — no blocking findings.
    /// Historical receipts with unknown acceptance stay NeedsEvidence (or another non-Done class).
    /// They never become Done merely because this ordering changed.
    /// </summary>
    public const string TotalOrder = "Failed > Blocked > NeedsApproval > NeedsReview > NeedsEvidence > Done";

    public static DecisionStatus Classify(IReadOnlyList<Finding> findings)
    {
        findings ??= [];
        if (findings.Any(f => f.RuleId == EvidenceRules.ValidationFailed))
        {
            return DecisionStatus.Failed;
        }

        if (findings.Any(f => f.RuleId == EvidenceRules.ValidationBlocked))
        {
            return DecisionStatus.Blocked;
        }

        if (findings.Any(f => f.RuleId == EvidenceRules.ApprovalRequired))
        {
            return DecisionStatus.NeedsApproval;
        }

        if (findings.Any(IsNeedsReviewFinding))
        {
            return DecisionStatus.NeedsReview;
        }

        if (findings.Any(IsNeedsEvidenceFinding))
        {
            return DecisionStatus.NeedsEvidence;
        }

        return DecisionStatus.Done;
    }

    public static bool IsNeedsReviewFinding(Finding f) =>
        f.RuleId is ScopeDriftRules.OutsideOwned or ScopeDriftRules.AvoidTouched or ScopeDriftRules.RenameCrosses
            or ClaimRules.Unsupported
        || (f.RuleId == EvidenceRules.AcceptanceUnsupported && f.Status == FindingStatus.Unsupported)
        || f.RuleId == EvidenceRules.AcceptanceUnverified
        || f.RuleId is "AWR002" or ScopeDriftRules.AmbiguousScope
        || f.Category == FindingCategory.AttributionAmbiguous;

    public static bool IsNeedsEvidenceFinding(Finding f) =>
        f.RuleId is EvidenceRules.ReceiptIncomplete or EvidenceRules.MandatoryMissing or EvidenceRules.ExpectedEvidenceMissing
            or ClaimRules.Unknown
        || (f.RuleId == EvidenceRules.AcceptanceUnsupported && f.Status == FindingStatus.Unknown);

    public static bool ContributesToReasons(Finding f) =>
        f.RuleId.StartsWith("AWE", StringComparison.Ordinal)
        || f.RuleId.StartsWith("AWD", StringComparison.Ordinal)
        || f.RuleId.StartsWith("AWK", StringComparison.Ordinal)
        || f.RuleId.StartsWith("AWR", StringComparison.Ordinal);

    public static IReadOnlyList<Finding> SortedForDisplay(IReadOnlyList<Finding> findings) =>
        (findings ?? [])
            .OrderBy(f => f.Severity == FindingSeverity.Error ? 0 : 1)
            .ThenBy(f => f.RuleId, StringComparer.Ordinal)
            .ThenBy(f => f.FindingId, StringComparer.Ordinal)
            .ThenBy(f => f.Message, StringComparer.Ordinal)
            .ToArray();

    public static string FormatFinding(Finding f) =>
        $"{f.RuleId}: {f.Message} [{string.Join(", ", f.EvidenceRefs)}]";

    public static IReadOnlyList<string> Reasons(IReadOnlyList<Finding> findings, DecisionStatus status, string doneFallback)
    {
        var formatted = SortedForDisplay(findings).Where(ContributesToReasons).Select(FormatFinding).ToList();
        var ambiguous = (findings ?? []).Count(f =>
            f.Category == FindingCategory.AttributionAmbiguous
            || f.RuleId is "AWR002" or ScopeDriftRules.AmbiguousScope);
        if (ambiguous > 0)
        {
            formatted.Add($"AWR002: {ambiguous} ambiguous change(s) need review [runDelta.attributionAmbiguities]");
        }

        if (formatted.Count == 0)
        {
            formatted.Add(status == DecisionStatus.Done
                ? doneFallback
                : $"{status}: see findings");
        }

        return formatted;
    }

    public static IReadOnlyList<string> BlockingReasons(IReadOnlyList<Finding> findings, DecisionStatus status)
    {
        return SortedForDisplay(findings).Where(f => MatchesDecisionClass(f, status)).Select(FormatFinding).ToList();
    }

    public static bool MatchesDecisionClass(Finding f, DecisionStatus status) => status switch
    {
        DecisionStatus.Failed => f.RuleId == EvidenceRules.ValidationFailed,
        DecisionStatus.Blocked => f.RuleId == EvidenceRules.ValidationBlocked,
        DecisionStatus.NeedsApproval => f.RuleId == EvidenceRules.ApprovalRequired,
        DecisionStatus.NeedsReview => IsNeedsReviewFinding(f),
        DecisionStatus.NeedsEvidence => IsNeedsEvidenceFinding(f),
        _ => false
    };

    public static string SeverityCountLabel(IReadOnlyList<Finding> findings)
    {
        findings ??= [];
        var errors = findings.Count(f => f.Severity == FindingSeverity.Error);
        var warnings = findings.Count(f => f.Severity == FindingSeverity.Warning);
        return $"error={errors}, warning={warnings}";
    }

    public static RunDecision Decide(IReadOnlyList<Finding> findings, string doneFallback)
    {
        var status = Classify(findings);
        return new RunDecision(status, Reasons(findings, status, doneFallback));
    }
}
