# AgentsWatch CLI Command Contracts

Last aligned: 2026-08-25  
Status: authoritative command contract after Gate 0

## Purpose

Define stable CLI behavior so implementation agents do not invent incompatible UX, storage, attribution, or verification semantics.

## Global rules

All commands must:

- default to the current repository root;
- avoid network calls by default;
- avoid overwriting user files without an explicit tested flag;
- print concise output;
- return non-zero on real failure/invalid evidence;
- keep source contents and secrets out of reports;
- keep full stdout/stderr out of persisted reports by default;
- preserve `unknown` / `ambiguous` rather than fabricating certainty;
- use canonical JSON models for contracts, active runs, and receipts;
- generate Markdown only as a projection of structured state.

Draft exit codes:

| Code | Meaning |
|---:|---|
| 0 | success / verified result |
| 1 | command or internal failure |
| 2 | invalid arguments or invalid contract |
| 3 | verification/validation failed or insufficient evidence |
| 4 | blocked by environment |
| 5 | unsafe operation refused / approval required |

## `agentswatch init`

Purpose: create the local AgentsWatch workspace.

Creates if missing:

```text
.ai/config.yml
.ai/runs/
.ai/handoffs/
.ai/STATUS.md
.ai/CHANGELOG_AI.md
.ai/REVIEW_CHECKLIST.md
.agentwatch/contracts/
.agentwatch/active-runs/
.agentwatch/runs/
```

Rules:

- idempotent;
- no overwrite of existing user files;
- print created vs existing paths;
- work on Windows/Linux/macOS path semantics;
- `--force` is future work and must be explicit/tested.

## `agentswatch status`

Purpose: concise repository and verification-state summary.

Target fields:

```text
Project root
Detected types
Branch / HEAD
Dirty files
Active run
Latest receipt
Open findings
Suggested validation
Next safe prompt
```

Rules:

- clean and dirty git repos supported;
- non-git directories return a clear result;
- validation is not executed by default.

## `agentswatch optimize <prompt>`

Legacy/secondary helper.

Purpose: deterministic broad-prompt risk/split advice.

Do not expand this command ahead of the verification spine.

## `agentswatch contract create <task-or-file>`

Purpose: create a `RunContract v1` draft.

Writes:

```text
.agentwatch/contracts/<contract-id>.json
```

Required model fields are defined in `docs/DATA_MODEL.md`.

Rules:

- explicit schema version;
- deterministic ID generation policy;
- do not silently invent risky scope;
- may write an incomplete draft with lint findings;
- must not overwrite an existing contract ID unless explicitly requested.

## `agentswatch contract check <contract-file-or-id>`

Purpose: lint contract readiness for execution.

Minimum checks:

- supported schema version;
- required IDs;
- intent;
- acceptance criteria for implementation mode;
- owned/avoid path syntax;
- validation contract;
- stop rules;
- expected evidence.

Output:

```text
Valid: yes/no
Findings:
- <rule id>: <reason>
```

Invalid implementation contracts return non-zero.

## `agentswatch start <contract-id>`

Purpose: capture the pre-run repository baseline.

Writes:

```text
.agentwatch/active-runs/<run-id>.json
```

Must capture:

- run/contract/task IDs;
- timestamp;
- branch;
- HEAD SHA;
- lossless staged state;
- lossless unstaged state;
- untracked set;
- state/diff fingerprints needed for attribution;
- optional tool/model/agent metadata.

Critical rules:

- dirty worktree is allowed but clearly reported;
- pre-existing dirty files are not automatically attributed to the run;
- refuse a second active run by default;
- start must be atomic enough that a partially written baseline cannot be mistaken for a valid run;
- do not store full source contents when hashes/diff fingerprints suffice.

## `agentswatch finish <run-id>`

Purpose: capture end state and compute `RunDelta`.

Required behavior:

1. load matching baseline;
2. capture end branch/HEAD/worktree state;
3. compare start and end evidence;
4. classify files as:
   - attributable;
   - pre-existing unchanged;
   - pre-existing changed further;
   - ambiguous;
5. preserve rename/add/delete/untracked semantics;
6. build/update canonical `RunReceipt v1`;
7. generate Markdown projection only after JSON succeeds.

Writes:

```text
.agentwatch/runs/<run-id>.json
.ai/runs/<run-id>.md
```

Rules:

- raw final `git status` is not the run delta;
- ambiguous attribution is a first-class finding;
- do not delete active-run state until final receipt persistence succeeds;
- no matching baseline => clear non-zero failure.

## `agentswatch receipt show <run-id>`

Purpose: render the canonical receipt.

Must expose at minimum:

- contract/task IDs;
- agent/tool/model if known;
- attributable changes;
- pre-existing/ambiguous changes;
- validation evidence;
- claims;
- acceptance findings;
- scope findings;
- decision and reasons.

## `agentswatch evidence check <run-id>`

Purpose: enforce required evidence.

Initial deterministic rules:

- required validation evidence missing;
- validation failed/blocked/unknown;
- required receipt fields missing;
- expected evidence missing;
- acceptance criterion unsupported/unknown when required for completion;
- approval-required risk unresolved.

Rules:

- mandatory validation missing => cannot return `Done`;
- agent prose saying tests passed is not validation evidence by itself;
- every non-Done decision lists reasons/evidence references;
- overrides, if later supported, must store an auditable reason.

## `agentswatch drift check <run-id>`

Purpose: verify attributable changes against contract path boundaries.

Checks:

- attributable path outside `ownedPaths`;
- attributable path matching `avoidPaths`;
- elevated-risk path patterns when configured.

Rules:

- pre-existing unchanged dirty files do not count as drift;
- ambiguous attribution is reported separately and may require review;
- output exact paths and matched rule/pattern.

## `agentswatch claims check <run-id>`

Purpose: verify common structured agent claims.

Initial supported types:

```text
TestsAdded
DocsOnly
BackendUnchanged
MigrationAdded
ValidationPassed
NoUnrelatedChanges
```

Rules:

- deterministic evidence first;
- unsupported claim => finding with exact reason;
- LLM extraction is optional later and cannot convert weak evidence into fact.

## `agentswatch handoff <run-id>`

Purpose: generate compact continuation context from the canonical receipt.

Writes:

```text
.ai/handoffs/<run-id>.md
```

Target content:

- task and decision;
- attributable files;
- validation summary;
- unresolved/unsupported findings;
- residual risk;
- next minimal prompt.

Target length: roughly 10–20 lines for normal runs.

## Validation execution — later

`agentswatch validate --suggest` and `agentswatch run -- <command>` remain useful follow-up capabilities but must not block RunContract/attribution/receipt/evidence work.

When implemented:

- command execution is always explicit;
- compact structured validation evidence feeds the receipt;
- full stdout/stderr is not persisted by default;
- secret-looking values are redacted;
- targeted validation is preferred before broad validation when justified.

## Compatibility rule

Future MCP/GitHub/vendor adapters must call the same application use cases and produce/consume the same canonical models. They must not define a separate verification truth model.
