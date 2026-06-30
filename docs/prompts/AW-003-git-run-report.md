# AW-003 — Git snapshots and run report

Run mode: implementation  
Token budget: medium  
Gate: after init is safe and build/test evidence exists

## Read first

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/REPORT_FORMATS.md`
- `docs/DATA_MODEL.md`
- `docs/ADAPTER_SPEC.md`
- `docs/TEST_MATRIX.md`

## Task

Implement or harden git snapshot capture and markdown run report writing.

## Owned paths

- `src/AgentsWatch.Git/`
- `src/AgentsWatch.Reports/`
- `src/AgentsWatch.Core/`
- `tests/AgentsWatch.Tests/`

## Required behavior

- parse clean, modified, added, deleted, renamed, copied, and untracked files;
- capture branch and commit;
- write run report in the shape defined by `REPORT_FORMATS.md`;
- record validation as pass/fail/not run;
- do not claim validation without evidence.

## Validation

```bash
dotnet build AgentsWatch.sln
dotnet test --filter Git
dotnet test --filter Report
```

## Stop rules

Stop if git command execution needs a larger abstraction or if report format changes require updating product contracts first.
