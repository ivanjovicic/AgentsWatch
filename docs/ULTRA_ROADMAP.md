# AgentsWatch Ultra Roadmap

Last aligned: 2026-08-21  
Status: strategic roadmap  
Scope: local CLI first, local dashboard second, policy/team after proof, optional Gateway later

## North Star

AgentsWatch helps developers and teams control AI coding-agent work and verify actual engineering outcomes.

Core promise:

```text
Control what AI agents can do. Verify what they actually did.
```

Supporting promise:

```text
Turn roadmap intent into verified change — across any coding agent.
```

Primary wedge:

```text
Vendor-neutral coding-agent trust and evidence layer.
```

The original token-economy objective remains important, but savings must be measured against verified outcomes rather than raw request counts.

## Product principles

1. Local-first before cloud.
2. CLI before dashboard.
3. Git evidence before AI interpretation.
4. Markdown prompts before LLM integrations.
5. Heuristic and explainable before smart and opaque.
6. Diff-only review before whole-repo review.
7. Handoff summaries before long chat history.
8. Validation evidence before `Done`.
9. One run mode per task: investigate, implement, test, review, docs.
10. Token budget and scope limiter on every non-trivial run.
11. Verify engineering outcomes before optimizing provider traffic.
12. Deterministic local policy before hosted governance.
13. Gateway only when real demand or measured product value justifies it.

---

## Phase 0 — Bootstrap validation

Goal: prove the generated skeleton is buildable and safe to extend.

Status: required before runtime feature expansion.

Must pass:

- restore/build/test;
- CLI smoke;
- validation evidence review;
- risk register update.

Key docs:

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/prompt_queues/bootstrap_validation.md`

Exit criteria:

- `dotnet restore AgentsWatch.sln` is verified;
- `dotnet build AgentsWatch.sln` is verified;
- `dotnet test AgentsWatch.sln` is verified;
- `agentswatch --help`, `--version`, `optimize`, and `status` are smoke-tested;
- any build/smoke failures become narrow follow-up prompts.

---

## Phase 1 — CLI foundation

Goal: make AgentsWatch useful locally without external services.

Milestones:

1. `agentswatch init` is safe and idempotent.
2. `agentswatch status` detects project type and git state reliably.
3. `agentswatch optimize` creates useful prompt-risk output.
4. `agentswatch task split` writes markdown task files.
5. `agentswatch start` records a run start snapshot.
6. `agentswatch finish` records changed files and validation notes.
7. `agentswatch report` writes markdown reports.
8. `agentswatch handoff` writes compact summaries.
9. `agentswatch review-diff` creates diff-only review prompts.

Definition of done:

- works on a sample .NET repo;
- works on a sample Flutter repo;
- no cloud dependency;
- reports are markdown-first;
- no user files overwritten;
- all commands have tests or smoke evidence.

---

## Phase 2 — Contract and token-economy MVP

Goal: turn vague work into bounded execution and reduce avoidable agent waste.

Features:

- roadmap/prompt contract compiler;
- risk checker for raw prompts;
- prompt splitter;
- token budget levels;
- scope limiter templates;
- investigation-only prompt generator;
- implementation prompt generator;
- test prompt generator;
- diff-only review prompt generator;
- handoff summary generator;
- token waste report.

Output examples:

```text
Risk: High
Budget: low
Waste causes:
- broad scope
- missing stop rules
- multiple task modes
Suggested split:
- 001-investigate-only.md
- 002-implement-minimal-fix.md
- 003-add-tests.md
- 004-diff-only-review.md
```

Success metric:

- user can convert one rough prompt into scoped, machine-checkable work in under one minute;
- next agent run reads a handoff instead of a long chat;
- review prompt is limited to changed files.

---

## Phase 3 — Agent Run Receipt, evidence and verification

Goal: make every AI-agent run auditable and make completion evidence-based.

Features:

- start/end git snapshots;
- changed file classification;
- validation evidence fields;
- missed-test detection heuristic;
- claimed-vs-actual diff check;
- Agent Run Receipt;
- risk report per run;
- scope drift findings;
- explicit `VERIFIED`, `UNVERIFIED`, or equivalent explainable verdict language only when supported by deterministic evidence.

Risk signals:

- tests claimed but no test files changed;
- backend claimed but only frontend changed;
- docs claimed but no docs changed;
- validation claimed but no validation evidence recorded;
- high-risk files touched;
- too many files changed for budget;
- no handoff after long run.

Success metric:

- every run leaves a short receipt that a developer can review without reading the full chat;
- at least one real scope-drift, false-Done, or missing-validation issue is caught during dogfood.

---

## Phase 4 — Language adapters and empirical learning

Goal: useful validation suggestions across common stacks and repository-local learning from real outcomes.

Adapter order:

1. universal git/files adapter;
2. .NET adapter;
3. Flutter adapter;
4. React/TypeScript adapter;
5. Node adapter;
6. Python adapter;
7. later: Java, Go, Rust.

Adapter responsibilities:

- detect project type;
- suggest validation commands;
- identify high-risk files;
- identify likely test paths;
- avoid running broad commands unless configured.

Learning capabilities:

- repeat-mistake detection;
- validation economy;
- counterfactual prompt suggestions;
- comparable run history;
- early project-local model/tool recommendations when evidence is sufficient.

Non-goal:

- deep static analysis in MVP.

---

## Phase 5 — Local dashboard

Goal: make local run history easy to inspect.

Prerequisite: CLI has real dogfood usage.

Activation target/hypothesis:

- at least 30 useful dogfood receipts;
- at least 2 repositories dogfooded;
- core receipt/evidence loop is repeatedly used.

Suggested stack:

- local .NET API;
- React dashboard;
- SQLite storage;
- optional file watcher.

Pages:

- Runs;
- Tasks;
- Changed files;
- Risk findings;
- Evidence and verdicts;
- Token/cost metadata when available;
- Validation;
- Handoffs;
- Settings.

Dashboard must remain local-first. No cloud account required.

---

## Phase 6 — PR/team workflow and local Policy Engine

Goal: move from observing agent work to deterministic local control without requiring SaaS.

Features:

- GitHub PR diff ingestion;
- PR risk report;
- CI status ingestion;
- missing-test warnings;
- exportable markdown report;
- optional PR comment draft;
- allowed/forbidden path policies;
- required-validation policies;
- changed-file limits;
- risky-command approval gates;
- model/provider allow lists only when reliable metadata exists;
- cost budget rules only when provider cost data is available.

Policy principle:

```text
Explainable deterministic rule first; no opaque governance score.
```

Activation gate:

- Phase 3 receipt/verification is runtime-proven;
- users ask for prevention/guardrails, not only reporting.

---

## Phase 7 — Team Server and commercial team product

Goal: centralize selected receipts, policies and verified-task analytics for teams only when local-only coordination becomes limiting.

Possible features:

- accounts and organizations;
- selected shared run receipts;
- shared team policies;
- GitHub/CI integration;
- team audit history;
- agent/model comparison by equivalent task class;
- false-Done rate;
- scope-drift rate;
- human rework rate when measurable;
- cost/time per verified task when provider metadata is available.

Privacy rule:

- do not require full source code or full conversation upload for baseline team value;
- upload/sync remains explicit and configurable.

Activation gate:

- at least one real small team requests shared history/policy/analytics;
- local-only workflow is a demonstrated coordination limitation.

---

## Phase 8 — Optional AgentsWatch Gateway

Goal: add centralized AI-provider control and telemetry only if it strengthens the verified-outcome loop.

This is not a standalone product pivot and not a prerequisite for AgentsWatch success.

Possible capabilities:

- OpenAI-compatible proxy surface where practical;
- OpenAI/Anthropic/Gemini/Azure/provider adapters;
- model/token/cost/latency/failure metadata;
- BYOK provider keys;
- budgets and rate limits;
- model/provider policy;
- PII/secret detection and optional redact/block actions;
- retry/fallback;
- routing suggestions tied to verified task outcomes.

Security requirements before production use:

- encrypted secrets; never plaintext provider keys;
- tenant isolation as a hard security boundary;
- full prompt/response retention off by default;
- compact metadata audit by default;
- explicit retention controls.

Activation gate:

- centralized model usage/control is requested by real users; or
- provider telemetry is required to measure a validated product metric such as cost per verified task.

Do not build Gateway because generic LLM observability is fashionable.

---

## Phase 9 — Enterprise control plane

Only after paying design-partner demand.

Possible features:

- SSO/RBAC;
- SCIM;
- private networking;
- long-retention controls;
- private/on-prem deployment;
- penetration/security validation;
- compliance-support audit exports;
- EU-hosting options.

Do not market AgentsWatch as an `AI Act compliance platform` without separate legal/product validation. Compliance-support tooling is not the same as guaranteeing compliance.

---

## Monetization hypothesis

Free/local:

- CLI core;
- contracts and receipts;
- basic evidence/drift checks;
- markdown reports;
- prompt optimizer;
- local handoffs.

Paid Pro hypothesis:

```text
EUR 15-25 / developer / month
```

Potential value:

- advanced local dashboard;
- richer policy packs;
- cross-repo history;
- validation economy;
- local agent/model comparisons.

Team hypothesis:

```text
EUR 49-99+ / month base, or per-seat after validation
```

Potential value:

- GitHub/CI integration;
- shared policies;
- team audit history;
- verified-task analytics.

Enterprise hypothesis:

```text
EUR 500-2,000+ / month depending on Gateway/private deployment requirements
```

This is not a published or validated price.

---

## Roadmap guardrails

Do not build:

- SaaS before CLI value is proven;
- dashboard before run reports are useful;
- automatic code editing in v1;
- opaque AI scoring without transparent reasons;
- token claims without visible waste metrics;
- provider-specific integration before markdown workflow works;
- hosted Gateway before verified-outcome demand exists;
- multi-tenant cloud backend merely because it appears in the strategic roadmap;
- SAML/SCIM/on-prem/Kubernetes before paying enterprise demand;
- generic LLM observability as a substitute for verification.

---

## Strategic success checkpoint

AgentsWatch is ready for broader testing when:

- a developer can initialize a repo;
- compile a bounded contract;
- optimize/split a rough prompt;
- run an AI agent manually;
- record changed files and validation evidence;
- generate an Agent Run Receipt;
- detect unsupported completion claims or scope drift;
- generate a handoff;
- review a diff-only prompt;
- repeat the workflow on a second repo.

The next strategic expansion after that is local policy, not automatically Gateway/SaaS.

See `docs/AGENT_TRUST_PLATFORM_EXPANSION_2026_08_21.md` for activation gates and Gateway security/privacy rules.
