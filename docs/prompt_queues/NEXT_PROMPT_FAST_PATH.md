# AgentsWatch Next Prompt Fast Path

Last aligned: 2026-08-25

## Current next prompt

```text
AW-VFY-001 — Git parser and CI hardening
```

Prompt file:

```text
docs/prompts/AW-VFY-001-git-parser-ci-hardening.md
```

Queue:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

## Why this is next

Latest known CI evidence already proves:

```text
restore: PASS
build: PASS
tests: FAIL
```

The failure is known and focused: git porcelain status parsing corrupts a path because the current parser trims the leading status column before fixed-position slicing.

Do not spend another broad validation-only run rediscovering the same failure.

## Copy-ready instruction

```text
Repository: ivanjovicic/AgentsWatch
Prompt ID: AW-VFY-001
Queue: docs/prompt_queues/verification_mvp_2026_08_25.md
Run mode: implementation + validation
Budget: low/medium

Read:
- AGENTS.md
- docs/BOOTSTRAP_NEXT_STEPS.md
- docs/ARCHITECTURE.md (Git evidence section)
- docs/prompts/AW-VFY-001-git-parser-ci-hardening.md
- exact Git parser/test files named by the prompt

Task:
Execute AW-VFY-001 exactly. Fix the known parser defect with a lossless machine-safe git porcelain contract, add focused edge-case tests, then run the full restore/build/test gate.

Do not add product features.
Do not expand into Contract/Receipt work.
Do not mark Done unless the complete test gate passes.

Return:
- root cause confirmation or changed finding
- exact implementation
- tests added/updated
- full validation results
- files changed
- remaining risk
- queue status / next prompt
- commit SHA
```

## Next after success

If AW-VFY-001 is complete and green:

```text
AW-VFY-002 — CLI smoke and Gate 0 closure
```

Then follow the strict order in `verification_mvp_2026_08_25.md`.
