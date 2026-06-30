# AgentsWatch Prompt Token Economy Quick Rules

Last aligned: 2026-06-30  
Status: default short rule set

## Purpose

Use this file as the default anti-waste rule set for normal agent runs.

Use the full `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` only when writing, changing, or auditing prompt rules.

## Default rules

1. One task per run.
2. One primary run mode per run.
3. Read the smallest useful context first.
4. Do not read docs because they are interesting.
5. Start with the owning prompt and one relevant contract doc.
6. Use low budget unless the prompt proves medium is needed.
7. Prefer investigation-only for uncertain design or root-cause work.
8. Prefer targeted validation before full validation.
9. Do not paste full terminal logs into prompts or reports.
10. Stop when the task crosses into another subsystem.

## Context budget

```text
Low budget: read up to 3 docs before first action
Medium budget: read up to 5 docs before first action
High budget: read up to 8 docs before first action
```

Count only docs or source files inspected for context. Do not count the current prompt text.

If more context is required, stop and explain why before reading more.

## Default read set

For most tasks:

```text
1. owning prompt or queue section
2. one relevant contract doc
3. one implementation file or one focused design doc
```

Add optional docs only when the task needs them:

- security/privacy doc: command output, secrets, network, or storage risks;
- data model doc: JSON/SQLite/storage shape changes;
- report formats doc: markdown/report/handoff changes;
- product/roadmap docs: positioning or priority changes;
- full rulebook/checklist: prompt-system changes.

## Compact final response

For low-budget runs, use:

```text
Prompt ID:
Files changed:
Validation:
Result:
Missed:
Next:
Risk:
```

For medium/high-budget runs, use the full final response format from the full rulebook.

## Command-output rule

```text
Command profile summary beats command log paste.
```

Record duration, exit code, byte counts, and compact error signatures. Do not include full stdout/stderr unless a later explicit debug/export workflow requires it.
