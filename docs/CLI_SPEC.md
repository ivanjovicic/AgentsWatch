# AgentsWatch CLI Spec

Last aligned: 2026-08-25  
Status: active product contract

## Product role

The CLI is the first interface to the AgentsWatch verification layer. It must remain thin and call reusable application use cases so the same behavior can later be exposed through MCP or a local API.

## Current implemented commands

```bash
agentswatch init
agentswatch optimize <prompt-file-or-text>
agentswatch status
```

`optimize` remains a secondary helper. New implementation work should prioritize the verification spine.

## Verification MVP commands

```bash
agentswatch init
agentswatch status

agentswatch contract create <task-or-file>
agentswatch contract check <contract-file-or-id>

agentswatch start <contract-id>
agentswatch finish <run-id>

agentswatch receipt show <run-id>
agentswatch evidence check <run-id>
agentswatch drift check <run-id>
agentswatch claims check <run-id>
agentswatch handoff <run-id>
```

Later, after stable receipt data:

```bash
agentswatch validate --suggest
agentswatch run -- <command>
agentswatch mistakes list
agentswatch route suggest
```

## `agentswatch init`

Creates local workspace without overwriting user-owned files:

```text
.ai/
  runs/
  handoffs/
  STATUS.md
  CHANGELOG_AI.md
  REVIEW_CHECKLIST.md
.agentwatch/
  contracts/
  active-runs/
  runs/
```

Existing `.ai/config.yml` may remain for local settings, but persisted verification state must use structured JSON contracts/receipts.

## `agentswatch contract create`

Purpose: convert a task/roadmap item/prompt into a `RunContract v1` draft.

MVP may require deterministic flags/interactive fields instead of LLM generation where necessary.

Writes:

```text
.agentwatch/contracts/<contract-id>.json
```

Output summarizes:

```text
Contract:
Intent:
Acceptance criteria:
Owned paths:
Avoid paths:
Validation:
Stop rules:
Lint result:
```

The command must not silently invent risky owned paths or validation requirements when insufficient context exists. It may produce an incomplete draft that `contract check` rejects.

## `agentswatch contract check`

Purpose: deterministic lint of a RunContract.

Checks at minimum:

- supported schema version;
- required identifiers;
- non-empty intent;
- acceptance criteria for implementation mode;
- valid path patterns;
- validation contract presence when required;
- stop rules;
- expected evidence.

Exit code must be non-zero for an invalid implementation contract.

## `agentswatch start <contract-id>`

Purpose: capture the repository baseline before external agent execution.

Writes:

```text
.agentwatch/active-runs/<run-id>.json
```

Captures:

- run/contract/task IDs;
- start timestamp;
- branch;
- HEAD SHA;
- staged changes;
- unstaged changes;
- untracked files;
- fingerprints/state needed for later attribution;
- optional tool/model/agent metadata.

Rules:

- dirty worktree is allowed and explicitly recorded;
- do not attribute existing dirty files to the run;
- refuse a second active run by default;
- do not persist full source contents unless a future explicit opt-in contract requires it.

## `agentswatch finish <run-id>`

Purpose: capture end state and compute attributable run delta.

Required behavior:

- load matching start baseline;
- capture end repository evidence;
- compute attributable additions/modifications/deletions/renames/untracked changes;
- separate pre-existing unchanged dirty state;
- surface pre-existing files changed further during the run;
- preserve ambiguous attribution explicitly;
- generate/update `RunReceipt v1`.

Writes:

```text
.agentwatch/runs/<run-id>.json
.ai/runs/<run-id>.md
```

The completed baseline may be removed/moved from `active-runs` only after the final receipt is safely written.

## `agentswatch receipt show <run-id>`

Purpose: render the canonical receipt concisely.

Default output includes:

```text
Run:
Contract:
Agent/tool/model:
Attributable files:
Pre-existing/ambiguous files:
Validation:
Claims:
Scope findings:
Acceptance findings:
Decision:
Reasons:
```

## `agentswatch evidence check <run-id>`

Purpose: run deterministic evidence and validation requirements against the receipt/contract.

Must never infer `validation passed` merely from agent prose.

Possible results:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

## `agentswatch drift check <run-id>`

Purpose: compare attributable changes against `ownedPaths` and `avoidPaths`.

Output must list exact path and violated rule/reason.

Raw pre-existing dirty files do not count as drift unless run attribution shows they changed further.

## `agentswatch claims check <run-id>`

Purpose: verify supported deterministic claim classes against receipt evidence.

Initial classes:

```text
TestsAdded
DocsOnly
BackendUnchanged
MigrationAdded
ValidationPassed
NoUnrelatedChanges
```

Claim extraction may initially be explicit/imported. LLM extraction is not required for MVP.

## `agentswatch handoff <run-id>`

Purpose: generate compact continuation context from the structured receipt.

Writes:

```text
.ai/handoffs/<run-id>.md
```

Target 10–20 lines. Include:

- task/decision;
- attributable files;
- validation summary;
- unsupported/unknown evidence;
- residual risk;
- next minimal prompt.

Do not copy full chat/session or terminal logs.

## `agentswatch status`

MVP target output:

```text
Project root:
Detected types:
Branch:
Commit:
Dirty files:
Active run:
Latest receipt:
Open verification findings:
Next safe prompt:
```

Must handle non-git directories clearly and avoid running validation by default.

## `agentswatch optimize`

Retain as a secondary deterministic helper for broad-prompt lint/splitting.

Do not expand it ahead of Contract/Run/Receipt/Evidence work.

## Validation adapters

Initial product priority:

1. universal git;
2. .NET;
3. Flutter.

Suggested validation remains non-executing by default.

## Canonical output rule

Structured JSON is authoritative:

```text
.agentwatch/contracts/*.json
.agentwatch/active-runs/*.json
.agentwatch/runs/*.json
```

Markdown is a generated projection:

```text
.ai/runs/*.md
.ai/handoffs/*.md
```

No verification command may depend on reparsing Markdown prose.
