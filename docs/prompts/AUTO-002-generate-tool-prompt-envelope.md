# AUTO-002 — Generate tool prompt envelope

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `AUTO-002`  
Run mode: docs/evidence  
Token budget: low  
Permission mode: docs_only  
Gate: after AUTO-001 queue design exists

## Purpose

Convert one Ready queue item into a copy-ready prompt envelope for Codex, Cursor, Claude Code, Copilot, or another coding agent.

## Minimum read

- selected queue item;
- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`;
- `docs/AGENT_RISK_BOUNDARIES.md`.

## Task

Generate one tool-targeted prompt envelope.

The prompt must include:

```text
AgentsWatch queue item:
Permission mode:
Run mode:
Token budget:
Scope:
Owned paths:
Avoid paths:
Validation:
Stop rules:
Approval required:
Final response format:
```

## Rules

- Generate exactly one prompt envelope.
- Do not include hidden instructions.
- Do not include secrets.
- Do not ask the tool to continue with the next prompt automatically.
- Do not allow deployment, merge, release, force push, or production changes.
- If the item is elevated risk, output an approval request instead of a prompt.

## Output

```text
Tool target:
Prompt envelope:
Manual run instructions:
Expected evidence:
Stop conditions:
```

## Stop rules

Stop if:

- queue item is not Ready;
- dependencies are not Done;
- permission mode is elevated_risk or blocked;
- prompt would require external upload of secrets/logs/source outside an approved tool;
- validation is missing.
