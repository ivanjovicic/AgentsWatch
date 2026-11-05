# AW-VFY-001 evidence: Git parser and CI hardening

- Prompt: AW-VFY-001 (`docs/prompts/AW-VFY-001-git-parser-ci-hardening.md`)
- Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`
- Run mode: implementation + validation
- Model/client: unknown-not-exposed (Grok Bot executor, local Windows machine)
- Timing: unknown-not-recorded (single session 2026-11-05)
- SDK: dotnet 10.0.201, test target net8.0

## Root cause (confirmed by pre-change run)

`dotnet test` before change: Failed 1, Passed 6, Total 7. `Parse_ParsesModifiedAndUntrackedFiles`
got `EADME.md`: `Split(..., TrimEntries)` removed the leading space of the fixed-width XY prefix before `line[3..]`.

## Parser contract chosen

- Snapshot reader now runs `git status --porcelain=v1 -z -uall` and calls `GitStatusParser.ParsePorcelainZ`.
- NUL-delimited, unquoted paths; XY prefix sliced at fixed width, never trimmed; path not trimmed.
- Rename/copy: extra NUL token read as original path -> new optional `ChangedFile.OriginalPath`
  (backward-compatible trailing optional param in `src/AgentsWatch.Core/Models.cs`).
- Legacy line parser `Parse` kept, fixed (no TrimEntries, strips only `\r`, handles `old -> new`).
- Status value semantics unchanged (XY trimmed, blank -> `modified`).

## Files changed

- `src/AgentsWatch.Git/GitCommandRunner.cs`
- `src/AgentsWatch.Core/Models.cs`
- `tests/AgentsWatch.Tests/GitStatusParserTests.cs` (+4 tests: clean -z, all entry kinds incl. staged/unstaged M, A, D, ??, R with old path, MM, spaces; leading/trailing space preservation; line-format rename + CRLF)
- queue + prompt status, this log

## Validation (executed)

- Targeted: `--filter GitStatusParser` Passed 6/6.
- `dotnet restore AgentsWatch.sln`: exit 0
- `dotnet build AgentsWatch.sln -c Release --no-restore`: succeeded, 0 warnings, 0 errors
- `dotnet test AgentsWatch.sln -c Release --no-build`: Failed 0, Passed 11, Skipped 0, Total 11
- Smoke: `AgentsWatch.Cli status` reported 3 changed files on dirty tree correctly.

## Missed / limitations (for AW-VFY-005)

- No real-repo integration test with git process (unit tests on parser only).
- `RawStatus` now holds NUL-delimited text; Markdown reports use parsed list, not RawStatus.
- No pre-start/after-start attribution, no line counts; unmerged (UU etc.) passed through as raw XY.
- GitHub CI not re-run (not pushed).

## Mistakes

Mistake ledger not consulted (out of prompt read list). Observed: none new.

## Follow-up

AW-VFY-002 - CLI smoke/Gate 0 closure: Ready now.

Completion: 100% of prompt scope locally; Gate 0 confirmed by GitHub CI run 42071854837 on 9d3f459 (build-test success, 2026-11-05).
Commit SHA (fix): e4977ae