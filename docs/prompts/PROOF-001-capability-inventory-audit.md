# PROOF-001 — Capability inventory audit

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_proof_and_verification.md`  
Run mode: review-only  
Token budget: low-medium

## Goal

Reconcile every claimed AgentsWatch feature with current code, tests, contracts, and executed evidence.

## Read

- `PROOF_AND_VERIFICATION_STRATEGY.md`
- `FEATURE_CAPABILITY_REGISTRY.md`
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`
- current CLI/core implementation and directly relevant tests
- current validation/CI evidence

## Work

1. List capabilities found in docs, help output, roadmap, release notes, and code.
2. Add missing capability IDs or merge duplicates.
3. Set maturity using evidence, never intention.
4. Distinguish test source from passing test evidence.
5. Downgrade claims that exceed evidence.
6. Add discovery/follow-up prompts for proof gaps.

## Validation

- every advertised feature has one registry row;
- every L2+ row has implementation path;
- every L3+ row has executed test evidence;
- every L4+ row has commit-matched CI/scenario evidence;
- docs-only behavior is not marked runtime-supported.

## Output

Capabilities added/changed, maturity changes, unsupported claims, missing proof, discoveries, and next proof prompt.
