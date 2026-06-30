# LOG-001 — Post-prompt run log

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `LOG-001`  
Run mode: docs/evidence  
Token budget: low  
Permission mode: docs_only  
Gate: after any agent run returns a final response or evidence

## Purpose

Turn one completed agent response into a compact local run log and one learning note.

## Minimum read

- latest agent final response or run report;
- changed-file summary;
- validation evidence or blocked reason;
- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`.

## Task

Write or update the post-prompt log for the run.

Record:

```text
Run ID:
Prompt ID:
Queue item:
Tool:
Model:
Permission mode:
Run mode:
Token budget:
Status:
Files inspected:
Files changed:
Validation:
Command profile:
Mistakes:
Missed work:
Scope creep:
Token waste:
Learning note:
Next prompt:
Do not repeat:
```

## Rules

- Keep the log compact.
- Do not paste full terminal output.
- Do not paste full chat history.
- Do not include secrets.
- Do not invent validation.
- If validation was not run, say why.
- Add exactly one useful learning note.
- Add `Do not repeat` only if the mistake is specific and repeatable.

## Output

```text
Run log:
Learning note:
Do not repeat:
Next prompt:
Risk:
```

## Stop rules

Stop if:

- evidence is missing;
- validation status is unclear;
- output may contain secrets;
- changed files are unknown for runtime work.
