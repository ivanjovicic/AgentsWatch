# AgentsWatch Prompt Token Rules Summary — 2026-06-29

## Scope

Added stricter prompt-writing and prompt-execution rules to reduce wasted AI-agent tokens.

## Added

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`

## Updated

- `AGENTS.md`
- `docs/PROMPT_RULES.md`
- `docs/CONTEXT_INDEX.md`
- `docs/DOCS_INDEX.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`

## Key improvements

- prompts are classified as green, yellow, or red;
- low, medium, and high budgets now have file/search/edit limits;
- every prompt needs scope, stop rules, validation, and evidence;
- broad prompts must be split;
- forbidden phrases are documented;
- final responses must report inspected files, changed files, validation, missed work, residual risk, and token waste avoided.

## Current next prompt

Gate 0 is still incomplete. The next prompt remains:

```text
AW-VAL-001 — Build validation
```

## Remaining gap

These rules are documentation-first. A future CLI command can automate prompt linting.
