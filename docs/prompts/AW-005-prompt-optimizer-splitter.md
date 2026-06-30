# AW-005 — Prompt optimizer and task splitter

Run mode: implementation  
Token budget: medium  
Gate: after prompt analyzer tests and Gate 0 evidence exist

## Read first

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/PROMPT_OPTIMIZATION_PLAYBOOK.md`
- `docs/PROMPT_RULES.md`
- `docs/PROMPT_QUALITY_CHECKLIST.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`

## Task

Improve prompt optimization and implement task split output.

## Owned paths

- `src/AgentsWatch.Core/`
- `src/AgentsWatch.Cli/`
- `tests/AgentsWatch.Tests/`

## Required behavior

- classify broad prompts as high risk;
- detect missing validation, stop rules, and scope limiter;
- output token budget and waste causes;
- generate or print four task prompts: investigation, implementation, tests, diff-only review;
- do not invent repo-specific paths when unknown;
- do not overwrite existing task files by default.

## Validation

```bash
dotnet build AgentsWatch.sln
dotnet test --filter PromptRiskAnalyzer
```

## Stop rules

Stop if task splitting requires a new storage format not defined in docs, or if output contract conflicts with `CLI_UX_OUTPUT_SPEC.md`.
