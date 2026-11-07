# AW-RES-001 — Competitive baseline and falsification protocol

Repository: `ivanjovicic/AgentsWatch`  
Owning queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Ready now; research-only, non-blocking for Gate 0  
Dependency: none for research; findings required as a reviewed input to AW-VFY-011  
Run mode: read-only external research + documents; no runtime edits  
Context budget: medium, two passes maximum; prefer primary docs and reproducible observations

## Goal

Try to **disprove** that RunContract -> run-boundary repository evidence -> required validation -> portable RunReceipt changes decisions beyond existing tooling. Do not claim an unobserved competitor feature is missing.

## Steps

1. Read only `AGENTS.md`, `docs/PRODUCT_SPEC.md`, `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`, `docs/research/agentswatch_deep_dive_2026_09/02_COMPETITOR_CAPABILITY_MATRIX.md` and `AW-VFY-011`.
2. Re-check current primary documentation for GitHub Copilot/Actions/Checks, Qodo, Cursor, Claude Code, Codex, CodeRabbit, Treft and one OSS verification alternative. Record exact product/version/date and direct source links. Classify each feature as Confirmed / Partial / Unknown / Contradicted. Do **not** infer "No" from absent marketing text.
3. Define at least eight repeatable scenarios against the **baseline actually used by each participant**: (a) dirty unchanged; (b) dirty changed-further; (c) untracked/rename; (d) missing mandatory tests; (e) stale validation after HEAD changed; (f) forbidden path; (g) unsupported "all tests pass"; (h) concurrent writer ambiguity.
4. For each scenario specify expected observable facts, baseline procedure (Git, CI, PR review, native hooks/logs and Qodo where accessible), minimal AgentsWatch incremental finding, decision changed yes/no, and measurement cost. Mark tools unavailable/not tested rather than substituting speculation.
5. Separate commodity features from defensible cross-vendor/run-interval evidence. Document copyability and the simplest competing implementation (for example a GitHub Action with shell scripts).
6. Define stop condition: if the relevant baseline finds the same material issues with equal/lower total reviewer effort, recommend OSS utility/narrow/STOP instead of a commercial platform.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/**` (new report only)
- `.ai/runs/**` compact research log when appropriate

## Avoid

- `src/**`, `tests/**`, CI workflows, existing historical research overwrites
- claims of competitor absence without independent observation
- installing paid third-party tools, supplying real secrets or source to unapproved vendors

## Required output

- `docs/research/agentswatch_market_validation_2026_11/competitive_baseline.md`: scenario matrix, source/date, status, setup and comparative decision-value criteria
- `docs/research/agentswatch_market_validation_2026_11/sources.md`: URLs + retrieval dates, primary/secondary provenance
- 3 most damaging counterexamples to the commercial thesis
- reviewed handoff link for AW-VFY-011, with untested fields explicitly marked Unknown

## Definition of done

At least eight falsifiable scenario definitions and all named competitor categories researched; every market/feature claim traceable to a source or explicitly Unknown. This prompt does not constitute external validation and cannot close Gate 6.
