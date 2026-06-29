# AgentsWatch Docs Governance

Last aligned: 2026-06-29

## Purpose

Keep AgentsWatch documentation useful, current, and small enough for agents to read.

## Source-of-truth rule

When documents disagree, use this order:

1. current code and tests;
2. `AGENTS.md`;
3. `docs/DOCS_INDEX.md`;
4. `docs/AGENT_OPERATING_SYSTEM.md` and `docs/CONTEXT_INDEX.md`;
5. bootstrap validation docs while Gate 0 is incomplete;
6. architecture and product contract docs;
7. roadmap and planning docs.

## Required docs update triggers

Update docs when changing:

- CLI command behavior;
- config shape;
- report format;
- data model;
- adapter detection;
- risk scoring;
- validation commands;
- roadmap gates;
- prompt queues.

## Broken reference rule

If a document references a file that does not exist, fix one of these:

1. create the missing file if it should exist;
2. replace the reference with the current file;
3. remove the reference if obsolete.

Do not leave known broken references for later.

## Docs size rule

Prefer short focused docs over one huge doc.

Use:

- `DOCS_INDEX.md` for navigation;
- `CONTEXT_INDEX.md` for task-specific read sets;
- topic docs for contracts and rules.

## Planning vs authority

Roadmap docs are plans, not proof.

Validation evidence, tests, CI, and real run reports are stronger than roadmap claims.

## Completion rule

Docs-only work should still report:

- files changed;
- what changed;
- validation run/not run;
- commit SHA;
- completion percentage;
- missed work;
- follow-up;
- residual risk.
