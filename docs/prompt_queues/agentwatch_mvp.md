# AgentsWatch MVP Prompt Queue

Last aligned: 2026-06-30  
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
- `../COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`
- `../ARCHITECTURE.md`

## Rules

- Start local CLI first; do not start SaaS, billing, auth, or cloud sync.
- Prefer .NET global tool architecture.
- Universal git/markdown/file-system behavior comes before language-specific adapters.
- Each implementation prompt must have a token budget, scope limiter, stop rules, validation, and handoff summary.
- Do not build dashboard before CLI MVP proves useful.
- Do not run AW-002+ until AW-VAL-001 and AW-VAL-002 evidence exists.
- Do not implement AW-011 runtime behavior until validation runner groundwork exists.
- Do not paste full command logs into prompts or markdown reports; use compact command evidence.

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
| AW-011 | Blocked until AW-003/AW-008 groundwork exists | Implement Command Profiler / Fast Validation Advisor to avoid slow repeated commands and large terminal logs. |

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

## AW-011 — Command Profiler / Fast Validation Advisor

Run mode: investigation-first, then implementation in smaller follow-up prompts  
Token budget: medium  
Gate: after AW-003 run report groundwork and AW-008 validation-command groundwork

Task: add a local command profiler and fast validation advisor that helps agents choose cheaper validation commands and avoid sending large terminal logs into model context.

Read first:

- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`
- `docs/prompts/AW-011-command-profiler-fast-validation-advisor.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/ADAPTER_SPEC.md`
- `docs/REPORT_FORMATS.md`
- `docs/DATA_MODEL.md`
- `docs/SECURITY_AND_PRIVACY.md`

Required behavior later:

- add `agentswatch run -- <command>` as a local profiler wrapper;
- add `agentswatch validate --suggest` fast validation recommendations;
- record command duration, exit code, byte counts, and compact error signatures;
- keep full stdout/stderr out of markdown reports by default;
- recommend faster language-specific alternatives for .NET, Flutter, React/TypeScript, Python, and Node;
- include compact command profile summaries in run reports and handoffs.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter Command
dotnet test --filter Validation
```

Stop rules:

- stop if Gate 0 evidence is missing;
- stop if command execution needs broad shell abstraction;
- stop if command output may expose secrets without redaction;
- stop if more than one runtime feature slice is required;
- stop if validation failures repeat twice.
