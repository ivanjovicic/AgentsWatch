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
