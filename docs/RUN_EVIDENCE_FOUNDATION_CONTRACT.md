# AgentsWatch Run Evidence Foundation Contract

Last aligned: 2026-07-05  
Status: implemented on `feature/pr-evidence-run-foundation`; execution proof pending

## Purpose

Define the first usable vertical slice beneath PR Evidence and the Trust Ledger.

This slice answers only:

```text
Where did the run start?
Where did it finish?
Which Git paths changed?
Which changed paths were outside declared scope?
Was validation evidence captured?
```

It does not yet observe a coding agent directly and does not prove build, test, CI, UI, database, runtime, or behavioral outcomes.

## Commands

```bash
agentswatch start <task-id> [--title <text>] [--scope <glob>]...
agentswatch finish <task-id>
agentswatch report [task-id]
```

## Storage

Machine-readable sidecar:

```text
.agentwatch/runs/<task-id>.json
```

Human-readable report:

```text
.ai/runs/<task-id>.md
```

The JSON sidecar contains no absolute repository root. The CLI may print the local root during execution, but shareable artifacts must not expose it.

## Start contract

`agentswatch start`:

1. validates a path-safe task ID;
2. resolves the Git repository root;
3. refuses to overwrite an existing task manifest;
4. refuses a second active run;
5. reads branch, full Git object ID, and working-tree status;
6. ignores existing managed run artifacts when evaluating dirty state;
7. refuses other pre-existing dirty changes because they cannot be attributed to the new run;
8. records optional title and deduplicated allowed-scope globs;
9. writes JSON and Markdown atomically;
10. records `Validation: NotRun`.

Task ID grammar:

```text
1-100 characters
letters, digits, dot, underscore, hyphen
no slash, whitespace, or path traversal
```

Current managed run artifacts:

```text
.agentwatch/runs/*.json
.ai/runs/*.md
```

These artifacts do not block a later run and are not attributed to a coding agent.

Exit behavior:

| Condition | Exit |
|---|---:|
| Start succeeds | 0 |
| Invalid arguments/task ID | 2 |
| Existing task, active run, or non-AgentsWatch dirty state | 3 |
| Unexpected Git/filesystem failure | 1 |

## Finish contract

`agentswatch finish`:

1. requires an existing in-progress run;
2. refuses to rewrite a previously finished run;
3. reads the current branch, full Git object ID, and status;
4. compares tracked changes from the immutable start commit;
5. adds untracked paths from current status;
6. normalizes added, modified, deleted, type-changed, unmerged, renamed, copied, and untracked states;
7. removes managed run artifacts from change attribution;
8. evaluates declared-scope globs;
9. records branch changes and uncommitted non-AgentsWatch files as warnings;
10. records `Validation: NotRun` plus an explicit evidence-boundary warning;
11. atomically replaces the JSON sidecar and Markdown report.

An empty scope list means unrestricted scope. The report must say that out-of-scope evaluation was not applicable rather than claiming that no scope violation occurred.

## Report contract

`agentswatch report` with no ID prints the most recently started run.

`agentswatch report <task-id>` prints the selected run.

Stable report sections:

```text
identity and lifecycle
validation status
start/end branch and object IDs
allowed scope
changed files
outside declared scope
warnings
evidence boundary
```

Changed and out-of-scope paths are sorted deterministically.

The report must not:

- claim validation passed;
- infer agent activity from missing events;
- include absolute local repository paths;
- include source contents, diffs, terminal logs, or secrets;
- treat AgentsWatch-generated run artifacts as agent changes.

## Manifest schema v1.0

```text
schemaVersion
taskId
title
startedAt
finishedAt
status
validationStatus
startBranch
startCommitSha
endBranch
endCommitSha
allowedPaths
changedFiles
outOfScopeFiles
warnings
```

Lifecycle values:

```text
InProgress
Finished
```

Validation values reserved by the schema:

```text
NotRun
Pass
Fail
BlockedByEnvironment
Unknown
```

This implementation slice only writes `NotRun`. Other values require command, user-entered, CI, or adapter evidence in later slices.

## Security and integrity boundaries

- Start/end revisions must be full 40- or 64-character hexadecimal Git object IDs.
- Git revisions are not accepted as arbitrary command text.
- Writes use a temporary file followed by an atomic move/replace.
- Existing task manifests are not overwritten by `start`.
- Unknown manifest schema versions are rejected.
- Finished manifests require end time, end branch, and end object ID.
- No network access is required by the design.

These are source-level contracts until executed tests and black-box proof exist.

## Acceptance scenarios

Minimum scenarios before L3:

1. clean repository start;
2. non-AgentsWatch dirty repository refusal;
3. existing managed reports do not block start;
4. duplicate task refusal/no overwrite;
5. second active run refusal;
6. finish with committed change;
7. finish with uncommitted and untracked change;
8. added/modified/deleted/renamed parsing;
9. managed artifacts excluded;
10. out-of-scope path detected;
11. unrestricted scope reported as not evaluated;
12. branch change warning;
13. report latest/default behavior;
14. unknown schema rejection;
15. no absolute local root in JSON/Markdown;
16. Windows and Linux path/glob behavior;
17. `Validation: NotRun` never presented as pass.

## Next slice

The next vertical slice is explicit command evidence:

```bash
agentswatch run -- <command>
```

It must add:

- command identity/display after redaction;
- start/end timestamps and duration;
- exit code;
- stdout/stderr byte counts;
- compact, redacted summary;
- first useful error signature;
- working-directory identity without leaking unnecessary absolute paths;
- commit binding before and after command execution;
- timeout/cancellation state;
- linkage to an active run.

Only after command evidence exists should AgentsWatch classify validation claims or generate a meaningful PR Evidence Packet.
