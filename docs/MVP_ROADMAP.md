# AgentsWatch MVP Roadmap

Last aligned: 2026-09-16  
Status: active execution roadmap

## Strategy

AgentsWatch should not compete on coding-agent execution, cloud sandboxes, generic orchestration, session management, scheduling, generic cost dashboards, generic AI-code tracking or generic AI code review.

The narrowed MVP wedge is:

```text
existing task / roadmap intent
  -> verification RunContract
  -> pre-run repository baseline
  -> external coding agent
  -> run-interval repository delta
  -> validation/evidence
  -> portable RunReceipt
  -> deterministic scope/claim/completion verification
  -> auditable run status
```

Commercial thesis:

> **AgentsWatch must prove that a vendor-neutral run-evidence and completion-verification layer changes real review/rework/merge decisions beyond Git + CI + PR review + native vendor/Qodo evidence.**

Critical wording:

`run-interval repository delta` does **not** automatically mean `agent-authored change`.

Humans, hooks, formatters, generators or another agent/process may write during the same interval. When causality cannot be established, record `Ambiguous` / `NeedsReview` rather than guessing.

Canonical strategy guardrails:
`docs/DEEP_DIVE_DECISIONS_2026_09_16.md`

Deep-dive evidence:
`docs/research/agentswatch_deep_dive_2026_09/`

## Gate 0 — close the known skeleton failure

Latest known GitHub CI evidence on `main`:
- restore: pass;
- build: pass;
- tests: fail;
- failing test: `GitStatusParserTests.Parse_ParsesModifiedAndUntrackedFiles`;
- root cause: `TrimEntries` removes the leading porcelain status column before fixed-position parsing, causing `README.md` to become `EADME.md`.

Required work:
1. harden Git status parsing using a lossless machine-safe porcelain contract, preferably NUL-delimited;
2. add tests for staged/unstaged, added, deleted, renamed, untracked, spaces and unusual paths;
3. rerun restore/build/test;
4. run CLI smoke for help/version/init/optimize/status in temporary repositories/directories;
5. record evidence and close Gate 0 only when the full gate passes.

Definition of done:
- solution builds;
- all tests pass;
- CLI smoke passes or environment block is explicitly documented;
- local writes remain under expected `.ai` / `.agentwatch` paths.

## Phase 1 — RunContract v1

Goal: normalize an existing issue/prompt/roadmap item into a deterministic **verification contract**, not create another task-management system.

Required verification-specific fields:

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
sourceTaskRef?
```

Requirements:
- JSON is canonical;
- contract can be linted without an LLM;
- importer/manual flow asks only for missing verification fields;
- incomplete implementation contracts fail with actionable findings;
- Markdown is projection, not source of truth.

Definition of done:
- valid/invalid fixtures;
- deterministic contract lint tests;
- stable storage: `.agentwatch/contracts/<contract-id>.json`.

## Phase 2 — Start-run repository baseline

Goal: know what existed before the delegated run interval began.

`agentswatch start <task-id-or-contract-id>` captures:
- run id;
- contract id;
- timestamp;
- branch;
- HEAD SHA;
- staged state/fingerprint;
- unstaged state/fingerprint;
- untracked-file set;
- clean/dirty state;
- optional executor/tool/model metadata.

Rules:
- pre-existing dirty work is allowed and explicitly recorded;
- a second active run is refused by default;
- baseline is machine-readable;
- full source contents are not persisted by default.

## Phase 3 — Finish-run interval delta

Goal: compute what repository state changed **inside the recorded interval**, instead of reporting raw final `git status`.

Required behavior:
- load start baseline;
- capture finish repository state;
- compute run-interval changed files/statuses;
- distinguish pre-existing unchanged dirt from files changed further during the interval;
- handle adds/deletes/renames/untracked files;
- preserve staged vs unstaged semantics where relevant;
- preserve ambiguity instead of assigning causal authorship without evidence.

Core classifications:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Definition of done:
- pre-existing dirty file is not falsely treated as newly changed;
- changed-further file is surfaced correctly;
- concurrent/uncertain cases can remain Ambiguous;
- finish fails clearly without a matching active run.

## Phase 4 — RunReceipt v1

Goal: create a compact vendor-neutral evidence artifact for one run.

Canonical JSON receipt includes:

```text
schemaVersion
runId
contractId
taskId
executor metadata if known
start/end timestamps
start/end repository evidence
runDelta
validation evidence
structured claims
acceptance findings
scope/risk findings
decision + reasons
missed work
evidence digest/reference if implemented
```

Do **not** require as canonical audit fields:

```text
learningNote
nextPrompt
routing advice
optimization advice
verbose session narrative
```

Those belong in optional handoff/learning projections after verification truth exists.

Outputs:

```text
.agentwatch/runs/<run-id>.json
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

Definition of done:
- JSON is canonical;
- Markdown is generated from structured state;
- receipt is useful without full chat history;
- no validation/completion claim is synthesized without evidence.

## Phase 5 — Validation evidence and Evidence Gate

Goal: prevent `Done` when required evidence is missing.

Initial checks:
- required validation evidence exists;
- validation result/provenance is known;
- required fields are present;
- acceptance criteria can be `Supported`, `Unsupported` or `Unknown`;
- risky/blocked cases remain explicit.

Statuses:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

Rules:
- mandatory validation missing => cannot be Done;
- every non-Done result lists reasons;
- user override requires auditable reason;
- LLM output cannot silently upgrade Unknown/NeedsReview to Done.

## Phase 6 — Scope Drift

Compare `ownedPaths` / `avoidPaths` with high-confidence run-interval changes.

Checks:
- changed paths outside owned scope;
- avoid-path touches;
- unexpected test/config/migration/security changes;
- pre-existing unchanged dirt excluded;
- material ambiguity surfaced separately.

No opaque score may decide status by itself.

## Phase 7 — Claims vs Diff vs Validation

Start with narrow deterministic claim classes:

```text
TestsAdded
DocsOnly
BackendUnchanged
MigrationAdded
ValidationPassed
NoUnrelatedChanges
```

Broad semantic claims such as `BugFixed` may be advisory later but must not be promoted to deterministic fact without evidence.

Provider/LLM claim extraction is optional and cannot replace verification.

## Phase 8 — 30-run dogfood proof

Use AgentsWatch on:
- AgentsWatch itself;
- at least one .NET repository;
- at least one Flutter repository;
- clean and dirty worktrees;
- migrations/config/docs/tests/refactors/features;
- at least two agent ecosystems where practical.

Collect at least 30 useful receipts.

Track:
- contract completeness;
- run-interval attribution correctness/ambiguity;
- unsupported claims;
- scope findings;
- evidence completeness;
- validation breadth/duration;
- false positives;
- false attribution;
- whether finding changes reviewer decision;
- verification overhead vs reviewer time saved.

Success evidence must include:
- real unsupported-claim catch;
- real scope-drift catch;
- missing-evidence block;
- no observed material false attribution in tested dogfood;
- ambiguity surfaced rather than guessed.

## Phase 9 — External value gate

Recruit at least 10 external developers/engineering teams using coding agents on real repositories.

First commercial ICP hypothesis:
**AI-heavy teams of roughly 5–30 developers**, especially teams using multiple agent/tool ecosystems and meaningful human review.

Compare explicitly against:
- Git diff/status;
- CI;
- PR review;
- native vendor logs/hooks/audit;
- Qodo/current independent-review alternatives where relevant.

Pass candidate:
- >=10 real external evaluations;
- >=3 request continued use;
- >=3 decision-changing evidence/scope/claim findings baseline workflows did not surface early enough;
- at least two agent ecosystems represented;
- no observed material false attribution;
- false-positive burden low enough that reviewers keep trusting findings.

Kill/narrow signal:
- native/Qodo evidence solves the problem well enough;
- receipt is read as a report but does not change decisions;
- neutrality has no value in real team workflows;
- verification adds more review cost than it removes.

If this gate fails, do not rescue the thesis with dashboards, orchestration, token analytics or unrelated integrations.

## Phase 10 — Commercial design-partner gate

Before SaaS/auth/billing/team administration:
- >=3 external teams agree to paid pilot/design-partner/equivalent concrete buying process;
- payer/budget owner identified;
- payment tied to verification/review/governance value;
- at least one pricing model tested against actual buyers.

Adjacent current pricing justifies testing team pricing, but AgentsWatch WTP is not proven until money/procurement evidence exists.

## Phase 11 — Learning and validation economy

Only after trustworthy receipts **and external value**:
- repeated mistake patterns;
- scoped do-not-repeat rules;
- validation ladders;
- avoidable validation estimates;
- learning confidence/expiry.

Learning stays outside the canonical RunReceipt truth model.

## Phase 12 — Cross-agent history and empirical routing

Only after comparable external/dogfood evidence:
- normalize vendor metadata;
- group comparable task types;
- compare accepted outcomes/retries/drift/evidence quality;
- route only when evidence supports it;
- otherwise return `unknown`.

## Phase 13 — Thin integrations

Preferred order after stable contracts and external-value proof:
1. GitHub Action/Check if validated users request repository-level verification;
2. MCP tools for contract/start/finish/receipt/evidence;
3. Codex/Claude/Cursor thin adapters for metadata/import;
4. additional session/evidence adapters.

External products execute/review. AgentsWatch verifies run evidence/completion.

## Phase 14 — Team packaging

Blocked until receipt dogfood, external-value and commercial gates pass.

Potential paid/open-core layer later:
- organization policies;
- central evidence retention/search;
- managed/signed attestations;
- cross-repository controls;
- team analytics;
- RBAC/SSO/compliance/support.

Do not build a visual workflow canvas.

## Current execution order

1. `AW-VFY-001` — fix/harden Git parser and make CI green.
2. `AW-VFY-002` — CLI smoke and Gate 0 closure.
3. `AW-VFY-003` — RunContract v1 schema/lint/storage.
4. `AW-VFY-004` — start-run dirty-worktree baseline.
5. `AW-VFY-005` — finish-run interval delta.
6. `AW-VFY-006` — slim RunReceipt v1 JSON + projections.
7. `AW-VFY-007` — validation evidence + Evidence Gate.
8. `AW-VFY-008` — Scope Drift v1.
9. `AW-VFY-009` — Claims-vs-Diff-vs-Validation v1.
10. `AW-VFY-010` — 30-run adversarial dogfood pilot.
11. `AW-VFY-011` — external-value validation.
12. `AW-VFY-012` — commercial design-partner validation.

Canonical queue:
`docs/prompt_queues/verification_mvp_2026_08_25.md`

## Explicitly de-prioritized

- generic agent runtime;
- cloud workspace infrastructure;
- visual orchestration;
- generic scheduler;
- generic token/cost dashboard as primary value;
- generic code review engine;
- full chat archive;
- autonomous merge/release/deploy;
- SaaS/billing/auth before local/external/commercial proof;
- complex routing before reliable comparable receipts;
- broad integration marketplace;
- generic AI-code tracking as a standalone product wedge.
