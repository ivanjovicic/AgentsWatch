# AgentsWatch MVP Prompt Queue

Last aligned: 2026-06-29  
Target repo: `ivanjovicic/AgentsWatch`

Purpose: build AgentsWatch as a local CLI that supervises AI coding-agent runs and reduces token waste.

Read first:

- `../../AGENTS.md`
- `../PRODUCT_SPEC.md`
- `../CLI_SPEC.md`
- `../MVP_ROADMAP.md`
- `../PROMPT_OPTIMIZATION_PLAYBOOK.md`
- `../ARCHITECTURE.md`

Rules:

- Start local CLI first; do not start SaaS, billing, auth, or cloud sync.
- Prefer .NET global tool architecture.
- Universal git/markdown/file-system behavior comes before language-specific adapters.
- Each implementation prompt must have a token budget, scope limiter, stop rules, validation, and handoff summary.
- Do not build dashboard before CLI MVP proves useful.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-001 | Done 70% (2026-06-29, initial skeleton on main; tests not run locally) | Create AgentsWatch solution skeleton as .NET CLI/core projects. |
| AW-002 | Ready | Harden `agentswatch init` and config template. |
| AW-003 | Ready | Implement git status/diff tracker and markdown run report end-to-end. |
| AW-004 | Ready | Improve basic risk scoring from changed files. |
| AW-005 | Ready | Implement prompt optimizer and task splitter for markdown prompts. |
| AW-006 | Ready | Implement handoff summary generator. |
| AW-007 | Ready | Implement diff-only review prompt generator. |
| AW-008 | Ready | Implement validation command runner with language adapters. |
| AW-009 | Ready | Implement claimed-vs-actual diff heuristic. |
| AW-010 | Ready | Create local dashboard plan after CLI MVP evidence exists. |

## AW-002 — Init command hardening

Run mode: implementation  
Token budget: low

Task: harden `agentswatch init`.

Required behavior:

- creates `.ai` and `.agentwatch` folders;
- does not overwrite existing user files;
- writes config/status/changelog/review checklist;
- works on Windows and Linux paths;
- has unit/integration tests around temp directories.

Validation:

```bash
dotnet test --filter Init
```

## AW-003 — Git diff tracker and run report

Run mode: implementation  
Token budget: medium

Task: complete git state capture and markdown run report.

Required behavior:

- capture start/end snapshots;
- parse clean, modified, added, deleted, renamed, and untracked files;
- write `.ai/runs/<timestamp>-<task>.md`;
- update `.ai/STATUS.md` and `.ai/CHANGELOG_AI.md`.

Validation:

```bash
dotnet test --filter Git
```

## AW-004 — Basic risk scoring

Run mode: implementation  
Token budget: low

Task: score changed files using transparent heuristics.

High-risk signals:

- auth/security files;
- migrations/schema files;
- config/secrets;
- provider/state ownership;
- backend API contracts;
- more than 20 files changed;
- no tests changed;
- validation not run.

## AW-005 — Prompt optimizer and splitter

Run mode: implementation  
Token budget: medium

Task: convert rough prompt text into smaller markdown tasks.

Required generated prompts:

```text
001-investigate-only.md
002-implement-minimal-fix.md
003-add-tests.md
004-diff-only-review.md
```

Each prompt must include run mode, token budget, scope limiter, owned paths, avoid paths, stop rules, validation, and final response shape.
