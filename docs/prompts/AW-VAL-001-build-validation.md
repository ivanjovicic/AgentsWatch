# AW-VAL-001 — Build validation

Run mode: validation-only  
Token budget: low

## Read first

- `AGENTS.md`
- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/ARCHITECTURE.md`

## Task

Verify the current skeleton builds and tests. Do not add features.

## Allowed

- run restore/build/test commands from `docs/BUILD_VALIDATION_PLAN.md`;
- inspect only files needed to understand failures;
- make the smallest build/test fix if validation fails.

## Forbidden

- new CLI commands;
- new product features;
- dashboard work;
- SaaS/cloud work;
- broad refactor.

## Return

1. validation results;
2. files changed;
3. remaining bootstrap risks;
4. next prompt.
