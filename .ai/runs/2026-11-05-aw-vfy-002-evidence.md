# AW-VFY-002 evidence: CLI smoke and Gate 0 closure

- Prompt: AW-VFY-002; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`
- Run mode: validation-first, minimal hardening
- Model/client: unknown-not-exposed (Grok Bot executor, Windows local); timing: unknown-not-recorded
- SDK: dotnet 10.0.201, tests on net8.0

## Findings

1. `status` in a non-git dir failed: exit 1, `error: git rev-parse --abbrev-ref HEAD failed: fatal: not a git repository`. CLI_SPEC requires clear non-git handling. Fixed: prints `Git: not a git repository (or git unavailable); git status skipped.`, exit 0. Also prints `Project root:`.
2. `init` lacked CLI_SPEC dirs `.ai/handoffs`, `.agentwatch/contracts`, `.agentwatch/active-runs`, `.agentwatch/runs`. Added (empty dirs only). Existing `.ai/tasks`, `.ai/generated`, `.ai/config.yml` kept.

## Files changed

- `src/AgentsWatch.Cli/Program.cs` (init dirs; StatusCommand non-git guard + optional TextWriter)
- `src/AgentsWatch.Cli/AgentsWatch.Cli.csproj` (InternalsVisibleTo AgentsWatch.Tests)
- `tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj` (Cli project reference)
- `tests/AgentsWatch.Tests/CliSmokeTests.cs` (+4 tests: init layout/exact files/idempotent no-overwrite; non-git status; clean+dirty temp git repo; help/version/optimize/unknown exit codes)
- queue + prompt status, this log

## Smoke (Release exe, temp dir %TEMP%\aw-smoke2)

- `--help`: exit 0; `--version`: `AgentsWatch 0.1.0` exit 0; `optimize "fix login bug"`: Risk High / Budget Low, exit 0
- `status` non-git: message above, exit 0
- `init` twice: exit 0 both; user-edited `.ai/STATUS.md` stayed `mine`; tree = only expected dirs + 4 files
- `status` clean git repo: Changed files 0, exit 0; dirty (untracked `x y.txt`): Changed files 1, exit 0
- `dotnet pack src/AgentsWatch.Cli` (to temp, not published): created AgentsWatch.Cli.0.1.0.nupkg; global-tool install not performed

## Gate

- `dotnet restore AgentsWatch.sln`: exit 0
- `dotnet build AgentsWatch.sln -c Release`: 0 warnings, 0 errors
- `dotnet test AgentsWatch.sln -c Release`: Failed 0, Passed 15, Skipped 0, Total 15

## Gate 0

Closed locally per BOOTSTRAP_NEXT_STEPS definition. Later confirmed: Gate 0 confirmed by GitHub CI run 42071854837 on 9d3f459 (build-test success, 2026-11-05). Originally NOT confirmed on GitHub CI (commits e4977ae..3290a02 not pushed). AW-VFY-003 held at "Ready after CI confirms Gate 0".

## Missed / risk

- Status output still not full CLI_SPEC target (Active run / receipt fields belong to later prompts).
- Git-missing case shares non-git message (not separately tested).
- Mistake ledger not read (outside prompt read list).

Follow-up: push + CI run, then AW-VFY-003 (RunContract v1).
Commit SHA (change): 3290a02