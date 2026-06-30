# AgentsWatch Prompt Queue Router

Last aligned: 2026-06-29

Use this file first when choosing the next agent prompt.

## Current global state

Gate 0 is incomplete.

That means validation-first prompts have priority over feature prompts.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`

If the prompt fails lint, rewrite or split it before execution.

## Fast decision tree

```text
Do we have restore/build/test evidence?
  no  -> run AW-VAL-001
  yes -> do we have CLI smoke evidence?
          no  -> run AW-VAL-002
          yes -> do we have evidence review?
                  no  -> run AW-VAL-003
                  yes -> is init hardened?
                          no  -> run AW-VAL-004 or AW-002
                          yes -> continue MVP/productization prompts
```

## Queue priority order

1. `bootstrap_validation.md`
2. `agentwatch_mvp.md`
3. `productization.md`
4. `roadmap_execution.md`
5. `architecture_evolution.md`

## Current next prompt

```text
AW-VAL-001 — Build validation
```

Prompt file:

```text
docs/prompts/AW-VAL-001-build-validation.md
```

## Do not run yet

Until AW-VAL-001 and AW-VAL-002 are complete, do not run:

- AW-002+ feature prompts;
- PROD-001+ productization prompts;
- ROAD implementation prompts;
- architecture evolution prompts;
- dashboard/SaaS prompts.

## After Gate 0

Recommended order:

1. AW-VAL-004 / AW-002 — init hardening
2. PROD-002 — init temp-directory tests
3. PROD-001 — help output UX alignment
4. PROD-003 — status non-git behavior
5. AW-003 — git status/diff tracker and run reports
6. AW-005 — prompt optimizer and task split
7. AW-006/AW-007 — handoff and diff-only review

## Rule

If any queue disagrees with this router, use this router while Gate 0 is incomplete.
