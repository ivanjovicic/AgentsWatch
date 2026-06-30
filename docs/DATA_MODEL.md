# AgentsWatch Data Model

Last aligned: 2026-06-30  
Status: draft contract

## Purpose

Start with markdown files. Move to SQLite only after the CLI report model stabilizes.

This document defines the future local data model so early commands do not invent incompatible shapes.

---

## Storage phases

### Phase 1 — Markdown only

Use:

```text
.ai/tasks/
.ai/runs/
.ai/generated/
.ai/STATUS.md
.ai/CHANGELOG_AI.md
```

### Phase 2 — JSON sidecars

Optional machine-readable run files:

```text
.agentwatch/runs/<run-id>.json
.agentwatch/command-history.jsonl
```

### Phase 3 — SQLite

Future local history:

```text
.agentwatch/agentswatch.db
```

---

## Entities

### Project

```text
ProjectId
Name
RootPath
DetectedTypes
CreatedAt
UpdatedAt
```

### Task

```text
TaskId
Title
RunMode
TokenBudget
Status
PromptPath
CreatedAt
UpdatedAt
```

### Run

```text
RunId
TaskId
StartedAt
FinishedAt
Tool
Model
StartCommitSha
EndCommitSha
Branch
RiskLevel
Status
```

### ChangedFile

```text
RunId
Path
Status
AddedLines
DeletedLines
RiskLevel
RiskReasons
```

### Validation

```text
RunId
Command
Status
StartedAt
FinishedAt
OutputSummary
```

### CommandProfile

Planned for AW-011.

```text
CommandId
RunId
WorkingDirectory
DetectedProjectTypes
Command
StartedAt
FinishedAt
DurationMs
ExitCode
StdoutBytes
StderrBytes
Status
FirstErrorLine
OutputSummary
SuggestedByAgentsWatch
ChangedFilesBefore
ChangedFilesAfter
```

Rules:

- command profiles are local-first evidence;
- markdown reports should include compact summaries only;
- full stdout/stderr is not part of the default data model;
- optional raw log files, if added later, must be local-only and referenced by path only when explicitly requested;
- secret-looking values must be redacted before `OutputSummary` is stored.

Phase 2 JSONL shape:

```json
{
  "schemaVersion": 1,
  "commandId": "2026-06-30T120000Z-dotnet-test-filter-git",
  "runId": "optional-run-id",
  "workingDirectory": ".",
  "detectedProjectTypes": ["dotnet"],
  "command": "dotnet test --filter Git",
  "startedAtUtc": "2026-06-30T12:00:00Z",
  "finishedAtUtc": "2026-06-30T12:00:04Z",
  "durationMs": 4012,
  "exitCode": 0,
  "stdoutBytes": 8200,
  "stderrBytes": 0,
  "status": "Pass",
  "firstErrorLine": null,
  "outputSummary": "Tests passed. 12 tests run.",
  "suggestedByAgentsWatch": true,
  "changedFilesBefore": 2,
  "changedFilesAfter": 2
}
```

### RiskFinding

```text
RunId
Level
Category
Message
Path
SuggestedFollowUp
```

### Handoff

```text
RunId
SummaryPath
NextPrompt
ResidualRisk
```

---

## Status values

Task status:

```text
Ready
InProgress
Done
Blocked
NeedsEvidence
NeedsHandoff
```

Validation status:

```text
Pass
Fail
NotRun
BlockedByEnvironment
```

Command status:

```text
Pass
Fail
NotRun
BlockedByEnvironment
TimedOut
Killed
Unknown
```

Risk levels:

```text
Low
Medium
High
```

---

## Compatibility rule

Every markdown report should be convertible to the future JSON/SQLite model without losing:

- task id;
- run mode;
- budget;
- changed files;
- validation status;
- command profile summary if available;
- risk findings;
- missed work;
- follow-up prompt;
- handoff summary.