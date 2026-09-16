# AgentsWatch Deep-Dive Decisions — 2026-09-16

Status: **accepted strategy/implementation guardrails derived from the September 2026 competitive deep dive**.

Canonical research evidence:
`docs/research/agentswatch_deep_dive_2026_09/`

## D1 — Narrow the product wedge

AgentsWatch is not differentiated enough as a generic "independent AI code reviewer" or generic governance product.

Qodo, GitHub, Cursor and adjacent products already cover substantial review, session/audit, policy and AI-code-tracking surface.

The surviving thesis is narrower:

> **vendor-neutral run evidence and completion verification for delegated coding-agent work**

Primary loop:

```text
existing task/issue/prompt
  -> verification RunContract
  -> pre-run repository baseline
  -> external coding agent
  -> run-interval repository delta
  -> required validation/evidence
  -> deterministic scope/claim/completion checks
  -> portable RunReceipt
```

Generic code review is an input/integration surface, not the product core.

## D2 — Run-interval delta is not guaranteed causal authorship

Git snapshots can prove that repository state changed between the recorded start and finish of a run.

They cannot always prove that the selected AI agent caused every byte when humans, formatters, generators, hooks or another agent can write concurrently.

Therefore implementation and wording must distinguish:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Use `agent-authored` only when executor instrumentation genuinely proves authorship.

Ambiguity is an explicit result, not an error to guess away.

## D3 — RunContract is a verification normalization layer

Do not create a competing task-management object.

Default behavior should import/normalize an existing GitHub/Jira/Linear issue, prompt or roadmap item and ask only for missing verification-specific fields:

- acceptance criteria;
- owned paths;
- avoid paths;
- validation requirements;
- expected evidence;
- stop/approval rules.

## D4 — RunReceipt is a compact evidence artifact

The canonical receipt should preserve only durable evidence needed by developer/reviewer/CI/audit consumers:

- schema/run/contract/task identity;
- executor metadata when known;
- start/end repository evidence;
- run-interval delta and ambiguities;
- validations and provenance;
- structured claims and support status;
- acceptance/scope/risk findings;
- decision and reasons;
- auditable override data when applicable.

Do **not** make `learningNote`, `nextPrompt`, routing advice or verbose session narrative mandatory canonical receipt fields.

Those belong in optional human handoff / learning projections downstream of trustworthy receipts.

## D5 — Deterministic facts before semantic judgment

AgentsWatch must not become another AI saying "trust me, the other AI was wrong".

Deterministic/core:
- repository state/fingerprints;
- path/scope rules;
- validation status/provenance;
- narrow claim classes;
- hashes/evidence references.

AI-assisted only:
- free-text claim extraction;
- semantic acceptance analysis;
- explanations;
- suggested next steps.

AI output must never silently upgrade evidence from Unknown/NeedsReview to Supported/Done.

## D6 — Initial commercial ICP

First commercial validation target:

**AI-heavy engineering teams of roughly 5–30 developers**, especially teams using more than one coding-agent/tool and still doing meaningful human review.

Solo developers remain useful OSS users/dogfood participants, but are not assumed to be the strongest payer.

Regulated enterprise is a later high-ARPA expansion, not the first sales motion.

## D7 — Open-core is the leading packaging hypothesis

A verification product benefits from inspectable deterministic logic.

Leading hypothesis:

Open-source/local:
- schemas;
- CLI;
- Git evidence model;
- deterministic verification rules;
- basic receipt verifier/projection;
- basic GitHub check/action after the core is reliable.

Potential paid layer later:
- organization policies;
- central evidence retention/search;
- managed/signed attestations;
- cross-repository controls;
- team analytics;
- RBAC/SSO/compliance/support.

This remains a commercial hypothesis until external value and WTP gates pass.

## D8 — Score and investment status

Deep-dive analytical score: **7.80/10**.

Verdict:
**CONTINUE, BUT NARROW WEDGE**.

The score is not product proof.

AgentsWatch remains a primary portfolio bet only if 60–90 day external/commercial gates demonstrate decision-changing value beyond Git + CI + PR review + native vendor/Qodo alternatives.

## D9 — Do not change execution order

The next real task remains Gate 0 / `AW-VFY-001` until CI is green.

The deep dive does not authorize skipping implementation gates or starting SaaS/dashboard/integration breadth.
