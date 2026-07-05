# AgentsWatch Product Spec

Last aligned: 2026-07-05  
Status: planning/specification; capability claims governed by proof registry

## Description

AgentsWatch is a local-first evidence and control layer for AI coding-agent work.

It does not replace Codex, Cursor, Claude Code, Copilot, ChatGPT, or code-review products. It helps developers run smaller, safer, more reviewable tasks and independently verify what an agent changed, executed, tested, proved, skipped, or only claimed.

## Core promise

Initial public-facing promise:

```text
Know what your coding agent changed, executed, tested, and missed.
```

Alternative tested wording:

```text
AgentsWatch verifies the work, not just the code.
```

Existing broader promise:

```text
Spend fewer tokens. Merge safer AI code.
```

The broader phrase remains valid as a direction, but the first market experiment should lead with evidence and completion integrity rather than token savings.

## Evidence-safe positioning

```text
AgentsWatch is designed to reduce avoidable context, repeated work, scope creep, and evidence mistakes through prompt splitting, scope limits, git/command/CI evidence, compact handoffs, compatibility-aware adapters, learning, discovery routing, and proof gates.
```

Candidate long-term internal positioning:

```text
The local control and evidence plane for coding agents: observe, bound, resume, coordinate, and verify work across tools.
```

The phrase `control plane` remains an internal/market hypothesis. It should not be the first public tagline until users recognize and value the category.

See `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`.

## First market-facing product

Recommended first product experiment:

```text
AgentsWatch PR Evidence + Trust Ledger
```

It should answer:

```text
What did the agent claim?
What was independently observed?
Which commands and tests ran?
For which commit did they run?
What changed outside declared scope?
What remains unverified or stale?
What should a human review next?
```

The first implementation should prefer independent evidence from git, commands, artifacts, and CI over another model-generated review opinion.

## Efficiency hypothesis

The previous `30-50%` target remains a product hypothesis, not a proven public result.

A numerical token/time/cost claim may be used only after paired benchmark, quality guardrail, sample-size, and independent review requirements in `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` are satisfied.

Until then, do not present `30-50%`, `70%+`, or any other percentage as measured product performance.

## Problem

AI coding agents often create trust and workflow debt because they:

- inspect too many files;
- repeat searches and failed commands;
- repeat slow validation after irrelevant or no changes;
- paste large terminal logs into model context;
- mix investigation, implementation, tests, docs, and review in one run;
- continue after the prompt should stop;
- edit unrelated files;
- claim tests, commands, or validation without matching evidence;
- bind evidence to an older commit than the current change;
- present inference, partial checks, or tautological validation as proof;
- rely on long chat history instead of durable handoff summaries;
- lose decisions during compaction, account changes, limits, or provider switching;
- consume quotas or cost without clear attribution;
- run parallel workers in the wrong workspace or with overlapping ownership;
- duplicate repository rules across vendor-specific files;
- notice useful out-of-scope issues but fail to preserve and route them;
- describe planned functionality as already implemented;
- increase code-production volume faster than teams can review it.

Community and market evidence is summarized in:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`.

## Target users

### Primary initial users

- solo developers and tech leads using coding agents heavily;
- developers reviewing agent-authored changes;
- small teams with increasing AI-assisted PR volume;
- developers switching between two or more coding-agent tools;
- developers who want reviewable AI-agent history and truthful capability evidence.

### Later users

- maintainers receiving agent-assisted contributions;
- teams running multiple agents in worktrees or cloud branches;
- security/platform teams defining repository-specific permissions;
- managers who want policy, cost, risk, and evidence visibility.

## Product layers

1. Local CLI — first product, using git, markdown, shell commands, config, and explicit local evidence imports.
2. GitHub Action/PR Evidence workflow — first distributed team integration after local value is proven.
3. Local dashboard/service — optional after sufficient local history and live-event value exist.
4. Team/GitHub App edition — review packets, policy packs, multi-agent evidence, and audit export.
5. SaaS/Team Server edition — later only after local use, privacy, payment, and evidence are proven.

See `PRODUCT_FORM_FACTORS_INSTALLATION_AND_DELIVERY_PLAN.md`.

## Strategic product lanes

### Lane A — Scope and prompt discipline

Current foundation:

- prompt optimizer;
- task splitting;
- scope limiter;
- stop rules;
- context packs.

### Lane B — Evidence and trust

Highest-priority progression:

- git/run evidence;
- claims-vs-actual checks;
- commit-bound build/test/CI evidence;
- PR Evidence Packet;
- claim-to-evidence Trust Ledger;
- imported Flight Recorder timeline;
- proof bundle integration;
- live event recording only after compatibility and privacy gates.

Required status vocabulary:

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

### Lane C — Context portability

Second-priority progression:

- handoff summary;
- context snapshot;
- fresh-session resume pack;
- versioned decisions and unresolved risks;
- provider-specific export;
- explicit represented/shortened/dropped/unsupported loss report;
- context-loss comparison.

### Lane D — Rules portability

Adoption-wedge progression:

- canonical rules source;
- rules lint;
- target compilation;
- drift detection;
- target-loss report;
- managed-policy conflict detection.

Target result vocabulary:

```text
EXACT
EQUIVALENT
WEAKER
ADVISORY_ONLY
UNSUPPORTED
CONFLICT_WITH_MANAGED_POLICY
```

### Lane E — Waste and loop control

Planned progression:

- command profiler;
- token/context waste report;
- offline repeated-action analysis;
- wall-clock and observable-work ledger;
- provider usage import where available;
- budget/checkpoint recommendation;
- optional live loop guard after dogfood.

Do not convert unknown quota percentages into exact token or currency claims.

### Lane F — Safety and permissions

Later progression:

- deterministic risk/policy findings;
- sensitive-path rules;
- policy dry-run/explain;
- effective-permission report;
- approval bundles;
- optional execution broker after security review.

Do not call advisory or incomplete interception a firewall.

### Lane G — Parallel-agent coordination

Later progression:

- workspace identity and ownership diagnostics;
- worktree/branch planner;
- worker bootstrap packs;
- heartbeat, handoff, stale-state, and duplicate-work checks;
- shared structured status/findings;
- integration readiness;
- no automatic merge/deploy.

Do not begin with a full autonomous orchestrator.

### Lane H — Review debt reduction

Initial/team progression:

- diff-only review prompt;
- claims-vs-diff/test/CI evidence;
- local PR Evidence Packet;
- reviewer order by risk and missing proof;
- GitHub Action;
- optional GitHub App;
- maintainer/OSS policy profile.

Do not compete primarily as a generic AI bug reviewer.

## Revised opportunity priority

Current product priority based on external problem evidence and strategic fit:

1. PR Evidence Packet, completion integrity, and Trust Ledger;
2. commit-bound claims-vs-actual validation;
3. Context Snapshot/Resume;
4. Rules Compiler and target-loss/drift report;
5. offline Loop/Waste Analyzer;
6. runtime compatibility detect/explain;
7. import-only Flight Recorder timeline;
8. Policy Dry Run/Explain;
9. GitHub Action and team evidence automation;
10. multi-agent diagnostics/ownership planner;
11. Regression Canary;
12. live enforcement and full coordination only after proof.

All items remain governed by `FEATURE_CAPABILITY_REGISTRY.md` maturity.

## Competitive boundary

AgentsWatch should not lead with:

```text
An AI reviewer that finds more bugs than every competing reviewer.
```

The initial differentiation is:

- verification of the work, not only review of final code;
- independent git/command/artifact/CI evidence;
- commit matching;
- declared scope versus actual changes;
- explicit unknown/not-observed status;
- cross-tool compatibility and fallback;
- local ownership and data minimization.

Static analyzers and external AI reviewers may later be evidence inputs, not the product identity.

## First market validation experiment

Run a manual-assisted study on 30 real AI-assisted PRs.

For every PR generate:

1. declared task and scope;
2. changed files and out-of-scope changes;
3. agent completion claims;
4. observed commands;
5. build/test/CI evidence;
6. evidence-to-commit match;
7. unsupported, stale, or contradicted claims;
8. remaining risks;
9. reviewer action list.

Directional advance criteria:

```text
At least 30% of reports change a real review action.
At least 50% of test users request or perform repeat use.
At least 3 teams request automated GitHub/CI execution.
At least 2 teams accept a paid or explicitly budgeted pilot.
```

These are internal product thresholds, not industry benchmarks.

## Adoption and monetization hypothesis

### Free/open wedge

- Rules Compiler and target-loss/drift report;
- basic context snapshot/export;
- basic local PR/run evidence report;
- local workspace doctor;
- public-repository GitHub Action allowance later.

### Pro solo value

- advanced Context Resume;
- Trust Ledger history and comparison;
- offline Loop/Waste Analyzer;
- supported premium adapters where justified;
- regression canaries later.

### Team value

- private-repository PR Evidence automation;
- shared policy/rules packs;
- team audit/history;
- organization compatibility reports;
- multi-agent diagnostics;
- signed/tamper-evident evidence later.

Pricing and packaging remain hypotheses until repeated usage and willingness-to-pay evidence exist.

## Capability truth

The authoritative capability/maturity state lives in:

- `FEATURE_CAPABILITY_REGISTRY.md`;
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`;
- commit-bound CI/proof bundles.

Roadmaps, external research, and this spec describe direction. They do not prove runtime support, usefulness, popularity, or market size.

## MVP feature list

### Core MVP 1 direction

- `.ai` folder generator;
- prompt optimizer;
- prompt splitter;
- scope limiter;
- git diff tracker;
- basic risk scoring;
- markdown run report;
- changelog generator.

### Core MVP 2 direction

- acceptance-criteria checker;
- claimed-vs-actual diff checker;
- validation runner;
- handoff summary generator;
- token/context waste report;
- diff-only review prompt generator;
- command profiler / fast validation advisor;
- mistake learning;
- discovery capture/reconciliation;
- capability proof and release evidence.

### Evidence-first market prototype after the core spine

- local PR Evidence Packet;
- Trust Ledger claim classification;
- commit-bound validation evidence;
- Context Snapshot/Resume;
- Rules Compiler and target-loss report;
- offline Loop/Waste Analyzer;
- compatibility report;
- event/session import timeline.

The registry distinguishes which items are specified, implemented, tested, CI-verified, dogfood-verified, or release-verified.

## Proof principle

```text
No registry row = no supported feature claim.
No executed evidence = no verified claim.
No commit match = no proof for this version.
Community discussion = problem signal, not market validation.
Adjacent paid products = budget signal, not willingness to buy AgentsWatch.
```

Use `PROOF_AND_VERIFICATION_STRATEGY.md` for L0-L6 maturity and `PROOF_BUNDLE_SPEC.md` for CI/release evidence.

## Community opportunity gate

Before broad implementation of advanced capabilities:

1. interview at least five target users for the opportunity;
2. collect at least three real examples;
3. identify stable local evidence/event inputs;
4. build manual, import-only, or dry-run prototype first;
5. define measurable success and kill criteria;
6. prove privacy boundaries;
7. avoid live enforcement until offline analysis is useful and low-noise.

For the first market-facing PR Evidence product, use the stricter 30-PR experiment in `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`.

## Command profiler principle

The Command Profiler / Fast Validation Advisor belongs before the live Cost and Loop Guard.

```text
Profile commands locally. Show agents only compact command evidence.
```

See `COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`.

## Discovery principle

```text
Do not fix unrelated work inside the current task.
Do not lose it either.
```

See `DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`.

## Commercial trial and licensing — post-MVP

AgentsWatch may later offer a permanent free tier plus a time/usage-limited Pro trial.

Commercial protection must follow these truths:

- local files and generated output shown to the user cannot be made impossible to copy;
- premium implementation details should not be shipped as editable plaintext when avoidable;
- enforcement should use server-signed entitlements and an offline-capable lease;
- user source, prompts, diffs, validation output, reports, discoveries, event journals, and run history stay local by default;
- license calls must be visible and must not upload repository content;
- expiration must never encrypt/delete/corrupt user-owned data;
- licensing runtime work starts only after CLI MVP, repeat use, and willingness-to-pay proof.

See:

- `TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`;
- `prompt_queues/agentwatch_trial_licensing.md`.

## Non-goals for v1

Do not start with:

- another autonomous coding agent;
- a foundation model;
- a generic AI bug reviewer as the main product;
- a full IDE;
- SaaS or billing;
- runtime DRM before CLI value is proven;
- cloud-only memory or event storage;
- automatic merge/deploy/release;
- deep IDE integration before local CLI evidence;
- automatic unrelated code editing;
- perfect token or provider billing reconstruction;
- broad policy/firewall claims without enforcement proof;
- full multi-agent orchestration;
- claims of complete prompt-injection/security prevention;
- unsupported numerical savings or popularity claims;
- uploading source, full logs, proof artifacts containing private content, or run history by default.
