# AgentsWatch Prompt Rules

Last aligned: 2026-06-29

## Purpose

Make every agent prompt small, scoped, testable, and evidence-driven.

For strict anti-waste enforcement, use:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`

## Required sections

Every non-trivial prompt must include:

- repository;
- prompt ID and queue;
- run mode;
- token budget;
- one clear task;
- read-first docs;
- scope limiter;
- owned paths;
- avoid paths;
- non-goals;
- stop rules;
- validation commands;
- final response format.

## Required header

```text
Repository:
Prompt ID:
Queue:
Run mode:
Token budget:
Gate:
Owned paths:
Avoid paths:
Validation:
Stop rules:
```

If the header is missing, rewrite the prompt before running it.

## Run modes

Use one:

```text
validation-only
investigation-only
implementation
tests
docs/evidence
review-only
diff-only review
```

Split the task if it needs more than one run mode.

## Token budgets

Use the strict limits from `PROMPT_TOKEN_ECONOMY_RULEBOOK.md`.

Default:

- low: inspect up to 8 files, edit up to 3 files;
- medium: inspect up to 15 files, edit up to 6 files;
- high: planning/audit only unless implementation scope is explicit.

## Scope limiter

```text
Inspect only:
- <path>

Do not inspect unless required:
- <path>

Do not edit:
- <path>
```

## Bootstrap rule

Until Gate 0 is complete, prompts should focus on:

- build validation;
- CLI smoke validation;
- evidence review;
- risk register update;
- init hardening after validation evidence exists.

Do not start dashboard, SaaS, GitHub integration, billing, or broad feature work before the gates allow it.

## Red flags

Avoid prompts that say:

- analyze the whole repo;
- fix everything;
- make it production-ready without gates;
- validation optional;
- mark Done without evidence;
- review whole repo when diff-only review is enough;
- continue huge chat history instead of a handoff;
- mix investigation, implementation, tests, docs, and review in one run.

## Final response format

```text
Prompt ID:
Run mode:
Token budget used:
Files inspected:
Files changed:
Validation run:
Validation not run:
Commit SHA:
Completion %:
Missed:
Follow-up:
Residual risk:
Token waste avoided:
```
