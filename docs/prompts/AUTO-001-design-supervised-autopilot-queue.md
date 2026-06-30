# AUTO-001 — Design supervised autopilot queue

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `AUTO-001`  
Run mode: investigation-only  
Token budget: low  
Permission mode: read_only  
Gate: docs/design only

## Purpose

Design the smallest safe autopilot queue contract for one project roadmap or prompt list.

## Minimum read

- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- `docs/AGENT_RISK_BOUNDARIES.md`
- `docs/AGENT_PERMISSION_MODEL.md`

## Task

Produce a queue design for a specific roadmap or prompt list.

Answer:

1. queue items;
2. dependencies;
3. permission modes;
4. approval gates;
5. validation gates;
6. model/tool recommendation per item;
7. stop conditions;
8. first safe queue item.

## Rules

- Do not implement code.
- Do not assume Codex/Cursor can accept an unbounded queue.
- Do not suggest UI automation.
- Do not bypass approval prompts.
- Block continuous autopilot for MVP.
- Prefer manual_queue or assisted_queue.

## Output

```text
Autopilot level:
Queue items:
First safe item:
Approval gates:
Blocked items:
Model/tool recommendations:
Validation plan:
Stop rules:
```

## Stop rules

Stop if:

- roadmap/prompt list is too vague;
- permission modes cannot be assigned;
- any item requires elevated risk approval;
- execution would require unofficial UI automation;
- external tool behavior is unknown.
