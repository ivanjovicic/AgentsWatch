# AgentsWatch Module Boundaries

Last aligned: 2026-06-29  
Status: target boundary map

## Purpose

This document prevents future implementation from turning into one large CLI project.

Use it when adding or moving code.

---

## Dependency direction

Allowed direction:

```text
CLI / future API / dashboard
  -> Application
    -> Domain

Infrastructure adapters
  -> Application ports / abstractions
```

Forbidden:

```text
Domain -> CLI
Domain -> Git process
Domain -> File system
Domain -> SQLite
Domain -> GitHub API
Domain -> Console
```

---

## Target modules

### Domain

Owns pure rules and concepts:

- run modes;
- token budgets;
- task/run state;
- risk levels;
- risk findings;
- policy rule results;
- prompt optimization result models.

Should not:

- read files;
- run git;
- write reports;
- print console output.

### Application

Owns use cases:

- initialize project;
- optimize prompt;
- split task;
- start run;
- finish run;
- generate report;
- generate handoff;
- generate diff-only review;
- run validation.

Should orchestrate domain logic and ports.

### Abstractions / Ports

Defines contracts:

- `IFileSystem`;
- `IGitRepository`;
- `IProcessRunner`;
- `IClock`;
- `IReportStore`;
- `IRunStore`;
- `IValidationRunner`;
- `IProjectDetector`.

### CLI

Owns:

- argument parsing;
- command routing;
- console rendering;
- exit codes.

Should not own:

- risk scoring;
- report format decisions;
- git parsing;
- storage rules.

### Git adapter

Owns:

- git command execution;
- git status/diff parsing;
- commit/branch lookup.

Should expose domain-friendly models and hide command details.

### File-system / storage adapters

Own:

- markdown file writes;
- JSON sidecar writes later;
- SQLite persistence later;
- atomic write helpers;
- no-overwrite behavior.

### Reports

Owns formatting:

- run report markdown;
- handoff markdown;
- diff-only review prompt;
- changelog entries;
- status file sections.

Reports should consume already-calculated domain/application data.

### Language adapters

Own stack-specific hints:

- project detection;
- validation suggestions;
- high-risk path patterns;
- likely test locations.

They must not own task/run lifecycle.

---

## Current-to-target mapping

Current project | Target role
---|---
`AgentsWatch.Cli` | CLI interface; later calls application use cases.
`AgentsWatch.Core` | temporary home for domain/application; later split.
`AgentsWatch.Git` | Git adapter.
`AgentsWatch.LanguageAdapters` | Adapter registry and stack hints.
`AgentsWatch.Reports` | Markdown/report formatting.
`AgentsWatch.Tests` | temporary combined tests; later split by module.

---

## Extraction order after Gate 0

1. Keep existing projects building.
2. Add application/use-case classes inside current structure first.
3. Extract file-system writes from CLI into a service/port.
4. Move prompt optimizer behind an application use case.
5. Move git snapshot flow into application orchestration.
6. Add report store abstraction before JSON/SQLite.
7. Split `Core` only when there are enough classes to justify it.

---

## Stop rules for agents

Stop and ask for a smaller prompt if a change requires:

- moving many projects at once;
- renaming solution/projects before Gate 0 passes;
- introducing dashboard/API before CLI reports are useful;
- adding cloud dependencies to core modules;
- making domain depend on console, git, file system, or HTTP.
