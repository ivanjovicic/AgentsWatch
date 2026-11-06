# AW-VFY-010 corrective PR evidence — acceptance CLI, decision order, receipt banner

Date: 2026-11-06 (Europe/Belgrade)
Branch: `fix/aw-accept-priority-banner` (worktree `C:\Users\Ivan\source\repos\_aw-vfy-010\fix-accept-priority-banner`, created from `origin/main` `ada2fcb8674aa7f9b2b34692f69a427d521615f7`)
Existing checkouts were not reset. No cloud agents. No deploy. No merge.

## Local gate

- `dotnet restore` — pass
- `dotnet build -c Release` — pass, 0 warnings, 0 errors
- `dotnet test -c Release` — **173 passed / 0 failed / 0 skipped** (was 164 on `ada2fcb`; +9 tests in `DecisionOrderingAndAcceptTests`)

## What changed

1. Public CLI `agentswatch accept record <run-id> --id <acN> --status supported|not-supported|unknown --evidence <validation-id>`.
   - Supported requires a recorded Pass validation (exit 0 or no exit code). Evidence-free Supported is rejected.
   - NotSupported requires a recorded validation id.
   - Unknown may omit `--evidence`.
   - Uses the same digest/integrity load path as evidence/claims; tampered receipts are rejected and not rewritten.
2. Deterministic total order in `DecisionOrdering` (documented in `docs/DECISION_ORDERING.md`):
   **Failed > Blocked > NeedsApproval > NeedsReview > NeedsEvidence > Done**
   NeedsReview (AWD001/002/003, AWK001, unsupported AWE007, attribution ambiguity) outranks unknown acceptance.
3. Receipt markdown, handoff, and CLI decision print show the decision, top blocking reasons for that class, and `error=`/`warning=` finding counts.

## Old-receipt impact

Re-applying `EvidenceGate.Apply` does **not** mark historical Unknown acceptance as Supported and does **not** turn historical NeedsEvidence receipts into Done. A dedicated test (`HistoricalNeedsEvidence_DoesNotAutoConvertToDone_WhenAcceptanceStaysUnknown`) locks that. Fail/TimedOut/Blocked/NeedsApproval and invalid receipts stay non-Done.

Original AW-VFY-010 batch-1 receipts (#1–#4, #7, #8) were not modified. A read-only replay is in `.ai/runs/2026-11-06-aw-vfy-010-batch1-replay-under-new-rules.md` (clearly labeled, not a new real run).

Replay summary: #1, #2, #3, #4, #8 stay NeedsEvidence (not Done). #7 changes NeedsEvidence → NeedsReview because AWD/AWK errors now outrank AWE007/Unknown. No replay row became Done.

## Remaining gaps

- Acceptance is still an explicit CLI mark linked to validation evidence, not an automatic semantic evaluator of criterion text.
- Happy-path Done now exists via `accept record` after passing evidence; batch-1 originals were intentionally left Unknown.
- #5/#6 Flutter tasks remain on hold and were not part of this PR.
- No production deploy and no merge without owner approval.

---

# Review follow-up (PR #11 review 5805629005) — P1 stale AWK001 + P2 Supported reason

Date: 2026-11-06 (Europe/Belgrade)
Parent SHA before this follow-up: `4fe216491a0cdf05416f2cd9bc398c7b3177aac6`
Scope: only these two review fixes. No new features, no deploy, no merge. PR stays draft. No cloud agents.

## Local gate (this follow-up)

- Negative tests (Fail / TimedOut / Blocked / NeedsApproval / tampered or missing digest): 8 passed / 0 failed
  - `ValidationFail_IsFailed`
  - `TimedOut_CannotProduceDone`
  - `BlockedByEnvironment_IsBlocked_NeverPass`
  - `ApprovalRequired_WithoutOverride_IsNeedsApproval`
  - `AcceptRecord_RejectsMissingValidation_FailedValidation_AndTamperedReceipt`
  - `MissingAcceptanceStatus_WithStaleDigest_CannotProduceDoneOrRewrite`
  - `MissingReceiptEvidenceDigest_CannotReblessTamperedAcceptanceOrRewrite`
  - `DecisionClass_IsIndependentOfFindingOrder`
- `dotnet restore` — pass
- `dotnet build -c Release` — pass, 0 warnings, 0 errors
- `dotnet test -c Release` — **176 passed / 0 failed / 0 skipped** (was 173 on `4fe2164`; +3 tests)

## What changed

P1. Stale `AWK001` no longer depends on command order.
- Regression first: `claims record` → `evidence record` → `accept record` → `evidence check`, then a subsequent `claims check` must not change the decision when evidence did not change (`ClaimsBeforeEvidence_ThenAcceptAndEvidenceCheck_DoesNotKeepStaleUnsupportedClaim`). Reverse order is covered too.
- `EvidenceGate.Apply` now re-evaluates recorded claims from current evidence (`ClaimVerifier.Evaluate`) and persists the refreshed `Claims` before `DecisionOrdering.Decide`. Prior `AWK` findings are replaced by that fresh evaluation, not dropped without a re-check.
- `ClaimsService.Persist` delegates to the same shared `Apply` path (no separate AWK strip-only path).

P2. `Supported` without a reason is rejected.
- Non-empty `--reason` is required for `Supported` (whitespace-only rejected; `AcceptRecord_SupportedWithoutReason_IsRejected`).
- The receipt stores a manual assessment (declared, not attested), the supplied reason, and the declared evidence ids.
- The same provenance is shown in the CLI output and decision banner (markdown + CLI).
- Docs (`DECISION_ORDERING.md`, `CLI_SPEC.md`, `COMMAND_CONTRACTS.md`) and `accept` usage text match.

## Safety unchanged

Fail / TimedOut / Blocked / NeedsApproval / invalid receipts still cannot become Done and are not rewritten on digest failure. Historical Unknown acceptance still does not auto-convert to Done. Original batch-1 receipts were not modified.

## Remaining gaps (honest)

- Linking a passing validation to a criterion is still a declared/manual judgment. AgentsWatch does not prove that the validation is semantically about that criterion text.
- `Supported` provenance is encoded in the stored reason string (no new receipt schema field) so older receipts keep deserializing.
- Incomplete-receipt early return in `EvidenceGate.Apply` still skips claim refresh (NeedsEvidence; `RunDelta` may be null).
- #5/#6 Flutter tasks remain on hold. No production deploy. No merge without owner OK.
