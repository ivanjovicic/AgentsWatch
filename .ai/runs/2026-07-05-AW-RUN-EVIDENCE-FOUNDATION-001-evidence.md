# AW-RUN-EVIDENCE-FOUNDATION-001 Evidence

Prompt/run ID: AW-RUN-EVIDENCE-FOUNDATION-001  
Date: 2026-07-05  
Branch: `feature/pr-evidence-run-foundation`  
Base: `research/community-opportunities-2026-07` at `561d2bed283b1dc163d806e307cf84ff45ec82c8`  
Implementation head before this evidence record: `f3851882a1f78f79aa537edf9c8500e77d1f7c3d`  
Capabilities: AW-CAP-007, AW-CAP-012, AW-CAP-013, AW-CAP-014; privacy prerequisite for AW-CAP-026  
Maturity asserted: L2 Implemented only

## Goal

Implement the first usable application slice beneath PR Evidence:

```text
agentswatch start
-> repository changes
-> agentswatch finish
-> agentswatch report
```

The slice must prove only Git/scope lifecycle evidence and must not claim command, build, test, CI, runtime, UI, database, or agent-claim evidence.

## Source implemented

### Core

`src/AgentsWatch.Core/RunEvidenceModels.cs`

- versioned `RunManifest` schema 1.0;
- `InProgress` / `Finished` lifecycle;
- `ValidationEvidenceStatus` with current default `NotRun`;
- full SHA-1/SHA-256 object ID validation;
- path-safe task ID;
- managed run-artifact classification;
- repository-relative glob matching.

### Git

`src/AgentsWatch.Git/GitChangeSetReader.cs`

- repository-root lookup;
- immutable-base change-set read;
- added/modified/deleted/type-changed/unmerged/renamed/copied parsing;
- untracked file inclusion;
- deterministic ordering;
- arbitrary revision text rejection.

### Reports/storage

`src/AgentsWatch.Reports/RunEvidenceReports.cs`

- `.agentwatch/runs/<task>.json` machine sidecar;
- `.ai/runs/<task>.md` human report;
- atomic create/replace;
- schema and required-field validation;
- active/latest manifest queries;
- deterministic Markdown;
- absolute local root omitted;
- explicit evidence boundary.

### CLI

`src/AgentsWatch.Cli/RunEvidenceCommands.cs`

- start/finish/report command behavior;
- duplicate and overlapping run refusal;
- clean non-AgentsWatch baseline requirement;
- previous managed artifacts ignored for attribution;
- out-of-scope findings;
- branch/uncommitted warnings;
- latest report default;
- `Validation: NotRun` output.

`src/AgentsWatch.Cli/Program.cs`

- thin dispatch and help integration.

## Tests authored

- `tests/AgentsWatch.Tests/RunEvidenceFoundationTests.cs`;
- `tests/AgentsWatch.Tests/RunArtifactPathTests.cs`.

Coverage authored for:

- path-safe IDs;
- scope globs and cross-platform separators;
- managed/current run artifacts;
- Git name-status parsing;
- tracked/untracked combination;
- invalid revision rejection;
- sidecar location/roundtrip/no-overwrite;
- active/latest lookup;
- unknown schema rejection;
- deterministic/privacy-safe/honest Markdown.

## Documentation aligned

- `RUN_EVIDENCE_FOUNDATION_CONTRACT.md`;
- `RUN_EVIDENCE_ACCEPTANCE_SCENARIOS.md`;
- `RUN_EVIDENCE_IMPLEMENTATION_BACKLOG.md`;
- `COMMAND_CONTRACTS.md`;
- `REPORT_FORMATS.md`;
- `DATA_MODEL.md`;
- `MVP_EPICS_AND_ACCEPTANCE.md`;
- `FEATURE_CAPABILITY_REGISTRY.md`;
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`.

## Validation attempted

### Local runtime

Command availability check:

```text
command -v dotnet
result: not found

dotnet --info
result: bash: dotnet: command not found
```

Result: BlockedByEnvironment.

No restore, build, test, CLI smoke, package, or black-box scenario was executed locally.

### GitHub Actions

The current workflow triggers only for pushes/PRs targeting `main`.

This branch is intentionally stacked on PR #5, which is stacked on PR #4, so no commit-bound workflow run exists for this implementation head.

Result: NotRun.

## Honest capability result

| Capability | Result | Reason |
|---|---|---|
| AW-CAP-012 Start evidence | L2 only | source exists; no executed test/CLI proof |
| AW-CAP-013 Finish evidence | L2 Git/scope slice only | source exists; no command validation evidence or execution proof |
| AW-CAP-014 Run report | L2 foundation only | formatter/storage source exists; no golden/black-box execution |
| AW-CAP-026 Privacy | no maturity increase | local-root omission assertions authored but not executed; broader privacy suite absent |
| AW-CAP-035 PR Evidence | remains L1 | run foundation is only a prerequisite; packet/claims/command evidence absent |

## Evidence boundary

Not implemented or proven:

- `agentswatch run -- <command>`;
- command duration/exit/output evidence;
- build/test/CI evidence;
- stale validation detection;
- structured claims or Trust Ledger;
- PR Evidence Packet;
- provider adapters/events;
- market usefulness or time savings.

## Required next proof

1. restore/build/test current branch with .NET 8;
2. fix compile/test failures without weakening contracts;
3. execute `RUN_EVIDENCE_ACCEPTANCE_SCENARIOS.md` on Linux and Windows;
4. add start/finish/report smoke transcripts and exit-code evidence;
5. package and clean-install current head;
6. update registry/matrix to L3/L4 only from commit-matched artifacts;
7. dogfood one real run before starting command-evidence implementation.

## Next implementation after proof

`CMD-001` and `CMD-002` in `RUN_EVIDENCE_IMPLEMENTATION_BACKLOG.md`:

```text
CommandEvidence model
-> explicit agentswatch run -- <command>
-> redaction/bounds/timeout
-> Git object binding
-> compact validation evidence
```
