# AgentsWatch Run Evidence Acceptance Scenarios

Last aligned: 2026-07-05  
Status: executable contract; not passing until commit-bound execution evidence exists

## Rules

Each scenario runs in a disposable temporary repository and records:

```text
scenario ID
capability IDs
commit/package
OS/runtime
Given / When / Then
exit code
stdout/stderr anchors
file tree and checksums
forbidden side effects
result/evidence path
```

A skipped or specified-only scenario is not a pass.

## AW-SCN-START-001 — Clean baseline starts

Capabilities: AW-CAP-012, AW-CAP-014

Given:

- clean temporary Git repository;
- one initial commit;
- no active run.

When:

```bash
agentswatch start TASK-001 --title "Test run" --scope "src/**"
```

Then:

- exit 0;
- stdout contains `Run started`, `Task: TASK-001`, `Base commit:`, `Validation: NotRun`;
- `.agentwatch/runs/TASK-001.json` exists;
- `.ai/runs/TASK-001.md` exists;
- manifest status is `InProgress`;
- manifest validation is `NotRun`;
- manifest contains the full current object ID;
- neither artifact contains the absolute temporary root;
- no network access occurs.

## AW-SCN-START-002 — User dirty state is refused

Capabilities: AW-CAP-012

Given:

- clean committed repository;
- modify or add `src/UserChange.cs` before start.

Then:

- exit 3;
- stderr identifies dirty attribution boundary and the path;
- no task JSON/Markdown is created;
- existing files are unchanged.

## AW-SCN-START-003 — Prior managed reports do not block

Capabilities: AW-CAP-012, AW-CAP-014

Given:

- prior finished run artifacts exist and are untracked/modified only under:
  - `.agentwatch/runs/*.json`;
  - `.ai/runs/*.md`;
- no active manifest;
- no other dirty files.

Then:

- new start exits 0;
- prior artifacts are preserved;
- new task artifacts are created;
- no prior artifact is attributed to the new run.

## AW-SCN-START-004 — Duplicate task refuses overwrite

Capabilities: AW-CAP-012

Given:

- `TASK-001` manifest/report already exist.

When:

- run start for `TASK-001` again.

Then:

- exit 3;
- existing checksums remain unchanged;
- stderr states that evidence will not be overwritten.

## AW-SCN-START-005 — Second active run is refused

Capabilities: AW-CAP-012

Given:

- one valid `InProgress` manifest.

When:

- start a different task.

Then:

- exit 3;
- active task ID/start time are shown;
- no second task artifacts are created.

## AW-SCN-START-006 — Unsafe task ID is rejected

Capabilities: AW-CAP-012

Inputs include:

```text
../escape
task/name
task name
```

Then:

- exit 2;
- no write outside/inside the repository;
- stderr explains valid characters.

## AW-SCN-FINISH-001 — Committed change is captured

Capabilities: AW-CAP-013, AW-CAP-014

Given:

- active run at object A;
- add/modify allowed file and commit object B.

When:

```bash
agentswatch finish TASK-001
```

Then:

- exit 0;
- manifest status becomes `Finished`;
- start object A and end object B recorded;
- changed file/status listed;
- `Validation: NotRun` remains;
- report states command/runtime evidence boundary.

## AW-SCN-FINISH-002 — Uncommitted and untracked paths captured honestly

Capabilities: AW-CAP-013

Given:

- active run;
- one modified tracked path and one untracked path.

Then:

- both paths listed;
- untracked status normalized;
- warning states non-AgentsWatch uncommitted changes exist;
- finish does not claim validation passed.

## AW-SCN-FINISH-003 — Out-of-scope path is identified

Capabilities: AW-CAP-013, AW-CAP-014

Given:

- allowed scope `src/Payments/**`;
- changes in `src/Payments/Retry.cs` and `src/Auth/Token.cs`.

Then:

- both in changed files;
- only Auth path in out-of-scope section;
- no source content/full diff emitted.

## AW-SCN-FINISH-004 — Unrestricted scope is not falsely clean

Capabilities: AW-CAP-013, AW-CAP-014

Given:

- run started without `--scope`.

Then:

- report says scope/out-of-scope evaluation was not applicable;
- report does not claim zero violations as a verified result;
- warning records missing restriction.

## AW-SCN-FINISH-005 — Managed artifacts excluded

Capabilities: AW-CAP-013

Given:

- active run artifacts and older managed run artifacts are untracked/modified;
- one user source path changes.

Then:

- only user source path is attributed;
- no `.agentwatch/runs/*.json` or `.ai/runs/*.md` appears in changed/out-of-scope findings.

## AW-SCN-FINISH-006 — Branch change warning

Capabilities: AW-CAP-013

Given:

- run starts on branch A;
- finishes on branch B.

Then:

- both branch identities retained;
- warning explicitly states A -> B;
- finish still records Git/scope evidence without claiming intent.

## AW-SCN-FINISH-007 — Second finish is refused

Capabilities: AW-CAP-013

Given:

- manifest status `Finished`.

Then:

- exit 3;
- JSON/Markdown checksums unchanged;
- stderr says finished evidence will not be rewritten.

## AW-SCN-FINISH-008 — Missing run is clear

Capabilities: AW-CAP-013

Then:

- exit 3;
- stderr identifies missing task;
- no artifacts created.

## AW-SCN-REPORT-001 — Explicit report

Capabilities: AW-CAP-014

Given:

- valid run manifest.

When:

```bash
agentswatch report TASK-001
```

Then:

- exit 0;
- stdout contains stable sections;
- file paths sorted;
- validation status shown;
- no absolute local root/full logs/diffs.

## AW-SCN-REPORT-002 — Latest report default

Capabilities: AW-CAP-014

Given:

- two valid manifests with different `StartedAt`.

When:

```bash
agentswatch report
```

Then:

- newest started run selected deterministically;
- no files rewritten.

## AW-SCN-REPORT-003 — No runs

Capabilities: AW-CAP-014

Then:

- exit 3;
- stderr says no run evidence exists;
- no artifacts created.

## AW-SCN-MANIFEST-001 — Unknown schema rejected

Capabilities: AW-CAP-012..014

Given:

- sidecar with unsupported schema version.

Then:

- command fails non-zero;
- no silent migration/defaulting;
- stderr identifies unsupported schema;
- source file remains unchanged.

## AW-SCN-MANIFEST-002 — Invalid Git object rejected

Capabilities: AW-CAP-012..014

Given:

- malformed/tampered persisted start or end object ID.

Then:

- load/save fails;
- arbitrary revision text is not passed to Git;
- no report presents it as valid evidence.

## AW-SCN-PRIV-005 — Run artifacts contain no absolute root

Capabilities: AW-CAP-014, AW-CAP-026

Given:

- repository path contains a user/profile path and spaces.

Then:

- CLI may print local root to the local terminal;
- JSON and Markdown artifacts do not contain that absolute root;
- repository-relative changed paths remain available.

## Required execution matrix before L3

- Ubuntu latest/.NET 8;
- Windows latest/.NET 8;
- path with spaces;
- LF and CRLF fixtures;
- clean, dirty, untracked, rename, delete;
- SHA-1 object IDs; SHA-256 support at least unit-tested where Git fixture availability is limited.

## Proof result rule

AW-CAP-012, AW-CAP-013, and AW-CAP-014 stay L2 until targeted tests and the relevant black-box scenarios execute against the same commit/package.
