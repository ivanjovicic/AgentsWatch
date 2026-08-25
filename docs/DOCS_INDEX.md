# AgentsWatch Documentation Index

Last aligned: 2026-08-25

## Canonical reading path

Do not load the full documentation tree by default.

For normal product/runtime work, read in this order and stop when you have enough context:

1. `../README.md` — current product definition and repository status.
2. `../AGENTS.md` — execution and agent rules.
3. `PRODUCT_SPEC.md` — product scope and differentiation.
4. `MVP_ROADMAP.md` — active implementation sequence.
5. `ARCHITECTURE.md` — verification-first architecture and attribution rule.
6. `DATA_MODEL.md` — canonical RunContract/RunBaseline/RunDelta/RunReceipt models.
7. `COMMAND_CONTRACTS.md` — authoritative CLI behavior.
8. `prompt_queues/PROMPT_QUEUE_ROUTER.md` — current next work.
9. `prompt_queues/verification_mvp_2026_08_25.md` — active verification MVP queue.

If code/tests or current CI evidence disagree with planning docs, code/tests/evidence win and docs should be synchronized.

## Current product truth

AgentsWatch is a local-first, vendor-neutral **verification and evidence layer for AI coding agents**.

Primary loop:

```text
Task -> RunContract -> Start baseline -> external agent -> Finish delta -> RunReceipt -> Evidence/Scope/Claims checks
```

Primary differentiators:

- machine-checkable task/run contract;
- dirty-worktree-safe run attribution;
- vendor-neutral run receipt;
- claims vs attributable diff vs validation;
- scope drift detection;
- evidence-based completion status.

Token/context/cost optimization remains secondary and should be derived from trusted receipts later.

## Active execution documents

| Document | Purpose |
|---|---|
| `BOOTSTRAP_NEXT_STEPS.md` | Current known Gate 0 failure and required closure. |
| `90_DAY_EXECUTION_PLAN.md` | Tactical 12-week verification MVP plan. |
| `MVP_ROADMAP.md` | Product phase gates and strict priority order. |
| `prompt_queues/PROMPT_QUEUE_ROUTER.md` | Canonical next-prompt decision. |
| `prompt_queues/NEXT_PROMPT_FAST_PATH.md` | Copy-ready next prompt only. |
| `prompt_queues/verification_mvp_2026_08_25.md` | Active implementation queue. |

## Core technical contracts

| Document | Purpose |
|---|---|
| `ARCHITECTURE.md` | Logical layers, ports/adapters, attribution semantics. |
| `ARCHITECTURE_DECISIONS.md` | Existing ADRs; apply only when consistent with newer canonical docs. |
| `CLI_SPEC.md` | Verification-first CLI surface. |
| `COMMAND_CONTRACTS.md` | Detailed command behavior and failure semantics. |
| `DATA_MODEL.md` | JSON-first canonical data contracts. |
| `ADAPTER_SPEC.md` | Universal and stack-specific detection/validation guidance. |
| `RISK_SCORING_MODEL.md` | Historical/secondary deterministic risk heuristics. |
| `REPORT_FORMATS.md` | Report formatting; must evolve as a projection of RunReceipt. |

## Validation and safety

Use when the selected task needs them:

- `BUILD_VALIDATION_PLAN.md`
- `VALIDATION_EVIDENCE_2026_06_29.md` — historical snapshot; do not treat as current CI truth.
- `RISK_REGISTER.md`
- `PROJECT_READINESS_CHECKLIST.md`
- `ROADMAP_VALIDATION_GATES.md`
- `SECURITY_AND_PRIVACY.md`
- `AGENT_RISK_BOUNDARIES.md`
- `AGENT_PERMISSION_MODEL.md`
- `TEST_MATRIX.md`

## Evidence / agent-development process documents

These documents govern repository development evidence, not the product's canonical RunReceipt model. Read only when relevant to agent workflow maintenance:

- `AGENT_SHARED_OPERATING_STANDARD.md`
- `AGENT_RUN_LOG_ENFORCEMENT.md`
- `AGENT_RUN_EVIDENCE_STANDARD.md`
- `AGENT_RUN_LOGGING_AND_LEARNING.md`
- `AGENT_OPERATING_SYSTEM.md`
- `AGENT_COMMAND_PLAYBOOK.md`
- `AGENT_LONG_TASK_PLAYBOOK.md`
- `AGENT_PATCH_PLAYBOOK.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `.ai/runs/README.md`
- `ai/learning/MISTAKE_LEDGER.md`

Do not preload these all for a normal feature task.

## Historical token/context economy research

Useful later for validation economy and learning, but no longer primary execution authority:

- `PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `PROMPT_TOKEN_ECONOMY_QUICK_RULES.md`
- `ZERO_WASTE_EXECUTION_PROTOCOL.md`
- `CONTEXT_PACKS.md`
- `CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md`
- `TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md`
- `TOKEN_ECONOMY_PREVIOUS_CONVERSATION_BACKFILL_2026_07_01.md`
- `TOKEN_WASTE_METRICS.md`
- token-economy prompt queues under `prompt_queues/`.

Rule: do not implement token/context optimization features ahead of RunContract, attribution, RunReceipt, Evidence Gate, Scope Drift, Claims verification, and dogfood proof.

## Productization / commercial documents

Post-MVP context only unless a task explicitly targets packaging/business:

- `POSITIONING_AND_PRICING_HYPOTHESES.md`
- `RELEASE_AND_PACKAGING_PLAN.md`
- `TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- `PRODUCTIZATION_EXPANSION_2026_06_29.md`
- productization/trial queues under `prompt_queues/`.

No SaaS, billing, OAuth, or team platform work before local verification dogfood proves value.

## Legacy queues and roadmaps

Files such as:

- `prompt_queues/bootstrap_validation.md`;
- `prompt_queues/agentwatch_mvp.md`;
- older evidence/token/productization/architecture queues;
- `ULTRA_ROADMAP.md`;
- historical audits;

remain useful for history and ideas but are **not current next-work authority**.

When they conflict with the 2026-08-25 verification queue, the newer queue wins.

## Context rule

For every task:

```text
router -> selected prompt -> required canonical docs -> exact code/tests
```

Expand only when evidence requires it.

The documentation system should reduce context cost, not require every agent to understand the entire repository history before making a focused change.
