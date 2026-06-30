# LOG-003 — Flutter agent run review

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `LOG-003`  
Run mode: review-only  
Token budget: low  
Permission mode: read_only  
Gate: after a Flutter-related agent run

## Purpose

Review one Flutter-related agent run and extract lessons that reduce future token use and validation waste.

## Minimum read

- latest run report or agent final response;
- changed-file summary;
- validation evidence or blocked reason;
- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`;
- Flutter section of `docs/ADAPTER_SPEC.md`.

## Task

Review whether the agent handled Flutter scope and validation efficiently.

Check:

1. Did it touch widgets only, or also state/navigation/persistence?
2. Did it inspect unrelated app areas?
3. Did it run or suggest the smallest useful validation?
4. Did it skip widget tests when UI behavior changed?
5. Did provider/state changes require a narrower follow-up?
6. Did navigation/router changes increase risk?
7. Did persistence/offline changes require extra validation?

## Output

```text
Flutter run result:
Touched areas:
Validation result:
Missed tests:
Token waste:
Learning note:
Next Flutter prompt:
Risk:
```

## Rules

- Prefer `flutter analyze` before full `flutter test` for small UI-only changes.
- Prefer targeted widget tests when a changed widget has matching tests.
- Split provider/state changes from navigation or persistence changes.
- Do not inspect the whole Flutter app unless changed files require it.
- Do not run integration tests unless navigation, platform, or persistence risk justifies it.

## Stop rules

Stop if:

- changed files are unknown;
- validation evidence is missing for runtime changes;
- touched areas include sensitive platform/config files;
- the next prompt would cross more than one Flutter subsystem.
