# AgentsWatch Prompt System Audit — 2026-06-29

## Scope

User request: add more prompts, improve existing prompts, and organize queues for faster and easier work.

This was a docs/prompt-system pass. Runtime code was not changed.

## Issues found

1. `agentwatch_mvp.md` still showed AW-002+ as Ready even though Gate 0 is incomplete.
2. There was no single router deciding which queue wins.
3. There was no copy-ready fast path for the next prompt.
4. Productization queue did not point to the new router.
5. `AGENTS.md` did not yet put the prompt router above normal queue selection.
6. `CONTEXT_INDEX.md` did not include the router/fast path in always-read context.

## Fixes made

- Added `PROMPT_QUEUE_ROUTER.md`.
- Added `NEXT_PROMPT_FAST_PATH.md`.
- Updated `agentwatch_mvp.md` to block AW-002+ until validation evidence exists.
- Updated `bootstrap_validation.md` to use router and fast path.
- Updated `productization.md` to use router and concrete prompt files where available.
- Updated `DOCS_INDEX.md` to index router and fast path.
- Updated `CONTEXT_INDEX.md` to route through router and fast path.
- Updated `AGENTS.md` working order to use router first.
- Added individual prompts for AW-002, AW-003, AW-005, PROD-001, and PROD-003.

## Added prompt files

- `docs/prompts/AW-002-init-hardening.md`
- `docs/prompts/AW-003-git-run-report.md`
- `docs/prompts/AW-005-prompt-optimizer-splitter.md`
- `docs/prompts/PROD-001-help-output.md`
- `docs/prompts/PROD-003-status-non-git.md`

## Current next prompt

`AW-VAL-001 — Build validation` remains the only safe next implementation-adjacent prompt.

## Blocked attempts

Some longer prompt index/review prompt files were blocked by safety checks. Mitigation: keep prompts smaller and add missing prompt candidates later one by one.

## Remaining gaps

- Add smaller prompt files for PROD-004, PROD-005, PROD-006, PROD-007, and PROD-008 later.
- Add a compact `docs/prompts/README.md` later if safety checks allow it.
- Run actual Gate 0 validation.

## Completion

Done 85% for prompt-system organization.

Tests: not run; docs-only GitHub connector pass.
Missed: runtime validation and a few blocked prompt files.
Follow-up: AW-VAL-001 build validation.
Residual risk: prompt selection is clearer, but build status is still unknown.
