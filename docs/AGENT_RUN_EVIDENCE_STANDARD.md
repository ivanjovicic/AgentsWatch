# AgentsWatch Agent Run Evidence Standard

Last aligned: 2026-06-29

## Purpose

Every agent run must leave evidence that can improve the next run.

AgentsWatch should not only track code changes. It should track:

- what was done;
- what was missed;
- where time/tokens were wasted;
- why waste happened;
- what rule should prevent repetition;
- what optimized prompt should be used next.

## Mandatory rule

Every non-trivial prompt must produce a run evidence entry before it is considered complete.

This applies to:

- validation prompts;
- docs prompts;
- implementation prompts;
- tests prompts;
- review prompts;
- dogfood prompts;
- blocked prompts.

## Evidence location

Preferred future path:

```text
.ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
```

Until the CLI writes that automatically, docs/evidence can be recorded in:

```text
docs/VALIDATION_EVIDENCE_<date>.md
docs/*_AUDIT_<date>.md
docs/*_EXPANSION_<date>.md
```

## Required fields

Each evidence entry must include:

```text
Prompt ID:
Queue:
Run mode:
Token budget:
Started from:
Files inspected:
Files changed:
What was done:
What was missed:
Validation run:
Validation not run:
Where time/tokens were wasted:
Why waste happened:
Rules/docs updated to prevent repeat:
New optimized prompt added:
Follow-up prompt:
Completion %:
Residual risk:
Commit SHA:
```

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
- runtime validation unavailable.

## Prevention rule

For every meaningful waste category, the agent must do at least one of:

1. update an existing docs rule;
2. add a new rule to the relevant playbook;
3. update the prompt queue;
4. add a new optimized prompt;
5. record why no rule update was needed.

## Optimized prompt rule

If the run discovers a better next step, write a copy-ready optimized prompt in one of:

```text
docs/prompts/
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
- residual risk.

## Final response rule

Every final response should include:

```text
Evidence recorded:
Waste found:
Rule/docs updated:
Optimized prompt added:
Validation:
Commit SHA:
Completion %:
Residual risk:
```
