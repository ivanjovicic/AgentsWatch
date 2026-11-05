# AW-VFY-005 evidence: finish-run interval delta

- Prompt: AW-VFY-005; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Gate basis: AW-VFY-004 done; Gate 0 CI-confirmed (run 42071854837 on 9d3f459). This commit not pushed.
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## Algorithm (`RunDeltaCalculator`, pure/deterministic)

Per path in union(start dirty, end dirty, paths changed by commits start..end HEAD):
1. HEAD non-fast-forward (reset/rewrite/unrelated branch, or HEAD lost) -> every path `Ambiguous`.
2. clean at start:
   - end entry renamed from a path dirty at start -> `Ambiguous`
   - dirty at end and/or committed during interval -> `RunIntervalChange` (reason says where; "authorship not proven")
3. dirty at start:
   - renamed away during interval -> `Ambiguous`
   - included in a commit made during interval -> `Ambiguous` (pre-existing vs interval content inseparable)
   - clean at end -> `PreExistingChangedFurther` (reverted/discarded)
   - per-area (staged/unstaged/untracked) status+fingerprint+oldPath identical -> `PreExistingUnchanged`; else `PreExistingChangedFurther` naming changed areas
HEAD transitions: Unchanged, FastForward (`merge-base --is-ancestor` + `diff --name-status -z --no-renames`), UnbornToCommit (`ls-tree`), NonFastForward. Branch change recorded (`branchChanged`).

## Causality policy

Persisted in every record (`attributionPolicy`): RunIntervalChange = observed during the interval, NOT proven authored by the executor; Git cannot distinguish concurrent writers; unprovable cases are Ambiguous. No `executorAttribution` is emitted (no trustworthy executor source yet).

## Persistence

`.agentwatch/runs/<run-id>.delta.json` (RunFinishRecord v1: ids, start/finish UTC, full start baseline, end worktree state, headTransition, delta{runIntervalChanges, preExistingUnchangedChanges, preExistingChangedFurther, attributionAmbiguities, endBranch, endHeadCommitSha}, attributionPolicy). Temp file + atomic rename; active baseline deleted only after the record is written; existing record -> refuse, baseline kept. Missing/invalid run id -> exit 1. The canonical receipt `.agentwatch/runs/<run-id>.json` and `.ai/runs/<run-id>.md` projection are left to AW-VFY-006.

## Files

new: `src/AgentsWatch.Core/Runs/RunDelta.cs`, `src/AgentsWatch.Cli/FinishCommand.cs`, `tests/AgentsWatch.Tests/RunFinishTests.cs`
changed: `src/AgentsWatch.Core/Runs/RunBaseline.cs` (IActiveRunStore.Remove), `src/AgentsWatch.Git/GitRepositoryStateReader.cs` (IRepositoryHistoryReader), `src/AgentsWatch.Cli/Program.cs` (route/help), queue/router/prompt status

## Test matrix (14 new, temp git repos) -> all pass

1 modified; 2 add+delete; 3 rename w/ oldPath; 4 untracked; 5 dirty unchanged; 6 dirty changed further + new interval file; 7 pre-existing untracked unchanged; 8 pre-existing untracked changed; 9 spaces in names (3,4,6); 10 staged+unstaged same path then staged further; 11 missing/invalid run id; 12 fast-forward commit (clean file -> interval, dirty file -> ambiguous) and non-fast-forward + branch switch -> ambiguous; 13 rename of pre-existing dirty path -> ambiguous (concurrent/ambiguous case); plus reverted dirty file, record round trip, existing record keeps baseline, CLI start->mutate->finish.

## Gate (executed)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 61, Skipped 0, Total 61.

## CLI smoke (temp repo)

clean start, edit b.txt + add "n w.txt" -> 2 RunIntervalChange. Dirty start (b.txt, "n w.txt" left dirty, a.txt edited) then a.txt edited further + c.txt added ->
`RunIntervalChange c.txt`; `PreExistingUnchanged b.txt, n w.txt`; `PreExistingChangedFurther M a.txt: dirty at start; unstaged changed during interval; authorship not proven`. `finish nope` -> exit 1.

## Known limitations

- Rename detection relies on git similarity (>=50%); a low-similarity rename of a dirty path shows as D + A (D classified changed-further, A as interval change).
- Committed renames are reported as D + A (`--no-renames`).
- True concurrent writers cannot be detected; only policy-level labeling.
- Changes made and fully reverted within the interval are invisible; ignored files not tracked; no line counts yet.
- Untracked directories hashed per file (`-uall`); submodules fingerprinted as `dir` only.

Commit SHA (change): b9cf193