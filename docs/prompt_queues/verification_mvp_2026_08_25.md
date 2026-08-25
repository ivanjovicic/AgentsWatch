# AgentsWatch Verification MVP Queue — 2026-08-25

Status: **canonical active implementation queue**  
Target repo: `ivanjovicic/AgentsWatch`

## Purpose

Build the smallest credible AgentsWatch product:

```text
Task -> RunContract -> Start baseline -> external agent -> Finish delta -> RunReceipt -> Evidence/Scope/Claims verification
```

This queue supersedes old next-work ordering in `bootstrap_validation.md`, `agentwatch_mvp.md`, token-economy queues, productization queues, and older roadmap-execution queues.

Historical prompts remain reference material only unless explicitly re-promoted here.

## Global rules

- Follow `AGENTS.md`.
- One implementation slice per prompt.
- Do not implement dashboard/SaaS/billing/agent runtime/orchestration.
- Do not expand token optimizer work ahead of the verification spine.
- JSON is canonical for Contract/Baseline/Receipt; Markdown is projection.
- Raw final `git status` is not run attribution.
- Preserve `unknown` / `ambiguous` instead of guessing.
- Add targeted tests for every runtime behavior change.
- Keep full source contents, chat history, and terminal logs out of persisted evidence by default.
- Do not claim validation passed without executed evidence.

## Strict dependency chain

```text
AW-VFY-001
  -> AW-VFY-002
  -> AW-VFY-003
  -> AW-VFY-004
  -> AW-VFY-005
  -> AW-VFY-006
  -> AW-VFY-007
  -> AW-VFY-008
  -> AW-VFY-009
  -> AW-VFY-010
```

Do not skip ahead unless an earlier prompt is explicitly completed or replaced with equivalent committed evidence.

## Active prompts

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| AW-VFY-001 | **Ready now** | `../prompts/AW-VFY-001-git-parser-ci-hardening.md` | Fix known git parser failure, harden porcelain parsing, make full CI test gate green. |
| AW-VFY-002 | Ready after 001 | `../prompts/AW-VFY-002-cli-smoke-gate0-close.md` | Prove CLI smoke/local writes and close Gate 0. |
| AW-VFY-003 | Ready after 002 | `../prompts/AW-VFY-003-run-contract-v1.md` | Implement canonical RunContract v1 model/storage/lint. |
| AW-VFY-004 | Ready after 003 | `../prompts/AW-VFY-004-start-run-baseline.md` | Implement dirty-worktree-safe start baseline. |
| AW-VFY-005 | Ready after 004 | `../prompts/AW-VFY-005-finish-run-attribution.md` | Compute attributable run delta from start/end evidence. |
| AW-VFY-006 | Ready after 005 | `../prompts/AW-VFY-006-run-receipt-v1.md` | Implement canonical RunReceipt v1 + Markdown projection/handoff. |
| AW-VFY-007 | Ready after 006 | `../prompts/AW-VFY-007-evidence-gate-v1.md` | Implement validation evidence model and deterministic completion gate. |
| AW-VFY-008 | Ready after 007 | `../prompts/AW-VFY-008-scope-drift-v1.md` | Verify attributable changes against owned/avoid paths. |
| AW-VFY-009 | Ready after 008 | `../prompts/AW-VFY-009-claims-verification-v1.md` | Verify initial structured claims against diff/validation evidence. |
| AW-VFY-010 | Ready after 009 | `../prompts/AW-VFY-010-dogfood-30-receipts.md` | Run structured dogfood, collect 30 receipts, decide next investment from evidence. |

## Gate definitions

### Gate 0 — Skeleton proven

Required before AW-VFY-003:

- restore pass;
- release build pass;
- full tests pass;
- CLI help/version/init/optimize/status smoke pass;
- init local-write behavior verified.

### Gate 1 — Contract proven

Required before start lifecycle:

- RunContract v1 schema stable;
- valid/invalid fixtures;
- deterministic lint;
- canonical JSON storage.

### Gate 2 — Attribution proven

Required before receipt verification:

- clean start/end cases;
- pre-existing dirty unchanged case;
- pre-existing dirty changed-further case;
- add/delete/rename/untracked cases;
- ambiguity represented explicitly.

### Gate 3 — Receipt proven

Required before evidence/drift/claims expansion:

- canonical JSON receipt;
- Markdown generated from JSON;
- no free-form Markdown parsing required;
- compact handoff generated from receipt.

### Gate 4 — Verification proven

Required before dogfood:

- mandatory validation can block Done;
- scope findings use attributable changes;
- initial claim classes have deterministic checks;
- every decision/finding has explainable reasons.

## Post-queue decision

After AW-VFY-010, choose the next phase from evidence only.

Possible next priorities:

1. validation economy / command profiler;
2. mistake-learning rules;
3. MCP exposure of stable use cases;
4. GitHub/PR evidence checks;
5. cross-agent import/routing;
6. local dashboard.

Do not pre-commit to a dashboard or SaaS before dogfood data identifies recurring user value.
