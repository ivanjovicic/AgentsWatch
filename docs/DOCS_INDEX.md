# AgentsWatch Documentation Index

Last aligned: 2026-06-29

## Start here

| Document | Use for |
|---|---|
| `../README.md` | Project overview and current status. |
| `../AGENTS.md` | Agent rules for AI-assisted work. |
| `AGENT_OPERATING_SYSTEM.md` | Canonical agent workflow adapted from MathLearning. |
| `CONTEXT_INDEX.md` | Choose the smallest useful docs before an agent run. |
| `PRODUCT_SPEC.md` | Product positioning, problem, users, MVP features. |
| `CLI_SPEC.md` | CLI commands, config shape, adapters, outputs. |
| `MVP_ROADMAP.md` | MVP phases and priority order. |
| `ULTRA_ROADMAP.md` | Full strategy from bootstrap to SaaS. |
| `ROADMAP_INDEX.md` | Roadmap entry point. |

## Bootstrap and validation

| Document | Use for |
|---|---|
| `BUILD_VALIDATION_PLAN.md` | Restore/build/test/CLI smoke order. |
| `VALIDATION_EVIDENCE_2026_06_29.md` | Current Gate 0 evidence status. |
| `RISK_REGISTER.md` | Bootstrap and product risks. |
| `BOOTSTRAP_NEXT_STEPS.md` | Required next order before feature work. |
| `PROJECT_READINESS_CHECKLIST.md` | Checklist before CLI feature expansion. |
| `ROADMAP_VALIDATION_GATES.md` | Phase gates and stop rules. |

## Agent workflow and prompts

| Document | Use for |
|---|---|
| `AGENT_COMMAND_PLAYBOOK.md` | Shell-neutral .NET/git/CLI validation commands. |
| `AGENT_LONG_TASK_PLAYBOOK.md` | Long-task control loop, environment blockers, evidence rules. |
| `AGENT_PATCH_PLAYBOOK.md` | Small safe patch strategy and retry limits. |
| `PROMPT_RULES.md` | Required prompt sections, run modes, bootstrap rules, final format. |
| `PROMPT_QUALITY_CHECKLIST.md` | Preflight checklist before adding/running prompts. |
| `PROMPT_EVIDENCE_TEMPLATE.md` | Done/Blocked/evidence row format. |
| `COMPLETION_ANALYTICS.md` | Completion percentages, missed work, follow-up mapping. |
| `CLAIMS_VS_ACTUAL_REVIEW.md` | Check final claims against actual diff and validation evidence. |
| `MATHLEARNING_DOCS_ADAPTATION.md` | What was adapted from MathLearning and what changed. |

## Architecture

| Document | Use for |
|---|---|
| `ARCHITECTURE.md` | Current MVP architecture snapshot. |
| `TARGET_ARCHITECTURE.md` | Future-proof target architecture for CLI, dashboard, team/PR workflow, and SaaS. |
| `ARCHITECTURE_DECISIONS.md` | Active ADRs and architectural tradeoffs. |
| `MODULE_BOUNDARIES.md` | Dependency direction, module ownership, extraction order, and stop rules. |

## Product contracts

| Document | Use for |
|---|---|
| `CONFIG_REFERENCE.md` | Draft config schema and parser behavior. |
| `REPORT_FORMATS.md` | Markdown run report, handoff, review prompt formats. |
| `DATA_MODEL.md` | Future markdown/JSON/SQLite model. |
| `ADAPTER_SPEC.md` | Universal, .NET, Flutter, React, Python, Node adapter scope. |
| `PROMPT_OPTIMIZATION_PLAYBOOK.md` | Prompt risk, budget, split, handoff, diff-only review. |

## Quality and safety

| Document | Use for |
|---|---|
| `TEST_MATRIX.md` | Required test areas by feature. |
| `SECURITY_AND_PRIVACY.md` | Local-first security and privacy rules. |
| `DOGFOOD_PLAN.md` | Real-repo dogfood workflow and evidence. |
| `../CONTRIBUTING.md` | Contributor workflow and PR checklist. |
| `../SECURITY.md` | Security reporting policy. |

## Prompt queues

| Document | Use for |
|---|---|
| `prompt_queues/bootstrap_validation.md` | Validation-first prompts. |
| `prompt_queues/agentwatch_mvp.md` | CLI MVP implementation prompts. |
| `prompt_queues/roadmap_execution.md` | Roadmap execution prompts. |
| `prompt_queues/architecture_evolution.md` | Safe architecture evolution prompts after Gate 0. |

## Rule

If documents disagree, use current code/tests first, then bootstrap validation docs, then agent workflow docs, then architecture docs, then product/roadmap docs.
