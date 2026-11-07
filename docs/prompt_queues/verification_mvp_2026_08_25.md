# AgentsWatch Verification MVP Queue — 2026-08-25

Status: **canonical active implementation + validation queue**  
Target repo: `ivanjovicic/AgentsWatch`  
Last aligned: 2026-11-07

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
| AW-VFY-001 | **Done** (2026-11-05, evidence `.ai/runs/2026-11-05-aw-vfy-001-evidence.md`) | `../prompts/AW-VFY-001-git-parser-ci-hardening.md` | Fix known Git parser failure, harden porcelain parsing, make full CI test gate green. |
| AW-VFY-002 | **Done** (Gate 0 confirmed by GitHub CI run 42071854837 on 9d3f459 (build-test success, 2026-11-05)) (2026-11-05, evidence `.ai/runs/2026-11-05-aw-vfy-002-evidence.md`) | `../prompts/AW-VFY-002-cli-smoke-gate0-close.md` | Prove CLI smoke/local writes and close Gate 0. |
| AW-VFY-003 | **Done** (2026-11-05; Gate 0 CI-confirmed run 42071854837; evidence `.ai/runs/2026-11-05-aw-vfy-003-evidence.md`) | `../prompts/AW-VFY-003-run-contract-v1.md` | Implement verification-focused RunContract v1 model/storage/lint/import boundary. |
| AW-VFY-004 | **Done** (CI green on 9d3f459) (2026-11-05; evidence `.ai/runs/2026-11-05-aw-vfy-004-evidence.md`) | `../prompts/AW-VFY-004-start-run-baseline.md` | Implement dirty-worktree-safe start baseline. |
| AW-VFY-005 | **Done** (CI run 42192082705 on d078295; evidence `.ai/runs/2026-11-05-aw-vfy-005-evidence.md`) | `../prompts/AW-VFY-005-finish-run-attribution.md` | Compute run-interval repository delta and explicit ambiguity from start/end evidence. |
| AW-VFY-006 | **Done** (CI run 42192082705 on d078295; finish-recovery follow-up 2026-11-06, evidence `.ai/runs/2026-11-06-aw-vfy-006-007-followup-evidence.md`) | `../prompts/AW-VFY-006-run-receipt-v1.md` | Implement slim canonical RunReceipt v1 + Markdown evidence/handoff projections. |
| AW-VFY-007 | **Done** (CI run 42192082705 on d078295; AWE006-per-item + unique evidence IDs follow-up 2026-11-06, evidence `.ai/runs/2026-11-06-aw-vfy-006-007-followup-evidence.md`) | `../prompts/AW-VFY-007-evidence-gate-v1.md` | Implement validation evidence model and deterministic completion gate. |
| AW-VFY-008 | **Done** (2026-11-06; CI run 42201075461 success 150/150 on ab3ebe6; evidence `.ai/runs/2026-11-06-aw-vfy-008-evidence.md`; review `.ai/runs/2026-11-06-aw-vfy-review-008-009-deliver-evidence.md`) | `../prompts/AW-VFY-008-scope-drift-v1.md` | Verify high-confidence run-interval changes against owned/avoid paths; surface ambiguity separately. |
| AW-VFY-009 | **Done** (2026-11-06; CI run 42201075461 success 150/150 on ab3ebe6; evidence `.ai/runs/2026-11-06-aw-vfy-009-evidence.md`; review `.ai/runs/2026-11-06-aw-vfy-review-008-009-deliver-evidence.md`) | `../prompts/AW-VFY-009-claims-verification-v1.md` | Verify narrow structured claims against diff/validation evidence. |
| AW-VFY-010 | **In progress — not complete** (2026-11-07). Batches and corrective PRs #11/#12/#13 exist; post-merge privacy/finish check after #13 succeeded. Unit tests and green CI do not close this program. Ledger inventory recorded 8 useful REAL of 30; next slice is AW-VFY-010-B3. | `../prompts/AW-VFY-010-dogfood-30-receipts.md` | Run adversarial 30-receipt dogfood and measure decision-changing value/false findings. |
| AW-VFY-011 | Ready after 010 + dogfood review | `../prompts/AW-VFY-011-external-value-validation.md` | Test value with 10+ external agent-heavy users/teams against Git/CI/PR/native/Qodo baselines. |
| AW-VFY-012 | Ready after 011 passes | `../prompts/AW-VFY-012-commercial-design-partner-validation.md` | Obtain 3+ concrete commercial commitments before SaaS/team expansion. |


## Current next-task order

There is one active queue (this file). Do not create another.

1. **AW-VFY-010-B3** — NEXT claimable task (`../prompts/AW-VFY-010-B3-small-dogfood-batch.md`).
2. Further AW-VFY-010 batches named by `../evidence/aw-vfy-010-ledger.md`.
3. AW-VFY-010-LEDGER is complete as an inventory slice; do not repeat unless new evidence appears.
4. **AW-VFY-011** — HOLD until AW-VFY-010 is complete and reviewed.
5. **AW-VFY-012** — HOLD until AW-VFY-011 passes.

Parallel support prompts (not next, not a second queue): see
`SUPPORT_PROMPT_COVERAGE.md`.

As of 2026-11-07, `origin/main` after merged PR #14 is
`d1028b357fa7ea7002aa5ce2e155d295a99c54f6` (CI run `42369838859`, 193/193).
Manual `Supported` remains AWE009 / NeedsReview. No independently verified
`Done` path exists from human-typed `Supported`. The planning file
`AgentsWatch_Operativni_Backlog_2026-11-07.md` was not found during AW-DOC-001;
do not invent its contents. Plans are not completion evidence.

## AW-VFY-010 execution slices

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| AW-VFY-010-LEDGER | **Done** (2026-11-07; useful REAL 8 of 30; `../evidence/aw-vfy-010-ledger.md`) | `../prompts/AW-VFY-010-LEDGER.md` | Inventory existing dogfood evidence and corrective PRs without modifying historical receipts. |
| AW-VFY-010-B3 | **Ready now / NEXT** | `../prompts/AW-VFY-010-B3-small-dogfood-batch.md` | Next small real dogfood batch against ledger gaps. Only useful REAL rows count toward 30. |

## Support track (parallel, not claimable as next)

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| AW-QA-001 | Parallel support | `../prompts/AW-QA-001-receipt-quality-protocol.md` | Quality protocol, adversarial catalog, and measurement rubric for current receipt/evidence-gate semantics. |
| AW-SEC-001 | Parallel support | `../prompts/AW-SEC-001-receipt-trust-boundary.md` | OSS/licensing/distribution/privacy preflight and receipt persist/display review. |
| AW-RES-001 | Parallel research | `../prompts/AW-RES-001-external-value-protocol.md` | Competitive baseline (>=8 scenarios) and interview protocol required later by AW-VFY-011. |
| AW-RES-002 | Parallel research | `../prompts/AW-RES-002-commercial-design-partner-protocol.md` | ICP/ROI/pricing prep and buyer-commitment protocol required later by AW-VFY-012. |

Open PRs #4, #5, #6, #8, and #9 are classified in `SUPPORT_PROMPT_COVERAGE.md`.
Do not change, merge, or close them from queue work.

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
