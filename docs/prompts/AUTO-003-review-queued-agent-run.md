# AUTO-003 — Review queued agent run

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `AUTO-003`  
Run mode: review-only  
Token budget: low  
Permission mode: read_only  
Gate: after one queued agent run returns evidence

## Purpose

Review one queued agent run and decide whether the queue may proceed.

## Minimum read

- queue item;
- agent final response or run report;
- changed-file summary;
- validation evidence or blocked reason.

Optional:

- `docs/AGENT_RISK_BOUNDARIES.md` if risk appears;
- command profile summary if validation behavior matters.

## Task

Decide whether to mark the queue item `Done`, `NeedsReview`, `NeedsApproval`, `Failed`, or `Blocked`.

Check:

1. Did the agent complete the queue item?
2. Did it stay inside owned paths?
3. Did it avoid sensitive paths?
4. Did it run validation or explain why blocked?
5. Did it avoid large logs and secrets?
6. Did it try to continue to another queue item without approval?
7. Is the next queue item safe to run?

## Output

```text
Queue item:
Decision:
Evidence:
Scope result:
Validation result:
Risk result:
Next queue status:
Next safe action:
```

## Stop rules

Stop and mark `NeedsApproval` or `Blocked` if:

- sensitive paths changed;
- validation failed;
- permission mode changed to elevated_risk;
- agent attempted a high-risk repository, release, or production operation;
- output may contain secrets;
- changed files exceed owned paths;
- next queue item is not clearly safe.
