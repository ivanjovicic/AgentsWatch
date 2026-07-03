# AgentsWatch Proof System Index

Last aligned: 2026-07-03

## Core truth

- `PROOF_AND_VERIFICATION_STRATEGY.md` — L0-L6 maturity and proof rules.
- `FEATURE_CAPABILITY_REGISTRY.md` — current feature/maturity inventory.
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md` — contract/test/scenario/CI/dogfood/release links.

## Executable proof

- `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md` — black-box scenarios.
- `TEST_STRATEGY.md` — detailed test layers.
- `TEST_STRATEGY_PROOF_ADDENDUM.md` — requirement traceability and proof additions.
- `TEST_MATRIX.md` — risk-focused test/proof matrix.
- `.github/workflows/ci.yml` — Linux/Windows build-test-smoke and package proof workflow.

## CI and release artifacts

- `PROOF_BUNDLE_SPEC.md` — manifest/artifact format.
- `RELEASE_AND_PACKAGING_PLAN.md` — package/checksum/install/release gates.
- `INDEPENDENT_VERIFICATION_RUNBOOK.md` — clean-environment verification.

## Usefulness and claims

- `DOGFOOD_RUNBOOK.md` — operational real-repo evidence.
- `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` — paired benchmark and safe percentage claims.
- `CLAIMS_VS_ACTUAL_REVIEW.md` — evidence-based claim audit.
- `POSITIONING_AND_PRICING_HYPOTHESES.md` — commercial hypotheses with proof gates.
- `PRODUCT_SPEC.md` and `../README.md` — public wording constrained by proof maturity.

## Planning and queues

- `MVP_PROOF_EPIC_ADDENDUM.md` — MVP proof epic and acceptance criteria.
- `prompt_queues/agentwatch_proof_and_verification.md` — implementation/workflow queue.
- `prompts/PROOF-001-capability-inventory-audit.md`
- `prompts/PROOF-002-traceability-review.md`
- `prompts/PROOF-003-black-box-acceptance.md`
- `prompts/PROOF-004-proof-bundle-review.md`
- `prompts/PROOF-005-dogfood-benchmark.md`
- `prompts/PROOF-006-independent-verification.md`
- `prompts/PROOF-007-release-claim-certification.md`

## Current state

The contracts and CI proof workflow exist. Actual capability levels remain unchanged until current-commit workflow/scenario artifacts are reviewed. Manifest validation, full black-box scenario automation, dedicated safety/privacy tests, and independent release verification remain follow-up work.
