# POST-RUN-001 — Retrospective and Prompt Improvement

Run mode: docs/evidence  
Token budget: low

## Purpose

Run this after every non-trivial agent prompt.

## Read first

- `AGENTS.md`
- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `docs/WASTE_LEARNING_LOOP.md`
- `docs/templates/AGENT_RUN_EVIDENCE_TEMPLATE.md`
- the prompt that was just executed
- files changed in the run

## Task

Create or update the run evidence entry for the completed prompt.

## Required output

Record:

- what was done;
- what was missed;
- validation run/not run;
- where time/tokens were wasted;
- why waste happened;
- which rule/doc/queue was updated;
- which optimized follow-up prompt was added;
- completion percentage;
- residual risk.

## Rules

- Do not hide skipped validation.
- Do not claim no waste unless the run was trivial and direct.
- If a docs/rule update is needed, make the smallest update.
- If a better next prompt exists, add it to the relevant queue or evidence entry.
- If no optimized prompt is needed, state why.

## Validation

Docs/evidence-only. No runtime validation required unless files changed in runtime code.

## Final response

Return:

```text
Evidence recorded:
Waste found:
Waste cause:
Rule/docs updated:
Optimized prompt added:
Commit SHA:
Completion %:
Residual risk:
```
