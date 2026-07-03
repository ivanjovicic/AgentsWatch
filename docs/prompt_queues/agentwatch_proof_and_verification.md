# AgentsWatch Proof and Verification Queue

Last aligned: 2026-07-03  
Target repo: `ivanjovicic/AgentsWatch`  
Parent router: `PROMPT_QUEUE_ROUTER.md`

## Purpose

Make every capability traceable from claim to contract, acceptance criteria, tests, black-box scenarios, CI artifacts, dogfood, and release evidence.

## Read first

- `../PROOF_AND_VERIFICATION_STRATEGY.md`
- `../FEATURE_CAPABILITY_REGISTRY.md`
- `../FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`
- `../REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`
- `../PROOF_BUNDLE_SPEC.md`
- `../BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`
- `../INDEPENDENT_VERIFICATION_RUNBOOK.md`
- `PROMPT_QUEUE_ROUTER.md`

## Rules

- Evidence is commit-bound and reproducible.
- Existing test source is not passing-test evidence.
- A green build does not prove every capability.
- Every maturity increase updates the registry and matrix.
- Failed, blocked, skipped, and missing evidence remain visible.
- Percentage savings claims require measured paired benchmarks.
- Proof artifacts stay local/CI and exclude private source, prompts, diffs, and run logs by default.

## Workflow prompts

| ID | Status | Purpose |
|---|---|---|
| AW-PROOF-001 | Ready now | Audit capability inventory against current code/tests/docs. |
| AW-PROOF-002 | Ready after implementation/test changes | Review contract-to-evidence traceability. |
| AW-PROOF-003 | Ready when CLI can execute | Run black-box acceptance scenarios in temp repos. |
| AW-PROOF-004 | Ready when CI artifacts exist | Review proof manifest, artifacts, checksums, and maturity. |
| AW-PROOF-005 | Ready after dogfood prerequisites | Run paired baseline-vs-assisted benchmark. |
| AW-PROOF-006 | Ready for milestone/release candidate | Perform clean-install independent verification. |
| AW-PROOF-007 | Ready before release/marketing changes | Certify or downgrade public claims. |

Prompt files:

- `../prompts/PROOF-001-capability-inventory-audit.md`
- `../prompts/PROOF-002-traceability-review.md`
- `../prompts/PROOF-003-black-box-acceptance.md`
- `../prompts/PROOF-004-proof-bundle-review.md`
- `../prompts/PROOF-005-dogfood-benchmark.md`
- `../prompts/PROOF-006-independent-verification.md`
- `../prompts/PROOF-007-release-claim-certification.md`

## Implementation slices

| ID | Status | Purpose |
|---|---|---|
| AW-PROOF-CI-001 | Added; needs CI run | Cross-platform restore/build/test/CLI-smoke artifacts. |
| AW-PROOF-CI-002 | Added; needs CI run | Package, checksum, clean install, and proof manifest. |
| AW-PROOF-TEST-001 | Ready after Gate 0 result | Direct CLI process tests for current commands. |
| AW-PROOF-TEST-002 | Ready after Gate 0 result | Init idempotency/no-overwrite/path-safety tests. |
| AW-PROOF-TEST-003 | Ready after Gate 0 result | Expanded git/project fixtures and golden outputs. |
| AW-PROOF-SAFE-001 | Ready after local command spine | No-network, outside-path, binary, and fake-secret tests. |
| AW-PROOF-MANIFEST-001 | Blocked until artifacts stabilize | Manifest schema validation and maturity calculation. |
| AW-PROOF-SCENARIO-001 | Blocked until command contracts stabilize | Executable scenario runner. |
| AW-PROOF-COVERAGE-001 | Backlog | Coverage as diagnostic evidence, never sole feature proof. |
| AW-PROOF-MUTATION-001 | Backlog | Mutation tests for critical deterministic rules. |

## Gate 0 relationship

CI may validate the existing skeleton now. New product features remain blocked until restore/build/test/CLI-smoke evidence passes.

When CI fails, record the exact stage, add a narrow validation discovery, fix only the blocker, and rerun. Do not raise maturity before green evidence exists.

## Exit criteria

- every advertised capability has registry/matrix coverage;
- CI produces Linux/Windows test and smoke artifacts;
- package proof includes checksum and isolated install;
- current command scenarios have executed results;
- proof manifest validation exists;
- actual CI updates Gate 0 evidence;
- dogfood supports value claims;
- independent verification exists before a stable public release.
