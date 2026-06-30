# PROD-003 — Status non-git behavior

Run mode: implementation  
Token budget: low  
Gate: after Gate 0 validation

## Read first

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/ADAPTER_SPEC.md`

## Task

Make `agentswatch status` handle folders that are not git repositories.

## Owned paths

- `src/AgentsWatch.Cli/`
- `src/AgentsWatch.Git/`
- `tests/AgentsWatch.Tests/`

## Required behavior

- no raw exception for non-git folders;
- output includes a stable `Git: not detected` style message;
- project type detection may still run;
- validation is not run automatically.

## Validation

Use targeted build and status tests.

## Stop rules

Stop if the fix expands into reports, prompt optimizer, or external integrations.
