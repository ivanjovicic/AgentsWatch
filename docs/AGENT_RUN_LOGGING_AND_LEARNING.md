# Agent Run Logging and Learning Loop

Last aligned: 2026-06-30  
Status: product/architecture plan; docs-only

## Purpose

AgentsWatch should log what happened after every prompt so the next prompt is smaller, safer, and less repetitive.

Core rule:

```text
Every agent run must leave compact evidence and one learning note.
```

This is not application telemetry. It is local-first run memory for the project.

## What exists already

AgentsWatch already has draft contracts for:

- run reports in `docs/REPORT_FORMATS.md`;
- handoff summaries in `docs/REPORT_FORMATS.md`;
- command profiles in `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`;
- validation records in `docs/DATA_MODEL.md`;
- Flutter detection and validation suggestions in `docs/ADAPTER_SPEC.md`.

What was missing:

- explicit post-prompt agent logging;
- mistake pattern tracking;
- prompt improvement feedback;
- per-stack lessons, especially Flutter state/test/validation lessons;
- a rule for what not to send back to future agents.

## Storage phases

### Phase 1 — markdown

Use:

```text
.ai/runs/<run-id>.md
.ai/runs/<run-id>-handoff.md
.ai/learning/LESSONS.md
.ai/learning/MISTAKE_PATTERNS.md
.ai/learning/DO_NOT_REPEAT.md
```

### Phase 2 — JSON sidecars

Use later:

```text
.agentwatch/runs/<run-id>.json
.agentwatch/learning-events.jsonl
.agentwatch/mistake-patterns.json
```

### Phase 3 — local database

Use later:

```text
.agentwatch/agentswatch.db
```

## Post-prompt log contract

After each prompt, record:

```text
Run ID:
Prompt ID:
Queue item:
Tool:
Model:
Permission mode:
Run mode:
Token budget:
Started:
Finished:
Status:
Files inspected:
Files changed:
Validation:
Command profile:
Mistakes:
Missed work:
Scope creep:
Token waste:
Learning note:
Next prompt:
Do not repeat:
```

The log must be compact. It should be useful without chat history.

## Learning note

Every run should produce one learning note:

```text
Next time, avoid <waste/mistake> by <specific rule>.
```

Good examples:

```text
Next time, avoid reading full adapter docs for one Flutter widget change; read only Flutter adapter and changed widget.
Next time, run flutter analyze before full flutter test for UI-only changes.
Next time, split provider state refactor from navigation changes.
```

Bad examples:

```text
Be better next time.
Fix everything.
Read the whole repo.
```

## Mistake categories

Track these categories:

```text
ScopeCreep
OverRead
OverTest
UnderTest
ValidationSkipped
WrongModel
WrongTool
SensitivePathTouched
LargeLogPasted
RepeatedFailure
MissingHandoff
ClaimedButNotDone
FlutterStateRisk
FlutterNavigationRisk
FlutterPersistenceRisk
```

## Token-saving learning rules

A learning rule may reduce future token use when it is specific and repeatable.

Examples:

- prefer changed files + relevant adapter over whole-repo read;
- prefer compact command profile over full logs;
- prefer targeted validation before full validation;
- prefer diff-only review after implementation;
- prefer investigation-only after repeated failure;
- prefer Flutter analyze before broad widget tests;
- prefer provider/state focused prompt before UI refactor prompt.

## Flutter-specific run logging

For Flutter projects, record these extra signals when relevant:

```text
Flutter project detected:
Touched widgets:
Touched providers/state:
Touched navigation/router:
Touched persistence/offline storage:
Touched platform files:
Validation suggested:
Validation run:
Widget tests affected:
Risk note:
```

Flutter adapter should recommend the smallest useful validation ladder:

```text
1. flutter analyze
2. targeted flutter test <file-or-name> when available
3. flutter test
4. integration test only when navigation/platform/persistence risk requires it
```

Do not run broad Flutter validation just because a small widget changed.

## What not to log

Do not log:

- full chat history;
- full terminal output;
- secrets;
- raw command strings that may contain secrets;
- entire diffs when file list is enough;
- screenshots or binary data by default;
- unrelated repo context.

## Learning loop

After each run:

```text
1. Write compact run report.
2. Write or update handoff.
3. Extract one learning note.
4. Classify mistake categories.
5. Update DO_NOT_REPEAT only if the mistake is repeatable.
6. Generate next prompt from evidence, not chat history.
```

## Done rule

A run is not Done until it has:

- status;
- changed-file summary or docs-only note;
- validation result or blocked reason;
- missed work;
- learning note;
- next prompt or completion note.

## Privacy and safety

Agent run logging inherits:

- `docs/SECURITY_AND_PRIVACY.md`;
- `docs/AGENT_RISK_BOUNDARIES.md`;
- `docs/COMMAND_PROFILE_PRIVACY_AND_STORAGE_POLICY.md`.

If policies conflict, the safer policy wins.
