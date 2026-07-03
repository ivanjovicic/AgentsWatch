# PROOF-004 — Proof bundle review

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_proof_and_verification.md`  
Run mode: review-only  
Token budget: low-medium

## Goal

Validate one CI/local proof bundle against `PROOF_BUNDLE_SPEC.md`.

## Check

- manifest commit/version equals tested/package commit;
- required artifacts exist;
- package checksum verifies;
- stages report Pass/Fail/Blocked/NotRun honestly;
- capability IDs exist in registry/matrix;
- claimed maturity does not exceed evidence;
- failed/skipped evidence remains visible;
- private source/prompts/diffs/run logs are not included by default.

## Output

Bundle ID, integrity result, missing artifacts, maturity corrections, claim mismatches, discoveries, and accept/reject/conditional decision.
