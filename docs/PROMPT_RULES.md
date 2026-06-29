# AgentsWatch Prompt Rules

Last aligned: 2026-06-29

## Purpose

Make every agent prompt small, scoped, testable, and evidence-driven.

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
Changed files:
What changed:
Validation run:
Validation not run:
Commit SHA:
Completion %:
Missed:
Follow-up:
Residual risk:
Token optimization applied:
```
