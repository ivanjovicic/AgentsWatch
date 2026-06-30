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

## Read rule

Do not read this whole queue plus every linked product document by default.

Minimum read:

- this prompt queue section for the selected prompt;
- `PROMPT_QUEUE_ROUTER.md` if gate status is unclear;
- one relevant contract doc from `docs/CONTEXT_INDEX.md`.

Use `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md` for default anti-waste rules.
Use the full rulebook only when changing prompt-system rules.

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
| AW-011A | Blocked until AW-003/AW-008 groundwork exists | Investigate Command Profiler / Fast Validation Advisor contracts. |
| AW-011B | Blocked until AW-011A evidence exists | Implement command history data model. |
| AW-011C | Blocked until AW-011B evidence exists | Implement `agentswatch run -- <command>` wrapper. |
| AW-011D | Blocked until AW-011C evidence exists | Implement `agentswatch validate --suggest` fast advisor. |
| AW-011E | Blocked until AW-011D evidence exists | Add command profile report/handoff integration. |

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

## AW-011A — Command Profiler / Fast Validation Advisor contracts

Run mode: investigation-only  
Token budget: low  
Gate: after AW-003 run report groundwork and AW-008 validation-command groundwork

Task: investigate the smallest safe contract for command profiling and fast validation advice.

Minimum read:

- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md`

Optional only if needed:

- `docs/REPORT_FORMATS.md` for report/handoff impact;
- `docs/DATA_MODEL.md` for command history shape;
- `docs/SECURITY_AND_PRIVACY.md` for command output redaction/storage.

Return:

1. minimal CLI contract;
2. minimum data record;
3. report/handoff impact;
4. security risks;
5. next prompt.

## AW-011B-E — Follow-up implementation slices

Do not run these until AW-011A produces accepted design evidence.

Suggested split:

```text
AW-011B — command history model
AW-011C — agentswatch run wrapper
AW-011D — validate --suggest fast advisor
AW-011E — report/handoff integration
```

Each follow-up must use one run mode, one implementation slice, and targeted validation only.