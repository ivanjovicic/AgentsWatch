# AgentsWatch Prompt Lint Checklist

Last aligned: 2026-06-30  
Status: prompt authoring checklist

## Purpose

Use this checklist when writing or changing reusable prompts.

Do not force every small agent run to read this full checklist. For normal runs, use `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md`.

## Quick run gate

A prompt can run if it has:

- one task;
- one primary run mode;
- a token budget;
- owned paths or a discovery limit;
- avoid paths or non-goals;
- validation or a blocked reason;
- stop rules.

If any item is missing, rewrite or split the prompt.

## Required pass criteria for reusable prompts

### Identity

- [ ] Repository is named.
- [ ] Prompt ID is named.
- [ ] Queue is named.
- [ ] Gate status is named.

### Mode and budget

- [ ] Exactly one primary run mode is selected.
- [ ] Token budget is low, medium, or high.
- [ ] File inspection limit is stated or inherited from the budget.
- [ ] File edit limit is stated or inherited from the budget.
- [ ] Context/doc read limit is stated or inherited from the budget.

### Scope

- [ ] Owned paths are listed.
- [ ] Avoid paths are listed.
- [ ] Non-goals are listed.
- [ ] Scope limiter is present.
- [ ] Prompt does not ask for whole-repo work.

### Evidence

- [ ] Validation commands are named, or blocked reason is named.
- [ ] Final response format is named.
- [ ] Completion percentage rule is clear for medium/high-budget work.
- [ ] Missed/follow-up/residual risk fields are required when relevant.

### Safety and waste

- [ ] Stop rules are explicit.
- [ ] The prompt does not mix multiple run modes.
- [ ] The prompt does not require long chat history.
- [ ] The prompt does not ask for unsupported product claims.
- [ ] The prompt does not bypass Gate 0.
- [ ] The prompt does not require full terminal logs by default.

## Automatic fail conditions

Fail the prompt if it includes:

- `analyze the whole repo`;
- `fix everything`;
- `make production-ready` without a gate;
- `validation optional`;
- dashboard/SaaS work before gates allow it;
- feature work before Gate 0;
- more than one run mode without split;
- no stop rules;
- no validation and no blocked reason;
- no owned paths or discovery limit;
- full command logs as required context.

## Lint result format

Compact:

```text
Prompt ID:
Lint: pass/fail
Reason:
Can run now: yes/no
Next:
```

Use the longer result only when the prompt fails and needs a rewrite.

## Rewrite rule

A failed prompt should be rewritten into the smallest safe prompt, usually one of:

- validation-only;
- investigation-only;
- implementation-only;
- tests-only;
- diff-only review;
- docs/evidence.
