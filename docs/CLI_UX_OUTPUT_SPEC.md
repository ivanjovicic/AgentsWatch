# AgentsWatch CLI UX Output Spec

Last aligned: 2026-07-05  
Status: current and planned UX contract

## Purpose

AgentsWatch output must be concise, evidence-first, testable, and safe to copy into a review or handoff.

## Tone and truth rules

Use:

- short stable labels;
- exact evidence identities;
- explicit unknown/NotRun states;
- clear next action;
- relative paths in reports.

Avoid:

- vague `done` or `all good` messages;
- claiming validation passed without evidence;
- treating missing observation as contradiction;
- dumping full diffs/logs/source;
- printing secrets;
- embedding absolute local roots in shareable artifacts.

Tests should assert stable labels and important values, not entire prose paragraphs.

## `--help`

Current shape:

```text
AgentsWatch — local evidence and control layer for AI coding-agent work

Usage:
  agentswatch init
  agentswatch optimize <prompt text or prompt file>
  agentswatch status
  agentswatch start <task-id> [--title <text>] [--scope <glob>]...
  agentswatch finish <task-id>
  agentswatch report [task-id]
  agentswatch --version

Current run-evidence boundary:
  ...

Planned:
  ...
```

Rules:

- current commands appear before planned commands;
- help states evidence limitations;
- no planned command is presented as shipped without a `Planned` section.

## `init`

Current anchor:

```text
AgentsWatch initialized.
```

Target richer output later:

```text
AgentsWatch initialized

Created:
- <paths>

Preserved:
- <paths>

Next:
- agentswatch status
```

No-overwrite behavior must be proven before richer success claims.

## `status`

Target shape:

```text
AgentsWatch status

Project root: <local path>
Detected types: <types>
Branch: <branch>
Commit: <object id>
Changed files: <count>
Risk: <level>

Suggested validation:
- <command>

Next safe prompt:
- <action>
```

If no Git repository:

```text
Git: not detected
Note: run inside a Git repository for diff evidence.
```

The current implementation has a smaller status output; maturity/claims follow the registry.

## `optimize`

Stable concepts:

```text
Risk:
Budget:
Waste causes:
Suggested split:
Optimized prompt:
```

Future wording changes should retain machine/test anchors or version the contract.

## `start`

Current success output:

```text
Run started
Task: <task-id>
Repository: <local root shown only in terminal>
Base commit: <full object id>
Declared scope: <patterns or unrestricted>
Manifest: <local path>
Run report: <local path>
Validation: NotRun
```

Failure anchors:

```text
Run '<id>' already exists.
Another run is already active.
Cannot start run evidence from a dirty working tree.
```

Rules:

- dirty output lists only non-AgentsWatch paths that block attribution;
- no artifacts created on refusal;
- local paths may be printed locally but not written into shareable artifacts;
- `Validation: NotRun` required.

## `finish`

Current success output:

```text
Run recorded
Task: <task-id>
Run report: <path>
Start commit: <object id>
End commit: <object id>
Changed files: <count>
Outside declared scope: <count>
Validation: NotRun
Evidence boundary: build, test, CI, runtime, and agent-claim evidence are not captured yet.
```

Failure anchors:

```text
Run '<id>' does not exist.
Run '<id>' is already finished.
```

Rules:

- `Run recorded` means Git/scope evidence was closed, not task correctness;
- never print `Validation: Pass` in this slice;
- out-of-scope count depends on a declared scope;
- managed report files are not included in changed-file count.

## `report`

Behavior:

```text
agentswatch report           # latest started run
agentswatch report <task-id> # selected run
```

Report starts with:

```text
# AgentsWatch Run Evidence — <task-id>
```

Required labels:

```text
Title:
Status:
Validation:
Started:
Finished:
Start branch:
Start commit:
End branch:
End commit:
Allowed scope
Changed files
Outside declared scope
Warnings
Evidence boundary
```

Missing data uses explicit placeholders such as `not finished` or `not captured`.

## Future `run --`

Target concise completion shape:

```text
Command recorded
Run: <task-id>
Command: <redacted display>
Status: <Pass|Fail|TimedOut|Cancelled|Refused|Unknown>
Exit code: <code or unavailable>
Duration: <duration>
Start commit: <object id>
End commit: <object id>
Output: <compact redacted summary>
```

Do not print/persist secrets or full output by default.

## Error output

Preferred shape:

```text
error: <clear summary>

Reason:
  <evidence/lifecycle/environment reason>

Next:
  <small safe action>
```

Unexpected exceptions currently use `error: <message>` and exit 1. Known lifecycle/argument cases should be handled explicitly and use their documented exit codes.

## Testable anchors

Current important anchors:

- `AgentsWatch — local evidence and control layer`;
- `Run started`;
- `Run recorded`;
- `Task:`;
- `Base commit:`;
- `Start commit:`;
- `End commit:`;
- `Changed files:`;
- `Outside declared scope:`;
- `Validation: NotRun`;
- `Evidence boundary:`;
- `Another run is already active`;
- `Cannot start run evidence from a dirty working tree`.

Tests should additionally assert exit codes, file creation/checksums, and forbidden side effects; stable labels alone do not prove behavior.
