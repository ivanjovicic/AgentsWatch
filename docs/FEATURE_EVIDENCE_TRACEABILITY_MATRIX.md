# AgentsWatch Feature Evidence Traceability Matrix

Last aligned: 2026-07-03  
Status: mandatory proof index

## Purpose

Map every claimed capability to its contract, implementation, acceptance criteria, automated tests, black-box scenarios, CI evidence, dogfood evidence, and release evidence.

A blank evidence column limits the capability maturity according to `PROOF_AND_VERIFICATION_STRATEGY.md`.

## Current traceability rows

| Capability | Contract | Implementation | Acceptance criteria | Automated tests | Acceptance scenario | CI evidence | Dogfood/release | Current gap |
|---|---|---|---|---|---|---|---|---|
| AW-CAP-001 Help | `CLI_SPEC.md`, `CLI_UX_OUTPUT_SPEC.md` | `src/AgentsWatch.Cli/Program.cs` | AW-AC-HELP-001..003 | missing direct CLI process test | AW-SCN-HELP-001 | missing for current commit | none | execute and archive smoke |
| AW-CAP-002 Version | CLI/version/release contracts | `Program.cs`, CLI csproj | AW-AC-VERSION-001..003 | missing direct CLI process test | AW-SCN-VERSION-001 | missing | none | match executable/package version |
| AW-CAP-003 Init | CLI/config/init contracts | `Program.cs` | AW-AC-INIT-001..008 | missing integration suite | AW-SCN-INIT-001..004 | missing | none | idempotency, no-overwrite, path safety |
| AW-CAP-004 Risk analysis | risk/prompt contracts | `PromptRiskAnalyzer.cs` | AW-AC-RISK-001..006 | `PromptRiskAnalyzerTests.cs` | AW-SCN-OPTIMIZE-001..003 | test execution missing | none | execute, add boundaries |
| AW-CAP-005 Optimize CLI | CLI/prompt contracts | `Program.cs`, analyzer | AW-AC-OPT-001..007 | core tests only | AW-SCN-OPTIMIZE-001..004 | missing CLI smoke | none | process/exit/output proof |
| AW-CAP-007 Git parser | git/test contracts | Git parser | AW-AC-GIT-001..009 | `GitStatusParserTests.cs` | AW-SCN-STATUS-001..004 | execution missing | none | rename/delete/binary/path fixtures |
| AW-CAP-008 Status CLI | CLI/git/adapter contracts | `Program.cs`, Git project | AW-AC-STATUS-001..007 | parser/detector unit sources | AW-SCN-STATUS-001..005 | missing | none | temp-repo and non-git proof |
| AW-CAP-009 Project detection | adapter contract | LanguageAdapters project | AW-AC-DETECT-001..006 | `ProjectTypeDetectorTests.cs` | AW-SCN-DETECT-001..006 | execution missing | none | React/Python/Node/mixed fixtures |
| AW-CAP-012 Start | CLI/report contracts | not implemented | AW-AC-START-001..007 | none | AW-SCN-START-001..004 | none | none | implementation |
| AW-CAP-013 Finish | CLI/report/evidence contracts | not implemented | AW-AC-FINISH-001..010 | none | AW-SCN-FINISH-001..006 | none | none | implementation and closure gates |
| AW-CAP-014 Report | report/data contracts | not implemented | AW-AC-REPORT-001..009 | none | AW-SCN-REPORT-001..004 | none | none | formatter/golden tests |
| AW-CAP-020 Mistake learning | learning contracts | manual docs only | AW-AC-LEARN-001..009 | none | AW-SCN-LEARN-001..005 | none | manual evidence only | runtime parser/linter |
| AW-CAP-022 Discovery reconciliation | discovery contracts | manual docs only | AW-AC-DISC-001..012 | none | AW-SCN-DISC-001..006 | none | one docs dogfood record | runtime AW-DISC implementation |
| AW-CAP-025 Packaging | release/package contracts | CLI csproj configured as tool | AW-AC-PACK-001..007 | none | AW-SCN-PACK-001..004 | missing | none | pack/install/checksum proof |
| AW-CAP-026 Local-first/privacy | security/privacy contracts | partial by architecture | AW-AC-PRIV-001..010 | none dedicated | AW-SCN-PRIV-001..006 | none | none | no-network/path/secret negative tests |
| AW-CAP-027 Proof bundle | proof contracts | CI enhancement planned | AW-AC-PROOF-001..010 | manifest lint planned | AW-SCN-PROOF-001..004 | none | none | implement artifact generation |

## Acceptance-criteria naming

Use stable IDs:

```text
AW-AC-<AREA>-<NNN>
```

Examples:

- `AW-AC-INIT-001` creates required directories;
- `AW-AC-INIT-002` preserves an existing user-edited file;
- `AW-AC-OPT-003` reports missing stop rules;
- `AW-AC-DISC-006` actionable discovery has one primary owner;
- `AW-AC-PROOF-004` proof manifest commit equals tested commit.

## Test and scenario naming

```text
AW-UT-<AREA>-<NNN>      unit
AW-IT-<AREA>-<NNN>      integration
AW-GOLD-<AREA>-<NNN>    golden/snapshot
AW-SAFE-<AREA>-<NNN>    safety/privacy
AW-REG-<AREA>-<NNN>     regression
AW-SCN-<AREA>-<NNN>     black-box acceptance
AW-DOG-<AREA>-<NNN>     dogfood
```

Test names or metadata should include the capability/acceptance ID where practical.

## Evidence update procedure

For each completed implementation prompt:

1. identify affected capability IDs;
2. update acceptance criteria;
3. add targeted automated tests;
4. add/update one black-box scenario;
5. run targeted and required broad validation;
6. link the CI run/artifact for the commit;
7. update registry maturity;
8. create discovery records for remaining gaps;
9. include the matrix diff in review.

## Release rule

A release proof review must fail when:

- an advertised capability is absent from this matrix;
- its implementation path is missing;
- required acceptance criteria have no tests/scenarios;
- CI evidence is for another commit;
- a release-level capability lacks clean-install evidence;
- a value claim lacks dogfood benchmark evidence.
