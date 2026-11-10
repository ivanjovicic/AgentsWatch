# AW-ROUTE-RFC-001 — Task difficulty assessment and advisory model selection

Date: 2026-11-10 (Europe/Belgrade)  
Status: PROPOSAL / docs-only / NOT AUTHORIZED FOR RUNTIME IMPLEMENTATION  
Parent direction: [MVP_ROADMAP.md](MVP_ROADMAP.md) Phase 12 and [ROADMAP_DRIVEN_AGENT_OS.md](ROADMAP_DRIVEN_AGENT_OS.md) model/tool recommendation  
Related but separate: [AW-ORCH-RFC-001 PR #22](https://github.com/ivanjovicic/AgentsWatch/pull/22) (manual review packets and possible future supervised execution)

## Decision and rationale

**Feasible:** yes, a deterministic, explainable task *assessment* and *recommendation* is implementable without an LLM key or dispatch access. **Business/user benefit:** plausible but **UNPROVEN**; only comparative measurements can justify expansion. **Automatic provider/model assignment:** NOT approved. Recommend a capability class and a *conditional* available-model shortlist; the owner selects and launches an agent.

Why not build a generic router now: Cursor already offers an Auto/Cursor Router that classifies task type and complexity. Model catalogs, permissions, prices, access modes, provider identities and availability are volatile. Research on agentic code routing suggests learning from outcomes can help, but other evaluations find that routers may lose to simple baselines. AgentsWatch's possible edge is **verifiable cross-provider evidence and outcomes per *completed, accepted* task**, not a claim of globally knowing the best model.

Existing governance is unchanged: AW-VFY-010 = ADJUST/PAUSE (12/30 useful REAL as of 2026-11-07); AW-VFY-011/012 and broad orchestration remain HOLD. This RFC does not create/claim a task, activate B5, authorize an API call, or authorize merge/deploy. PR #21 executor-provenance correction and the cross-platform receipt/contract digest defect are separate trust-priority work.

## 1. Inputs: assess *before* work begins

Minimum authorized inputs: user-approved task text/acceptance criteria, stack/language, known repository/module metadata, paths *expected* to be touched, explicit constraints and test/build availability. Extra read-only, bounded context may come from an authorized repo/issue. Do not scrape private chats or fetch every file. No initial code change or full-repo scan is required.

Keep three axes separate:

1. **Difficulty** `LOW | MEDIUM | HIGH | UNKNOWN`: scope size and coupling, design ambiguity, novelty/unfamiliar stack, dependency chains, environment/test complexity, expected debugging/iteration. Missing task boundaries are `UNKNOWN`, not automatically `LOW`.
2. **Risk** `LOW | MEDIUM | HIGH | BLOCKED`: security, secrets, privacy, payment/auth, data loss, DB migrations, deploy/production permissions, irreversible operations, ownership/lease and operational blast radius. Reuse existing permission/risk rules. A one-line destructive SQL change may be technically EASY but HIGH RISK.
3. **Confidence/evidence** `HIGH | MEDIUM | LOW | UNKNOWN`: based on completeness and provenance of inputs. No invented file count, test existence, duration or price. Disagreement, stale sources and unknown repository state lower confidence.

Initial heuristic, strictly advisory:
- LOW: well-scoped documentation/tests or bounded code change, clear acceptance and validation path, low coupling, few unknowns.
- MEDIUM: bounded multiple-file feature/refactor, familiar dependencies, explicit interfaces/tests.
- HIGH: wide architectural changes, cross-system concurrency, difficult production diagnostics, security/auth boundary, complex data migration or high uncertainty with broad effects.
- UNKNOWN: insufficient context or conflicting expectations. Ask for missing details or select a conservative capability fallback; do not express numerical certainty without calibration.

**Rule:** risk can veto execution or require a stronger reviewer regardless of difficulty. The estimate is pre-execution and must not use the final diff as if it had been known at routing time. If a later plan changes scope, reassess and preserve both versions.

## 2. Recommendation, never a fabricated capability promise

Return a `TaskAssessment` with `assessmentVersion`, `sourceRef`, `sourceShaOrVersion`, `difficulty`, `risk`, `confidence`, `reasons[]`, `unknowns[]`, `requiredValidation[]` and `assessedAt`. Derived recommendation (not in canonical RunReceipt v1):
- `recommendedCapabilityClass`: `FAST_CODE | GENERAL_CODING | DEEP_REASONING_CODING | SPECIALIST_REVIEW | UNKNOWN`.
- `eligibleCandidates[]`: only *user-authorized and actually available* provider/model IDs; with provider, surface (Codex CLI vs Cursor IDE/Cloud vs API vs Claude web etc.), known supported capabilities, context/tool needs, dated pricing/availability provenance when confirmed, permission mode, limitations, and whether choice is **manual-only**.
- `primaryCandidate?` and `alternatives[]` only when evidence warrants; `fallback` if model not available, context too large, cost capped, unknown catalog or policy denies. Otherwise return `UNKNOWN` + capability advice.
- `reviewPlan`: independent reviewer capability/provider where possible, with actual provenance `DECLARED/UNVERIFIED` as applicable. Different brand is not proof of different model/provider, and a second AI is not a second human approval.
- `estimatedCost` and `estimatedDuration` default **Unknown**. Do not transform subscription limits or raw token prices into ungrounded per-task savings. Record user preference: quality-first vs cost/latency, max spend, allowed provider/model, local/cloud data boundaries.

Examples (not vendor-specific rankings): simple docs -> fast class; bounded multi-file .NET work -> general/deep coding depending on coupling; security/payment/DB incident -> deep reasoning + risk-specific review and owner approval; validation log classification -> lower-cost summarizer with deterministic checks.

**No hidden model switching, no automatic dispatch, no bearer/API key collection, no usage charges, no provider access outside user-approved permissions.** A deterministic rule recommendation can be given even when no provider connection is configured.

## 3. Distinguish existing provider routing from AgentsWatch

Current primary-source observations (verified 2026-11-10, subject to provider/version/plan drift):
- [Cursor Router](https://prod.cursor.com/help/models-and-usage/cursor-router) already routes by task type and complexity for supported Auto modes. Its availability and pool differ by plan; do not claim it works for all users or discloses all chosen models.
- [Cursor models and pricing](https://cursor.com/docs/models-and-pricing) vary by plan, routed model and billing mode.
- [OpenAI model selection](https://developers.openai.com/api/docs/guides/model-selection) advises choosing task-fit, reasoning and cost/quality experimentally; no universal cheapest model solves all code tasks.
- [OpenAI agent model configuration](https://developers.openai.com/api/docs/guides/agents/models) supports explicit per-agent/run model selection in suitable SDK/API workflows, not control of an unrelated consumer chat UI.
- [Claude Code settings](https://code.claude.com/docs/en/settings) allow model configuration in its **Code CLI**; do not confuse this with Claude **web**.
- Research examples (not validation of AgentsWatch): [Agent-as-a-Router](https://arxiv.org/abs/2606.22902), [Task-to-Model Optimization](https://arxiv.org/abs/2608.08528), and [Dynamic LLM Routers are Often Misguided](https://arxiv.org/abs/2610.02762).

Prefer **provider-neutral capability classes**, current eligible catalog, and task-specific real outcomes. Treat advertised vendor benchmark claims as hypotheses to test on own workloads.

## 4. Learning later from trustworthy completed-task evidence

Use a separate opt-in, privacy-minimized `RoutingOutcome` store derived from authorized tasks; never mutate historical RunReceipt, core evidence, acceptance or the AW-VFY-010 ledger. A source agent's success prose, `Done` claim or Git commit is **not** an accepted outcome. Record only evidence-backed, comparable signals:
- task type, difficulty/risk *as estimated before execution*, relevant stack and size/context bucket;
- exact model *when authentically known*, actual agent/provider/surface, user/manual versus router choice, reasoning settings, catalog version;
- exact SHA-bound build/tests, review findings, merged/accepted/rolled-back status and verified quality issues, known retries/corrections;
- elapsed hands-on and wall time, observed billable/API usage and additional review cost (or `Unknown`), user-approved quality rubric;
- limitations, missing data, date and provenance. No secrets or raw private prompts unless explicitly permitted.

Compare **total cost per independently accepted task**, not just input token price: retries, CI runs, corrections, latency, review time and failures matter. Do not infer a per-model win from unmatched tasks, different acceptance bars, leakage from test to training, or one successful run.

## 5. Staged plan and hard gates

**AW-ROUTE-000 — docs-only design (THIS RFC).** No app code, queue/router status, runtime, new REAL run, model adapter, billing or automation.

**AW-ROUTE-001 — optional read-only/manual advisor prototype (separate owner GO, after receipt/digest trust work):** deterministic rules + explainable difficulty/risk/confidence + user-configured candidate catalog; manual task card import/export; no new API required; no routing/dispatch. Can be a standalone tool or non-authoritative derived output, not a mandatory RunContract/RunReceipt field. No production side effects.

**AW-ROUTE-002 — prospective shadow evaluation (separate GO, after AW-VFY-011 external-value evidence or explicit limited owner-approved experiment that does NOT change Gate 5):** preregister at least ~20-30 varied real, eligible task cards if available; do not manufacture work to hit a number. Seal assessments before outcomes. Compare against (a) explicit user-chosen baseline and (b) a simple fixed capability/model baseline; optionally Cursor Auto where legitimately available. Predefine quality, accepted-outcome gate, median overhead and cost endpoints; blind adjudication and cross-platform proven receipts. The pilot is *not* counted as AW-VFY-010 REAL/decision-change evidence without its independent protocol.

Candidate **hypothesis thresholds** (must be approved and frozen BEFORE pilot, not presented as measured): demonstrable >=15% lower **observed total cost per accepted task** versus the manual baseline, no material quality/safety regressions, no increase >10% in median completion time, and <=2 min median additional routing/admin time. If cost isn't observable or task outcomes aren't comparable, return INCONCLUSIVE, not GO.

**AW-ROUTE-003 — empirical recommendation (conditional after 002 success):** learned project-local tier comparisons; calibration, uncertainty and fallback; small eligible roster, not an opaque universal leaderboard. Models/rates/catalog expire and require revalidation. Compare against simple heuristic before paying for routing inference.

**AW-ROUTE-004 — supervised/automatic assignment (LATER, separate explicit owner GO and coordination/lease/provider integration approval):** only for an allowlisted low-risk task subset with existing isolated worktree, exclusive ownership, evidence/permission checks, spend caps, explicit model availability, rollback/cancel and owner controls. No auto-merge, no production deploy and no high-risk delegation by a heuristic score. Human review stays required by repo policy.

If tasks repeatedly classify UNKNOWN, recommendations don't improve outcomes, setup exceeds benefit, or provider routers do as well for less overhead: **NARROW/STOP**. Do not expand into hosting, a paid router or UI dashboard to rescue an unvalidated claim.

## 6. Required tests once AW-ROUTE-001 is authorized

- Same task inputs and catalog -> deterministic reasons and output; stable across Windows/Linux.
- Easy/high-risk (tiny destructive DB command) is *not* auto-approved; high complexity/low operational risk remains distinct.
- Missing repo context/test availability -> UNKNOWN, not unjustified LOW.
- Unavailable/denied provider or model, mismatched capability and expired catalog -> excluded with reason.
- User override cannot be silently ignored; no auto-switch, executor call or billing in advisory mode.
- Unverified implementer identity and conflicting receipt metadata do not become verified model-performance samples.
- Stale exact-SHA and CI, duplicate tasks/retries, post-hoc difficulty labels or previous failures never masquerade as independent comparable success.
- No exposure of secrets/private task content; prompt/issue text is untrusted; no model recommendation can grant approval.
- Backward-compatible receipt/contract digest, no historical rewrites, no changed Gate 5 counts.
- Evaluate false-confidence, calibration and misrouting rates as well as measured ROI; test on held-out authorized tasks.

## 7. Decision

**Record as a conditional feature hypothesis, not an approved implementation or a commercial differentiator.** Next priority remains independent PR #21 review and cross-platform digest correction. If those stabilize and the owner wants a low-cost trial, start at AW-ROUTE-001 only. PR #22's ReviewPacket first path can consume the future **advisory** TaskAssessment without auto-dispatching or inventing evidence.

No changes to `AGENTS.md`, `PROMPT_QUEUE_ROUTER.md`, live queue, RunReceipt schema, CI, or production are authorized by this plan.
