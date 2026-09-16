# AW-VFY-005 — Finish-run interval delta

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-004  
Run mode: implementation  
Budget: medium/high  
Gate: start baseline proven

## Read only

- `AGENTS.md`
- `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`
- `docs/ARCHITECTURE.md`
- `docs/DATA_MODEL.md` — RunDelta
- `docs/CLI_SPEC.md` / `docs/COMMAND_CONTRACTS.md` — `finish`
- current start-baseline implementation and direct Git/Core/storage/tests

## Task

Implement `agentswatch finish <run-id>` and compute a trustworthy **run-interval repository delta** from start baseline vs end repository evidence.

Do not equate "changed during the recorded interval" with "proven AI-agent authored" unless a trustworthy executor-specific evidence source proves causality.

## Required classifications

For each relevant file classify as one of:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Optional executor attribution may be added only with explicit evidence/provenance.

Do not equate final dirty state with run changes.

## Required behavior

- load matching active-run baseline;
- capture end branch/HEAD/staged/unstaged/untracked state;
- detect additions, modifications, deletions, renames and untracked changes observed during the interval;
- exclude pre-existing unchanged dirty state from interval changes;
- detect when a pre-existing dirty file changed further after start;
- represent cases that cannot be proven safely as `Ambiguous` with reason;
- preserve staged/unstaged semantics where needed to avoid false conclusions;
- persist enough structured end/delta state for RunReceipt v1;
- do not remove/move active baseline until final structured persistence succeeds;
- fail clearly for missing/invalid run IDs.

If branch/HEAD changes during a run, do not silently assume attribution is trivial. Record the transition and either handle it deterministically or mark affected evidence ambiguous/needs review.

If another process/human/agent may have modified the workspace during the run, do not fabricate causal authorship from Git alone.

## Owned paths

- `src/AgentsWatch.Core/**`
- `src/AgentsWatch.Git/**`
- `src/AgentsWatch.Cli/**`
- local storage code
- `tests/AgentsWatch.Tests/**`

## Avoid

- evidence/decision rules;
- scope drift/claims checks;
- dashboard/MCP;
- full source snapshots unless absolutely required and justified;
- broad Git abstraction rewrite unrelated to interval evidence.

## Required integration tests

Use temporary git repos and cover at minimum:

1. clean start -> one modified file;
2. clean start -> add/delete;
3. clean start -> rename;
4. clean start -> untracked file;
5. dirty-at-start file unchanged -> `PreExistingUnchanged`;
6. dirty-at-start file changed further -> `PreExistingChangedFurther` plus interval evidence;
7. pre-existing untracked unchanged -> not a new interval change;
8. pre-existing untracked changed -> detected/surfaced correctly;
9. filenames with spaces;
10. partial staged + unstaged state for same path where practical;
11. missing active run;
12. branch/HEAD transition behavior;
13. one explicitly modeled ambiguous/concurrent-writer case if feasible.

## Stop rules

If an edge case cannot be proven robustly without a much larger redesign, preserve it as `Ambiguous`, add a targeted follow-up note/test fixture, and do not guess.

## Validation

Run targeted interval-delta tests, then:

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

CLI smoke:

```text
start -> mutate repo -> finish
```

for representative clean and dirty starts.

## Expected evidence

- interval-delta algorithm summary;
- exact ambiguity/causality policy;
- integration-test matrix/results;
- sample delta for dirty-at-start changed-further case;
- full validation result;
- known limitations.

## Completion rule

Do not promote RunReceipt work until dirty-at-start false attribution is prevented by tests and the model does not overclaim agent authorship.
