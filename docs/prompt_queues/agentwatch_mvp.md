# AgentsWatch MVP Prompt Queue

Last aligned: 2026-06-29  
Target repo: `ivanjovicic/AgentsWatch`

Purpose: build AgentsWatch as a local CLI that supervises AI coding-agent runs and reduces token waste.

## Current gate status

Gate 0 is incomplete. Use `PROMPT_QUEUE_ROUTER.md` before selecting any MVP prompt.

Current next prompt:

```text
AW-VAL-001 — Build validation
```

## Read first

- `../../AGENTS.md`
- `PROMPT_QUEUE_ROUTER.md`
- `NEXT_PROMPT_FAST_PATH.md`
- `../PRODUCT_SPEC.md`
- `../COMMAND_CONTRACTS.md`
- `../CLI_UX_OUTPUT_SPEC.md`
- `../MVP_EPICS_AND_ACCEPTANCE.md`
- `../PROMPT_OPTIMIZATION_PLAYBOOK.md`
- `../ARCHITECTURE.md`

## Rules

- Start local CLI first; do not start SaaS, billing, auth, or cloud sync.
- Prefer .NET global tool architecture.
- Universal git/markdown/file-system behavior comes before language-specific adapters.
- Each implementation prompt must have a token budget, scope limiter, stop rules, validation, and handoff summary.
- Do not build dashboard before CLI MVP proves useful.
- Do not run AW-002+ until AW-VAL-001 and AW-VAL-002 evidence exists.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-001 | Done 70% (2026-06-29, initial skeleton on main; tests not run locally; follow-up AW-VAL-001) | Create AgentsWatch solution skeleton as .NET CLI/core projects. |
| AW-VAL-001 | Ready now | Verify restore/build/test for the skeleton. |
| AW-VAL-002 | Ready after AW-VAL-001 | Verify CLI help/version/optimize/status smoke behavior. |
| AW-VAL-003 | Ready after AW-VAL-002 | Review validation evidence and decide if feature work is safe. |
| AW-VAL-004 | Ready after AW-VAL-003 | Harden init command only after validation is known. |
| AW-002 | Blocked until AW-VAL-001/002 evidence exists | Harden `agentswatch init` and config template. |
| AW-003 | Blocked until AW-VAL-001/002 evidence exists | Implement git status/diff tracker and markdown run report end-to-end. |
| AW-004 | Blocked until AW-VAL-001/002 evidence exists | Improve basic risk scoring from changed files. |
| AW-005 | Blocked until AW-VAL-001/002 evidence exists | Implement prompt optimizer and task splitter for markdown prompts. |
| AW-006 | Blocked until AW-VAL-001/002 evidence exists | Implement handoff summary generator. |
| AW-007 | Blocked until AW-VAL-001/002 evidence exists | Implement diff-only review prompt generator. |
| AW-008 | Blocked until AW-VAL-001/002 evidence exists | Implement validation command runner with language adapters. |
| AW-009 | Blocked until AW-VAL-001/002 evidence exists | Implement claimed-vs-actual diff heuristic. |
| AW-010 | Blocked until CLI MVP evidence exists | Create local dashboard plan after CLI MVP evidence exists. |

## AW-002 — Init command hardening

Run mode: implementation  
Token budget: low  
Gate: after AW-VAL-001 and AW-VAL-002 pass

Task: harden `agentswatch init`.

Required behavior:

- creates `.ai` and `.agentwatch` folders;
- does not overwrite existing user files;
- writes config/status/changelog/review checklist;
- works on Windows and Linux paths;
- has unit/integration tests around temp directories.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter Init
```

## AW-003 — Git diff tracker and run report

Run mode: implementation  
Token budget: medium  
Gate: after init is safe and tests exist

Task: complete git state capture and markdown run report.

Required behavior:

- capture start/end snapshots;
- parse clean, modified, added, deleted, renamed, and untracked files;
- write `.ai/runs/<timestamp>-<task>.md`;
- update `.ai/STATUS.md` and `.ai/CHANGELOG_AI.md`.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter Git
```

## AW-004 — Basic risk scoring

Run mode: implementation  
Token budget: low  
Gate: after git parser evidence exists

Task: score changed files using transparent heuristics from `docs/RISK_SCORING_MODEL.md`.

High-risk signals:

- CI/build/project files;
- command execution;
- file-system writes;
- config/secrets;
- more than 20 files changed;
- no tests changed with runtime changes;
- validation not run.

## AW-005 — Prompt optimizer and splitter

Run mode: implementation  
Token budget: medium  
Gate: after prompt risk analyzer tests are expanded

Task: convert rough prompt text into smaller markdown tasks.

Required generated prompts:

```text
001-investigate-only.md
002-implement-minimal-fix.md
003-add-tests.md
004-diff-only-review.md
```

Each prompt must include run mode, token budget, scope limiter, owned paths, avoid paths, stop rules, validation, and final response shape.
