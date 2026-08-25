# AW-VFY-004 — Start-run dirty-worktree baseline

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-003  
Run mode: implementation  
Budget: medium  
Gate: RunContract v1 proven

## Read only

- `AGENTS.md`
- `docs/ARCHITECTURE.md` — attribution rule
- `docs/DATA_MODEL.md` — RunBaseline/WorktreeState
- `docs/CLI_SPEC.md` / `docs/COMMAND_CONTRACTS.md` — `start`
- current Git/Core/CLI/test files directly involved

## Task

Implement `agentswatch start <contract-id>` and persist a machine-readable active-run baseline that is sufficient for later run attribution.

## Critical correctness requirement

A dirty file that existed before `start` must not later be treated as an agent-created run change merely because it is still dirty at finish.

Capture enough state to distinguish:

- staged changes;
- unstaged changes;
- untracked files;
- branch/HEAD;
- content/diff state needed to determine whether a pre-existing dirty file changed further during the run.

Prefer fingerprints/diff hashes over storing full source contents.

## Required behavior

- load and validate referenced RunContract;
- generate stable unique run ID;
- capture UTC timestamp, branch and HEAD;
- capture lossless staged/unstaged/untracked state;
- persist `.agentwatch/active-runs/<run-id>.json` atomically/safely;
- report dirty baseline clearly;
- optional agent/tool/model metadata may be accepted if small and useful;
- refuse overlapping active run by default unless there is already an explicitly designed safe multi-run model (do not invent one here).

## Owned paths

- `src/AgentsWatch.Core/**`
- `src/AgentsWatch.Git/**`
- `src/AgentsWatch.Cli/**`
- local storage adapter code
- `tests/AgentsWatch.Tests/**`

## Avoid

- finish/delta implementation;
- receipt/evidence/drift/claims;
- full file-content snapshots;
- vendor APIs;
- SQLite/cloud storage.

## Required tests

Use temporary git repositories and cover:

- clean repo start;
- unstaged dirty file at start;
- staged dirty file at start;
- untracked file at start;
- filename with spaces;
- invalid/missing contract;
- active run already exists;
- persistence/round trip;
- branch/HEAD captured correctly.

## Stop rules

If attribution requires a richer Git state primitive than current models support, add only the minimal reusable primitive needed by AW-VFY-005; do not implement finish in this prompt.

## Validation

Run targeted start/Git tests, then:

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

CLI smoke the new `start` command in clean and dirty temporary repos.

## Expected evidence

- exact baseline fields persisted;
- explanation of how pre-existing changes are fingerprinted;
- test matrix/results;
- sample concise CLI output;
- full validation result;
- remaining attribution cases for AW-VFY-005.
