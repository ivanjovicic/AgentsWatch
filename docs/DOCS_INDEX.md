# AgentsWatch Documentation Index

Last aligned: 2026-09-16

## Canonical reading path

Do not load the full documentation tree by default.

For normal product/runtime work, read in this order and stop when enough context exists:

1. `../README.md` — current product definition and repository status.
2. `../AGENTS.md` — execution and agent rules.
3. `PRODUCT_SPEC.md` — product scope and differentiation.
4. `DEEP_DIVE_DECISIONS_2026_09_16.md` — latest accepted competitive/verification guardrails.
5. `MVP_ROADMAP.md` — active implementation sequence.
6. `ARCHITECTURE.md` — verification-first architecture.
7. `DATA_MODEL.md` — canonical RunContract/RunBaseline/RunDelta/RunReceipt models.
8. `COMMAND_CONTRACTS.md` — authoritative CLI behavior.
9. `prompt_queues/PROMPT_QUEUE_ROUTER.md` — current next-work selector.
10. `prompt_queues/verification_mvp_2026_08_25.md` — active verification queue.

If code/tests/current CI evidence disagree with planning docs, code/tests/evidence win and docs must be synchronized.

## Current product truth

AgentsWatch is a local-first, vendor-neutral **run-evidence and completion-verification layer for delegated coding-agent work**.

Primary loop:

```text
existing task / issue / prompt
  -> verification RunContract
  -> pre-run repository baseline
  -> external agent
  -> run-interval repository delta
  -> validation evidence
  -> deterministic scope/claim/completion checks
  -> portable RunReceipt
```

Key wording rule:

`run-interval repository delta` does not automatically mean `agent-authored change`.

Where concurrent writers/processes make causality unknowable, record `Ambiguous` / `NeedsReview` instead of guessing.

## Latest deep-dive research

`research/agentswatch_deep_dive_2026_09/`

Start with:
- `00_EXECUTIVE_DECISION.md`
- `02_COMPETITOR_CAPABILITY_MATRIX.md`
- `03_20_SCENARIO_GAP_ANALYSIS.md`
- `04_DIRTY_WORKTREE_ATTRIBUTION.md`
- `06_RUN_CONTRACT_AND_RECEIPT_VALUE.md`
- `07_BUYERS_ICP_AND_WTP.md`
- `12_MOAT_AND_COPY_TEST.md`
- `14_FAILURE_MODES_AND_KILL_CRITERIA.md`
- `15_UPDATED_SCORECARD.md`
- `16_SCORE_IMPROVEMENT_LEVERS.md`
- `SOURCES.md`

Deep-dive verdict: **CONTINUE, BUT NARROW WEDGE**. Analytical score: **7.80/10**. The score is not proof.

## Accepted deep-dive guardrails

See `DEEP_DIVE_DECISIONS_2026_09_16.md`.

Most important decisions:
- generic AI code review/tracking/governance is not the wedge;
- Qodo/GitHub/Cursor/native tools are direct/adjacent baselines that external validation must beat;
- RunContract should normalize existing task intent, not replace Jira/GitHub/Linear;
- canonical RunReceipt stays a compact evidence artifact;
- learning/router/next-prompt advice lives downstream, not as mandatory audit receipt data;
- first commercial ICP is agent-heavy 5–30 developer teams;
- open-core is the leading packaging hypothesis only after product-value gates pass.

## Active execution documents

| Document | Purpose |
|---|---|
| `BOOTSTRAP_NEXT_STEPS.md` | Current Gate 0 failure and closure requirements. |
| `90_DAY_EXECUTION_PLAN.md` | Tactical verification MVP plan. |
| `MVP_ROADMAP.md` | Product phases and gates. |
| `prompt_queues/PROMPT_QUEUE_ROUTER.md` | Canonical next-prompt decision. |
| `prompt_queues/NEXT_PROMPT_FAST_PATH.md` | Copy-ready next prompt only. |
| `prompt_queues/verification_mvp_2026_08_25.md` | Active queue AW-VFY-001..012. |

Current next implementation task remains `AW-VFY-001` until Gate 0 CI/tests are green.

## Core technical contracts

| Document | Purpose |
|---|---|
| `ARCHITECTURE.md` | Logical layers, ports/adapters, run evidence semantics. |
| `ARCHITECTURE_DECISIONS.md` | ADR history; newer canonical decisions win on conflict. |
| `CLI_SPEC.md` | Verification-first CLI surface. |
| `COMMAND_CONTRACTS.md` | Detailed command behavior and failure semantics. |
| `DATA_MODEL.md` | JSON-first canonical data contracts. |
| `ADAPTER_SPEC.md` | Universal and stack-specific validation guidance. |

## Validation and safety

Use only when relevant:
- `BUILD_VALIDATION_PLAN.md`
- `RISK_REGISTER.md`
- `ROADMAP_VALIDATION_GATES.md`
- `SECURITY_AND_PRIVACY.md`
- `AGENT_RISK_BOUNDARIES.md`
- `AGENT_PERMISSION_MODEL.md`
- `TEST_MATRIX.md`

## Historical/secondary context

Token/context economy, broad productization, legacy queues, `ULTRA_ROADMAP.md`, old audits and generic control-plane ideas remain historical/secondary context.

They must not override:

```text
README
-> Product Spec
-> Deep-Dive Decisions
-> MVP Roadmap
-> active verification queue
-> actual code/tests/CI evidence
```

## Context rule

For every task:

```text
router -> selected prompt -> required canonical docs -> exact code/tests
```

Expand only when evidence requires it.
