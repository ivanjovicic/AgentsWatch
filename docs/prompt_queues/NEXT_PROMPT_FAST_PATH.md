# AgentsWatch Next Prompt Fast Path

Last aligned: 2026-06-29

## Use this when you want the next prompt quickly

Current safest next prompt is always:

```text
AW-VAL-001 — Build validation
```

until restore/build/test evidence exists.

## Copy-ready prompt

```text
Repository: ivanjovicic/AgentsWatch
Prompt ID: AW-VAL-001
Queue: docs/prompt_queues/bootstrap_validation.md
Run mode: validation-only
Token budget: low

Read first:
- AGENTS.md
- docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
- docs/AGENT_OPERATING_SYSTEM.md
- docs/CONTEXT_INDEX.md
- docs/AGENT_COMMAND_PLAYBOOK.md
- docs/prompts/AW-VAL-001-build-validation.md

Task:
Run build validation for the current skeleton.

Scope limiter:
Inspect only files needed for restore/build/test failures.
Do not add features.
Do not refactor.
Do not edit docs unless recording validation evidence.

Commands:
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln

If a command fails:
- fix only the build/test failure;
- rerun the failing command;
- record evidence.

Final response:
- validation results
- files changed
- commit SHA
- completion percentage
- missed work
- follow-up prompt
- residual risk
```

## After AW-VAL-001 passes

Run:

```text
AW-VAL-002 — CLI smoke validation
```

## After AW-VAL-002 passes

Run:

```text
AW-VAL-003 — Validation evidence review
```

## After AW-VAL-003 passes

Run:

```text
AW-VAL-004 — Init command hardening
```
