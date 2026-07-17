# AgentsWatch Feature Portfolio Review — 2026-07-17

Status: competitive scope audit

## Executive verdict

The original local-first direction remains correct, but the market has moved.

Parallel agents, background execution, schedules, playbooks, knowledge, session analysis, approvals, run history, PR automation, cloud sandboxes and visual engineering workflows are already offered by established products.

AgentsWatch should not compete on those generic capabilities.

The differentiated first product is:

```text
local CLI
+ roadmap/run contract
+ vendor-neutral Agent Run Receipt
+ claims/diff/validation evidence gate
+ scope and roadmap drift detection
+ compact project-local learning
```

## Keep and elevate

These are now core rather than optional convenience features:

- task/run lifecycle;
- roadmap or prompt contract;
- git start/end evidence;
- Agent Run Receipt;
- validation evidence or blocked reason;
- claims-vs-diff-vs-validation checks;
- Scope Drift Score;
- explainable Evidence Score;
- compact handoff and next prompt;
- local privacy/no telemetry.

## Differentiate after the core spine

Add after the receipt and evidence gate work:

- roadmap status updated from actual receipt evidence;
- validation economy and avoidable-work estimates;
- counterfactual prompt/context/validation recommendations;
- mistake rules with evidence count, confidence and expiry;
- cross-agent normalized history;
- project-local empirical model/tool router;
- roadmap progress confidence.

## Parity features: integrate, do not rebuild

Treat these as adapters or future integrations:

- agent execution;
- parallel/background agents;
- cloud workspaces;
- schedules and recurring automations;
- full session timelines;
- generic playbooks and knowledge bases;
- GitHub issue-to-PR automation;
- generic code review;
- visual workflow orchestration;
- CI/CD, release, incident and infrastructure automation.

## Remove from early product scope

Do not prioritize:

- visual dashboard before receipt dogfood;
- SaaS/billing/cloud sync;
- team policy engine;
- remote mistake database;
- integration marketplace;
- automatic code editing;
- autonomous merge/release;
- exact token accounting without provider data;
- proprietary agent runtime;
- deep IDE extension.

## Strongest market-gap hypotheses

Validate these rather than assuming them:

1. Cross-vendor local Agent Run Receipt.
2. Roadmap Contract Compiler with fail-closed completeness checks.
3. Claims-vs-diff-vs-validation evidence gate.
4. Roadmap and owned-path drift detection.
5. Counterfactual learning with scoped, expiring rules.
6. Repository-local model/tool routing based on comparable outcomes.
7. Validation Economy that measures avoidable command and context work.

## Product gates

1. `AW-VAL-001` restore/build/test.
2. `AW-VAL-002` CLI smoke validation.
3. Run lifecycle.
4. Agent Run Receipt.
5. Evidence lint.
6. Contract compiler.
7. Evidence and drift gate.
8. Validation economy.
9. Counterfactual learning.
10. Empirical router.
11. Thin adapters/MCP.
12. Dashboard only after at least 30 useful receipts.

## Dogfood requirement

Use AgentsWatch and MathLearning to collect comparable runs.

Do not claim differentiation through token savings alone. Measure:

- scope drift;
- evidence completeness;
- validation breadth and duration;
- retries;
- repeated mistakes;
- whether accepted learning rules improve later comparable runs;
- whether model/tool recommendations become useful with enough evidence.

## Decision

AgentsWatch should become the evidence and learning layer between roadmap intent and external agent execution, not another agent platform.

See:

- `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`
- `docs/PRODUCT_SPEC.md`
- `docs/MVP_ROADMAP.md`
