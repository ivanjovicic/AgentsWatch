# AW-VFY-004 evidence: start-run dirty-worktree baseline

- Prompt: AW-VFY-004; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Gate basis: AW-VFY-003 done locally; Gate 0 GitHub CI confirmation still pending (user chose to proceed without push).
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## Persisted baseline (`.agentwatch/active-runs/<run-id>.json`)

schemaVersion=1, runId (`<contract-id>-<yyyyMMddTHHmmssZ lowercase>-<6 hex>`), contractId, taskId, startedAtUtc, branch,
headCommitSha (null on unborn branch), worktreeState{porcelainVersion "v1-z", staged[], unstaged[], untracked[], stateFingerprint},
executorMetadata?{agent,tool,model} (omitted when none). Entry = {path, status, fingerprint, oldPath?}.

## Fingerprinting (no source contents stored)

- staged: index blob id + mode from `git ls-files -s -z` (`blob:<sha>:<mode>`); staged delete = `deleted`
- unstaged / untracked: `sha256:` of current worktree bytes; missing = `deleted`; directory/submodule = `dir`
- a file both staged and further edited (MM) appears in both lists with distinct fingerprints
- stateFingerprint: sha256 over canonical sorted (area,status,path,oldPath,fingerprint) tuples
- `.agentwatch/` tool-owned paths excluded from state
- AW-VFY-005 can classify a pre-existing dirty file as unchanged vs changed-further by fingerprint comparison

## Behavior

- contract loaded + linted; missing/invalid (errors) -> refused, exit 1, findings listed
- one active run only: any `active-runs/*.json` -> refused
- atomic write: temp `.<id>.<guid>.tmp` then `File.Move(overwrite:false)`; temp cleaned; temp never matches `*.json`
- architecture: `RunStartService` + `IRepositoryStateReader`/`IActiveRunStore` ports in Core; `GitRepositoryStateReader` adapter in Git; thin `Cli/StartCommand.cs`

## Files

new: `src/AgentsWatch.Core/Runs/RunBaseline.cs`, `src/AgentsWatch.Git/GitRepositoryStateReader.cs`, `src/AgentsWatch.Cli/StartCommand.cs`, `tests/AgentsWatch.Tests/RunStartTests.cs`, `tests/AgentsWatch.Tests/TempGitRepo.cs`; changed: `src/AgentsWatch.Cli/Program.cs` (route/help), queue/prompt status

## Tests (9 new, temp git repos)

clean start + branch/HEAD/run id/executor; staged vs unstaged vs untracked incl. MM and spaces in names; fingerprint stable and changes when dirty file changes further; staged delete + rename oldPath; unborn repo (null HEAD); missing + invalid contract; second active run refused; disk round trip + no temp files + no content/isDirty in JSON; CLI dirty report + refusal.

## Gate (executed)

restore exit 0; build Release 0 errors; test Release: Failed 0, Passed 47, Skipped 0, Total 47.
(Smoke run found computed `isDirty` serialized and contract warnings printed on non-contract refusal; both fixed, gate re-run: 47/47.)

## CLI smoke (temp repo)

- clean: `Run started: t-1-20261105t170657z-ef4a13` / `Worktree clean at start.` exit 0
- second start: `error: an active run already exists (...)` exit 1
- dirty: `Dirty at start: staged 0, unstaged 1, untracked 1 (recorded as pre-existing, not attributed to this run)` exit 0
- missing contract: `error: contract 'nope' is missing or invalid` + AWC000, exit 1

## Remaining for AW-VFY-005

finish/delta classification; concurrent-writer ambiguity; HEAD moved/commits during run; ignored files not tracked; line counts not captured; filename with newline untested; no explicit abort/cleanup of active run (manual delete only).

Commit SHA (change): b1c7f8d