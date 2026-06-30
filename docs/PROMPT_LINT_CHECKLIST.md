# AgentsWatch Prompt Lint Checklist

Last aligned: 2026-06-29  
Status: pre-run checklist

## Purpose

Use this checklist before running any agent prompt.

If the prompt fails this lint, rewrite or split it before execution.

## Required pass criteria

### Identity

- [ ] Repository is named.
- [ ] Prompt ID is named.
- [ ] Queue is named.
- [ ] Gate status is named.

### Mode and budget

- [ ] Exactly one run mode is selected.
- [ ] Token budget is low, medium, or high.
- [ ] File inspection limit is stated or inherited from the budget.
- [ ] File edit limit is stated or inherited from the budget.

### Scope

- [ ] Owned paths are listed.
- [ ] Avoid paths are listed.
- [ ] Non-goals are listed.
- [ ] Scope limiter is present.
- [ ] Prompt does not ask for whole-repo work.

### Evidence

- [ ] Validation commands are named.
- [ ] Final response format is named.
- [ ] Completion percentage rule is clear.
- [ ] Missed/follow-up/residual risk fields are required.

### Safety and waste

- [ ] Stop rules are explicit.
- [ ] The prompt does not mix multiple run modes.
- [ ] The prompt does not require long chat history.
- [ ] The prompt does not ask for unsupported product claims.
- [ ] The prompt does not bypass Gate 0.

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
- no validation;
- no owned paths.

## Lint result format

```text
Prompt ID:
Lint result: pass/fail
Reason:
Required rewrite:
Can run now: yes/no
Next safe prompt:
```

## Rewrite rule

A failed prompt should be rewritten into the smallest safe prompt, usually one of:

- validation-only;
- investigation-only;
- implementation-only;
- tests-only;
- diff-only review;
- docs/evidence.
