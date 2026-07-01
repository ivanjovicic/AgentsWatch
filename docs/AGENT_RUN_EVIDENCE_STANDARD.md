# AgentsWatch Agent Run Evidence Standard

Last aligned: 2026-07-01

## Purpose

Every agent run must leave evidence that can improve the next run.

AgentsWatch should not only track code changes. It should track:

- what was done;
- what was missed;
- where time/tokens were wasted;
- why waste happened;
- which prior mistakes were considered;
- what mistake was observed or avoided;
- what rule should prevent repetition;
- what optimized prompt should be used next.

## Mandatory rule

Every non-trivial prompt must produce a compact run evidence file before it is considered complete.

This applies to:

- validation prompts;
- docs prompts;
- implementation prompts;
- tests prompts;
- review prompts;
- dogfood prompts;
- blocked prompts;
- evidence backfills.

Also follow:

- `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `.ai/runs/README.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`

## Evidence location

Canonical path:

```text
.ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
```

Use `.ai/RUN_LOG_TEMPLATE.md`.

Older broad docs such as `docs/VALIDATION_EVIDENCE_<date>.md`, `docs/*_AUDIT_<date>.md`, and `docs/*_EXPANSION_<date>.md` may remain as historical evidence, but new non-trivial runs should use `.ai/runs/` and link any broad evidence from the compact run log.

## Required fields

Each evidence entry must include:

```text
Prompt ID:
Queue:
Agent/tool:
Model provider:
Model name/id:
Model mode/settings:
Client/IDE:
Run mode:
Token budget:
Started from:
Relevant prior mistakes read:
How this run avoids prior mistakes:
Elapsed time:
Phase time breakdown:
Files inspected:
Files changed:
What was done:
What was missed:
Validation run:
Validation not run:
Waste categories:
Mistakes observed:
Where time/tokens were wasted:
Why waste happened:
Rules/docs updated to prevent repeat:
New optimized prompt added:
Follow-up prompt:
Completion %:
Residual risk:
Commit SHA:
```

Use required placeholders:

```text
unknown-not-exposed
unknown-not-recorded
not run - <reason>
none
```

Do not guess model names, timings, validation status, or CI status.

## Waste categories

Use one or more:

- wrong queue selected;
- missing Gate 0 check;
- stale docs reference;
- broad prompt;
- unclear owned paths;
- missing avoid paths;
- missing validation command;
- repeated file reads;
- repeated failed patch;
- tool limitation;
- environment blocker;
- over-large file write blocked;
- unsupported CI evidence;
- runtime validation unavailable;
- docs-only work overclaimed;
- missing run-log path;
- unclassified mistake.

## Mistake-learning rule

A run is not learning-complete until every observed mistake is classified as:

```text
new mistake with a mistake card
repeated mistake with a rule/prompt/test/queue/lint update
false alarm with explanation
```

Before starting, read `docs/ai/learning/MISTAKE_LEDGER.md` and choose only relevant IDs.

Before marking Done, update one of:

1. existing docs rule;
2. relevant playbook;
3. prompt queue;
4. optimized prompt;
5. test or lint prompt;
6. mistake ledger card;
7. documented no-op reason.

## Optimized prompt rule

If the run discovers a better next step, write a copy-ready optimized prompt in one of:

```text
docs/ai/prompts/
docs/prompt_queues/
docs/*_AUDIT_<date>.md
```

The optimized prompt must include:

- repository;
- prompt id;
- queue;
- run mode;
- token budget;
- read-first docs;
- scope limiter;
- owned paths;
- avoid paths;
- stop rules;
- validation;
- final evidence format.

## Completion rule

A prompt cannot be scored above 79% if it did not record:

- validation run or why validation did not run;
- missed work;
- time/token waste;
- follow-up prompt;
- residual risk;
- relevant prior mistakes read;
- mistakes observed or `none`.

Use stricter caps from `docs/AGENT_RUN_LOG_ENFORCEMENT.md` when evidence is weaker.

## Final response rule

Every final response should include:

```text
Run log:
Mistake IDs:
Evidence recorded:
Waste found:
Rule/docs updated:
Optimized prompt added:
Validation:
Commit SHA:
Completion %:
Residual risk:
Next prompt:
```
