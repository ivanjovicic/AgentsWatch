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
delta / receipt / evidence-gate / scope / claims spine. Use it to judge later
dogfood batches and any schema change. This prompt does not execute the
30-receipt dogfood and does not mark AW-VFY-010 complete.

Earlier drafts of this prompt said the protocol had to finish before AW-VFY-010
started. That timing is obsolete: AW-VFY-010 is already in progress. Align the
protocol to the code now on `main` and to remaining ledger/B3 work.

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

## Work steps

1. Read current `docs/DATA_MODEL.md`, `docs/COMMAND_CONTRACTS.md`,
   `docs/DECISION_ORDERING.md`, `docs/RECEIPT_REDACTION.md`, and the parent
   AW-VFY-010 adversarial list.
2. Catalog predicates the product actually implements (decision codes, status
   promotions that are illegal, attribution classes, redaction surfaces).
3. Map each parent-prompt adversarial case and the N2-class unrelated
   validation case to an expected decision/code under current semantics.
4. Note which cases LEDGER/B3 already cover versus which remain protocol-only
   until a real run exists. Do not invent run results.
5. List schema stability rules: any future runtime change needs targeted tests
   and must not silently reintroduce unverified `Done`.

## Acceptance criteria

- Protocol document exists and matches current `main` behavior, including
  AWE009 / NeedsReview for manual `Supported`.
- No duplicated next-task instructions that compete with the router.
- No invented dogfood outcomes.
- Explicit statement that this work does not complete AW-VFY-010.

## STOP conditions

- Need to change runtime code to make the protocol look finished.
- Need to mark AW-VFY-010 complete.
- Conflict with `DECISION_ORDERING.md` or `RECEIPT_REDACTION.md` that would
  require guessing; stop and report the conflict.
