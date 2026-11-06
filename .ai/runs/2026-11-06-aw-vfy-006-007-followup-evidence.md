# AW-VFY-006/007 follow-up evidence

- Prompt: follow-up to review `.ai/runs/2026-11-06-aw-vfy-review-005-007.md` (not AW-VFY-008)
- Run mode: implementation (focused defect fixes)
- Model/client: unknown-not-exposed; timing: unknown-not-recorded
- Basis: origin/main `d078295`, CI run 42192082705 green (79/79)

## 1. Finish recovery (AW-VFY-006)

`RunFinishService` now treats a delta file without a receipt as a partial attempt: it removes the orphan delta and continues. If receipt `WriteNew` throws, the just-written delta is deleted and the active baseline is kept, so the next `finish` can retry.

Test: `RunReceiptTests.ReceiptWriteFailure_IsRetryable` (FailOnceReceiptStore throws on first WriteNew, then succeeds). First finish fails, baseline remains, no delta file; second finish writes the receipt and clears the baseline.

## 2. AWE006 per expected item (AW-VFY-007)

Each `expectedEvidence` entry must be referenced (ordinal, case-insensitive) by a Pass validation's outputSummary, evidenceDigest, sourceRef, commandDisplay, or validationId. One backed Pass no longer clears the whole list.

Test: `EachExpectedEvidenceItem_MustBeBacked_OrNotDone` — contract wants "unit test log" and "coverage report"; only the first is backed -> AWE006 for coverage report, not Done; second Pass backing coverage report -> Done (acceptance pre-marked Supported in the test).

## 3. Unique evidence IDs (AW-VFY-007)

`EvidenceService.Record`: empty id -> next free `vN`; existing id without `replace: true` -> error (`use --replace to overwrite`); `--replace` overwrites that id only. CLI `--id` omitted sends an empty id (no more hardcoded `v1`); `--replace` is a flag.

Tests: `RecordWithoutId_AllocatesUniqueIds_AndRefusesSilentOverwrite` (v1 Fail kept, v2 Pass added, clash without replace refused, replace updates v1); `Cli_RecordWithoutId_DoesNotOverwrite`.

## Queue / router

AW-VFY-005..007 marked Done with CI run 42192082705 on d078295. AW-VFY-008 is Ready now after these follow-up fixes (review asked not to treat the Evidence Gate as fully proven until they landed).

## Gate (executed)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 83, Skipped 0, Total 83.

## Remaining risk

- If delta cleanup after a receipt failure also fails, retry still proceeds via the orphan-delta path, but a leftover unreadable delta could surface as a later IO error.
- Expected-item matching is substring-based, not a typed evidence catalog.
- No command runner; acceptance criteria still stay Unknown in the real CLI flow, so Done still needs a later AC evaluator.
- This follow-up is not pushed; CI has not run on it.

Commit SHA (change): 8cabed7

