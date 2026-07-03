# AgentsWatch Feature Capability Registry

Last aligned: 2026-07-03  
Status: canonical capability inventory

## Purpose

This registry is the authoritative list of what AgentsWatch claims to provide.

A feature not listed here must not be presented as supported. Maturity is valid only when linked evidence exists for the named commit or package.

Maturity levels are defined in `PROOF_AND_VERIFICATION_STRATEGY.md`.

## Current capability inventory

| ID | Capability | Surface | Current maturity | Current evidence | Main gap / next proof action |
|---|---|---|---|---|---|
| AW-CAP-001 | CLI help | `agentswatch --help` | L4 CI-verified | Linux/Windows smoke + isolated installed-tool help in workflow `28650547744` | independent release verification for L6 |
| AW-CAP-002 | CLI version | `agentswatch --version` | L4 CI-verified | Linux/Windows smoke + installed package reports `0.1.0` | tie future release tag/version and independent verification |
| AW-CAP-003 | Workspace initialization | `agentswatch init` | L2 Implemented | `src/AgentsWatch.Cli/Program.cs` | temp-directory integration, idempotency, no-overwrite, path-safety, cross-platform proof |
| AW-CAP-004 | Prompt risk analysis | core optimizer | L4 CI-verified for current basic rules | broad/scoped tests pass 8/8 on Linux and Windows; optimize smoke passes | boundary/golden cases and larger prompt corpus |
| AW-CAP-005 | Prompt optimization output | `agentswatch optimize` | L4 CI-verified for current output contract | Linux/Windows CLI smoke + analyzer tests in workflow `28650547744` | invalid file/encoding paths and golden output stability |
| AW-CAP-006 | Broad-task split recommendation | optimizer result | L3 Test-backed | broad multi-mode analyzer test passes on Linux/Windows | broad-prompt black-box/golden scenario |
| AW-CAP-007 | Git status parsing | core git parser | L3 Test-backed | modified/untracked/index/CRLF regression tests pass Linux/Windows | rename/delete/quoted/binary/path fixture coverage and dirty-repo scenario |
| AW-CAP-008 | Git snapshot/status display | `agentswatch status` | L3 Test-backed with clean-repo smoke | status smoke passes Linux/Windows | dirty/non-git/path-spaces integration scenarios |
| AW-CAP-009 | Project type detection | status/adapters | L3 Test-backed for .NET/Flutter | detector tests pass Linux/Windows | React/Python/Node/mixed fixtures |
| AW-CAP-010 | Validation command suggestions | status/adapters | L3 Test-backed for current .NET/Flutter rules | validation-command test passes Linux/Windows | mixed/scoped output and no-auto-execution scenarios |
| AW-CAP-011 | Task markdown generation | `agentswatch task split` | L1 Specified | CLI/product/test docs | implement after Gate 0, add no-overwrite and golden tests |
| AW-CAP-012 | Run start evidence | `agentswatch start` | L1 Specified | CLI/test/report contracts | implement and prove against temporary git repos |
| AW-CAP-013 | Run finish evidence | `agentswatch finish` | L1 Specified | CLI/test/report contracts | implement required-field/completion-gate integration tests |
| AW-CAP-014 | Markdown run report | `agentswatch report` | L1 Specified | report/data/test contracts | implement formatter and golden outputs |
| AW-CAP-015 | Handoff generation | `agentswatch handoff` | L1 Specified | report/test contracts | implement, enforce length and evidence fields |
| AW-CAP-016 | Diff-only review prompt | `agentswatch review-diff` | L1 Specified | report/test contracts | implement and prove changed-file-only scope |
| AW-CAP-017 | Validation runner | `agentswatch validate` | L1 Specified | CLI/test/adapter docs | implement opt-in execution, timeout/cancel/failure evidence |
| AW-CAP-018 | Command profiler | `agentswatch run --` | L1 Specified | profiler contracts/queue | gated implementation and privacy-safe output tests |
| AW-CAP-019 | Claims-vs-actual review | lint/review | L1 Specified/manual checklist | `CLAIMS_VS_ACTUAL_REVIEW.md` | deterministic rules and regression scenarios |
| AW-CAP-020 | Mistake ledger/list/check | `agentswatch mistakes ...` | L1 Specified; manual docs exist | learning specs/ledger | implement parser/lint and repeated-mistake regression suite |
| AW-CAP-021 | Evidence lint | `agentswatch lint evidence` | L1 Specified | evidence standards/prompts | implement deterministic linter and known-error fixture suite |
| AW-CAP-022 | Discovery capture/reconciliation | `agentswatch discover ...` | L1 Specified; manual workflow available | discovery contracts/prompts/inbox | implement AW-DISC slices and dogfood end-to-end |
| AW-CAP-023 | Discovery prompt generation | `agentswatch discover prompt` | L1 Specified | discovery CLI/prompt contracts | deterministic template generation and queue-link tests |
| AW-CAP-024 | Supervised prompt queue | manual/assisted queue | L1 Specified | autopilot docs/prompts | implement only after evidence, permission, and stop gates |
| AW-CAP-025 | Package as .NET tool | NuGet/local tool | L4 CI-verified | `AgentsWatch.Cli.0.1.0.nupkg`, SHA-256 `3bc0a9b2...30288`, isolated install/help/version in artifact `8062492587` | signed/public release and independent verification for L6 |
| AW-CAP-026 | Local-first/no telemetry default | whole product | L1 Specified | privacy/security/test docs | automated no-network/path/privacy negative tests and release audit |
| AW-CAP-027 | Proof bundle generation | CI/release evidence | L4 CI-verified for initial bundle | workflow `28650547744` produced Linux/Windows TRX/smoke artifacts and package manifest/checksum | automatic schema/maturity validator, safety suite, permanent release retention |
| AW-CAP-028 | Agent event flight recorder | `record`, `timeline`, `replay` | L1 Specified | community research + `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md` | validate event availability and implement import-only schema |
| AW-CAP-029 | Claim-to-evidence trust ledger | `evidence verify` | L1 Specified | false-completion research + epic contract | implement deterministic claim fixtures |
| AW-CAP-030 | Portable context snapshot and session rescue | `context snapshot/export`, `resume` | L1 Specified | context-loss research + epic contract | interview users and prototype fresh-session resume |
| AW-CAP-031 | Agent rules compiler and drift detector | `rules compile/diff/lint` | L1 Specified | cross-tool rules research + epic contract | validate target formats and no-overwrite ownership |
| AW-CAP-032 | Cost and loop guard | `budget`, `loops analyze`, `watch` | L1 Specified | usage/loop research + epic contract | implement offline action fingerprinting before live control |
| AW-CAP-033 | Policy firewall and safe execution broker | `policy`, optional execution wrapper | L1 Specified | scope/security research + epic contract | threat model and dry-run path/command rules |
| AW-CAP-034 | Multi-agent worktree coordinator | `swarm`, `worker`, `integrate check` | L1 Specified | multi-agent/worktree research + epic contract | validate workflow and build dry-run ownership planner |
| AW-CAP-035 | AI PR review debt reducer | `pr analyze/evidence/review-pack` | L1 Specified | agent-PR/review research + epic contract | maintainer interviews and deterministic review packet |
| AW-CAP-036 | Agent/model regression canary | `canary init/run/compare` | L1 Specified | regression research + opportunity map | define stable task suite and comparison metrics |
| AW-CAP-037 | Runtime capability negotiation and fallback planning | `compatibility detect/explain/compare` | L1 Specified | compatibility research, adapter contract, 50 acceptance scenarios | implement profile schema, detectors, handshake, support decision engine, and cross-surface fixtures |

## Community opportunity and compatibility status

The following documents establish problem, compatibility, and contract evidence only:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`;
- `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`;
- `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`;
- `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`;
- `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`.

They do not establish runtime implementation, test support, equal support across tools, popularity, market demand, savings, or security effectiveness.

Before AW-CAP-028 through AW-CAP-037 move beyond L1:

1. complete user/problem validation where required;
2. identify stable local inputs/events;
3. detect the actual model/tool/surface/permission/environment profile;
4. build import-only or dry-run prototypes;
5. add deterministic acceptance tests and downgrade/fallback tests;
6. prove local-first/privacy behavior;
7. update the traceability matrix;
8. run paired dogfood where usefulness is claimed.

## Current proof snapshot

Workflow run: `28650547744`  
PR merge commit tested: `fe0c92ac98d3d88fe1bb967385ed935fd3aa808c`  
Source branch head represented by that run: `2cc16f6297a3450c64f958402d0b1b3d6b670f30`  
SDK: `.NET 8.0.422`  
Tests: `8 executed, 8 passed` on Linux and Windows  
CLI smoke: help/version/optimize/status exit `0`; unknown command expected exit `2`  
Package: `AgentsWatch.Cli.0.1.0.nupkg`  
Package SHA-256: `3bc0a9b2acb20c200ffd749186a2697ac95e42a8497647c36234d1a79d330288`

The current branch contains later documentation-only descendants. Those descendants require their own CI run after PR #5 is retargeted to `main` before the whole PR head is called current-commit verified.

## Status interpretation

- `L1 Specified` proves contracts/prompts only.
- `L2 Implemented` proves source exists, not that behavior passed.
- `L3 Test-backed` requires executed targeted tests.
- `L4 CI-verified` requires commit-bound CI and relevant observable scenario/artifact.
- `L5 Dogfood-verified` requires real-repository usefulness evidence.
- `L6 Release-verified` requires packaged release proof and independent verification.

## Registry update rule

Update a row when runtime, acceptance, tests, CI, dogfood, package, release, compatibility, limitation, or deprecation evidence changes.

Every maturity increase must update the traceability matrix and cite exact evidence.

## Allowed wording now

```text
AgentsWatch has a CI-verified 0.1.0 CLI skeleton for help, version, basic prompt optimization/risk analysis, and clean-repository status output.
The CLI packs and installs successfully as a local .NET tool in CI.
Community and compatibility research has specified advanced control-plane opportunities and honest fallback modes, but these remain L1 planning contracts.
```

## Not allowed now

```text
All planned features work equally across every model and coding tool.
AgentsWatch automatically detects and enforces every tool permission or sandbox.
The community-derived capabilities are already implemented.
AgentsWatch has proven numerical savings or complete security protection.
AgentsWatch is production-ready or release-verified.
```
