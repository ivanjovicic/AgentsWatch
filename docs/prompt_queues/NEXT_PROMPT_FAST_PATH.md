# AgentsWatch Next Prompt Fast Path

Last aligned: 2026-11-07

This file is a copy-ready pointer only. If it disagrees with
`PROMPT_QUEUE_ROUTER.md` or `verification_mvp_2026_08_25.md`, those two files
win and this file must be updated.

There is exactly one canonical next runtime/docs task. Parallel support
prompts (`AW-QA-*`, `AW-SEC-*`, `AW-RES-*`) are not a second queue and are
not the next claimable task unless the owner explicitly redirects.

## Current next prompt

```text
AW-VFY-010 ADJUST / PAUSE (B5 not started) — do not start a new batch from this pointer
```

Prompt file:

```text
docs/evidence/aw-vfy-010-ledger.md
```

Queue:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

Depends on completed ledger:

```text
docs/evidence/aw-vfy-010-ledger.md
```

Parent program (not complete, not the next single-session task):

```text
AW-VFY-010 — 30-receipt dogfood proof
docs/prompts/AW-VFY-010-dogfood-30-receipts.md
```

## Why this is next

B3 executed three useful REAL runs and raised the useful REAL total from 8 to 11 of 30.
B4 added one legitimate REAL and raised the total to 12 of 30 (gap 18).
On 2026-11-07 the owner recorded Gate 5 as ADJUST / PAUSE. The original 30/30 goal remains unmet.
The twelve existing REAL runs must not be reused to prove the new usefulness goal.

Do not start B5 automatically from this pointer.
Do not mark AW-VFY-010 complete from unit tests, a plan, or green CI.
Do not pick AW-VFY-011 or AW-VFY-012 (HOLD).
Do not pad the remaining 18 with extra synthetic N2/privacy variations.

## Copy-ready instruction

```text
Repository: ivanjovicic/AgentsWatch
Prompt ID: AW-VFY-010 ADJUST/PAUSE (B5 not started; do not start automatically)
Queue: docs/prompt_queues/verification_mvp_2026_08_25.md
Run mode: small real dogfood batch
Budget: one small batch; only useful REAL rows advance the 30

Read:
- AGENTS.md
- docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
- docs/prompt_queues/verification_mvp_2026_08_25.md
- docs/prompts/AW-VFY-010-dogfood-30-receipts.md
- docs/prompts/AW-VFY-010-B3-small-dogfood-batch.md
- docs/evidence/aw-vfy-010-ledger.md
- docs/DECISION_ORDERING.md
- docs/RECEIPT_REDACTION.md

Task:
Execute AW-VFY-010-B3 exactly. Prefer the REAL slots named in the ledger.
Do not invent run counts. Do not mark AW-VFY-010 complete. Do not lift
Flutter/Mobile HOLD items without owner approval.

Return:
- new rows with kind and TP/FP/FN/TN using the Batch 2 positive class
- useful REAL count after the batch and remaining gap to 30
- updated ledger
- explicit statement that AW-VFY-010 is still not complete
```

## Next after success

Continue only the remaining AW-VFY-010 batches named by the updated ledger,
still without claiming the parent program complete until its exit gate is met.
