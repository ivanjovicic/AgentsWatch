# AW-011 Command Profiler / Fast Validation Advisor

Last aligned: 2026-06-30  
Status: planned docs-only epic; blocked until Gate 0 validation and validation runner groundwork

## Purpose

AgentsWatch should help agents avoid slow, repeated, and overly broad validation commands without sending large terminal logs back into the model context.

The feature records local command telemetry, summarizes command outcomes, and recommends faster validation alternatives for the detected project stack.

Core rule:

```text
Profile commands locally. Show agents only compact command evidence.
```

## Product value

This epic extends the core promise from prompt token reduction to command-loop reduction:

- fewer repeated full test/build runs;
- fewer pasted terminal logs;
- faster agent feedback loops;
- more honest validation evidence;
- better language-specific next-command recommendations.

It is especially useful when agents repeatedly run commands such as full `dotnet test`, full `flutter test`, full `npm test`, or full `pytest` after small scoped edits.

## Gate

Do not implement runtime behavior until:

1. `AW-VAL-001` restore/build/test evidence exists;
2. `AW-VAL-002` CLI smoke evidence exists;
3. `AW-003` run report groundwork exists;
4. `AW-008` validation command suggestion behavior is at least partially designed or implemented.

Docs-only planning is allowed before the gate, but it must not claim runtime completion.

## New CLI concepts

### `agentswatch run -- <command>`

Purpose: run a command through a local profiler wrapper.

MVP behavior later:

```bash
agentswatch run -- dotnet test --filter Git
agentswatch run -- flutter analyze
agentswatch run -- npm run build
agentswatch run -- pytest tests/foo -q
```

Records:

- command text;
- working directory;
- detected project types;
- start/end timestamps;
- duration in milliseconds;
- exit code;
- stdout/stderr byte counts;
- first useful error line;
- compact output summary;
- whether the command was suggested by AgentsWatch;
- before/after git changed-file counts when available.

Does not write full stdout/stderr to markdown reports by default.

### `agentswatch validate --suggest`

Purpose: recommend validation commands without running them.

Output should include:

- detected project types;
- changed-file hints;
- recommended fast validation;
- recommended full validation;
- commands to avoid by default;
- reason for each recommendation.

### `agentswatch validate --profile`

Purpose: show local history of slowest and most useful commands.

Output should include only compact summaries, for example:

```text
Slowest recent commands:
1. dotnet test AgentsWatch.sln — 42.3s — fail
2. npm run build — 31.1s — pass

Fast alternatives:
- dotnet test --filter Git
- npm run typecheck
```

### Future `agentswatch validate --run fast`

Later behavior only. Running commands must remain explicit and visible.

## Storage

Phase 1 storage should stay local and simple:

```text
.agentwatch/command-history.jsonl
```

Optional raw command logs, if added later, must be local-only and excluded from reports by default:

```text
.agentwatch/logs/<command-id>.stdout.txt
.agentwatch/logs/<command-id>.stderr.txt
```

Raw logs should be disabled or size-limited in MVP unless there is a clear debug need.

## Command history record shape

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

## Token-safe report section

Run reports may include this compact section:

```markdown
## Command profile

Slowest commands:
- `dotnet test AgentsWatch.sln`: 42.3s, fail, error signature `CS0246`
- `dotnet test --filter Git`: 3.8s, pass

Recommended next validation:
- `dotnet build --no-restore`
- `dotnet test --filter Git`

Avoid by default:
- full solution test unless shared contracts changed

Command-output policy:
- full stdout/stderr omitted from report
- first useful error line recorded only if present
```

## Log and token rules

Non-negotiable rules:

- never paste full command logs into prompts by default;
- never write full stdout/stderr into markdown run reports by default;
- store byte counts, duration, exit code, and compact error signatures;
- include at most the first useful error line and a short output summary;
- include full logs only behind an explicit debug/export flag later;
- redact secrets before writing any command-output summary;
- prefer command summaries in handoff files.

## Fast validation ladder

AgentsWatch should recommend the narrowest useful validation first, then widen only when needed.

```text
Level 0: no command; explain why validation is not needed or blocked
Level 1: format/lint/typecheck for changed files or affected project
Level 2: targeted test for affected area
Level 3: project/package build
Level 4: full test suite
Level 5: CI/release validation
```

The agent should not jump to Level 4 or Level 5 unless the changed files justify it.

## Language-specific rules

### .NET

Fast suggestions:

```bash
dotnet build --no-restore
dotnet test --no-build --filter <test-filter>
dotnet test <test-project> --filter <test-filter>
```

Default full validation:

```bash
dotnet restore
dotnet build
dotnet test
```

Avoid by default:

- full solution test after a one-file parser/formatter change;
- restore on every run when dependencies did not change;
- rebuild before every targeted test if a successful build already exists.

Widen validation if these changed:

- `.sln`, `.csproj`, `Directory.Build.props`, `Directory.Packages.props`;
- dependency injection composition;
- public API contracts;
- migrations;
- auth/security/config files.

### Flutter

Fast suggestions:

```bash
flutter analyze
flutter test test/<affected_test>.dart
```

Default full validation:

```bash
flutter analyze
flutter test
```

Avoid by default:

- full widget test suite after a tiny pure-Dart utility change;
- platform builds unless platform files changed.

Widen validation if these changed:

- `pubspec.yaml`;
- platform folders;
- routing/navigation;
- persistence/offline/state providers;
- localization or theme-wide files.

### React / TypeScript

Fast suggestions:

```bash
npm run typecheck
npm test -- <affected-test>
npm run lint -- <changed-file>
```

Default full validation:

```bash
npm run build
npm test
npm run lint
```

Avoid by default:

- full build after a CSS-only or copy-only change unless routing/build config changed;
- broad snapshot updates without diff review.

Widen validation if these changed:

- `package.json`, lockfiles;
- `vite.config.*`, `tsconfig.json`;
- API clients;
- route guards/auth state;
- generated clients.

### Python

Fast suggestions:

```bash
ruff check <changed-files>
pytest tests/<affected-area> -q
mypy <affected-package>
```

Default full validation:

```bash
ruff check .
pytest
mypy .
```

Avoid by default:

- full `pytest` after a one-file utility change;
- mypy across the whole repo unless type contracts changed.

Widen validation if these changed:

- `pyproject.toml`, `requirements.txt`, lockfiles;
- migrations;
- IO/network wrappers;
- background jobs;
- auth/security files.

### Node

Fast suggestions:

```bash
npm test -- <affected-test>
npm run lint -- <changed-file>
```

Default full validation:

```bash
npm test
npm run build
npm run lint
```

Widen validation if these changed:

- `package.json`, lockfiles;
- build scripts;
- runtime configuration;
- shared library entrypoints.

## Recommendation algorithm draft

1. Detect project types using adapters.
2. Read changed files from git snapshot when available.
3. Match changed files to likely adapter and risk category.
4. Pick the cheapest command that covers the change.
5. Check command history for recent slow/failing commands.
6. Recommend fast validation first.
7. Recommend full validation only when risk or changed files justify it.
8. Print concise reasons.

## Example output

```text
AgentsWatch validation advisor

Detected types: dotnet
Changed files: 2
Risk: Medium

Recommended fast validation:
- dotnet build --no-restore
- dotnet test --filter Git

Why:
- changed files are under AgentsWatch.Git
- last full solution test took 42.3s
- targeted Git tests previously passed in 3.8s

Avoid by default:
- dotnet test AgentsWatch.sln unless project references or shared contracts changed
```

## Tests to add later

- command history record serializes with required fields;
- failed command captures exit code and first useful error line;
- markdown report omits full stdout/stderr;
- advisor suggests `.NET` targeted tests for `.cs` changes;
- advisor suggests full restore/build when `.csproj` changes;
- advisor suggests `flutter analyze` / targeted `flutter test` for Dart changes;
- advisor suggests `npm run typecheck` for TypeScript changes;
- advisor suggests `ruff check <changed-files>` for Python changes;
- sensitive-looking output is redacted in summaries.

## Files likely involved later

```text
src/AgentsWatch.Cli/
src/AgentsWatch.Core/
src/AgentsWatch.LanguageAdapters/
src/AgentsWatch.Reports/
tests/AgentsWatch.Tests/
docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md
```

## Stop rules

Stop and report instead of implementing if:

- Gate 0 validation evidence is still missing;
- command execution would require shell-specific behavior not designed yet;
- capturing logs risks storing secrets;
- adapter detection is too uncertain;
- the implementation needs broad refactor across CLI, reports, config, and adapters in one run;
- validation cannot be proven without running unsafe or very slow commands.
