# AgentsWatch Feature Selection Spec

Last aligned: 2026-07-17  
Status: planning/specification

## Purpose

AgentsWatch capabilities must be modular, but feature selection should reinforce the differentiated product loop rather than grow into a generic agent platform.

Core principle:

```text
Select only the control, evidence and learning capabilities needed by this repository.
```

Safety principle:

```text
Local-first. No telemetry. No source upload. Risky execution remains outside AgentsWatch unless explicitly integrated later.
```

## Differentiated feature packages

`core` is required. The first useful profile should include the complete receipt/evidence spine rather than isolated convenience features.

| Feature | Default | Requires | Purpose |
|---|---:|---|---|
| `core` | on | none | Config, local folders, project detection and status. |
| `lifecycle` | on | `core` | Task/run start, finish and status transitions. |
| `contract` | on | `core` | Bounded run contracts from prompts, issues or roadmap items. |
| `receipt` | on | `lifecycle` | Vendor-neutral Agent Run Receipt. |
| `git-evidence` | on | `lifecycle` | Start/end snapshots, changed files and ownership evidence. |
| `validation-evidence` | on | `receipt` | Record validation result or blocked reason. Execution remains opt-in. |
| `evidence-gate` | on | `receipt`, `git-evidence`, `validation-evidence` | Claims-vs-diff-vs-validation checks and Evidence Score. |
| `drift` | on | `contract`, `git-evidence` | Scope and roadmap drift findings. |
| `handoff` | on | `receipt` | Compact next-agent handoff and next prompt. |
| `learning` | suggested | `receipt`, `evidence-gate` | Learning events, mistake patterns and do-not-repeat rules. |
| `validation-economy` | suggested | `validation-evidence`, `adapters` | Targeted validation, command profiling and avoidable-work estimates. |
| `roadmap` | suggested | `contract`, `receipt`, `drift` | Roadmap checks, queue generation and evidence-based status updates. |
| `router` | later/local | `receipt`, `learning`, `metrics` | Project-local model/tool recommendation with confidence. |
| `metrics` | optional | `receipt` | Explainable proxy metrics and provider usage import when available. |
| `adapters` | auto-detect | `core` | Stack-specific risk, evidence and validation suggestions. |
| `connectors` | future opt-in | stable internal contracts | Thin integrations with external agent products. |
| `dashboard` | future-only | dogfood evidence | Local visualization after enough receipts exist. |
| `team` | future-only | dashboard/privacy review | Shared policies and signed evidence export. |
| `cloud` | future-only opt-in | explicit consent | Optional metadata services; never source upload by default. |

## Dependency rules

- `core` cannot be disabled.
- `lifecycle`, `contract`, `receipt`, `git-evidence`, `validation-evidence`, `evidence-gate`, `drift`, and `handoff` form the recommended MVP spine.
- `learning` must not run without receipt and evidence inputs.
- `router` must not produce a recommendation when comparable history is insufficient.
- `validation-economy` may suggest commands but execution is opt-in.
- `connectors`, `dashboard`, `team`, and `cloud` are never default MVP features.
- Disabled features must remain inert and must not create artifacts.

## Profiles

| Profile | Features | Use case |
|---|---|---|
| `receipt-only` | `core`, `lifecycle`, `receipt`, `git-evidence`, `validation-evidence`, `handoff` | Minimal trustworthy run record. |
| `verified-local` | receipt-only + `contract`, `evidence-gate`, `drift`, `adapters` | Recommended first product. |
| `roadmap-local` | verified-local + `roadmap`, `learning` | Roadmap-driven supervised execution. |
| `economy-local` | verified-local + `learning`, `validation-economy`, `metrics` | Reduce validation and context waste. |
| `strict-local` | roadmap-local + `validation-economy`, `metrics` | High-evidence dogfood and production repositories. |
| `router-lab` | strict-local + `router` | Experimental cross-agent comparison after enough runs. |

No profile enables `connectors`, `dashboard`, `team`, or `cloud` by default.

## CLI direction

### Initialize a profile

```bash
agentswatch init --profile verified-local
```

Expected:

- resolve dependencies;
- preview writes with `--dry-run`;
- create only selected local artifacts;
- preserve existing user files;
- show disabled and future-only capabilities.

### Feature inspection

```bash
agentswatch features list
agentswatch features status
```

Show:

- enabled features;
- dependency additions;
- local artifact paths;
- unavailable/future features;
- missing dogfood prerequisites.

### Enable later

```bash
agentswatch features enable learning
agentswatch features enable validation-economy
```

Enabling a feature must not silently execute validation, upload data, or modify external services.

## Config shape

```yaml
schemaVersion: 2
project:
  name: MathLearning Mobile
  profile: strict-local
  types:
    - flutter

features:
  core: true
  lifecycle: true
  contract: true
  receipt: true
  git-evidence: true
  validation-evidence: true
  evidence-gate: true
  drift: true
  handoff: true
  learning: true
  validation-economy: true
  roadmap: true
  router: false
  metrics: true
  adapters: true
  connectors: false
  dashboard: false
  team: false
  cloud: false

featureOptions:
  validation-evidence:
    executeByDefault: false
  learning:
    minimumEvidenceCountForRule: 2
    requireHumanAcceptance: true
    defaultExpiryRuns: 30
  router:
    minimumComparableRuns: 5
    allowUnknownResult: true
  metrics:
    exactProviderUsageOnlyWhenAvailable: true
```

## Artifact mapping

| Feature | Artifacts |
|---|---|
| `core` | `.ai/config.yml`, `.ai/STATUS.md` |
| `lifecycle` | `.ai/tasks/`, `.ai/runs/` |
| `contract` | `.ai/contracts/` |
| `receipt` | `.ai/runs/<run-id>-receipt.md` |
| `git-evidence` | receipt sections or JSON sidecars later |
| `validation-evidence` | compact validation sections |
| `evidence-gate` | evidence findings and score explanation |
| `drift` | scope/roadmap drift findings |
| `handoff` | `.ai/runs/<run-id>-handoff.md` |
| `learning` | `.ai/learning/LESSONS.md`, `MISTAKE_PATTERNS.md`, `DO_NOT_REPEAT.md` |
| `validation-economy` | compact command profiles and validation recommendations |
| `roadmap` | `.ai/roadmap/`, `.ai/queue/` |
| `router` | `.agentwatch/router-evidence.jsonl` later |
| `metrics` | receipt metrics and local rollups |

## Feature-gated command direction

| Command | Required feature |
|---|---|
| `agentswatch start/finish` | `lifecycle` |
| `agentswatch contract check/build` | `contract` |
| `agentswatch receipt create/check` | `receipt` |
| `agentswatch evidence check` | `evidence-gate` |
| `agentswatch drift check` | `drift` |
| `agentswatch handoff` | `handoff` |
| `agentswatch mistakes list/check/rollup` | `learning` |
| `agentswatch validate --suggest/--profile` | `validation-economy` |
| `agentswatch roadmap check/next/review` | `roadmap` |
| `agentswatch route suggest` | `router` |

## Features deliberately excluded as packages

Do not add first-class MVP packages for:

- agent execution runtime;
- cloud sandbox;
- generic scheduling;
- generic playbook/knowledge library;
- full session archive;
- visual workflow canvas;
- production deployment orchestration;
- automatic merge/release;
- incident management;
- integration marketplace.

External products already cover these areas. AgentsWatch should add thin adapters only after its internal receipt and evidence contracts are stable.

## Required tests when implemented

At minimum:

1. profile resolution and dependency expansion;
2. dry-run writes nothing;
3. disabled features remain inert;
4. local artifacts stay inside repository root;
5. future-only features are rejected;
6. validation execution defaults to false;
7. receipt/evidence spine cannot be configured inconsistently;
8. learning rules require sufficient evidence and acceptance;
9. router returns unknown with insufficient comparable runs;
10. disabling features preserves user data;
11. no profile enables cloud/team/dashboard/connectors automatically.

## MVP decision

Implement the `verified-local` profile first.

The next profile should be `roadmap-local` only after the Agent Run Receipt and evidence gate are trustworthy.

See:

- `docs/PRODUCT_SPEC.md`
- `docs/MVP_ROADMAP.md`
- `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`
