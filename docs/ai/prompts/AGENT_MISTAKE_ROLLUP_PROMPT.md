# Agent Mistake Rollup Prompt

Use this after 5 meaningful AgentsWatch run logs, 3 repeated mistake categories, or one high-severity evidence/release mistake.

```text
Use only this repository:
ivanjovicic/AgentsWatch

Prompt ID:
AW-MISTAKE-ROLLUP-001

Run mode:
docs/evidence

Token budget:
low-medium

Goal:
Review recent `.ai/runs` logs and update the AgentsWatch mistake-learning system so repeated mistakes become prevention rules, prompts, tests, or lint checks.

Read first:
- docs/AGENT_SHARED_OPERATING_STANDARD.md
- docs/AGENT_RUN_LOG_ENFORCEMENT.md
- docs/ai/learning/MISTAKE_LEDGER.md
- docs/ai/learning/MISTAKE_CARD_TEMPLATE.md
- .ai/runs/README.md
- docs/WASTE_LEARNING_LOOP.md
- docs/prompt_queues/PROMPT_QUEUE_ROUTER.md

Inspect only:
- last 5-8 `.ai/runs/*-evidence.md` logs;
- queue rows referenced by those logs;
- existing mistake cards that match repeated categories.

Required work:
1. List runs reviewed.
2. Identify repeated mistakes and waste categories.
3. Add or update mistake cards.
4. Make exactly one prevention change: rule, prompt, queue, lint prompt, or test prompt.
5. Add a compact rollup note under `docs/ai/learning/` if useful.
6. Do not edit runtime code.

Validation:
- git diff --check
- verify every referenced run-log path exists
- verify every repeated mistake has a prevention action or documented no-op reason

Final response:
- runs reviewed
- mistake IDs updated
- prevention change made
- validation
- residual risk
- commit SHA
```
