# Investigation-only Prompt Template

Repository:

Run mode: investigation-only  
Token budget: low

## Task

<Describe the problem.>

## Scope limiter

Inspect only:

- <path>

Do not inspect unless required:

- unrelated features
- auth/session
- migrations

Do not edit:

- generated files
- unrelated providers/services

## Allowed

- search code;
- read targeted files;
- inspect existing tests;
- report root cause and minimal fix plan.

## Forbidden

- edit files;
- refactor;
- run broad test suites;
- mark task as Done.

## Return

1. root cause;
2. exact files involved;
3. minimal fix plan;
4. tests to add/update;
5. stop risks.
