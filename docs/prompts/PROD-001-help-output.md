# PROD-001 — Help output UX alignment

Run mode: implementation  
Token budget: low  
Gate: after AW-VAL-001 and AW-VAL-002 pass

## Read first

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/COMMAND_CONTRACTS.md`

## Task

Align `agentswatch --help` output with `docs/CLI_UX_OUTPUT_SPEC.md`.

## Owned paths

- `src/AgentsWatch.Cli/`
- `tests/AgentsWatch.Tests/` if CLI smoke tests exist or are added

## Required behavior

- help output lists supported and planned commands clearly;
- output uses stable labels suitable for tests;
- no unrelated command behavior changes.

## Validation

```bash
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
dotnet run --project src/AgentsWatch.Cli -- --help
```

## Stop rules

Stop if build/test evidence is still unknown or if help output requires broader command framework refactor.
