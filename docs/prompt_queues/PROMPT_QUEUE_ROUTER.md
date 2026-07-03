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

- proof, discovery, product/compatibility research, interviews, docs, and review may continue;
- community-opportunity runtime prototypes remain blocked until their explicit gates pass;
- new mainline product feature work remains blocked until merge + green main confirmation.

## Mandatory pre-run lint

Before running any prompt, apply:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`;
- `docs/PROMPT_LINT_CHECKLIST.md`;
- `docs/CONTEXT_PACKS.md`;
- relevant mistake-ledger entries;
- `docs/FEATURE_CAPABILITY_REGISTRY.md` when behavior, tests, release, positioning, compatibility, or claims change;
- `docs/COMPATIBILITY_INDEX.md` for AW-CAP-028 through AW-CAP-037.

If the prompt fails lint, has no suitable context pack, or assumes equal provider support, rewrite or split it.

## Mandatory post-run reconciliation

Before learning-complete status:

1. write compact run evidence;
2. classify mistakes;
3. capture/reconcile discoveries;
4. assign owners and focused follow-ups;
5. update capability/traceability rows only to the proven level;
6. link matching evidence and runtime profile where applicable;
7. preserve failures, rejects, blockers, blind spots, downgrades, and limitations.

## Runtime compatibility routing

Any work on AW-CAP-028 through AW-CAP-036 must first establish or explicitly select a runtime profile.

```text
Model/tool/surface/rights/environment unclear?
  -> OPP-RSCH-004 runtime compatibility audit

No runtime profile schema/decision engine?
  -> COMPAT-001 and COMPAT-002

Chat/manual or unsupported tool?
  -> generic/manual adapter and Manual/PostHoc fallback

Hook/event surface documented but effective state unknown?
  -> adapter handshake before Full/Guarded claim

Live enforcement requested?
  -> require permission/environment profile, enforcement class, process/hook ownership, checkpoint, and safety gate

Cloud/PR task?
  -> CloudBranchPR/PostHoc evidence profile; do not assume local worktree/process control

Canary/model comparison?
  -> pin tool, surface, permission, environment, repository, prompt/rules, model role, and network profile; otherwise mark confounded
```

Allowed support modes:

```text
Full | Guarded | Advisory | PostHoc | Manual | Unavailable
```

Hard compatibility rules:

- never infer capability from model/provider name alone;
- never infer safety from `read-only`, `safe`, `auto`, `YOLO`, or `full access` labels alone;
- never treat missing events as proof that an action did not happen;
- never call advisory text enforcement;
- never convert unknown quota/usage into tokens or currency;
- never preserve a Full state after hook/adapter/environment failure;
- every non-Full result needs a reason and fallback where one exists.

## Community opportunity routing

Use `community_opportunity_validation.md` when work concerns AW-CAP-028 through AW-CAP-037 or claims about community demand or cross-tool support.

```text
Only online reports/papers exist?
  -> OPP-RSCH-001 user interviews
Stable data/event source unknown?
  -> OPP-RSCH-002 adapter feasibility
Substitute/differentiation unclear?
  -> OPP-RSCH-003 competitive review
Runtime differences unclear?
  -> OPP-RSCH-004 compatibility audit
Problem validated but no runtime compatibility foundation?
  -> COMPAT-001..012
Runtime profile exists but no event foundation?
  -> OPP-EVENT-001..003
Foundation exists and idea has evidence?
  -> one import-only or dry-run OPP-PROT slice
Prototype useful and support modes correct?
  -> cross-surface dogfood
Live observation/enforcement/spawning/posting requested?
  -> remain blocked until explicit compatibility, safety, and proof gates
```

Additional hard rules:

- community frequency is not market size;
- pain is not willingness to pay;
- research/specification remains L1;
- import-only/dry-run precedes live control;
- no source upload or hidden telemetry;
- unknown provider capabilities remain unknown;
- every idea must be allowed to be Parked or Rejected.

## Proof routing

Use `agentwatch_proof_and_verification.md` whenever work changes runtime behavior, tests, acceptance criteria, compatibility, CI, packaging, release, README/product claims, versions, or value claims.

```text
Capability missing from registry? -> AW-PROOF-001
Traceability incomplete? -> AW-PROOF-002
Tests/scenarios absent or failing? -> AW-PROOF-003
Proof bundle missing/mismatched? -> AW-PROOF-004
Usefulness/efficiency claim? -> AW-PROOF-005
Release candidate? -> AW-PROOF-006 and AW-PROOF-007
Advanced feature lacks runtime support decision? -> AW-CAP-037 / COMPAT backlog first
```

## Gate decision

```text
PR #4 branch proof green? yes
PR #4 merged to main? no
Main proof green? pending
Community research complete? initial desk research yes
Cross-runtime compatibility specified? yes, L1 docs only
Compatibility runtime implemented? no
Community market validation complete? no
New opportunity runtime work allowed? no, except approved fixture/import work after main Gate 0 and its stated compatibility dependencies
```

## Queue priority before main confirmation

1. merge review and main-branch proof confirmation for PR #4;
2. `bootstrap_validation.md` and proof follow-ups;
3. community user interviews;
4. adapter/source feasibility;
5. competitive substitute review;
6. runtime compatibility audits for candidate tools/surfaces;
7. evidence/discovery queues;
8. MVP runtime feature queues after main Gate 0;
9. compatibility foundation before advanced community prototypes.

## First core sequence after main Gate 0

1. direct CLI process tests;
2. init hardening and temp-repo safety;
3. run-report/evidence spine;
4. evidence validator;
5. discovery runtime foundation;
6. command profiler and safety/privacy suite.

## Opportunity incubator sequence after the core evidence spine

1. Rules Compiler and generic target-loss reports;
2. manual Context Snapshot and Resume Pack;
3. AW-CAP-037 runtime profile schema and decision engine;
4. environment, permission, VCS, and generic/manual adapters;
5. compatibility CLI and fixture matrix;
6. normalized event schema/import journal;
7. Flight Recorder and Trust Ledger;
8. offline Cost and Loop Guard;
9. Policy Firewall dry-run with enforcement class;
10. local/cloud PR evidence packet;
11. coordination planner for local worktree/cloud branch/shared workspace;
12. Regression Canary with confounder detection;
13. live/enforcing behavior only after cross-surface dogfood and review.

## Rule

Capability maturity follows commit-bound evidence and the effective runtime support mode, not queue, provider name, or popularity status. If another queue conflicts before main confirmation, this router wins.
