# AgentsWatch Run Evidence Implementation Backlog

Last aligned: 2026-07-05  
Status: issue-ready execution plan

## Goal

Deliver and prove the smallest useful application flow:

```text
start -> make changes -> finish -> report
```

Then extend it with explicit command evidence before building claims, PR packets, integrations, or infrastructure.

## Foundation slice

### RUN-001 — Versioned run manifest model

Status: Implemented on feature branch; execution proof pending  
Capabilities: AW-CAP-012, AW-CAP-013

Implemented:

- schema version 1.0;
- path-safe task ID;
- lifecycle and validation enums;
- full Git object ID validation;
- start/end branch/object identity;
- allowed paths, changes, out-of-scope files, warnings.

Acceptance remaining:

- compile/test execution;
- tampered/invalid manifest black-box case;
- schema migration policy before any schema change.

### RUN-002 — Atomic manifest/report persistence

Status: Implemented on feature branch; execution proof pending  
Capabilities: AW-CAP-012..014, AW-CAP-026

Implemented:

- `.agentwatch/runs/<task>.json` sidecar;
- `.ai/runs/<task>.md` report;
- create-without-overwrite;
- temporary-file atomic create/replace;
- schema/required-field validation;
- active/latest manifest queries;
- no absolute repository root in artifacts.

Acceptance remaining:

- interrupted-write fixture where practical;
- read-only directory behavior;
- path with spaces on Windows/Linux;
- corrupt JSON and unknown schema scenarios.

### RUN-003 — Git change-set reader

Status: Implemented on feature branch; execution proof pending  
Capabilities: AW-CAP-007, AW-CAP-013

Implemented:

- diff from immutable full start object;
- added/modified/deleted/type-changed/unmerged/renamed/copied parsing;
- current untracked paths;
- deterministic path order;
- arbitrary revision text rejected.

Acceptance remaining:

- real temporary Git repo tests;
- rename/delete/binary/quoted path fixtures;
- SHA-256 repository fixture where available;
- branch switch and detached-head behavior.

### RUN-004 — Scope and managed-artifact classification

Status: Implemented on feature branch; execution proof pending  
Capabilities: AW-CAP-012..014

Implemented:

- `*`, `?`, and `**` repository-relative matching;
- slash normalization;
- unrestricted scope behavior;
- managed artifact recognition for `.agentwatch/runs/*.json` and `.ai/runs/*.md`;
- managed artifacts excluded from dirty baseline and change attribution.

Acceptance remaining:

- case-sensitivity decision for Windows versus Git paths;
- character-class/escaping decision if demanded;
- performance/bounds test for many patterns/paths.

### RUN-005 — Start/finish/report CLI lifecycle

Status: Implemented on feature branch; execution proof pending  
Capabilities: AW-CAP-012..014

Implemented:

- duplicate task refusal;
- one active run;
- non-AgentsWatch dirty baseline refusal;
- latest report default;
- explicit task report;
- finished-run rewrite refusal;
- stable labels;
- explicit `Validation: NotRun` and evidence boundary.

Acceptance remaining:

- black-box CLI process tests and exit codes;
- current help smoke;
- non-Git behavior;
- no-network proof;
- package clean-install smoke.

### RUN-006 — Unit tests for foundation

Status: Authored; not executed  
Capabilities: AW-CAP-007, AW-CAP-012..014, partial AW-CAP-026

Authored coverage:

- path-safe IDs;
- scope globs and slash normalization;
- current/managed artifact paths;
- Git name-status parsing;
- tracked/untracked merge;
- invalid revision rejection;
- manifest roundtrip/no-overwrite;
- active/latest lookup;
- schema rejection;
- deterministic, honest, privacy-safe Markdown.

Done only when:

- tests compile;
- Linux and Windows results retained;
- failures fixed without weakening assertions;
- registry/matrix updated with exact run/commit evidence.

### RUN-007 — Black-box acceptance harness

Status: Ready after test compile

Tasks:

- create disposable Git repositories;
- run scenarios in `RUN_EVIDENCE_ACCEPTANCE_SCENARIOS.md`;
- capture stdout/stderr/exit/file tree/checksums;
- run Linux and Windows;
- retain artifacts;
- add start/finish/report smoke to CI.

Acceptance:

- all required scenarios Pass;
- skipped is not Pass;
- no writes outside fixture repository;
- current commit/package identity recorded.

### RUN-008 — Documentation and proof reconciliation

Status: In progress on feature branch

Tasks:

- align command/report/data contracts;
- update capability registry/matrix;
- add evidence run log;
- document blocked validation honestly;
- update PR description after test results.

Acceptance:

- no document claims command evidence/PR Evidence is implemented;
- all new capabilities remain L2 until execution;
- next slice and blockers explicit.

## Next vertical slice: command evidence

### CMD-001 — Command evidence model

Status: Ready after RUN-006/007 prove foundation  
Capabilities: AW-CAP-018, prerequisite for AW-CAP-017/029/035

Implement:

- versioned `CommandEvidence`;
- active run link;
- redacted display/hash;
- timestamps/duration;
- start/end Git object IDs;
- exit code/status;
- stdout/stderr byte counts;
- compact redacted summary;
- first useful error signature;
- timeout/cancellation/refusal state.

Do not store full stdout/stderr by default.

### CMD-002 — Explicit `agentswatch run -- <command>` execution

Dependencies: CMD-001, threat/redaction rules

Implement:

- require `--` separator;
- preserve argument boundaries safely;
- no shell interpolation unless an explicit later mode exists;
- process timeout/cancellation;
- stdout/stderr capture with size bounds;
- stream useful output locally while persisting compact evidence;
- attach evidence to one active run.

Acceptance:

- pass/fail/timeout/cancel fixtures;
- secret-like fixture redacted;
- huge-output fixture bounded;
- exit code 0 not converted to broad completion success;
- Windows/Linux command behavior.

### CMD-003 — Command history storage

Planned path:

```text
.agentwatch/command-history.jsonl
```

Acceptance:

- append-only/idempotency identity;
- corrupt-line handling;
- no absolute path leakage unless explicitly required and minimized;
- active-run filtering;
- deterministic projection into report.

### CMD-004 — Validation evidence projection

Capabilities: AW-CAP-017

Implement after command evidence:

- map configured validation commands to evidence;
- Pass/Fail/NotRun/BlockedByEnvironment;
- bind evidence to Git object;
- detect source changes after validation;
- classify stale evidence.

Acceptance:

- tests passed at object A, source changed to object B => stale/not current;
- absent command evidence remains NotRun;
- stderr warning can prevent overly broad success classification where rules define it.

## Trust Ledger and PR packet

### CLAIM-001 — Structured claim input

Dependencies: command/validation evidence

Start with JSON/YAML/manual structured claims, not unrestricted LLM extraction.

Fields:

```text
claimId
text
type
expectedEvidence
```

### CLAIM-002 — Deterministic claim assessment

Statuses:

```text
SUPPORTED
PARTIALLY_SUPPORTED
CONTRADICTED
MISSING_EVIDENCE
STALE_EVIDENCE
NOT_OBSERVED
NOT_VERIFIABLE
SKIPPED
```

Acceptance:

- every status has fixtures;
- missing observation differs from contradiction;
- stale evidence names the object mismatch;
- confidence derives from evidence quality.

### PR-001 — Local PR Evidence Packet

Dependencies: RUN foundation, command evidence, claim assessment

Command target:

```bash
agentswatch pr evidence --run <task-id> --base <branch-or-object>
```

Outputs:

```text
.agentwatch/evidence/<task-id>-pr-evidence.json
.ai/evidence/<task-id>-pr-evidence.md
```

Do not start GitHub Action/App work until the local packet is useful and users request automation.

## Deferred

Not part of the next implementation sequence:

- dashboard;
- daemon;
- cloud backend;
- billing/auth;
- GitHub App;
- IDE extension;
- live provider hooks;
- generic AI bug reviewer;
- policy enforcement;
- multi-agent orchestration.

## Recommended execution order

```text
RUN-006 compile/unit tests
-> RUN-007 black-box Linux/Windows
-> reconcile proof and merge stacked PRs
-> CMD-001 command model
-> CMD-002 explicit runner
-> CMD-003 history/projection
-> CMD-004 stale validation evidence
-> CLAIM-001/002
-> PR-001 local evidence packet
-> 5 internal PR dogfood runs
-> external pilot and 30-PR market validation
```
