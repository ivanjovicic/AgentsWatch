# AgentsWatch 90-Day Execution Plan

Last aligned: 2026-06-29  
Status: tactical execution plan

## Goal

In 90 days, AgentsWatch should be a useful local CLI for solo developers, with enough dogfood evidence to decide whether a dashboard or paid product is worth building.

## Workstream map

| Workstream | Goal |
|---|---|
| Bootstrap | Validate solution, tests, CLI smoke, CI. |
| CLI Core | Commands, config, init, status, start/finish/report. |
| Prompt Optimizer | Risk, split, budget, scope, handoff, review prompts. |
| Git Evidence | Snapshots, diff stats, changed files, risk signals. |
| Adapters | .NET, Flutter, React/TypeScript, Python validation suggestions. |
| Dogfood | Use AgentsWatch on AgentsWatch and MathLearning. |
| Product | Positioning, examples, landing page copy, pricing hypothesis. |

---

## Weeks 1-2 — Stabilize skeleton

Objectives:

- validate build/test;
- fix solution/project issues;
- run CLI smoke;
- stabilize `init`, `status`, and `optimize` basics.

Tasks:

- complete `AW-VAL-001`;
- complete `AW-VAL-002`;
- complete `AW-VAL-003`;
- complete `AW-VAL-004`;
- add tests for init no-overwrite behavior;
- add tests for git parser edge cases.

Exit criteria:

- CI green;
- CLI help/version works;
- `agentswatch init` works in temp directory;
- `agentswatch optimize` returns risk/split output;
- risk register updated.

---

## Weeks 3-4 — Local CLI MVP

Objectives:

- make the CLI useful without dashboard;
- generate markdown outputs;
- dogfood on at least one repo.

Tasks:

- implement `task split`;
- implement `start` and `finish` run snapshots;
- implement markdown run report;
- implement AI changelog update;
- implement handoff summary output;
- write docs for manual workflow.

Exit criteria:

- raw prompt can become markdown tasks;
- a run can be started and finished;
- report is written under `.ai/runs/`;
- handoff is written and usable in next prompt.

---

## Weeks 5-6 — Token optimizer v1

Objectives:

- improve prompt risk scoring;
- generate higher-quality split prompts;
- produce useful token waste reports.

Tasks:

- expand prompt risk patterns;
- add budget enforcement warnings;
- add scope limiter generator;
- add investigation-only prompt generator;
- add implementation prompt generator;
- add test prompt generator;
- add diff-only review prompt generator;
- add token waste report fields.

Exit criteria:

- high-risk broad prompts are split automatically;
- output prompts include run mode, budget, scope, stop rules, validation;
- waste report shows files inspected/changed and broad commands avoided.

---

## Weeks 7-8 — Git evidence and risk scoring

Objectives:

- make run reports trustworthy;
- detect common AI-agent claim mismatches.

Tasks:

- parse modified/added/deleted/renamed/untracked files;
- classify changed files by stack and risk;
- add claimed-vs-actual diff heuristic;
- detect tests changed/not changed;
- detect high-risk categories;
- generate review checklist per run.

Exit criteria:

- report flags missing tests when runtime changed;
- report flags backend/frontend/docs claim mismatch;
- report lists high-risk file categories and reasons.

---

## Weeks 9-10 — Language adapters

Objectives:

- support common developer stacks with validation suggestions.

Tasks:

- harden .NET detection;
- harden Flutter detection;
- add React/TypeScript detection;
- add Python detection;
- add Node detection;
- map changed files to likely validation commands;
- avoid broad validation unless configured.

Exit criteria:

- AgentsWatch gives useful validation suggestions for .NET, Flutter, React, Python, and Node repos;
- commands are suggestions by default, not unsafe automatic execution.

---

## Weeks 11-12 — Dogfood and packaging

Objectives:

- use AgentsWatch for real work;
- package it for easier local use;
- decide dashboard/no-dashboard.

Tasks:

- dogfood on AgentsWatch repo;
- dogfood on MathLearning Flutter repo;
- dogfood on a .NET backend repo;
- add `dotnet pack` validation;
- test local global tool install;
- write examples in `examples/`;
- write landing-page copy draft.

Exit criteria:

- at least 5 real run reports exist;
- handoff summaries were reused;
- rough estimate of token waste reduction is documented;
- local tool install works;
- decision made: continue CLI or start local dashboard.

---

## 90-day success criteria

AgentsWatch is successful if, by day 90:

- CLI is installable locally;
- `init`, `optimize`, `task split`, `start`, `finish`, `report`, `handoff`, and `review-diff` are useful;
- two real repos were dogfooded;
- run reports catch at least one missed-test or scope-creep issue;
- handoff summaries reduce repeated context in follow-up runs;
- dashboard decision is based on evidence, not excitement.

---

## Do not do in first 90 days

- SaaS;
- billing;
- cloud sync;
- OAuth;
- GitHub App;
- team permissions;
- enterprise policy engine;
- deep IDE plugin;
- automatic code editing;
- opaque AI scoring.
