# AgentsWatch MVP Epics and Acceptance Criteria

Last aligned: 2026-06-29  
Status: implementation planning contract

## Purpose

Turn the roadmap into implementation-ready epics with acceptance criteria.

Do not start an epic until its gate prerequisites are met.

---

## Epic 0 — Bootstrap validation

Goal: prove the skeleton can be safely extended.

Prerequisite: none.

Stories:

- validate restore/build/test;
- validate CLI smoke;
- record CI or local evidence;
- update risk register;
- fix only build/test/smoke failures.

Acceptance criteria:

- `dotnet restore AgentsWatch.sln` result recorded;
- `dotnet build AgentsWatch.sln` result recorded;
- `dotnet test AgentsWatch.sln` result recorded;
- `--help`, `--version`, `optimize`, and `status` smoke results recorded;
- remaining risks documented.

Exit: Gate 0 passes or has explicit blockers.

---

## Epic 1 — Safe workspace init

Goal: make `agentswatch init` safe and testable.

Prerequisite: Gate 0 evidence exists.

Stories:

- create workspace folders;
- write default config/status/changelog/checklist;
- preserve existing files;
- support temp-directory tests;
- print clear created/preserved output.

Acceptance criteria:

- init is idempotent;
- no-overwrite behavior has tests;
- output paths match `CONFIG_REFERENCE.md`;
- UX output matches `CLI_UX_OUTPUT_SPEC.md`;
- command works on Windows and Unix paths.

---

## Epic 2 — Repo status and adapter detection

Goal: show useful local repo state without running validation automatically.

Stories:

- detect git repo / non-git folder;
- read branch, commit, changed files;
- detect project types;
- suggest validation commands;
- show risk summary.

Acceptance criteria:

- clean repo test;
- dirty repo test;
- non-git directory behavior documented/tested;
- .NET and Flutter detection tested;
- UX output has stable labels.

---

## Epic 3 — Prompt optimizer v1

Goal: convert rough prompts into safer prompts.

Stories:

- classify prompt risk;
- list waste causes;
- recommend token budget;
- generate scope limiter;
- generate optimized prompt;
- suggest split for high-risk prompts.

Acceptance criteria:

- broad repo prompt is high risk;
- scoped prompt with validation is low risk;
- missing validation/stop/scope are detected;
- output includes run mode, budget, scope, stop rules, validation;
- examples exist under `examples/`.

---

## Epic 4 — Task split command

Goal: write scoped markdown task prompts.

Stories:

- parse rough prompt file;
- generate investigation prompt;
- generate implementation prompt;
- generate test prompt;
- generate diff-only review prompt;
- avoid overwriting by default.

Acceptance criteria:

- four expected files are written;
- each generated file passes `PROMPT_QUALITY_CHECKLIST.md` basics;
- no-overwrite behavior is tested;
- generated files reference relevant docs, validation, and stop rules.

---

## Epic 5 — Run start/finish evidence

Goal: make every AI-agent run auditable.

Stories:

- record start snapshot;
- record end snapshot;
- compare changed files;
- record validation status;
- record missed/follow-up/risk;
- write run report.

Acceptance criteria:

- start captures branch/commit/status;
- finish writes report path;
- changed files are listed;
- validation not run is represented honestly;
- report format matches `REPORT_FORMATS.md`.

---

## Epic 6 — Handoff and diff-only review

Goal: reduce repeated context and whole-repo reviews.

Stories:

- generate handoff from latest run;
- generate diff-only review prompt;
- include claims-vs-actual checklist;
- include missed-test checklist;
- keep handoff short.

Acceptance criteria:

- handoff targets 10-20 lines;
- diff-review prompt forbids whole-repo scan by default;
- generated content references changed files;
- examples exist.

---

## Epic 7 — Risk scoring and claims-vs-actual

Goal: catch common AI-agent mistakes.

Stories:

- classify high-risk file categories;
- detect tests not changed with runtime changes;
- detect docs/runtime mismatch;
- detect validation claim without evidence;
- write review findings.

Acceptance criteria:

- mismatch examples are tested;
- risk reasons are transparent;
- findings map to follow-up prompts;
- no opaque AI scoring in MVP.

---

## Epic 8 — Packaging and dogfood

Goal: make AgentsWatch usable locally.

Stories:

- pack CLI as local tool;
- test local install;
- dogfood on AgentsWatch;
- dogfood on MathLearning;
- collect 5 real run reports;
- decide dashboard later.

Acceptance criteria:

- `dotnet pack` evidence recorded;
- local tool install evidence recorded;
- at least two repos dogfooded;
- dashboard decision uses evidence, not assumptions.
