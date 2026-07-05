# AgentsWatch Feature Evidence Traceability Matrix

Last aligned: 2026-07-03  
Status: mandatory proof index

## Purpose

Map every claimed capability to its contract, implementation, acceptance criteria, automated tests, black-box scenarios, CI evidence, dogfood evidence, and release evidence.

Evidence snapshot used below:

- workflow run `28650547744`;
- tested PR merge commit `fe0c92ac98d3d88fe1bb967385ed935fd3aa808c`;
- source head `2cc16f6297a3450c64f958402d0b1b3d6b670f30`;
- Linux artifact `8062464124`;
- Windows artifact `8062470807`;
- package artifact `8062492587`.

## Current traceability rows

| Capability | Contract | Implementation | Automated/acceptance proof | CI/package evidence | Current maturity | Remaining gap |
|---|---|---|---|---|---|---|
| AW-CAP-001 Help | `CLI_SPEC.md`, `CLI_UX_OUTPUT_SPEC.md` | `src/AgentsWatch.Cli/Program.cs` | AW-SCN-HELP-001 behavior: exit 0, expected output, no write | Linux/Windows smoke + installed package help | L4 | independent release verification |
| AW-CAP-002 Version | CLI/version/release contracts | `Program.cs`, CLI csproj | AW-SCN-VERSION-001 behavior: exit 0, `0.1.0` | Linux/Windows smoke + installed package version | L4 | tag/package/release certification |
| AW-CAP-003 Init | CLI/config/init contracts | `Program.cs` | no temp-directory integration yet | build only | L2 | idempotency, no-overwrite, path safety, cross-platform scenario |
| AW-CAP-004 Risk analysis | risk/prompt contracts | `PromptRiskAnalyzer.cs` | broad and scoped unit cases pass | 8/8 tests on Linux/Windows + optimize smoke | L4 for current rule set | boundaries, corpus, golden stability |
| AW-CAP-005 Optimize CLI | CLI/prompt contracts | `Program.cs`, analyzer | scoped CLI smoke passes; analyzer tests pass | Linux/Windows smoke transcripts | L4 for current output | invalid input/file/encoding scenarios |
| AW-CAP-006 Broad split | prompt contracts | analyzer | broad multi-mode unit case passes | Linux/Windows TRX | L3 | broad-prompt black-box/golden scenario |
| AW-CAP-007 Git parser | git/test contracts | `GitCommandRunner.cs` parser | clean, modified/untracked, index/CRLF regression cases pass | Linux/Windows TRX, 8/8 | L3 | rename/delete/quoted/binary/path fixture + dirty-repo black-box |
| AW-CAP-008 Status CLI | CLI/git/adapter contracts | `Program.cs`, Git project | clean repository status smoke passes | Linux/Windows smoke | L3 | dirty/non-git/path-space scenarios |
| AW-CAP-009 Project detection | adapter contract | LanguageAdapters project | .NET and Flutter unit cases pass | Linux/Windows TRX | L3 | React/Python/Node/mixed fixtures |
| AW-CAP-010 Validation suggestions | adapter contract | LanguageAdapters project | current command-provider test passes | Linux/Windows TRX | L3 | mixed repo/no-auto-execution scenario |
| AW-CAP-012 Start | CLI/report contracts | not implemented | none | none | L1 | implementation |
| AW-CAP-013 Finish | CLI/report/evidence contracts | not implemented | none | none | L1 | implementation and closure gates |
| AW-CAP-014 Report | report/data contracts | not implemented | none | none | L1 | formatter/golden tests |
| AW-CAP-020 Mistake learning | learning contracts | manual docs only | manual workflow only | none | L1 | runtime parser/linter |
| AW-CAP-022 Discovery reconciliation | discovery contracts | manual docs only | docs dogfood record | none for runtime | L1 | AW-DISC implementation and scenarios |
| AW-CAP-025 Packaging | release/package contracts | CLI csproj tool config | AW-SCN-PACK-001/002 behavior: package, hash, isolated install/help/version | package artifact `8062492587`; checksum verified | L4 | signed/public release and independent verification |
| AW-CAP-026 Local-first/privacy | security/privacy contracts | partial architecture | no dedicated negative suite | none | L1 | no-network/path/secret/binary tests |
| AW-CAP-027 Proof bundle generation | proof contracts | `.github/workflows/ci.yml` | Linux/Windows TRX/smoke + package manifest/checksum generated | all three artifacts produced; checksum verified | L4 initial bundle | automatic schema/maturity validator and release retention |
| AW-CAP-028 Flight recorder | AW-OPP-01 + compatibility contracts | not implemented | timeline acceptance plus COMP-001..006, 016, 029..031, 048 | none | L1 | runtime profile, event adapters, import-only timeline, downgrade tests |
| AW-CAP-029 Trust ledger | AW-OPP-01 + compatibility contracts | not implemented | claim states plus COMP-001..010, 022..024, 034, 038..040, 049 | none | L1 | deterministic claim fixtures and evidence-grade engine |
| AW-CAP-030 Context/session rescue | AW-OPP-02 + compatibility contracts | not implemented | deterministic export plus COMP-001, 004..006, 021, 025..026, 032..033, 036 | none | L1 | interviews, manual snapshot prototype, target loss reports |
| AW-CAP-031 Rules compiler/drift | AW-OPP-02 + `ADAPTER_SPEC.md` | not implemented | no-overwrite and target-loss criteria plus COMP-001, 012, 021, 032, 036, 046 | none | L1 | target adapters, precedence model, golden/loss tests |
| AW-CAP-032 Cost/loop guard | AW-OPP-03 + compatibility contracts | not implemented | repeated-action/budget criteria plus COMP-003, 005, 008..010, 017, 025..027, 041..044 | none | L1 | offline analyzer, usage adapters, process ownership/checkpoint gates |
| AW-CAP-033 Policy firewall | AW-OPP-04 + compatibility contracts | not implemented | policy criteria plus COMP-002..016, 029..030, 037..038, 045, 049 | none | L1 | enforcement classes, threat model, dry-run engine, security review |
| AW-CAP-034 Multi-agent coordinator | AW-OPP-05 + compatibility contracts | not implemented | worktree/ownership criteria plus COMP-005, 007, 018..021, 035 | none | L1 | coordination-mode selector and local/cloud/shared fixtures |
| AW-CAP-035 PR review debt reducer | AW-OPP-06 + compatibility contracts | not implemented | scope/evidence criteria plus COMP-001, 005..006, 019, 021..024, 034, 039 | none | L1 | maintainer interviews and local/cloud PR fixture benchmark |
| AW-CAP-036 Regression canary | opportunity map + compatibility contracts | concept only | COMP-013, 017, 025..028, 041..042, 047 define comparison validity | none | L1 | dedicated runtime, repetitions, confounder and metrics implementation |
| AW-CAP-037 Runtime capability negotiation | `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`, `ADAPTER_SPEC.md` | not implemented | `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md` defines 50 scenarios | none | L1 | profile schema, adapter declarations/handshakes, detectors, decision/fallback engine, Linux/Windows black-box proof |

## Compatibility proof rule

The planned capabilities are semantically reusable but not equally observable or enforceable across models, tools, surfaces, permissions, and environments.

A feature proof is invalid when it:

- selects support solely by model/provider name;
- reports Full without the required runtime capabilities;
- treats missing events as proof that an action did not happen;
- calls advisory classification enforcement;
- compares canaries across changed environments without a confounder warning;
- converts unknown quota/cost units into invented token or currency values;
- uses local-worktree language for a cloud branch/PR flow;
- ignores managed policy, read-only mounts, remote execution, or missing credentials;
- fails to downgrade after adapter/hook failure.

AW-CAP-028 through AW-CAP-036 cannot move beyond L1 without an AW-CAP-037 support decision for every tested runtime profile.

## Community opportunity proof rule

The July 2026 research proves that problems are repeatedly reported across public sources. It does **not** prove market size, willingness to pay, runtime feasibility, provider event stability, equal tool support, security effectiveness, numerical savings, or popularity.

## Test result details for current implemented skeleton

Both Linux and Windows TRX files report:

```text
total: 8
executed: 8
passed: 8
failed: 0
```

Covered named tests:

- broad and scoped prompt-risk analysis;
- clean and changed Git status parsing;
- CRLF/leading status-column regression;
- .NET and Flutter project detection;
- validation command suggestions.

Smoke exit codes on both systems:

```text
help: 0
version: 0
optimize: 0
status: 0
unknownCommand: 2 (expected)
```

Package proof:

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

Release proof fails when an advertised capability lacks a registry/matrix row, implementation, required scenarios, compatibility support decision, commit match, checksum/clean-install proof, or dogfood evidence for value claims.
