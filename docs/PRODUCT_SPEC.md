# AgentsWatch Product Spec

Last aligned: 2026-09-16  
Status: planning/specification with validated skeleton gaps

## Product definition

AgentsWatch is a local-first, vendor-neutral **run-evidence and completion-verification layer for delegated coding-agent work**.

External agents such as Codex, Claude Code, Cursor, Copilot, Devin and OpenHands execute coding work. AgentsWatch does not replace them and does not try to become another generic AI reviewer.

AgentsWatch normalizes the verification contract, records pre/post repository evidence, computes a run-interval repository delta, verifies required validation/scope/completion claims and emits a portable RunReceipt.

## Core promise

```text
Turn delegated coding work into independently verifiable evidence.
```

Supporting principle:

```text
Trust evidence, not the executor's confidence.
```

## Competitive boundary — 2026-09-16

Cursor, GitHub, Qodo and adjacent products already provide increasingly strong:
- agent session logs;
- AI-code attribution/tracking;
- generic code review;
- hooks/policies;
- audit/governance surfaces.

Therefore these are **not** sufficient differentiation.

The surviving thesis is narrower:

> **execution-independent, cross-vendor verification of run scope, run-interval repository evidence, required validation and completion claims.**

Qodo/current independent-review products are explicit validation baselines, not ignored competitors.

Canonical strategy guardrails:
`docs/DEEP_DIVE_DECISIONS_2026_09_16.md`

Canonical deep-dive evidence:
`docs/research/agentswatch_deep_dive_2026_09/`

## Critical evidence rule

A repository change observed between recorded run start and finish is not automatically proven to be authored by the selected agent.

Humans, hooks, formatters, generators or another agent/process may write concurrently.

Core classifications:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Use executor/agent authorship language only when trustworthy executor-specific provenance supports it.

Ambiguity is an explicit evidence state and may force `NeedsReview`.

## Target users

### Dogfood / OSS users
- solo developers using coding agents on real repositories;
- developers working in dirty local worktrees;
- maintainers who want deterministic scope/evidence checks.

### First commercial ICP hypothesis
**AI-heavy engineering teams of roughly 5–30 developers**, especially teams that:
- use coding agents frequently;
- use or evaluate more than one agent/tool ecosystem;
- still perform meaningful human review;
- can adopt a local CLI/GitHub check without enterprise procurement.

### Later only after commercial proof
- DevEx/platform teams;
- security/compliance teams;
- regulated enterprise engineering organizations.

## Problems to solve

- task/acceptance intent is often not machine-checkable;
- agent summaries are assertions, not independent evidence;
- dirty worktrees make interval attribution easy to misstate;
- required validation may be missing despite a `Done` claim;
- scope drift can be discovered late;
- vendor logs/session formats differ;
- teams may need one compact evidence artifact without retaining full chat;
- executor-native evidence may not be sufficient or independent for all workflows.

The product must prove these problems remain material **after** Git + CI + PR review + native vendor/Qodo alternatives are considered.

## Product pillars

### 1. RunContract — verification normalization

RunContract is not another task-management system.

Default direction:

```text
existing GitHub/Jira/Linear issue, roadmap item or prompt
  -> import/normalize
  -> request only missing verification-specific fields
```

Minimum fields:

```text
schemaVersion
contractId
taskId
intent
acceptanceCriteria[]
ownedPaths[]
avoidPaths[]
permissionMode
runMode
validationContract
stopRules[]
expectedEvidence[]
sourceTaskRef?
```

Incomplete implementation contracts produce lint findings instead of invented scope.

### 2. Run baseline and interval evidence

At run start capture enough state to distinguish pre-existing repository dirt from changes observed during the run interval:
- branch;
- HEAD;
- staged state/fingerprint;
- unstaged state/fingerprint;
- untracked set/fingerprint;
- timestamp;
- optional executor metadata.

At finish compare equivalent state and produce explicit interval classifications/ambiguities.

Raw final `git status` is never equivalent to run evidence.

### 3. Compact RunReceipt

Produce one canonical vendor-neutral machine-readable evidence artifact plus human projections.

Canonical receipt should contain durable verification data:

```text
schemaVersion
runId
contractId
taskId
executor metadata if known
start/end repository evidence
runDelta
validation evidence + provenance
structured claims
acceptance findings
scope/risk findings
decision + reasons
missedWork[]
override/evidence digest where supported
```

Do **not** require as canonical verification truth:

```text
learningNote
nextPrompt
routeSuggestion
optimizationAdvice
verbose session narrative
```

Those belong in optional handoff/learning projections downstream.

### 4. Evidence Gate

Compare:

```text
verification contract
vs run-interval evidence
vs validation evidence
vs structured claims
vs acceptance/scope findings
```

Statuses:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

Mandatory evidence missing => cannot be `Done`.

No opaque numeric score or LLM opinion may independently decide completion.

### 5. Scope Drift

Compare high-confidence run-interval changes with `ownedPaths` / `avoidPaths`.

Pre-existing unchanged dirt must not create current-run drift findings.
Material ambiguity must remain visible.

### 6. Claims vs Diff vs Validation

Start with narrow deterministic claims:
- `TestsAdded`;
- `DocsOnly`;
- `BackendUnchanged`;
- `MigrationAdded`;
- `ValidationPassed`;
- `NoUnrelatedChanges`.

Broad semantic claims such as `BugFixed` or full acceptance completion may be advisory/Unknown unless evidence can support them.

LLM extraction may assist later but cannot upgrade evidence status by itself.

### 7. Learning — downstream only

Repository-local learning starts only after:
- receipt/evidence correctness;
- dogfood proof;
- external value.

Learning events, routing and next-prompt suggestions are not canonical RunReceipt truth.

## Canonical data rule

```text
JSON = canonical verification state
Markdown = human-readable projection/handoff
```

Expected paths:

```text
.agentwatch/contracts/<contract-id>.json
.agentwatch/active-runs/<run-id>.json
.agentwatch/runs/<run-id>.json
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

## MVP scope

Required:
1. Gate 0 CI/test/smoke closure.
2. verification-focused `RunContract v1` schema/lint/storage.
3. start baseline.
4. finish run-interval delta with ambiguity.
5. slim `RunReceipt v1` JSON + projections.
6. validation evidence/provenance.
7. deterministic Evidence Gate.
8. Scope Drift v1.
9. initial claims verification.
10. .NET + Flutter + universal Git behavior.
11. adversarial 30-run dogfood.

## Explicitly not MVP

- proprietary agent runtime;
- generic AI code-review engine;
- cloud sandbox/workspaces;
- broad orchestration/scheduling;
- visual workflow canvas;
- full chat/session archive;
- generic token/cost dashboard;
- generic AI-code tracking;
- SaaS/billing/OAuth/team admin;
- automatic merge/release/deploy;
- sophisticated routing/learning before trusted evidence;
- broad integration marketplace.

## External-value gate

Dogfood success is necessary but insufficient.

Pass candidate after dogfood:
- >=10 external real evaluations;
- >=2 agent ecosystems represented where practical;
- >=3 users/teams request continued use;
- >=3 real decision-changing evidence/scope/claim findings beyond baseline workflow;
- no observed material false attribution;
- false-positive burden low enough to preserve reviewer trust;
- clear value beyond Git + CI + PR review + native vendor/Qodo evidence.

If this fails, narrow/pivot/stop rather than adding unrelated breadth.

## Commercial gate

Before team/SaaS infrastructure:
- >=3 paid pilots/design-partner/equivalent concrete commercial commitments;
- payer/budget owner identified;
- payment tied to verification/review/governance value;
- pricing tested with real buyers.

## Packaging hypothesis

Leading hypothesis after value proof: **open core**.

Open/local core may include:
- schemas;
- CLI;
- Git evidence model;
- deterministic rules;
- basic receipt verifier/projection;
- basic GitHub check after demand.

Potential paid later:
- organization policies;
- managed evidence retention/search;
- signed/managed attestations;
- cross-repository controls;
- team analytics;
- RBAC/SSO/compliance/support.

This is a hypothesis until WTP evidence exists.

## Signature metrics

Verification quality:
- interval evidence correctness/ambiguity;
- material false attribution;
- false-positive findings;
- unsupported-claim catches;
- scope-drift catches;
- evidence completeness;
- decision-changing findings;
- reviewer time added/saved.

External/commercial proof:
- continued-use requests;
- second-repo/team adoption;
- paid/design-partner commitments;
- repeated usage across real teams.
