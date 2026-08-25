# AW-VFY-001 — Git parser and CI hardening

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready now  
Run mode: implementation + validation  
Budget: low/medium  
Gate: current Gate 0 blocker

## Read only

- `AGENTS.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/ARCHITECTURE.md` — Git evidence section
- `src/AgentsWatch.Git/GitCommandRunner.cs`
- `tests/AgentsWatch.Tests/GitStatusParserTests.cs`
- any directly related Git parser tests/files discovered from those references

## Known evidence/root cause

Latest CI restores/builds successfully but fails `GitStatusParserTests.Parse_ParsesModifiedAndUntrackedFiles`.

Current parser splits with `TrimEntries`, which removes the leading fixed-width porcelain status column before `line[3..]`, turning `README.md` into `EADME.md`.

Do not spend the run rediscovering this known root cause unless code has changed materially.

## Task

Replace the fragile status parser contract with a lossless machine-safe git porcelain approach.

Prefer NUL-delimited porcelain such as:

```bash
git status --porcelain=v1 -z -uall
```

or an equally robust approach justified by tests.

Implement the smallest coherent fix that prepares the Git adapter for later attribution work.

## Required behavior/tests

Cover at minimum:

- clean output;
- unstaged modified file;
- staged modified/added file;
- deleted file;
- untracked file;
- renamed file including old/new path handling if the selected porcelain format exposes it;
- filename containing spaces;
- no accidental trimming/corruption of path characters.

If the existing `ChangedFile` model cannot represent rename safely, make only the minimal backward-compatible model change necessary and document the follow-up if richer attribution still belongs to AW-VFY-005.

## Owned paths

- `src/AgentsWatch.Git/**`
- `src/AgentsWatch.Core/Models.cs` only if minimally required for lossless parser semantics
- `tests/AgentsWatch.Tests/*Git*`
- Gate 0 evidence/status docs only if recording the result

## Avoid

- Contract/Receipt implementation;
- prompt optimizer changes;
- language adapter expansion;
- dashboard/SaaS/productization;
- broad refactors.

## Stop rules

Stop and report instead of expanding scope if:

- the failure is no longer caused by the documented parser behavior;
- fixing it requires redesign outside Git evidence/model boundaries;
- environment/SDK failure blocks validation.

## Validation

During iteration use targeted tests as useful, then run the complete gate:

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln --configuration Release --no-restore
dotnet test AgentsWatch.sln --configuration Release --no-build
```

## Expected evidence

- exact parser contract chosen;
- targeted parser tests added/updated;
- full restore/build/test result;
- changed files;
- remaining Git-attribution limitations for AW-VFY-005, if any.

## Completion rule

Mark AW-VFY-001 Done only if the full test gate passes. Otherwise mark Blocked/Needs follow-up with exact evidence and do not promote AW-VFY-002.
