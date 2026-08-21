# AgentsWatch Positioning and Pricing Hypotheses

Last aligned: 2026-08-21  
Status: hypothesis, not validated

## Positioning

AgentsWatch is a local-first, vendor-neutral trust, control, and evidence layer for AI coding agents.

Primary line:

```text
Control what AI agents can do. Verify what they actually did.
```

Supporting line:

```text
Turn roadmap intent into verified change — across any coding agent.
```

Differentiation shorthand:

```text
AgentsWatch verifies the work, not just the traffic.
```

Current category:

```text
AI coding-agent trust and evidence layer
```

Long-term category hypothesis:

```text
Trust and control layer for autonomous coding agents
```

## Primary value

AgentsWatch helps developers:

- split broad prompts into safer runs;
- compile bounded execution contracts;
- limit what agents inspect and edit;
- track what actually changed;
- record validation evidence;
- catch claims-vs-actual mismatches;
- detect scope drift and unsupported `Done` claims;
- generate short handoff summaries;
- review only changed files;
- learn which execution route produces better verified outcomes.

For teams later, the higher-value questions become:

- which agent/model has the highest Verified Task Rate;
- which agent produces the lowest False Done Rate;
- how often scope drift occurs;
- how much human rework follows agent completion;
- what one verified completed task costs;
- whether team policies are being violated.

## What AgentsWatch is not

Do not position the product primarily as:

- another coding agent;
- generic agent runtime;
- generic LLM observability dashboard;
- standalone API gateway;
- generic prompt archive;
- AI Act compliance product.

A future Gateway may strengthen AgentsWatch, but verification remains the center of gravity.

## Free local CLI hypothesis

Likely free/open features:

- init;
- status;
- prompt/contract optimizer;
- task splitter;
- Agent Run Receipt basics;
- basic evidence and scope-drift checks;
- markdown run reports;
- handoff summaries;
- diff-only review prompts;
- basic risk scoring.

Goal: adoption, trust, and enough real receipts to validate the product loop.

## Pro hypothesis

Possible paid local/pro features later:

- local dashboard;
- cross-repo history;
- advanced token/validation waste analytics;
- richer policy packs;
- advanced evidence/verification views;
- exportable reports;
- saved templates;
- local agent/model comparisons where evidence is sufficient.

Pricing hypothesis only:

```text
Solo Pro: EUR 15-25 / developer / month
```

Do not validate pricing before CLI dogfood evidence exists.

## Team hypothesis

Possible team features later:

- GitHub/CI integration;
- shared policy rules;
- PR risk/evidence reports;
- shared Agent Run Receipts;
- team audit history;
- verified-task analytics;
- agent/model comparison by equivalent task class.

Pricing experiment only:

```text
Team: EUR 49-99+ / month base, or per-seat after validation
```

Do not lock the pricing model before observing whether value scales more with seats, repositories, verified runs, or managed policies.

## Enterprise hypothesis

Only after real demand for centralized provider controls, private deployment, or enterprise identity/security requirements.

Possible value:

- optional AgentsWatch Gateway;
- provider/model policies;
- BYOK management;
- cost budgets and rate limits;
- PII/secret detection controls;
- SSO/RBAC/SCIM;
- retention controls;
- EU-hosting/private/on-prem deployment;
- compliance-support audit exports.

Pricing hypothesis only:

```text
Enterprise: EUR 500-2,000+ / month depending on deployment and security requirements
```

This must not be published as validated pricing.

## Gateway commercialization rule

Do not create a separate generic Gateway product by default.

Gateway work is justified only when:

- real users request centralized model usage/control; or
- provider telemetry is required to improve a validated AgentsWatch metric such as cost per verified task.

The preferred commercial story is:

```text
Free/local evidence -> Pro analytics/policies -> Team shared trust layer -> optional Enterprise Gateway/control plane
```

## Do not claim yet

Do not claim:

- exact token savings without measurement;
- support for all languages;
- security certifications;
- production-ready PR integration;
- SaaS availability;
- reliable cross-agent superiority without comparable task data;
- legal or AI Act compliance;
- enterprise-grade Gateway security before dedicated hardening evidence exists.

## Evidence needed before selling

Minimum early proof:

- at least 5 real run reports/receipts;
- at least 2 repos dogfooded;
- at least 1 missed-test, false-Done, or scope-creep issue caught;
- at least 2 handoff summaries reused;
- at least 1 user willing to repeat the workflow.

Before local dashboard/policy expansion, stronger target/hypothesis:

- at least 30 useful dogfood receipts;
- repeat use of the receipt/evidence loop;
- at least one user explicitly asking for prevention/guardrails rather than only reporting.

Before Team Server:

- at least one small team has a real shared-history/policy problem.

Before Gateway:

- centralized model usage/control is explicitly requested by real users, or reliable provider telemetry is necessary for a validated AgentsWatch outcome metric.

See `docs/AGENT_TRUST_PLATFORM_EXPANSION_2026_08_21.md` for the full activation-gate strategy.
