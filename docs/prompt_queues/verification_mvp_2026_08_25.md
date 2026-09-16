# AgentsWatch Verification MVP Queue — 2026-08-25

Status: **canonical active implementation + validation queue**  
Target repo: `ivanjovicic/AgentsWatch`  
Last aligned: 2026-09-16

## Purpose

Build and validate the smallest credible AgentsWatch product:

```text
existing Task/Issue/Prompt
  -> verification RunContract
  -> Start baseline
  -> external agent
  -> Finish run-interval delta
  -> compact RunReceipt
  -> Evidence/Scope/Claims verification
  -> External value
  -> Commercial proof
```

This queue supersedes old next-work ordering in `bootstrap_validation.md`, `agentwatch_mvp.md`, token-economy queues, productization queues and older roadmap-execution queues.

Latest accepted strategy guardrails:
`docs/DEEP_DIVE_DECISIONS_2026_09_16.md`

Latest deep-dive evidence:
`docs/research/agentswatch_deep_dive_2026_09/`

## Global rules

- Follow `AGENTS.md`.
- One implementation or validation slice per prompt.
- Do not implement dashboard/SaaS/billing/agent runtime/orchestration.
- Do not expand token optimizer work ahead of verification proof.
- JSON is canonical for Contract/Baseline/Receipt; Markdown is projection.
- Raw final `git status` is not run-interval attribution.
- `run-interval change` does not automatically mean `agent-authored`; preserve ambiguity unless causal evidence exists.
- Preserve `unknown` / `ambiguous` instead of guessing.
- RunContract normalizes existing task intent; do not build a second project-management system.
- RunReceipt is a compact evidence artifact; `learningNote`, `nextPrompt`, routing and optimization advice are not mandatory canonical receipt fields.
- Add targeted tests for every runtime behavior change.
- Keep full source contents, chat history and terminal logs out of persisted evidence by default.
- Do not claim validation passed without executed/provenanced evidence.
- LLM output cannot silently upgrade Unknown/NeedsReview to Supported/Done.
- After dogfood, external/commercial validation is required before broad product expansion.
- Generic AI-code tracking and generic AI review are not differentiated product wedges.

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
  -> AW-VFY-011
  -> AW-VFY-012
```

Do not skip ahead unless an earlier prompt is explicitly completed or replaced with equivalent committed evidence.

## Active prompts

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| AW-VFY-001 | **Ready now** | `../prompts/AW-VFY-001-git-parser-ci-hardening.md` | Fix known Git parser failure, harden porcelain parsing, make full CI test gate green. |
| AW-VFY-002 | Ready after 001 | `../prompts/AW-VFY-002-cli-smoke-gate0-close.md` | Prove CLI smoke/local writes and close Gate 0. |
| AW-VFY-003 | Ready after 002 | `../prompts/AW-VFY-003-run-contract-v1.md` | Implement verification-focused RunContract v1 model/storage/lint/import boundary. |
| AW-VFY-004 | Ready after 003 | `../prompts/AW-VFY-004-start-run-baseline.md` | Implement dirty-worktree-safe start baseline. |
| AW-VFY-005 | Ready after 004 | `../prompts/AW-VFY-005-finish-run-attribution.md` | Compute run-interval repository delta and explicit ambiguity from start/end evidence. |
| AW-VFY-006 | Ready after 005 | `../prompts/AW-VFY-006-run-receipt-v1.md` | Implement slim canonical RunReceipt v1 + Markdown evidence/handoff projections. |
| AW-VFY-007 | Ready after 006 | `../prompts/AW-VFY-007-evidence-gate-v1.md` | Implement validation evidence model and deterministic completion gate. |
| AW-VFY-008 | Ready after 007 | `../prompts/AW-VFY-008-scope-drift-v1.md` | Verify high-confidence run-interval changes against owned/avoid paths; surface ambiguity separately. |
| AW-VFY-009 | Ready after 008 | `../prompts/AW-VFY-009-claims-verification-v1.md` | Verify narrow structured claims against diff/validation evidence. |
| AW-VFY-010 | Ready after 009 | `../prompts/AW-VFY-010-dogfood-30-receipts.md` | Run adversarial 30-receipt dogfood and measure decision-changing value/false findings. |
| AW-VFY-011 | Ready after 010 + dogfood review | `../prompts/AW-VFY-011-external-value-validation.md` | Test value with 10+ external agent-heavy users/teams against Git/CI/PR/native/Qodo baselines. |
| AW-VFY-012 | Ready after 011 passes | `../prompts/AW-VFY-012-commercial-design-partner-validation.md` | Obtain 3+ concrete commercial commitments before SaaS/team expansion. |

## Gate 0 — Skeleton proven

Required before AW-VFY-003:
- restore pass;
- release build pass;
- full tests pass;
- CLI help/version/init/optimize/status smoke pass;
- init local-write behavior verified.

## Gate 1 — Contract proven

Required before lifecycle:
- RunContract schema stable;
- valid/invalid fixtures;
- deterministic lint;
- canonical JSON storage;
- no required duplication of rich task-management metadata beyond verification normalization.

## Gate 2 — Run-interval evidence proven

Required before receipt verification:
- clean start/end cases;
- pre-existing dirty unchanged case;
- pre-existing dirty changed-further case;
- add/delete/rename/untracked cases;
- staged/unstaged semantics sufficiently preserved;
- ambiguity represented explicitly;
- no causal `agent-authored` claim without executor evidence.

## Gate 3 — Receipt proven

Required before evidence/drift/claims expansion:
- compact canonical JSON receipt;
- Markdown generated from JSON;
- no free-form Markdown parsing required;
- evidence projection distinguishes interval/pre-existing/ambiguous state;
- learning/next-step text is optional handoff state, not canonical verification truth.

## Gate 4 — Verification proven

Required before dogfood:
- mandatory validation can block Done;
- scope findings use high-confidence run-interval evidence;
- ambiguity can force NeedsReview;
- initial claim classes have deterministic checks;
- every decision/finding has explainable reasons;
- AI interpretation cannot promote evidence status without deterministic/provenanced support.

## Gate 5 — Dogfood value proven

Required before external validation:
- 30 useful receipts across varied real tasks;
- real unsupported-claim catch;
- real scope-drift catch;
- real missing-evidence block;
- no observed material false attribution;
- explicit ambiguity rather than guesses;
- false-positive/verification overhead tracked;
- evidence summary separates product findings from implementation bugs.

## Gate 6 — External value proven

First commercial ICP hypothesis:
**AI-heavy engineering teams of roughly 5–30 developers**.

Required before advanced learning, broad integrations, dashboard or team/SaaS packaging:
- >=10 real external evaluations;
- >=2 agent ecosystems/vendors represented where practical;
- >=3 users/teams request continued use;
- >=3 decision-changing evidence/scope/claim findings beyond baseline workflow;
- no observed material false attribution;
- false-positive burden low enough that reviewers keep trusting the product;
- native vendor logs + Git + CI + PR review + current independent-review alternatives are demonstrably insufficient for validated cases.

Kill/narrow signal:
- receipt does not change decisions;
- false positives/ambiguity create more review cost than value;
- cross-vendor independence is not valued;
- Qodo/native tooling covers the high-value scenarios with equal or lower friction.

## Gate 7 — Commercial value proven

Required before SaaS/auth/billing/team administration:
- >=3 paid-pilot/design-partner or equivalent concrete buyer commitments;
- budget owner/payer identified;
- pricing/package tested with actual buyers;
- payment reason is verification/review/governance value, not unrelated platform features.

## Post-queue decision

After AW-VFY-012, choose the next phase from evidence only.

Possible priorities **only if gates pass**:
1. GitHub Action/Check requested by validated users;
2. MCP exposure of stable use cases;
3. organization policies / managed evidence;
4. validation economy / command profiling;
5. mistake-learning rules outside canonical receipt;
6. cross-agent import/routing;
7. local/team dashboard;
8. minimal commercial packaging.

Do not pre-commit to SaaS, broad integration marketplace, proprietary agent runtime or generic code-review engine.

## Current competitive context

Use:
- `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`
- `docs/research/agentswatch_deep_dive_2026_09/02_COMPETITOR_CAPABILITY_MATRIX.md`
- `docs/research/agentswatch_deep_dive_2026_09/03_20_SCENARIO_GAP_ANALYSIS.md`

Key rule:

> Do not compete on tracking or generic review; prove portable run evidence and completion verification that changes real decisions.
