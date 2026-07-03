# AgentsWatch Proof and Verification Strategy

Last aligned: 2026-07-03  
Status: canonical proof contract

## Purpose

AgentsWatch must be able to prove, at a specific commit, which capabilities exist, which are only specified, which are tested, and which have been demonstrated on real repositories.

The proof system must answer:

```text
What capability is claimed?
Where is its contract?
What acceptance criteria define success?
Which automated tests cover it?
Which reproducible scenario demonstrates it?
Which CI run verified it?
Which dogfood evidence shows real value?
Which release contains it?
```

A final response, roadmap row, documentation statement, or marketing claim is never sufficient proof by itself.

## Proof principles

1. **Commit-bound** — evidence must identify the exact commit or release.
2. **Reproducible** — another person can repeat the same scenario.
3. **Layered** — unit tests alone do not prove end-to-end CLI behavior.
4. **Negative as well as positive** — failure, safety, privacy, and refusal behavior are tested.
5. **Traceable** — every supported capability maps to acceptance criteria and evidence.
6. **Local-first** — proof does not require uploading repository source, prompts, diffs, or run logs.
7. **Truthful maturity** — claims never exceed the strongest available evidence.
8. **Independent where important** — release candidates receive black-box verification outside the developer workspace.

## Capability maturity levels

| Level | Name | Required evidence | Allowed claim |
|---|---|---|---|
| L0 | Idea | backlog/discovery only | planned idea |
| L1 | Specified | canonical contract and acceptance criteria | specified/planned |
| L2 | Implemented | runtime code exists and diff matches claim | implemented, not yet verified |
| L3 | Test-backed | targeted automated tests pass locally or in CI | automated tests cover it |
| L4 | CI-verified | required CI matrix and acceptance scenario pass for the commit | verified for listed environments |
| L5 | Dogfood-verified | real-repo evidence demonstrates expected behavior and value | demonstrated in dogfood |
| L6 | Release-verified | clean install, package checksum, black-box scenarios, and release proof bundle pass | supported in this release |

Rules:

- A capability may not skip required lower levels.
- Documentation-only work remains at L1.
- Runtime code without executed tests remains at L2.
- Existing tests with no visible passing run do not establish L3/L4 for the current commit.
- A green build does not prove every feature; the traceability matrix must identify the relevant tests/scenarios.
- Token-saving or productivity claims require L5 evidence from repeated comparable tasks.

## Five proof layers

### 1. Contract proof

Required:

- stable capability ID;
- owning command/module;
- canonical behavior contract;
- acceptance criteria;
- known non-goals and safety boundaries.

Primary artifact:

- `FEATURE_CAPABILITY_REGISTRY.md`.

### 2. Automated behavior proof

Use the smallest suitable combination:

- unit tests for pure parsing, scoring, normalization, and formatting;
- fixture tests for project/repository types;
- integration tests in temporary git repositories;
- golden tests for stable markdown/JSON output;
- regression tests for every accepted mistake/bug;
- safety/privacy tests for overwrite, path, network, and secret behavior;
- mutation testing later for critical pure logic where ordinary coverage can pass despite weak assertions.

### 3. Executable acceptance proof

Each user-visible capability needs at least one black-box scenario containing:

```text
Scenario ID
Capability ID
Given
When
Then
Expected exit code
Expected stdout/stderr anchors
Expected files/artifacts
Forbidden side effects
Cleanup
```

Scenarios must run against a temporary or fixture repository, not the developer's real working tree.

### 4. Real-repository proof

Dogfood evidence must show more than command success. It should record:

- raw task and optimized task;
- baseline and AgentsWatch-assisted workflow;
- files inspected and changed when observable;
- validation commands and results;
- scope violations prevented/caught;
- evidence mistakes caught;
- repeated work avoided;
- token/time values only when actually available;
- outcome quality and unresolved risk.

### 5. Release proof

A release is supported only when a proof bundle contains:

- commit and version manifest;
- build and test results;
- acceptance scenario results;
- CLI smoke transcripts;
- package and checksum;
- environment/runtime versions;
- capability matrix snapshot;
- known limitations;
- clean-install/black-box result;
- dogfood references for value claims.

See `PROOF_BUNDLE_SPEC.md`.

## Evidence types and strength

| Evidence | Strength | Notes |
|---|---|---|
| planning document | weak | proves intent only |
| source diff | moderate | proves code changed, not that it works |
| unit test source | moderate | proves a test exists, not that it passed |
| passing targeted CI test | strong for narrow behavior | must map to capability |
| passing integration/acceptance scenario | strong | proves observable behavior |
| golden output comparison | strong for output contract | use fixed clocks/paths |
| safety/privacy negative test | strong for prohibited behavior | include forbidden side effects |
| real-repo dogfood | strong for usefulness | not a replacement for deterministic tests |
| independent clean-install run | strongest release evidence | black-box, pinned artifact |

## Proof hierarchy by feature type

| Feature type | Minimum credible proof |
|---|---|
| pure parser/scorer | unit + boundary + regression tests |
| file-writing command | temp-directory integration + idempotency + path-safety tests |
| git behavior | temporary git repo integration across clean/dirty/untracked/rename/delete cases |
| report/template generator | golden output + schema/required-field lint |
| CLI command | black-box process execution, exit code, stdout/stderr, and artifact assertions |
| safety/privacy claim | negative tests plus network/path/secret audit evidence |
| token-saving claim | repeated baseline-vs-assisted dogfood benchmark with disclosed limitations |
| cross-platform claim | required scenario passes on each named OS in CI |
| release support claim | clean install from packaged artifact plus proof bundle |

## Required evidence chain

Every capability marked L2 or higher must have a traceability row:

```text
Capability ID
Claim
Contract path
Implementation path
Acceptance criteria IDs
Automated test IDs/paths
Acceptance scenario IDs
CI evidence
Dogfood evidence
Release evidence
Current maturity
Known gap
Next proof action
```

Missing links automatically cap maturity.

## False-proof prevention

The proof system must reject or downgrade:

- test files that exist but have no executed result;
- CI that passes while the relevant scenario was skipped;
- documentation that claims runtime behavior with no implementation diff;
- coverage percentages without requirement mapping;
- snapshot tests that only approve incorrect output;
- dogfood anecdotes with no baseline or dated evidence;
- token-saving percentages based on one task or estimated values presented as measured;
- manually edited proof manifests without linked raw artifacts;
- release claims against a different commit than the packaged artifact.

## Current AgentsWatch truth

At the time of this document:

- `init`, `optimize`, `status`, `--help`, and `--version` have runtime code;
- prompt risk analysis, git status parsing, and project-type/validation suggestions have some unit-test source;
- current Gate 0 restore/build/test and CLI-smoke execution evidence is still incomplete;
- many later commands are specified but not implemented;
- the discovery/self-improvement workflow is documentation/prompt-ready but runtime automation is not implemented.

Therefore the project must not describe the full planned product as already functional.

## Required proof artifacts

- `FEATURE_CAPABILITY_REGISTRY.md`
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`
- `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`
- `PROOF_BUNDLE_SPEC.md`
- `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`
- `INDEPENDENT_VERIFICATION_RUNBOOK.md`
- `prompt_queues/agentwatch_proof_and_verification.md`

## Completion rule

A capability is supported only when:

1. its registry row is current;
2. acceptance criteria are explicit;
3. required automated tests pass;
4. required black-box scenario passes;
5. CI evidence is linked to the same commit;
6. safety/privacy requirements pass when applicable;
7. the proof bundle contains the required artifacts;
8. known limitations are visible.

Anything less must use a lower maturity label.
