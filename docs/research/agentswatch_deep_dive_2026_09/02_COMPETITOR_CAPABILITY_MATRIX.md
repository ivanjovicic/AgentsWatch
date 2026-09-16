# Competitor Capability Matrix — Interpretation

Full machine-readable matrix: `data/competitor_capability_matrix.csv`.

Research cut-off: **2026-09-16**.

## Evidence rule

The matrix is intentionally conservative:

- `YES` only when current primary/official material clearly supports the capability.
- `PARTIAL` when only part of the capability is evidenced.
- `NO` when the product category/evidence clearly excludes it.
- `UNKNOWN` when current evidence is insufficient.

`UNKNOWN` does **not** mean the competitor lacks the capability.

## Direct competitive pressure

### Qodo — closest direct competitor

Qodo Agentic Toolbox and Qodo for Codex currently support independent review of local committed/uncommitted changes; the Codex integration explicitly mentions untracked files, ticket requirements, organizational rules, security/correctness review and review-resolution loops.

This overlaps materially with the old “independent reviewer” framing.

What is not clearly evidenced as a complete Qodo primitive:
- a persisted pre-run dirty-state baseline;
- explicit start/end run-boundary attribution with `preExistingChangedFurther` semantics;
- vendor-neutral completion receipt whose decision is based on required validation evidence rather than review findings.

### GitHub Copilot — strongest platform threat

GitHub increasingly owns repository-native traceability:
- agent session management/logs;
- commit/session linkage;
- hooks/policy surfaces;
- audit events;
- repository/PR/CI identity.

If GitHub standardizes vendor-neutral agent provenance, AgentsWatch should integrate with it rather than compete with the standard.

### Cursor

Cursor’s Enterprise AI Code Tracking API provides per-commit AI-contribution metrics and accepted AI changes. This is strong attribution telemetry but not equivalent to evidence-backed task completion.

### Claude Code

Claude Code provides fine-grained permissions and rich lifecycle hooks that can block/allow tools, log activity, run deterministic commands, validate before Stop, and audit session/tool events. This means AgentsWatch cannot claim policy hooks or “run tests before stopping” as unique.

### OpenAI Codex

OpenAI publicly describes Codex governance in terms of technical boundaries, approvals and agent-native telemetry. Current OpenAI managed-agent APIs also expose structured session events/artifacts. AgentsWatch must remain useful without depending on or duplicating executor-native telemetry.

### CodeRabbit / Graphite / Devin Review

These products intensify the generic review market:
- CodeRabbit reviews uncommitted local changes via CLI and integrates with coding agents/MCP;
- Graphite AI Reviews/Agent target PR review and agent-created PR workflows;
- Devin Review is a dedicated review product with enterprise audit surfaces.

AgentsWatch should consume review outputs later if useful, not rebuild generic code-review engines.

### Sourcegraph

Agentic Batch Changes now plans, executes, monitors CI and coordinates large changes across repositories, including delegation to Codex/Claude Code. Sourcegraph also emits structured audit events for batch executions. This reinforces that orchestration/control-plane breadth is a bad wedge for AgentsWatch.

## Adjacent, not direct

### Snyk / Semgrep / Sonar / Harness

Security/policy/governance products compete for the same enterprise budget and can become evidence providers, but they are not direct substitutes for a run-scoped repository receipt unless they add that exact workflow.

### LangSmith / Langfuse / Braintrust / Arize / OpenTelemetry

These are primarily observability/evaluation/telemetry layers. They matter for agent traces and evaluation, but should not be treated as direct code-run verification substitutes unless they add repository-delta and completion semantics.

## Ruthless classification of AgentsWatch capabilities

### A — potentially unique / hard to replace
- pre-run dirty-state baseline tied to one delegated run;
- explicit `preExistingChangedFurther` / ambiguity semantics;
- required-evidence completion gate independent of agent prose;
- portable vendor-neutral run receipt;
- deterministic claim checks tied to run-interval delta.

### B — useful combination, not individually unique
- owned/avoid-path scope policy;
- acceptance-criterion evidence mapping;
- normalized validation evidence;
- auditable override;
- compact handoff from structured evidence.

### C — commodity
- Git diff/status;
- generic PR/code review;
- session/tool logs;
- AI-code percentage;
- test execution;
- generic policy rules;
- dashboards;
- token/cost telemetry.

### D — should remain de-prioritized
- generic prompt optimizer as the main product;
- proprietary agent runtime;
- broad orchestration/control plane;
- session archive;
- generic AI reviewer;
- sophisticated learning/router before evidence proof.
