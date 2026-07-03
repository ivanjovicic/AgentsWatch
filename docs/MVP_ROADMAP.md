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
- New community-derived capabilities remain incubator work until Gate 0, user discovery, and the existing run/evidence spine are complete.
- Import-only and dry-run slices come before live monitoring or enforcement.
- Do not build six independent pipelines; share the normalized event/evidence foundation.

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

Goal: validate the highest-signal problems without derailing the core MVP.

Research source:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`;
- `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`.

### Incubator Gate A — problem validation

For each opportunity:

- five target-user interviews;
- three real problem examples;
- current substitute/workaround inventory;
- willingness-to-try and willingness-to-pay notes;
- measurable success and kill criteria.

No runtime implementation before this gate unless the work is a very small reversible fixture/import spike.

### Incubator Gate B — shared event foundation

Build only after the existing run/evidence model is stable:

1. normalized event schema;
2. synthetic fixture importer;
3. event timeline projection;
4. adapter capability declaration;
5. missing/unsupported data handling;
6. privacy/redaction tests.

### Incubator Gate C — low-risk prototypes

Recommended order:

1. **Rules Compiler and Drift Detector** — deterministic free wedge.
2. **Manual Context Snapshot and Resume Pack** — no live adapter.
3. **Flight Recorder Import and Timeline** — fixture/session-log import only.
4. **Offline Loop Analyzer** — no process termination.
5. **Policy Dry-Run and Explain** — no execution broker.
6. **Worktree Ownership Planner** — dry-run only.
7. **Local PR Evidence Packet** — local git range only.

### Incubator Gate D — dogfood decision

A prototype advances only when:

- at least three users recognize the output as useful;
- false positives are acceptable;
- local-first input is available;
- the output changes a real decision or saves measurable work;
- maintenance cost of provider adapters is understood.

### Incubator Gate E — live/control behavior

Only after import-only/dry-run dogfood:

- live loop warnings;
- checkpoints and optional stopping;
- policy enforcement/execution broker;
- provider hooks;
- agent process spawning;
- GitHub posting.

These require separate safety, failure, and privacy gates.

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
- Event timeline when available;
- Context snapshots when available;
- Loop findings when available.

The dashboard is not a prerequisite for event import or analysis.

## Phase 4: Team beta, 3-5 months

Add only after solo dogfood:

- GitHub PR diff/check ingestion;
- PR review packet;
- shared policy packs;
- audit/evidence export;
- multi-agent worktree status;
- team usage overview;
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

Reason: git/diff/report creates immediate value before advanced agent integrations. Command profiling and evidence create the foundation for later loop, trust, policy, and coordination work.

## Opportunity prototype priority after the core spine

1. Rules Compiler and Drift Detector.
2. Context Snapshot and Session Resume.
3. Flight Recorder import/timeline.
4. Offline Cost and Loop Guard.
5. Policy Firewall dry-run.
6. Local PR Review Debt packet.
7. Multi-Agent Worktree planner.
8. Regression Canary.

## Explicitly deferred

- live termination of agent processes;
- autonomous merge/deploy/release;
- full IDE replacement;
- cloud-only memory;
- unrestricted subagent communication;
- security claims before independent review;
- public savings/popularity claims before measured validation.
