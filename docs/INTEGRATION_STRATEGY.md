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

## Proposed narrow Codex/Cursor + web-review path (2026-11-10; NOT ACTIVE)

Full feasibility, source references, ownership design and phase gates: `ORCHESTRATION_RFC_CODEX_CURSOR_WEB_REVIEW_2026_11_10.md`.

- User-preferred implementers: Codex CLI/SDK first and Cursor Cloud Agents v1 second, **only after separate owner GO**. Cursor API v1 is beta; do not presume v1 webhooks exist.
- User-preferred reviewers: Claude **web** and ChatGPT **web**. First deliver a bounded PR review packet (exact head SHA/diff/CI) plus manual structured-result import. Claude GitHub integration syncs files, not PRs/commit history; a web chat is not an unattended provider API. ChatGPT Work may have eligible PR-event triggers but requires separate proven task setup and result callback; do not assume availability or autonomous review.
- Claude Code Review and Anthropic API reviews are separate, optional products, not silent substitutes for Claude web.
- External model review is advisory and does not attest authorship, replace human independent review, upgrade Unknown to Done, or authorize merge/deploy.
- This is an experimental future plan: AW-VFY-010 ADJUST/PAUSE, AW-VFY-011/012 HOLD, canonical queue and no-cloud/no-telemetry MVP remain unchanged. No new integration is activated by this documentation.

## Integration stop rule

Do not build an integration if the same user value can be proven with local markdown reports first.
