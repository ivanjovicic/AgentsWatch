# AW-VAL-002 — CLI smoke validation

Run mode: validation-only  
Token budget: low

## Prerequisite

`AW-VAL-001` passed or remaining build failures are documented.

## Read first

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/CLI_SPEC.md`

## Task

Run CLI smoke checks after build/test pass.

## Smoke checks

- help output;
- version output;
- optimize output for a broad prompt;
- status output inside a git repo.

## Rules

Fix only smoke-check failures. Do not add new commands or new product features.

## Return

1. smoke results;
2. minimal fixes;
3. remaining CLI risks;
4. next prompt.
