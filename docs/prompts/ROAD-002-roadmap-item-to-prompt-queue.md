# ROAD-002 — Roadmap item to prompt queue

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `ROAD-002`  
Run mode: docs/evidence  
Token budget: low  
Gate: after ROAD-001 execution plan exists

## Purpose

Turn one roadmap item or epic into a safe prompt queue.

## Minimum read

- selected roadmap item;
- `docs/ROADMAP_DRIVEN_AGENT_OS.md`;
- one relevant contract doc.

## Task

Generate a prompt queue for one roadmap item.

Each prompt must include:

- prompt ID;
- one run mode;
- token budget;
- gate;
- owned paths;
- avoid paths;
- validation or blocked reason;
- stop rules;
- final response shape.

## Required split pattern

Use this order unless the roadmap item proves a smaller split is enough:

```text
001-investigate
002-implement-smallest-slice
003-add-targeted-tests
004-run-validation
005-diff-only-review
006-record-evidence
```

## Output

```text
Epic:
Generated prompts:
First safe prompt:
Validation plan:
Human-review gates:
Risks:
```

## Stop rules

Stop if:

- the roadmap item has no acceptance criteria;
- owned paths cannot be identified;
- validation cannot be named;
- the item crosses more than one subsystem and needs a split.
