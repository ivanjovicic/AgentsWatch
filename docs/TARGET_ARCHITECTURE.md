# AgentsWatch Target Architecture

Last aligned: 2026-06-29  
Status: target architecture / future-proof design  
Scope: CLI MVP, local dashboard, team/PR workflow, and future SaaS

## Executive summary

AgentsWatch should be designed as a local-first, evidence-driven developer tool with a clean path to dashboard, team, and SaaS editions.

The architecture should not be built around a single CLI command. It should be built around stable domain concepts:

- prompt optimization;
- task splitting;
- run tracking;
- git evidence;
- validation evidence;
- risk findings;
- reports;
- handoff summaries;
- adapters;
- policy rules.

The CLI is only the first interface. The same application core should later support a local dashboard, GitHub PR analysis, CI ingestion, and team policies.

---

## Architectural goals

1. Keep the MVP local-first and useful without cloud accounts.
2. Keep domain logic independent from console, file system, git process calls, and future web APIs.
3. Make every output auditable: reports should explain why a risk was raised.
4. Prefer markdown outputs first, JSON sidecars second, SQLite third.
5. Let adapters add stack-specific knowledge without owning core workflow.
6. Support future dashboard/API without rewriting the CLI core.
7. Support future SaaS by adding sync/integration boundaries later, not by coupling MVP to cloud early.
8. Keep privacy boundaries strict: no source upload or telemetry by default.

---

## Recommended architecture style

Use a modular monolith with clean architecture / ports-and-adapters boundaries.

For the first year, do not split into microservices. The product needs fast iteration, reliable local behavior, and clear module boundaries more than distributed complexity.

Recommended layers:

```text
Interfaces
  CLI
  future local API
  future dashboard
  future GitHub App webhook/API

Application
  use cases / workflows
  command handlers
  orchestration

Domain
  prompt optimization
  tasks and runs
  evidence model
  risk model
  policies
  reports

Ports
  git
  file system
  clock
  process runner
  validation runner
  storage
  integrations

Adapters
  git CLI adapter
  markdown file adapter
  JSON/SQLite storage adapter
  language adapters
  GitHub adapter later
  CI adapter later
```

Dependency rule:

```text
Interfaces -> Application -> Domain
Adapters -> Ports
Domain must not depend on CLI, git process, file system, SQLite, HTTP, or GitHub.
```

---

## Bounded contexts

### 1. Prompt Optimization

Owns:

- prompt risk classification;
- waste cause detection;
- token budget recommendation;
- scope limiter generation;
- task split generation;
- optimized prompt output.

Does not own:

- git snapshots;
- report writing;
- validation command execution;
- storage.

### 2. Task and Run Tracking

Owns:

- task IDs;
- run IDs;
- run mode;
- token budget;
- start/end lifecycle;
- run status transitions.

Does not own:

- language-specific validation rules;
- risk scoring details;
- dashboard rendering.

### 3. Evidence Collection

Owns:

- start/end git snapshot;
- changed file list;
- validation evidence;
- command output summaries;
- CI/PR evidence later.

Does not own:

- final risk decision;
- prompt generation.

### 4. Risk and Policy

Owns:

- risk scoring;
- policy rules;
- high-risk category detection;
- claimed-vs-actual mismatch;
- missing-test heuristic;
- stop-rule findings.

Does not own:

- reading git directly;
- writing markdown reports directly.

### 5. Reporting and Handoff

Owns:

- run report formatting;
- handoff summary formatting;
- AI changelog entries;
- diff-only review prompt output.

Does not own:

- underlying risk calculation;
- git process execution.

### 6. Adapters

Owns:

- project type detection;
- suggested validation commands;
- stack-specific risk patterns;
- likely test path hints.

Does not own:

- command orchestration;
- domain model definitions;
- report lifecycle.

### 7. Integrations Later

Future modules:

- GitHub PR ingestion;
- GitHub Actions/CI evidence;
- LLM provider prompt export/import;
- cloud sync;
- team policies.

These must be optional adapters, not core dependencies.

---

## Recommended solution structure

Near-term:

```text
src/
  AgentsWatch.Cli/
  AgentsWatch.Application/
  AgentsWatch.Domain/
  AgentsWatch.Abstractions/
  AgentsWatch.Git/
  AgentsWatch.FileSystem/
  AgentsWatch.LanguageAdapters/
  AgentsWatch.Reports/
  AgentsWatch.Storage.Markdown/
  AgentsWatch.Storage.Json/

tests/
  AgentsWatch.Domain.Tests/
  AgentsWatch.Application.Tests/
  AgentsWatch.Cli.Tests/
  AgentsWatch.Git.Tests/
  AgentsWatch.Reports.Tests/
  AgentsWatch.Integration.Tests/
```

Current repo can evolve gradually from the existing projects. Do not rename projects before Gate 0 passes.

Evolution from current skeleton:

```text
AgentsWatch.Core -> split later into Domain + Application + Abstractions
AgentsWatch.Git -> Git adapter
AgentsWatch.Reports -> Reporting module
AgentsWatch.LanguageAdapters -> Adapter registry
AgentsWatch.Cli -> thin CLI interface
```

---

## Interface strategy

### CLI

Primary interface for MVP.

CLI should be thin:

- parse arguments;
- call application use cases;
- render console output;
- return exit codes.

CLI should not contain domain logic, risk rules, or file format logic long-term.

### Local API later

For dashboard, expose a local-only API over the same application use cases.

Possible project later:

```text
AgentsWatch.LocalApi
```

It should read local storage and never require cloud auth.

### Dashboard later

Dashboard should be a viewer/controller for local data:

- runs;
- tasks;
- risk findings;
- validation;
- handoffs;
- settings.

It should not become the only way to use the product.

### GitHub App later

GitHub integration should be adapter-based:

- read PR diffs;
- read check runs;
- generate PR review report;
- optionally draft PR comments.

Do not couple local CLI to GitHub App auth.

---

## Storage strategy

### Phase 1 — Markdown source of truth

Use current files:

```text
.ai/tasks/
.ai/runs/
.ai/generated/
.ai/STATUS.md
.ai/CHANGELOG_AI.md
```

Best for transparency, git-friendly history, and user trust.

### Phase 2 — JSON sidecars

Add machine-readable outputs:

```text
.agentwatch/runs/<run-id>.json
.agentwatch/tasks/<task-id>.json
```

Markdown remains user-facing. JSON powers dashboard and later imports.

### Phase 3 — SQLite local store

Use for dashboard and history queries:

```text
.agentwatch/agentswatch.db
```

SQLite is local-only. It should be rebuildable from markdown/JSON where possible.

### Phase 4 — Cloud sync later

Only for paid/team product.

Cloud should sync metadata and reports only after explicit user opt-in. Source code and full diffs should remain local unless the user explicitly enables PR integration.

---

## Policy engine design

Policy rules should be explainable and deterministic first.

Rule examples:

- runtime files changed and no tests changed;
- high-risk path changed;
- prompt budget exceeded;
- more than N files changed;
- claimed tests but no test files changed;
- diff-only review requested but whole repo was inspected;
- validation not recorded;
- secrets-like path changed.

Recommended model:

```text
PolicyRule
  Id
  Name
  Severity
  AppliesTo
  Evaluate(context) -> Finding[]
```

Keep this as an internal domain pattern first. Do not build a full user policy DSL until real use cases prove it is needed.

---

## Plugin / adapter model

Adapters should implement small contracts:

```text
IProjectDetector
IValidationSuggester
IRiskPatternProvider
ITestPathSuggester
IReportLabelProvider
```

A registry combines all matching adapters:

```text
AdapterRegistry
  DetectProjectTypes(root)
  GetValidationSuggestions(context)
  GetRiskPatterns(context)
```

Adapters must never own core task/run state.

---

## Command orchestration

Application use cases should represent commands:

```text
InitProjectUseCase
OptimizePromptUseCase
SplitTaskUseCase
StartRunUseCase
FinishRunUseCase
GenerateReportUseCase
GenerateHandoffUseCase
GenerateDiffReviewUseCase
ValidateUseCase
```

Each use case should depend on ports, not concrete file/git/process implementations.

Example:

```text
FinishRunUseCase
  input: task id, validation notes, optional claims
  reads: start snapshot, current git snapshot
  calculates: changed files, risk findings, missed tests
  writes: run report, handoff summary, status/changelog update
```

---

## Privacy and security boundaries

Default behavior:

- no telemetry;
- no cloud sync;
- no hidden network calls;
- no LLM credentials required;
- do not upload source code;
- do not write secret values into reports.

Security-sensitive paths should be represented as metadata-only findings:

```text
Path: appsettings.Production.json
Risk: High
Reason: production config changed
Content included: no
```

---

## Future SaaS architecture

Only after CLI/dashboard proves value.

Recommended path:

```text
Local CLI + local store
  -> optional local dashboard
  -> GitHub PR report generator
  -> hosted team dashboard
  -> organization policies and billing
```

Cloud components later:

```text
AgentsWatch.Api
AgentsWatch.Workers
AgentsWatch.Web
AgentsWatch.Billing
AgentsWatch.GitHubApp
AgentsWatch.Sync
```

Cloud data should start with metadata:

- project id;
- run id;
- timestamps;
- risk categories;
- validation statuses;
- aggregate savings signals.

Do not upload full source/diffs by default.

---

## Reliability principles

- Commands should be idempotent where possible.
- `init` must never overwrite user files by default.
- Report writes should be atomic where possible.
- Failed validation should not corrupt run history.
- A run should be resumable or clearly marked incomplete.
- Every blocked state should have a follow-up prompt.

---

## Observability for the tool itself

Local logs later:

```text
.agentwatch/logs/<date>.log
```

Do not log secrets or full command output by default.

Track locally:

- command started/finished;
- duration;
- output path;
- warning count;
- risk count;
- validation status.

---

## Architecture decisions

Current target decisions:

1. Modular monolith, not microservices.
2. Local-first CLI before dashboard.
3. Markdown reports before database.
4. Deterministic heuristics before LLM-dependent analysis.
5. Ports/adapters for git, file system, process, storage, integrations.
6. Dashboard reads same application model, not separate logic.
7. Cloud/SaaS added only after dogfood evidence.

---

## Migration plan from current skeleton

Do not refactor immediately. First pass Gate 0.

After Gate 0:

1. Extract application use cases from `Program.cs` gradually.
2. Keep existing `AgentsWatch.Core` while introducing domain/application namespaces.
3. Move prompt risk logic behind an application service.
4. Move init file writes behind file-system ports.
5. Add report writer contracts before adding JSON/SQLite.
6. Add adapter registry before adding more stack-specific logic.
7. Add dashboard/API only after run report workflow is useful.

---

## Anti-patterns to avoid

- Putting all logic in CLI `Program.cs`.
- Making adapters mutate core state directly.
- Making dashboard use different logic than CLI.
- Adding cloud sync before local trust exists.
- Writing secret values to markdown reports.
- Adding LLM provider dependencies to the core domain.
- Creating a policy DSL before simple rules prove insufficient.
- Building SaaS around unvalidated token-savings claims.
