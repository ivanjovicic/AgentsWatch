# ROAD-003 — Roadmap run self-review

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `ROAD-003`  
Run mode: review-only  
Token budget: low  
Gate: after an agent run has report/handoff evidence

## Purpose

Review one completed agent run against the roadmap item that caused it.

## Minimum read

- roadmap item;
- latest run report;
- latest handoff summary.

Optional:

- command profile summary if validation behavior is relevant;
- changed-file list if scope creep is suspected.

## Task

Decide whether the roadmap item should move forward, retry, split, or stop.

Review:

1. Did the run address the roadmap item?
2. Did the agent stay in owned paths?
3. Did it inspect too many files?
4. Did it run validation or record a blocked reason?
5. Did it avoid large logs and broad commands?
6. Did it miss tests, docs, or acceptance criteria?
7. What is the next minimal prompt?

## Output

```text
Roadmap item:
Run status: pass/retry/split/blocked
Scope result:
Validation result:
Missed work:
Next prompt:
Roadmap status update:
Risk:
```

## Stop rules

Stop and mark blocked if:

- validation evidence is missing for runtime changes;
- changed files do not match the roadmap item;
- security/data-loss/migration risk appears;
- next work requires product clarification.
