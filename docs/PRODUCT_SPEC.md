# AgentsWatch Product Spec

Last aligned: 2026-07-03  
Status: planning/specification; capability claims governed by proof registry

## Description

AgentsWatch is an AI coding-agent supervisor and token/context-waste optimizer for developers.

It does not replace Codex, Cursor, Claude Code, Copilot, or ChatGPT. It sits above them and helps developers run smaller, safer, more reviewable coding-agent tasks.

## Core promise

```text
Spend fewer tokens. Merge safer AI code.
```

Current evidence-safe positioning:

```text
AgentsWatch is designed to reduce avoidable context, repeated work, scope creep, and evidence mistakes through prompt splitting, scope limits, git evidence, compact handoffs, learning, discovery routing, and proof gates.
```

Candidate expanded positioning from July 2026 community research:

```text
The local control plane for coding agents: observe, bound, resume, coordinate, and verify work across tools.
```

The phrase `control plane` is a positioning hypothesis. It must be tested with users before becoming the main public tagline.

## Efficiency hypothesis

The previous `30-50%` target remains a product hypothesis, not a proven public result.

A numerical token/time/cost claim may be used only after the paired benchmark, quality guardrail, sample-size, and independent review requirements in `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` are satisfied.

Until then, do not present `30-50%`, `70%+`, or any other percentage as measured product performance.

## Problem

AI coding agents often waste context and create risk because they:

- inspect too many files;
- repeat searches and failed commands;
- repeat slow validation after irrelevant or no changes;
- paste large terminal logs into model context;
- mix investigation, implementation, tests, docs, and review in one run;
- continue after the prompt should stop;
- edit unrelated files;
- claim tests or validation without evidence;
- rely on long chat history instead of handoff summaries;
- lose decisions during compaction, account changes, limits, or provider switching;
- consume quotas or cost without clear attribution;
- run parallel workers in the wrong workspace or with overlapping ownership;
- duplicate repository rules across vendor-specific files;
- notice useful out-of-scope issues but fail to preserve and route them;
- describe planned functionality as already implemented.

Community and research evidence is summarized in `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`.

## Target users

Primary:

- solo developers using coding agents heavily;
- developers working across .NET, React, Flutter, Python, Node, or mixed repos;
- power users with usage limits or high AI spend;
- developers switching between two or more coding-agent tools;
- developers who want reviewable AI-agent history and truthful capability evidence.

Later:

- small teams reviewing AI-assisted pull requests;
- maintainers receiving agent-assisted contributions;
- teams running multiple agents in worktrees;
- security/platform teams defining repository-specific permissions;
- managers who want policy, cost, risk, and evidence visibility.

## Product layers

1. Local CLI — first product, using git, markdown, shell commands, config, and local event imports.
2. Local dashboard — optional after CLI value is proven.
3. Team/GitHub edition — review packets, policy packs, multi-agent evidence, and audit export.
4. SaaS edition — later only after local use, privacy, and evidence are proven.

## Strategic product lanes

### Lane A — Scope and prompt discipline

Current foundation:

- prompt optimizer;
- task splitting;
- scope limiter;
- stop rules;
- context packs.

### Lane B — Evidence and trust

Planned progression:

- git/run evidence;
- claims-vs-actual checks;
- Agent Flight Recorder;
- claim-to-evidence Trust Ledger;
- replayable run timeline;
- proof bundle integration.

### Lane C — Context portability

Planned progression:

- handoff summary;
- context snapshot;
- fresh-session resume pack;
- rules compiler and drift detector;
- provider-specific export;
- context-loss comparison.

### Lane D — Waste and loop control

Planned progression:

- command profiler;
- token/context waste report;
- offline repeated-action analysis;
- budget ledger;
- checkpoint/stop recommendation;
- optional live loop guard after dogfood.

### Lane E — Safety and permissions

Planned progression:

- deterministic risk/policy findings;
- sensitive-path rules;
- policy dry-run/explain;
- approval bundles;
- optional execution broker after security review.

### Lane F — Parallel-agent coordination

Planned progression:

- worktree/ownership planner;
- worker bootstrap packs;
- shared structured status/findings;
- integration readiness;
- no automatic merge/deploy.

### Lane G — Review debt reduction

Planned progression:

- diff-only review prompt;
- claims-vs-diff/test evidence;
- local PR reviewer packet;
- optional GitHub adapter;
- maintainer/OSS policy profile.

## Opportunity priority

Community-derived ranking is maintained in `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`.

Highest-priority hypotheses:

1. Agent Flight Recorder and Trust Ledger;
2. Context and Memory Portability;
3. Cost and Loop Guard;
4. Agent Rules Compiler and Drift Detector;
5. Policy Firewall;
6. Multi-Agent Worktree Coordinator;
7. AI PR Review Debt Reducer;
8. Agent Regression Canary.

These remain L1 specified capabilities until implementation and proof exist.

## Adoption and monetization hypothesis

### Free/open wedge

- rule compiler/drift detector;
- basic context snapshot/export;
- basic evidence timeline viewer;
- local workspace doctor.

### Pro solo value

- Cost and Loop Guard;
- advanced session rescue;
- Trust Ledger verification;
- regression canaries and comparisons.

### Team value

- Policy Firewall;
- signed/tamper-evident evidence later;
- PR Review Debt Reducer;
- multi-agent worktree coordination;
- shared policy and audit export.

Pricing and packaging remain hypotheses until usage and willingness-to-pay interviews exist.

## Capability truth

The authoritative capability/maturity state lives in:

- `FEATURE_CAPABILITY_REGISTRY.md`;
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`;
- commit-bound CI/proof bundles.

Roadmaps, community research, and this spec describe direction. They do not prove runtime support or popularity.

## MVP feature list

MVP 1 direction:

- `.ai` folder generator;
- prompt optimizer;
- prompt splitter;
- scope limiter;
- git diff tracker;
- basic risk scoring;
- markdown run report;
- changelog generator.

MVP 2 direction:

- acceptance-criteria checker;
- claimed-vs-actual diff checker;
- validation runner;
- handoff summary generator;
- token waste report;
- diff-only review prompt generator;
- command profiler / fast validation advisor;
- mistake learning;
- discovery capture/reconciliation;
- capability proof and release evidence.

Community opportunity incubator, after the existing run/evidence spine:

- event import and Flight Recorder timeline;
- context snapshot and rules compiler;
- offline loop analysis;
- policy dry-run;
- worktree ownership planner;
- local PR review packet.

The registry distinguishes which items are specified, implemented, tested, CI-verified, dogfood-verified, or release-verified.

## Proof principle

```text
No registry row = no supported feature claim.
No executed evidence = no verified claim.
No commit match = no proof for this version.
Community discussion = problem signal, not market validation.
```

Use `PROOF_AND_VERIFICATION_STRATEGY.md` for L0-L6 maturity and `PROOF_BUNDLE_SPEC.md` for CI/release evidence.

## Community opportunity gate

Before implementing AW-CAP-028 through AW-CAP-036:

1. interview at least five target users for the opportunity;
2. collect at least three real examples;
3. identify stable local evidence/event inputs;
4. build manual, import-only, or dry-run prototype first;
5. define measurable success and kill criteria;
6. prove privacy boundaries;
7. avoid live enforcement until offline analysis is useful and low-noise.

See `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`.

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
- licensing runtime work starts only after CLI MVP and dogfood proof.

See:

- `TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`;
- `prompt_queues/agentwatch_trial_licensing.md`.

## Non-goals for v1

Do not start with:

- another autonomous coding agent;
- a foundation model;
- a full IDE;
- SaaS or billing;
- runtime DRM before CLI value is proven;
- cloud-only memory or event storage;
- automatic merge/deploy/release;
- deep IDE integration before local CLI evidence;
- automatic unrelated code editing;
- perfect token or provider billing reconstruction;
- claims of complete prompt-injection/security prevention;
- unsupported numerical savings or popularity claims;
- uploading source, full logs, proof artifacts containing private content, or run history by default.
