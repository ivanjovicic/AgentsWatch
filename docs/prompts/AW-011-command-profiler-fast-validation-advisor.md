# AW-011A — Command Profiler / Fast Validation Advisor contracts

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `AW-011A`  
Queue: `docs/prompt_queues/agentwatch_mvp.md`  
Run mode: investigation-only  
Token budget: low  
Gate: after `AW-003` run report groundwork and `AW-008` validation-command groundwork

## Purpose

Investigate the smallest safe contract for command profiling and fast validation advice.

Do not implement code in this prompt.

## Minimum read

- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md`

## Optional read only if needed

- `docs/REPORT_FORMATS.md` if report/handoff shape is unclear;
- `docs/DATA_MODEL.md` if command history shape is unclear;
- `docs/SECURITY_AND_PRIVACY.md` if output redaction/storage is unclear;
- `docs/ADAPTER_SPEC.md` if language-specific validation rules are unclear.

## Task

Produce a minimal implementation plan for AW-011 without adding runtime behavior.

Answer only:

1. minimal CLI contract for `agentswatch run -- <command>`;
2. minimal CLI contract for `agentswatch validate --suggest`;
3. minimum command history record;
4. token-safe report/handoff section;
5. security/redaction risks;
6. exact next prompt.

## Scope limiter

Inspect only:

```text
docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md
docs/COMMAND_CONTRACTS.md
docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md
```

Do not inspect unless required:

```text
docs/REPORT_FORMATS.md
docs/DATA_MODEL.md
docs/SECURITY_AND_PRIVACY.md
docs/ADAPTER_SPEC.md
```

Do not edit:

```text
src/
tests/
.github/
```

## Validation

No runtime validation required. This is investigation-only.

If docs are changed later, record:

```text
Validation: docs-only, not run
```

## Stop rules

Stop and report if:

- Gate evidence is missing;
- implementation work is needed;
- more than five docs are needed;
- shell/process execution design requires broad architecture work;
- command output privacy cannot be summarized safely.

## Follow-up split

Use one follow-up prompt per implementation slice:

```text
AW-011B — command history model
AW-011C — agentswatch run wrapper
AW-011D — validate --suggest fast advisor
AW-011E — report/handoff integration
```

## Compact final response

```text
Prompt ID:
Files inspected:
Decision:
Next CLI contract:
Storage shape:
Report shape:
Risks:
Next prompt:
```
