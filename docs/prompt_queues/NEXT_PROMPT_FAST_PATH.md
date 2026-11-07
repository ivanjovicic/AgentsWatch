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
AW-VFY-010-LEDGER — Inventory existing AW-VFY-010 evidence
```

Prompt file:

```text
docs/prompts/AW-VFY-010-LEDGER.md
```

Queue:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

Parent program (not complete, not the next single-session task):

```text
AW-VFY-010 — 30-receipt dogfood proof
docs/prompts/AW-VFY-010-dogfood-30-receipts.md
```

## Why this is next

AW-VFY-001 through AW-VFY-009 are Done on `origin/main`.
Current `origin/main` after merged PR #13 is
`edbbbe4a3e2962a8b20f7536d4a106420ff8492f`. Main CI run `42361996228`
reported 193/193 tests (Linux + Windows).

AW-VFY-010 is **in progress and not complete**. Some real dogfood batches and
corrective runtime PRs (#11, #12, #13) already exist. The next safe slice is
to inventory that evidence without rewriting historical receipts, then run a
small remaining batch (`AW-VFY-010-B3`) against the documented gaps.

Do not start another broad 30-run session.
Do not mark AW-VFY-010 complete from unit tests, a plan, or green CI.
Do not pick AW-VFY-001 (historical; this file previously pointed there).
Do not pick AW-VFY-011 or AW-VFY-012 (HOLD until AW-VFY-010 completes and is reviewed).

## Copy-ready instruction

```text
Repository: ivanjovicic/AgentsWatch
Prompt ID: AW-VFY-010-LEDGER
Queue: docs/prompt_queues/verification_mvp_2026_08_25.md
Run mode: docs / evidence inventory
Budget: low/medium

Read:
- AGENTS.md
- docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
- docs/prompt_queues/verification_mvp_2026_08_25.md
- docs/prompts/AW-VFY-010-dogfood-30-receipts.md
- docs/prompts/AW-VFY-010-LEDGER.md
- docs/DECISION_ORDERING.md
- docs/RECEIPT_REDACTION.md
- docs/evidence/2026-11-07-aw-vfy-010-batch2-summary.md

Task:
Execute AW-VFY-010-LEDGER exactly. Inventory existing dogfood evidence and
corrective PRs without modifying historical receipts or local `.ai/runs/`
originals. Produce a sanitized ledger that names remaining gaps for B3.
Do not run a new dogfood tour. Do not mark AW-VFY-010 complete.

Return:
- ledger path and SHA
- counted evidence sources (no invented runs)
- coverage vs the AW-VFY-010 required events
- remaining slots recommended for AW-VFY-010-B3
- explicit statement that AW-VFY-010 is still not complete
```

## Next after success

If AW-VFY-010-LEDGER is complete:

```text
AW-VFY-010-B3 — Next small dogfood batch
docs/prompts/AW-VFY-010-B3-small-dogfood-batch.md
```

Then continue only the remaining AW-VFY-010 batches named by the ledger,
still without claiming the parent program complete until its exit gate is met.
