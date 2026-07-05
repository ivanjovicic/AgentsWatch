# AgentsWatch Feature Evidence Traceability Matrix

Last aligned: 2026-07-05  
Status: mandatory proof index

## Purpose

Map every claimed capability to its contract, implementation, acceptance criteria, automated tests, black-box scenarios, CI evidence, dogfood evidence, and release evidence.

The last executed proof snapshot is still:

- workflow run `28650547744`;
- tested PR merge commit `fe0c92ac98d3d88fe1bb967385ed935fd3aa808c`;
- represented source head `2cc16f6297a3450c64f958402d0b1b3d6b670f30`;
- Linux artifact `8062464124`;
- Windows artifact `8062470807`;
- package artifact `8062492587`.

The newer `feature/pr-evidence-run-foundation` branch is not represented by that snapshot. Source and unexecuted tests justify L2 only for its new capabilities.

## Current traceability rows

| Capability | Contract | Implementation | Automated/acceptance proof | CI/package evidence | Current maturity | Remaining gap |
|---|---|---|---|---|---|---|
| AW-CAP-001 Help | `CLI_SPEC.md`, `CLI_UX_OUTPUT_SPEC.md` | `src/AgentsWatch.Cli/Program.cs` | AW-SCN-HELP-001 behavior defined | Earlier Linux/Windows smoke + installed package help | L4 for earlier tested head | rerun for current head; independent release verification |
| AW-CAP-002 Version | CLI/version/release contracts | `Program.cs`, CLI csproj | AW-SCN-VERSION-001 behavior defined | Earlier Linux/Windows smoke + installed package version | L4 for earlier tested head | rerun for current head; tag/package/release certification |
| AW-CAP-003 Init | CLI/config/init contracts | `Program.cs` | no temp-directory integration yet | earlier build only | L2 | idempotency, no-overwrite output, path safety, cross-platform scenario |
| AW-CAP-004 Risk analysis | risk/prompt contracts | `PromptRiskAnalyzer.cs` | broad/scoped unit cases passed on earlier head | earlier 8/8 tests Linux/Windows + optimize smoke | L4 for earlier rule set | rerun current head; boundaries, corpus, golden stability |
| AW-CAP-005 Optimize CLI | CLI/prompt contracts | `Program.cs`, analyzer | scoped CLI smoke/analyzer tests passed earlier | earlier Linux/Windows smoke | L4 for earlier output | rerun current head; invalid input/file/encoding scenarios |
| AW-CAP-006 Broad split | prompt contracts | analyzer | broad multi-mode unit case passed earlier | earlier Linux/Windows TRX | L3 for earlier head | rerun current head; broad-prompt black-box/golden scenario |
| AW-CAP-007 Git parser | git/test contracts | `GitCommandRunner.cs`, `GitChangeSetReader.cs` | earlier status parser tests passed; new name-status/change-set tests added but not run | no current-head evidence | L2 for new change-set additions; prior parser subset L3 | execute rename/delete/untracked/object-ID tests and dirty-repo black-box |
| AW-CAP-008 Status CLI | CLI/git/adapter contracts | `Program.cs`, Git project | clean repository status smoke passed earlier | earlier Linux/Windows smoke | L3 for earlier head | rerun current head; dirty/non-git/path-space scenarios |
| AW-CAP-009 Project detection | adapter contract | LanguageAdapters project | .NET/Flutter unit cases passed earlier | earlier Linux/Windows TRX | L3 for earlier head | rerun current head; React/Python/Node/mixed fixtures |
| AW-CAP-010 Validation suggestions | adapter contract | LanguageAdapters project | provider test passed earlier | earlier Linux/Windows TRX | L3 for earlier head | rerun current head; mixed repo/no-auto-execution scenario |
| AW-CAP-012 Start evidence | `COMMAND_CONTRACTS.md`, `MVP_EPICS_AND_ACCEPTANCE.md`, run foundation contract | `RunEvidenceModels.cs`, `RunEvidenceReports.cs`, `RunEvidenceCommands.cs`, dispatcher in `Program.cs` | unit tests added for IDs, schema, atomic/no-overwrite storage, active/latest lookup; black-box start scenarios specified | none for feature branch | L2 | execute tests; clean/dirty/duplicate/overlap CLI proof on Linux/Windows |
| AW-CAP-013 Finish evidence | command/report/evidence contracts | same foundation files + `GitChangeSetReader.cs` | tests added for change-set parsing, full object IDs, self-artifact filtering, scope matching, explicit NotRun reporting; black-box finish scenarios specified | none for feature branch | L2 for Git/scope-only slice | execute tests/CLI scenarios; add command/build/test/CI evidence and closure gates |
| AW-CAP-014 Run report | report/data contracts | `RunEvidenceReports.cs`, `RunEvidenceCommands.cs` | tests added for deterministic order, privacy, schema, latest-run lookup, evidence boundary; golden black-box scenario specified | none for feature branch | L2 for foundation report | execute tests/golden scenarios; add remaining validation/risk/missed/follow-up sections |
| AW-CAP-015 Handoff | handoff/report contracts | not implemented | scenarios/contracts only | none | L1 | implementation after run report proof |
| AW-CAP-016 Diff-only review | report/test contracts | not implemented | scenarios/contracts only | none | L1 | implementation and changed-file-only proof |
| AW-CAP-017 Validation runner | CLI/test/adapter docs | not implemented | contract only | none | L1 | explicit execution, timeout/cancel/failure evidence |
| AW-CAP-018 Command profiler | command profiler contract | not implemented | contract only | none | L1 | next slice: command execution, redaction, commit binding, compact evidence |
| AW-CAP-019 Claims-vs-actual | checklist/evidence contracts | manual checklist only | no deterministic runtime tests | none | L1 | claim schema/classifier and fixtures |
| AW-CAP-020 Mistake learning | learning contracts | manual docs only | manual workflow only | none | L1 | runtime parser/linter |
| AW-CAP-022 Discovery reconciliation | discovery contracts | manual docs only | docs dogfood record | none for runtime | L1 | AW-DISC implementation and scenarios |
| AW-CAP-025 Packaging | release/package contracts | CLI csproj tool config | package/hash/isolated install passed earlier | package artifact `8062492587` | L4 for prior package | package and clean-install current implementation head |
| AW-CAP-026 Local-first/privacy | security/privacy contracts | partial architecture; run manifests omit absolute repository root | privacy-oriented unit assertions added but not run | none current | L1 overall | no-network/path/secret/binary black-box suite |
| AW-CAP-027 Proof bundle | proof contracts | `.github/workflows/ci.yml` | earlier Linux/Windows TRX/smoke + package manifest/checksum | earlier bundle only | L4 for earlier bundle | generate current-head bundle; schema/maturity validator and safety suite |
| AW-CAP-028 Flight recorder | AW-OPP-01 + compatibility contracts | not implemented | timeline/compatibility scenarios | none | L1 | runtime profile, event adapters, import-only timeline |
| AW-CAP-029 Trust ledger | AW-OPP-01 + compatibility contracts | not implemented; run-evidence prerequisite partially exists | claim states/compatibility scenarios | none | L1 | deterministic claims and evidence-grade engine after command evidence |
| AW-CAP-030 Context/session rescue | AW-OPP-02 + compatibility contracts | not implemented | deterministic export/compatibility scenarios | none | L1 | interviews, manual snapshot, target-loss reports |
| AW-CAP-031 Rules compiler/drift | AW-OPP-02 + `ADAPTER_SPEC.md` | not implemented | no-overwrite/target-loss criteria | none | L1 | adapters, precedence, golden/loss tests |
| AW-CAP-032 Cost/loop analysis | AW-OPP-03 + compatibility contracts | not implemented | repeated-action/budget criteria | none | L1 | offline analyzer after command evidence; telemetry provenance |
| AW-CAP-033 Policy dry-run/broker | AW-OPP-04 + compatibility contracts | not implemented | policy/compatibility criteria | none | L1 | enforcement classes, threat model, dry-run, security review |
| AW-CAP-034 Multi-agent diagnostics/coordinator | AW-OPP-05 + compatibility contracts | not implemented | worktree/ownership criteria | none | L1 | coordination selector and local/cloud/shared fixtures |
| AW-CAP-035 PR Evidence | AW-OPP-06 + market/compatibility contracts | not implemented; AW-CAP-012..014 prerequisite slice exists | scope/evidence criteria and 30-PR runbook | none | L1 | command/claim evidence, deterministic packet, market experiment |
| AW-CAP-036 Regression canary | opportunity/compatibility contracts | concept only | comparison validity scenarios | none | L1 | stable suite, repetitions, confounders, metrics |
| AW-CAP-037 Runtime negotiation | runtime fallback and adapter contracts | not implemented | 50 compatibility scenarios | none | L1 | profile schema, declarations/handshake, detectors, decision engine |

## Run-evidence foundation proof boundary

The feature branch source currently intends to prove only:

- immutable start/end Git object identities;
- one active run per repository;
- duplicate run IDs do not overwrite evidence;
- clean-start attribution boundary;
- changed-file collection since start plus untracked files;
- current run artifacts are excluded from agent-change findings;
- declared-scope comparison;
- JSON machine sidecar and Markdown human report;
- latest-run report selection;
- `Validation: NotRun` when no command evidence exists;
- no absolute local repository root in shareable run artifacts.

It does not yet prove:

- build/test/CI execution;
- agent tool actions or files read;
- agent claims;
- runtime/UI/database behavior;
- complete PR Evidence or Trust Ledger output;
- market usefulness.

## Compatibility proof rule

The planned capabilities are semantically reusable but not equally observable or enforceable across models, tools, surfaces, permissions, and environments.

A feature proof is invalid when it:

- selects support solely by model/provider name;
- reports Full without required runtime capabilities;
- treats missing events as proof that an action did not happen;
- calls advisory classification enforcement;
- compares canaries across changed environments without a confounder warning;
- converts unknown quota/cost units into invented token/currency values;
- uses local-worktree language for a cloud branch/PR flow;
- ignores managed policy, read-only mounts, remote execution, or missing credentials;
- fails to downgrade after adapter/hook failure.

AW-CAP-028 through AW-CAP-036 cannot move beyond L1 without an AW-CAP-037 support decision for every runtime-specific tested profile. Generic/manual Git evidence may progress independently where its observable boundary is explicit.

## Community opportunity proof rule

Research proves repeated problem reports. It does not prove market size, willingness to pay, runtime feasibility, provider event stability, equal tool support, security effectiveness, numerical savings, or popularity.

## Earlier executed test details

The prior Linux and Windows TRX files reported 8/8 tests passed and covered prompt analysis, status parsing, .NET/Flutter detection, and validation suggestions. They do not cover the new run-evidence files.

Prior package proof:

```text
AgentsWatch.Cli.0.1.0.nupkg
SHA-256: 3bc0a9b2acb20c200ffd749186a2697ac95e42a8497647c36234d1a79d330288
checksum verification: OK
isolated install: Pass
installed help/version: Pass
SDK: 8.0.422
```

## Evidence update procedure

For each implementation change:

1. identify capability IDs;
2. update observable acceptance criteria;
3. add targeted tests;
4. add/update black-box and compatibility scenarios;
5. run required validation;
6. link commit-matched CI/package artifacts;
7. raise maturity only to the proven level;
8. create discoveries for remaining gaps.

## Release rule

Release proof fails when an advertised capability lacks a registry/matrix row, implementation, required scenarios, compatibility support decision where relevant, commit match, checksum/clean-install proof, or dogfood evidence for value claims.
