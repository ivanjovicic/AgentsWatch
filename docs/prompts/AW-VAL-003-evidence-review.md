# AW-VAL-003 — Validation evidence review

Run mode: review-only  
Token budget: low

## Task

Review validation evidence for the current `main` branch.

## Read first

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`

## Scope

Inspect only validation output, build notes, workflow result summaries, and files directly related to a validation failure.

## Return

1. validation status;
2. failing area if any;
3. smallest next fix;
4. whether runtime feature work is safe to continue;
5. residual bootstrap risk.
