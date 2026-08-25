# AgentsWatch MVP Roadmap

Last aligned: 2026-08-25  
Status: active execution roadmap

## Strategy

AgentsWatch should not compete on coding-agent execution, cloud sandboxes, generic orchestration, session management, scheduling, or generic cost dashboards.

The MVP wedge is:

```text
Task / roadmap intent
  -> machine-readable Run Contract
  -> external coding agent
  -> attributable repository delta
  -> Agent Run Receipt
  -> claims/diff/validation verification
  -> auditable run status
```

## Gate 0 — close the known skeleton failure

Latest known GitHub CI evidence on `main`:

- restore: pass;
- build: pass;
- test: fail;
- failing test: `GitStatusParserTests.Parse_ParsesModifiedAndUntrackedFiles`;
- root cause: `TrimEntries` removes the leading porcelain status column before fixed-position parsing, causing `README.md` to become `EADME.md`.

Required work:

1. harden git status parsing using a robust porcelain contract, preferably `git status --porcelain=v1 -z` or equivalent lossless parsing;
2. add edge-case tests for staged/unstaged, added, deleted, renamed, untracked, spaces and unusual paths;
3. rerun restore/build/test;
4. run CLI smoke for `help`, `version`, `init`, `optimize`, and `status` in temporary repositories/directories;
5. record evidence and close Gate 0 only when the full gate passes.

Definition of done:

- solution builds;
- all tests pass;
- CLI smoke passes or any environment block is explicitly documented;
- local writes remain under expected `.ai` / `.agentwatch` paths.

## Phase 1 — RunContract v1

Goal: create a deterministic execution contract before implementing the run lifecycle.

Required fields:

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

Requirements:

- JSON is canonical;
- schema/version is explicit;
- contract can be linted without an LLM;
- incomplete implementation contracts fail with actionable findings;
- Markdown may be generated for humans but is not the source of truth.

Definition of done:

- valid/invalid contract fixtures exist;
- contract lint has deterministic tests;
- storage path is stable: `.agentwatch/contracts/<contract-id>.json`.

## Phase 2 — Start-run attribution baseline

Goal: know what existed before the agent touched the repository.

`agentswatch start <task-id-or-contract-id>` must capture:

- run id;
- contract id;
- timestamp;
- branch;
- HEAD SHA;
- staged state/fingerprint;
- unstaged state/fingerprint;
- untracked-file set;
- clean/dirty state;
- optional agent/tool/model metadata.

Rules:

- pre-existing dirty work is allowed but must be explicitly recorded;
- a second active run is refused by default;
- the baseline is machine-readable;
- no source contents are persisted unless specifically required by a future opt-in feature.

Definition of done:

- start state is reproducible enough to compare with finish state;
- tests cover clean and dirty repositories;
- existing user changes are not silently treated as agent changes.

## Phase 3 — Finish-run attributable delta

Goal: compute what changed during this run rather than reporting raw end-of-run status.

Required behavior:

- load the recorded start baseline;
- capture end repository state;
- compute attributable changed files and statuses;
- distinguish pre-existing unchanged dirty files from files changed further during the run;
- handle adds/deletes/renames/untracked files;
- preserve ambiguity as an explicit finding instead of guessing.

Definition of done:

- tests prove a pre-existing dirty file is not falsely attributed;
- tests prove a pre-existing dirty file changed further during the run is surfaced appropriately;
- finish fails clearly if no matching active run exists.

## Phase 4 — RunReceipt v1

Goal: create a vendor-neutral record of one run.

Canonical JSON receipt:

```text
schemaVersion
runId
contractId
taskId
agent/tool/model if known
start/end timestamps
start/end repository metadata
attributable changed files
validation evidence
agent claims
acceptance findings
scope findings
risk findings
status
missed work
learning note
next prompt
```

Outputs:

```text
.agentwatch/runs/<run-id>.json
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

Definition of done:

- JSON is canonical and Markdown is generated from structured data;
- receipt is useful without full chat history;
- no validation claim is synthesized without evidence.

## Phase 5 — Validation evidence and Evidence Gate

Goal: prevent `Done` when required evidence is missing.

Initial deterministic checks:

- required validation evidence exists;
- validation exit/status is known;
- required evidence fields are present;
- acceptance criteria can be marked `supported`, `unsupported`, or `unknown`;
- risky/blocked cases are surfaced explicitly.

Statuses:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

Definition of done:

- `Done` is impossible when mandatory validation is missing;
- every non-Done result lists reasons;
- user override requires an auditable reason.

## Phase 6 — Scope Drift

Goal: compare attributable run changes with contract scope.

Checks:

- changed files outside `ownedPaths`;
- changed files matching `avoidPaths`;
- unexpected test/config/migration/security changes;
- pre-existing dirty files excluded from drift unless attributable delta exists.

Definition of done:

- findings identify exact paths and rule/reason;
- no opaque score decides status by itself;
- common glob/path edge cases are tested cross-platform.

## Phase 7 — Claims vs Diff vs Validation

Goal: independently verify common agent statements.

Start with deterministic claim classes:

```text
tests added
docs only
backend unchanged
migration added
validation passed
no unrelated files changed
```

Definition of done:

- unsupported claims create `NeedsEvidence` or `NeedsReview` findings as appropriate;
- claim extraction can initially be explicit/manual/structured;
- provider/LLM claim extraction is optional later and cannot replace deterministic verification.

## Phase 8 — Dogfood proof

Goal: prove usefulness before expanding the product.

Use AgentsWatch on:

- AgentsWatch itself;
- at least one .NET repository;
- at least one Flutter repository.

Collect at least 30 useful receipts across comparable task types.

Track:

- contract completeness;
- attribution correctness/ambiguity;
- unsupported claims;
- scope findings;
- evidence completeness;
- validation breadth/duration;
- retries;
- accepted/rejected run results;
- whether handoffs reduce repeated context.

Success evidence must include at least:

- one real unsupported-claim catch;
- one real scope-drift catch;
- one missing-evidence block;
- no observed false attribution in tested dogfood cases.

## Phase 9 — Learning and validation economy

Only after receipts are trustworthy:

- mistake pattern recurrence;
- scoped do-not-repeat rules;
- targeted validation ladders;
- repeated/broad command detection;
- avoidable validation estimates;
- learning confidence and expiry/deprecation.

## Phase 10 — Cross-agent history and empirical routing

Only after enough comparable data:

- normalize vendor metadata;
- group comparable task types;
- compare accepted outcomes, retries, drift and evidence quality;
- import provider token/cost data when available;
- recommend a route only when evidence is sufficient;
- otherwise return `unknown`.

## Phase 11 — Thin integrations

Preferred order after stable internal contracts:

1. MCP tools for contract/start/finish/receipt/evidence;
2. GitHub/PR evidence check;
3. Codex/Claude/Cursor thin adapters;
4. additional session import adapters.

External products execute. AgentsWatch verifies.

## Phase 12 — Dashboard/team packaging

Blocked until receipt dogfood proves recurring value.

Potential local dashboard views:

- run receipts;
- unsupported claims;
- scope drift;
- acceptance evidence;
- validation evidence;
- repeated mistake patterns;
- later agent/model comparisons.

Do not build a visual workflow canvas.

## Current execution order

1. `AW-VFY-001` — fix/harden git parser and make CI green.
2. `AW-VFY-002` — CLI smoke and Gate 0 closure.
3. `AW-VFY-003` — RunContract v1 schema/lint/storage.
4. `AW-VFY-004` — start-run dirty-worktree baseline.
5. `AW-VFY-005` — finish-run attributable delta.
6. `AW-VFY-006` — RunReceipt v1 JSON + Markdown projection.
7. `AW-VFY-007` — validation evidence + Evidence Gate.
8. `AW-VFY-008` — Scope Drift v1.
9. `AW-VFY-009` — Claims-vs-Diff-vs-Validation v1.
10. `AW-VFY-010` — 30-run dogfood pilot and evidence review.

Canonical queue:

`docs/prompt_queues/verification_mvp_2026_08_25.md`

## Explicitly de-prioritized

- generic agent runtime;
- cloud workspace infrastructure;
- visual orchestration;
- generic scheduler;
- generic token/cost dashboard as primary value;
- full chat archive;
- autonomous merge/release/deploy;
- SaaS/billing/auth before local proof;
- complex routing before reliable comparable receipts;
- broad integration marketplace.
