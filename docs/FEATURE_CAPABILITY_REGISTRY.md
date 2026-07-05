# AgentsWatch Feature Capability Registry

Last aligned: 2026-07-05  
Status: canonical capability inventory

## Purpose

This registry is the authoritative list of what AgentsWatch claims to provide.

A feature not listed here must not be presented as supported. Maturity is valid only when linked evidence exists for the named commit or package.

Maturity levels are defined in `PROOF_AND_VERIFICATION_STRATEGY.md`.

Market priority does not change capability maturity. A high-priority opportunity remains L1 until implementation and proof exist.

## Current capability inventory

| ID | Capability | Surface | Current maturity | Current evidence | Main gap / next proof action |
|---|---|---|---|---|---|
| AW-CAP-001 | CLI help | `agentswatch --help` | L4 CI-verified for prior tested head | Linux/Windows smoke + isolated installed-tool help in workflow `28650547744` | rerun on current implementation head; independent release verification for L6 |
| AW-CAP-002 | CLI version | `agentswatch --version` | L4 CI-verified for prior tested head | Linux/Windows smoke + installed package reports `0.1.0` | rerun on current implementation head; tie future release tag/version and independent verification |
| AW-CAP-003 | Workspace initialization | `agentswatch init` | L2 Implemented | `src/AgentsWatch.Cli/Program.cs` | temp-directory integration, idempotency output, no-overwrite, path-safety, cross-platform proof |
| AW-CAP-004 | Prompt risk analysis | core optimizer | L4 CI-verified for prior tested basic rules | broad/scoped tests pass 8/8 on Linux and Windows; optimize smoke passes | rerun on current head, boundary/golden cases, larger prompt corpus |
| AW-CAP-005 | Prompt optimization output | `agentswatch optimize` | L4 CI-verified for prior tested output contract | Linux/Windows CLI smoke + analyzer tests in workflow `28650547744` | rerun on current head, invalid file/encoding paths, golden output stability |
| AW-CAP-006 | Broad-task split recommendation | optimizer result | L3 Test-backed for prior tested head | broad multi-mode analyzer test passed on Linux/Windows | rerun on current head; broad-prompt black-box/golden scenario |
| AW-CAP-007 | Git status parsing | core git parser | L3 Test-backed for prior tested head | modified/untracked/index/CRLF regression tests passed Linux/Windows | rerun on current head; quoted/binary/path fixture coverage and dirty-repo scenario |
| AW-CAP-008 | Git snapshot/status display | `agentswatch status` | L3 Test-backed with prior clean-repo smoke | status smoke passed Linux/Windows | rerun on current head; dirty/non-git/path-spaces integration scenarios |
| AW-CAP-009 | Project type detection | status/adapters | L3 Test-backed for prior .NET/Flutter head | detector tests passed Linux/Windows | rerun on current head; React/Python/Node/mixed fixtures |
| AW-CAP-010 | Validation command suggestions | status/adapters | L3 Test-backed for prior .NET/Flutter rules | validation-command test passed Linux/Windows | rerun on current head; mixed/scoped output and no-auto-execution scenarios |
| AW-CAP-011 | Task markdown generation | `agentswatch task split` | L1 Specified | CLI/product/test docs | implement after foundation proof, add no-overwrite and golden tests |
| AW-CAP-012 | Run start evidence | `agentswatch start` | L2 Implemented on feature branch | `RunEvidenceModels.cs`, `RunEvidenceReports.cs`, `RunEvidenceCommands.cs`; clean baseline, active-run, duplicate-ID, schema and atomic-write guards | execute unit and black-box tests on Linux/Windows; prove clean/dirty/overlap/no-overwrite scenarios |
| AW-CAP-013 | Run finish evidence | `agentswatch finish` | L2 Implemented for Git/scope-only slice | start/end object IDs, branch, changed files, out-of-scope files, uncommitted-state warnings, self-artifact exclusion, explicit `Validation: NotRun` | execute tests and black-box scenarios; add command/build/test/CI evidence and completion gates |
| AW-CAP-014 | Markdown run report | `agentswatch report [task-id]` | L2 Implemented for foundation report | JSON sidecar in `.agentwatch/runs`, Markdown in `.ai/runs`, latest-run lookup, deterministic file order, local-root omission, evidence-boundary text | execute formatter/store tests and golden CLI scenarios; add full report sections without overstating evidence |
| AW-CAP-015 | Handoff generation | `agentswatch handoff` | L1 Specified | report/test contracts | implement after run report proof; enforce length and evidence fields |
| AW-CAP-016 | Diff-only review prompt | `agentswatch review-diff` | L1 Specified | report/test contracts | implement and prove changed-file-only scope |
| AW-CAP-017 | Validation runner | `agentswatch validate` | L1 Specified | CLI/test/adapter docs | implement opt-in execution, timeout/cancel/failure evidence |
| AW-CAP-018 | Command profiler | `agentswatch run --` | L1 Specified | profiler contracts/queue | next vertical slice: explicit command execution, redaction, commit binding, compact evidence tests |
| AW-CAP-019 | Claims-vs-actual review | lint/review | L1 Specified/manual checklist | `CLAIMS_VS_ACTUAL_REVIEW.md` | deterministic claim classification, commit identity, and regression scenarios |
| AW-CAP-020 | Mistake ledger/list/check | `agentswatch mistakes ...` | L1 Specified; manual docs exist | learning specs/ledger | implement parser/lint and repeated-mistake regression suite |
| AW-CAP-021 | Evidence lint | `agentswatch lint evidence` | L1 Specified | evidence standards/prompts | implement deterministic linter and known-error fixture suite |
| AW-CAP-022 | Discovery capture/reconciliation | `agentswatch discover ...` | L1 Specified; manual workflow available | discovery contracts/prompts/inbox | implement AW-DISC slices and dogfood end-to-end |
| AW-CAP-023 | Discovery prompt generation | `agentswatch discover prompt` | L1 Specified | discovery CLI/prompt contracts | deterministic template generation and queue-link tests |
| AW-CAP-024 | Supervised prompt queue | manual/assisted queue | L1 Specified | autopilot docs/prompts | implement only after evidence, permission, and stop gates |
| AW-CAP-025 | Package as .NET tool | NuGet/local tool | L4 CI-verified for prior tested package | `AgentsWatch.Cli.0.1.0.nupkg`, SHA-256 `3bc0a9b2...30288`, isolated install/help/version in artifact `8062492587` | package and clean-install current run-evidence head before claiming commands ship |
| AW-CAP-026 | Local-first/no telemetry default | whole product | L1 Specified | privacy/security/test docs; current run slice has no network dependency in source | automated no-network/path/privacy negative tests and release audit |
| AW-CAP-027 | Proof bundle generation | CI/release evidence | L4 CI-verified for prior initial bundle | workflow `28650547744` produced Linux/Windows TRX/smoke artifacts and package manifest/checksum | generate bundle for current head; add schema/maturity validator, safety suite, permanent release retention |
| AW-CAP-028 | Agent event flight recorder | `record`, `timeline`, `replay` | L1 Specified | community research + epic contract | validate event availability and implement import-only schema after evidence spine |
| AW-CAP-029 | Claim-to-evidence Trust Ledger and completion integrity | `evidence verify`, claim classification | L1 Specified | false-completion research + market synthesis + epic contract | define deterministic statuses/fixtures and link claims to Git/command/CI evidence |
| AW-CAP-030 | Portable context snapshot and session rescue | `context snapshot/export`, `resume` | L1 Specified | context-loss research + market synthesis + epic contract | prototype fresh-session/cross-tool resume with explicit loss report |
| AW-CAP-031 | Agent rules compiler, target-loss report, and drift detector | `rules compile/diff/lint` | L1 Specified | cross-tool rules research + market synthesis + epic contract | validate target formats, semantic-loss statuses, precedence, and no-overwrite ownership |
| AW-CAP-032 | Offline cost/loop/waste analysis and later guard | `waste`, `loops`, optional `watch` | L1 Specified | usage/loop research + market synthesis + epic contract | implement observable offline action fingerprinting after command evidence; no live control or exact cost claims |
| AW-CAP-033 | Policy dry-run and optional safe execution broker | `policy`, optional execution wrapper | L1 Specified | scope/security research + epic contract | threat model, effective-permission report, and dry-run rules before enforcement naming |
| AW-CAP-034 | Multi-agent workspace/ownership diagnostics and later coordinator | `coordination`, `worker`, `integrate check` | L1 Specified | multi-agent/worktree research + market synthesis + epic contract | validate workflow and build dry-run workspace/ownership/stale-state diagnostics |
| AW-CAP-035 | PR Evidence Packet and review debt reducer | `pr evidence/analyze/review-pack` | L1 Specified; run-evidence prerequisite partially implemented | agent-PR/review research + market synthesis + epic contract; AW-CAP-012..014 feature branch foundation | add command and claim evidence, define deterministic packet, run 30-real-PR experiment, measure decision impact/repeat use |
| AW-CAP-036 | Agent/model regression canary | `canary init/run/compare` | L1 Specified | regression research + opportunity map | define stable task suite and full-profile comparison metrics |
| AW-CAP-037 | Runtime capability negotiation and fallback planning | `compatibility detect/explain/compare` | L1 Specified | compatibility research, adapter contract, 50 acceptance scenarios | implement profile schema, detectors, handshake, support decision engine, and cross-surface fixtures |

## Market priority versus implementation dependency

Current market-experiment order:

1. AW-CAP-035 PR Evidence Packet;
2. AW-CAP-029 Trust Ledger/completion integrity;
3. AW-CAP-030 Context Snapshot/Resume;
4. AW-CAP-031 Rules Compiler/target-loss report;
5. AW-CAP-032 offline Loop/Waste Analyzer.

Implementation order begins with the run/Git/report spine now represented by AW-CAP-012 through AW-CAP-014, then explicit command evidence (AW-CAP-018/AW-CAP-017), then claims and the PR packet.

No row advances beyond L2 merely because source and unexecuted tests exist.

## Community opportunity, market, and compatibility status

The following documents establish problem, market-hypothesis, compatibility, and contract evidence only:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`;
- `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`;
- `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`;
- `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`;
- `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`;
- `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`.

They do not establish runtime implementation, test support, equal support across tools, popularity, market size, willingness to pay, savings, security effectiveness, or product-market fit.

Before AW-CAP-028 through AW-CAP-037 move beyond L1:

1. complete user/problem validation where required;
2. identify stable local inputs/events;
3. detect the actual model/tool/surface/permission/environment profile;
4. build manual, import-only, or dry-run prototypes;
5. add deterministic acceptance tests and downgrade/fallback tests;
6. prove local-first/privacy behavior;
7. update the traceability matrix;
8. run paired dogfood where usefulness is claimed.

Before AW-CAP-035 is called market-validated:

- complete the 30-real-PR experiment;
- measure decision impact and false positives;
- demonstrate repeat use;
- obtain paid or explicitly budgeted pilot evidence.

## Current proof snapshot

The last executed proof remains workflow run `28650547744` for an earlier source head:

- tested PR merge commit: `fe0c92ac98d3d88fe1bb967385ed935fd3aa808c`;
- represented source head: `2cc16f6297a3450c64f958402d0b1b3d6b670f30`;
- SDK: `.NET 8.0.422`;
- tests: 8 executed, 8 passed on Linux and Windows;
- CLI smoke: help/version/optimize/status exit `0`; unknown command expected exit `2`;
- package: `AgentsWatch.Cli.0.1.0.nupkg`;
- package SHA-256: `3bc0a9b2acb20c200ffd749186a2697ac95e42a8497647c36234d1a79d330288`.

The run-evidence implementation on `feature/pr-evidence-run-foundation` has source and test code but no executed local or CI validation yet. It must not inherit L3/L4 from the earlier proof snapshot.

## Status interpretation

- `L1 Specified` proves contracts/prompts only.
- `L2 Implemented` proves source exists, not that behavior passed.
- `L3 Test-backed` requires executed targeted tests.
- `L4 CI-verified` requires commit-bound CI and relevant observable scenario/artifact.
- `L5 Dogfood-verified` requires real-repository usefulness evidence.
- `L6 Release-verified` requires packaged release proof and independent verification.

## Registry update rule

Update a row when runtime, acceptance, tests, CI, dogfood, package, release, compatibility, limitation, deprecation, or market-validation evidence changes.

Every maturity increase must update the traceability matrix and cite exact evidence.

Market-validation outcomes may change priority or positioning without changing technical maturity.

## Allowed wording now

```text
AgentsWatch has a CI-verified 0.1.0 CLI skeleton for help, version, basic prompt optimization/risk analysis, and clean-repository status output on an earlier tested commit.
A feature branch implements the first Git/scope-only run evidence slice for start, finish, and report, but it is not test-backed or CI-verified yet.
External research identifies completion integrity, review debt, context loss, fragmented rules, and repeated work as strong problem signals.
PR Evidence, Trust Ledger, context, rules, loop, policy, coordination, and compatibility features remain planning contracts except for the explicitly identified run-evidence prerequisite slice.
```

## Not allowed now

```text
The new start, finish, or report commands are verified or shipped.
AgentsWatch captures build, test, CI, runtime, or agent-claim evidence.
AgentsWatch PR Evidence is implemented or proven useful.
AgentsWatch has product-market fit or proven willingness to pay.
All planned features work equally across every model and coding tool.
AgentsWatch automatically detects and enforces every tool permission or sandbox.
AgentsWatch has proven numerical savings or complete security protection.
AgentsWatch is production-ready or release-verified.
```
