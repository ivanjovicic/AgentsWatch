# ROAD-001 — Roadmap to execution plan

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `ROAD-001`  
Run mode: investigation-only  
Token budget: medium  
Gate: roadmap planning only; no runtime implementation

## Purpose

Convert a rough roadmap into a gated execution plan without implementing code.

## Minimum read

- roadmap input file or pasted roadmap text;
- `docs/ROADMAP_DRIVEN_AGENT_OS.md`;
- `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md`.

## Task

Create a roadmap execution plan with:

1. epics;
2. milestones;
3. dependencies;
4. gates;
5. risk categories;
6. suggested prompt queue;
7. first safe prompt.

## Rules

- Do not implement code.
- Do not create dashboard/SaaS work unless roadmap gates allow it.
- Split vague items into investigation prompts first.
- Each generated work item must have one run mode.
- Prefer low/medium token budgets.
- Add human-review gates for security, migrations, billing, cloud, auth, and data-loss risk.

## Output

```text
Roadmap summary:
Epics:
Milestones:
Blocked items:
First safe prompt:
Prompt queue draft:
Risks:
Missing decisions:
```

## Stop rules

Stop if:

- roadmap is too vague to produce acceptance criteria;
- more than five docs are needed;
- the plan requires unsupported cloud/SaaS behavior;
- human product decisions are required before execution.
