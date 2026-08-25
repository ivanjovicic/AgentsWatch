# AW-VFY-005 — Finish-run attributable delta

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-004  
Run mode: implementation  
Budget: medium/high  
Gate: start baseline proven

## Read only

- `AGENTS.md`
- `docs/ARCHITECTURE.md` — attribution rule
- `docs/DATA_MODEL.md` — RunDelta
- `docs/CLI_SPEC.md` / `docs/COMMAND_CONTRACTS.md` — `finish`
- current start-baseline implementation and direct Git/Core/storage/tests

## Task

Implement `agentswatch finish <run-id>` and compute a trustworthy run delta from start baseline vs end repository evidence.

## Required classifications

For each relevant file classify as one of:

```text
Attributable
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Do not equate final dirty state with run changes.

## Required behavior

- load matching active-run baseline;
- capture end branch/HEAD/staged/unstaged/untracked state;
- detect additions, modifications, deletions, renames and untracked changes introduced during the run;
- exclude pre-existing unchanged dirty state from attributable changes;
- detect when a pre-existing dirty file changed further after start;
- represent cases that cannot be proven safely as `Ambiguous` with reason;
- persist enough structured end/delta state for RunReceipt v1;
- do not remove/move active baseline until final structured persistence succeeds;
- fail clearly for missing/invalid run IDs.

If branch/HEAD changes during a run, do not silently assume attribution is trivial. Record the transition and either handle it deterministically or mark affected attribution as ambiguous/needs review.

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
- broad Git abstraction rewrite unrelated to attribution.

## Required integration tests

Use temporary git repos and cover at minimum:

1. clean start -> one new modified file;
2. clean start -> add/delete;
3. clean start -> rename;
4. clean start -> untracked file;
5. dirty-at-start file unchanged during run -> not attributable;
6. dirty-at-start file changed further -> surfaced as changed-further/attributable-with-preexisting-context according to final model;
7. pre-existing untracked unchanged -> not attributable;
8. pre-existing untracked changed -> detected/surfaced correctly;
9. filenames with spaces;
10. missing active run;
11. branch/HEAD transition behavior.

## Stop rules

If an edge case cannot be proven robustly without a much larger redesign, preserve it as `Ambiguous`, add a targeted follow-up note/test fixture, and do not guess.

## Validation

Run targeted attribution tests, then:

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

CLI smoke start -> mutate repo -> finish for representative clean and dirty starts.

## Expected evidence

- attribution algorithm summary;
- exact ambiguity policy;
- integration-test matrix/results;
- sample delta for dirty-at-start case;
- full validation result;
- known limitations.

## Completion rule

Do not promote RunReceipt verification work until the dirty-at-start false-attribution case is proven by tests.
