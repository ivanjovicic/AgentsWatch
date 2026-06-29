# AgentsWatch Data Model

Last aligned: 2026-06-29  
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
- risk findings;
- missed work;
- follow-up prompt;
- handoff summary.
