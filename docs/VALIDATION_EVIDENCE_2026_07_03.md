# AgentsWatch Validation Evidence — 2026-07-03

Status: Gate 0 passed on PR branch proof run; main-branch confirmation required after merge

## Final successful proof run

Workflow run: `28650547744`  
Source branch: `feature/automatic-discovery-learning-loop`  
Source head: `2cc16f6297a3450c64f958402d0b1b3d6b670f30`  
Tested PR merge commit: `fe0c92ac98d3d88fe1bb967385ed935fd3aa808c`  
SDK: `.NET 8.0.422`

## Linux

Artifact: `proof-Linux-fe0c92ac98d3d88fe1bb967385ed935fd3aa808c` (`8062464124`)

- restore: Pass
- build Release: Pass
- tests: 8 executed, 8 passed, 0 failed
- CLI help: Pass, exit 0
- CLI version: Pass, exit 0
- CLI optimize: Pass, exit 0
- CLI status: Pass, exit 0
- unknown command: Pass, expected exit 2
- proof artifact upload: Pass

## Windows

Artifact: `proof-Windows-fe0c92ac98d3d88fe1bb967385ed935fd3aa808c` (`8062470807`)

- restore: Pass
- build Release: Pass
- tests: 8 executed, 8 passed, 0 failed
- CLI help: Pass, exit 0
- CLI version: Pass, exit 0
- CLI optimize: Pass, exit 0
- CLI status: Pass, exit 0
- unknown command: Pass, expected exit 2
- proof artifact upload: Pass

## Package and clean install

Artifact: `package-proof-fe0c92ac98d3d88fe1bb967385ed935fd3aa808c` (`8062492587`)

- restore/build: Pass
- `dotnet pack`: Pass
- package: `AgentsWatch.Cli.0.1.0.nupkg`
- SHA-256: `3bc0a9b2acb20c200ffd749186a2697ac95e42a8497647c36234d1a79d330288`
- checksum verification: Pass
- isolated `dotnet tool install`: Pass
- installed `agentswatch --help`: Pass
- installed `agentswatch --version`: Pass (`AgentsWatch 0.1.0`)
- proof manifest generated: Pass

## Test coverage represented

- broad prompt risk classification;
- scoped prompt low-risk classification;
- clean Git status;
- modified/untracked/index Git status parsing;
- CRLF and leading porcelain-status-column regression;
- .NET project detection;
- Flutter project detection;
- validation-command suggestions.

## Failures found and repaired by the proof loop

### Run `28649826676`

Restore/build passed, but tests failed on Linux and Windows.

Root cause: `StringSplitOptions.TrimEntries` removed the first Git porcelain status-column space, causing `README.md` to parse as `EADME.md`.

Repair:

- preserve leading status columns;
- trim only trailing carriage return;
- add CRLF/index-status regression coverage.

### Run `28650352024`

All tests passed on Linux and smoke outputs were correct, but the PowerShell step failed because the last intentionally invalid command left exit code 2 after the assertion.

Repair:

- explicitly exit 0 after verifying the expected unknown-command exit code;
- retain exit-code evidence in JSON.

### SDK evidence

The initial runner selected a newer installed SDK. `global.json` now pins the project to .NET 8 with latest feature-band roll-forward. Final artifacts report SDK `8.0.422`.

## Gate decision

For the tested PR branch/merge commit:

```text
restore: Pass
build: Pass
tests: Pass
CLI smoke: Pass
package: Pass
clean install: Pass
```

Therefore Gate 0 is passed for this PR branch evidence.

Main remains unchanged until PR merge. After merge, the main-branch workflow must pass before the repository-wide router marks Gate 0 complete on main.

## Not yet proven

- init idempotency/no-overwrite/path safety;
- dirty/non-git status scenarios;
- full acceptance scenario runner;
- no-network/secret/binary/privacy negative tests;
- discovery/mistake/evidence runtime commands;
- dogfood usefulness or percentage savings;
- independent release verification;
- stable/public release support.

## Related proof documents

- `PROOF_AND_VERIFICATION_STRATEGY.md`
- `FEATURE_CAPABILITY_REGISTRY.md`
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`
- `PROOF_BUNDLE_SPEC.md`
- `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`
