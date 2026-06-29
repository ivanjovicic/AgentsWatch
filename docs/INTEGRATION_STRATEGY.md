# AgentsWatch Integration Strategy

Last aligned: 2026-06-29  
Status: future plan, not MVP implementation

## Purpose

Define integration order so AgentsWatch does not start with the hardest and riskiest work.

## Integration principle

Local CLI first. Integrations later.

A local workflow must be useful before any GitHub App, CI annotation, SaaS, or LLM-provider integration is built.

---

## Stage 1 — No external integration

MVP behavior:

- read local git state;
- read local files;
- write markdown reports;
- suggest validation commands;
- generate prompts.

No account, cloud, telemetry, or network required.

---

## Stage 2 — Local git and shell integration

Allowed after Gate 0:

- git status/diff snapshots;
- local command suggestions;
- explicit validation command execution later;
- markdown/JSON local artifacts.

Rules:

- validation execution must be explicit;
- command output must be redacted later if sensitive;
- failed commands must be recorded honestly.

---

## Stage 3 — GitHub workflow as input

Allowed after CLI reports are useful.

Possible features:

- read PR diff;
- read CI status;
- generate PR risk report locally;
- generate PR comment draft without posting automatically.

Rules:

- do not post comments automatically in MVP;
- do not require GitHub token for local core features;
- document every data access.

---

## Stage 4 — GitHub App

Allowed only after manual PR workflow proves value.

Possible features:

- PR checks;
- policy rules;
- risk summary comment;
- missing-test warnings;
- claims-vs-actual check.

Prerequisites:

- privacy model;
- auth model;
- permission model;
- opt-out behavior;
- rate-limit handling.

---

## Stage 5 — LLM provider integration

Not required for MVP.

Possible future features:

- automatic prompt rewriting;
- local rules plus provider calls;
- model-specific prompt templates.

Rules:

- never require provider keys for core CLI;
- do not send source code by default;
- show exactly what is sent;
- support offline/manual mode.

---

## Stage 6 — SaaS/team sync

Only after local and team evidence exists.

Possible features:

- shared run history;
- organization policies;
- dashboards;
- billing;
- team analytics.

Blocked until:

- real local usage;
- real PR workflow demand;
- security/privacy plan;
- pricing validation.

---

## Integration stop rule

Do not build an integration if the same user value can be proven with local markdown reports first.
