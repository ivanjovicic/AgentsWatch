# AW-VFY-010-LEDGER — Inventory existing AW-VFY-010 evidence

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Parent: `docs/prompts/AW-VFY-010-dogfood-30-receipts.md`  
Status: Ready now (NEXT claimable task)  
Depends on: AW-VFY-001..009 Done on current `origin/main`  
Unlocks: `AW-VFY-010-B3`  
Run mode: docs / evidence inventory (no runtime product changes)  
Budget: low/medium; one focused session

## Purpose

Produce one sanitized, reviewable ledger of every existing AW-VFY-010 dogfood
run, corrective product change, and post-merge check that can be cited without
guessing. The parent 30-receipt program is **in progress and not complete**.
This slice does not close AW-VFY-010.

## Current baseline (do not invent beyond cited evidence)

Start from current `origin/main`. As of AW-DOC-001 the expected SHA is
`edbbbe4a3e2962a8b20f7536d4a106420ff8492f` (merge of PR #13). Verify HEAD
before writing. Main CI run `42361996228` reported 193/193 tests on that SHA.

Planning notes, including any operative backlog file, are not completion
evidence. The file `AgentsWatch_Operativni_Backlog_2026-11-07.md` was not
found during AW-DOC-001; do not invent its contents.

## Trust-boundary rules that the ledger must apply

- JSON receipts are canonical; Markdown is a projection.
- Manual `Supported` is `AWE009` / `NeedsReview`. It is not independently
  verified `Done`.
- `--accept-validation` binds a command to a criterion (`acN`). It does not
  certify semantic acceptance or `Done`.
- Historical receipts that recorded `Done` under older semantics must be
  preserved as written. In the ledger, annotate them with current semantics
  (especially after PR #12) without rewriting the original files.
- Same-account `COMMENTED` reviews are not independent `APPROVED` reviews.
- Unit tests and green CI are not dogfood receipts.
- Invented sentinels only. Never reuse a previously published sentinel token
  and never copy private source into AgentsWatch artifacts.

## Known evidence sources to inspect (read-only)

Inspect what actually exists. If a path is missing, record it as missing.
Do not recreate or “fix” historical files.

Public / in-repo (may be committed):

- `docs/evidence/2026-11-07-aw-vfy-010-batch2-summary.md` (sanitized Batch 2 summary)
- `docs/RECEIPT_REDACTION.md`
- `docs/DECISION_ORDERING.md`
- GitHub PRs #11, #12, #13 (merged corrective work during dogfood)
- This prompt and the parent AW-VFY-010 prompt

Local / typically gitignored (do not commit secrets or overwrite):

- `.ai/runs/2026-11-07-aw-vfy-010-batch2-evidence.md` (original Batch 2 write-up)
- `.ai/runs/2026-11-07-aw-vfy-010-r2-n2-postmerge-check.md`
- `.ai/runs/2026-11-07-aw-vfy-010-finish-output-final-check.md`
- any Batch 1 local evidence / replay notes still present
- local worktrees under the owner’s `_aw-vfy-010` layout, if accessible

Expected historical shape to verify rather than assume:

- Batch 1 (2026-11-06 era): receipts commonly cited as #1–#4, #7, #8;
  Flutter/Mobile items #5 and #6 remain owner-HOLD unless evidence says otherwise.
- Batch 2 (started from `852ecd1`, documented 2026-11-07): R1–R5 and N1–N2 in
  the sanitized summary. Treat the historical matrix as written. After PR #12,
  do not restate historical `Done` on manual `Supported` as currently trustworthy.
- Corrective merged PRs: #11 (accept / decision priority), #12 (redaction +
  AWE009 + replay-stable `AWD004`), #13 (finish CLI renders the redacted receipt).
- Post-merge checks requested by the owner: R2/N2 after PR #12 on merged main
  `04d064f…`; finish-output privacy check after PR #13 on `edbbbe4…` reported
  successful (0 invented-sentinel hits on stored artifacts and finish
  stdout/stderr). Those checks are evidence of specific controls, not a 30-run close.

If additional genuine receipts exist, include them with path, SHA, and date.
If they do not exist, say so.

## Owned paths

- `docs/evidence/aw-vfy-010-ledger.md` (create; sanitized public ledger)
- optional compact pointer in `docs/prompt_queues/verification_mvp_2026_08_25.md`
  only if the ledger requires a status-line update (no second queue)
- local evidence note under `.ai/runs/` for this inventory session (gitignored)

## Avoid paths / do not touch

- `src/**` and `tests/**`
- historical receipts, original local `.ai/runs/*batch2-evidence*` and similar originals
- production SQL, deploy config, GitHub merge/close of other PRs
- Flutter / MathLearning-Mobile worktrees except read-only citation if needed
- open PRs #4, #5, #6, #8, #9
- `origin/main` (no direct push)

## Work steps

1. Verify `origin/main` SHA and record it.
2. Read the parent AW-VFY-010 prompt, `DECISION_ORDERING.md`, and
   `RECEIPT_REDACTION.md`.
3. Read every known evidence source above without modifying it.
4. Build a table with one row per real run or named check. Suggested columns:
   `id`, `date` (Europe/Belgrade), `workspace/repo`, `product SHA`,
   `decision`, `codes`, `evidenceDigest` if present, `TP/FP/TN/FN/Unknown`,
   `source path`, `counts toward 30?`, `current-semantics note`,
   `covers which parent-prompt event / adversarial control`.
5. Map coverage against the parent target sample (docs-only, test-only, small
   bug fix, .NET change, Flutter widget, Flutter provider/state,
   investigation/fix split, drift, missing validation, dirty worktree) and
   against the parent adversarial matrix.
6. List remaining gaps as candidate B3 slots. Prefer the smallest set that
   closes the highest-value holes. Do not schedule Flutter/Mobile #5/#6 unless
   the owner has lifted that HOLD.
7. State explicitly that AW-VFY-010 is still not complete, and why
   (missing count, missing event classes, or missing reviewed exit-gate report).

## Acceptance criteria

- A sanitized ledger exists at `docs/evidence/aw-vfy-010-ledger.md`.
- Every included row cites a real file, PR, SHA, or CI run. No invented runs.
- Historical originals are unchanged (byte-stable where they already existed).
- Current semantics are applied as annotations, not as silent rewrites of old
  `Done` / `Supported` outcomes.
- Remaining B3 slots are named with expected decision/codes where the current
  product makes that prediction possible.
- The ledger does not mark AW-VFY-010, AW-VFY-011, or AW-VFY-012 complete.
- No runtime code changes.

## Expected evidence

- `docs/evidence/aw-vfy-010-ledger.md`
- `.ai/runs/<date>-aw-vfy-010-ledger-evidence.md` with files read, files written,
  and a statement that originals were not overwritten
- queue status remains: parent AW-VFY-010 In progress; next task AW-VFY-010-B3

## STOP conditions

- Temptation to run a new dogfood batch inside this prompt (that is B3).
- Temptation to mark AW-VFY-010 complete because CI is green or PRs #12/#13 merged.
- Any need to overwrite or “clean up” a historical receipt.
- Any request to merge, deploy, run production SQL, or launch a cloud agent.
- Discovery that `origin/main` is not the expected post-#13 SHA and the owner
  has not authorized working from a different SHA: stop and report.
- Need to invent missing Batch 1/2 rows that cannot be found.

## Deliverable shape

Return:

- ledger path;
- commit SHA if committed;
- counted sources and how many rows may count toward the 30-receipt target;
- uncovered parent-prompt events;
- recommended B3 slots;
- explicit “AW-VFY-010 is not complete”.
