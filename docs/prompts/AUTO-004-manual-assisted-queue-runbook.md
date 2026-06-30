# AUTO-004 — Manual/assisted queue runbook

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `AUTO-004`  
Run mode: docs/evidence  
Token budget: low  
Permission mode: docs_only  
Gate: after AUTO-001 queue design exists

## Purpose

Create a practical runbook for using AgentsWatch queue items with an external coding agent without continuous autopilot.

## Minimum read

- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- selected queue item list
- `docs/AGENT_PERMISSION_MODEL.md`

## Task

Create a manual or assisted runbook that tells the user how to execute queue items one by one.

Include:

1. how to select the next Ready item;
2. how to copy the prompt envelope into the chosen tool;
3. what evidence to collect;
4. how to mark item status;
5. when to stop;
6. when approval is required;
7. how to generate the next prompt.

## Output

```text
Runbook:
Next item rule:
Manual execution steps:
Evidence checklist:
Stop conditions:
Approval conditions:
Next prompt:
```

## Stop rules

Stop if:

- a queue item is not Ready;
- dependencies are missing;
- the chosen tool requires risky automation;
- the next step would need elevated approval.
