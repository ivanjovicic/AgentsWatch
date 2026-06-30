# AW-002 — Init command hardening

Run mode: implementation  
Token budget: low  
Gate: after AW-VAL-001 and AW-VAL-002 pass

## Read first

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/CONFIG_REFERENCE.md`
- `docs/TEST_MATRIX.md`

## Task

Harden `agentswatch init`.

## Owned paths

- `src/AgentsWatch.Cli/`
- `tests/AgentsWatch.Tests/`

## Required behavior

- creates `.ai` and `.agentwatch` folders;
- creates config/status/changelog/review checklist;
- preserves existing files;
- prints created/preserved paths;
- works in temp-directory tests;
- does not touch unrelated commands.

## Validation

```bash
dotnet build AgentsWatch.sln
dotnet test --filter Init
```

## Stop rules

Stop if build/test status is still unknown, or if fixing init requires changing git/report/prompt optimizer behavior.

## Final response

Include changed files, validation, commit SHA, completion %, missed work, follow-up, residual risk.
