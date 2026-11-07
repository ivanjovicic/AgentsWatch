# AW-QA-001 — Receipt and evidence-gate quality protocol

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track, not a second runtime queue)  
Status: Parallel support; not the next claimable task  
Depends on: AW-VFY-003..009 implemented on `origin/main` (schema is no longer pending)  
Does not replace: `AW-VFY-010-LEDGER` or `AW-VFY-010-B3`  
Run mode: documentation / protocol / fixture catalog  
Budget: medium

## Purpose

Write a reviewable quality protocol for the current RunContract / baseline /
delta / receipt / evidence-gate / scope / claims spine so later dogfood
batches are rigorous rather than anecdotal. This prompt does not execute the
30-receipt dogfood and does not mark AW-VFY-010 complete.

Earlier drafts said the protocol had to finish before AW-VFY-010 started.
That timing is obsolete: AW-VFY-010 is already in progress. Align the protocol
to the code now on `main` and to remaining ledger/B3 work.

## Current semantics that the protocol must encode

- Evidence Gate order: Failed > Blocked > NeedsApproval > NeedsReview >
  NeedsEvidence > Done (`docs/DECISION_ORDERING.md`).
- Manual `Supported` is `AWE009` / `AcceptanceUnverified` / `NeedsReview`.
  There is no independently verified `Done` path from human-typed `Supported`.
- `--accept-validation` binds a command to a criterion. It does not certify
  `Done`.
- `Fail` must not become `Done`.
- Missing mandatory validation must not become `Done`.
- JSON is canonical; Markdown is a projection.
- Receipt persistence uses `ReceiptRedaction` (JSON string-value walker;
  `evidenceDigest` is sha256 of canonical JSON after redaction). Start
  baselines are not a redacted persistence surface.
- Finish CLI must render from `result.Receipt`, not a raw unredacted record
  (`docs/RECEIPT_REDACTION.md`). Residual documented limits: unexpected
  `Program.Main` exceptions; artifact paths outside the workspace may not be
  repo-relative.
- `AWD004` when a scoped path is or will be stored as `[redacted]`.
- `ValidationSource.CI` supplied by a human is not authenticated CI provenance.

Use the same TP/FP/FN/TN positive class as the historical Batch 2 matrix:
**unsafe completion / violation requiring intervention**. A correctly blocked
N2-class case is TP, not TN. See `docs/prompts/AW-VFY-010-LEDGER.md`.

## Adversarial and measurement requirements (restored from PR #9)

Include all of these must-catch cases in the protocol catalog, mapped to
expected current-main decisions/codes: pre-existing dirty unchanged; dirty
changed-further; existing/new untracked; add/delete/rename; same-path
staged+unstaged; branch/HEAD switch; concurrent writer ambiguity; missing test
run; test failure; CI evidence for wrong SHA; blocked environment; out-of-scope
path; avoid-path touch; false DocsOnly; false TestsAdded; false
ValidationPassed; unsupported broad BugFixed claim; N2-class unrelated passing
validation + manual Supported.

Specify for every protocol slot: baseline repo state, RunContract requirements,
agent task, expected machine evidence, expected run decision, privacy/cleanup
controls, independent reviewer judgment, and baseline workflow result
(Git+CI+PR+native/Qodo if available).

Define measurement: high-confidence true/false findings, material attribution
errors, Unknown/Ambiguous, receipt capture overhead, reviewer minutes used or
saved, changed decision, setup friction and repeat use. Missing measurement is
Unknown, not success. Do not invent token savings or claim reviewer benefit
was measured when it was not.

Specify failure severity: any material false attribution or unsupported Done
is release-blocking; benign ambiguity must not be counted as a false author
claim. Include a 5–10 run batch review cadence, independent adjudication of
disagreements and a stop rule for excessive false-positive burden. Separate
product failures from fixture mistakes.

Use disposable branches/temporary repos and synthetic files for deliberate
failures. No destructive operations in Trendplus/MathLearning or production
data. Label each slot REAL / FIXTURE / NEGATIVE_CONTROL so it cannot be
confused with the 30-REAL dogfood denominator owned by LEDGER/B3.

A 30-row protocol matrix is a design aid for later batches. It does not
execute AW-VFY-010 and does not invent run results.

## Owned paths

- `docs/qa/RECEIPT_EVIDENCE_PROTOCOL.md` (create if absent)
- optional `docs/qa/fixtures/` markdown or JSON examples that contain only
  invented data
- a pointer row in `docs/prompt_queues/SUPPORT_PROMPT_COVERAGE.md` if status
  changes

## Avoid paths

- `src/**` and `tests/**` unless a later separate runtime PR is explicitly
  authorized; this prompt itself is protocol work
- rewriting historical receipts or dogfood evidence
- executing AW-VFY-010 batches (LEDGER/B3 own that)
- claiming customer findings or independently verified `Done`
- creating another active queue

## Acceptance criteria

- Protocol document exists and matches current `main` behavior, including
  AWE009 / NeedsReview for manual `Supported`.
- The restored adversarial catalog and measurement rubric are present.
- No duplicated next-task instructions that compete with the router.
- No invented dogfood outcomes.
- Explicit statement that this work does not complete AW-VFY-010.

## STOP conditions

- Need to change runtime code to make the protocol look finished.
- Need to mark AW-VFY-010 complete.
- Conflict with `DECISION_ORDERING.md` or `RECEIPT_REDACTION.md` that would
  require guessing; stop and report the conflict.
