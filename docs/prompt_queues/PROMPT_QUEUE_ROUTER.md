# AgentsWatch Prompt Queue Router

Last aligned: 2026-07-03

Use this file first when choosing the next agent prompt.

## Current global state

Gate 0 has passed on PR #4 proof run `28650547744` for its tested PR merge commit:

- Linux restore/build/test/smoke: Pass;
- Windows restore/build/test/smoke: Pass;
- tests: 8/8 passed on each OS;
- package/checksum/isolated install: Pass;
- evidence: `docs/VALIDATION_EVIDENCE_2026_07_03.md`.

Main has not received these changes yet. Repository-wide Gate 0 remains pending until PR #4 is merged and the main-branch workflow passes.

Therefore:

- proof, discovery, product research, interviews, docs, and review may continue;
- community-opportunity runtime prototypes remain blocked until their explicit gates pass;
- new mainline product feature work remains blocked until merge + green main confirmation.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`;
- `docs/PROMPT_LINT_CHECKLIST.md`;
- `docs/CONTEXT_PACKS.md`;
- relevant mistake-ledger entries;
- `docs/FEATURE_CAPABILITY_REGISTRY.md` when behavior, tests, release, positioning, or claims change.

If the prompt fails lint or has no suitable pack, rewrite or split it.

## Mandatory post-run reconciliation

Before learning-complete status:

1. write compact run evidence;
2. classify mistakes;
3. capture/reconcile discoveries;
4. assign owners and focused follow-ups;
5. update capability/traceability rows only to the proven level;
6. link matching evidence;
7. preserve failures, rejects, blockers, and limitations.

## Community opportunity routing

Use `community_opportunity_validation.md` when work concerns AW-CAP-028 through AW-CAP-036 or claims about community demand.

```text
Only online reports/papers exist?
  -> OPP-RSCH-001 user interviews
Stable local data source unknown?
  -> OPP-RSCH-002 adapter feasibility
Substitute/differentiation unclear?
  -> OPP-RSCH-003 competitive review
Problem validated but no shared event foundation?
  -> OPP-EVENT-001..003
Foundation exists and idea has evidence?
  -> one import-only or dry-run OPP-PROT slice
Prototype useful and low-noise?
  -> paired dogfood
Live observation/enforcement/spawning/posting requested?
  -> remain blocked until explicit safety and proof gate
```

Hard rules:

- community frequency is not market size;
- pain is not willingness to pay;
- research/specification remains L1;
- import-only/dry-run precedes live control;
- no source upload or hidden telemetry;
- unknown provider capabilities remain unknown;
- every idea must be allowed to be Parked or Rejected.

## Proof routing

Use `agentwatch_proof_and_verification.md` whenever work changes runtime behavior, tests, acceptance criteria, CI, packaging, release, README/product claims, versions, or value claims.

```text
Capability missing from registry? -> AW-PROOF-001
Traceability incomplete? -> AW-PROOF-002
Tests/scenarios absent or failing? -> AW-PROOF-003
Proof bundle missing/mismatched? -> AW-PROOF-004
Usefulness/efficiency claim? -> AW-PROOF-005
Release candidate? -> AW-PROOF-006 and AW-PROOF-007
```

## Gate decision

```text
PR #4 branch proof green? yes
PR #4 merged to main? no
Main proof green? pending
Community research complete? initial desk research yes
Community market validation complete? no
New opportunity runtime work allowed? no, except explicitly approved tiny fixture/import spikes after main Gate 0
```

## Queue priority before main confirmation

1. merge review and main-branch proof confirmation for PR #4;
2. `bootstrap_validation.md` and proof follow-ups;
3. community OPP-RSCH-001 interviews;
4. community OPP-RSCH-002 adapter feasibility;
5. community OPP-RSCH-003 competitive substitute review;
6. evidence/discovery queues;
7. MVP runtime feature queues after main Gate 0;
8. community prototypes only after their validation gates.

## First core sequence after main Gate 0

1. direct CLI process tests;
2. init hardening and temp-repo safety;
3. run-report/evidence spine;
4. evidence validator;
5. discovery runtime foundation;
6. command profiler and safety/privacy suite.

## Opportunity incubator sequence after the core evidence spine

1. Rules Compiler and Drift Detector;
2. manual Context Snapshot and Resume Pack;
3. normalized event schema/import journal;
4. Flight Recorder and Trust Ledger;
5. offline Cost and Loop Guard;
6. Policy Firewall dry-run;
7. local PR evidence packet;
8. Worktree Coordinator planner;
9. Regression Canary;
10. live/enforcing behavior only after dogfood and review.

## Rule

Capability maturity follows commit-bound evidence, not queue or popularity status. If another queue conflicts before main confirmation, this router wins.
