# AgentsWatch CLI Command Contracts

Last aligned: 2026-06-29  
Status: product contract, not yet fully implemented

## Purpose

Define command behavior before implementation so agents do not invent different UX, file paths, or report formats.

This document is authoritative for CLI behavior after Gate 0 validation passes.

---

## Global command rules

All commands should:

- work from any repository root;
- use current directory as default project root;
- avoid network calls by default;
- avoid overwriting user files without an explicit flag;
- print concise output by default;
- return non-zero exit code on real failure;
- write markdown artifacts only when the command promises to write them;
- keep sensitive file contents out of reports.

Exit codes draft:

| Code | Meaning |
|---:|---|
| 0 | success |
| 1 | command failed |
| 2 | invalid arguments |
| 3 | validation failed |
| 4 | blocked by environment |
| 5 | unsafe operation refused |

---

## `agentswatch init`

Purpose: create local AgentsWatch workspace.

Creates:

```text
.ai/config.yml
.ai/tasks/
.ai/runs/
.ai/generated/
.ai/STATUS.md
.ai/CHANGELOG_AI.md
.ai/REVIEW_CHECKLIST.md
.agentwatch/
```

Rules:

- must be idempotent;
- must not overwrite existing files;
- should print what was created and what already existed;
- future `--force` must be explicit and tested.

Minimum tests:

- empty temp directory;
- existing `.ai/config.yml` is preserved;
- existing folders are handled;
- Windows/Unix path compatibility.

---

## `agentswatch status`

Purpose: show repository, project-type, validation, and git status summary.

Output should include:

```text
Project root:
Detected types:
Branch:
Commit:
Changed files:
Suggested validation:
Open risks:
Next safe prompt:
```

Rules:

- should handle clean git repo;
- should handle dirty git repo;
- should handle non-git directory with a clear message;
- should not run validation by default.

---

## `agentswatch optimize <prompt>`

Purpose: classify a rough prompt and return a safer prompt plan.

Output should include:

```text
Risk:
Budget:
Waste causes:
Suggested split:
Scope limiter:
Optimized prompt:
```

Rules:

- file path input and inline text input both supported;
- broad prompts should become high-risk;
- missing validation/stop/scope should be listed as waste causes;
- generated prompt must include run mode, token budget, scope limiter, stop rules, and validation.

---

## `agentswatch task split <prompt-file>`

Purpose: write scoped markdown task files from one broad prompt.

Default output:

```text
.ai/tasks/001-investigate-only.md
.ai/tasks/002-implement-minimal-fix.md
.ai/tasks/003-add-tests.md
.ai/tasks/004-diff-only-review.md
```

Rules:

- refuse to overwrite existing generated task files unless explicit flag is provided;
- print generated paths;
- include required prompt fields from `docs/PROMPT_RULES.md`.

---

## `agentswatch start <task-id>`

Purpose: record run start state.

Writes later:

```text
.ai/runs/<run-id>-start.md
```

Captures:

- task id;
- timestamp;
- branch;
- commit;
- current changed files;
- run mode and budget if known.

Rules:

- should refuse a second active run unless `--allow-overlap` exists later;
- should warn if worktree is already dirty.

---

## `agentswatch finish <task-id>`

Purpose: record run completion state and produce evidence.

Captures:

- end timestamp;
- changed files;
- validation entered by user or read from run notes;
- missed work;
- follow-up;
- residual risk.

Writes:

```text
.ai/runs/<run-id>.md
.ai/runs/<run-id>-handoff.md
```

---

## `agentswatch report`

Purpose: generate or reprint latest run report.

Rules:

- default to latest run;
- support `--task <id>` later;
- never claim validation passed unless evidence exists.

---

## `agentswatch handoff`

Purpose: generate compact continuation context.

Rules:

- target 10-20 lines;
- include relevant files, validation, missed work, next prompt, residual risk;
- no long chat history.

---

## `agentswatch review-diff <commit-or-range>`

Purpose: generate a diff-only review prompt.

Rules:

- review changed files only;
- include missed-test checklist;
- include claims-vs-actual checklist;
- forbid whole-repo review by default.

---

## `agentswatch validate`

Purpose: run or suggest configured validation commands.

MVP behavior:

- suggest commands by default;
- running commands should be explicit with `--run` later;
- record pass/fail/not-run honestly.

Do not implement broad automatic validation before the command safety design is validated.
