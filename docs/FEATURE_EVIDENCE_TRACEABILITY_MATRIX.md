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
| AW-CAP-028 Flight recorder | `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md` AW-OPP-01 | not implemented | acceptance criteria defined; no tests/scenarios | none | L1 | validate event sources and implement import-only schema |
| AW-CAP-029 Trust ledger | AW-OPP-01 claim verifier contract | not implemented | supported/contradicted/missing/not-verifiable cases specified | none | L1 | deterministic claim fixture suite |
| AW-CAP-030 Context/session rescue | AW-OPP-02 | not implemented | deterministic export and lost-rule criteria specified | none | L1 | interviews, manual snapshot prototype, paired resume study |
| AW-CAP-031 Rules compiler/drift | AW-OPP-02 | not implemented | no-overwrite, deterministic output, unsupported-target criteria specified | none | L1 | target format research and fixture/golden tests |
| AW-CAP-032 Cost/loop guard | AW-OPP-03 | not implemented | repeated-action, budget, checkpoint, false-positive criteria specified | none | L1 | offline analyzer corpus and provider event adapters |
| AW-CAP-033 Policy firewall | AW-OPP-04 | not implemented | path, precedence, approval, traversal, and secret-path criteria specified | none | L1 | threat model, dry-run implementation, security review |
| AW-CAP-034 Multi-agent coordinator | AW-OPP-05 | not implemented | worktree, ownership, status, integration criteria specified | none | L1 | workflow interviews and synthetic worktree scenarios |
| AW-CAP-035 PR review debt reducer | AW-OPP-06 | not implemented | scope/evidence packet and commit-match criteria specified | none | L1 | maintainer interviews and local fixture benchmark |
| AW-CAP-036 Regression canary | `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md` | concept only | no acceptance suite yet | none | L1 | dedicated contract, stable canary tasks, comparison metrics |

## Community opportunity proof rule

The July 2026 research proves that the problems are repeatedly reported across public sources. It does **not** prove:

- market size;
- willingness to pay;
- runtime feasibility;
- provider event stability;
- security effectiveness;
- numerical savings;
- product popularity.

AW-CAP-028 through AW-CAP-036 may move to L2 only after code exists. They may move to L3/L4 only after the specific tests and black-box scenarios in the epic contracts pass.

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
4. add/update black-box scenarios;
5. run required validation;
6. link commit-matched CI/package artifacts;
7. raise maturity only to the proven level;
8. create discoveries for remaining gaps.

## Release rule

Release proof fails when an advertised capability lacks a registry/matrix row, implementation, required scenarios, commit match, checksum/clean-install proof, or dogfood evidence for value claims.
