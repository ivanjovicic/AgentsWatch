# AgentsWatch CLI Command Contracts

Last aligned: 2026-07-05  
Status: product contract; implementation maturity governed by capability registry

## Purpose

Define command behavior before and during implementation so agents do not invent incompatible UX, file paths, exit codes, or evidence claims.

Use with:

- `RUN_EVIDENCE_FOUNDATION_CONTRACT.md`;
- `CLI_UX_OUTPUT_SPEC.md`;
- `REPORT_FORMATS.md`;
- `DATA_MODEL.md`;
- `FEATURE_CAPABILITY_REGISTRY.md`.

## Global command rules

All commands should:

- work from the current directory and resolve the repository root when Git evidence is required;
- avoid network calls by default;
- avoid overwriting user-owned or prior evidence files without an explicit contract;
- print concise, stable labels;
- return non-zero on real failure;
- keep sensitive contents and full stdout/stderr out of default reports;
- distinguish observed evidence from inference;
- never claim validation passed without command, CI, adapter, or explicit user-entered evidence;
- preserve unknown/not-observed states.

Exit codes:

| Code | Meaning |
|---:|---|
| 0 | success |
| 1 | unexpected command/runtime failure |
| 2 | invalid arguments |
| 3 | evidence/validation/lifecycle condition failed |
| 4 | blocked by environment |
| 5 | unsafe operation refused |

## `agentswatch init`

Purpose: create the local AgentsWatch workspace.

Creates:

```text
.ai/config.yml
.ai/tasks/
.ai/runs/
.ai/generated/
.ai/STATUS.md
.ai/CHANGELOG_AI.md
.ai/REVIEW_CHECKLIST.md
.agentwatch/
```

Rules:

- idempotent;
- preserve existing files;
- future `--force` must be explicit and tested;
- output should eventually list created and preserved paths.

Minimum proof:

- empty temporary directory;
- second run;
- edited generated file remains unchanged;
- Windows/Unix paths;
- no writes outside selected root.

## `agentswatch status`

Purpose: show repository, project type, validation suggestions, and Git summary without running validation.

Target output:

```text
Project root:
Detected types:
Branch:
Commit:
Changed files:
Suggested validation:
Open risks:
Next safe prompt:
```

Rules:

- handle clean and dirty Git repositories;
- handle non-Git directories with a clear message;
- do not run validation automatically;
- do not print file contents.

## `agentswatch optimize <prompt>`

Purpose: classify a rough prompt and return a safer plan.

Output includes:

```text
Risk:
Budget:
Waste causes:
Suggested split:
Scope limiter:
Optimized prompt:
```

Rules:

- support inline text and file input;
- broad/multi-mode prompts become higher risk;
- identify missing scope, stop, and validation fields;
- do not invent repository-specific paths.

## `agentswatch task split <prompt-file>` — planned

Purpose: write focused markdown task prompts.

Default planned output:

```text
.ai/tasks/001-investigate-only.md
.ai/tasks/002-implement-minimal-fix.md
.ai/tasks/003-add-tests.md
.ai/tasks/004-diff-only-review.md
```

Rules:

- no overwrite by default;
- print generated paths;
- include required prompt fields and stop/validation rules.

## `agentswatch start <task-id> [--title <text>] [--scope <glob>]...`

Purpose: create an attributable Git/scope run baseline.

Implemented storage:

```text
.agentwatch/runs/<task-id>.json
.ai/runs/<task-id>.md
```

Captures:

- path-safe task ID;
- optional title;
- UTC timestamp;
- branch;
- full 40/64-character Git object ID;
- optional allowed-scope globs;
- lifecycle `InProgress`;
- validation `NotRun`.

Rules:

- refuse an existing task ID rather than overwrite;
- refuse a second active run;
- ignore existing AgentsWatch-managed run artifacts during dirty-state evaluation;
- refuse other dirty paths so later changes can be attributed to this run;
- write JSON and Markdown atomically;
- do not store the absolute local repository root in shareable artifacts;
- do not claim that an agent was observed.

Current managed artifacts:

```text
.agentwatch/runs/*.json
.ai/runs/*.md
```

Detailed contract: `RUN_EVIDENCE_FOUNDATION_CONTRACT.md`.

## `agentswatch finish <task-id>`

Purpose: close the Git/scope run and write evidence honestly.

Captures:

- finish timestamp;
- end branch and full Git object ID;
- tracked changes since the immutable start object;
- current untracked paths;
- normalized statuses;
- paths outside declared scope;
- branch-change warning;
- uncommitted non-AgentsWatch change warning;
- `Validation: NotRun` in the current slice.

Rules:

- require an existing in-progress run;
- refuse a second finish/rewrite;
- exclude all managed run artifacts from agent-change attribution;
- sort file output deterministically;
- empty allowed scope means unrestricted, so report `not evaluated`, not a false clean result;
- never claim validation passed;
- do not include full diffs or file contents.

Current limitation:

```text
No command, build, test, CI, UI, database, runtime, or agent-claim evidence is captured yet.
```

## `agentswatch report [task-id]`

Purpose: reprint a human-readable run report.

Behavior:

- no task ID: select the most recently started manifest;
- one task ID: select that manifest;
- more than one argument: invalid arguments;
- no runs or missing task: exit 3 with a clear message.

Rules:

- stable sections and deterministic file order;
- preserve `Validation: NotRun` until evidence exists;
- omit absolute local root, file contents, raw diffs, raw logs, and secrets;
- make the evidence boundary explicit.

## `agentswatch run -- <command>` — next planned vertical slice

Purpose: execute a local command explicitly and record compact evidence linked to an active run.

Planned records:

- redacted command display/hash;
- repository-relative or minimized working-directory identity;
- start/end timestamps and duration;
- start/end Git object IDs;
- exit code;
- stdout/stderr byte counts;
- first useful redacted error signature;
- compact output summary;
- timeout/cancellation/refusal status;
- whether AgentsWatch suggested the command.

Planned storage:

```text
.agentwatch/command-history.jsonl
```

Rules:

- execution must be explicit after `--`;
- no upload;
- no full output in Markdown by default;
- redact secret-looking values before persistence/display;
- preserve failure, timeout, cancellation, and unknown states;
- exit code 0 alone must not automatically prove every completion claim.

See `COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`.

## `agentswatch validate` — planned

Purpose: suggest or explicitly run configured validation commands.

Initial sequence:

```text
validate --suggest
run -- <selected command>
validation evidence projection
```

Rules:

- suggestion is default/no execution;
- execution requires explicit choice;
- targeted validation before broad validation where justified;
- record Pass/Fail/NotRun/BlockedByEnvironment honestly;
- no automatic broad validation before command safety is proven.

## `agentswatch handoff` — planned

Purpose: generate compact continuation context from verified run evidence.

Rules:

- target 10-20 lines;
- include changed files, validation state, missed work, next prompt, and residual risk;
- no long chat history or full logs;
- distinguish evidence from user/agent claims.

## `agentswatch review-diff <commit-or-range>` — planned

Purpose: generate a changed-file-only review prompt.

Rules:

- changed files only by default;
- include missed-test and claims-vs-actual checklist;
- include compact evidence references;
- forbid whole-repository review unless a changed-file dependency requires it.

## `agentswatch pr evidence --run <task-id> --base <branch-or-object>` — planned

Purpose: generate the first market-facing PR Evidence Packet after command and claim evidence exist.

Required inputs later:

- declared task/scope;
- Git change inventory;
- command/build/test/CI evidence with commit binding;
- structured claims;
- evidence classifications;
- remaining reviewer actions.

This command must not be implemented as a generic model-only bug reviewer or claim support from missing observations.

## Command implementation order

```text
start / finish / report foundation
-> execute and prove foundation scenarios
-> run -- command evidence
-> validation evidence projection
-> structured claims/Trust Ledger
-> pr evidence packet
-> GitHub Action only after user demand
```
