# AW-VFY-010-B3 — Next small dogfood batch

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Parent: `docs/prompts/AW-VFY-010-dogfood-30-receipts.md`  
Status: Ready after `AW-VFY-010-LEDGER`  
Depends on: completed sanitized ledger at `docs/evidence/aw-vfy-010-ledger.md`  
Run mode: small real dogfood batch + honest evidence; targeted fix only if a
high-confidence defect invalidates the next receipt  
Budget: one small batch (5–7 receipts max); do not run the rest of the 30

## Purpose

Execute the next small AW-VFY-010 batch against gaps named by the ledger.
Measure current-main behavior after PRs #11, #12 and #13. Do not treat this
batch as completing the parent 30-receipt program.

## Dependency

If `docs/evidence/aw-vfy-010-ledger.md` does not exist, STOP and run
`AW-VFY-010-LEDGER` first.

The ledger wins when it names specific remaining slots. The default slot list
below is only a fallback for gaps the ledger still shows as open.

## Product SHA

Start each receipt from current `origin/main` (expected post-PR #13:
`edbbbe4a3e2962a8b20f7536d4a106420ff8492f` unless a later approved merge moved
main). Rebuild the CLI used for the run from that SHA. Do not reuse a stale
CLI from an older worktree.

## Trust-boundary rules

- Write independent expected outcomes **before** reading AgentsWatch output.
- Manual `Supported` => `AWE009` / `NeedsReview`. Never claim independently
  verified `Done` from a human-typed `Supported` line.
- `--accept-validation` is not a Done certification.
- `Fail` validation must not yield `Done` (AWE003 family).
- Missing mandatory validation must not yield `Done`.
- Dirty-at-start unchanged files are not run-interval attribution.
- Finish summaries and stored receipts must not leak invented sentinel secrets.
  Use a fresh `AWSENTINEL-TOKEN-…` value never used before. Known already-used
  suffixes include `c0ffee99deadbeef41`, `fake123`, `7f3a9c2e1b48`,
  `e4c81b0a72d69911`, and `91d3f0b86a47cc22` — do not reuse them.
- Do not copy private source into AgentsWatch artifacts.
- Do not overwrite historical receipts or prior local evidence files.

## Default slots if the ledger still lists them as open

Keep the batch to at most these classes (skip any the ledger already covers
with current-main semantics):

1. **Docs-only / unknown acceptance** on AgentsWatch itself: expect
   `NeedsEvidence` (or clearer missing-evidence block), not `Done`.
2. **Unrelated passing validation + manual `Supported`** (N2-class): expect
   `NeedsReview` and `AWE009`. This is a true negative for silent `Done`, not
   a product failure.
3. **Structured `Fail` validation**: expect not-`Done` (typically `Failed` /
   AWE003). Record TP if the gate holds.
4. **Dirty-unchanged file plus a later allowed edit**: the unchanged dirty
   file must not be attributed as a run-interval change; allowed interval
   edits may be attributed or marked ambiguous per current rules (`AWD004`
   when a path is or will be `[redacted]`).
5. **Optional privacy regression** only if the ledger does not already count
   the post-#13 finish-output check: one real finish with a new invented
   sentinel; stored artifacts and finish stdout/stderr must show 0 hits.
6. **Optional docs-owned scoped change** that stays `NeedsReview` after
   manual `Supported` (do not “force Done”).

Do not include Flutter / MathLearning-Mobile #5/#6 unless the owner has
explicitly lifted that HOLD.
Do not start a second parallel batch or a third full dogfood tour.

## Owned paths

- Local run worktrees created for this batch (outside published product
  checkouts the owner asked not to disturb)
- New local evidence: `.ai/runs/<date>-aw-vfy-010-b3-evidence.md`
- Optional sanitized public summary:
  `docs/evidence/<date>-aw-vfy-010-b3-summary.md`
- Targeted runtime fix branch only if a newly observed high-confidence defect
  would invalidate the next receipt (separate small PR; do not mix with docs
  inventory work)

## Avoid paths

- Historical receipts and prior `.ai/runs/` originals
- Unrelated feature work, dashboard/SaaS/MCP/learning engines
- Production SQL, deploy, direct push to `main`, merges without owner approval
- Cloud agents
- Open PRs #4, #5, #6, #8, #9
- Changing `docs/prompt_queues/` to claim AW-VFY-010 complete

## Work steps

1. Read the ledger, parent prompt, `DECISION_ORDERING.md`, and
   `RECEIPT_REDACTION.md`.
2. Confirm `origin/main` SHA and rebuild the CLI.
3. For each slot: create a clean dedicated worktree, write expected outcomes,
   start, perform only the intended action, finish, adjudicate TP/FP/TN/FN/Unknown.
4. After the small batch, review false findings, attribution ambiguity, and
   user friction. Fix only a high-confidence defect that would invalidate the
   next receipt.
5. Write local evidence and, if anything is published, a sanitized summary.
6. Update the ledger with the new rows (append; do not rewrite old rows except
   to add a “superseded by later check” note).

## Acceptance criteria

- Ledger existed first.
- At most 5–7 new real receipts/checks, each with pre-declared expected outcomes.
- No invented counts or customer feedback.
- No independently verified `Done` claimed from manual `Supported`.
- Historical evidence files unchanged.
- Parent AW-VFY-010 remains In progress unless the parent exit gate is
  genuinely met (it will not be met by B3 alone).
- Next remaining 010 batch, if any, is named from the updated ledger.

## Expected evidence

- `.ai/runs/<date>-aw-vfy-010-b3-evidence.md`
- optional sanitized `docs/evidence/<date>-aw-vfy-010-b3-summary.md`
- updated `docs/evidence/aw-vfy-010-ledger.md`
- reproduction notes (commands, SHA, worktree names) without private source

## STOP conditions

- Ledger missing.
- Expected `origin/main` SHA mismatch without owner authorization.
- Pressure to mark AW-VFY-010 complete.
- Pressure to produce independently verified `Done` via `--accept-validation`
  or manual `Supported`.
- Need to unstick Flutter/Mobile #5/#6 without owner approval.
- Need to merge, deploy, run production SQL, or launch a cloud agent.
- Discovery of a silent false-`Done` on current main: stop the batch, record
  the finding, and open a targeted corrective prompt instead of continuing
  the tour.
