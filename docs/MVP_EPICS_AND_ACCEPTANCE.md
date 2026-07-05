# AgentsWatch MVP Epics and Acceptance Criteria

Last aligned: 2026-07-05  
Status: implementation planning contract

## Purpose

Turn the roadmap into implementation-ready epics with observable acceptance criteria.

Do not start a later epic merely because it is documented. Gate prerequisites, capability maturity, and proof requirements apply.

## Epic 0 — Bootstrap validation

Goal: prove the skeleton can be safely extended.

Stories:

- restore/build/test;
- CLI smoke;
- retain commit-bound evidence;
- update risk/limitations;
- fix only proof failures.

Acceptance:

- restore/build/test results;
- help/version/optimize/status smoke;
- remaining risks documented;
- package/clean-install proof where claimed.

Current note: the last executed proof applies to an earlier head. The run-evidence feature branch requires its own proof.

## Epic 1 — Safe workspace init

Goal: make `agentswatch init` safe and testable.

Stories:

- create workspace folders/files;
- preserve existing files;
- temp-directory tests;
- clear created/preserved output.

Acceptance:

- idempotent;
- no-overwrite tested;
- paths match config contract;
- Windows/Unix compatibility;
- no writes outside selected root.

## Epic 2 — Repository status and adapter detection

Goal: show useful local state without running validation.

Stories:

- Git/non-Git handling;
- branch/object/status;
- project types;
- validation suggestions;
- risk summary.

Acceptance:

- clean/dirty/non-Git scenarios;
- .NET/Flutter detection;
- stable labels;
- no automatic validation.

## Epic 3 — Prompt optimizer v1

Goal: convert rough prompts into safer focused prompts.

Stories:

- classify risk;
- list waste causes;
- recommend budget;
- generate scope/stop/validation fields;
- suggest split.

Acceptance:

- broad multi-mode prompt high risk;
- scoped prompt expected lower risk;
- missing scope/stop/validation detected;
- no invented repo paths;
- examples and golden cases.

## Epic 4 — Task split command

Goal: write scoped Markdown task prompts.

Stories:

- investigation;
- implementation;
- tests;
- diff-only review;
- no overwrite.

Acceptance:

- four expected files;
- prompt quality basics;
- required scope/stop/validation fields;
- no-overwrite tests.

## Epic 5A — Run evidence foundation

Goal: create the smallest usable application flow before command/claim/PR evidence.

Commands:

```text
agentswatch start <task-id> [--title] [--scope]...
agentswatch finish <task-id>
agentswatch report [task-id]
```

Stories:

- versioned run manifest;
- path-safe task ID;
- one active run;
- clean attributable baseline;
- start/end branch and full Git object IDs;
- tracked/untracked change set;
- managed-artifact exclusion;
- scope matching/out-of-scope findings;
- atomic JSON/Markdown writes;
- latest report selection;
- explicit `Validation: NotRun`;
- local-root privacy.

Acceptance:

- contracts in `RUN_EVIDENCE_FOUNDATION_CONTRACT.md`;
- scenarios in `RUN_EVIDENCE_ACCEPTANCE_SCENARIOS.md`;
- Linux/Windows targeted tests;
- duplicate/overlap/dirty/no-overwrite behavior;
- rename/delete/untracked/scope fixtures;
- no absolute root in artifacts;
- no claim that build/test/CI/runtime validation occurred.

Current status: source and tests authored on `feature/pr-evidence-run-foundation`; execution proof pending, so AW-CAP-012..014 remain L2.

## Epic 5B — Command and validation evidence

Goal: make validation claims supportable with observed command evidence.

Prerequisite: Epic 5A passes targeted and black-box proof.

Stories:

- `CommandEvidence` schema;
- explicit `agentswatch run -- <command>`;
- safe argument/process execution;
- timeout/cancellation/refusal;
- redacted compact output;
- stdout/stderr byte counts;
- start/end Git object binding;
- append-only command history;
- validation projection;
- stale evidence after source/object change.

Acceptance:

- pass/fail/timeout/cancel fixtures;
- secret/huge-output fixtures;
- no full output in Markdown;
- missing command remains NotRun;
- object A validation cannot prove object B;
- Linux/Windows proof.

Backlog: `RUN_EVIDENCE_IMPLEMENTATION_BACKLOG.md`.

## Epic 6 — Handoff and diff-only review

Goal: reduce repeated context and whole-repo review.

Prerequisite: stable run report and evidence projection.

Stories:

- compact handoff;
- diff-only review prompt;
- evidence references;
- claims/missed-test checklist;
- short continuation context.

Acceptance:

- handoff target 10-20 lines;
- changed files only by default;
- no raw logs/chat;
- unknown/unverified state preserved;
- examples.

## Epic 7A — Deterministic risk and claims

Goal: catch common completion-integrity failures without opaque scoring.

Prerequisite: Git and command/validation evidence.

Stories:

- structured claim input;
- claim/evidence links;
- validation claim without evidence;
- stale commit evidence;
- changed files outside scope;
- tests-claimed mismatch;
- transparent risk reasons.

Acceptance:

- fixtures for every claim status;
- missing observation differs from contradiction;
- stale evidence names object mismatch;
- findings map to reviewer actions;
- no generic model-only bug review requirement.

## Epic 7B — Local PR Evidence Packet

Goal: deliver the first market-facing result locally.

Prerequisites: Epics 5A, 5B, and 7A.

Command target:

```text
agentswatch pr evidence --run <task-id> --base <branch-or-object>
```

Acceptance:

- declared scope and actual changes;
- command/build/test/CI evidence where available;
- commit/object matching;
- claim classifications;
- missing/stale evidence;
- reviewer action list;
- Markdown and JSON;
- no source/full logs uploaded or embedded;
- internal dogfood before GitHub automation.

## Epic 8 — Packaging, proof, and internal dogfood

Goal: make the local CLI usable and prove it on real repositories.

Stories:

- package current implementation;
- clean-install current package;
- run AgentsWatch on AgentsWatch;
- run on MathLearning and/or Trendplus;
- collect at least 5 real run/PR evidence cases;
- measure false positives/manual work;
- decide next automation from evidence.

Acceptance:

- package checksum and clean install;
- current-commit Linux/Windows proof;
- at least two repositories dogfooded;
- five real reports reviewed;
- limitations and missed evidence recorded;
- no dashboard/GitHub Action decision based only on roadmap.

## Epic 9 — Market validation and requested automation

Goal: validate repeat use and reviewer decision impact.

Use `PR_EVIDENCE_MARKET_VALIDATION_RUNBOOK.md`.

Sequence:

1. 5 internal/close design-partner cases;
2. revise packet and false positives;
3. expand to 30 real AI-assisted PRs;
4. measure repeat use, decision impact, and payment/budget signal;
5. build GitHub Action only after automation demand.

Infrastructure gates remain in `MVP_ROADMAP.md` and the product form-factor plan.

## Explicitly deferred

- dashboard/daemon before repeated-history/live-event need;
- GitHub App before Action/manual paid demand;
- Team Server before shared-metadata and budget evidence;
- full policy enforcement;
- multi-agent orchestration;
- generic AI bug-review competition;
- exact billing reconstruction from incomplete telemetry.
