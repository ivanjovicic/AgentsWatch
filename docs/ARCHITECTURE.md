# AgentsWatch Architecture

Last aligned: 2026-08-25

## Goal

AgentsWatch starts as a local, vendor-neutral verification and evidence layer for AI coding-agent runs.

External agents execute coding work. AgentsWatch owns:

- machine-readable run contracts;
- repository start/end evidence and attribution;
- vendor-neutral run receipts;
- deterministic evidence/scope/claim findings;
- compact human-readable reports and handoffs;
- later repository-local learning built on trusted receipts.

## Architectural style

Use a local-first modular monolith with ports and adapters.

Do not introduce microservices, hosted services, or an agent runtime for the MVP.

## Target logical layers

```text
AgentsWatch.Cli / future MCP
        |
        v
Application use cases
  CreateContract
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
```

## Current project mapping

The existing projects remain useful but should evolve:

```text
AgentsWatch.Cli
  command parsing and console rendering only

AgentsWatch.Core
  domain models and application use cases

AgentsWatch.Git
  git repository evidence adapter and attribution primitives

AgentsWatch.LanguageAdapters
  stack detection, risk hints and validation suggestions

AgentsWatch.Reports
  Markdown projections and compact handoffs
```

After Gate 0, avoid growing `Program.cs` with business logic. Introduce application services/use cases and interfaces before adding the verification features.

## Canonical data flow

```text
roadmap / issue / prompt
  -> RunContract v1 JSON
  -> StartRun baseline
  -> external agent execution
  -> FinishRun end state
  -> attributable RunDelta
  -> RunReceipt v1 JSON
  -> Evidence / Scope / Claims checks
  -> auditable RunDecision
  -> Markdown report + handoff
  -> later learning
```

## Critical attribution rule

Raw end-of-run `git status` is not sufficient evidence of what the agent changed.

Example:

```text
before start:
 M src/UserService.cs

during run:
 M src/OrderService.cs
```

The receipt must not attribute `UserService.cs` to the run merely because it is dirty at finish.

`StartRun` must record a baseline capable of distinguishing:

- pre-existing staged changes;
- pre-existing unstaged changes;
- pre-existing untracked files;
- repository HEAD and branch.

`FinishRun` computes attributable delta from start to end. Ambiguous attribution must be represented explicitly rather than guessed.

## Git evidence contract

Prefer a lossless, machine-safe git porcelain format such as:

```bash
git status --porcelain=v1 -z -uall
```

and additional targeted git diff commands/fingerprints as required for attribution.

Do not parse by trimming fixed-width status prefixes.

The adapter must handle at minimum:

- clean repository;
- staged modification;
- unstaged modification;
- add/delete;
- rename;
- untracked files;
- filenames with spaces;
- cross-platform paths;
- pre-existing dirty files.

## Storage

Machine-readable state is canonical from the start of the verification MVP:

```text
.agentwatch/
  contracts/
    <contract-id>.json
  active-runs/
    <run-id>.json
  runs/
    <run-id>.json
```

Human-readable projections:

```text
.ai/
  runs/
    <run-id>.md
  handoffs/
    <run-id>.md
```

Rule:

```text
JSON = source of truth
Markdown = projection
```

Do not make domain verification depend on parsing Markdown.

SQLite may be added after receipt schemas stabilize and local history queries justify it.

## Deterministic verification first

MVP verification should work offline and without an LLM provider key.

Initial checks should be deterministic:

- contract completeness;
- required validation present/missing;
- owned/avoid path violations;
- common claim classes vs attributable diff;
- expected evidence present/missing;
- run status reasons.

LLM-based claim extraction or semantic acceptance analysis may be added later as advisory evidence, not as the sole source of truth.

## Validation adapters

Initial priority:

1. universal git behavior;
2. .NET;
3. Flutter.

Adapters suggest validation by default. Execution remains explicit.

React/TypeScript, Node and Python support may remain available where already inexpensive, but must not block the verification spine.

## Future interfaces

After internal contracts stabilize:

- MCP can expose the same application use cases;
- GitHub checks can consume/export receipts/findings;
- vendor adapters can map session metadata into the same run model;
- a local dashboard can read structured receipts without changing core logic.

## Non-goals

Do not make the architecture depend on:

- a proprietary agent loop;
- cloud sandbox/workspace management;
- generic workflow orchestration;
- hosted database;
- message bus;
- microservices;
- SaaS authentication/billing;
- full chat capture;
- a generic observability trace store.
