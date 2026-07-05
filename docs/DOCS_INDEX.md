# AgentsWatch Documentation Index

Last aligned: 2026-07-05

## Start here

| Document | Use for |
|---|---|
| `../README.md` | Project overview and current status. |
| `../AGENTS.md` | Agent rules for AI-assisted work. |
| `PRODUCT_SPEC.md` | Evidence-first positioning, strategic lanes, first market product, and MVP boundaries. |
| `MVP_ROADMAP.md` | Core roadmap, 30-PR Market Gate M, and community-opportunity incubator gates. |
| `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md` | Current evidence on real agent problems, revised product focus, competitive boundary, and what remains unproven. |
| `PR_EVIDENCE_MARKET_VALIDATION_RUNBOOK.md` | Execute the 30-real-PR experiment and decide Advance/Revise/Park/Reject. |
| `PRODUCT_FORM_FACTORS_INSTALLATION_AND_DELIVERY_PLAN.md` | Canonical one-product plan: CLI, adapters, optional service/dashboard/IDE, GitHub Action/App, Team Server, installation, and delivery order. |
| `PROOF_AND_VERIFICATION_STRATEGY.md` | Capability maturity and product-proof rules. |
| `FEATURE_CAPABILITY_REGISTRY.md` | Truthful inventory of supported, planned, and unverified capabilities. |
| `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md` | Claim-to-contract-to-test-to-CI-to-release links. |
| `COMPATIBILITY_INDEX.md` | Cross-model, tool, permission, environment, and fallback entry point. |
| `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md` | External problem research and source catalogue. |
| `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md` | Revised evidence-first ranking and portfolio strategy. |
| `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md` | Detailed product epics, CLI concepts, acceptance, and proof gates. |
| `COMMUNITY_OPPORTUNITY_BACKLOG.md` | Issue-ready research, foundation, prototype, and dogfood slices. |

## Market and problem validation

| Document | Use for |
|---|---|
| `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md` | Separates strong external problem signals from missing AgentsWatch-specific demand/payment evidence. |
| `PR_EVIDENCE_MARKET_VALIDATION_RUNBOOK.md` | Sample, packet format, metrics, privacy, thresholds, and decision process for 30 PRs. |
| `POSITIONING_AND_PRICING_HYPOTHESES.md` | Evidence-first public messaging, Free/Pro/Team hypotheses, buyer roles, and claims that remain prohibited. |
| `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md` | Ranks PR Evidence/Trust Ledger first, then context, rules, offline waste, and compatibility. |
| `USER_PERSONAS_AND_JOBS.md` | Target users and jobs to be done. |
| `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md` | Controlled usefulness and efficiency evidence. |

Current decision: the broad control-plane architecture remains valid, but the first market-facing experiment is `PR Evidence + Trust Ledger`. External research establishes a strong problem signal; it does not establish product-market fit or willingness to pay.

## Runtime compatibility

| Document | Use for |
|---|---|
| `COMPATIBILITY_INDEX.md` | Entry point and proof order. |
| `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md` | Detailed research, compatibility dimensions, per-feature adaptations, and feasibility matrix. |
| `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md` | Effective runtime profile, support-mode decision, provenance, confidence, downgrade, and fallback contract. |
| `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md` | Fifty scenarios across local, IDE, cloud, CI, chat, read-only, no-git, containers, WSL, remote, worktrees, monorepos, and production-risk cases. |
| `ADAPTER_SPEC.md` | Composable tool, surface, model, event, permission, environment, VCS/CI, stack, rules, and usage adapter families. |
| `COMPATIBILITY_IMPLEMENTATION_BACKLOG.md` | COMPAT-001 through COMPAT-020 issue-ready implementation order for AW-CAP-037. |
| `prompts/OPP-004-runtime-compatibility-audit.md` | Audit one real tool/surface/model/permission/environment combination. |

Compatibility research proves that planned concepts are not equally observable or enforceable in every setup. `AW-CAP-037` remains L1; no current runtime automatically negotiates these profiles.

## Community opportunity incubator

| Document | Use for |
|---|---|
| `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md` | Repeated problems from papers, public issue trackers, Reddit/HN research, and security reports. |
| `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md` | Converts external evidence into product priority, competitive boundaries, and validation questions. |
| `POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md` | Revised ranking, Free/Pro/Team portfolio, execution order, and kill criteria. |
| `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md` | Flight Recorder, Context Portability, Loop Guard, Policy, Worktree, and PR Evidence contracts. |
| `COMMUNITY_OPPORTUNITY_ARCHITECTURE_ADDENDUM.md` | Shared normalized local event journal and bounded-context architecture. |
| `COMMUNITY_OPPORTUNITY_BACKLOG.md` | OPP-001 through OPP-081 issue-ready work. |
| `prompt_queues/community_opportunity_validation.md` | Discovery, compatibility, prototype, dogfood, and live-feature gates. |
| `prompts/OPP-001-user-interview-synthesis.md` | Turn interviews into Advance/Revise/Park/Reject decisions. |
| `prompts/OPP-002-adapter-feasibility.md` | Verify stable local event/log/hook inputs and blind spots. |
| `prompts/OPP-003-competitive-substitutes.md` | Compare substitutes, differentiation, and kill conditions. |
| `prompts/OPP-004-runtime-compatibility-audit.md` | Evaluate model/tool/surface/rights/environment support rather than provider-name assumptions. |

Community and market research establishes problem/budget signals, not runtime support, market size, willingness to pay, savings, security effectiveness, equal cross-tool support, or product-market fit. AW-CAP-028 through AW-CAP-037 remain L1 until implementation and proof exist.

## Proof and verification

| Document | Use for |
|---|---|
| `PROOF_AND_VERIFICATION_STRATEGY.md` | L0-L6 maturity, evidence hierarchy, false-proof prevention, and completion gate. |
| `FEATURE_CAPABILITY_REGISTRY.md` | Current capability/maturity truth and market-priority distinction. |
| `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md` | Acceptance/test/scenario/CI/dogfood/release traceability. |
| `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md` | Black-box scenarios for current and planned commands. |
| `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md` | Cross-runtime support/downgrade/fallback scenarios. |
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
| `VALIDATION_EVIDENCE_2026_06_29.md` | Historical Gate 0 evidence. |
| `VALIDATION_EVIDENCE_2026_07_03.md` | Successful PR-branch Linux/Windows/package proof. |
| `RISK_REGISTER.md` | Bootstrap and product risks. |
| `BOOTSTRAP_NEXT_STEPS.md` | Required next order before feature work. |
| `PROJECT_READINESS_CHECKLIST.md` | Checklist before CLI feature expansion. |
| `ROADMAP_VALIDATION_GATES.md` | Phase gates and stop rules. |
| `TEST_STRATEGY.md` | Test layers and command coverage. |
| `TEST_MATRIX.md` | High-risk coverage summary. |

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
| `PRODUCT_FORM_FACTORS_INSTALLATION_AND_DELIVERY_PLAN.md` | One shared core and the planned CLI, adapters, optional service/dashboard/IDE, GitHub, and Team Server topology. |
| `COMMUNITY_OPPORTUNITY_ARCHITECTURE_ADDENDUM.md` | Event-ingestion, trust, context, usage, policy, coordination, and review extensions. |
| `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md` | Compatibility layer required before advanced provider integrations. |
| `ARCHITECTURE_DECISIONS.md` | ADRs and tradeoffs. |
| `MODULE_BOUNDARIES.md` | Dependency direction and ownership. |
| `COMMAND_CONTRACTS.md` | Detailed CLI command behavior. |
| `CLI_UX_OUTPUT_SPEC.md` | Output labels and test anchors. |
| `CONFIG_REFERENCE.md` | Config schema. |
| `REPORT_FORMATS.md` | Reports, handoffs, review prompts, and future PR Evidence output. |
| `DATA_MODEL.md` | Markdown/JSON/SQLite model. |
| `ADAPTER_SPEC.md` | All adapter families and composition rules. |
| `RISK_SCORING_MODEL.md` | Explainable risk scoring. |
| `TOKEN_WASTE_METRICS.md` | Token/context metrics and safe claims. |
| `PROMPT_OPTIMIZATION_PLAYBOOK.md` | Prompt risk, split, handoff, and review. |

## Productization and delivery

| Document | Use for |
|---|---|
| `PRODUCT_FORM_FACTORS_INSTALLATION_AND_DELIVERY_PLAN.md` | Canonical component, installation, edition, and staged-delivery plan. |
| `MVP_EPICS_AND_ACCEPTANCE.md` | Existing implementation epics and acceptance criteria. |
| `COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md` | New opportunity contracts and gates. |
| `PR_EVIDENCE_MARKET_VALIDATION_RUNBOOK.md` | First market experiment before broad infrastructure. |
| `ISSUE_BACKLOG.md` | Existing issue-ready backlog. |
| `COMMUNITY_OPPORTUNITY_BACKLOG.md` | Community-derived discovery/prototype backlog. |
| `COMPATIBILITY_IMPLEMENTATION_BACKLOG.md` | Runtime compatibility and fallback implementation backlog. |
| `USER_PERSONAS_AND_JOBS.md` | Users and jobs. |
| `POSITIONING_AND_PRICING_HYPOTHESES.md` | Evidence-first positioning/pricing hypotheses and evidence rules. |
| `RELEASE_AND_PACKAGING_PLAN.md` | NuGet, standalone, package-manager, Action, optional component, and Team Server packaging stages. |
| `EXAMPLES_CATALOG.md` | Example quality rules. |
| `INTEGRATION_STRATEGY.md` | Local CLI → Action → adapters/service/dashboard/IDE → App/Team integration order. |
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
| `prompt_queues/community_opportunity_validation.md` | Community, market, compatibility, and gated prototype workflow. |
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

When documents disagree, use current code/tests and commit-matched proof first, then `AGENTS.md`, proof strategy/registry/matrix, compatibility contracts, shared operating standard, run-log/discovery contracts, prompt router, bootstrap validation evidence, and finally planning/research/roadmap documents.
