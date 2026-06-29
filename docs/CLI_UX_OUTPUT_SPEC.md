# AgentsWatch CLI UX Output Spec

Last aligned: 2026-06-29  
Status: draft UX contract

## Purpose

Define what the CLI should print so users can trust it and agents can test it.

AgentsWatch output must be concise, evidence-first, and copy-paste friendly.

---

## Tone rules

Use:

- short labels;
- clear risk reasons;
- file paths in backticks when shown in markdown;
- no exaggerated claims;
- no hidden success states.

Avoid:

- noisy logs by default;
- vague “done” messages;
- claiming validation passed without evidence;
- dumping full diffs unless explicitly requested;
- printing secret values.

---

## `--help`

Expected shape:

```text
AgentsWatch — AI coding-agent supervisor and token optimizer

Usage:
  agentswatch <command> [options]

Commands:
  init          Create .ai and .agentwatch workspace files
  status        Show repo, git, project-type, and validation summary
  optimize      Classify and improve a rough prompt
  task split    Split a broad prompt into scoped task prompts
  start         Record run start evidence
  finish        Record run completion evidence
  report        Show or write run report
  handoff       Generate compact continuation summary
  review-diff   Generate diff-only review prompt
  validate      Suggest or run validation commands
```

---

## `status` output

Example:

```text
AgentsWatch status

Project root: /repo
Detected types: dotnet, react
Branch: main
Commit: abc1234
Changed files: 3
Risk: Medium

Suggested validation:
- dotnet build
- dotnet test

Next safe prompt:
- AW-VAL-001 build validation
```

If not a git repo:

```text
AgentsWatch status

Project root: /folder
Git: not detected
Detected types: dotnet
Changed files: unknown
Risk: Medium

Note: run inside a git repository for diff evidence.
```

---

## `optimize` output

Example:

```text
Original prompt risk: HIGH
Recommended budget: low

Estimated waste causes:
- broad scope
- multiple task modes
- missing validation
- missing stop rules

Suggested split:
1. 001-investigate-only.md
2. 002-implement-minimal-fix.md
3. 003-add-tests.md
4. 004-diff-only-review.md

Optimized prompt:
<copy-paste prompt>
```

---

## `init` output

Example:

```text
AgentsWatch initialized

Created:
- .ai/tasks/
- .ai/runs/
- .ai/generated/
- .agentwatch/

Preserved:
- .ai/config.yml already existed

Next:
- agentswatch status
```

---

## `finish` output

Example:

```text
Run recorded

Task: AW-002
Run report: .ai/runs/2026-06-29-001-AW-002.md
Handoff: .ai/runs/2026-06-29-001-AW-002-handoff.md
Risk: Medium
Validation: not run

Follow-up:
- run dotnet test --filter Init
```

---

## Error output

Example:

```text
error: validation command failed

Command:
  dotnet test AgentsWatch.sln

Likely cause:
  test failure or build failure

Next:
  inspect the first failing test and fix only the owned area
```

---

## Testable UX rules

Tests should assert stable labels, not exact long prose.

Good test anchors:

- `Original prompt risk:`
- `Suggested split:`
- `AgentsWatch initialized`
- `Validation:`
- `Risk:`
- `Next:`

Avoid brittle tests for exact paragraphs.
