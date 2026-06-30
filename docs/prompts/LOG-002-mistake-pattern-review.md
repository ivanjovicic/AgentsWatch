# LOG-002 — Mistake pattern review

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `LOG-002`  
Run mode: review-only  
Token budget: low  
Permission mode: read_only  
Gate: after at least three run logs exist, or after one repeated failure

## Purpose

Find repeated mistakes and convert them into smaller future prompts and token-saving rules.

## Minimum read

- recent run logs;
- `.ai/learning/MISTAKE_PATTERNS.md` if present;
- `.ai/learning/DO_NOT_REPEAT.md` if present;
- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`.

## Task

Review recent run logs and identify patterns.

Classify:

```text
ScopeCreep
OverRead
OverTest
UnderTest
ValidationSkipped
WrongModel
WrongTool
SensitivePathTouched
LargeLogPasted
RepeatedFailure
MissingHandoff
ClaimedButNotDone
FlutterStateRisk
FlutterNavigationRisk
FlutterPersistenceRisk
```

For each repeated pattern, propose:

- prompt rule;
- smaller next prompt;
- validation improvement;
- context reduction;
- model/tool recommendation change if needed.

## Output

```text
Repeated patterns:
Token waste sources:
Validation issues:
Prompt rules to add:
Do not repeat updates:
Next minimal prompt:
```

## Rules

- Do not blame the model generically.
- Only add a rule if it is specific and repeatable.
- Prefer fewer, stronger rules over many vague rules.
- Do not recommend full repo reads as a fix.
- Do not recommend full test suites unless risk justifies it.

## Stop rules

Stop if:

- there is not enough evidence;
- run logs contain secrets;
- failures are unrelated and should not become a general rule.
