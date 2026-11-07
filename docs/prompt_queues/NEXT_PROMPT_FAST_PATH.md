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
AW-VFY-010 ADJUST / PAUSE — prepare a real usefulness evaluation (no auto-run)
```

Prompt file:

```text
docs/evidence/aw-vfy-010-benefit-measurement-protocol.md
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

Do not start B3, B4, or B5 from this pointer. B3 and B4 already executed; B5 is not started.
Do not mark AW-VFY-010 complete from unit tests, a plan, or green CI.
Do not pick AW-VFY-011 or AW-VFY-012 (HOLD).
Do not pad the remaining 18 with extra synthetic N2/privacy variations.
The intended next product step, only after a new explicit owner instruction, is preparing a real usefulness evaluation — not launching another dogfood batch.

## Copy-ready instruction

```text
Repository: ivanjovicic/AgentsWatch
Prompt ID: AW-VFY-010 ADJUST/PAUSE — prepare a real usefulness evaluation (no auto-run)
Queue: docs/prompt_queues/verification_mvp_2026_08_25.md
Run mode: HOLD / handoff only; do not execute dogfood or measurement runs
Budget: none; no batches, no runs, no outreach, no HOLD lifting

Read:
- AGENTS.md
- docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
- docs/prompt_queues/verification_mvp_2026_08_25.md
- docs/evidence/aw-vfy-010-ledger.md
- docs/evidence/aw-vfy-010-gate5-interim.md
- docs/evidence/aw-vfy-010-competitive-baseline-2026-11-07.md
- docs/evidence/aw-vfy-010-benefit-measurement-protocol.md
- docs/DECISION_ORDERING.md
- docs/RECEIPT_REDACTION.md

Task:
Do not execute AW-VFY-010-B3, B4, or B5. Do not start AW-VFY-011 or AW-VFY-012.
Do not collect the remaining 18 useful REAL runs. Do not invent work to pad the count.
Do not lift any HOLD and do not do external outreach.
Gate 5 is ADJUST / PAUSE. The original 30/30 goal remains unmet at 12/30.
The existing 12 REAL runs must not be reused to prove the new usefulness goal.
The only copy-ready next step is to prepare (not start) a real usefulness
evaluation against the benefit-measurement protocol and its pre-defined
pilot criteria. Wait for a new, explicit owner instruction before any run.

Return:
- confirmation that no batch, measurement run, or HOLD-gated work was started
- pointers to the Gate 5 interim, competitive baseline, and benefit protocol
```

## Next after success

Do not continue remaining AW-VFY-010 batches from this pointer. After a
separate owner authorization, the next product step is preparing a real
usefulness evaluation, not automatically gathering the remaining 18 runs
and not starting B5 or AW-VFY-011.
