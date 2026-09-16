# Sources

Research cut-off: **2026-09-16**  
Access date unless otherwise stated: **2026-09-16**

Primary/official sources are preferred. Vendor claims are labeled as vendor evidence, not independent proof of product quality.

## Repository / CI

### R01 — AgentsWatch current main
- URL: https://github.com/ivanjovicic/AgentsWatch/tree/main
- Confidence: HIGH
- Supports: current repo structure/product truth.

### R02 — AgentsWatch audited CI run
- URL: https://github.com/ivanjovicic/AgentsWatch/actions/runs/35087436507
- Date: 2026-09-16
- Confidence: HIGH
- Supports: restore/build pass, one failing GitStatusParser test.

## Market / behavior

### M01 — JetBrains AI Coding Agents: Adoption Trends
- URL: https://blog.jetbrains.com/research/2026/08/ai-coding-agent-adoption-2026/
- Date: 2026-08
- Type: survey / primary publisher
- Confidence: HIGH for surveyed population
- Supports: rapid professional coding-agent adoption.

### M02 — Snyk: developer-environment agentic risk study
- URL: https://snyk.io/blog/agentic-development-security-ai-coding-risk/
- Date: 2026-06-23
- Type: vendor research
- Confidence: MEDIUM-HIGH
- Supports: in nearly 10,000 observed developer environments, Snyk reports 43% using two or more AI coding environments and >50% with at least one MCP server.

## Direct / adjacent competitors

### C01 — Qodo Agentic Toolbox
- URL: https://www.qodo.ai/blog/introducing-qodos-agentic-toolbox/
- Date: 2026-09-09
- Type: vendor primary
- Confidence: HIGH for advertised capability
- Supports: independent review on committed/uncommitted local changes, context/rules, agent workflow integration.

### C02 — Qodo for Codex
- URL: https://www.qodo.ai/blog/introducing-qodo-for-codex/
- Date: 2026-09-09
- Type: vendor primary
- Confidence: HIGH for advertised capability
- Supports: review of committed/uncommitted/untracked local work, ticket requirements and organizational rules inside Codex workflow.

### C03 — Cursor AI Code Tracking API
- URL: https://prod.cursor.com/docs/account/teams/ai-code-tracking-api
- Type: official docs
- Confidence: HIGH
- Supports: Enterprise Alpha per-commit AI contribution/accepted-change metrics.

### C04 — GitHub Copilot agent session tracking
- URL: https://docs.github.com/en/copilot/how-tos/copilot-on-github/use-copilot-agents/manage-and-track-agents
- Type: official docs
- Confidence: HIGH
- Supports: session logs/agent execution traceability.

### C05 — GitHub Copilot hooks
- URL: https://docs.github.com/en/copilot/concepts/agents/hooks
- Type: official docs
- Confidence: HIGH
- Supports: lifecycle policy/validation hooks.

### C06 — GitHub agentic audit log events
- URL: https://docs.github.com/en/copilot/reference/enterprise-administrators/agentic-audit-log-events
- Type: official docs
- Confidence: HIGH
- Supports: enterprise agent audit/session events.

### C07 — Claude Code permissions
- URL: https://code.claude.com/docs/en/permissions
- Type: official docs
- Confidence: HIGH
- Supports: fine-grained permission rules and managed policy.

### C08 — Claude Code / Agent SDK hooks
- URL: https://code.claude.com/docs/en/agent-sdk/hooks
- Type: official docs
- Confidence: HIGH
- Supports: tool/session lifecycle hooks, logging/audit, blocking/approval and policy controls.

### C09 — Claude Code hooks guide
- URL: https://code.claude.com/docs/en/hooks-guide
- Type: official docs
- Confidence: HIGH
- Supports: deterministic command hooks and Stop-time validation; agent/prompt hooks also exist.

### C10 — OpenAI: Running Codex safely at OpenAI
- URL: https://openai.com/index/running-codex-safely/
- Date: 2026-05-08
- Type: vendor primary
- Confidence: HIGH for described OpenAI practices
- Supports: technical boundaries, approvals and agent-native telemetry.

### C11 — OpenAI Managed Agents sessions API
- URL: https://developers.openai.com/api/reference/typescript/resources/beta/subresources/agents/subresources/sessions/methods/create
- Type: official API docs
- Confidence: HIGH
- Supports: structured managed-agent sessions/events/artifacts; not proof of AgentsWatch-equivalent verification.

### C12 — CodeRabbit CLI
- URL: https://docs.coderabbit.ai/cli
- Type: official docs
- Confidence: HIGH
- Supports: local uncommitted-change review before commit.

### C13 — CodeRabbit IDE/CLI agent integration
- URL: https://docs.coderabbit.ai/overview/ide-cli-review
- Type: official docs
- Confidence: HIGH
- Supports: local review, Claude plugin, integration with Cursor/Codex/other agents.

### C14 — CodeRabbit pricing
- URL: https://www.coderabbit.ai/pricing
- Type: vendor primary
- Confidence: HIGH
- Supports: $24/$48/$72 annual per-developer plans and enterprise controls; current pricing page also describes CLI/MCP/agentic review.

### C15 — Graphite AI Reviews
- URL: https://graphite.com/docs/ai-reviews
- Type: official docs
- Confidence: HIGH
- Supports: AI PR review.

### C16 — Graphite pricing
- URL: https://www.graphite.com/pricing
- Type: vendor primary
- Confidence: HIGH
- Supports: $20 Starter / $40 Team per user monthly when billed annually.

### C17 — Graphite Agents
- URL: https://graphite.com/docs/agents
- Type: official docs
- Confidence: HIGH
- Supports: Cursor Cloud Agents creating/updating PRs through Graphite.

### C18 — Devin Review
- URL: https://docs.devin.ai/work-with-devin/devin-review
- Type: official docs
- Confidence: HIGH
- Supports: dedicated PR review product.

### C19 — Devin enterprise audit logs
- URL: https://docs.devin.ai/api-reference/v3/audit-logs/enterprise-audit-logs
- Type: official docs
- Confidence: HIGH
- Supports: enterprise audit API.

### C20 — OpenHands file-based agents
- URL: https://docs.openhands.dev/sdk/guides/agent-file-based
- Type: official docs
- Confidence: MEDIUM for competitive comparison
- Supports: customizable agent/reviewer workflows; does not by itself prove run-receipt semantics.

### C21 — Sourcegraph Agentic Batch Changes
- URL: https://sourcegraph.com/docs/agentic-batch-changes
- Type: official docs
- Confidence: HIGH
- Supports: planning, staged execution, CI iteration and changeset orchestration.

### C22 — Sourcegraph Agentic Batch Changes GA
- URL: https://sourcegraph.com/changelog/agentic-batch-changes-ga
- Date: 2026-09-14
- Type: vendor primary
- Confidence: HIGH
- Supports: current GA status and outcome-based pricing model.

### C23 — Sourcegraph audit changes
- URL: https://sourcegraph.com/changelog/2026-08-10
- Date: 2026-08-10
- Type: vendor primary
- Confidence: HIGH
- Supports: structured audit events for batch specs/commands/agent inputs.

### C24 — Snyk Agentic Development Security
- URL: https://snyk.io/news/snyk-launches-evo-agentic-development-security/
- Date: 2026-06-23
- Type: vendor primary
- Confidence: HIGH for positioning
- Supports: adjacent real-time agent governance/security.

## Pricing anchors

### P01 — GitHub Copilot plans
- URL: https://docs.github.com/en/copilot/get-started/plans
- Confidence: HIGH
- Supports: Business/Enterprise pricing references.

### P02 — Cursor team pricing
- URL: https://cursor.com/blog/teams-pricing-june-2026
- Date: 2026-06
- Confidence: HIGH
- Supports: team pricing anchor.

### P03 — LangSmith pricing
- URL: https://www.langchain.com/pricing
- Confidence: HIGH
- Supports: adjacent agent tooling price anchor.

### P04 — Snyk plans
- URL: https://snyk.io/plans/
- Confidence: HIGH
- Supports: developer/security tooling price anchor.

## Standards / interoperability

### S01 — SLSA 1.2 specification
- URL: https://slsa.dev/spec/v1.2/
- Type: open specification
- Confidence: HIGH
- Supports: current approved provenance/source/build framework.

### S02 — SLSA provenance
- URL: https://slsa.dev/spec/v1.2/provenance
- Type: open specification
- Confidence: HIGH
- Supports: verifiable information about where/when/how artifacts are produced.

### S03 — SLSA Source requirements
- URL: https://slsa.dev/spec/v1.2/source-requirements
- Type: open specification
- Confidence: HIGH
- Supports: source provenance and enforced change-management controls.

### S04 — in-toto Attestation Framework
- URL: https://github.com/in-toto/attestation
- Type: CNCF/open specification
- Confidence: HIGH
- Supports: authenticated metadata and custom predicates for software artifacts/processes.

### S05 — in-toto Attestation spec v1.2
- URL: https://github.com/in-toto/attestation/blob/main/spec/README.md
- Type: open specification
- Confidence: HIGH
- Supports: statement/predicate/envelope/bundle model.

### S06 — Sigstore/Cosign attestations
- URL: https://github.com/sigstore/docs/blob/main/content/en/quickstart/quickstart-cosign.md
- Type: open project docs
- Confidence: HIGH
- Supports: signing/verification of in-toto attestations.

### S07 — OpenTelemetry GenAI semantic conventions
- URL: https://github.com/open-telemetry/semantic-conventions-genai
- Type: open specification project
- Confidence: MEDIUM-HIGH; agent spans are still evolving
- Supports: agent/tool/workflow telemetry interoperability, not completion verification.

### S08 — MCP 2026-07-28 specification release
- URL: https://blog.modelcontextprotocol.io/posts/2026-07-28/
- Date: 2026-07-28
- Type: protocol maintainer publication
- Confidence: HIGH
- Supports: MCP as an agent-tool interoperability substrate.

## Limitations

- Vendor capability claims describe advertised/current documented features, not independent quality benchmarks.
- Capability matrix `UNKNOWN` values are deliberately preserved when documentation did not justify a stronger claim.
- Pricing can change after the research cut-off.
- Unit economics, TAM ranges and AgentsWatch pricing are hypotheses until external usage/payment evidence exists.
- No source currently proves that a buyer will pay for AgentsWatch specifically.
