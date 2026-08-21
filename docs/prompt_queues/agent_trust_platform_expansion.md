# AgentsWatch Trust Platform Expansion Queue

Last aligned: 2026-08-21  
Target repo: `ivanjovicic/AgentsWatch`  
Status: future-only strategic queue; no implementation row is currently claimable

Purpose: preserve the long-term Observe -> Control -> Verify -> Learn expansion without allowing it to outrun the local Agent Run Receipt MVP.

## Read first

- `../../AGENTS.md`
- `PROMPT_QUEUE_ROUTER.md`
- `../AGENT_TRUST_PLATFORM_EXPANSION_2026_08_21.md`
- `../PRODUCT_SPEC.md`
- `../ULTRA_ROADMAP.md`
- `../ROADMAP_VALIDATION_GATES.md`
- `../SECURITY_AND_PRIVACY.md`

## Hard rules

- Gate 0 validation remains first priority.
- Do not select this queue as the next runtime work while the current MVP/evidence spine is incomplete.
- Gateway is an optional future module, not a standalone pivot.
- Do not implement hosted multi-tenancy, billing, SSO/SCIM, on-prem, or Gateway runtime from this queue without the explicit activation gate being met.
- A docs-only feasibility prompt may refine strategy but must not claim runtime completion.
- Prefer deterministic local policy before cloud policy services.
- Compliance-support features must not be marketed as legal compliance.

## Activation summary

| Gate | Required evidence before runtime work |
|---|---|
| A — Core trust loop | Gate 0 complete, runtime Agent Run Receipt/evidence loop, meaningful dogfood, real issue caught, repeat user workflow. |
| B — Local Policy Engine | Gate A plus user demand for prevention/guardrails. |
| C — Team Server | A real team needs shared receipts/policies/history. |
| D — Gateway | Real users request centralized provider control, or provider telemetry is required for a validated metric such as cost per verified task. |
| E — Enterprise | Paying design-partner demand for identity, private networking, retention, on-prem, or compliance-support exports. |

Detailed thresholds and caveats live in `../AGENT_TRUST_PLATFORM_EXPANSION_2026_08_21.md`.

---

## Future prompts

| ID | Status | Run mode | Purpose |
|---|---|---|---|
| AW-TRUST-001 | Blocked — requires Gate A | docs/spec | Define deterministic local Policy Engine contracts from proven receipt data. |
| AW-TRUST-002 | Blocked — requires AW-TRUST-001 + Gate B | implementation | Implement minimal local path/validation/command policy checks. |
| AW-TRUST-003 | Blocked — requires comparable real runs | docs/spec | Define verified-task analytics and confidence rules. |
| AW-TEAM-001 | Blocked — requires Gate C | investigation-only | Validate Team Server problem, privacy boundary, and minimal shared metadata. |
| AW-TEAM-002 | Blocked — requires AW-TEAM-001 decision | docs/spec | Define Team Server API/data isolation contracts without implementation. |
| AW-GATEWAY-001 | Blocked — requires Gate D | investigation-only | Run Gateway feasibility spike and build-vs-integrate decision. |
| AW-GATEWAY-002 | Blocked — requires AW-GATEWAY-001 go decision | docs/spec | Define Gateway threat model, BYOK, tenant isolation, retention, and provider adapter contracts. |
| AW-GATEWAY-003 | Blocked — requires AW-GATEWAY-002 + explicit runtime approval | implementation | Implement smallest OpenAI-compatible/provider-proxy spike with metadata-only audit. |
| AW-GATEWAY-004 | Blocked — requires validated Gateway use | implementation | Add cost budgets, provider/model policy, retry/fallback and PII/secret actions. |
| AW-ENTERPRISE-001 | Blocked — requires Gate E | investigation-only | Validate enterprise identity/private deployment/compliance-support requirements with a design partner. |

---

## AW-TRUST-001 — Local Policy Engine contract

Run mode: docs/spec  
Token budget: medium  
Activation: Gate A only

Goal: define the smallest deterministic policy schema that uses existing contract/receipt evidence.

Expected scope:

```text
allowed_paths
forbidden_paths
required_validation
max_changed_files
risky_command_approval
optional provider/model allow-list fields only if metadata is already available
```

Do not design a hosted policy service.

Expected evidence:

- policy schema examples;
- mapping from each policy to existing receipt evidence;
- explainable failure output;
- migration/compatibility rule;
- explicit non-goals.

---

## AW-TRUST-002 — Minimal local Policy Engine runtime

Run mode: implementation  
Token budget: medium  
Activation: AW-TRUST-001 accepted and Gate B met

Goal: implement the smallest local deterministic policy checks against existing run evidence.

Do not add networking, accounts, cloud storage, or provider proxies.

Validation must include tests proving:

- allowed paths pass;
- forbidden paths fail;
- missing required validation fails;
- risky command policy is explainable;
- existing local-first workflows remain unchanged when policies are disabled.

---

## AW-TRUST-003 — Verified-task analytics contract

Run mode: docs/spec  
Token budget: medium  
Activation: enough comparable real runs exist

Define metrics such as:

```text
Verified Task Rate
False Done Rate
Scope Drift Rate
Validation Pass Rate
Human Rework Rate
Cost per Verified Task
Time per Verified Task
Policy Violation Rate
```

Require equivalent task classes, sample-size labels, confidence, and `unknown` when data is insufficient.

Do not rank agents from incomparable or tiny samples.

---

## AW-TEAM-001 — Team Server problem validation

Run mode: investigation-only  
Token budget: medium  
Activation: Gate C

Answer before architecture work:

- what cannot be solved locally;
- which receipt fields teams actually need shared;
- whether GitHub/CI integration is enough without a server;
- which data must never be uploaded by default;
- tenant/isolation requirements;
- minimum viable team workflow.

Stop if there is no real coordination problem.

---

## AW-GATEWAY-001 — Gateway feasibility spike

Run mode: investigation-only  
Token budget: high  
Activation: Gate D

Goal: decide whether AgentsWatch should build, integrate, or avoid a Gateway.

Compare at least:

- using existing gateway/observability components;
- thin provider adapters only;
- OpenAI-compatible proxy;
- no Gateway, provider metadata import only.

Decision criteria:

- does it materially improve verified-task metrics;
- does it unlock demanded policy/control;
- security burden;
- operational burden;
- differentiation versus generic gateways;
- local-first compatibility.

The valid outcome may be `do not build`.

---

## AW-GATEWAY-002 — Gateway security and data contracts

Run mode: docs/spec  
Token budget: high  
Activation: AW-GATEWAY-001 explicit go decision

Must define before runtime code:

- BYOK handling;
- encryption/key rotation boundary;
- organization/project/agent identity;
- tenant isolation;
- metadata-only default audit record;
- prompt/response retention OFF by default;
- PII/secret categories and actions;
- provider failure/retry/fallback semantics;
- deletion/retention behavior;
- threat model and abuse cases.

No plaintext provider keys. No hidden prompt retention.

---

## AW-GATEWAY-003 — Minimal Gateway runtime spike

Run mode: implementation  
Token budget: high  
Activation: AW-GATEWAY-002 complete plus explicit runtime approval

Goal: smallest useful proxy spike.

Prefer:

- one compatible API surface;
- one provider first;
- metadata-only audit;
- BYOK;
- no billing;
- no full content retention;
- no enterprise identity layer.

Success is measured by reliable integration with the AgentsWatch receipt/verification loop, not request throughput alone.

---

## AW-GATEWAY-004 — Cost, policy and privacy controls

Run mode: implementation  
Token budget: high  
Activation: Gateway spike proves product value

Potential increments, split into separate implementation prompts when activated:

- cost metadata and budgets;
- provider/model allow/deny;
- rate limiting;
- retry/fallback;
- PII/secret redact/block;
- cost-per-verified-task feed into empirical router.

Do not combine all of these in one runtime change.

---

## AW-ENTERPRISE-001 — Enterprise demand validation

Run mode: investigation-only  
Token budget: medium  
Activation: Gate E

Validate concrete paid requirements before implementation:

- SSO/RBAC/SCIM;
- private networking;
- retention duration;
- audit export;
- EU-only hosting;
- on-prem/private deployment;
- penetration/security requirements;
- compliance-support reporting.

Do not infer enterprise requirements from general market trends. Require design-partner evidence.
