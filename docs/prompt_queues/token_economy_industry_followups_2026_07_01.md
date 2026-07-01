# Token Economy Industry Follow-ups — 2026-07-01

Target repo: `ivanjovicic/AgentsWatch`  
Lane: advanced token/context economy  
Status: prompt-ready docs/spec queue

## Read first

- `../TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md`
- `../TOKEN_ECONOMY_PREVIOUS_CONVERSATION_BACKFILL_2026_07_01.md`
- `../CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md`
- `../CONTEXT_PACKS.md`
- `../TOKEN_WASTE_METRICS.md`
- `../PROMPT_TOKEN_ECONOMY_RULEBOOK.md`

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-TOKEN-IND-001 | Done-docs-created | Add context-pack spec and pack registry. See `docs/CONTEXT_PACKS.md`. |
| AW-TOKEN-IND-002 | Prompt-ready after 001 | Add cache-aware prompt skeleton and cache-breaker linter spec. |
| AW-TOKEN-IND-003 | Prompt-ready after 001 | Add AGENTS/CLAUDE/GEMINI configuration smell checklist. |
| AW-TOKEN-IND-004 | Prompt-ready after 001 | Add stale-context guard and context-rot check plan. |
| AW-TOKEN-IND-005 | Prompt-ready after 002 | Add queue-row token-budget fields and migration prompt. |
| AW-TOKEN-IND-006 | Prompt-ready after 001-005 | Add dogfood measurement matrix and cost-per-solved-task metric. |
| AW-TOKEN-IND-007 | Gate 0 required | Add future CLI command contracts for context plan/map/budget/stale-check/tokens report. |
| AW-TOKEN-IND-008 | Gate 0 required | Add rules-lint command contract for duplicate/conflicting/bloated agent instructions. |
| AW-TOKEN-IND-009 | Gate 0 required | Add evidence compaction command contract with source-pointer preservation. |
| AW-TOKEN-IND-010 | Gate 0 required | Add provider-aware cache profile config contract. |
| AW-TOKEN-IND-011 | Prompt-ready | Add state-ownership token filter guide and examples. |
| AW-TOKEN-IND-012 | Prompt-ready | Add feature-profile context gating guide. |
| AW-TOKEN-IND-013 | Prompt-ready | Add queue lifecycle token report template for `next/run/finish/report`. |
| AW-TOKEN-IND-014 | Prompt-ready | Add domain playbook template for zero-waste risky changes. |
| AW-TOKEN-IND-015 | Prompt-ready | Add batch-review compaction checklist. |

---

## AW-TOKEN-IND-001 — Context-pack registry

Status: Done-docs-created  
Run mode: docs/spec  
Token budget: low

Created: `docs/CONTEXT_PACKS.md`

Follow-up only if needed: dogfood the packs on one Flutter, one backend, and one AgentsWatch prompt.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-002 — Cache-aware prompt skeleton

Run mode: docs/prompt  
Token budget: low

Create `docs/ai/prompts/CACHE_AWARE_PROMPT_SKELETON.md`.

Must include:

- stable prefix;
- cache-safe section;
- selected context pack;
- validation contract;
- variable suffix;
- cache breaker examples;
- OpenAI/Anthropic/Gemini notes without hardcoding provider-specific behavior as universal.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-003 — Agent configuration smell checklist

Run mode: docs/quality  
Token budget: low

Create `docs/AGENT_CONFIG_SMELL_CHECKLIST.md`.

Detect:

- context bloat;
- conflicting instructions;
- skill leakage;
- lint leakage;
- stale path references;
- stale command examples;
- duplicate rules;
- always-loaded rules that should be path-scoped;
- docs-only rules that imply runtime proof.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-004 — Stale-context guard

Run mode: docs/evidence  
Token budget: low

Create or update `docs/STALE_CONTEXT_GUARD.md`.

Must define:

- `fresh`;
- `recent`;
- `stale`;
- `unknown-freshness`;
- runtime Done blocker;
- current-code-wins rule;
- stale-context run-log fields.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-005 — Queue row token-budget fields

Run mode: docs/migration-plan  
Token budget: medium

Add a migration plan for prompt queues to include:

```text
Token budget: XS/S/M/L
Context pack: <pack>
Max files before expansion: <n>
Expected output mode: brief-done/review-table/full-analysis
Cache profile: static-prefix yes/no
State owner: <owner or not-applicable>
Feature profile: <profile or not-applicable>
```

Do not mass-update every queue in this prompt. Create the plan and one example row only.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-006 — Dogfood measurement matrix

Run mode: docs/measurement  
Token budget: low

Create `docs/TOKEN_ECONOMY_DOGFOOD_MATRIX_2026_07_01.md`.

Define:

- 3 Flutter runs;
- 3 backend runs;
- 3 AgentsWatch runs;
- baseline prompt style;
- improved prompt style;
- metrics;
- validation;
- failure cases;
- public-claim threshold.

Metric must be `cost per validated completed prompt`, not only raw token count.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-007 — Context CLI contracts

Run mode: docs/product-contract  
Token budget: medium  
Gate: run only after Gate 0 validation

Add future command contracts for:

```text
agentswatch context plan
agentswatch context pack
agentswatch context map
agentswatch context expand
agentswatch context stale-check
agentswatch context budget
agentswatch tokens report
```

No runtime implementation.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-008 — Rules lint command contract

Run mode: docs/product-contract  
Token budget: medium  
Gate: run only after Gate 0 validation

Define future `agentswatch rules lint` behavior.

It should scan AGENTS.md, CLAUDE.md, GEMINI.md, .cursorrules, .windsurfrules, queue files, and shared standards for bloat, conflict, stale references, and duplicated instructions.

No runtime implementation.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-009 — Evidence compaction command contract

Run mode: docs/product-contract  
Token budget: medium  
Gate: run only after Gate 0 validation

Define future `agentswatch evidence compact` behavior.

Must preserve:

- source log paths;
- commit SHAs;
- tests/validation pointers;
- residual risks;
- mistake IDs;
- summarized-by links.

No runtime implementation.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-010 — Provider-aware cache profiles

Run mode: docs/product-contract  
Token budget: medium  
Gate: run only after Gate 0 validation

Define config shape for provider-aware cache profiles:

- OpenAI;
- Anthropic;
- Gemini;
- local/no-provider-cache.

Rules:

- record exposed cached-token metrics where available;
- never invent cache savings;
- warn if dynamic content appears before cacheable prefix;
- keep provider-specific details out of generic prompt rules.

No runtime implementation.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-011 — State-ownership token filter guide

Run mode: docs/spec  
Token budget: low

Create `docs/STATE_OWNERSHIP_TOKEN_FILTER.md`.

Must include:

- owner categories: backend, local-cache, display-only, config, filesystem, git, external-service;
- examples from MathLearning economy/rewards and AgentsWatch config/reporting;
- wrong-layer read examples;
- run-log fields;
- Done blockers for unknown owner in risky prompts.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-012 — Feature-profile context gating guide

Run mode: docs/spec  
Token budget: low

Create `docs/FEATURE_PROFILE_CONTEXT_GATING.md`.

Must include:

- feature packages from previous feature-selection planning;
- which docs are allowed by profile;
- docs to avoid when profile is core/local MVP;
- warning examples for dashboard/team/cloud leakage;
- relation to `pack.feature-profile.gating`.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-013 — Queue lifecycle token report template

Run mode: docs/template  
Token budget: low

Create `docs/QUEUE_LIFECYCLE_TOKEN_REPORT.md`.

Must cover:

```text
agentwatch next
agentwatch run
agentwatch finish
agentwatch report
```

Until CLI exists, define the run-log equivalent fields:

- selected pack;
- token budget;
- files opened before expansion;
- negative cache;
- validation;
- cost/waste summary.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-014 — Domain playbook template for zero-waste risky changes

Run mode: docs/template  
Token budget: low

Create `docs/ZERO_WASTE_DOMAIN_PLAYBOOK_TEMPLATE.md`.

Must generalize the earlier Access/ERP cutoff pattern:

```text
clone/report layer first -> validate counts -> preserve base semantics -> change defaults only after proof
```

Include examples for:

- reporting/query layer;
- backend settlement/idempotency;
- UI display-only polish;
- config/feature profile changes.

Validation: `git diff --check`.

---

## AW-TOKEN-IND-015 — Batch-review compaction checklist

Run mode: docs/evidence  
Token budget: low

Create `docs/BATCH_REVIEW_COMPACTION_CHECKLIST.md`.

Must define how a 3-5 commit batch review also creates a compact rollup:

- changed docs/queues;
- broken refs;
- stale statuses;
- duplicated/conflicting rules;
- evidence gaps;
- summarized-by links;
- next prompt list.

Validation: `git diff --check`.
