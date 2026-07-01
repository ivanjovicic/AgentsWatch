# AgentsWatch Documentation Index

Last aligned: 2026-07-01

## Start here

| Document | Use for |
|---|---|
| `../README.md` | Project overview and current status. |
| `../AGENTS.md` | Agent rules for AI-assisted work. |
| `AGENT_SHARED_OPERATING_STANDARD.md` | Shared cross-repo rules for prompt shape, token budget, evidence, score caps, mistake learning, validation honesty, and docs-only truth. |
| `AGENT_RUN_LOG_ENFORCEMENT.md` | Hard gate: no complete run log / classified mistakes means no high-confidence Done. |
| `../.ai/RUN_LOG_TEMPLATE.md` | Copyable compact run-log template. |
| `../.ai/runs/README.md` | Run-log naming, required evidence, and learning rules. |
| `ai/learning/MISTAKE_LEDGER.md` | Active memory of repeated AgentsWatch agent mistakes. |
| `DOCS_GOVERNANCE.md` | Source-of-truth, broken-reference, and docs-update rules. |
| `DOCUMENTATION_AUDIT_2026_06_29.md` | Latest docs audit, findings, fixes, and remaining gaps. |
| `PRODUCTIZATION_EXPANSION_2026_06_29.md` | Productization expansion evidence and remaining gaps. |
| `AGENT_OPERATING_SYSTEM.md` | Canonical agent workflow adapted from MathLearning. |
| `CONTEXT_INDEX.md` | Choose the smallest useful docs before an agent run. |
| `CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md` | Token/context economy blueprint: context packs, repo maps, cache-aware prompts, stale-context guard, dogfood metrics. |
| `TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md` | Industry research synthesis: prompt caching, repo maps, path-scoped rules, AGENTS minimalism, stale context, context budgets, and cost-per-solved-task. |
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

## Agent workflow, evidence, and learning

| Document | Use for |
|---|---|
| `AGENT_SHARED_OPERATING_STANDARD.md` | Shared cross-repo minimum behavior for all agents. |
| `AGENT_RUN_LOG_ENFORCEMENT.md` | Score caps, Done-row blocker, mistake classification, final response requirements. |
| `AGENT_RUN_EVIDENCE_STANDARD.md` | Mandatory `.ai/runs` evidence fields, waste categories, mistake-learning hooks, optimized prompt rules. |
| `WASTE_LEARNING_LOOP.md` | Convert wasted steps into docs/rule/queue updates and new optimized prompts. |
| `ai/learning/MISTAKE_LEDGER.md` | Known AW-MISTAKE-* patterns and prevention rules. |
| `ai/learning/MISTAKE_CARD_TEMPLATE.md` | Template for new mistake cards. |
| `ai/prompts/RUN_LOG_EVIDENCE_LINT_PROMPT.md` | Lint queue rows and run logs for missing evidence, score-cap, and mistake-learning gaps. |
| `ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md` | Roll up recent run logs and update ledger/rules/prompts. |
| `ai/prompts/CROSS_REPO_AGENT_STANDARD_SYNC_PROMPT.md` | Keep AgentsWatch, MathLearning backend, and Flutter rules aligned. |
| `CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md` | Best-practice blueprint for reducing token/context waste without harming correctness. |
| `TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md` | Research-backed strategy for token/context economy, including industry findings and prioritized implementation roadmap. |
| `PROMPT_TOKEN_ECONOMY_RULEBOOK.md` | Hard anti-waste rules, budgets, limits, stop rules, forbidden prompt phrases. |
| `PROMPT_LINT_CHECKLIST.md` | Pre-run pass/fail checklist for prompt quality and token discipline. |
| `ZERO_WASTE_EXECUTION_PROTOCOL.md` | Mandatory execution protocol for minimal context, discovery, patching, validation, and waste checkpoints. |
| `PROMPT_BATCH_REVIEW_POLICY.md` | Batch review after 3-5 important prompt/rule/queue/evidence commits. |
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
| `COMMAND_CONTRACTS.md` | Detailed behavior contract for each CLI command. |
| `CLI_UX_OUTPUT_SPEC.md` | Expected CLI output, labels, and test anchors. |
| `CONFIG_REFERENCE.md` | Draft config schema and parser behavior. |
| `REPORT_FORMATS.md` | Markdown run report, handoff, review prompt formats. |
| `DATA_MODEL.md` | Future markdown/JSON/SQLite model. |
| `ADAPTER_SPEC.md` | Universal, .NET, Flutter, React, Python, Node adapter scope. |
| `RISK_SCORING_MODEL.md` | Transparent risk scoring inputs, levels, and reasons. |
| `TOKEN_WASTE_METRICS.md` | Token waste metrics, cache/context metrics, stale-context tracking, and safe savings claims. |
| `PROMPT_OPTIMIZATION_PLAYBOOK.md` | Prompt risk, budget, split, handoff, diff-only review. |

## Productization and delivery

| Document | Use for |
|---|---|
| `MVP_EPICS_AND_ACCEPTANCE.md` | Implementation epics and acceptance criteria. |
| `ISSUE_BACKLOG.md` | Issue-ready backlog for GitHub issues/PRs/agent tasks. |
| `USER_PERSONAS_AND_JOBS.md` | Target users and jobs to be done. |
| `POSITIONING_AND_PRICING_HYPOTHESES.md` | Positioning, free/pro/team hypotheses, evidence rules. |
| `RELEASE_AND_PACKAGING_PLAN.md` | Local tool packaging, release stages, version plan. |
| `EXAMPLES_CATALOG.md` | Planned examples and example quality rules. |
| `INTEGRATION_STRATEGY.md` | Local-first integration order before GitHub/SaaS/LLM integration. |
| `DOGFOOD_RUNBOOK.md` | Operational dogfood workflow and evidence template. |

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
| `prompt_queues/PROMPT_QUEUE_ROUTER.md` | First stop for choosing the next prompt. |
| `prompt_queues/NEXT_PROMPT_FAST_PATH.md` | Copy-ready safest next prompt. |
| `prompt_queues/PROMPT_SYSTEM_AUDIT_2026_06_29.md` | Prompt-system reorganization evidence. |
| `prompt_queues/bootstrap_validation.md` | Validation-first prompts. |
| `prompt_queues/agent_evidence_validation_followups_2026_07_01.md` | Evidence validator/workflow dogfood prompts before feature work resumes. |
| `prompt_queues/token_economy_hardening_2026_07_01.md` | Token/context economy prompts: packs, repo maps, cache-aware skeletons, stale-context guard, dogfood measurement, CLI command contracts. |
| `prompt_queues/token_economy_industry_followups_2026_07_01.md` | Advanced industry-backed token economy prompts: config smell checks, queue token budgets, cost-per-solved-task, provider cache profiles, evidence compaction. |
| `prompt_queues/agentwatch_mvp.md` | CLI MVP implementation prompts. |
| `prompt_queues/roadmap_execution.md` | Roadmap execution prompts. |
| `prompt_queues/architecture_evolution.md` | Safe architecture evolution prompts after Gate 0. |
| `prompt_queues/productization.md` | Productization prompts after Gate 0. |

## Rule

If documents disagree, use current code/tests first, then `AGENTS.md`, then `AGENT_SHARED_OPERATING_STANDARD.md`, then `.ai/RUN_LOG_TEMPLATE.md` / `.ai/runs/README.md`, then `TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md` for token-economy strategy, then `CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md`, then `ai/learning/MISTAKE_LEDGER.md`, then prompt queue router, then prompt token economy rulebook, then prompt batch review policy, then run evidence standard, then bootstrap validation docs, then docs governance, then agent workflow docs, then architecture docs, then product/roadmap docs.
