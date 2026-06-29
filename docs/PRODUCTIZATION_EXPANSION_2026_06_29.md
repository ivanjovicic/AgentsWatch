# AgentsWatch Productization Expansion — 2026-06-29

## Scope

User request: add the next massive and useful improvements for the project.

This pass intentionally did not modify runtime code because Gate 0 validation is still incomplete.

## Added docs

Product contracts:

- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/RISK_SCORING_MODEL.md`
- `docs/TOKEN_WASTE_METRICS.md`

Execution planning:

- `docs/MVP_EPICS_AND_ACCEPTANCE.md`
- `docs/ISSUE_BACKLOG.md`
- `docs/prompt_queues/productization.md`

Product and market planning:

- `docs/USER_PERSONAS_AND_JOBS.md`
- `docs/POSITIONING_AND_PRICING_HYPOTHESES.md`

Delivery and adoption:

- `docs/RELEASE_AND_PACKAGING_PLAN.md`
- `docs/EXAMPLES_CATALOG.md`
- `docs/INTEGRATION_STRATEGY.md`
- `docs/DOGFOOD_RUNBOOK.md`

Examples:

- `examples/rough-prompts/broad-repo-fix.md`

Index update:

- `docs/DOCS_INDEX.md`

## Why this is useful

The project now has:

- exact CLI command contracts before implementation;
- expected CLI output and stable test anchors;
- issue-sized backlog;
- MVP epics with acceptance criteria;
- risk scoring rules;
- token waste metrics;
- release and packaging path;
- integration order that keeps MVP local-first;
- dogfood workflow and evidence template;
- user/persona positioning;
- productization prompt queue.

## Blocked attempts

Two larger example files were blocked while writing prompt/report examples:

- optimized prompt split example;
- sample run report example.

Mitigation: added `EXAMPLES_CATALOG.md` and one simpler rough prompt example. Add more examples later as smaller files after Gate 0 validation.

## Remaining gap

Gate 0 is still incomplete. Build/test/smoke validation remains the highest-priority next step.

Required next evidence:

- restore result;
- build result;
- test result;
- CLI smoke result;
- validation evidence update.

## Completion

Done 90% for productization/docs expansion.

Tests: not run; docs-only GitHub connector pass.
Missed: runtime validation, blocked example files, README link update.
Follow-up: AW-VAL-001 build validation.
Residual risk: project is better specified, but actual build status is still unknown.
