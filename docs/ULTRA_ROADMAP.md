# AgentsWatch Ultra Roadmap

Last aligned: 2026-06-29  
Status: strategic roadmap  
Scope: local CLI first, local dashboard second, team/SaaS last

## North Star

AgentsWatch helps developers spend fewer AI-agent tokens and merge safer AI-generated code.

Core promise:

```text
Spend fewer tokens. Merge safer AI code.
```

Primary wedge:

```text
AI coding-agent supervisor + token optimizer for multi-file development tasks.
```

## Product principles

1. Local-first before cloud.
2. CLI before dashboard.
3. Git evidence before AI interpretation.
4. Markdown prompts before LLM integrations.
5. Heuristic and explainable before smart and opaque.
6. Diff-only review before whole-repo review.
7. Handoff summaries before long chat history.
8. Validation evidence before “Done”.
9. One run mode per task: investigate, implement, test, review, docs.
10. Token budget and scope limiter on every non-trivial run.

---

## Phase 0 — Bootstrap validation

Goal: prove the generated skeleton is buildable and safe to extend.

Status: required before runtime feature expansion.

Must pass:

- restore/build/test;
- CLI smoke;
- validation evidence review;
- risk register update.

Key docs:

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/prompt_queues/bootstrap_validation.md`

Exit criteria:

- `dotnet restore AgentsWatch.sln` is verified;
- `dotnet build AgentsWatch.sln` is verified;
- `dotnet test AgentsWatch.sln` is verified;
- `agentswatch --help`, `--version`, `optimize`, and `status` are smoke-tested;
- any build/smoke failures become narrow follow-up prompts.

---

## Phase 1 — CLI foundation

Goal: make AgentsWatch useful locally without external services.

Milestones:

1. `agentswatch init` is safe and idempotent.
2. `agentswatch status` detects project type and git state reliably.
3. `agentswatch optimize` creates useful prompt-risk output.
4. `agentswatch task split` writes markdown task files.
5. `agentswatch start` records a run start snapshot.
6. `agentswatch finish` records changed files and validation notes.
7. `agentswatch report` writes markdown reports.
8. `agentswatch handoff` writes compact summaries.
9. `agentswatch review-diff` creates diff-only review prompts.

Definition of done:

- works on a sample .NET repo;
- works on a sample Flutter repo;
- no cloud dependency;
- reports are markdown-first;
- no user files overwritten;
- all commands have tests or smoke evidence.

---

## Phase 2 — Token optimizer MVP

Goal: prove the token-saving value.

Features:

- risk checker for raw prompts;
- prompt splitter;
- token budget levels;
- scope limiter templates;
- investigation-only prompt generator;
- implementation prompt generator;
- test prompt generator;
- diff-only review prompt generator;
- handoff summary generator;
- token waste report.

Output examples:

```text
Risk: High
Budget: low
Waste causes:
- broad scope
- missing stop rules
- multiple task modes
Suggested split:
- 001-investigate-only.md
- 002-implement-minimal-fix.md
- 003-add-tests.md
- 004-diff-only-review.md
```

Success metric:

- user can convert one rough prompt into 3-4 scoped prompts in under one minute;
- next agent run reads a handoff instead of a long chat;
- review prompt is limited to changed files.

---

## Phase 3 — Git evidence and run reports

Goal: make every AI-agent run auditable.

Features:

- start/end git snapshots;
- changed file classification;
- validation evidence fields;
- missed-test detection heuristic;
- claimed-vs-actual diff check;
- AI changelog;
- run status page in markdown;
- risk report per run.

Risk signals:

- tests claimed but no test files changed;
- backend claimed but only frontend changed;
- docs claimed but no docs changed;
- validation claimed but no validation evidence recorded;
- high-risk files touched;
- too many files changed for budget;
- no handoff after long run.

Success metric:

- every run leaves a short report that a developer can review without reading the full chat.

---

## Phase 4 — Language adapters

Goal: useful validation suggestions across common stacks.

Adapter order:

1. universal git/files adapter;
2. .NET adapter;
3. Flutter adapter;
4. React/TypeScript adapter;
5. Node adapter;
6. Python adapter;
7. later: Java, Go, Rust.

Adapter responsibilities:

- detect project type;
- suggest validation commands;
- identify high-risk files;
- identify likely test paths;
- avoid running broad commands unless configured.

Non-goal:

- deep static analysis in MVP.

---

## Phase 5 — Local dashboard

Goal: make local run history easy to inspect.

Prerequisite: CLI has real dogfood usage.

Suggested stack:

- local .NET API;
- React dashboard;
- SQLite storage;
- optional file watcher.

Pages:

- Runs;
- Tasks;
- Changed files;
- Risk findings;
- Token waste;
- Validation;
- Handoffs;
- Settings.

Dashboard must remain local-first. No cloud account required.

---

## Phase 6 — PR and team workflow

Goal: help teams review AI-assisted code.

Features:

- GitHub PR diff ingestion;
- PR risk report;
- CI status ingestion;
- missing-test warnings;
- policy rule checks;
- exportable markdown report;
- optional PR comment draft.

Team edition only after CLI/dashboard prove usefulness.

---

## Phase 7 — SaaS and commercial product

Only after there is clear demand.

Possible features:

- accounts;
- billing;
- organization policies;
- cloud run history;
- GitHub App integration;
- hosted dashboards;
- team analytics;
- enterprise controls.

Do not start here.

---

## Monetization hypothesis

Free/local:

- CLI core;
- markdown reports;
- prompt optimizer;
- local handoffs.

Paid Pro:

- advanced dashboard;
- richer risk scoring;
- cross-repo history;
- report templates;
- configurable policies;
- productivity analytics.

Team:

- GitHub PR integration;
- org policies;
- shared dashboards;
- audit history;
- CI/PR annotations.

---

## Roadmap guardrails

Do not build:

- SaaS before CLI value is proven;
- dashboard before run reports are useful;
- automatic code editing in v1;
- opaque AI scoring without transparent reasons;
- token claims without visible waste metrics;
- provider-specific integration before markdown workflow works.

---

## Roadmap success checkpoint

AgentsWatch is ready for broader testing when:

- a developer can initialize a repo;
- optimize a rough prompt;
- split it into tasks;
- run an AI agent manually;
- record changed files;
- generate a report;
- generate a handoff;
- review a diff-only prompt;
- repeat the workflow on a second repo.
