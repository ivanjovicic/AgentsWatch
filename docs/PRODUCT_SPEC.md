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

## Efficiency hypothesis

The previous `30-50%` target remains a product hypothesis, not a proven public result.

A numerical token/time/cost claim may be used only after the paired benchmark, quality guardrail, sample-size, and independent review requirements in `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` are satisfied.

Until then, do not present `30-50%`, `70%+`, or any other percentage as measured product performance.

## Problem

AI coding agents often waste context and create risk because they:

- inspect too many files;
- repeat searches;
- repeat slow validation commands after small changes;
- paste large terminal logs into model context;
- mix investigation, implementation, tests, docs, and review in one run;
- continue after the prompt should stop;
- edit unrelated files;
- claim tests or validation without evidence;
- rely on long chat history instead of handoff summaries;
- notice useful out-of-scope issues but fail to preserve and route them;
- describe planned functionality as already implemented.

## Target users

Primary:

- solo developers using coding agents heavily;
- developers working across .NET, React, Flutter, Python, Node, or mixed repos;
- power users with usage limits or high AI spend;
- developers who want reviewable AI-agent history and truthful capability evidence.

Later:

- small teams reviewing AI-assisted pull requests;
- maintainers who want AI-run evidence;
- managers who want policy/risk visibility.

## Product layers

1. Local CLI — first product, using git, markdown, shell commands, and local config.
2. Local dashboard — optional after CLI value is proven.
3. Team/SaaS edition — later only after local use, privacy, and evidence are proven.

## Capability truth

The authoritative capability/maturity state lives in:

- `FEATURE_CAPABILITY_REGISTRY.md`;
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`;
- commit-bound CI/proof bundles.

Roadmaps and this spec describe direction. They do not prove runtime support.

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

The registry distinguishes which items are specified, implemented, tested, CI-verified, dogfood-verified, or release-verified.

## Proof principle

```text
No registry row = no supported feature claim.
No executed evidence = no verified claim.
No commit match = no proof for this version.
```

Use `PROOF_AND_VERIFICATION_STRATEGY.md` for L0-L6 maturity and `PROOF_BUNDLE_SPEC.md` for CI/release evidence.

## Command profiler principle

The Command Profiler / Fast Validation Advisor is a planned docs-first epic.

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
- user source, prompts, diffs, validation output, reports, discoveries, and run history stay local by default;
- license calls must be visible and must not upload repository content;
- expiration must never encrypt/delete/corrupt user-owned data;
- licensing runtime work starts only after CLI MVP and dogfood proof.

See:

- `TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`;
- `prompt_queues/agentwatch_trial_licensing.md`.

## Non-goals for v1

Do not start with:

- SaaS;
- billing;
- runtime DRM before CLI value is proven;
- cloud sync;
- deep IDE integration;
- automatic unrelated code editing;
- perfect token counting;
- unsupported numerical savings claims;
- uploading command logs, proof artifacts containing private content, or run history by default.
