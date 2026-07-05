# AgentsWatch Report Formats

Last aligned: 2026-07-05  
Status: current foundation format plus future report contracts

## Purpose

AgentsWatch produces small, evidence-first Markdown reports that remain useful without the original chat history.

Machine-readable state belongs in `.agentwatch`; human-readable artifacts belong in `.ai`.

Reports must never present planned or unavailable evidence as observed success.

## Current run-evidence foundation

Machine sidecar:

```text
.agentwatch/runs/<task-id>.json
```

Human report:

```text
.ai/runs/<task-id>.md
```

The current implemented report contains:

```markdown
# AgentsWatch Run Evidence — <task-id>

- Title: <title>
- Status: <InProgress|Finished>
- Validation: NotRun
- Started: <timestamp>
- Finished: <timestamp or not finished>
- Start branch: `<branch>`
- Start commit: `<full object id>`
- End branch: `<branch or not captured>`
- End commit: `<full object id or not captured>`

## Allowed scope

- `<glob>`

## Changed files

- `<path>` — <status>

## Outside declared scope

- `<path>` — <status>

## Warnings

- <warning>

## Evidence boundary

This report records Git state and declared scope only.
Validation is `NotRun`; no build, test, CI, UI, database, runtime, or agent-claim evidence was captured by this slice.
```

Current formatting rules:

- changed and out-of-scope paths sorted ordinally;
- no absolute local repository root;
- no source contents, full diffs, full logs, prompts, or secrets;
- managed run artifacts excluded from changed-file attribution;
- empty allowed scope shown as unrestricted/not evaluated;
- validation remains `NotRun` until evidence exists;
- branch/object IDs remain exact and full in machine/human evidence.

See `RUN_EVIDENCE_FOUNDATION_CONTRACT.md`.

## Future full run report

After command/validation/claim evidence exists, the foundation report may expand toward:

```markdown
# AgentsWatch Run Report — <task-id>

Started: <timestamp>
Finished: <timestamp>
Tool: <optional, source/provenance required>
Model: <optional, source/provenance required>
Run mode: <optional>
Token budget: <optional>
Risk: <Low|Medium|High|Unknown>
Validation: <Pass|Fail|NotRun|BlockedByEnvironment|Unknown>

## Scope

Allowed/owned paths:
- <path/glob>

Excluded paths:
- <path/glob>

## Git evidence

Start commit: `<object id>`
End commit: `<object id>`
Branch: `<branch>`

Changed files:
- `<status>` `<path>`

Outside scope:
- `<status>` `<path>`

## Validation evidence

- `<command>`: <status>, <duration>, object `<id>`

## Command profile

- `<command>`: <duration>, <status>, <compact redacted error or none>

Output policy:
- full stdout/stderr omitted
- byte counts and compact summary only

## Claims vs actual

| Claim | Evidence | Status | Confidence |
|---|---|---|---|
| | | | |

## Missed / remaining risk

- <item>

## Learning

- <specific reusable learning or none>

## Follow-up

- <next focused prompt/action or none>
```

No future section may appear as populated unless the required evidence source exists.

## Validation section rules

Allowed validation statuses:

```text
Pass
Fail
NotRun
BlockedByEnvironment
Unknown
```

Rules:

- `Pass` requires captured command/CI/manual evidence with provenance;
- exit code 0 is evidence for that command, not proof of every behavior;
- stale evidence must identify the object/commit mismatch;
- unavailable command events remain `NotRun`, `Unknown`, or `NotObserved` at the claim layer;
- raw terminal output remains outside the default Markdown report.

## Command profile rules

The command profile is governed by `COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`.

Include only:

- redacted command display;
- duration/status;
- start/end object IDs where relevant;
- stdout/stderr byte counts when useful;
- first useful redacted error signature;
- compact summary;
- next smallest useful validation suggestion.

Never include full stdout/stderr by default.

## Claims and Trust Ledger rules

Future claim statuses:

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

Rules:

- one primary status per claim;
- links to concrete evidence IDs/paths;
- confidence reflects evidence quality, not model confidence;
- missing events do not prove an action did not occur;
- stale command/CI evidence must not support the current object.

## Handoff summary — planned

Path:

```text
.ai/runs/<task-id>-handoff.md
```

Target 10-20 lines:

```markdown
# Handoff — <task-id>

Task:
Status:
Relevant files:
Files changed:
Validation:
Command profile:
Missed work:
Next minimal prompt:
Residual risk:
```

No long chat history, raw logs, or unsupported completion claims.

## Diff-only review prompt — planned

Path:

```text
.ai/generated/<task-id>-diff-review.md
```

Required boundaries:

- changed files only by default;
- compact validation/command evidence;
- claims-vs-actual and missed-test checklist;
- explicit instruction not to scan the whole repository unless a changed dependency requires it.

## PR Evidence Packet — planned

Human path:

```text
.ai/evidence/<task-id>-pr-evidence.md
```

Machine path:

```text
.agentwatch/evidence/<task-id>-pr-evidence.json
```

Required later sections:

1. declared task/scope;
2. actual changes;
3. outside-scope changes;
4. claims;
5. commands and validation;
6. CI/artifact evidence;
7. object/commit match;
8. claim classifications;
9. remaining risks;
10. reviewer action list.

The packet is not implemented by the current run foundation.

## Status and changelog files

Planned `.ai/STATUS.md` fields:

- latest run ID;
- latest object/commit;
- validation status;
- compact command summary;
- open risks;
- next action.

Planned `.ai/CHANGELOG_AI.md` entry:

```markdown
## <date> — <task-id>

Changed:
- <summary>

Validation:
- <status and evidence reference>

Risk:
- <level/reason>

Follow-up:
- <action or none>
```

## Compatibility rule

Every Markdown report must be convertible to the future JSON/SQLite model without losing:

- task/run identity;
- lifecycle and timestamps;
- start/end branch/object IDs;
- declared scope;
- changed/out-of-scope files;
- validation status and provenance;
- command summaries;
- risk/findings;
- claims/evidence classifications;
- missed/follow-up state.
