# AgentsWatch Prompt Evidence Template

Last aligned: 2026-06-29

Use this template when closing a prompt, recording validation, or writing a run report.

## Completion row

```text
Done <percent>% (YYYY-MM-DD, commit <short-sha> on main)
Tests: <commands and result>
Missed: <what was not completed>
Follow-up: <prompt id or none>
Residual risk: <risk>
```

## Blocked row

```text
Blocked <percent>% (YYYY-MM-DD, commit <short-sha or none>)
Reason: <blocker>
Validation: <what ran or could not run>
Missed: <unfinished work>
Follow-up: <next prompt>
Residual risk: <risk>
```

## Validation evidence

```text
Validation run:
- <command>: pass/fail/not run

Validation not run:
- <command>: <reason>
```

## Final response block

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

## Percentage guide

| Percent | Meaning |
|---:|---|
| 95-100 | complete, validated, low residual risk |
| 80-94 | complete enough, minor validation/docs gaps |
| 60-79 | landed but missing important validation/evidence |
| 40-59 | partial, useful but not safe to call Done |
| 0-39 | blocked, investigation, or evidence-only |

## Rule

Do not hide unfinished work in chat. Record it under `Missed`, add a `Follow-up`, and state `Residual risk`.
