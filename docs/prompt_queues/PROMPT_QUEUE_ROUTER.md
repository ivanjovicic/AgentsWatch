# AgentsWatch Prompt Queue Router

Last aligned: 2026-07-03

Use this file first when choosing the next agent prompt.

## Current global state

Gate 0 is incomplete.

Validation and proof prompts have priority over new feature prompts.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`
- `docs/CONTEXT_PACKS.md`
- `docs/ai/learning/MISTAKE_LEDGER.md` for relevant prior mistakes only
- `docs/FEATURE_CAPABILITY_REGISTRY.md` when behavior, tests, release, or claims change

If the prompt fails lint or has no suitable pack, rewrite or split it before execution.

## Mandatory post-run reconciliation

Before a non-trivial run is learning-complete:

1. record compact `.ai/runs` evidence;
2. classify observed mistakes;
3. capture meaningful out-of-scope findings;
4. reconcile them against discoveries, mistakes, risks, docs, prompts, and queues;
5. assign each actionable finding one primary owner;
6. add a focused follow-up prompt or a documented no-op;
7. update queue/router state only when evidence and gates support it;
8. update affected capability/traceability rows without exceeding available proof.

Use the discovery queue for findings and the proof queue for capability evidence.

## Proof routing

Use `agentwatch_proof_and_verification.md` when a task:

- implements or changes a user-visible capability;
- adds or changes tests/acceptance criteria;
- changes CI, packaging, release, README/product claims, or versioning;
- claims a capability works;
- claims token/time/cost savings;
- needs Gate 0 or release verification.

Fast proof decision:

```text
Is the capability listed in FEATURE_CAPABILITY_REGISTRY.md?
  no  -> AW-PROOF-001
  yes -> does the traceability row have contract + acceptance + implementation?
           no  -> AW-PROOF-002
           yes -> do targeted tests and black-box scenarios pass for this commit?
                    no  -> AW-PROOF-003 / targeted test prompt
                    yes -> does the proof bundle match the commit/package?
                             no  -> AW-PROOF-004
                             yes -> is a usefulness/value claim being made?
                                      yes -> AW-PROOF-005
                                      no  -> use current evidenced maturity
```

Release claims additionally require AW-PROOF-006 and AW-PROOF-007.

## Gate 0 decision tree

```text
Do we have restore/build/test evidence for the current commit?
  no  -> run/inspect AW-VAL-001 and CI proof workflow
  yes -> do we have CLI smoke evidence?
           no  -> run AW-VAL-002 / AW-PROOF-003
           yes -> do we have evidence review and proof artifact integrity?
                    no  -> AW-VAL-003 / AW-PROOF-004
                    yes -> is init hardened?
                             no  -> AW-VAL-004 or AW-002
                             yes -> continue gated MVP prompts
```

## Post-run overrides

```text
Meaningful missed work, residual risk, unrelated finding, or stale doc?
  yes -> DISC-001 and DISC-002
  no  -> record `Out-of-scope discoveries: none found`

Runtime/test/claim changed?
  yes -> update capability registry + traceability and run AW-PROOF-002

Recent run logs contain follow-up items without discovery IDs?
  yes -> DISC-005
```

## Queue priority order

1. `bootstrap_validation.md`
2. `agentwatch_proof_and_verification.md` for Gate 0/claim/CI evidence
3. `agent_evidence_validation_followups_2026_07_01.md`
4. `agentwatch_discovery_and_self_improvement.md`
5. `token_economy_hardening_2026_07_01.md`
6. `token_economy_industry_followups_2026_07_01.md`
7. `agentwatch_mvp.md`
8. `productization.md`
9. `roadmap_execution.md`
10. `architecture_evolution.md`

Discovery/proof docs workflows do not replace the owning feature queue. They close findings and establish truthful evidence.

## Current next action

The CI workflow on the active proof/discovery PR should establish or refute Gate 0 for the tested commit.

If no current green CI evidence exists:

```text
AW-VAL-001 — Build/test validation
AW-VAL-002 — CLI smoke validation
AW-PROOF-004 — proof artifact review after workflow completes
```

## Do not run yet

Until AW-VAL-001 and AW-VAL-002 are complete, do not run:

- AW-002+ product feature prompts except narrow validation fixes;
- AW-DISC-001+ runtime discovery commands;
- productization/roadmap runtime prompts;
- architecture evolution implementation;
- dashboard/SaaS prompts.

Allowed while Gate 0 is incomplete:

- CI/test/smoke/package proof of the existing skeleton;
- docs/evidence discovery workflows;
- capability inventory/traceability reviews;
- proof-bundle review;
- narrow fixes for actual restore/build/test/smoke/package failures.

Do not describe docs/manual workflows as implemented runtime automation.

## After Gate 0

Recommended order:

1. review CI/proof bundle and update Gate 0 evidence;
2. AW-PROOF-TEST-001 — direct CLI process tests;
3. AW-VAL-004 / AW-002 — init hardening;
4. AW-PROOF-TEST-002 — init temp-repo safety tests;
5. AW-EVIDENCE-VAL-001 / AW-EVIDENCE-VAL-002;
6. AW-DISC-001 / AW-DISC-002;
7. product help/status hardening;
8. AW-003 run reports;
9. AW-DISC-003 / AW-DISC-004;
10. AW-005 optimizer/task split;
11. AW-DISC-005 / AW-DISC-006;
12. AW-006/AW-007 handoff/review;
13. safety/privacy scenario suite;
14. dogfood benchmark after usable command spine;
15. independent verification before stable release.

## Rule

If any queue disagrees with this router, use this router while Gate 0 is incomplete. Capability maturity still follows actual proof, not queue status.
