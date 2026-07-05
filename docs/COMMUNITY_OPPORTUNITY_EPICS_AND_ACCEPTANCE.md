# AgentsWatch Community Opportunity Epics and Acceptance

Last aligned: 2026-07-03  
Status: product contracts at L1 only; no runtime support implied

## Purpose

Turn the highest-priority community problems into bounded, testable development epics.

## Global rules

Every epic must:

- remain local-first by default;
- avoid requiring an LLM credential for deterministic analysis;
- use provider adapters rather than provider-specific core logic;
- expose dry-run/explain behavior before enforcement;
- preserve raw source, prompts, and full logs locally unless explicitly exported;
- record unsupported/missing provider data honestly;
- create commit-bound evidence before maturity increases;
- include failure, privacy, and negative scenarios.

---

# Epic AW-OPP-01 — Agent Flight Recorder and Trust Ledger

## Problem

Agent self-report is not sufficient evidence. Developers need a replayable account of what happened and whether completion claims are supported.

## Jobs to be done

- Show what the agent read, changed, executed, and verified.
- Compare final claims with observable evidence.
- Identify gaps without exposing full source or logs by default.
- Preserve evidence across providers and clients.

## Proposed commands

```bash
agentswatch record start <task-id> [--adapter <name>]
agentswatch record import <path> [--adapter <name>]
agentswatch record finish <task-id>
agentswatch timeline <run-id>
agentswatch evidence verify <run-id>
agentswatch replay <run-id> [--summary|--events]
```

## Core data

```text
AgentRun
  RunId
  TaskId
  Provider
  Client
  Model
  ClientVersion
  StartedAtUtc
  FinishedAtUtc
  WorkingDirectory
  GitStart
  GitEnd
  Events[]
  Claims[]
  EvidenceLinks[]
  Approvals[]
  TerminalState

AgentEvent
  EventId
  TimestampUtc
  EventType
  ParentEventId
  AgentId
  WorkspaceId
  ToolName
  Operation
  TargetMetadata
  ExitCode
  Duration
  BeforeHash
  AfterHash
  RedactionState
  SourceAdapter
  Confidence

CompletionClaim
  ClaimType
  ClaimText
  RequiredEvidence
  LinkedEvidence
  Result: Supported | Contradicted | Missing | NotVerifiable
```

## MVP slices

### AW-FR-001 — Event schema and JSON import

- define normalized event schema;
- import synthetic fixture logs;
- retain unknown vendor fields in an extension bag;
- reject invalid timestamps and path escapes safely.

### AW-FR-002 — Git and process evidence correlation

- correlate changed files with process/test events;
- represent not-run and missing evidence explicitly;
- avoid storing full command output by default.

### AW-FR-003 — Claim verifier

Initial deterministic claims:

- tests passed;
- build passed;
- file added/changed;
- command succeeded;
- no unrelated files changed;
- package/install succeeded.

### AW-FR-004 — Human timeline report

- stable markdown timeline;
- compact JSON sidecar;
- links to raw local artifacts;
- redaction summary.

## Acceptance criteria

- Given a synthetic run with a failed test and a final `tests passed` claim, verification returns `Contradicted`.
- Given no test event, `tests passed` returns `Missing`, not `Supported`.
- Event ordering is stable after import.
- Unknown provider events do not corrupt the run.
- Full source and command output are absent by default.
- Secret-like fixture values are redacted or omitted.
- Manifest hashes verify after write/read round trip.
- A changed-file claim is checked against git evidence, not agent text.

## Proof gate

L4 requires:

- at least two provider fixture adapters;
- deterministic claim regression suite;
- corrupted/missing/out-of-order event tests;
- CLI black-box import/verify/timeline scenarios;
- privacy review.

## Non-goals

- recording private screen/video by default;
- storing hidden chain of thought;
- claiming cryptographic non-repudiation before key/signature design exists;
- automatic cloud upload.

---

# Epic AW-OPP-02 — Context and Memory Portability

## Problem

Task knowledge is trapped in sessions and vendor-specific files. Context loss and provider switching cause repeated reading, corrective prompts, and inconsistent rules.

## Jobs to be done

- Resume a task in a fresh session without reconstructing history manually.
- Switch from one supported agent to another.
- Keep repository rules synchronized across target formats.
- Detect decisions lost or changed during compaction.

## Proposed commands

```bash
agentswatch context snapshot <task-id>
agentswatch context show <snapshot-id>
agentswatch context diff <a> <b>
agentswatch context compact <snapshot-id>
agentswatch context export <snapshot-id> --target <agent>
agentswatch resume <task-id> --target <agent>
agentswatch rules compile [--target all]
agentswatch rules diff
agentswatch rules lint
```

## Canonical context pack

```text
ContextSnapshot
  SnapshotId
  TaskId
  BaseCommit
  CreatedAtUtc
  Goals
  NonGoals
  Decisions[]
  Constraints[]
  OpenQuestions[]
  RelevantPaths[]
  AvoidPaths[]
  ValidationState
  ChangedFiles
  Discoveries
  ResidualRisk
  NextAction
  SourceRunIds[]
  ContentHashes
```

## Rules compiler targets

Initial target set:

- `AGENTS.md`;
- `CLAUDE.md`;
- Cursor project rules;
- Codex repository instructions;
- generic handoff markdown.

Target adapters must declare which concepts they cannot represent.

## MVP slices

### AW-CTX-001 — Manual snapshot

- create from task/run evidence;
- stable markdown and JSON;
- fixed maximum summary budget;
- no invented decisions.

### AW-CTX-002 — Fresh-session resume pack

- create one compact, copy-ready pack;
- include commit and validation state;
- include unknown/missing fields visibly.

### AW-CTX-003 — Rules compiler

- canonical YAML/JSON/markdown source;
- deterministic generated files;
- no-overwrite unless ownership marker exists;
- drift report for edited generated targets.

### AW-CTX-004 — Context diff and loss check

- decision added/removed/changed;
- path scope changed;
- validation status changed;
- unresolved risk lost;
- target export dropped unsupported rule.

## Acceptance criteria

- Exporting twice from the same snapshot is deterministic.
- Existing user-owned rule files are not overwritten.
- Unsupported target semantics produce warnings.
- Snapshot references a real commit or records `unknown`.
- A lost stop rule is detected by context diff.
- A fresh-session evaluator can identify task goal, scope, next action, and validation state from the resume pack.
- Sensitive values are not copied into generated context.

## Proof gate

L5 requires paired resume tests:

- baseline fresh session without a pack;
- fresh session with AgentsWatch pack;
- same task and repository commit;
- fewer corrective prompts or faster validated resume without lower quality.

## Non-goals

- unlimited autonomous long-term memory;
- copying entire conversations into every prompt;
- guaranteeing identical behavior across models;
- replacing repository documentation.

---

# Epic AW-OPP-03 — Cost and Loop Guard

## Problem

Agent runs can consume limits or money through repeated no-progress actions, hidden background work, and context/tool overhead.

## Jobs to be done

- See where usage is going.
- Detect repeated actions before the budget is exhausted.
- Stop safely and preserve a resumable checkpoint.
- Compare cost per validated outcome across tools.

## Proposed commands

```bash
agentswatch budget set --task <id> [--tokens N] [--cost amount] [--time duration]
agentswatch watch -- <agent-command>
agentswatch loops analyze <run-id>
agentswatch usage report <run-id>
agentswatch checkpoint <run-id>
agentswatch stop-policy evaluate <run-id>
```

## Detection signals

- same normalized command repeated;
- same search/read target repeated;
- same validation failure without relevant file change;
- alternating file hashes;
- repeated edit/revert cycle;
- high token/cost increase with no new evidence;
- subagent fan-out above limit;
- context/tool schema overhead above policy;
- approaching limit without checkpoint.

## Loop states

```text
Healthy
Watch
SuspectedLoop
BudgetRisk
CheckpointRequired
StopRecommended
Stopped
FalsePositiveDismissed
```

## MVP slices

### AW-LOOP-001 — Offline action fingerprinting

- analyze imported/synthetic events;
- report repetitions and no-progress windows;
- no live process control.

### AW-LOOP-002 — Budget ledger

- normalize known token/cost/limit events;
- retain provider raw units;
- allow unknown price/rate values;
- never fabricate monetary cost.

### AW-LOOP-003 — Stop recommendation

- deterministic thresholds;
- explain exact repeated actions;
- produce checkpoint prompt;
- dry-run only.

### AW-LOOP-004 — Live wrapper

- optional process wrapper;
- warning and checkpoint before termination;
- user-configurable kill behavior;
- safe signal/cleanup handling.

## Acceptance criteria

- Same failed command repeated without relevant change is detected.
- Same command after a relevant code change is not automatically treated as identical no-progress work.
- Unknown token prices remain unknown.
- Budget alerts name the source and confidence.
- A stopped run leaves a readable checkpoint.
- False-positive dismissal becomes regression feedback without weakening unrelated rules.
- Live termination is opt-in and disabled by default in MVP.

## Proof gate

- synthetic loop corpus;
- real dogfood runs with known repetitive behavior;
- false-positive and false-negative report;
- measurable repeated actions prevented;
- quality guardrail proving useful work was not stopped prematurely.

## Non-goals

- bypassing provider limits;
- predicting exact provider billing when data is unavailable;
- terminating processes without explicit configuration;
- claiming universal token accuracy.

---

# Epic AW-OPP-04 — Policy Firewall and Safe Execution Broker

## Problem

Agents receive broad file, shell, and network access. Existing permission prompts may not capture repository-specific scope, untrusted instructions, or sensitive paths.

## Jobs to be done

- Explain whether an operation is allowed before it runs.
- Prevent or require approval for out-of-scope actions.
- Protect sensitive paths and destinations.
- Preserve enough evidence for incident review.

## Proposed commands

```bash
agentswatch policy init
agentswatch policy lint
agentswatch policy explain --operation <op> --target <target>
agentswatch policy check --operation <op> --target <target>
agentswatch exec --policy <path> -- <command>
agentswatch approvals list
```

## Policy dimensions

```text
Operation: Read | Write | Delete | Execute | Network | Git | PackageInstall
PathScope: allow | deny | approval-required
CommandRisk: low | medium | high | prohibited
NetworkScope: offline | allow-list | approval-required
InstructionTrust: user | repository | tool-output | remote-content | unknown
CheckpointRule: none | recommended | required
ApprovalRule: none | once | per-command | per-target | session
```

## MVP slices

### AW-POL-001 — Policy schema and linter

- deterministic parsing;
- conflict detection;
- explain precedence;
- default-deny only where explicitly configured.

### AW-POL-002 — Path and command dry-run

- read/write/delete path checks;
- sensitive path presets;
- command tokenization and risk rules;
- no enforcement.

### AW-POL-003 — Approval bundle

- command/target/risk/reason;
- expected side effects;
- rollback/checkpoint status;
- source instruction provenance.

### AW-POL-004 — Optional execution broker

- execute only after check/approval;
- capture exit and side-effect evidence;
- deny path escapes;
- never execute shell strings through unsafe concatenation.

## Acceptance criteria

- `.env`, private keys, SSH, and cloud credential fixtures can be denied by policy.
- A repository README instruction cannot silently override a user policy.
- Path normalization blocks traversal and symlink escape scenarios.
- A destructive command requires the configured approval/checkpoint.
- Dry-run explains the matched rule and precedence.
- Approval denial is recorded without exposing secret content.
- Policy failure is fail-safe for enforcement mode.

## Proof gate

- dedicated threat model;
- path/symlink/platform test matrix;
- command-injection regression suite;
- malicious repository fixture;
- network disabled/allow-list scenarios;
- independent security review before strong security claims.

## Non-goals

- a complete endpoint-security product;
- guaranteed prevention of all prompt injection;
- replacing OS sandboxing;
- hidden command interception.

---

# Epic AW-OPP-05 — Multi-Agent Worktree Coordinator

## Problem

Parallel coding agents need isolated workspaces, explicit ownership, structured communication, and safe integration.

## Jobs to be done

- Ensure each worker operates in the correct worktree.
- Prevent overlapping ownership unless intentional.
- Share compact findings without merging full conversations.
- Know which worker is blocked, complete, or unsafe to integrate.

## Proposed commands

```bash
agentswatch swarm plan <task-file>
agentswatch worker create <worker-id> --task <task-id>
agentswatch worker start <worker-id> --adapter <agent>
agentswatch worker status [<worker-id>]
agentswatch worker message <from> <to> --type <type>
agentswatch worker finish <worker-id>
agentswatch integrate check [--worker <id>]
agentswatch swarm cleanup --dry-run
```

## Core data

```text
SwarmRun
  SwarmId
  RootTaskId
  BaseCommit
  Workers[]
  Dependencies[]
  SharedFindings[]
  IntegrationState

WorkerAssignment
  WorkerId
  TaskId
  AgentAdapter
  WorktreePath
  Branch
  OwnedPaths[]
  AvoidPaths[]
  Dependencies[]
  Status
  StartSnapshot
  EndSnapshot
  EvidenceRunId

WorkerMessage
  Type: Finding | Blocker | DependencyReady | Conflict | Handoff
  From
  To
  Timestamp
  Summary
  EvidenceLinks[]
```

## MVP slices

### AW-SWARM-001 — Worktree assignment planner

- validate git root/base commit;
- create planned paths/branches;
- detect overlapping ownership;
- dry-run only.

### AW-SWARM-002 — Worker bootstrap pack

- exact `cwd` and ownership;
- task scope and stop rules;
- required evidence;
- no direct process spawning initially.

### AW-SWARM-003 — Shared inbox/status

- local structured messages;
- parent/worker lineage;
- bounded summaries;
- no full transcript sharing.

### AW-SWARM-004 — Integration check

- wrong-worktree modifications;
- overlapping changed files;
- missing worker validation;
- dependency not complete;
- likely merge conflicts.

## Acceptance criteria

- Worker package contains one canonical absolute worktree path.
- Changed files outside ownership are flagged.
- Two workers with overlapping write ownership are blocked or require explicit override.
- Parent can see status without loading full worker history.
- Integration check never merges automatically.
- Cleanup is dry-run by default and never deletes uncommitted work.
- Worker evidence links to the exact base/end commits.

## Proof gate

- two and five worker synthetic scenarios;
- wrong-worktree regression test;
- concurrent status update tests;
- merge conflict forecast fixtures;
- dogfood on independent tasks before write-heavy parallel use.

## Non-goals

- replacing the underlying coding agents;
- autonomous merge/release/deploy;
- unrestricted inter-agent chat;
- cloud coordination in the first version.

---

# Epic AW-OPP-06 — AI PR Review Debt Reducer

## Problem

AI increases change volume, while reviewer time is spent proving whether the change matches the requested behavior and validation claims.

## Jobs to be done

- Review the highest-risk behavior first.
- Detect evidence and scope mismatches.
- Reduce noise without hiding changes.
- Help maintainers reject duplicate or unwanted work quickly.

## Proposed commands

```bash
agentswatch pr analyze <commit-or-range>
agentswatch pr evidence <commit-or-range>
agentswatch pr review-pack <commit-or-range>
agentswatch pr policy-check <commit-or-range>
```

## Review packet

```text
Requested scope
Changed behavior inventory
High-risk files/paths
Unexpected files
Tests added/changed/run
Claims-vs-evidence
Validation failures/not-run
Configuration/dependency changes
Security/privacy-sensitive changes
Duplicate/issue-alignment signals
Suggested review order
Questions for author/agent
Residual risk
```

## MVP slices

### AW-PR-001 — Scope and evidence packet

- local git range only;
- deterministic change inventory;
- no generic LLM review.

### AW-PR-002 — Behavioral risk heuristics

- runtime without tests;
- validation/config/security changes;
- broad file count;
- public API/schema changes;
- generated-looking structure with missing implementation evidence only when deterministically detectable.

### AW-PR-003 — Agent run linkage

- link PR range to AgentsWatch runs;
- compare declared scope and changed files;
- verify claimed tests/validation.

### AW-PR-004 — GitHub adapter

- optional PR metadata and checks;
- draft report/comment only;
- explicit user approval before posting.

## Acceptance criteria

- Review pack lists every changed file but prioritizes high-risk paths.
- Missing test evidence is not reported as test failure.
- A claimed passing check is linked to the exact commit/check result.
- Unexpected scope is explainable and reproducible.
- No source or diff is uploaded by the local command.
- GitHub posting is opt-in and drafts before send.
- Packet remains useful without an LLM call.

## Proof gate

- accepted/rejected agent PR fixture set;
- reviewer time and missed-finding study;
- false-positive analysis;
- comparison against plain diff/stat and existing review workflow;
- maintainer dogfood before public claims.

## Non-goals

- replacing human review;
- claiming authorship detection from style alone;
- automatic PR rejection or merge;
- duplicating generic static-analysis tools.

---

# Cross-epic foundation

The six epics share these foundations:

1. normalized agent event model;
2. workspace and path identity;
3. git snapshot/evidence model;
4. redaction and sensitive-path rules;
5. provider adapter capability declarations;
6. deterministic policy/explanation engine;
7. stable markdown/JSON artifacts;
8. capability registry and proof-bundle integration.

## Recommended implementation sequence

```text
Event schema/import
-> Flight Recorder timeline
-> Context snapshot/rules compiler
-> Offline loop analysis
-> Policy dry-run
-> Worktree planning/status
-> PR evidence packet
-> live/enforcement integrations only after proof
```

## Shared kill rule

Do not implement a live or enforcing slice when the import-only/dry-run slice cannot demonstrate useful, low-noise findings on real repositories.
