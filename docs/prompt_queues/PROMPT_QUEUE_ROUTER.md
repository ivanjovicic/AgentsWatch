# AgentsWatch Prompt Queue Router

Last aligned: 2026-07-03

Use this file first when choosing the next agent prompt.

## Current global state

Gate 0 is incomplete.

That means validation-first prompts have priority over feature prompts.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`
- `docs/CONTEXT_PACKS.md`
- `docs/ai/learning/MISTAKE_LEDGER.md` for relevant prior mistakes only

If the prompt fails lint or has no suitable pack, rewrite or split it before execution.

## Mandatory post-run reconciliation

Before a non-trivial run is learning-complete:

1. record compact `.ai/runs` evidence;
2. classify observed mistakes;
3. capture meaningful out-of-scope findings;
4. reconcile them against `.ai/discoveries/`, the mistake ledger, risks, docs, prompts, and queues;
5. assign each actionable finding one primary owner;
6. add a focused follow-up prompt or a documented no-op;
7. update queue/router state only when evidence and gates support it.

Use:

- `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`;
- `docs/prompt_queues/agentwatch_discovery_and_self_improvement.md`;
- `docs/prompts/DISC-001-capture-run-discoveries.md`;
- `docs/prompts/DISC-002-reconcile-discovery-inbox.md`.

This workflow may run as docs/evidence while Gate 0 is incomplete. Runtime discovery commands remain blocked.

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

Post-run override:

```text
Did the run mention missed work, residual risk, unrelated findings, stale docs, or follow-up work?
  yes -> run DISC-001 and DISC-002 before learning-complete status
  no  -> record `Out-of-scope discoveries: none found`
```

Periodic override:

```text
Do recent run logs contain follow-up items without discovery IDs?
  yes -> run DISC-005 review of untracked findings
```

## Queue priority order

1. `bootstrap_validation.md`
2. `agent_evidence_validation_followups_2026_07_01.md`
3. `agentwatch_discovery_and_self_improvement.md` for docs/evidence capture and reconciliation
4. `token_economy_hardening_2026_07_01.md`
5. `token_economy_industry_followups_2026_07_01.md`
6. `agentwatch_mvp.md`
7. `productization.md`
8. `roadmap_execution.md`
9. `architecture_evolution.md`

Discovery docs/evidence prompts do not replace the current feature queue. They close and route findings from any run.

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
- AW-DISC-001+ runtime discovery commands;
- PROD-001+ productization prompts;
- ROAD implementation prompts;
- architecture evolution prompts;
- dashboard/SaaS prompts.

Evidence validator prompts may run after AW-VAL-001/AW-VAL-002 because they validate the agent process and do not add product features.

Discovery capture, reconciliation, documentation promotion, prompt generation, and stale-item review may run as docs/evidence workflows. They must not claim that CLI automation is implemented.

Token economy hardening prompts may run as docs-only planning after the evidence validator queue is clean. They must not implement runtime CLI behavior until Gate 0 is complete.

Industry token economy follow-ups may run as docs/spec/checklist work after the first token economy queue. Runtime commands from that queue require Gate 0.

Prior-conversation backfill docs are safe to read when choosing packs, state owners, feature profiles, and queue lifecycle fields. Do not load entire old conversations; read `TOKEN_ECONOMY_PREVIOUS_CONVERSATION_BACKFILL_2026_07_01.md` instead.

## After Gate 0

Recommended order:

1. AW-EVIDENCE-VAL-001 / AW-EVIDENCE-VAL-002 — evidence validator and workflow proof
2. AW-TOKEN-IND-002 — cache-aware prompt skeleton
3. AW-TOKEN-IND-003 / AW-TOKEN-IND-004 / AW-TOKEN-IND-005 — config smell checklist, stale-context guard, queue token-budget fields
4. AW-TOKEN-IND-011 / AW-TOKEN-IND-012 / AW-TOKEN-IND-013 — state-owner filter, feature-profile gating, queue lifecycle token report
5. AW-VAL-004 / AW-002 — init hardening
6. AW-DISC-001 / AW-DISC-002 — discovery workspace and markdown model
7. PROD-002 — init temp-directory tests
8. PROD-001 — help output UX alignment
9. PROD-003 — status non-git behavior
10. AW-003 — git status/diff tracker and run reports
11. AW-DISC-003 / AW-DISC-004 — run extraction, reconciliation, and duplicate detection
12. AW-005 — prompt optimizer and task split
13. AW-DISC-005 / AW-DISC-006 — generated discovery prompts and lint gate
14. AW-006/AW-007 — handoff and diff-only review
15. AW-DISC-007 — rollup, stale review, and metrics after dogfood evidence

## Rule

If any queue disagrees with this router, use this router while Gate 0 is incomplete.
