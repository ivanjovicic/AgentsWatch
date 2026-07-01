# AgentsWatch Waste Learning Loop

Last aligned: 2026-07-01

## Purpose

Turn every wasted step into a better rule, better prompt, better queue decision, or mistake-ledger prevention card.

If the same mistake happens twice, the documentation, prompt, queue, test, or lint system is failing.

Use this with:

- `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`
- `docs/ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md`
- `docs/ai/prompts/RUN_LOG_EVIDENCE_LINT_PROMPT.md`

## Loop

```text
1. Run prompt.
2. Record `.ai/runs` evidence.
3. Identify waste.
4. Classify observed mistakes.
5. Find root cause.
6. Update rule/doc/queue/test/lint or mistake ledger.
7. Add optimized prompt when useful.
8. Mark follow-up.
9. Reuse learning in next run.
```

## What counts as waste

Waste is any action that used time/tokens without moving the task closer to safe completion.

Common examples:

- reading broad docs when a context index existed;
- starting feature work before Gate 0;
- retrying a blocked write with the same shape;
- asking for whole-repo analysis when diff-only review was enough;
- running a command after an earlier required command failed;
- claiming validation without evidence;
- editing docs but not updating `DOCS_INDEX.md`;
- adding a plan without a runnable next prompt;
- marking Done without `.ai/runs` evidence;
- recording a repeated mistake without updating prevention.

## Root cause categories

Use one:

- prompt missing scope;
- prompt missing stop rule;
- prompt missing validation;
- queue status misleading;
- docs reference stale;
- command environment unknown;
- tool limitation not recognized;
- output too large;
- no evidence template used;
- no router consulted;
- no mistake ledger consulted;
- docs-only work overclaimed.

## Required remediation

For every major waste item, choose one:

| Waste cause | Required remediation |
|---|---|
| wrong queue selected | update router or queue status |
| stale docs reference | fix reference and update docs governance if needed |
| repeated blocked write | split into smaller files or record stop rule |
| missing validation | update prompt with validation command |
| broad scope | add scope limiter or split prompt |
| missed follow-up | add optimized follow-up prompt |
| missing evidence | add `.ai/runs` evidence entry and template reference |
| unclassified mistake | update `docs/ai/learning/MISTAKE_LEDGER.md` or mark false alarm with reason |
| repeated mistake | update rule/prompt/test/queue/lint or document no-op reason |
| docs-only overclaim | downgrade status and add evidence lint follow-up |

## Mistake ledger handoff

If a waste item is also a repeated agent mistake, use an `AW-MISTAKE-*` card from `docs/ai/learning/MISTAKE_LEDGER.md`.

If no matching card exists, add one using `docs/ai/learning/MISTAKE_CARD_TEMPLATE.md`.

Every affected run log should say:

```text
Relevant prior mistakes read:
Mistakes observed:
Prevention added:
```

## Optimized prompt generation

A good optimized follow-up prompt should be shorter and more specific than the original.

It must name:

- exact queue;
- exact run mode;
- exact owned paths;
- what not to inspect;
- what not to edit;
- validation command;
- evidence output;
- relevant prior mistake IDs.

## Review cadence

After every 5 completed runs, review evidence entries and ask:

1. Which waste category repeats?
2. Which mistake ID repeats?
3. Which prompt caused the most wasted work?
4. Which docs were missing or stale?
5. Which command failed most often?
6. Which rule should become mandatory?

Use `docs/ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md` when the same mistake or waste category repeats.

## Product implication

This loop is not only development hygiene. It is a core AgentsWatch product feature.

Future CLI commands should support it directly:

```text
agentswatch finish --retrospective
agentswatch waste report
agentswatch prompt improve
agentswatch mistakes list
agentswatch mistakes check
agentswatch lint evidence
```
