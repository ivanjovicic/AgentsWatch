# AgentsWatch Product Spec

Last aligned: 2026-07-17  
Status: planning/specification

## Description

AgentsWatch is a local-first, vendor-neutral control and evidence plane for AI coding agents.

It does not replace Codex, Cursor, Claude Code, Copilot, Devin, OpenHands, or Superplane. External tools execute coding work. AgentsWatch:

- converts roadmap items into bounded execution contracts;
- recommends an appropriate model/tool route;
- verifies what the agent actually changed;
- requires compact evidence before completion;
- learns which execution path works best for the repository.

## Core promise

```text
Turn roadmap intent into verified change — across any coding agent.
```

Supporting promise:

```text
Spend fewer tokens. Prove every change. Do not repeat avoidable mistakes.
```

Do not publish token-saving percentages until dogfood evidence supports them.

## Category

```text
Local coding-agent governance, evidence, and learning layer.
```

AgentsWatch sits between planning and execution:

```text
Roadmap / issue / prompt
  -> AgentsWatch run contract
  -> Codex / Cursor / Claude / Copilot / Devin / OpenHands
  -> AgentsWatch run receipt and evidence gate
  -> next roadmap decision
```

## Target users

Primary:

- solo developers using multiple coding agents;
- developers working across .NET, Flutter, React, Python, Node, or mixed repositories;
- developers with usage limits or high agent spend;
- developers who need reviewable local evidence;
- developers who want roadmap-driven execution without uncontrolled autonomy.

Later:

- small teams comparing agent outcomes;
- maintainers reviewing AI-generated pull requests;
- organizations requiring explainable policy and evidence gates.

## Problems

Coding-agent platforms already execute tasks, run in parallel, maintain sessions, schedule work, and create pull requests.

The remaining cross-vendor problems are:

- roadmap items are vague and not machine-checkable;
- each vendor records runs differently;
- agent claims are not consistently checked against diff and validation;
- scope drift is discovered late;
- broad validation and large logs waste time and context;
- session learning is often vendor-specific or stored as generic knowledge;
- model selection is usually generic rather than based on repository-local outcomes;
- completion status often reflects agent confidence rather than evidence.

## Differentiated product pillars

### 1. Roadmap Contract Compiler

Convert a roadmap item into:

```text
ID
intent
acceptance criteria
owned paths
avoid paths
dependencies
permission mode
run mode
token budget
risk gates
validation contract
stop rules
expected evidence
```

Incomplete contracts produce investigation/planning prompts, not implementation prompts.

### 2. Agent Run Receipt

Create one vendor-neutral local record containing:

```text
prompt/roadmap item
agent/model/tool
files inspected
files changed
commands and compact profiles
validation evidence
claims
risk findings
scope drift
missed work
learning note
next prompt
```

The receipt must remain useful without full chat history.

### 3. Evidence and Drift Gate

Compare:

```text
roadmap intent
vs acceptance criteria
vs agent claims
vs actual diff
vs validation evidence
```

Produce explainable findings and prevent `Done` when evidence is insufficient.

### 4. Counterfactual Learning

After failed, expensive, or drifting runs, generate:

- the smaller prompt that should have been used;
- the minimum context that would have been enough;
- the cheaper model/tool route that may have sufficed;
- the smallest validation ladder;
- a specific `do not repeat` rule.

Rules require scope, evidence count, confidence, and expiry/deprecation behavior.

### 5. Project-Local Empirical Router

Recommend the cheapest sufficient model/tool using repository-local evidence:

- task type;
- quality/evidence result;
- scope discipline;
- validation efficiency;
- retries;
- elapsed time;
- provider cost/token data when available.

Every recommendation must show reasons, confidence, and fallback.

### 6. Validation Economy

Profile and reduce waste from:

- repeated commands;
- unnecessarily broad test suites;
- oversized terminal output;
- validation sequences that do not match changed files;
- repeated failures without investigation.

## Signature metrics

```text
Contract Completeness Score
Scope Drift Score
Evidence Score
Validation Efficiency
Avoidable Work Estimate
Repeat Mistake Rate
Router Confidence
Roadmap Progress Confidence
```

Metrics must be explainable. Proxy values must be labeled as estimates.

## Product layers

1. **Local CLI** — contract, receipt, evidence and learning spine.
2. **Thin adapters/MCP** — integrate external agents without owning their runtime.
3. **Local dashboard** — only after CLI dogfood proves value.
4. **Team edition** — only after local cross-agent evidence is useful.

## MVP wedge

The first credible product is not a broad agent platform.

It is:

```text
Roadmap item -> bounded run contract -> external agent -> verified Agent Run Receipt
```

Required MVP capabilities:

1. `.ai` initialization and config.
2. Task/run lifecycle.
3. Roadmap or prompt contract creation.
4. Git start/end snapshot.
5. Agent Run Receipt.
6. Claims-vs-diff checker.
7. Validation evidence capture.
8. Evidence Score and Scope Drift findings.
9. Compact handoff and next prompt.
10. One learning note per run.

## Phase 2 differentiated capabilities

- roadmap status updated from receipts;
- mistake patterns with confidence and expiry;
- command profiler and validation economy;
- cross-agent normalized history;
- first empirical model/tool recommendations;
- compare two agents on equivalent task types;
- counterfactual prompt and validation suggestions.

## Integration direction

AgentsWatch should later expose:

- CLI commands;
- MCP tools;
- Codex skill/plugin;
- Cursor preflight/postflight adapter;
- GitHub check or agent app;
- Superplane preflight/postflight component;
- OpenHands and Devin session import adapters.

## Non-goals for v1

Do not build:

- another coding agent;
- agent reasoning loop;
- cloud sandbox/runtime;
- visual workflow canvas;
- generic scheduling;
- generic knowledge/playbook library;
- full chat archive;
- production deployment orchestration;
- automatic merge/release;
- hosted dashboard before CLI proof;
- exact token accounting without provider data;
- opaque auto-routing;
- autonomous background execution before evidence and risk gates work.

## Privacy

Default behavior:

- local-first;
- no telemetry;
- no source, prompt, diff, command-log, receipt, or learning upload;
- compact evidence rather than full session capture;
- secret redaction before persistence;
- external integrations explicit and opt-in.

## Commercial trial and licensing — post-MVP

AgentsWatch may later offer a permanent free tier plus a time-limited or usage-limited Pro trial.

Licensing must not upload repository content, encrypt user data, or block access to user-owned receipts and reports.

See:

- `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`
- `docs/TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- `docs/prompt_queues/agentwatch_trial_licensing.md`
