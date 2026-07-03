# AgentsWatch Reproducible Acceptance Scenarios

Last aligned: 2026-07-03  
Status: executable acceptance contract

## Purpose

Define black-box scenarios that prove observable CLI behavior without trusting implementation details or final chat claims.

Each scenario must run in a clean temporary workspace and record:

```text
Scenario ID
Capability IDs
Tested commit/package
Operating system
.NET/runtime version
Given
When
Then
Exit code
Stdout/stderr anchors
Created/changed files
Forbidden side effects
Result
Evidence path
```

## General rules

- Use temporary directories and disposable git repositories.
- Pin the commit or package checksum.
- Use fixed input files and normalize timestamps/paths in golden output.
- Capture stdout, stderr, exit code, file tree before/after, and checksums when relevant.
- Never use the developer's real home/repository for write tests.
- A skipped scenario is not a pass.
- A scenario only proves the capability IDs it names.

## Gate 0 scenarios

### AW-SCN-HELP-001 — Help works

Capability: AW-CAP-001

Given:
- built CLI for the tested commit.

When:
- run `agentswatch --help`.

Then:
- exit code is 0;
- stdout includes product name and supported current commands;
- stderr is empty;
- no files are created.

### AW-SCN-VERSION-001 — Version is consistent

Capability: AW-CAP-002

When:
- run `agentswatch --version`.

Then:
- exit code is 0;
- output contains exactly one parseable version;
- version matches the built package metadata;
- no files are created.

### AW-SCN-UNKNOWN-001 — Unknown command fails honestly

Capabilities: AW-CAP-001, AW-CAP-002

When:
- run `agentswatch command-that-does-not-exist`.

Then:
- exit code is the documented user-error code;
- stderr identifies the unknown command;
- help guidance is shown;
- no files are created.

## Init scenarios

### AW-SCN-INIT-001 — Fresh initialization

Capability: AW-CAP-003

Given:
- empty temporary repository.

When:
- run `agentswatch init`.

Then:
- exit code is 0;
- required `.ai/` and `.agentwatch/` paths exist;
- generated files match the documented contract;
- files are inside the selected repository only.

### AW-SCN-INIT-002 — Idempotent second run

Given:
- workspace initialized once.

When:
- run `agentswatch init` again.

Then:
- exit code is 0;
- existing file hashes remain unchanged;
- output reports preserved/existing artifacts;
- no duplicate files are created.

### AW-SCN-INIT-003 — User edits are preserved

Given:
- initialize workspace;
- edit a generated user-owned file.

When:
- run `agentswatch init` again.

Then:
- edited content remains unchanged;
- command reports preservation;
- no backup/destructive overwrite occurs unless explicitly documented and requested.

### AW-SCN-INIT-004 — Path escape is rejected

Given:
- config or path input attempts to write outside the selected repository.

When:
- run the relevant init command.

Then:
- command fails safely;
- outside file tree is unchanged;
- stderr explains the boundary.

## Optimize scenarios

### AW-SCN-OPTIMIZE-001 — Broad prompt is constrained

Capabilities: AW-CAP-004, AW-CAP-005, AW-CAP-006

Given:
- prompt requests whole-repo analysis, implementation, tests, docs, and review.

When:
- run `agentswatch optimize <prompt>`.

Then:
- risk is High;
- broad scope and multi-mode waste causes are reported;
- token budget is constrained;
- split recommends investigation, implementation, tests, and review;
- optimized prompt includes scope, stop, validation, and return sections.

### AW-SCN-OPTIMIZE-002 — Scoped prompt remains focused

Given:
- prompt names one file, one behavior, a stop condition, and targeted validation.

Then:
- risk is Low or the documented expected level;
- output does not invent unrelated repositories/files;
- single focused execution is recommended.

### AW-SCN-OPTIMIZE-003 — Missing input fails

When:
- run `agentswatch optimize` without prompt text/file.

Then:
- documented user-error exit code;
- usage goes to stderr or documented stream;
- no artifacts are written.

### AW-SCN-OPTIMIZE-004 — Prompt file input

Given:
- a fixed UTF-8 prompt file.

When:
- run optimize with its path.

Then:
- file content is analyzed;
- output is deterministic after normalizing environment data;
- source file remains unchanged.

## Status scenarios

### AW-SCN-STATUS-001 — Clean .NET repository

Capabilities: AW-CAP-007, AW-CAP-008, AW-CAP-009, AW-CAP-010

Given:
- temporary clean git repo with minimal `.sln` file.

Then:
- branch and commit are shown;
- changed file count is zero;
- .NET is detected;
- .NET validation suggestions are present.

### AW-SCN-STATUS-002 — Dirty/untracked repository

Given:
- one modified, one added, and one untracked file.

Then:
- each path/status is represented correctly;
- no file content is printed by default.

### AW-SCN-STATUS-003 — Mixed project repository

Given:
- minimal .NET and Flutter fixture markers.

Then:
- both types are detected;
- suggestions are scoped and deterministic;
- command does not execute validation automatically.

### AW-SCN-STATUS-004 — Non-git directory

Then:
- behavior matches the documented contract;
- error is clear and non-destructive;
- no misleading branch/commit is invented.

### AW-SCN-STATUS-005 — Repository path contains spaces

Then:
- command succeeds and paths remain correct.

## Future run/report scenarios

### AW-SCN-FINISH-001 — Validation not run is not pass

Capabilities: AW-CAP-013, AW-CAP-014

Given:
- a started run with changed files and no validation execution.

Then:
- finish/report records NotRun plus reason;
- completion gate does not treat validation as success.

### AW-SCN-FINISH-002 — Missing required evidence blocks completion

Then:
- command returns NeedsEvidence/Blocked according to contract;
- missing fields are listed;
- no high-confidence Done status is produced.

### AW-SCN-REPORT-001 — Stable report golden

Then:
- markdown contains required sections in stable order;
- unknown values use documented placeholders;
- paths/files are sorted deterministically;
- full diff/log content is absent by default.

## Learning/discovery scenarios

### AW-SCN-DISC-001 — Out-of-scope finding is preserved

Capabilities: AW-CAP-022, AW-CAP-023

Given:
- run log contains one actionable out-of-scope finding.

Then:
- one discovery record is created/updated;
- originating run is linked;
- category, confidence, severity, owner, queue, and disposition exist;
- a focused prompt is generated or no-op reason recorded.

### AW-SCN-DISC-002 — Duplicate finding reuses canonical record

Given:
- two runs describe the same root issue.

Then:
- one canonical discovery remains;
- second run links to it;
- seen/review evidence updates;
- no parallel queue item is created.

### AW-SCN-DISC-003 — Uncertain finding becomes investigation

Then:
- confidence is Possible/Unknown;
- generated prompt is investigation-only;
- implementation is not marked Ready.

### AW-SCN-DISC-004 — Missing reconciliation fails lint

Then:
- evidence/discovery lint fails;
- run is not learning-complete;
- missing owner/prompt/disposition is reported.

### AW-SCN-DISC-005 — Stale resolved item closes with evidence

Then:
- resolution links to commit/PR/test evidence;
- history is retained;
- item is not deleted.

### AW-SCN-DISC-006 — Security finding needs review

Then:
- automatic closure is refused;
- explicit review/evidence requirement is reported.

## Privacy/safety scenarios

### AW-SCN-PRIV-001 — No default network calls

Given:
- network is blocked/monitored.

When:
- run current local commands.

Then:
- commands succeed or fail for local reasons without attempted outbound connections.

### AW-SCN-PRIV-002 — No writes outside repo

Then:
- before/after snapshot outside selected repo is unchanged.

### AW-SCN-PRIV-003 — Secret-like values are not emitted

Given:
- fixture contains fake secret patterns.

Then:
- default reports/output redact or omit them according to policy.

### AW-SCN-PRIV-004 — Binary content is not inlined

Then:
- binary path may be listed;
- binary body is absent.

## Package/release scenarios

### AW-SCN-PACK-001 — Package is created

Capability: AW-CAP-025

Then:
- `dotnet pack` succeeds;
- one expected tool package is produced;
- package version equals manifest version;
- SHA-256 checksum is recorded.

### AW-SCN-PACK-002 — Clean tool install

Given:
- clean isolated tool path and packaged artifact.

Then:
- installation succeeds;
- help/version scenarios pass using installed tool;
- no source checkout is required.

### AW-SCN-PROOF-001 — Proof manifest matches commit

Capability: AW-CAP-027

Then:
- manifest commit equals checked-out/tested commit;
- artifact checksums verify;
- required files exist;
- capability claims do not exceed linked evidence.

## Scenario execution status

Until CI or local evidence is attached, all scenarios in this document are contracts, not passing results.
