# AgentsWatch MVP Roadmap

Last aligned: 2026-07-03  
Status: planning/specification

## Strategy

Do not start with SaaS. Start with a local CLI that works with any repo through git, markdown, shell commands, config, and explicit local evidence imports.

Recommended first product:

```text
AgentsWatch CLI — AI coding-agent supervisor and local control plane.
```

The `control plane` positioning remains a user-validation hypothesis.

## Roadmap rules

- Current code/tests/proof override this roadmap.
- Community demand signals do not prove market demand.
- A model or provider name does not prove runtime capability.
- Tool, surface, observation, permission, environment, and VCS/delivery profiles must be resolved independently.
- Every advanced feature must expose `Full`, `Guarded`, `Advisory`, `PostHoc`, `Manual`, or `Unavailable` support.
- New community-derived capabilities remain incubator work until Gate 0, user discovery, the run/evidence spine, and the required compatibility gate are complete.
- Import-only and dry-run slices come before live monitoring or enforcement.
- Do not build independent provider pipelines; share runtime-profile, normalized-event, evidence, and fallback foundations.

## Prototype: 3-7 days

Goal: prove the workflow manually with markdown files.

Deliverables:

- `.ai/` folder shape;
- example optimized prompt set;
- example run report;
- example risk report;
- example handoff summary;
- example changelog entry.

No database, no dashboard, no SaaS.

## Phase 1: CLI MVP, 2-4 weeks

Goal: useful local tool.

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

## Phase 2: Useful solo tool, 4-8 weeks total

Add:

1. Acceptance criteria checker.
2. Claimed-vs-actual diff checker.
3. Validation runner.
4. Handoff summary generator.
5. Token waste report.
6. Diff-only review prompt generator.
7. Language adapters for Flutter, .NET, React/TypeScript, Python, Node.
8. Command Profiler / Fast Validation Advisor.
9. Mistake/discovery reconciliation.
10. Proof-bundle validation.

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

## Community Opportunity Incubator — runs beside Phase 2 discovery

Goal: validate high-signal problems without derailing the core MVP or assuming equal support across tools.

Research and contracts:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
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

No broad runtime implementation before this gate unless the work is a small reversible fixture/import spike.

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

1. **Rules Compiler and Drift Detector** — deterministic free wedge with target-loss reports.
2. **Manual Context Snapshot and Resume Pack** — generic/manual target first.
3. **Compatibility CLI** — detect/explain/compare/export runtime profiles.
4. **Flight Recorder Import and Timeline** — fixture/session-log import only.
5. **Trust Ledger** — git/process/CI evidence grades independent of agent self-report.
6. **Offline Loop Analyzer** — no process termination.
7. **Policy Dry-Run and Explain** — enforcement class and bypass disclosure, no execution broker.
8. **Coordination Planner** — select LocalWorktree, CloudBranchPR, SharedWorkspaceOwnership, MessageOnly, or Unavailable.
9. **PR Evidence Packet** — local range and cloud/PR variants.
10. **Regression Canary** — complete comparison profile and confounder detection.

Every prototype consumes an `EffectiveRuntimeProfile` or explicitly selects the generic/manual profile.

### Incubator Gate E — cross-surface dogfood decision

A prototype advances only when:

- at least three users recognize the output as useful;
- false positives are acceptable;
- required inputs are locally available or provided through a documented export;
- support modes downgrade correctly;
- the output changes a real decision or saves measurable work;
- maintenance cost of adapters is understood.

Minimum compatibility dogfood:

- one rich-event local tool;
- one local no-hook/wrapper flow;
- one IDE/local flow;
- one cloud/PR flow;
- one chat/manual flow;
- one constrained read-only or no-network flow.

### Incubator Gate F — live/control behavior

Only after import-only/dry-run and compatibility dogfood:

- live loop warnings;
- checkpoints and optional stopping when AgentsWatch owns the process or a supported stop API exists;
- policy enforcement only at a declared PE3/PE4/PE5 class;
- provider hooks with handshake and downgrade;
- controlled worker launching with exact workspace identity;
- GitHub posting with explicit approval.

These require separate safety, failure, privacy, and compatibility gates.

## Phase 3: Local dashboard, 8-12 weeks total

Goal: visual run history and risk review.

Suggested stack:

- backend: .NET local API;
- frontend: React;
- storage: SQLite.

Dashboard pages:

- Runs;
- Tasks;
- Changed files;
- Risk report;
- Token waste;
- Validation results;
- Command profile;
- Changelog;
- Runtime compatibility profile and revisions;
- Event timeline when available;
- Context snapshots when available;
- Loop findings when available.

The dashboard is not a prerequisite for compatibility detection, event import, or analysis.

## Phase 4: Team beta, 3-5 months

Add only after solo and cross-surface dogfood:

- GitHub PR diff/check ingestion;
- PR review packet;
- shared policy packs;
- audit/evidence export;
- local worktree and cloud branch/PR coordination views;
- team usage overview with metric provenance;
- optional minimal metadata sync.

## Phase 5: Paid SaaS, 6-9 months

Only after local CLI/dashboard shows real usage.

Add:

- auth;
- billing;
- explicit opt-in cloud sync;
- teams;
- hosted metadata dashboards;
- historical analytics;
- organization policies.

Source, prompts, raw events, and full diffs remain local by default.

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
11. Command Profiler / Fast Validation Advisor.
12. Evidence/proof validation.

Reason: git/diff/report creates immediate value before advanced integrations. Command profiling and evidence create the foundation for later trust, loop, policy, and coordination work.

## Opportunity prototype priority after the core spine

1. Rules Compiler and Drift Detector using generic target/loss reports.
2. Manual Context Snapshot and Session Resume.
3. AW-CAP-037 runtime profile and support decision foundation.
4. Compatibility CLI and fixture matrix.
5. Flight Recorder import/timeline.
6. Trust Ledger evidence grades.
7. Offline Cost and Loop Guard.
8. Policy Firewall dry-run.
9. Local/cloud PR Review Debt packets.
10. Multi-Agent coordination planner.
11. Regression Canary.

## Explicitly deferred

- live process termination without process ownership, checkpoint, and opt-in;
- claims of equal cross-tool support;
- policy enforcement without a declared enforcement class;
- autonomous merge/deploy/release;
- full IDE replacement;
- cloud-only memory;
- unrestricted subagent communication;
- security claims before independent review;
- pure model comparisons when tool, permissions, or environment changed;
- public savings/popularity claims before measured validation.
