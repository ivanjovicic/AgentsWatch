# AgentsWatch Documentation Index

Last aligned: 2026-07-03

## Start here

| Document | Use for |
|---|---|
| `../README.md` | Project overview and current status. |
| `../AGENTS.md` | Agent rules for AI-assisted work. |
| `AGENT_SHARED_OPERATING_STANDARD.md` | Shared rules for prompt shape, token budget, evidence, validation honesty, and docs-only truth. |
| `AGENT_RUN_LOG_ENFORCEMENT.md` | Hard gate: no complete run log / classified mistakes means no high-confidence Done. |
| `PROOF_AND_VERIFICATION_STRATEGY.md` | Canonical capability maturity and product-proof rules. |
| `FEATURE_CAPABILITY_REGISTRY.md` | Truthful inventory of supported, implemented, specified, and unverified capabilities. |
| `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md` | Claim-to-contract-to-test-to-CI-to-release evidence links. |
| `DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md` | Capture, route, document, prompt, and close findings outside active scope. |
| `DISCOVERY_INDEX.md` | Discovery-system entry point. |
| `../.ai/RUN_LOG_TEMPLATE.md` | Compact run-log template. |
| `../.ai/DISCOVERY_RECORD_TEMPLATE.md` | Discovery record template. |
| `../.ai/runs/README.md` | Run-log naming and evidence rules. |
| `ai/learning/MISTAKE_LEDGER.md` | Active repeated-mistake memory. |
| `DOCS_GOVERNANCE.md` | Source-of-truth and docs-update rules. |
| `AGENT_OPERATING_SYSTEM.md` | Canonical agent workflow. |
| `CONTEXT_INDEX.md` | Choose the smallest useful context. |
| `CONTEXT_PACKS.md` | Active context-pack registry. |
| `PRODUCT_SPEC.md` | Product positioning and MVP features. |
| `CLI_SPEC.md` | CLI commands and outputs. |
| `MVP_ROADMAP.md` | MVP phases and priority order. |
| `ROADMAP_INDEX.md` | Roadmap entry point. |

## Proof and verification

| Document | Use for |
|---|---|
| `PROOF_AND_VERIFICATION_STRATEGY.md` | L0-L6 maturity, evidence hierarchy, false-proof prevention, and completion gate. |
| `FEATURE_CAPABILITY_REGISTRY.md` | Current capability/maturity truth. |
| `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md` | Acceptance/test/scenario/CI/dogfood/release traceability. |
| `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md` | Black-box scenarios for current and planned commands. |
| `PROOF_BUNDLE_SPEC.md` | Commit-bound CI/release artifact and manifest format. |
| `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` | Paired usefulness/token/context benchmark rules. |
| `INDEPENDENT_VERIFICATION_RUNBOOK.md` | Clean-install, black-box, independent release review. |
| `prompt_queues/agentwatch_proof_and_verification.md` | Proof implementation and workflow queue. |
| `prompts/PROOF-001-capability-inventory-audit.md` | Audit claims against current code/test evidence. |
| `prompts/PROOF-002-traceability-review.md` | Review proof chain completeness. |
| `prompts/PROOF-003-black-box-acceptance.md` | Run pinned black-box acceptance. |
| `prompts/PROOF-004-proof-bundle-review.md` | Review manifest, artifacts, checksums, and maturity. |
| `prompts/PROOF-005-dogfood-benchmark.md` | Paired baseline/assisted value proof. |
| `prompts/PROOF-006-independent-verification.md` | Clean-install independent verification. |
| `prompts/PROOF-007-release-claim-certification.md` | Certify or downgrade release/marketing claims. |

## Bootstrap and validation

| Document | Use for |
|---|---|
| `BUILD_VALIDATION_PLAN.md` | Restore/build/test/CLI smoke order. |
| `VALIDATION_EVIDENCE_2026_06_29.md` | Historical Gate 0 evidence; must be updated from current CI. |
| `RISK_REGISTER.md` | Bootstrap and product risks. |
| `BOOTSTRAP_NEXT_STEPS.md` | Required next order before feature work. |
| `PROJECT_READINESS_CHECKLIST.md` | Checklist before CLI feature expansion. |
| `ROADMAP_VALIDATION_GATES.md` | Phase gates and stop rules. |
| `TEST_STRATEGY.md` | Test layers and command coverage. |
| `TEST_MATRIX.md` | High-risk coverage summary. |
| `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md` | Observable end-to-end behavior. |

## Agent workflow, evidence, learning, and discovery

| Document | Use for |
|---|---|
| `AGENT_SHARED_OPERATING_STANDARD.md` | Cross-repo minimum agent behavior. |
| `AGENT_RUN_LOG_ENFORCEMENT.md` | Score caps, Done blocker, mistake classification. |
| `AGENT_RUN_EVIDENCE_STANDARD.md` | Mandatory evidence fields and learning hooks. |
| `WASTE_LEARNING_LOOP.md` | Convert waste into prevention. |
| `DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md` | Durable out-of-scope finding lifecycle. |
| `DISCOVERY_ARCHITECTURE_ADDENDUM.md` | Discovery data flow. |
| `DISCOVERY_CLI_CONTRACT.md` | Planned discovery commands. |
| `DISCOVERY_DATA_MODEL_ADDENDUM.md` | Discovery markdown/JSON/SQLite model. |
| `ai/learning/MISTAKE_LEDGER.md` | Known AW-MISTAKE patterns. |
| `ai/learning/MISTAKE_CARD_TEMPLATE.md` | New mistake template. |
| `ai/prompts/RUN_LOG_EVIDENCE_LINT_PROMPT.md` | Evidence and score-cap lint. |
| `ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md` | Roll up recent run logs. |
| `CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md` | Context/token economy blueprint. |
| `TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md` | Industry research synthesis. |
| `TOKEN_ECONOMY_PREVIOUS_CONVERSATION_BACKFILL_2026_07_01.md` | Prior patterns preserved outside always-loaded rules. |
| `PROMPT_TOKEN_ECONOMY_RULEBOOK.md` | Hard anti-waste rules. |
| `PROMPT_LINT_CHECKLIST.md` | Pre-run prompt pass/fail checklist. |
| `ZERO_WASTE_EXECUTION_PROTOCOL.md` | Minimal execution protocol. |
| `PROMPT_BATCH_REVIEW_POLICY.md` | Batch review after significant prompt/rule changes. |
| `AGENT_COMMAND_PLAYBOOK.md` | Shell-neutral validation commands. |
| `AGENT_LONG_TASK_PLAYBOOK.md` | Long-task control. |
| `AGENT_PATCH_PLAYBOOK.md` | Small safe patch strategy. |
| `PROMPT_RULES.md` | Required prompt sections. |
| `PROMPT_QUALITY_CHECKLIST.md` | Prompt preflight. |
| `PROMPT_EVIDENCE_TEMPLATE.md` | Done/Blocked evidence row. |
| `COMPLETION_ANALYTICS.md` | Completion/missed/follow-up mapping. |
| `CLAIMS_VS_ACTUAL_REVIEW.md` | Check final claims against actual evidence. |

## Architecture and product contracts

| Document | Use for |
|---|---|
| `ARCHITECTURE.md` | Current MVP architecture snapshot. |
| `TARGET_ARCHITECTURE.md` | Future-proof target architecture. |
| `ARCHITECTURE_DECISIONS.md` | ADRs and tradeoffs. |
| `MODULE_BOUNDARIES.md` | Dependency direction and ownership. |
| `COMMAND_CONTRACTS.md` | Detailed CLI command behavior. |
| `CLI_UX_OUTPUT_SPEC.md` | Output labels and test anchors. |
| `CONFIG_REFERENCE.md` | Config schema. |
| `REPORT_FORMATS.md` | Reports, handoffs, and review prompts. |
| `DATA_MODEL.md` | Markdown/JSON/SQLite model. |
| `ADAPTER_SPEC.md` | Stack adapter scope. |
| `RISK_SCORING_MODEL.md` | Explainable risk scoring. |
| `TOKEN_WASTE_METRICS.md` | Token/context metrics and safe claims. |
| `PROMPT_OPTIMIZATION_PLAYBOOK.md` | Prompt risk, split, handoff, and review. |

## Productization and delivery

| Document | Use for |
|---|---|
| `MVP_EPICS_AND_ACCEPTANCE.md` | Implementation epics and acceptance criteria. |
| `ISSUE_BACKLOG.md` | Issue-ready backlog. |
| `USER_PERSONAS_AND_JOBS.md` | Users and jobs. |
| `POSITIONING_AND_PRICING_HYPOTHESES.md` | Positioning/pricing hypotheses and evidence rules. |
| `RELEASE_AND_PACKAGING_PLAN.md` | Packaging/release stages. |
| `EXAMPLES_CATALOG.md` | Example quality rules. |
| `INTEGRATION_STRATEGY.md` | Local-first integration order. |
| `DOGFOOD_RUNBOOK.md` | Operational dogfood workflow. |
| `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` | Controlled usefulness evidence. |
| `INDEPENDENT_VERIFICATION_RUNBOOK.md` | Release-candidate verification. |

## Quality and safety

| Document | Use for |
|---|---|
| `TEST_MATRIX.md` | Test areas by feature. |
| `TEST_STRATEGY.md` | Detailed testing strategy. |
| `SECURITY_AND_PRIVACY.md` | Local-first security/privacy. |
| `DOGFOOD_PLAN.md` | Real-repo dogfood workflow. |
| `../CONTRIBUTING.md` | Contributor workflow. |
| `../SECURITY.md` | Security reporting. |

## Prompt queues

| Document | Use for |
|---|---|
| `prompt_queues/PROMPT_QUEUE_ROUTER.md` | First stop for selecting work. |
| `prompt_queues/NEXT_PROMPT_FAST_PATH.md` | Copy-ready next prompt. |
| `prompt_queues/bootstrap_validation.md` | Validation-first prompts. |
| `prompt_queues/agentwatch_proof_and_verification.md` | Proof, acceptance, CI, benchmark, release verification. |
| `prompt_queues/agentwatch_discovery_and_self_improvement.md` | Discovery capture/routing and runtime slices. |
| `prompt_queues/agent_evidence_validation_followups_2026_07_01.md` | Evidence validator prompts. |
| `prompt_queues/token_economy_hardening_2026_07_01.md` | Token/context hardening. |
| `prompt_queues/token_economy_industry_followups_2026_07_01.md` | Advanced token/context work. |
| `prompt_queues/agentwatch_mvp.md` | CLI MVP implementation. |
| `prompt_queues/roadmap_execution.md` | Roadmap execution. |
| `prompt_queues/architecture_evolution.md` | Architecture evolution after gates. |
| `prompt_queues/productization.md` | Productization after gates. |

## Rule

When documents disagree, use current code/tests and commit-matched proof first, then `AGENTS.md`, proof strategy/registry/matrix, shared operating standard, run-log/discovery contracts, prompt router, bootstrap validation evidence, and finally planning/roadmap documents.
