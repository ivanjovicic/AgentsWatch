# AgentsWatch Agent Trust Platform Expansion — 2026-08-21

Status: strategic expansion hypothesis; not current runtime scope  
Scope rule: preserve the local-first Agent Run Receipt MVP; do not start Gateway or Team Server work until explicit activation gates are met.

## Executive decision

AgentsWatch should not pivot into a standalone generic LLM gateway.

The stronger long-term product is:

```text
AgentsWatch = trust and control layer for autonomous coding agents
```

The differentiated wedge remains:

```text
Roadmap intent
  -> bounded execution contract
  -> external coding agent
  -> Agent Run Receipt
  -> claims/diff/validation verification
  -> explainable trust result
```

A future AI Gateway can become one optional module inside AgentsWatch after the evidence/verification product proves useful. It must not replace or delay the core MVP.

## Product thesis

Generic gateways and observability products answer questions such as:

- which model was called;
- how many tokens were used;
- how much the call cost;
- how long it took;
- which provider failed.

AgentsWatch should answer the harder engineering-management questions:

- what was the agent allowed to do;
- what did it actually change;
- did it stay inside scope;
- did required validation run and pass;
- is the agent's `Done` claim supported by evidence;
- how much did one verified task cost;
- which agent/model produces the best verified outcome for this repository;
- how often human rework is needed after agent completion.

Positioning principle:

```text
Observe the run. Control the boundaries. Verify the result.
```

Supporting line:

```text
AgentsWatch verifies the work, not just the traffic.
```

## Long-term product layers

### 1. Observe — current/core direction

Vendor-neutral local evidence:

- run contract;
- git start/end state;
- changed files;
- commands and compact command profiles;
- validation evidence;
- agent claims;
- scope drift;
- missed work;
- run receipt;
- learning note.

This layer is the MVP and remains first priority.

### 2. Control — local Policy Engine

After the receipt/evidence loop is proven, add deterministic local policies such as:

```yaml
agent:
  allowed_paths:
    - src/**
    - tests/**
  forbidden_paths:
    - infra/production/**
    - migrations/**
  required_validation:
    - dotnet build AgentsWatch.sln
    - dotnet test AgentsWatch.sln
  max_task_cost: 2.00
```

Initial policy capabilities should stay explainable and local:

- allowed/forbidden paths;
- required validation;
- maximum changed-file budget;
- command allow/deny/approval rules;
- model/provider allow list when model data is available;
- cost budget when provider cost data is available;
- explicit approval gates for risky operations.

Do not begin with a cloud policy service.

### 3. Verify — signature moat

The verification layer compares:

```text
intent
vs acceptance criteria
vs policy
vs actual diff
vs validation evidence
vs agent claims
```

Possible result:

```text
Run: AW-18391
Agent: Codex
Task: Implement login
Files changed: 3
Tests: 27 passed
Scope drift: none
Policy violations: none
Claim: "Implemented and tested"
Verdict: VERIFIED
```

Prefer explainable findings over a single opaque score. A later `Trust Score` may summarize deterministic inputs, but the underlying reasons must always be visible.

### 4. Learn and Economize

Use verified outcomes rather than raw token totals to compare execution routes.

Target metrics:

```text
Verified Task Rate
False Done Rate
Scope Drift Rate
Validation Pass Rate
Human Rework Rate
Average Cost per Verified Task
Average Time per Verified Task
Policy Violation Rate
Repeat Mistake Rate
Router Confidence
```

The important commercial metric is not `cost per request`; it is closer to:

```text
cost per verified completed task
```

## Optional AgentsWatch Gateway — later module

The Gateway is a future control/telemetry module, not the initial product.

Possible flow:

```text
Coding agent / internal AI app
          |
          v
AgentsWatch Gateway
  - identity / project
  - provider/model policy
  - usage + cost
  - rate/budget limits
  - PII/secret detection
  - routing/fallback
          |
          v
OpenAI / Anthropic / Gemini / Azure / local model
```

Potential capabilities:

- OpenAI-compatible proxy surface where practical;
- provider adapters;
- model, token, cost, latency and failure metadata;
- budgets by organization/project/user/agent;
- provider/model allow and deny policies;
- rate limits;
- retries and fallback;
- secret/PII detection before external model calls;
- optional redaction/blocking;
- BYOK (bring your own provider key);
- EU-hosted deployment option later;
- private/on-prem deployment only for validated enterprise demand.

## Gateway security principles

If Gateway work is activated later:

1. Customer provider keys should be BYOK by default.
2. Provider keys must never be stored in plaintext.
3. Full prompt/response retention must be OFF by default.
4. Default audit records should prefer metadata such as:

```text
RequestId
OrganizationId
ProjectId
AgentId
Provider
Model
Tokens
Cost
Latency
PII/secret categories
Policy decision
Timestamp
Content hash when useful
```

5. Full content retention, if ever supported, must be explicit, encrypted and retention-limited.
6. Tenant isolation must be treated as a security boundary, not a convenience field.
7. Gateway data must not weaken the current local-first privacy promise for users who do not enable cloud/team features.

## Compliance positioning guardrail

Do not initially market AgentsWatch as an `AI Act compliance platform` or promise legal compliance.

Safer product language:

```text
AI agent visibility, policy, evidence and control
```

Possible future compliance-support features may include audit export, retention policies and explainable policy history, but legal/compliance claims require separate validation and legal review.

## Team Server — future control plane

Only after local evidence has repeatable value, a Team Server may centralize selected metadata and receipts.

Possible shape:

```text
Developer repos / CI
      |
      +-- AgentsWatch CLI
      +-- Codex / Claude Code / Cursor / other agents
      |
      v
AgentsWatch Team Server
      |
      +-- Runs and receipts
      +-- Team policies
      +-- Cost metadata
      +-- Agent/model comparison
      +-- PR/CI evidence
      +-- Audit history
```

The Team Server should accept compact, configurable evidence. It must not require uploading full source code or full conversations to provide basic team value.

## Cross-agent analytics opportunity

With enough comparable runs, AgentsWatch can answer questions a simple gateway cannot:

| Metric | Meaning |
|---|---|
| Verified Task Rate | Share of runs whose claims are supported by scope and validation evidence. |
| False Done Rate | Agent claimed completion but evidence gate rejected it. |
| Scope Drift Rate | Runs touching unrelated/forbidden scope. |
| Human Rework Rate | Verified measure or explicit estimate of follow-up human correction. |
| Cost / Verified Task | Provider cost divided by verified outcomes, not requests. |
| Validation Efficiency | Useful validation relative to changed scope and elapsed command cost. |

Comparison must use equivalent task classes and label small-sample results as low confidence.

## Activation gates

### Gate A — core trust loop proven

Do not start Policy Engine runtime expansion until there is evidence that the core loop is useful.

Minimum evidence target/hypothesis:

- Gate 0 restore/build/test/CLI smoke is complete;
- Agent Run Receipt exists in runtime, not only docs;
- at least 30 useful dogfood receipts;
- at least 2 repositories dogfooded;
- at least one real scope-drift, false-Done or missing-validation issue caught;
- at least one user repeats the workflow voluntarily.

These are product decision thresholds, not claims that current runtime has met them.

### Gate B — local Policy Engine

Activate when Gate A is met and users want prevention/guardrails in addition to reporting.

Start only with deterministic local policies. No hosted control plane is required.

### Gate C — Team Server

Activate only after at least one small team wants shared receipts/policies/history and local-only workflow becomes a real coordination limitation.

### Gate D — Gateway feasibility

Start a Gateway spike only when centralized model usage/control is requested by real users or is needed to unlock a measured AgentsWatch metric such as cost per verified task.

Do not build a Gateway only because it is strategically interesting.

### Gate E — Enterprise/on-prem

SSO, SCIM, private networking, long retention, on-prem deployment, penetration testing and compliance exports require paying design-partner demand. They are not default roadmap work.

## Roadmap sequence

```text
Phase 0-4: validate/build the local CLI and evidence spine
Phase 5: local dashboard after dogfood threshold
Phase 6: PR/team workflow + deterministic local policies
Phase 7: Team Server only after team demand
Phase 8: optional AgentsWatch Gateway feasibility and MVP
Phase 9: enterprise controls/on-prem/compliance-support tooling
```

The Gateway must be removable from the sequence without breaking the core product thesis.

## Monetization hypothesis

Do not treat these prices as validated.

### Free / local

- CLI core;
- contracts and receipts;
- basic evidence/drift checks;
- local reports and handoffs.

### Pro

Possible range:

```text
EUR 15-25 / developer / month
```

Potential value:

- advanced local analytics;
- richer policy packs;
- cross-repo history;
- advanced validation economy;
- agent/model comparison on local evidence.

### Team

Possible experiment:

```text
EUR 49-99+ / month base, or per-seat pricing after validation
```

Potential value:

- shared receipts;
- GitHub/CI integration;
- team policies;
- audit history;
- verified-task analytics.

### Enterprise

Only after Gateway/private deployment demand:

```text
EUR 500-2,000+ / month is a hypothesis, not a published price
```

Potential value:

- Gateway;
- centralized provider policies;
- PII/secret controls;
- SSO/RBAC;
- retention controls;
- private/on-prem deployment;
- compliance-support exports.

## Explicit non-goals now

Do not add to the current MVP merely because this strategy exists:

- hosted Gateway;
- multi-tenant SaaS backend;
- billing;
- SAML/SCIM;
- Kubernetes;
- on-prem installer;
- local-model runtime;
- full AI Act classifier;
- generic LLM observability dashboard;
- generic prompt archive;
- autonomous payment handling for provider usage;
- opaque automatic model routing.

## Strategic summary

The long-term defensible loop is:

```text
Contract -> Observe -> Control -> Verify -> Learn -> better next contract
```

AgentsWatch should win by connecting execution evidence to actual verified engineering outcomes. Gateway telemetry can strengthen that loop later, but the verification layer remains the product center of gravity.
