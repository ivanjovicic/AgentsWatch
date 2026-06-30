# AgentsWatch Continuous Improvement Prompt Queue

Last aligned: 2026-06-29

## Purpose

Make every agent run improve the next one.

This queue is not blocked by Gate 0 because it is docs/evidence-only, but it must not be used to sneak in feature work.

## Read first

- `../../AGENTS.md`
- `../AGENT_RUN_EVIDENCE_STANDARD.md`
- `../WASTE_LEARNING_LOOP.md`
- `../templates/AGENT_RUN_EVIDENCE_TEMPLATE.md`

## Rules

- Run POST-RUN-001 after every non-trivial prompt.
- Record what was done and missed.
- Record where time/tokens were wasted and why.
- Update rules/docs/queues when waste should not repeat.
- Add a new optimized prompt when analysis reveals a better next step.
- Do not claim completion without evidence.

## Active prompts

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| POST-RUN-001 | Always Ready | `../prompts/POST-RUN-001-retrospective-and-prompt-improvement.md` | Record evidence, waste, rule updates, and optimized follow-up prompt. |
| CI-001 | Ready | inline below | Review last 5 evidence entries and find repeating waste patterns. |
| CI-002 | Ready | inline below | Convert repeated waste pattern into a new mandatory rule. |
| CI-003 | Ready | inline below | Optimize a stale queue after evidence shows wrong prompt ordering. |

## CI-001 — Review last 5 evidence entries

Run mode: docs/evidence  
Token budget: low

Task: review the last 5 run evidence entries and identify repeated waste categories.

Return:

- repeated waste categories;
- affected prompts;
- root cause;
- recommended rule update;
- optimized queue change.

## CI-002 — Convert repeated waste into rule

Run mode: docs/evidence  
Token budget: low

Task: update the smallest relevant rule document so a repeated waste pattern is less likely to happen again.

Owned docs:

- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `docs/WASTE_LEARNING_LOOP.md`
- `docs/AGENT_COMMAND_PLAYBOOK.md`
- `docs/PROMPT_RULES.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`

## CI-003 — Optimize stale queue

Run mode: docs/evidence  
Token budget: low

Task: update a queue when evidence proves its ordering or status misleads agents.

Required:

- cite evidence file;
- change only queue status/order;
- add optimized next prompt;
- record residual risk.
