# AgentsWatch Prompt Queue Router

Last aligned: 2026-07-03

Use this file first when choosing the next agent prompt.

## Current global state

Gate 0 has passed on PR #4 proof run `28650547744` for the tested PR merge commit.

Evidence:

- Linux restore/build/test/smoke: Pass;
- Windows restore/build/test/smoke: Pass;
- tests: 8/8 passed on each OS;
- package/checksum/isolated install: Pass;
- validation record: `docs/VALIDATION_EVIDENCE_2026_07_03.md`.

Main has not received these changes yet. Repository-wide Gate 0 remains pending until the PR is merged and the main-branch workflow passes.

Therefore:

- proof/discovery docs and review work may continue on the PR;
- narrow corrections to this PR are allowed;
- new mainline product feature work remains blocked until merge + green main confirmation.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/PROMPT_LINT_CHECKLIST.md`
- `docs/CONTEXT_PACKS.md`
- relevant `docs/ai/learning/MISTAKE_LEDGER.md` items
- `docs/FEATURE_CAPABILITY_REGISTRY.md` when behavior, tests, release, or claims change

If the prompt fails lint or has no suitable pack, rewrite or split it.

## Mandatory post-run reconciliation

Before learning-complete status:

1. write compact run evidence;
2. classify mistakes;
3. capture/reconcile out-of-scope discoveries;
4. assign owners and focused follow-ups;
5. update affected capability/traceability rows only to the proven level;
6. link matching CI/package evidence;
7. preserve failures, skips, blockers, and limitations.

## Proof routing

Use `agentwatch_proof_and_verification.md` whenever work changes runtime behavior, tests, acceptance criteria, CI, packaging, release, README/product claims, versions, or value claims.

```text
Capability missing from registry?
  -> AW-PROOF-001
Traceability incomplete?
  -> AW-PROOF-002
Tests/scenarios absent or failing?
  -> AW-PROOF-003 / targeted test prompt
Proof bundle missing/mismatched?
  -> AW-PROOF-004
Usefulness or efficiency claim?
  -> AW-PROOF-005
Release candidate?
  -> AW-PROOF-006 and AW-PROOF-007
```

## Gate decision

```text
PR branch restore/build/test/smoke green?
  yes
Package/checksum/isolated install green?
  yes
Proof bundle manually inspected?
  yes, automatic validator still planned
PR merged to main?
  no -> keep mainline feature work blocked
Main workflow green after merge?
  pending -> then mark repository Gate 0 complete
```

## Post-run overrides

```text
Meaningful missed/risk/unrelated item?
  -> DISC-001 and DISC-002
Runtime/test/claim changed?
  -> registry + traceability + AW-PROOF-002
Recent follow-up lacks discovery ID?
  -> DISC-005
```

## Queue priority order before main confirmation

1. final PR proof/traceability review;
2. merge review and main-branch proof confirmation;
3. `bootstrap_validation.md` evidence status update;
4. `agentwatch_proof_and_verification.md` follow-ups;
5. `agent_evidence_validation_followups_2026_07_01.md`;
6. discovery/learning queues;
7. MVP feature queues only after main Gate 0 passes.

## Current next actions

```text
1. Complete final CI for the latest PR head.
2. Review final proof artifacts/claims.
3. Merge PR #4 when review is accepted.
4. Confirm the main-branch CI proof run.
5. Run AW-VAL-003 evidence review.
6. Continue with AW-VAL-004 / AW-002 init hardening.
```

## First feature-proof sequence after main Gate 0

1. AW-PROOF-TEST-001 — direct CLI process tests;
2. AW-VAL-004 / AW-002 — init hardening;
3. AW-PROOF-TEST-002 — init temp-repo/idempotency/no-overwrite/path safety;
4. AW-EVIDENCE-VAL-001 / AW-EVIDENCE-VAL-002;
5. AW-DISC-001 / AW-DISC-002;
6. run-report spine;
7. optimizer/task-split expansion;
8. safety/privacy negative suite;
9. dogfood benchmark after usable command spine;
10. independent verification before stable release.

## Rule

Capability maturity follows commit-bound evidence, not queue status. If another queue conflicts with this router before main confirmation, this router wins.
