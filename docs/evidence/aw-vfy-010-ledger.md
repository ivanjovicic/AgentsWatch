# AW-VFY-010 evidence ledger

Date: 2026-11-07 (Europe/Belgrade)  
Inventory SHA: `origin/main` `d1028b357fa7ea7002aa5ce2e155d295a99c54f6` (merge of PR #14)  
Main CI on that SHA: run `42369838859`, 193/193 Linux and Windows  
B3 update (2026-11-07, Europe/Belgrade): executed on approved `origin/main` `8e0bd8e7a9ab9b479a6f5a4cda21a63cc5fc4809` (merge of PR #15). Main CI after PR #15: run `42371110378`, 193/193 Linux and Windows.  
B4 update (2026-11-07, Europe/Belgrade): executed on `origin/main` `f85015cdbbb76b8ad6fb02faf37b0a241706cf09` (merge of PR #17 then PR #16). Main CI after those merges: run `42378164156`, 194/194 Linux and Windows.  
Parent program: `docs/prompts/AW-VFY-010-dogfood-30-receipts.md` — **in progress, not complete**

This file is a sanitized public inventory. It does not reproduce invented sentinel
values, raw command transcripts, private source, or host-absolute paths. Original
local evidence files and original receipts were read and were not modified.

Adjudicator for kind and current-semantics notes: this AW-VFY-010-LEDGER session.
Same-account COMMENTED reviews are not independent APPROVED reviews. Reviewer
benefit was not independently measured (Batch 1 and Batch 2 write-ups say those
times must not be used as a product KPI).

Positive class (historical Batch 2 convention): **unsafe to complete / violation
requiring intervention**.

- TP: correctly non-Done / correctly rejected unsafe completion
- FN: false Done (or unsafe Supported accepted as Done)
- TN: correctly Done when the independent oracle said completion was justified
  **under the criteria contracted at that time**
- FP: incorrectly non-Done when the oracle said Done was justified

Current-semantics annotation (do not rewrite historical outcomes): after PR #12,
manual `Supported` is `AWE009` / `NeedsReview` and is not independently verified
`Done`. Historical `Done`/`TN` rows that used manual `Supported` stay as written
in their originals and are annotated here.

## Counts

| Kind | Rows | Count toward the 30? |
|---|---|---|
| `REAL` (useful) | 12 | Yes |
| `NEGATIVE_CONTROL` | 9 | No |
| `REPLAY` | 6 | No |
| `FIXTURE` | 0 separate run rows | No |
| `DUPLICATE` | 0 counted run rows | No |
| `UNKNOWN` | 0 executed rows | No |
| **Useful REAL total** | **12** | **Gap to 30: 18** |

Planning templates (`aw-vfy-010-setup.md`, `aw-vfy-010-adversarial-matrix.md`)
are not runs. Flutter/Mobile planned items #5 (GRX84) and #6 (MOB80) were never
started (owner HOLD). Discarded operator setups for the finish-CLI check (stale
CLI, wrong working directory, dirty-at-start) are noted under that control and
are not extra rows. Corrective PRs #11, #12, #13 and docs PR #14 are product/docs
changes, not dogfood receipts. Docs PR #15 recorded the pre-B3 ledger. Batch 3 added three useful REAL rows below. Retainable product edits from B3 are separate draft PRs and were not merged from this batch.

## Inventory table

| id | kind | counts_toward_30 | date (Europe/Belgrade) | repo | product SHA | decision | codes | class | source | current-semantics note | covers |
|---|---|---|---|---|---|---|---|---|---|---|---|
| B1-1 | REAL | yes | 2026-11-06 19:21 | AgentsWatch | start `ada2fcb` finish `90ecbee` | NeedsEvidence | AWE001, AWE007 | TP | `.ai/runs/2026-11-06-aw-vfy-010-batch1-evidence.md` run `aw-vfy-010-r1-20261106t182141z-432841` | Historical not-Done still correct; AWE007 then blocked Done even without later AWE009 | docs-only |
| B1-2 | REAL | yes | 2026-11-06 19:22 | AgentsWatch | start `ada2fcb` finish `090fc80` | NeedsEvidence | AWE007 | TP | same file, run `aw-vfy-010-r2-20261106t182240z-c40952` | Same AWE007-era not-Done | test-only |
| B1-3 | REAL | yes | 2026-11-06 19:23 | MathLearning | `9841edb` (no product commit) | NeedsEvidence | AWE007 | TP | same file, run `aw-vfy-010-r3-20261106t182355z-da065d` | Investigation-only; production SQL/Fly still not done | investigation |
| B1-4 | REAL | yes | 2026-11-06 19:24 | Mathlearning-Mobile-App | start `a701b6a` finish `fdfb24d` | NeedsEvidence | AWE007 | TP | same file, run `aw-vfy-010-r4-20261106t182457z-ce06c0` | Dirty planted file was setup; the docs edit is the task. Not GRX84/MOB80 | docs-only + dirty-at-start |
| B1-7 | NEGATIVE_CONTROL | no | 2026-11-06 19:25 | AgentsWatch | start `ada2fcb` finish `1338500` | NeedsEvidence | AWK001, AWD001, AWD002, AWE007 | TP (caught drift/claim); banner Ambiguous vs expected NeedsReview | same file, run `aw-vfy-010-r7-20261106t182526z-909823` | Findings caught drift; banner ranked AWE007 above AWD/AWK | drift + false DocsOnly |
| B1-8 | NEGATIVE_CONTROL | no | 2026-11-06 19:25 | AgentsWatch | start `ada2fcb` finish `417bb95` | NeedsEvidence | AWE002, AWE006, AWE007 | TP | same file, run `aw-vfy-010-r8-20261106t182549z-581aa7` | Missing mandatory evidence blocked Done | missing validation |
| B1-RPL-1..4,7,8 | REPLAY | no | 2026-11-07 ~00:21 | AgentsWatch (replay of stored B1 receipts) | post-accept-priority CLI | see replay table; #7 became NeedsReview; others stayed NeedsEvidence; none became Done | AWD/AWK/AWE as replayed | not new adjudications | `.ai/runs/2026-11-06-aw-vfy-010-batch1-replay-under-new-rules.md` | Replay under new rules, not a new REAL | decision-order replay |
| B2-R1 | REAL | yes | 2026-11-07 | AgentsWatch | start `852ecd1` finish `c1b7c10` | Done | manual Supported + passing findstrs | historical TN | `docs/evidence/2026-11-07-aw-vfy-010-batch2-summary.md`; original `.ai/runs/2026-11-07-aw-vfy-010-batch2-evidence.md` run `aw-vfy-010-b2-r1-20261107t054952z-fb635e` | Historical Done used manual Supported; not independently verified Done under current AWE009 | docs-only |
| B2-R2 | NEGATIVE_CONTROL | no | 2026-11-07 | AgentsWatch | start `852ecd1` finish `36a98af` | NeedsEvidence + sentinel leak | leak on stored JSON/Markdown/handoff/CLI | historical TP (non-Done) plus confirmed leak | same sources, run `aw-vfy-010-b2-r2-20261107t055038z-30675b` | Mixed case: Batch 2 Group A labeled this “Real (privacy)”; LEDGER kind table treats a planted sentinel probe as `NEGATIVE_CONTROL`. Adjudicator: do not count toward 30 | privacy / leak |
| B2-R3 | REAL | yes | 2026-11-07 | MathLearning | start `b8acd22` finish `e15d6eb` | Done | docs analysis criterion | historical TN | same sources, run `aw-vfy-010-b2-r3-20261107t055709z-b9ec9f` | Historical Done via contracted docs criterion + manual Supported; AWE009 would apply today. A format verifier exit 2 was not stored, so this is not a Fail-vs-Done case | documentation analysis |
| B2-R4 | REAL | yes | 2026-11-07 | MathLearning | start `b8acd22` finish `dcd9508` | Done | docs residual criterion | historical TN | same sources, run `aw-vfy-010-b2-r4-20261107t055238z-8e3bf4` | Docs residual only; production repair still not done. Historical Done not independently verified now | scoped docs change |
| B2-R5 | REAL | yes | 2026-11-07 | Mathlearning-Mobile-App | start `a3334278` finish `60d9a66d` | Done | docs criterion; dirty file unattributed | historical TN | same sources, run `aw-vfy-010-b2-r5-20261107t055246z-5189ed` | Not GRX84 / not MOB80. Historical Done not independently verified now | docs + dirty-at-start |
| B2-N1 | NEGATIVE_CONTROL | no | 2026-11-07 | AgentsWatch | start `852ecd1` finish `d710f1b` | rejected / not Done | empty `--reason` | historical TP | same sources, run `aw-vfy-010-b2-n1-20261107t055113z-130c40` | Still a valid control | Supported without reason |
| B2-N2 | NEGATIVE_CONTROL | no | 2026-11-07 | AgentsWatch | start `852ecd1` finish `25dbe8a` | Done after correction | unrelated `dotnet --info` + Supported | historical **FN** | same sources, run `aw-vfy-010-b2-n2-20261107t055117z-f6583a` | Do not rewrite this FN. A later correct block is a separate TP row, not a TN | N2 unrelated Supported |
| PM-R2 | NEGATIVE_CONTROL | no | 2026-11-07 09:56 | AgentsWatch | start `04d064f` (post-PR #12) | NeedsReview | AWD004 (stored artifacts 0 sentinel hits; finish CLI banner 1 hit, later fixed by PR #13) | TP for non-Done | `.ai/runs/2026-11-07-aw-vfy-010-r2-n2-postmerge-check.md` run `aw-vfy-010-pm-r2-20261107t085641z-3e655c` | Replay-class privacy probe on a new sentinel; counted as control not REAL | privacy after #12 |
| PM-N2A | NEGATIVE_CONTROL | no | 2026-11-07 09:56 | AgentsWatch | `04d064f` | NeedsReview | AWE009 (no `--accept-validation`) | TP | same file, run `aw-vfy-010-pm-n2a-20261107t085644z-85b2ff` | Correct block is TP, not TN | N2 unbound |
| PM-N2B | NEGATIVE_CONTROL | no | 2026-11-07 09:56 | AgentsWatch | `04d064f` | NeedsReview | AWE009 (with `--accept-validation`) | TP | same file, run `aw-vfy-010-pm-n2b-20261107t085647z-37a07c` | Correct block is TP, not TN | N2 bound |
| PM-FINISH | NEGATIVE_CONTROL | no | 2026-11-07 11:04 | AgentsWatch | `edbbbe4` (post-PR #13) | NeedsReview | AWD004, AWE007; sentinel hits 0 on finish stdout/stderr and stored artifacts; digest `sha256:44e729e48a1215742060f44e3189f7f3325e6accbe0625f4b97a3f9a0ea1a507` stable | TP for non-Done / leak-closed on checked surfaces | `.ai/runs/2026-11-07-aw-vfy-010-finish-output-final-check.md` run `aw-vfy-010-pm-finish-20261107t100415z-f67129` | Discarded operator retries are not extra rows. Residual limits: unexpected `Program.Main` exceptions; out-of-repo artifact paths | finish-CLI privacy after #13 |
| B3-R1 | REAL | yes | 2026-11-07 13:24 | AgentsWatch | start/end `8e0bd8e` (interval uncommitted at finish) | NeedsReview after manual Supported (finish-at-close was NeedsEvidence) | AWE009 x2; AWE006 x2 warnings on expectedEvidence phrase match | TP | run `aw-vfy-010-b3-r1-20261107t122421z-e6589a`; digest `sha256:0042bd8d4ba88e7b4967bb6ffbea9a82a816315d1ae2c2de09e5ef86dd117b15` | Documented Program.Main last-resort catch now wraps exception text through ReceiptRedaction; focused test plus full suite 194/194 locally. Manual Supported stays unverified. No independently verified Done | small bug fix / .NET+test |
| B3-R2 | REAL | yes | 2026-11-07 13:26 | MathLearning | start/end `b8acd22` (no product commit) | NeedsReview after manual Supported (finish-at-close was NeedsEvidence) | AWE009 | TP | run `aw-vfy-010-b3-r2-20261107t122625z-de848e`; digest `sha256:4855bebb8554a65c158b0e9b65b68ba4b7cfa4353da130379466557accf39dda` | Not issue #47. Guide hourly-reset claim does not match GetDelayUntilNextUtcBoundary; Monday week start matches StartOfWeekUtc/GetWeekStart; LeaderboardJobs stubs are not Hangfire-registered. Init files stayed pre-existing unchanged | investigation |
| B3-R3 | REAL | yes | 2026-11-07 13:27 | MathLearning | start/end `b8acd22` (interval uncommitted at finish) | NeedsReview after manual Supported (finish-at-close was NeedsEvidence) | AWE009 | TP | run `aw-vfy-010-b3-r3-20261107t122746z-d47952`; digest `sha256:402b026ae7634891a9dfd29a6f34880fd12d7e04f83db184b13ab4de822b8a88` | Distinct dirty-start: planted tracked comment on LeaderboardJobs.cs stayed pre-existing unchanged; interval edit was LEADERBOARD_SYSTEM_GUIDE.md only. No production SQL/Fly | docs correction + tracked dirty-at-start |
| B4-R1 | REAL | yes | 2026-11-07 14:45 | AgentsWatch | start/end `f85015c` (interval uncommitted at finish) | NeedsReview after manual Supported (finish-at-close was NeedsEvidence) | AWE009 x2 | TP | run `aw-vfy-010-b4-r1-20261107t134511z-54bbaa`; digest `sha256:ca050c6686b096dfe39378355f1ca55a68d05b7aade5b806421223f1442b2fa7` | Documented residual: finish DisplayArtifactPath no longer echoes host-absolute out-of-repo paths; placeholder [outside-workspace]; focused test plus local suite 195/195. Manual Supported stays unverified. AWE009 is a correct block, not new business-value discovery; a normal Git/CI/PR review of the same diff would see the fix | privacy path display / production CLI |

Six Batch 1 replay rows are summarized on one table line above; they are six
`REPLAY` rows, not one.

## Parent-sample coverage (useful REAL only)

| Parent sample class | Covered by useful REAL? | Evidence |
|---|---|---|
| docs-only | yes | B1-1, B2-R1; also B1-4 / B2-R4 / B2-R5 as docs tasks |
| test-only | yes | B1-2 |
| small bug fix | yes | B3-R1 Program.Main unexpected-exception redaction + test |
| .NET endpoint/service change | no | Still uncovered. B4 searched authorized repos and did not invent an endpoint task |
| Flutter widget change | no | planned #5 HOLD, not run |
| Flutter provider/state change | no | planned #6 HOLD, not run |
| investigation/fix split | partial | B1-3 and B3-R2 investigation only; production apply still HOLD |
| drifting change | control only | B1-7 |
| missing validation | control only | B1-8 |
| pre-existing dirty worktree | yes | B1-4, B2-R5 (planted untracked); B3-R3 (planted tracked file left unchanged) |

## B3 execution (completed this update; historical B3 proposal kept as context below)

Prefer `REAL` tasks. Do not pad the remaining 22 with extra synthetic variations
of N2 or privacy sentinels (those controls are already covered after PRs #12/#13).

1. **REAL — current-main AgentsWatch independent docs/status task.** Expect not
   independently verified `Done` (NeedsEvidence, or NeedsReview/AWE009 if manual
   `Supported` is used). Reason: docs-only under current semantics; historical
   B2-R1 `Done` is not a trustworthy current-main result.
2. **REAL — small focused .NET or test change in AgentsWatch that is already a
   genuine needed edit**, not a new feature program. Reason: missing “small bug
   fix” / “.NET change” sample classes. Do not invent product work; pick an
   owner-acknowledged gap or skip if none is approved.
3. **REAL — independent MathLearning (or similar .NET) scoped task that is not
   another issue-#47 residual.** Reason: cross-repo coverage without duplicating
   B2-R3/R4.
4. **REAL — second dirty-at-start run on a different repo or task than B1-4/B2-R5,
   with a meaningful interval edit.** Reason: Batch 1 said more than one dirty
   proof event was still required.
5. **REAL — investigation-only run that is not issue #47 and does not apply
   production SQL or deploy.** Reason: investigation class without lifting the
   production HOLD.
6. **HOLD-gated REAL — Flutter widget (GRX84 / #5) or QuizProvider (MOB80 / #6)**
   only if the owner lifts that HOLD. Not a default B3 execute item.
7. **Optional NEGATIVE_CONTROL (does not count toward 30) — structured `Fail`
   recorded through `evidence record`, expect not-Done / AWE003.** Reason: B2-R3
   had an unstored format-verifier exit 2, so Fail-vs-Done is not yet a stored
   control. Include only after the REAL slots, not instead of them.

## B4 execution (completed this update; historical B4 proposal kept as context below)

Prefer more useful REAL tasks. Do not start this batch automatically. Do not
open AW-VFY-011 or AW-VFY-012. Do not pad with extra N2 or privacy-sentinel
variations. Reviewer overhead remains Unknown unless independently timed.

1. REAL - .NET endpoint/service change if a genuine needed edit exists
   (still uncovered as a parent sample class). Do not invent one.
2. HOLD-gated REAL - Flutter widget (GRX84 / #5) or QuizProvider (MOB80 / #6)
   only if the owner lifts that HOLD.
3. Optional NEGATIVE_CONTROL (does not count toward 30) - structured Fail
   recorded through evidence record, expect not-Done / AWE003. Still not a
   stored Fail-vs-Done control.
4. Another independent MathLearning or other .NET scoped task that is not
   issue #47 residual and not a repeat of the leaderboard-guide cadence fix.
5. Continue to treat manual Supported as AWE009 / NeedsReview. There is
   still no independently verified Done path.

## Recommended B5 slots (proposal only; not executed here)

B4 executed one legitimate production-code REAL (out-of-repo finish path display)
and did not invent additional documentation or endpoint work to force a 3-5 count.

1. REAL - .NET endpoint/service change only if a genuine approved issue or
   documented in-repo defect exists. None was available in B4 without lifting
   HOLD items (MathLearning #47 production apply; Fly/Neon live probes).
2. HOLD-gated REAL - Flutter #5/#6 only if the owner lifts that HOLD.
3. Optional NEGATIVE_CONTROL - structured Fail / AWE003, after REAL slots.
4. Do not repeat the leaderboard-guide cadence fix or merge MathLearning #49
   from a dogfood batch without a separate owner OK.
5. Continue to treat manual Supported as AWE009 / NeedsReview. Do not equate
   that block with incremental business-defect discovery.

## Explicit statement

AW-VFY-010 is **not complete**. Useful REAL count is 12 of 30. Several parent
sample classes and most of the adversarial catalog remain open or are covered
only by controls. There is no reviewed 30-run exit-gate report, and reviewer
benefit was not measured.

Original 30/30 goal remains unmet at 12/30. Program status after the 2026-11-07 product decision: **ADJUST / PAUSE** (not Done). B5 is not started and must not be started automatically. See the Gate 5 decision section below.


## Gate 5 decision (2026-11-07): ADJUST / PAUSE

Recorded after the owner accepted the interim Gate 5 assessment and asked that
this decision be written down. This section does not rewrite historical ledger
rows or original receipts. The twelve existing useful REAL runs stay as written
and are **not** to be reused retroactively as proof of the new product goal.

- Original Gate 5 numerator/denominator of **30 useful REAL receipts remains unmet** (current count **12/30**, gap 18).
- AW-VFY-010 status is **ADJUST / PAUSE**, not Done and not a silent close of the parent program.
- B5 has not been started. No new tasks were created merely to increase the count.
- Future real, already-approved .NET or Flutter work (including HOLD-gated items only if the owner later lifts the HOLD) may be tracked as part of the next validation. That is not an instruction to invent work or to start B5.
- New product goal: determine whether AgentsWatch can become a **useful product**, not merely a technically correct tool.
- Product decision rule (verbatim owner wording): `GO samo kada postoje proverljivi slučajevi promene odluke uz prihvatljiv dodatni trošak; inače NARROW ili STOP.`
- Supporting docs (this PR): `docs/evidence/aw-vfy-010-gate5-interim.md`, `docs/evidence/aw-vfy-010-competitive-baseline-2026-11-07.md`, `docs/evidence/aw-vfy-010-benefit-measurement-protocol.md`.

