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
.ai/learning/LESSONS.md
.ai/learning/MISTAKE_PATTERNS.md
.ai/learning/DO_NOT_REPEAT.md
.ai/STATUS.md
.ai/CHANGELOG_AI.md
```

### Phase 2 — JSON sidecars

Optional machine-readable run files:

```text
.agentwatch/runs/<run-id>.json
.agentwatch/command-history.jsonl
.agentwatch/learning-events.jsonl
.agentwatch/mistake-patterns.json
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
CommandDisplay
CommandHash
CommandRedactionApplied
CommandRefusedReason
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
- secret-looking values must be redacted before `OutputSummary` is stored;
- raw secret-looking command strings must not be persisted or displayed.

Phase 2 JSONL shape:

```json
{
  "schemaVersion": 1,
  "commandId": "2026-06-30T120000Z-dotnet-test-filter-git",
  "runId": "optional-run-id",
  "workingDirectory": ".",
  "detectedProjectTypes": ["dotnet"],
  "commandDisplay": "dotnet test --filter Git",
  "commandHash": "sha256:<hash>",
  "commandRedactionApplied": false,
  "commandRefusedReason": null,
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

### AgentRunLog

```text
RunId
PromptId
QueueItemId
Tool
Model
PermissionMode
RunMode
TokenBudget
Status
FilesInspectedCount
FilesChangedCount
ValidationStatus
CommandProfileSummary
MistakeCategories
MissedWork
ScopeCreep
TokenWasteSummary
LearningNote
NextPrompt
DoNotRepeat
```

Rules:

- one compact log per agent run;
- one learning note per completed or blocked run;
- do not store full chat history;
- do not store full terminal output;
- do not store secrets.

### LearningEvent

```text
LearningEventId
RunId
Category
Message
RuleCandidate
AppliesToProjectTypes
CreatedAt
Accepted
```

Use for project-local learning such as:

```text
Next time, run flutter analyze before full flutter test for small UI-only changes.
```

### MistakePattern

```text
PatternId
Category
Description
SeenCount
LastSeenRunId
RecommendedPromptRule
RecommendedValidationRule
RecommendedContextRule
Status
```

Status values:

```text
Candidate
Accepted
Ignored
Deprecated
```

### FlutterRunSignals

Optional per-run shape for Flutter projects:

```text
RunId
TouchedWidgets
TouchedProvidersOrState
TouchedNavigationOrRouter
TouchedPersistenceOrOfflineStorage
TouchedPlatformFiles
WidgetTestsAffected
ValidationSuggested
ValidationRun
RiskNote
```

Use only when Flutter project files are detected.

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
LearningNote
DoNotRepeat
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
NeedsLearningLog
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

Run status:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

Risk levels:

```text
Low
Medium
High
```

Mistake categories:

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
- learning note;
- do-not-repeat rule if present;
- follow-up prompt;
- handoff summary.