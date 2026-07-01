# AgentsWatch Test Strategy

Last aligned: 2026-06-30  
Status: planning/specification  
Scope: AgentsWatch CLI/core implementation tests  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/AGENTWATCH_TEST_STRATEGY.md`

AgentsWatch must be safe to run on real repositories.

Main testing goal:

```text
Prove that AgentsWatch reads local project state, writes only intended local artifacts, reports evidence honestly, and never uploads source code or run logs by default.
```

## Test layers

| Layer | Purpose | Examples |
|---|---|---|
| Unit tests | Pure parsing/scoring/formatting behavior | config parser, risk scoring, report formatter |
| Fixture tests | Stack/repo detection and file layout behavior | Flutter fixture, .NET fixture, mixed repo fixture |
| Integration tests | CLI command runs against temp git repos | init, start, finish, report, review-diff |
| Golden/snapshot tests | Stable markdown/JSON outputs | run report, handoff, review prompt, mistake card |
| Safety tests | Prevent destructive or privacy-risky behavior | no overwrite, no outside-repo writes, no telemetry |
| Regression tests | Lock known mistakes and lint behavior | Done row missing run log, missing validation, score-cap contradiction |

## Required test infrastructure

Future implementation should include:

```text
tests/
  AgentsWatch.Cli.Tests/
  AgentsWatch.Core.Tests/
  AgentsWatch.Git.Tests/
  AgentsWatch.Reports.Tests/
  AgentsWatch.LanguageAdapters.Tests/
  AgentsWatch.Learning.Tests/
  fixtures/
    flutter_minimal/
    dotnet_minimal/
    react_typescript_minimal/
    python_minimal/
    node_minimal/
    mixed_repo/
    git_repo_clean/
    git_repo_dirty/
    git_repo_untracked/
    run_logs/
    prompt_inputs/
    reports_expected/
```

Fixture repositories must be tiny and contain no real secrets, no private code, and no generated dependency folders.

## Core command coverage

### `agentswatch --help` and `agentswatch --version`

Required tests:

- help exits successfully;
- version exits successfully;
- unknown command returns user-error exit code;
- missing required argument returns user-error exit code;
- errors go to stderr;
- machine-readable output is stable when `--json` is used.

### `agentswatch init`

Required tests:

- creates `.ai/` and `.agentwatch/` structure;
- creates config and templates;
- creates missing folders if partially initialized;
- running twice is idempotent;
- does not overwrite user-edited files;
- `--dry-run` writes nothing;
- `--force` overwrites only documented safe files;
- `--link-existing-learning-docs` links/detects existing `docs/ai/learning/*` instead of duplicating;
- works from repo root and subdirectory;
- rejects writes outside the selected repo.

### `agentswatch start`

Required tests:

- records task id, branch, start commit, clean/dirty/untracked state;
- handles detached HEAD clearly;
- rejects missing task id;
- creates run file in a stable path.

### `agentswatch finish`

Required tests:

- records end state, changed files, diff stat, validation status, missed work, residual risk;
- handles no changes;
- handles validation not run as `not-run`, not success;
- does not mark run complete if required evidence is missing unless status is `blocked` or `needs-handoff`.

### `agentswatch report`

Required tests:

- renders markdown report with required sections;
- renders optional JSON report with schema version;
- preserves unknown values as `unknown-not-recorded` or `unknown-not-exposed`;
- sorts changed files deterministically;
- produces stable output for golden fixtures.

### `agentswatch handoff`

Required tests:

- includes task, status, changed files, validation, blocked items, residual risk, and next minimal prompt;
- excludes full diff by default;
- stays below configured length budget;
- reads latest run by default;
- can target a specific run id.

### `agentswatch review-diff`

Required tests:

- uses changed files from commit/range;
- includes diff stat;
- generates review-only prompt;
- forbids whole-repo scan unless symbol/contract lookup is needed;
- handles empty diff;
- handles uncommitted diff if supported.

## Prompt optimizer and splitter coverage

Required tests:

- broad prompt is split into investigate/implement/test/review tasks;
- generated tasks include run mode, token budget, scope limiter, owned paths, avoid paths, stop rules, validation, and final response format;
- generated tasks do not include unrelated repos;
- generated review prompt is review-only;
- optimizer refuses unsupported destructive requests;
- optimizer keeps user-provided repository path and does not invent a different repo;
- generated prompt length stays under configured max lines.

## Git/diff tracker coverage

Required tests:

- clean repo;
- dirty tracked file;
- untracked file;
- renamed file;
- deleted file;
- binary file listed without trying to inline contents;
- large diff summarized safely;
- repo with spaces in path;
- command executed from subdirectory;
- git unavailable produces clear error.

## Risk scoring coverage

Required tests:

- auth/security files raise risk;
- migration/schema files raise risk;
- config/secrets patterns raise risk;
- provider/state files raise risk;
- backend API contract files raise risk;
- many files changed raises risk;
- no tests changed raises risk when runtime files changed;
- docs-only changes do not get runtime-risk warnings;
- risk reasons are stable and explainable;
- unknown stack still produces universal git/file risk score.

## Validation/adapters coverage

Required tests:

- Flutter detection from `pubspec.yaml`, `lib/`, `test/`;
- .NET detection from `.sln`, `.csproj`, `*.cs`;
- React/TypeScript detection from `package.json`, `src/`, `*.tsx`;
- Python detection from `pyproject.toml`, `requirements.txt`, `*.py`;
- Node detection without React assumptions;
- mixed repo returns scoped backend/frontend commands;
- validation suggestions are shown before execution;
- validation execution is opt-in;
- failed validation is recorded as failed, not skipped or passed;
- timeout/cancelled validation is recorded explicitly.

## Mistake-learning coverage

Required tests:

- mistake ledger is detected;
- mistake card template is created;
- run log includes prior mistakes section;
- run log includes observed mistakes section;
- `mistakes list` returns ids, titles, severity, status;
- `mistakes check` rejects unclassified observed mistakes;
- `mistakes add --from-run` avoids duplicate IDs;
- repeated mistake without prevention update fails learning lint;
- false alarm with explanation passes learning lint;
- rollup over last N logs finds repeated categories;
- rollup output creates next prevention prompt when needed.

## Evidence lint coverage

Required tests:

- Done row without run log fails;
- referenced run log missing fails;
- model/client blank fails unless explicit `unknown-not-exposed`;
- elapsed time blank fails unless explicit `unknown-not-recorded`;
- validation claimed without command evidence fails;
- residual risk says target evidence missing while completion is 100% fails;
- docs-only completion cannot claim runtime fixed;
- missing follow-up for missed work fails;
- clean evidence passes.

## Safety and privacy coverage

Required tests:

- no network calls by default;
- no telemetry endpoint configured by default;
- no source-code upload behavior exists in local MVP;
- reports stay local;
- command output does not print secrets from config fixtures;
- binary files are not inlined;
- files outside repo are not written;
- destructive overwrite requires explicit flag;
- generated reports warn that diffs/paths may be sensitive local artifacts.

## Golden output contracts

Golden tests should exist for:

- default config;
- run report markdown;
- run report JSON;
- handoff summary;
- diff-only review prompt;
- mistake card;
- rollup report;
- validation failure report;
- risk report.

Golden files must avoid timestamps or use fixed clocks.

## Recommended test order

1. Config schema and CLI UX tests.
2. Init idempotency and path safety tests.
3. Git status/diff tracker tests.
4. Run report and handoff golden tests.
5. Risk scoring tests.
6. Prompt optimizer/splitter tests.
7. Validation adapter fixture tests.
8. Claimed-vs-actual tests.
9. Mistake-learning tests.
10. Evidence/learning lint regression tests.
11. Packaging smoke tests.

## Minimum release gate for AgentsWatch CLI MVP

Before calling AgentsWatch CLI MVP usable:

- all core commands have at least one happy-path and one failure-path test;
- `init` is idempotent and overwrite-safe;
- git tracker works on clean/dirty/untracked fixtures;
- reports have golden tests;
- validation execution is opt-in;
- no telemetry/network behavior exists by default;
- lint catches the known evidence mistakes defined in `MISTAKE_LEARNING_SPEC.md`;
- dogfood report exists for MathLearning or another real repository.
