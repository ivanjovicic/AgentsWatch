# AgentsWatch Feature Capability Registry

Last aligned: 2026-07-03  
Status: canonical capability inventory

## Purpose

This registry is the authoritative list of what AgentsWatch claims to provide.

A feature not listed here must not be presented as supported. A maturity level is valid only when the linked evidence exists for the same commit or release.

Maturity levels are defined in `PROOF_AND_VERIFICATION_STRATEGY.md`.

## Current capability inventory

| ID | Capability | Surface | Current maturity | Current evidence | Main gap / next proof action |
|---|---|---|---|---|---|
| AW-CAP-001 | CLI help | `agentswatch --help` | L2 Implemented | `src/AgentsWatch.Cli/Program.cs` | run black-box smoke in CI and assert output/exit code |
| AW-CAP-002 | CLI version | `agentswatch --version` | L2 Implemented | `src/AgentsWatch.Cli/Program.cs` | run black-box smoke in CI and match package/version contract |
| AW-CAP-003 | Workspace initialization | `agentswatch init` | L2 Implemented | `src/AgentsWatch.Cli/Program.cs` | temp-directory integration, idempotency, no-overwrite, path-safety, cross-platform proof |
| AW-CAP-004 | Prompt risk analysis | core optimizer | L2 Implemented; test source exists | `PromptRiskAnalyzer.cs`, `PromptRiskAnalyzerTests.cs` | execute tests for current commit and add boundary/golden cases |
| AW-CAP-005 | Prompt optimization output | `agentswatch optimize` | L2 Implemented | CLI + core code | CLI process test, stable output anchors, invalid-input paths |
| AW-CAP-006 | Broad-task split recommendation | optimizer result | L2 Implemented; test source exists | analyzer tests | prove generated split/fields through golden and black-box tests |
| AW-CAP-007 | Git status parsing | core git parser | L2 Implemented; test source exists | `GitStatusParserTests.cs` | execute tests and add rename/delete/binary/path cases |
| AW-CAP-008 | Git snapshot/status display | `agentswatch status` | L2 Implemented | CLI/Git code | temp git repo integration and non-git behavior |
| AW-CAP-009 | Project type detection | status/adapters | L2 Implemented; test source exists | `ProjectTypeDetectorTests.cs` | execute tests and add React/Python/Node/mixed fixtures |
| AW-CAP-010 | Validation command suggestions | status/adapters | L2 Implemented; test source exists | detector/provider tests | execute tests, verify scoped mixed-repo output |
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
| AW-CAP-025 | Package as .NET tool | NuGet/local tool | L1 Specified | csproj/release plan | pack, checksum, clean install, version/help smoke |
| AW-CAP-026 | Local-first/no telemetry default | whole product | L1 Specified | privacy/security/test docs | automated network/path/privacy negative tests and release audit |
| AW-CAP-027 | Proof bundle generation | CI/release evidence | L1 Specified | proof strategy/spec/CI plan | implement CI artifact production and manifest verification |

## Status interpretation

- `L1 Specified` means contracts or prompts exist; runtime behavior must not be claimed.
- `L2 Implemented` means source code exists; current commit still requires executed proof.
- `test source exists` means a test file was inspected, not that it passed.
- Gate 0 remains incomplete until restore/build/test and CLI smoke evidence is visible.

## Registry update rule

Update a row when:

- runtime code is added or removed;
- acceptance criteria change;
- targeted tests are added or executed;
- CI verifies a capability on a commit;
- dogfood or release evidence is added;
- a known limitation changes;
- a capability is deprecated or split.

Every maturity increase must include a traceability-matrix update and evidence path.

## Claim examples

Allowed now:

```text
AgentsWatch currently contains an early CLI skeleton with init, optimize, status, help, and version code.
Prompt risk analysis, git status parsing, and basic project-type detection have unit-test source.
The broader learning, discovery, report, handoff, validation, and queue capabilities are specified but not yet runtime-verified.
```

Not allowed now:

```text
AgentsWatch fully validates agent work automatically.
AgentsWatch has proven 30-50% token savings.
All listed CLI commands work.
AgentsWatch is production-ready.
```
