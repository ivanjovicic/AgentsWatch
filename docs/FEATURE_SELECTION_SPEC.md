# AgentsWatch Feature Selection Spec

Last aligned: 2026-06-30  
Status: planning/specification  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/AGENTWATCH_FEATURE_SELECTION_SPEC.md`

## Purpose

AgentsWatch should not apply every feature to every repository.

The user must be able to choose which AgentsWatch capabilities are enabled for a repo, and AgentsWatch must add only the local files, prompts, templates, lint checks, and commands needed for those selected capabilities.

Core principle:

```text
Select features -> write config -> apply only selected local artifacts -> keep disabled features inert.
```

MVP safety principle:

```text
Local-first by default. No telemetry. No cloud upload. No dashboard/team/cloud feature unless explicitly supported in a later phase.
```

## Why this matters

Different users and repositories need different levels of supervision.

Examples:

- a small solo repo may need only reports and handoffs;
- a high-risk production repo may need evidence lint and mistake learning;
- a learning app may need strict validation and run logs;
- a later team setup may need PR reports, but that is not MVP.

AgentsWatch should be modular from the start so the first install does not feel heavy or invasive.

## Feature packages

Feature packages are composable. `core` is the only required package.

| Feature | Default MVP state | Requires | Purpose |
|---|---:|---|---|
| `core` | on | none | `.ai` structure, config, tasks, runs, status. |
| `reports` | on | `core` | run report, changed files, validation evidence. |
| `handoff` | on for solo/dev | `reports` | compact next-agent handoff summaries. |
| `review` | on for solo/dev | `reports` | diff-only review prompt generation. |
| `risk` | on | `reports` | heuristic risk scoring. |
| `validation` | suggested | `core` | validation command suggestions; execution is opt-in. |
| `adapters` | auto-detect | `validation` | Flutter/.NET/React/Python/Node detection presets. |
| `learning` | off/minimal, on/strict | `reports` | mistake ledger, mistake cards, rollups. |
| `lint` | off/minimal, on/strict | `reports` | evidence and learning lint gates. |
| `metrics` | optional | `reports` | proxy token-waste metrics, not exact token accounting. |
| `dogfood` | off by default | `reports`, `handoff` | dogfood report templates and prompts. |
| `dashboard` | future-only | CLI dogfood evidence | local UI after CLI MVP proves value. |
| `team` | future-only | dashboard/privacy review | PR/team reports after local evidence. |
| `cloud` | future-only opt-in | explicit future consent | cloud sync; never default. |

Rules:

- `core` cannot be disabled.
- `reports` requires `core`.
- `handoff`, `review`, `risk`, `learning`, `lint`, and `metrics` require `reports`.
- `adapters` requires `validation`.
- `lint` can run without `learning`, but learning lint checks are disabled unless `learning` is enabled.
- `metrics` must use proxy metrics unless provider token data exists.
- `dashboard`, `team`, and `cloud` must never be enabled by default.

## Suggested repo profiles

Profiles are presets. Users can override features after selecting a profile.

| Profile | Enabled features | Use case |
|---|---|---|
| `minimal` | `core`, `reports` | lightweight run evidence only. |
| `solo` | `core`, `reports`, `handoff`, `review`, `risk`, `metrics` | solo developer using AI agents. |
| `solo-dev` | `core`, `reports`, `handoff`, `review`, `risk`, `validation`, `adapters`, `metrics` | active local development. |
| `strict-local` | `core`, `reports`, `handoff`, `review`, `risk`, `validation`, `adapters`, `learning`, `lint`, `metrics`, `dogfood` | high-evidence local workflow; good for MathLearning dogfood. |
| `reviewer` | `core`, `reports`, `review`, `risk`, `lint` | review-focused use. |
| `adapter-dev` | `core`, `reports`, `validation`, `adapters`, `risk` | improving stack detection and validation commands. |
| `repo-audit` | `core`, `reports`, `handoff`, `review`, `risk`, `metrics` | one-off audit without heavy learning/lint setup. |

No profile enables `dashboard`, `team`, or `cloud` in MVP.

## CLI behavior

### Init with explicit features

```bash
agentswatch init --features core,reports,handoff,review,risk
```

Expected behavior:

- validate feature names;
- expand dependencies;
- write selected features to config;
- create only required local files/templates;
- summarize enabled, skipped, dependency-added, and future-only features;
- never enable cloud/team/dashboard by accident.

### Init with profile

```bash
agentswatch init --profile strict-local
```

Expected behavior:

- resolve profile to features;
- write profile and resolved features to config;
- allow explicit override when supported;
- show final feature set before writing when `--dry-run` is used.

### Profile plus feature overrides

Preferred MVP behavior:

```bash
agentswatch init --profile solo-dev --enable learning,lint --disable adapters
```

If implementation does not support profile + overrides yet, it should reject the mixed input with a clear error instead of guessing.

### Enable feature later

Preferred command group:

```bash
agentswatch features enable learning
agentswatch features enable lint
```

Expected behavior:

- validate dependencies;
- create only missing files/templates;
- preserve user-edited files;
- update config;
- show what changed;
- suggest `--dry-run` if the user wants a preview.

### Disable feature later

```bash
agentswatch features disable learning
```

Expected behavior:

- update config;
- do not delete user data by default;
- stop running feature commands/lints by default;
- preserve existing files unless user explicitly asks to remove generated artifacts;
- warn if another enabled feature depends on the disabled feature.

### Inspect feature state

```bash
agentswatch features list
agentswatch features status
```

Expected behavior:

- show enabled features;
- show disabled features;
- show future-only unavailable features;
- show dependency warnings;
- show where each enabled feature stores local artifacts.

## Config shape

Example:

```yaml
schemaVersion: 1
project:
  name: MathLearning Mobile
  profile: strict-local
  types:
    - flutter

features:
  core: true
  reports: true
  handoff: true
  review: true
  risk: true
  validation: true
  adapters: true
  learning: true
  lint: true
  metrics: false
  dogfood: true
  dashboard: false
  team: false
  cloud: false

featureOptions:
  validation:
    executeByDefault: false
  learning:
    ledgerPath: docs/ai/learning/MISTAKE_LEDGER.md
  metrics:
    exactProviderTokens: false
```

Rules:

- disabled features must not run checks;
- disabled features must not create new artifacts during normal commands;
- feature dependencies must be validated;
- unknown feature names are config errors;
- future config versions need migration rules;
- `validation.executeByDefault` must default to `false`;
- `cloud`, `team`, and `dashboard` cannot be enabled by profile defaults.

## Artifact mapping

Each feature package needs a deterministic manifest.

| Feature | Local artifacts |
|---|---|
| `core` | `.ai/config.yml`, `.ai/tasks/`, `.ai/runs/`, `.ai/STATUS.md` |
| `reports` | run report templates, report output folder |
| `handoff` | handoff template/prompt |
| `review` | diff-only review prompt template |
| `risk` | risk rules/config block |
| `validation` | validation command config/presets |
| `adapters` | adapter detection config/presets |
| `learning` | `.ai/learning/MISTAKE_LEDGER.md`, mistake card template, rollup template |
| `lint` | lint rules config and evidence/learning lint command access |
| `metrics` | proxy metrics config/report section |
| `dogfood` | dogfood report template |

Do not create dashboard/team/cloud artifacts in MVP.

## Feature-gated commands

Commands must check feature state before running.

| Command | Required feature |
|---|---|
| `agentswatch report` | `reports` |
| `agentswatch handoff` | `handoff` |
| `agentswatch review-diff` | `review` |
| `agentswatch validate` | `validation` |
| `agentswatch mistakes list/check/add` | `learning` |
| `agentswatch lint evidence` | `lint` |
| `agentswatch lint learning` | `lint`, and `learning` for learning-specific checks |
| `agentswatch metrics` | `metrics` |

If a feature is disabled, the CLI should return a clear user error and suggest the enable command.

## Safety rules

- Feature selection is local-only.
- No feature enables telemetry by default.
- No feature uploads source code, prompts, diffs, run logs, or mistake ledgers by default.
- `cloud` is not an MVP feature and must require explicit future opt-in.
- Disabling a feature never deletes user data by default.
- `--dry-run` must show planned writes and config changes without changing files.
- Enabling a feature must preserve user-edited files.
- If dependency resolution would enable many features, show a summary before writing.

## Required tests when implemented

Feature selection needs tests for:

1. explicit feature list;
2. profile resolution;
3. profile plus override behavior;
4. dependency expansion;
5. invalid feature name;
6. conflict handling;
7. future-only feature rejection;
8. `core` cannot be disabled;
9. enable feature later;
10. disable feature later;
11. disabled feature does not run;
12. feature-gated command produces clear user error;
13. init creates only selected artifacts;
14. dry-run writes nothing;
15. user-edited files are preserved;
16. cloud/team/dashboard are never enabled by default;
17. feature manifest rejects outside-repo paths;
18. disabled feature data is preserved.

## Non-goals

Do not implement now:

- SaaS profile;
- cloud sync profile;
- team policy profile;
- dashboard defaults;
- marketplace/plugin feature packs;
- automatic feature choice based on uploading repo data;
- deleting user data when disabling a feature.

## MVP decision

Feature selection should be implemented before broad usage, but after the basic CLI skeleton and config schema exist.

Recommended prompt order:

```text
AW-SCOPE-001 -> AW-LIFECYCLE-001 -> AW-PRIVACY-001 -> AW-CONFIG-001 -> AW-FEATURES-001 -> AW-FEATURES-002 -> AW-001/AW-002 implementation
```

The first implementation can support profiles and config without implementing every optional feature command immediately.
