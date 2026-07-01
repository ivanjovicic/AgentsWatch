# AgentsWatch Feature Selection Prompt Queue

Last aligned: 2026-06-30  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: AgentsWatch feature-selection and modular install behavior  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/prompt_queues/agentwatch_feature_selection.md`  
Parent docs: [../FEATURE_SELECTION_SPEC.md](../FEATURE_SELECTION_SPEC.md), [agentwatch_foundation_followups.md](agentwatch_foundation_followups.md)

## Purpose

Ensure users can choose which AgentsWatch features are enabled and that only selected local artifacts are applied to a repository.

## Rules

- `core` is required.
- Disabled features must stay inert.
- No telemetry/cloud/team/dashboard feature is enabled by default.
- Feature selection must be local-first.
- `init` and `enable` must preserve user-edited files.
- `disable` must not delete user data by default.
- Every implementation prompt needs tests for selected/disabled features.
- Prefer non-interactive flags before interactive UI.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-FEATURES-001 | Ready after AW-CONFIG-001 | Add feature model and dependency validation for `core`, `reports`, `handoff`, `review`, `risk`, `validation`, `adapters`, `learning`, `lint`, `metrics`, `dogfood`. |
| AW-FEATURES-002 | Ready after AW-FEATURES-001 | Add profile resolution for `minimal`, `solo`, `solo-dev`, `strict-local`, `reviewer`, `adapter-dev`, `repo-audit`. |
| AW-FEATURES-003 | Ready after AW-FEATURES-002 | Make `agentswatch init --profile` and `--features` create only selected artifacts. |
| AW-FEATURES-004 | Ready after AW-FEATURES-003 | Add feature-gated command behavior with clear disabled-feature errors. |
| AW-FEATURES-005 | Ready after AW-FEATURES-004 | Add tests for disabled feature inertness, dependency expansion, future-only rejection, and dry-run safety. |

## Validation

```bash
dotnet test --filter Features
```

Adjust filters to actual test traits once implementation exists.
