# AgentsWatch Product Spec

Last aligned: 2026-08-25  
Status: planning/specification with validated skeleton gaps

## Product definition

AgentsWatch is a local-first, vendor-neutral verification and evidence layer for AI coding agents.

It does not replace Codex, Claude Code, Cursor, Copilot, Devin, OpenHands, or similar execution tools. External agents write code. AgentsWatch independently records the execution contract and repository evidence, then verifies whether the result is supported by the diff and validation.

## Core promise

```text
Turn roadmap intent into verified change — across any coding agent.
```

Supporting promise:

```text
Trust the diff, not the agent summary.
```

Efficiency metrics such as tokens, command time, retries, and avoidable validation remain secondary product signals. They must not displace verification as the MVP wedge.

## Category

Preferred category language:

```text
AI coding-agent verification and evidence layer
```

Avoid relying on `agent control plane` as the primary category label. Broad control-plane, orchestration, scheduling, session-management, cost-tracking, and governance capabilities are increasingly native to coding-agent and developer platforms.

## Target users

Primary MVP users:

- solo developers using one or more coding agents on real repositories;
- developers who delegate implementation but still need independent evidence;
- developers working with dirty local worktrees where attribution matters;
- developers who need deterministic scope and validation checks;
- developers working across .NET and Flutter first.

Later:

- small teams reviewing AI-generated changes;
- maintainers requiring evidence before accepting agent-created pull requests;
- organizations needing auditable AI-change receipts and policy gates.

## Problems to solve

Execution itself is no longer the main gap. The verification gaps are:

- tasks and roadmap items are vague and not machine-checkable;
- agent final summaries are assertions, not evidence;
- pre-existing local changes can be incorrectly attributed to an agent run;
- scope drift is often discovered only during review;
- `tests added`, `bug fixed`, or `all tests pass` claims are not consistently checked;
- different vendors expose different run/session formats;
- completion often reflects agent confidence instead of independent evidence;
- learning about repeated mistakes is fragmented by vendor/session.

## Product pillars

### 1. Run Contract

Convert a roadmap item, issue, or prompt into a machine-readable contract containing at minimum:

```text
schemaVersion
contractId
taskId
intent
acceptanceCriteria
ownedPaths
avoidPaths
permissionMode
runMode
validationContract
stopRules
expectedEvidence
```

Optional later fields include budget guidance, dependencies, risk gates, and route recommendation.

Incomplete contracts should fail lint or produce an investigation/planning contract rather than silently invent implementation scope.

### 2. Run attribution

At run start, capture enough repository state to distinguish pre-existing changes from changes introduced during the run.

Minimum baseline:

- branch;
- HEAD commit SHA;
- staged diff fingerprint/state;
- unstaged diff fingerprint/state;
- untracked-file set;
- timestamp.

At finish, compute attributable delta from start to end. Scope and claims checks must use attributable run changes, not raw end-of-run `git status` alone.

### 3. Agent Run Receipt

Produce one vendor-neutral machine-readable receipt plus a Markdown projection containing:

```text
schemaVersion
runId
contractId
agent/tool/model if known
start/end repository state
attributable files changed
validation evidence
agent claims
acceptance-criteria findings
scope findings
risk findings
status
missed work
learning note
next prompt
```

The receipt must remain useful without full chat history.

### 4. Evidence Gate

Compare:

```text
contract intent
vs acceptance criteria
vs agent claims
vs attributable diff
vs validation evidence
```

Return deterministic, explainable findings and one auditable status:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

No opaque score may independently decide completion.

### 5. Scope Drift

Compare `ownedPaths` / `avoidPaths` against attributable changes.

Examples:

```text
Owned: src/Profile/**
Changed: src/Auth/SessionManager.cs
=> scope drift finding
```

Pre-existing dirty files must not produce scope-drift findings for the current run unless the run actually changed them further and the delta is attributable.

### 6. Claims-vs-Diff-vs-Validation

Start with deterministic claims that can be checked reliably, for example:

- `tests added`;
- `docs only`;
- `backend unchanged`;
- `migration added`;
- `validation passed`;
- `no unrelated files changed`.

LLM interpretation may later expand claim extraction, but core verification must work without a provider key.

### 7. Repository-local learning

After the receipt/evidence loop is proven, record scoped, reviewable learning events such as:

- repeated scope drift patterns;
- repeated missing-test patterns;
- validation sequences that were broader than necessary;
- task types that repeatedly require retries.

Learning is downstream of trustworthy receipts. Do not build a sophisticated router on untrusted run data.

## Canonical data rule

From the MVP onward:

```text
JSON = canonical machine-readable state
Markdown = human-readable projection
```

Expected paths:

```text
.agentwatch/contracts/<contract-id>.json
.agentwatch/runs/<run-id>.json
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

Verification logic must not depend on parsing free-form Markdown reports.

## MVP scope

Required:

1. Gate 0 CI/test/smoke closure.
2. `RunContract v1` schema and lint.
3. `start` baseline and active-run state.
4. `finish` attributable delta.
5. `RunReceipt v1` JSON + Markdown projection.
6. Validation evidence model/capture.
7. Deterministic evidence gate.
8. Scope drift findings.
9. Initial claims-vs-diff rules.
10. Handoff + one learning note.
11. Universal git behavior plus .NET and Flutter adapter support.

## Explicitly not MVP

Do not prioritize:

- another coding-agent runtime;
- generic background-agent queue;
- cloud sandbox/workspaces;
- visual workflow canvas;
- generic scheduling;
- full conversation/session archive;
- generic token/cost dashboard as the main product;
- SaaS, billing, OAuth, team administration;
- automatic merge/release/deploy;
- complex empirical routing before comparable receipt data exists;
- broad integration marketplace.

## Post-MVP sequence

After the verification spine is reliable:

1. dogfood at least 30 useful receipts;
2. improve validation economy from real evidence;
3. add mistake-pattern learning with confidence/expiry;
4. add cross-agent normalized imports;
5. add empirical route suggestions only when comparable evidence is sufficient;
6. expose stable contracts through MCP;
7. add GitHub/PR checks;
8. build a local dashboard only when receipt data proves which views matter;
9. consider team/commercial packaging.

## Signature metrics

Primary verification metrics:

```text
Contract Completeness
Attribution Confidence
Acceptance-Criteria Coverage
Evidence Completeness
Scope Drift Findings
Unsupported Claim Count
Run Status Confidence
```

Secondary efficiency metrics:

```text
Validation Efficiency
Retry Count
Avoidable Work Estimate
Repeat Mistake Rate
Provider cost/token data when available
```

Rules:

- no fake precision;
- every finding and score must expose its evidence/reason;
- estimates must be labeled as estimates;
- unknown must remain a valid result;
- no completion status may be upgraded solely because a numeric score is high.

## Privacy and safety

Default behavior:

- local-first;
- no telemetry;
- no source/prompt/diff/receipt upload;
- no hidden network calls;
- secret redaction before persistence;
- compact evidence rather than full logs;
- external integrations explicit and opt-in;
- risky command execution requires explicit user action/approval.

## Success criteria for product validation

Before dashboard/SaaS work, AgentsWatch should prove on real repositories that it can:

- correctly distinguish pre-existing changes from run-attributable changes;
- catch at least one unsupported agent claim;
- catch at least one real scope-drift case;
- prevent `Done` when required validation evidence is missing;
- produce receipts useful enough to inspect/reuse without full chat history;
- support repeated dogfood use without excessive setup or context overhead.

See:

- `docs/MVP_ROADMAP.md`
- `docs/ARCHITECTURE.md`
- `docs/DATA_MODEL.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/prompt_queues/verification_mvp_2026_08_25.md`
