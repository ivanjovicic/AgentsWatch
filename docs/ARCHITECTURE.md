# AgentsWatch Architecture

Last aligned: 2026-09-16

## Goal

AgentsWatch is a local-first, vendor-neutral **run-evidence and completion-verification layer** for delegated coding-agent work.

External agents execute coding work. AgentsWatch owns:
- verification-focused RunContract normalization;
- repository start/end evidence;
- run-interval delta and ambiguity classification;
- compact vendor-neutral RunReceipt;
- deterministic evidence/scope/claim findings;
- compact human-readable projections/handoffs;
- later learning built on trusted evidence and external value.

AgentsWatch does not need to own generic code review, agent execution or session orchestration.

## Architectural style

Use a local-first modular monolith with ports and adapters.

Do not introduce microservices, hosted services, message buses or a proprietary agent runtime for MVP.

## Target logical layers

```text
AgentsWatch.Cli / future GitHub Check / MCP
        |
        v
Application use cases
  CreateOrImportContract
  CheckContract
  StartRun
  FinishRun
  BuildReceipt
  CheckEvidence
  CheckScopeDrift
  CheckClaims
  CreateHandoff
        |
        v
Domain
  RunContract
  RunBaseline
  RunDelta
  RunReceipt
  ValidationEvidence
  Finding
  RunDecision
        |
        v
Ports
  IRepositoryEvidenceReader
  IContractStore
  IRunStore
  IReportWriter
  IValidationEvidenceSource
  IClock
        |
        v
Adapters
  Git CLI
  local JSON/file system
  Markdown projection
  .NET / Flutter validation adapters
  later thin executor/session metadata adapters
```

## Current project mapping

```text
AgentsWatch.Cli
  command parsing and console rendering only

AgentsWatch.Core
  domain models and application use cases

AgentsWatch.Git
  Git repository evidence adapter and interval-delta primitives

AgentsWatch.LanguageAdapters
  stack detection, risk hints and validation suggestions

AgentsWatch.Reports
  Markdown projections and compact handoffs
```

After Gate 0, avoid growing `Program.cs` with domain/application logic.

## Canonical data flow

```text
existing roadmap / issue / prompt
  -> verification RunContract JSON
  -> StartRun baseline
  -> external agent execution
  -> FinishRun end evidence
  -> run-interval RunDelta
  -> compact RunReceipt JSON
  -> Evidence / Scope / Claims checks
  -> auditable RunDecision
  -> Markdown evidence projection / handoff
  -> later optional learning
```

## Critical run-evidence rule

Raw end-of-run `git status` is not evidence of what changed during the recorded interval.

More importantly:

> **A change observed during the run interval is not automatically proven to be authored by the selected agent.**

Example:

```text
before start:
 M src/UserService.cs

during interval:
 - external agent changes src/OrderService.cs
 - formatter or human may also write files
```

StartRun records a baseline capable of distinguishing:
- pre-existing staged changes;
- pre-existing unstaged changes;
- pre-existing untracked files;
- HEAD/branch.

FinishRun compares end evidence and produces classifications such as:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Executor-specific authorship is optional evidence and must not be invented from Git snapshots alone.

Material ambiguity remains explicit and may require human review.

## Git evidence contract

Prefer machine-safe, lossless porcelain such as:

```bash
git status --porcelain=v1 -z -uall
```

plus targeted diff/fingerprint commands as required.

Do not parse by trimming fixed-width status prefixes.

Adapter must handle at minimum:
- clean repository;
- staged modification;
- unstaged modification;
- partial staged/unstaged state where relevant;
- add/delete;
- rename;
- untracked files;
- filenames with spaces;
- cross-platform paths;
- pre-existing dirty files;
- changed-further dirty files;
- explicit ambiguity cases.

## RunContract architecture

RunContract is a verification normalization layer, not a new project-management database.

Preferred flow:

```text
GitHub/Jira/Linear issue or prompt
  -> importer/manual normalization
  -> verification-specific fields only
```

Core should allow future source-task adapters without coupling the domain to a specific tracker.

## Storage

Canonical machine state:

```text
.agentwatch/
  contracts/<contract-id>.json
  active-runs/<run-id>.json
  runs/<run-id>.json
```

Human projections:

```text
.ai/
  runs/<run-id>.md
  handoffs/<run-id>.md
```

Rule:

```text
JSON = verification source of truth
Markdown = projection / handoff
```

SQLite may be added later only when stable schemas and real query needs justify it.

## RunReceipt boundary

Canonical RunReceipt is a compact evidence artifact.

It contains repository/evidence/claim/scope/decision truth needed by developer, reviewer, CI or audit consumers.

Do not make these mandatory canonical receipt fields:
- learning note;
- next prompt;
- route suggestion;
- optimization advice;
- full session narrative.

Those may be derived downstream into handoff/learning outputs.

## Deterministic verification first

MVP verification works offline and without an LLM provider key.

Deterministic/core checks:
- contract completeness;
- repository evidence/fingerprints;
- required validation present/missing;
- owned/avoid path rules;
- narrow claim classes vs interval evidence;
- expected evidence present/missing;
- status reasons.

AI-assisted later:
- free-text claim extraction;
- semantic acceptance analysis;
- explanation;
- suggested follow-up.

AI output must not silently upgrade `Unknown` / `NeedsReview` to `Supported` / `Done`.

## Validation adapters

Initial priority:
1. universal Git behavior;
2. .NET;
3. Flutter.

Adapters suggest validation by default. Execution remains explicit.

## Future interfaces

After internal contracts and external value stabilize:
- GitHub Check/Action can consume/export findings/receipts;
- MCP can expose stable use cases;
- thin vendor adapters can import executor/session metadata;
- organization policy/evidence services can be added if commercial gates pass;
- a dashboard may read structured receipts only when users prove which views matter.

## Packaging direction

Leading hypothesis is open core after validation:
- inspectable local verifier/schemas/rules;
- paid organization policy, managed evidence, cross-repo controls, compliance/team features later.

Packaging is not an MVP architecture requirement.

## Non-goals

Do not make architecture depend on:
- proprietary agent loop;
- generic AI code-review engine;
- cloud sandbox/workspace management;
- generic workflow orchestration;
- hosted database;
- message bus/microservices;
- SaaS authentication/billing;
- full chat capture;
- generic observability trace store;
- generic AI-code contribution analytics.

Latest strategy guardrails:
`DEEP_DIVE_DECISIONS_2026_09_16.md`
