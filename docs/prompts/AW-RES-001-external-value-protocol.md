# AW-RES-001 — Competitive baseline, falsification scenarios, and external-value interview protocol

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track, not a second runtime queue)  
Status: Parallel research; required input to AW-VFY-011, not a substitute for it  
Depends on: current product promises (receipts, evidence gate, AWE009 / NeedsReview)  
Run mode: read-only external research + protocol design; no runtime edits  
Budget: medium, two passes maximum

## Purpose

Prepare the competitive and interview instruments that AW-VFY-011 will later
need. This prompt tries to **disprove** that RunContract -> run-boundary
repository evidence -> required validation -> portable RunReceipt changes
decisions beyond existing tooling. It collects no customer quotes, invents no
evaluations, and does not start AW-VFY-011. Gate 6 and AW-VFY-011 remain HOLD.

## Part A — Competitive baseline (restored from PR #9)

1. Read only `AGENTS.md`, `docs/PRODUCT_SPEC.md`,
   `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`,
   `docs/research/agentswatch_deep_dive_2026_09/02_COMPETITOR_CAPABILITY_MATRIX.md`
   and `docs/prompts/AW-VFY-011-external-value-validation.md`.
2. Re-check current primary documentation for GitHub Copilot/Actions/Checks,
   Qodo, Cursor, Claude Code, Codex, CodeRabbit, Treft and one OSS verification
   alternative. Record exact product/version/date and direct source links.
   Classify each feature as Confirmed / Partial / Unknown / Contradicted.
   Do **not** infer "No" from absent marketing text.
3. Define at least eight repeatable, falsifiable scenarios against the
   **baseline actually used by each participant**:
   (a) dirty unchanged;
   (b) dirty changed-further;
   (c) untracked/rename;
   (d) missing mandatory tests;
   (e) stale validation after HEAD changed;
   (f) forbidden path;
   (g) unsupported "all tests pass";
   (h) concurrent writer ambiguity.
4. For each scenario specify expected observable facts, baseline procedure
   (Git, CI, PR review, native hooks/logs and Qodo where accessible), minimal
   AgentsWatch incremental finding, decision changed yes/no, and measurement
   cost. Mark tools unavailable/not tested rather than substituting speculation.
5. Separate commodity features from defensible cross-vendor/run-interval
   evidence. Document copyability and the simplest competing implementation
   (for example a GitHub Action with shell scripts).
6. Define the stop/kill condition: if the relevant baseline finds the same
   material issues with equal or lower total reviewer effort, recommend OSS
   utility / narrow / STOP instead of a commercial platform.

## Part B — Interview protocol (kept from the earlier PR #14 draft)

Design a later interview script that forces interviewers to answer:

- Did a receipt change a merge, rewrite, or acceptance decision?
- Did unverified `Supported` / `NeedsReview` (AWE009) increase or reduce trust?
- Were false positives or ambiguity more expensive than the catch?
- Was cross-vendor independence valued?
- Which baseline already covered the case?

The script must compare against native vendor logs, Git, CI, PR review, and
current independent-review tools (including Qodo where relevant). "No
decision-changing value" is a valid outcome.

Do not contact people from this prompt. Contact requires an explicitly
authorized outreach workflow naming who is contacted.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/competitive_baseline.md`
- `docs/research/agentswatch_market_validation_2026_11/sources.md`
- `docs/research/agentswatch_market_validation_2026_11/external_interview_protocol.md`
- compact `.ai/runs/**` research log when appropriate

## Avoid

- `src/**`, `tests/**`, CI workflows, overwriting historical research
- claiming a competitor feature is absent without independent observation
- installing paid third-party tools or supplying real secrets/source to
  unapproved vendors
- inventing respondents or starting AW-VFY-011
- creating another active queue

## Acceptance criteria

- At least eight falsifiable scenario definitions exist.
- Named competitor categories are researched with dated primary sources or
  explicitly marked Unknown.
- Three most damaging counterexamples to the commercial thesis are listed.
- Interview script exists and includes AWE009 / NeedsReview.
- A reviewed handoff link for later AW-VFY-011 marks untested fields Unknown.
- This work does not constitute external validation and cannot close Gate 6.
- AW-VFY-011 remains HOLD.

## STOP conditions

- Pressure to invent "3 teams asked to keep using it" or competitor gaps.
- Pressure to start external outreach without owner authorization naming who
  is contacted.
- Pressure to mark AW-VFY-011 complete or ready-to-execute before AW-VFY-010
  completes and is reviewed.
