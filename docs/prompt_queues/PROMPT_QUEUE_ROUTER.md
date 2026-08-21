# AgentsWatch Prompt Queue Router

Last aligned: 2026-08-21

Use this file first when choosing the next agent prompt.

## Current global state

Gate 0 is incomplete.

That means validation-first prompts have priority over feature prompts.

The 2026-08-21 trust-platform/Gateway strategy is planning only. It does not change the current next-prompt order.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`
- `docs/CONTEXT_PACKS.md`

If the prompt fails lint or has no suitable pack, rewrite or split it before execution.

## Fast decision tree

```text
Do we have restore/build/test evidence?
  no  -> run AW-VAL-001
  yes -> do we have CLI smoke evidence?
          no  -> run AW-VAL-002
          yes -> do we have evidence validator/workflow proof?
                  no  -> run AW-EVIDENCE-VAL-001 / AW-EVIDENCE-VAL-002
                  yes -> do we have evidence review?
                          no  -> run AW-VAL-003
                          yes -> is init hardened?
                                  no  -> run AW-VAL-004 or AW-002
                                  yes -> continue MVP/productization prompts
```

## Queue priority order

1. `bootstrap_validation.md`
2. `agent_evidence_validation_followups_2026_07_01.md`
3. `token_economy_hardening_2026_07_01.md`
4. `token_economy_industry_followups_2026_07_01.md`
5. `agentwatch_mvp.md`
6. `productization.md`
7. `roadmap_execution.md`
8. `architecture_evolution.md`

Future-only, never part of the automatic/current selection order until explicit activation gates are met:

- `agent_trust_platform_expansion.md`

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
- dashboard/SaaS prompts;
- AW-TRUST-*, AW-TEAM-*, AW-GATEWAY-*, or AW-ENTERPRISE-* runtime prompts.

Evidence validator prompts may run after AW-VAL-001/AW-VAL-002 because they validate the agent process and do not add product features.

Token economy hardening prompts may run as docs-only planning after the evidence validator queue is clean. They must not implement runtime CLI behavior until Gate 0 is complete.

Industry token economy follow-ups may run as docs/spec/checklist work after the first token economy queue. Runtime commands from that queue require Gate 0.

Prior-conversation backfill docs are safe to read when choosing packs, state owners, feature profiles, and queue lifecycle fields. Do not load entire old conversations; read `TOKEN_ECONOMY_PREVIOUS_CONVERSATION_BACKFILL_2026_07_01.md` instead.

## Trust platform expansion activation rule

`agent_trust_platform_expansion.md` preserves future strategy but is intentionally excluded from normal queue selection.

The sequence is:

```text
core Agent Run Receipt/evidence proof
  -> local deterministic Policy Engine when users ask for prevention
  -> Team Server only when shared coordination is a real problem
  -> optional Gateway only when real users request centralized model control or verified-task metrics require provider telemetry
  -> enterprise/private deployment only with paying design-partner demand
```

Do not convert strategic interest into runtime work without satisfying the corresponding activation gate in:

- `docs/AGENT_TRUST_PLATFORM_EXPANSION_2026_08_21.md`
- `docs/prompt_queues/agent_trust_platform_expansion.md`

A docs-only feasibility update may refine these plans, but it must remain explicitly non-runtime and must not mark blocked implementation rows Ready.

## After Gate 0

Recommended order:

1. AW-EVIDENCE-VAL-001 / AW-EVIDENCE-VAL-002 — evidence validator and workflow proof
2. AW-TOKEN-IND-002 — cache-aware prompt skeleton
3. AW-TOKEN-IND-003 / AW-TOKEN-IND-004 / AW-TOKEN-IND-005 — config smell checklist, stale-context guard, queue token-budget fields
4. AW-TOKEN-IND-011 / AW-TOKEN-IND-012 / AW-TOKEN-IND-013 — state-owner filter, feature-profile gating, queue lifecycle token report
5. AW-VAL-004 / AW-002 — init hardening
6. PROD-002 — init temp-directory tests
7. PROD-001 — help output UX alignment
8. PROD-003 — status non-git behavior
9. AW-003 — git status/diff tracker and run reports
10. AW-005 — prompt optimizer and task split
11. AW-006/AW-007 — handoff and diff-only review

Trust-platform rows do not enter this order merely because Gate 0 completes. Their own later activation gates still apply.

## Rule

If any queue disagrees with this router, use this router while Gate 0 is incomplete.

For future trust-platform work, both this router and the activation gates in `AGENT_TRUST_PLATFORM_EXPANSION_2026_08_21.md` must allow the work before a blocked row can become Ready.
