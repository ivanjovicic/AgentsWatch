# AgentsWatch Bootstrap Next Steps

Last aligned: 2026-06-29

## Current status

The repository has an initial .NET CLI skeleton, docs, tests, and CI workflow. The skeleton was created through GitHub file writes and must be validated before new runtime feature work.

## Required next order

Run in this order:

1. `AW-VAL-001` — build validation.
2. `AW-VAL-002` — CLI smoke validation.
3. `AW-VAL-003` — validation evidence review.
4. `AW-VAL-004` — init command hardening.
5. Continue with `AW-002+` from `docs/prompt_queues/agentwatch_mvp.md`.

## Temporary block

Treat these MVP prompts as blocked until build and CLI smoke evidence exists:

- `AW-002`;
- `AW-003`;
- `AW-004`;
- `AW-005`;
- `AW-006`;
- `AW-007`;
- `AW-008`;
- `AW-009`;
- `AW-010`.

## Why

The first risk is not product design. The first risk is whether the generated solution, project references, tests, CLI command dispatch, and CI workflow are valid.

## Minimal next prompt

```text
Use docs/prompts/AW-VAL-001-build-validation.md.
Do not add features.
Run restore/build/test.
Fix only build or test failures.
Return validation evidence and remaining risk.
```
