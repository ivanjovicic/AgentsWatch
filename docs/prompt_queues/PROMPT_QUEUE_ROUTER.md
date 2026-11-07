# AgentsWatch Prompt Queue Router

Last aligned: 2026-11-07

Use this file first when choosing the next AgentsWatch task.

## Canonical queue

Current active implementation + validation queue:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

There is one active queue. Do not create a second runtime queue from support
prompts, old `AW-VAL-*` files, or planning notes.

Older bootstrap, MVP, token-economy, productization, roadmap, architecture,
licensing, and evidence queues are historical/supporting context unless a task
is explicitly re-promoted into the canonical queue.

Copy-ready next-task pointer: `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`.
If that pointer disagrees with this router or the canonical queue, this router
and the canonical queue win.

Support-prompt coverage and open-PR classification:
`docs/prompt_queues/SUPPORT_PROMPT_COVERAGE.md`.

## Current known state

Updated 2026-11-07 (planning/docs reconciliation; not additional dogfood runs):

```text
origin/main: edbbbe4a3e2962a8b20f7536d4a106420ff8492f
restore: PASS
build: PASS
tests: PASS (193/193 CI-confirmed on edbbbe4, GitHub Actions run 42361996228)
```

AW-VFY-001..009 are Done on `origin/main`.

Merged after the previous router note (which still said 150/150 on `ab3ebe6`
and “AW-VFY-010 not started”):

- PR #12 merged; head `d09bfea3a6b21397776d949a18180f72fdc10074`; merge commit
  `04d064fde822c7907b7bd3f65eb1161eeae6032b`; PR CI run `42356206659` (190/190);
  main CI run `42357282636` (190/190). Receipt redaction walker, AWE009 /
  `NeedsReview` for unverified / manual `Supported`, replay-stable `AWD004`.
- PR #13 merged; head `5453420870c993201424d40f34f1f2cfce0974df`; merge commit
  `edbbbe4a3e2962a8b20f7536d4a106420ff8492f`; PR CI run `42359180448` (193/193);
  main CI run `42361996228` (193/193). Finish CLI renders from the redacted
  receipt. The requested post-merge privacy / finish-output check succeeded
  (local evidence `.ai/runs/2026-11-07-aw-vfy-010-finish-output-final-check.md`:
  invented-sentinel hits on stored artifacts and finish stdout/stderr = 0).
  Two known non-blocking residual limits remain documented in
  `docs/RECEIPT_REDACTION.md` (unexpected `Program.Main` exceptions;
  artifact paths outside the workspace may not be repo-relative).

AW-VFY-010 is **in progress and not complete**. Unit tests, merged corrective
PRs, and a successful privacy check do not close the 30-receipt program.
Existing batches and checks were inventoried in `docs/evidence/aw-vfy-010-ledger.md` (useful REAL 11 of 30 after B3). The next slice is a further AW-VFY-010 batch (B4) named in that ledger; do not start it automatically. Do not invent run counts, customer feedback, or
independent approvals. Planning documents are not completion evidence.

The attached planning file `AgentsWatch_Operativni_Backlog_2026-11-07.md` was
not found on the searched machines or `/workspace/uploads` during AW-DOC-001.
Do not invent its contents.

Manual `Supported` remains `AWE009` / `NeedsReview`. `--accept-validation`
binds a command to a criterion and does not independently certify `Done`.

## Current next prompt

```text
Further AW-VFY-010 batch (B4) - named in the ledger; do not start automatically
```

Prompt file:

```text
docs/evidence/aw-vfy-010-ledger.md (Recommended B4 slots)
```

Immediate order after that:

```text
1. AW-VFY-010-B3         (executed; ledger updated)
2. Further AW-VFY-010 batch / B4 (NEXT; proposal only in the ledger; do not start automatically)
3. Remaining AW-VFY-010 batches named by the ledger, until the parent exit gate
4. (AW-VFY-010-LEDGER is complete as an inventory slice; do not repeat unless new evidence appears)
5. AW-VFY-011            (HOLD until AW-VFY-010 is complete and reviewed)
6. AW-VFY-012            (HOLD until AW-VFY-011 passes)
```

Parent prompt (program, not the next single-session task):

```text
docs/prompts/AW-VFY-010-dogfood-30-receipts.md
```

## Strict decision tree

```text
Are AW-VFY-001..009 done on current origin/main?
  no  -> stop and report; this router is stale
  yes -> Is the AW-VFY-010 30-run proof complete (exit gate in the parent prompt)?
          no  -> Has AW-VFY-010-LEDGER produced a sanitized inventory of existing evidence?
                  no  -> AW-VFY-010-LEDGER
                  yes -> Run the next small named batch (B4 per ledger; not started) <== CURRENT NEXT TASK
                         then only further 010 batches the ledger still shows as open
          yes -> Is external value proven (AW-VFY-011)?
                  no  -> AW-VFY-011            (HOLD until 010 is complete and reviewed)
                  yes -> Is commercial design-partner value proven (AW-VFY-012)?
                          no  -> AW-VFY-012    (HOLD until 011 passes)
                          yes -> choose post-gate investment from evidence
```

Do not select:

- AW-VFY-001 through AW-VFY-009 (already Done);
- the parent AW-VFY-010 prompt as one giant session;
- AW-QA-001, AW-SEC-001, AW-RES-001, or AW-RES-002 as the next runtime task
  (parallel support track only; see coverage matrix);
- AW-VAL-* or other historical `Ready` prompts that are not in the canonical queue;
- open PRs #4, #5, #6, #8, or #9 as merge/close/update work unless the owner
  separately asks (classification only; do not change those PRs).

## Non-negotiable ordering rules

Do not implement before their gate:

- Contract features before Gate 0 closes;
- `finish` attribution before start baseline is stable;
- receipt verification before attribution is proven;
- scope/claims checks on raw final git status;
- external/commercial packaging before trustworthy receipts/dogfood;
- learning/router/dashboard/broad integrations before AW-VFY-011 external-value evidence;
- SaaS/auth/billing/team administration before AW-VFY-012 commercial evidence.

## Product guardrails

Do not route current work toward:

- proprietary coding-agent execution;
- generic control-plane/session manager;
- cloud workspace/orchestration;
- generic token/cost dashboard as the primary product;
- generic AI-code tracking as the product wedge;
- SaaS/billing/OAuth before commercial gate;
- visual workflow builder;
- automatic merge/release/deploy.

Current competitive boundary:

```text
Cursor/GitHub/Qodo and adjacent vendors increasingly provide tracking, session/audit and governance surfaces.
AgentsWatch must prove independent cross-vendor verification value, not duplicate native tracking.
```

Canonical addendum:

```text
docs/research/COMPETITIVE_VALIDATION_ADDENDUM_2026_09_16.md
```

## Context rule

Normal prompt selection should require only:

```text
AGENTS.md
-> this router
-> selected prompt file
-> prompt's listed canonical docs
-> exact code/tests
```

Do not load all historical docs or queues.

For AW-VFY-011/012, load the competitive addendum and the reviewed AW-VFY-010 evidence summary in addition to the selected prompt.

## Historical queue rule

If an older queue says `Ready now` but the prompt is not present/promoted in `verification_mvp_2026_08_25.md`, treat that status as superseded.

The canonical queue wins until a newer dated queue explicitly replaces it.
