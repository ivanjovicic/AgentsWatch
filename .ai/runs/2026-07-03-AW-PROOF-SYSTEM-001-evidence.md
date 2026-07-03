# AW-PROOF-SYSTEM-001 Evidence

Prompt ID: AW-PROOF-SYSTEM-001  
Queue: `agentwatch_proof_and_verification.md`  
Agent/tool: ChatGPT with GitHub connector and GitHub Actions evidence  
Model provider: OpenAI  
Model name/id: GPT-5.5 Thinking  
Client/IDE: ChatGPT web  
Run mode: implementation + validation + docs/evidence split across focused commits  
Token budget: high  
Relevant prior mistakes: AW-MISTAKE-EVIDENCE-001, AW-MISTAKE-GATE-001, AW-MISTAKE-AUDIT-001, AW-MISTAKE-DISCOVERY-001  
Elapsed time: unknown-not-recorded

## Goal

Create a reproducible way to prove which AgentsWatch capabilities exist, work at a specific commit, provide real value, and are safe to advertise or release.

## What was added

- L0-L6 capability maturity model;
- canonical feature registry;
- claim-to-contract-to-test-to-CI traceability matrix;
- reproducible black-box acceptance scenarios;
- proof bundle/manifest/checksum contract;
- paired dogfood and benchmark methodology;
- independent release verification runbook;
- proof queue and PROOF-001 through PROOF-007 prompts;
- Linux/Windows build-test-smoke CI artifacts;
- NuGet package, SHA-256, isolated installation, and proof manifest;
- proof gates in AGENTS.md, router, test, dogfood, claims, product, positioning, README, and release documentation;
- `global.json` pinning project evidence to .NET 8.

## Runtime defect found by proof

Initial CI run `28649826676` passed restore/build but failed tests on Linux and Windows.

Root cause:

- `GitStatusParser` used `TrimEntries`;
- Git porcelain leading status-column whitespace was removed;
- ` M README.md` became `M README.md`;
- parser returned `EADME.md`.

Fix:

- preserve leading status columns;
- trim only trailing `\r`;
- add `Parse_PreservesLeadingStatusColumn_WithCrLfInput` regression test.

## Proof workflow defect found

CI run `28650352024` passed tests and generated correct smoke outputs, but the smoke step failed because the final intentionally invalid command left PowerShell exit code 2.

Fix:

- assert unknown command exit 2;
- write the exit-code artifact;
- explicitly exit the smoke step with 0 after the assertion.

## Successful proof evidence

Workflow run: `28650547744`  
Tested PR merge commit: `fe0c92ac98d3d88fe1bb967385ed935fd3aa808c`  
Source head: `2cc16f6297a3450c64f958402d0b1b3d6b670f30`  
SDK: `.NET 8.0.422`

Linux artifact `8062464124`:

- restore/build: Pass;
- 8 tests executed, 8 passed;
- help/version/optimize/status smoke: Pass;
- unknown-command expected exit 2: Pass.

Windows artifact `8062470807`:

- restore/build: Pass;
- 8 tests executed, 8 passed;
- help/version/optimize/status smoke: Pass;
- unknown-command expected exit 2: Pass.

Package artifact `8062492587`:

- package: `AgentsWatch.Cli.0.1.0.nupkg`;
- SHA-256: `3bc0a9b2acb20c200ffd749186a2697ac95e42a8497647c36234d1a79d330288`;
- checksum verification: Pass;
- isolated install: Pass;
- installed help/version: Pass;
- proof manifest generated: Pass.

## Claims corrected

- Removed treatment of `30-50%` savings as an existing result.
- Reclassified it as a hypothesis requiring measured paired benchmarks.
- Separated current implemented/verified commands from planned commands.
- Added rule: no registry row means no supported capability claim.

## Capability results

Raised with evidence:

- AW-CAP-001 Help: L4;
- AW-CAP-002 Version: L4;
- AW-CAP-004 Basic risk analysis: L4;
- AW-CAP-005 Optimize CLI current contract: L4;
- AW-CAP-006 Broad split: L3;
- AW-CAP-007 Git parser: L3;
- AW-CAP-008 Status clean-repo behavior: L3;
- AW-CAP-009 Project detection .NET/Flutter: L3;
- AW-CAP-010 Validation suggestions current rules: L3;
- AW-CAP-025 Package/local tool: L4;
- AW-CAP-027 Initial proof-bundle generation: L4.

Kept at L1/L2 where proof is incomplete:

- init safety/idempotency;
- run/report/handoff/validation commands;
- mistake/discovery runtime commands;
- supervised queue runtime;
- privacy/no-network guarantees;
- dogfood savings claims;
- independent release verification.

## Discoveries

- `AW-DISC-PROOF-001`: proof bundle validator and executable full scenario runner remain planned.
- `AW-DISC-VALIDATION-001`: parser and smoke-runner defects found and resolved.

## Validation not yet complete

- latest documentation-only PR head must receive its own final CI run;
- main-branch proof must pass after merge;
- proof manifest validation is not yet automatic;
- safety/privacy negative suite is not implemented;
- public value claims lack benchmark evidence;
- independent verification has not run.

## Residual risk

The initial proof bundle is manually reviewable and CI-generated, but automatic schema/maturity validation and full black-box scenario orchestration remain future work.

## Next prompt

1. final PR-head CI review;
2. merge/main confirmation;
3. AW-VAL-003 evidence review;
4. AW-VAL-004 / AW-002 init hardening;
5. AW-PROOF-TEST-001 and AW-PROOF-TEST-002.

## Commit SHA

- final documentation head pending after this evidence commit.
