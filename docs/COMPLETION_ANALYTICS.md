# AgentsWatch Completion Analytics

Last aligned: 2026-06-29

## Purpose

Track how complete each prompt really is, what was missed, and which follow-up prompt should close the gap.

This prevents optimistic `Done` labels after partial agent work.

## Required fields

Every completed, blocked, or evidence-only prompt should record:

- completion percentage;
- commit SHA or evidence location;
- validation run;
- validation not run;
- missed work;
- follow-up prompt;
- residual risk.

## Scoring guide

| Score | Meaning |
|---:|---|
| 100 | complete and validated |
| 90 | complete, minor evidence gap |
| 75 | useful but missing important validation |
| 50 | partial implementation or docs-only evidence |
| 35 | evidence recorded, validation still missing |
| 20 | investigation only, no implementation |
| 0 | not started |

## AgentsWatch examples

### Gate 0 evidence-only

```text
AW-VAL-001: 35%
Reason: evidence file created, but restore/build/test and CLI smoke not run.
Follow-up: AW-VAL-001 local build validation.
Residual risk: .sln/project references may still fail.
```

### Init command hardening

```text
AW-002: 85%
Reason: init behavior implemented and tests added, but local tool install not checked.
Follow-up: packaging validation prompt.
Residual risk: global tool packaging unverified.
```

### Full command completion

```text
ROAD-005: 95%
Reason: report writing implemented, tests pass, docs updated.
Follow-up: none.
Residual risk: manual dogfood still needed.
```

## Analytics review questions

Ask after each larger run:

1. What did the prompt promise?
2. What actually changed?
3. What validation proved it?
4. What validation was skipped?
5. What user-visible or product risk remains?
6. What should be the next smallest prompt?

## Rule

If validation is missing, do not score above 79 unless the task was explicitly docs-only or evidence-only.
