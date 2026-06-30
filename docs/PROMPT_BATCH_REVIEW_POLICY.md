# AgentsWatch Prompt Batch Review Policy

Last aligned: 2026-06-29

## Rule

After 3-5 important prompt, queue, rule, or evidence commits, stop and review the batch before adding more prompts.

## Why

Batch review prevents:

- broken references;
- stale queue statuses;
- duplicated rules;
- contradictory docs;
- hidden validation gaps;
- prompts marked Ready when a gate blocks them.

## Triggers

Run this review after:

- 3-5 prompt-system commits;
- new prompt queue;
- `AGENTS.md` rule change;
- prompt router/fast path change;
- token economy or evidence rule change;
- docs-only expansion without runtime validation.

## Inspect only

- changed prompt/rule/docs files;
- `AGENTS.md`;
- `docs/DOCS_INDEX.md`;
- prompt queue router;
- owning queues;
- evidence summary files.

Do not inspect runtime code unless the batch claims runtime behavior changed.

## Checklist

- [ ] New docs are indexed.
- [ ] `AGENTS.md` routes to the new rules.
- [ ] `DOCS_INDEX.md` matches source-of-truth order.
- [ ] Queue statuses are accurate.
- [ ] Blocked prompts are marked blocked.
- [ ] Prompts include run mode, budget, scope, validation, and stop rules.
- [ ] No broken references exist.
- [ ] No validation is claimed without evidence.
- [ ] Missed work and residual risk are visible.
- [ ] Follow-up prompts exist for found issues.

## Output

```text
Batch reviewed:
Files reviewed:
Pass/fail:
Issues found:
Fixes applied:
Follow-up prompts added:
Validation:
Residual risk:
```

## Stop rule

If more than three unrelated issues are found, add follow-up prompts instead of fixing everything in one run.
