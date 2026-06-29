# Prompt Optimization Playbook

Last aligned: 2026-06-29

Purpose: reduce wasted AI coding-agent tokens by shrinking task scope before a run, limiting file reads, splitting work into smaller modes, and replacing long chat history with compact handoff evidence.

## Expected savings

| Task type | Typical token-waste reduction |
|---|---:|
| Small one-file fix | 5-15% |
| Bug across 2-5 files | 20-40% |
| Frontend plus backend task | 30-50% |
| Repo/root-cause investigation | 40-70% |
| Oversized “analyze whole repo” prompt | 60-85% |
| Diff-only review | 30-60% |
| Long chat/history continuation | 40-80% |

Default product-safe claim: **reduce AI coding-agent token waste by 30-50% on typical multi-file tasks**.

## Pre-run prompt risk checker

High-risk signs:

- says “analyze the whole app/repo”;
- mixes investigation, implementation, tests, docs, and review in one run;
- lacks owned paths and avoid paths;
- lacks stop rules;
- asks to fix multiple unrelated features;
- includes long chat history instead of a compact handoff;
- does not name validation.

If two or more signs apply, split the prompt before running an agent.

## Token budget levels

| Budget | Use for | Limits |
|---|---|---|
| low | one bug, one widget, one service, investigation-only | inspect <= 8 files, edit <= 3 files, targeted validation only |
| medium | one feature slice or cross-file bug | inspect <= 15 files, edit <= 6 files, no full suite unless justified |
| high | audit/migration/release evidence | inspect as needed, summarize every 10 files and stop if scope expands |

## Scope limiter block

```text
Scope limiter:
Inspect only:
- <folder/file 1>
- <folder/file 2>

Do not inspect unless the first scan proves it is required:
- unrelated feature folders
- theme system
- migrations
- auth/session
- backend contracts

Do not edit:
- generated files
- unrelated providers/services
- unrelated screens
```

## Investigation-only mode

Use for uncertain bugs.

```text
Run mode: investigation-only
Allowed:
- search code
- read targeted files
- inspect existing tests
- report root cause and minimal fix plan

Forbidden:
- edit files
- refactor
- run broad test suites

Return:
1. root cause
2. exact files involved
3. minimal fix plan
4. tests to add/update
5. stop risks
```

## Split prompt pattern

Bad:

```text
Analyze the whole app and fix workers, logs, performance page, UI popups, and tests.
```

Good:

```text
001-investigate-workers-run-now.md
002-implement-workers-run-now-minimal-fix.md
003-add-workers-run-now-tests.md
004-review-workers-run-now-diff.md
```

## Diff-only review mode

```text
Run mode: diff-only review
Review only:
- changed files in commit <sha>
- listed diff hunks

Do not inspect whole repo unless a changed file references a missing symbol or contract.

Return:
1. blocking issues
2. missed tests
3. risky scope creep
4. follow-up prompt if needed
```

## Handoff summary

```text
Task:
Status:
Relevant files:
Files changed:
Root cause:
Validation run:
Validation blocked:
Do not inspect next:
Next minimal prompt:
Residual risk:
```

## Stop rules

Stop and report if:

- more than the budgeted file count is required;
- root cause is still unknown after the initial scan;
- task crosses repo boundaries unexpectedly;
- schema/auth/security/economy logic enters scope unexpectedly;
- tests fail for unrelated environment reasons twice;
- a broad refactor appears necessary.

## Token waste report

```text
Files inspected:
Files changed:
Repeated searches:
Broad commands avoided:
Tests run:
Environment retries:
Largest waste source:
Next token-saving improvement:
```
