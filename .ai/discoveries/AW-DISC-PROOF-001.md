# AW-DISC-PROOF-001 — Proof generation exists before proof validation automation

Discovery ID: AW-DISC-PROOF-001  
Status: Planned  
Category: ValidationGap  
Severity: P1  
Confidence: Confirmed  
Found in run: AW-PROOF-SYSTEM-001  
Found while doing: capability proof and CI artifact design  
Created: 2026-07-03  
Last reviewed: 2026-07-03

## Evidence summary

The CI workflow now generates test/smoke/package/checksum/manifest artifacts, but no deterministic validator yet checks the manifest schema, artifact presence, commit match, capability maturity calculation, or full acceptance scenario set.

## Affected paths or contracts

- `.github/workflows/ci.yml`
- `docs/PROOF_BUNDLE_SPEC.md`
- `docs/REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`
- `docs/FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`

## Reason it was not handled in the active task

The current work establishes the proof contract and initial CI evidence. A validator and scenario runner require a separate tested implementation slice after Gate 0 results are known.

## Reconciliation

Duplicate of: none  
Primary owner: `docs/prompt_queues/agentwatch_proof_and_verification.md`  
Canonical document target: `docs/PROOF_BUNDLE_SPEC.md`  
Queue target: AW-PROOF-MANIFEST-001 and AW-PROOF-SCENARIO-001  
Prompt target: PROOF-004 and PROOF-003  
Gate or dependencies: stable first proof artifacts and Gate 0 review  
Recommended validation: schema fixture tests, corrupted/missing artifact cases, commit mismatch tests, scenario runner integration tests

## Disposition

Action: implement in focused proof slices  
Reason: generated evidence must be mechanically checked before it can establish release trust  
Resolved by: none

## Links

Run log: `.ai/runs/2026-07-03-AW-PROOF-SYSTEM-001-evidence.md`  
Queue row: `docs/prompt_queues/agentwatch_proof_and_verification.md`  
Commit or pull request: `https://github.com/ivanjovicic/AgentsWatch/pull/4`
