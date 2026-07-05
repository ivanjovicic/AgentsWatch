# AgentsWatch MVP Roadmap

Last aligned: 2026-07-05  
Status: planning/specification

## Strategy

Do not start with SaaS. Start with a local CLI that works with any repo through git, markdown, shell commands, config, and explicit local evidence imports.

Recommended first product foundation:

```text
AgentsWatch CLI — local evidence and control layer for AI coding-agent work.
```

Recommended first market-facing experiment:

```text
AgentsWatch PR Evidence + Trust Ledger
```

Initial public outcome:

```text
Know what your coding agent changed, executed, tested, and missed.
```

The broader `control plane` positioning remains an internal/user-validation hypothesis.

## Roadmap rules

- Current code/tests/proof override this roadmap.
- Community demand signals do not prove market demand.
- Adjacent paid products prove a neighboring budget category, not willingness to buy AgentsWatch.
- A model or provider name does not prove runtime capability.
- Tool, surface, observation, permission, environment, and VCS/delivery profiles must be resolved independently.
- Every advanced feature must expose `Full`, `Guarded`, `Advisory`, `PostHoc`, `Manual`, or `Unavailable` support.
- New community-derived capabilities remain incubator work until Gate 0, user discovery, the run/evidence spine, and the required compatibility gate are complete.
- Import-only, dry-run, and manual-assisted slices come before live monitoring or enforcement.
- Do not build independent provider pipelines; share runtime-profile, normalized-event, evidence, and fallback foundations.
- Do not lead with a generic AI bug reviewer; verify work/evidence first.
- Infrastructure follows repeat use and paid demand; later components must not become dependencies of local value.

## Prototype: 3-7 days

Goal: prove the workflow manually with markdown files.

Deliverables:

- `.ai/` folder shape;
- example optimized prompt set;
- example run report;
- example risk report;
- example handoff summary;
- example changelog entry;
- example PR Evidence Packet containing claims, commands, changed files, validation, commit match, and missing evidence.

No database, dashboard, daemon, GitHub App, or SaaS.

## Phase 1: CLI MVP, 2-4 weeks

Goal: useful local tool and evidence spine.

Features:

1. `.ai` folder generator.
2. Basic `agentswatch.yml` config.
3. Prompt optimizer.
4. Prompt splitter.
5. Git diff tracker.
6. Basic risk score.
7. Markdown run report.
8. Changelog generator.

Definition of done:

- works on a Flutter repo;
- works on a .NET repo;
- reports changed files;
- generates a prompt split;
- creates run report and changelog;
- no cloud dependency.

## Phase 2: Useful solo/evidence tool, 4-8 weeks total

Add:

1. Acceptance criteria checker.
2. Claimed-vs-actual diff checker.
3. Validation runner.
4. Commit-bound evidence identity.
5. Handoff summary generator.
6. Token/context waste report.
7. Diff-only review prompt generator.
8. Language adapters for Flutter, .NET, React/TypeScript, Python, Node.
9. Command Profiler / Fast Validation Advisor.
10. Mistake/discovery reconciliation.
11. Proof-bundle validation.
12. Local PR Evidence Packet prototype.

### PR Evidence Packet minimum output

- declared task/scope;
- actual changed files;
- files outside scope;
- observed commands and exit codes;
- summarized build/test evidence;
- CI/artifact evidence when supplied;
- evidence commit versus current commit;
- claim statuses;
- missing validation;
- remaining reviewer actions.

Claim statuses:

```text
SUPPORTED
PARTIALLY_SUPPORTED
CONTRADICTED
MISSING_EVIDENCE
STALE_EVIDENCE
NOT_OBSERVED
NOT_VERIFIABLE
SKIPPED
```

## AW-011 timing

`AW-011 Command Profiler / Fast Validation Advisor` belongs after the first validation and report foundations exist.

It should not block Phase 1. It should be implemented only after:

- build/test/smoke validation evidence exists;
- git run reports exist;
- validation command suggestion behavior is designed or implemented;
- command-output privacy rules are accepted.

Value target:

```text
Reduce command-loop waste by recommending targeted validation and avoiding large terminal logs in agent context.
```

## Market Validation Gate M — before product/infrastructure expansion

Goal: determine whether evidence reports change real decisions and produce repeat use.

Run a manual-assisted study on 30 real AI-assisted PRs.

For each PR produce:

1. declared task and scope;
2. changed files and out-of-scope changes;
3. agent completion claims;
4. observed commands;
5. build/test/CI evidence;
6. evidence-to-commit match;
7. unsupported, stale, contradicted, or missing claims;
8. remaining risks;
9. reviewer action list.

Measure:

- whether the report changes reviewer action;
- review minutes saved or added;
- false positives;
- missing/stale evidence discovered;
- repeat use;
- requests for CI/GitHub automation;
- willingness to pay.

Directional advance criteria:

```text
At least 30% of reports change a real review action.
At least 50% of test users request or perform repeat use.
At least 3 teams request automated GitHub/CI execution.
At least 2 teams accept a paid or explicitly budgeted pilot.
```

These are internal product thresholds, not industry benchmarks.

### Gate M decisions

- **Advance:** repeat use and decision impact exist; automate the most repeated manual step.
- **Revise:** problem is recognized but output is noisy, slow, or poorly presented.
- **Park:** value exists for a narrow segment but not enough for current focus.
- **Reject:** reports do not change decisions and users do not return.

Do not build a Team Server, billing system, broad GitHub App, or enterprise deployment before Gate M produces payment/budget evidence.

See `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`.

## Community Opportunity Incubator — runs beside Phase 2 discovery

Goal: validate high-signal problems without derailing the core MVP or assuming equal support across tools.

Research and contracts:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`;
- `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`;
- `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`;
- `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`;
- `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`;
- `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`.

### Incubator Gate A — problem validation

For each opportunity:

- five target-user interviews;
- three real problem examples;
- current substitute/workaround inventory;
- willingness-to-try and willingness-to-pay notes;
- measurable success and kill criteria.

The first PR Evidence opportunity uses the stricter Gate M 30-PR experiment.

No broad runtime implementation before this gate unless the work is a small reversible fixture/import/manual prototype.

### Incubator Gate B — runtime compatibility foundation

Implement AW-CAP-037 before provider-specific live features:

1. versioned `EffectiveRuntimeProfile` schema;
2. model-role, tool, and surface profiles;
3. observation/event capability declaration;
4. configured-versus-effective permission model;
5. environment and VCS/delivery detection;
6. deterministic support decision and fallback planner;
7. generic/manual adapter;
8. Linux/Windows compatibility report fixtures;
9. profile revision and mid-run downgrade behavior.

Definition of done:

- every requested advanced capability receives exactly one support mode;
- every non-Full mode has a reason and fallback;
- Full/Guarded modes cite capability sources and blind spots;
- unknown/conflicted capability cannot silently enable enforcement;
- chat, local CLI, IDE, cloud/PR, CI, read-only, no-git, container, remote, worktree, and monorepo fixtures are represented.

### Incubator Gate C — shared event/evidence foundation

Build only after the run/evidence model and initial runtime profile are stable:

1. normalized event schema;
2. synthetic fixture importer;
3. event timeline projection;
4. adapter static declaration plus dynamic handshake;
5. missing/unsupported event handling;
6. adapter-health and profile-revision events;
7. privacy/redaction tests.

The first event adapters should represent materially different cases, not two nearly identical local CLIs.

### Incubator Gate D — low-risk prototypes

Recommended order:

1. **PR Evidence Packet and Trust Ledger** — manual/local evidence first.
2. **Manual Context Snapshot and Resume Pack** — generic/manual target first.
3. **Rules Compiler and Drift Detector** — deterministic free wedge with target-loss reports.
4. **Compatibility CLI** — detect/explain/compare/export runtime profiles.
5. **Offline Loop/Waste Analyzer** — no process termination or exact billing promise.
6. **Flight Recorder Import and Timeline** — fixture/session-log import only.
7. **Policy Dry-Run and Explain** — enforcement class and bypass disclosure, no execution broker.
8. **Coordination Diagnostics/Planner** — workspace, ownership, stale-state, duplicate-work, and mode selection.
9. **Regression Canary** — complete comparison profile and confounder detection.

Every prototype consumes an `EffectiveRuntimeProfile` or explicitly selects the generic/manual profile.

### Incubator Gate E — cross-surface dogfood decision

A prototype advances only when:

- at least three users recognize the output as useful;
- false positives are acceptable;
- required inputs are locally available or provided through a documented export;
- support modes downgrade correctly;
- output changes a real decision or saves measurable work;
- maintenance cost of adapters is understood.

Minimum compatibility dogfood:

- one rich-event local tool;
- one local no-hook/wrapper flow;
- one IDE/local flow;
- one cloud/PR flow;
- one chat/manual flow;
- one constrained read-only or no-network flow.

### Incubator Gate F — live/control behavior

Only after import-only/dry-run, Gate M evidence, and compatibility dogfood:

- live loop warnings;
- checkpoints and optional stopping when AgentsWatch owns the process or a supported stop API exists;
- policy enforcement only at a declared PE3/PE4/PE5 class;
- provider hooks with handshake and downgrade;
- controlled worker launching with exact workspace identity;
- GitHub posting with explicit approval.

These require separate safety, failure, privacy, compatibility, and user-value gates.

## Phase 3: Verified distribution and GitHub Action

Goal: make proven local value easy to install and automate.

Sequence:

1. verified .NET tool package;
2. isolated clean-install proof;
3. standalone executables when maintenance is justified;
4. package-manager distribution where testable;
5. local PR Evidence command;
6. GitHub Action after at least three teams request automation.

The GitHub Action should initially publish an artifact/job summary before automatic comments/checks.

## Phase 4: Optional local dashboard/service

Goal: visual run history and live local event value after enough data exists.

Do not build merely to make the product look complete.

Prerequisites:

- repeated local use;
- users struggle with terminal/markdown history;
- at least three users request history/dashboard value;
- live hooks/events justify a resident process;
- CLI remains independently useful.

Suggested stack:

- backend: .NET local API;
- frontend: React;
- storage: SQLite.

Dashboard pages:

- Runs;
- Tasks;
- Changed files;
- PR Evidence/Trust Ledger;
- Risk report;
- Token/waste findings;
- Validation results;
- Command profile;
- Changelog;
- Runtime compatibility profile and revisions;
- Event timeline when available;
- Context snapshots when available;
- Loop findings when available.

The first CLI release must not install/start a daemon automatically.

## Phase 5: Team beta and GitHub App

Add only after local/Action use and paid or budgeted pilot evidence:

- GitHub PR diff/check ingestion;
- PR Evidence checks;
- shared policy/rules packs;
- audit/evidence export;
- local worktree and cloud branch/PR coordination views;
- team usage overview with metric provenance;
- optional minimal metadata sync;
- GitHub App only when Action/manual workflow limitations are documented.

## Phase 6: Paid SaaS/Team Server

Only after local CLI, PR workflow, payment, privacy, and data-minimization proof.

Add:

- auth;
- billing;
- explicit opt-in metadata sync;
- teams;
- hosted metadata dashboards;
- historical analytics;
- organization policies.

Source, prompts, raw events, full diffs, full terminal logs, and complete local history remain local by default.

Infrastructure should be added only when it removes a proven bottleneck or supports existing paid demand.

## Core MVP priority order

1. Init `.ai` folder.
2. Git diff tracker.
3. Run report.
4. Risk scoring.
5. Prompt optimizer.
6. Prompt splitter.
7. Handoff summary.
8. Changelog generator.
9. Validation runner.
10. Claimed-vs-actual diff.
11. Commit-bound evidence identity.
12. Command Profiler / Fast Validation Advisor.
13. Evidence/proof validation.
14. Local PR Evidence Packet.

Reason: git/diff/report/validation/evidence creates immediate value before advanced integrations and directly supports the first market experiment.

## Opportunity prototype priority after the core spine

1. PR Evidence Packet and Trust Ledger.
2. Manual Context Snapshot and Session Resume.
3. Rules Compiler and target-loss/drift report.
4. AW-CAP-037 runtime profile and support decision foundation.
5. Compatibility CLI and fixture matrix.
6. Offline Cost/Loop/Waste Analyzer.
7. Flight Recorder import/timeline.
8. Policy Firewall dry-run/explain.
9. Multi-agent workspace/ownership diagnostics.
10. Regression Canary.

## Explicitly deferred

- generic AI bug-review competition as the main product;
- broad GitHub App before local/Action demand;
- Team Server before paid/budgeted pilots;
- dashboard/daemon before repeated local-history/live-event need;
- live process termination without process ownership, checkpoint, and opt-in;
- claims of equal cross-tool support;
- policy enforcement without a declared enforcement class;
- autonomous merge/deploy/release;
- full IDE replacement;
- cloud-only memory;
- unrestricted subagent communication;
- full multi-agent orchestration before diagnostic demand;
- security claims before independent review;
- exact provider billing reconstruction from incomplete telemetry;
- pure model comparisons when tool, permissions, or environment changed;
- public savings/popularity claims before measured validation.
