# AgentsWatch Architecture Evolution Queue

Last aligned: 2026-06-29  
Target repo: `ivanjovicic/AgentsWatch`

Purpose: evolve the current skeleton toward the target architecture without risky broad refactors.

## Read first

- `../TARGET_ARCHITECTURE.md`
- `../ARCHITECTURE_DECISIONS.md`
- `../MODULE_BOUNDARIES.md`
- `../ROADMAP_VALIDATION_GATES.md`
- `../BUILD_VALIDATION_PLAN.md`
- `bootstrap_validation.md`

## Rules

- Gate 0 must pass before architecture refactoring.
- Do not rename projects before restore/build/test are green.
- Do not add dashboard/API/SaaS in this queue.
- Keep every prompt focused on one boundary or one use case.
- Prefer adding abstractions before moving many files.
- Runtime behavior must stay unchanged unless the prompt explicitly fixes a validation failure.

---

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| ARCH-001 | Blocked until Gate 0 passes | Extract `InitProjectUseCase` from CLI file writes. |
| ARCH-002 | Blocked until Gate 0 passes | Add file-system abstraction for safe writes and no-overwrite behavior. |
| ARCH-003 | Blocked until Gate 0 passes | Extract `OptimizePromptUseCase` from CLI command flow. |
| ARCH-004 | Blocked until Gate 0 passes | Introduce run/task application models without changing CLI behavior. |
| ARCH-005 | Blocked until Gate 0 passes | Add report store abstraction before JSON/SQLite storage. |
| ARCH-006 | Blocked until Gate 0 passes | Introduce adapter registry for language adapters. |
| ARCH-007 | Blocked until Gate 0 passes | Add policy/risk rule interface and move heuristics behind it. |
| ARCH-008 | Blocked until CLI reports are useful | Prepare local API boundary for future dashboard, docs-only. |
| ARCH-009 | Blocked until dogfood evidence exists | Design GitHub PR integration adapter, docs-only. |
| ARCH-010 | Blocked until dogfood evidence exists | Design SaaS sync boundary, docs-only. |

---

## ARCH-001 — Extract init use case

Run mode: implementation  
Token budget: low

Prerequisite: Gate 0 build/test/smoke evidence exists.

Task: extract init behavior from CLI into an application use case while preserving behavior.

Owned paths:

- `src/AgentsWatch.Cli/`
- future application/core files
- targeted init tests

Non-goals:

- no new commands;
- no dashboard;
- no storage redesign;
- no project rename.

Validation:

```bash
dotnet test --filter Init
dotnet build AgentsWatch.sln
```

---

## ARCH-002 — File-system abstraction

Run mode: implementation  
Token budget: low

Task: introduce a file-system port for init/report writes.

Required behavior:

- no-overwrite by default;
- temp-directory tests;
- atomic write plan documented if not implemented.

---

## ARCH-003 — Optimize prompt use case

Run mode: implementation  
Token budget: low

Task: extract prompt optimization flow behind an application use case.

Required behavior:

- CLI output remains compatible;
- existing prompt risk analyzer tests remain valid;
- no LLM dependency added.

---

## ARCH-004 — Run/task application models

Run mode: implementation  
Token budget: medium

Task: introduce application-level task/run lifecycle models that map to current domain records.

Required behavior:

- no storage migration yet;
- no SQLite yet;
- no dashboard/API yet.

---

## ARCH-005 — Report store abstraction

Run mode: implementation  
Token budget: medium

Task: add a report store abstraction before adding JSON/SQLite.

Required behavior:

- markdown report writer remains user-facing;
- future JSON sidecars can be added without changing use cases.

---

## ARCH-006 — Adapter registry

Run mode: implementation  
Token budget: low

Task: introduce an adapter registry that combines project detectors and validation suggestions.

Required behavior:

- current detector behavior preserved;
- unknown projects still fall back to universal behavior.

---

## ARCH-007 — Policy/risk rule interface

Run mode: implementation  
Token budget: low

Task: add deterministic risk rule contracts.

Required behavior:

- transparent reasons;
- unit tests for rules;
- no opaque AI scoring.
