# Competitive Validation Addendum — 2026-09-16

Status: current strategy addendum
Purpose: update AgentsWatch validation priorities using September 2026 market evidence.

## Decision

AgentsWatch should **not** position generic AI-code tracking, agent session logging, generic governance, or generic AI code review as its moat.

Current competitive pressure:

- Cursor exposes an Enterprise AI Code Tracking API;
- GitHub Copilot coding-agent commits can be traced back to session logs and enterprise audit events;
- Qodo explicitly brings independent review/governance into coding-agent workflows;
- Harness and Snyk are expanding adjacent enterprise agent governance/security surfaces.

This strengthens the case for governance demand while narrowing the independent whitespace.

## Surviving wedge to validate

> **Execution-independent, cross-vendor verification:** a machine-checkable Run Contract, start-state attribution, attributable repository delta, validation evidence, claims/scope checks, and one vendor-neutral Run Receipt.

The value claim must be:

> AgentsWatch catches a decision-relevant verification/evidence problem that the executing agent's own summary, Git diff, CI, PR review, or native vendor logs did not surface early or consistently enough.

## What is not differentiation

- percentage of AI-generated code;
- session history alone;
- generic diff viewing;
- token/cost dashboards;
- another generic AI code reviewer;
- generic orchestration/control-plane features.

## Required external validation before product expansion

After the 30-run dogfood gate:

1. test with at least 10 external developers/teams using real coding-agent work;
2. include at least two agent ecosystems/vendors across the cohort where practical;
3. record whether the Run Receipt changes review, merge, rework, or evidence decisions;
4. compare explicitly against Git + CI + PR review + native vendor logs;
5. collect false-positive/attribution ambiguity evidence;
6. require at least 3 requests for continued use before investing in broad integrations/dashboard/team packaging;
7. test real payment/design-partner commitment before SaaS/auth/billing work.

## Kill/narrow condition

Do not rescue the thesis by adding more features if external users consistently conclude that native platform evidence plus Git/CI/PR review is sufficient.

If the independent verification wedge does not change real decisions, narrow or stop the product.

## Source notes

Current research references used for this strategy update:

- Cursor AI Code Tracking API: https://prod.cursor.com/docs/account/teams/ai-code-tracking-api
- GitHub Copilot agent session tracking: https://docs.github.com/en/copilot/how-tos/copilot-on-github/use-copilot-agents/manage-and-track-agents
- GitHub agent audit events: https://docs.github.com/en/copilot/reference/enterprise-administrators/agentic-audit-log-events
- Qodo Agentic Toolbox: https://www.qodo.ai/blog/introducing-qodos-agentic-toolbox/
- Harness Agent DLC: https://www.harness.io/press-and-news/introducing-harness-agent-dlc
- Snyk Agentic Development Security: https://snyk.io/news/snyk-launches-evo-agentic-development-security/

These sources show competitive pressure; they do not prove that any one competitor fully solves the AgentsWatch run-receipt thesis.
