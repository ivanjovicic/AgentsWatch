# AgentsWatch Feature Portfolio Review — 2026-06-30

Status: product/tooling scope audit  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/AGENTWATCH_FEATURE_PORTFOLIO_REVIEW_2026_06_30.md`

## Executive verdict

AgentsWatch has a strong and useful product direction, but the MVP is at risk of becoming too broad if CLI, dashboard, adapters, validation, SaaS, and PR integration are built too early.

The right first product remains:

```text
local-first CLI + git/diff/run report + prompt optimization + handoff + validation evidence
```

## Enough for MVP

Keep these first:

- local `.ai` init and templates;
- task/run lifecycle;
- git status/diff tracker;
- markdown run report;
- handoff summary;
- prompt optimizer and splitter;
- diff-only review prompt;
- basic risk scoring;
- validation command suggestions;
- local privacy/no-telemetry guarantees.

## Useful but phase 2

Add after build/smoke validation and first dogfood reports:

- mistake ledger and mistake checks;
- evidence lint;
- command profiler / fast validation advisor;
- language adapter refinements;
- claimed-vs-actual checks.

## Too much for MVP

Do not start early with:

- SaaS;
- billing;
- cloud sync;
- hosted dashboards;
- team policy engine;
- automatic code editing;
- deep IDE extension;
- remote mistake database;
- exact token accounting without provider data.

## Scope decision

AgentsWatch should prove value locally before dashboard/team/cloud work.

Recommended next product gates:

1. `AW-VAL-001` restore/build/test validation.
2. `AW-VAL-002` CLI smoke validation.
3. `AW-SCOPE-001` lock MVP scope and non-goals.
4. `AW-LIFECYCLE-001` task/run lifecycle contract.
5. `AW-PRIVACY-001` local privacy contract.
6. Only then continue runtime implementation.

## Dogfood source

MathLearning remains a good dogfood repo for:

- missing run-log detection;
- prompt splitting;
- evidence lint;
- mistake-learning loop;
- token/context waste reporting.

But AgentsWatch product specs and implementation prompts should live in this repo.
