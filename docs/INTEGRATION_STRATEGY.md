# AgentsWatch Integration Strategy

Last aligned: 2026-07-03  
Status: future plan, not MVP implementation

## Purpose

Define integration order so AgentsWatch does not start with the hardest and riskiest work.

Use with `PRODUCT_FORM_FACTORS_INSTALLATION_AND_DELIVERY_PLAN.md`.

## Integration principle

```text
One product. Local CLI first. Optional components and integrations later.
```

A local workflow must be useful before any background service, IDE extension, GitHub Action/App, SaaS, or LLM-provider integration is built.

All integrations must reuse the same AgentsWatch core, compatibility engine, evidence model, policy contracts, and capability registry.

## Stage 1 — No external integration

MVP behavior:

- read local git state;
- read selected local files;
- write markdown reports;
- suggest validation commands;
- generate prompts/context packs;
- support generic/manual fallbacks.

No account, cloud, telemetry, daemon, or network required.

## Stage 2 — Local git and shell integration

Allowed after Gate 0:

- git status/diff snapshots;
- local command suggestions;
- explicit validation command execution later;
- markdown/JSON local artifacts;
- process-wrapper evidence where AgentsWatch owns the process;
- runtime compatibility profile detection.

Rules:

- validation execution must be explicit;
- command output must be redacted when sensitive;
- failed commands must be recorded honestly;
- process ownership must not be inferred;
- no provider hook installation without preview and approval.

## Stage 3 — Manual/import-only adapter workflow

Allowed before live provider integrations:

- user-supplied transcript/session exports;
- generic chat/manual evidence;
- imported CI/check artifacts;
- target-specific rules/context export;
- adapter feasibility and compatibility audits.

Rules:

- import does not execute embedded commands;
- missing events remain not observed;
- unsupported target semantics produce a loss report;
- no hidden upload.

## Stage 4 — GitHub workflow as local input

Allowed after CLI reports are useful.

Possible features:

- read PR diff;
- read CI/check status;
- generate PR risk/evidence report locally;
- generate PR comment/check draft without posting automatically;
- handle stacked PR base/merge-base correctly.

Rules:

- do not post comments automatically initially;
- do not require GitHub token for local core features;
- document every permission and data access;
- unavailable checks/secrets are not reported as failures.

## Stage 5 — GitHub Action

The GitHub Action should be the first distributed team integration before the full GitHub App.

Example target:

```yaml
- name: Verify AI-assisted change
  uses: agentswatch/verify-pr@v1
```

Possible features:

- run a pinned AgentsWatch CLI version in CI;
- inspect the current PR range;
- consume build/test/check evidence;
- produce an artifact or job summary;
- later publish a check/comment only when configured.

Prerequisites:

- useful local PR evidence packet;
- commit-bound proof;
- fork and restricted-token scenarios;
- no hidden source upload;
- Action/CLI version and supply-chain policy;
- public/private repository packaging decision.

## Stage 6 — First live local provider adapters

Allowed only after runtime compatibility negotiation and import-only value exist.

Recommended scope:

- one rich-event local tool adapter;
- one materially different local adapter or process-wrapper flow;
- static declaration plus dynamic handshake;
- mid-run downgrade on adapter/hook failure;
- local event journal.

Rules:

- do not infer support from model/provider name;
- do not install hooks silently;
- do not call incomplete interception Full enforcement;
- adapters remain optional;
- generic/manual fallback remains available.

## Stage 7 — Optional local background service

Allowed only when supported hooks/live warnings create enough value to justify a resident process.

Possible features:

- receive local adapter events;
- maintain local event journal;
- local notifications;
- loop warnings/checkpoints;
- local API for dashboard and IDE clients.

Rules:

- optional and explicitly enabled;
- visible start/stop/status;
- loopback-only default;
- bounded local retention;
- no cloud sync by default;
- CLI works without it;
- no automatic process termination without explicit gate and opt-in.

## Stage 8 — Local dashboard

Allowed after useful local run history exists.

Possible features:

- run/task history;
- changed files and risk;
- validation and Trust Ledger;
- context snapshots;
- compatibility/blind spots;
- adapter health;
- loop/usage findings;
- PR evidence packets.

Rules:

- same application core/use cases as CLI;
- local-only by default;
- not the only interface;
- no mandatory cloud account.

## Stage 9 — Thin IDE extension

First candidate: VS Code, after CLI/local API value is proven.

Possible features:

- show current scope/risk/validation state;
- link findings to files;
- invoke approved CLI/local API actions;
- display compatibility and adapter-health warnings.

Rules:

- thin client only;
- no duplicated evidence/policy/risk logic;
- safe degraded mode without local service;
- no silent control of the coding agent.

## Stage 10 — GitHub App

Allowed only after the manual and GitHub Action PR workflows prove value.

Possible features:

- PR checks;
- organization policy rules;
- risk/evidence summary;
- missing-validation warnings;
- claims-vs-actual checks;
- team audit metadata;
- optional Team Server connection.

Prerequisites:

- privacy/data-minimization model;
- auth and tenant model;
- least-privilege GitHub permissions;
- explicit post/draft behavior;
- opt-out behavior;
- rate-limit/retry handling;
- fork/private repository scenarios;
- willingness-to-pay evidence.

Do not couple local CLI to GitHub App authentication.

## Stage 11 — LLM provider API integration

Not required for MVP and not required for most local evidence functionality.

Possible future features:

- optional prompt rewriting;
- model-specific prompt/context templates;
- supported usage metadata import;
- provider task/session API integration where documented.

Rules:

- never require provider keys for core CLI;
- do not send source code by default;
- show exactly what is sent;
- support offline/manual mode;
- unknown provider usage remains unknown;
- provider API integration does not replace independent git/CI evidence.

## Stage 12 — Team Server / SaaS metadata sync

Only after local and team evidence exists.

Possible delivery forms:

- hosted AgentsWatch Cloud;
- hybrid local evidence plus metadata sync;
- self-hosted Team Server later if enterprise demand exists.

Possible features:

- shared run/evidence summaries;
- organization policies;
- team dashboards;
- audit history;
- billing/licensing;
- compatibility/adapter fleet visibility;
- historical analytics.

Default sync should exclude:

- source code;
- full prompts;
- full diffs;
- raw terminal output;
- secrets;
- complete local event journals.

Blocked until:

- real local usage;
- real GitHub Action/App workflow demand;
- security/privacy and tenant-isolation plan;
- pricing validation;
- export/delete/retention behavior;
- offline/sync-failure behavior.

## Integration dependency order

```text
Local CLI
-> local evidence and compatibility profile
-> manual/import-only adapters
-> local PR evidence
-> GitHub Action
-> first live local adapters
-> optional local service/dashboard
-> thin IDE extension
-> GitHub App
-> optional Team Server/SaaS
```

Research may run earlier, but a later stage must not become required for an earlier one.

## Integration stop rules

Do not build an integration when:

- the same value can still be tested with local markdown/JSON reports;
- a stable documented input/event source does not exist;
- the integration requires source upload by default;
- it creates a mandatory account for core local use;
- it duplicates core logic in another client;
- it requires broad permissions before user value is proven;
- maintenance cost exceeds observed demand;
- compatibility cannot downgrade safely when the integration fails.
