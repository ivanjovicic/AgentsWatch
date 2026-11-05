# AW-RES-002 — Independent buyer discovery and commercial-test design

Repository: `ivanjovicic/AgentsWatch`  
Owning queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Ready now for **preparation**; external evaluations happen only at AW-VFY-011 and commercial gate at AW-VFY-012  
Run mode: research / outreach plan, no source changes  
Context budget: low/medium; one concise workbook-equivalent Markdown report

## Goal

Disprove the assumption that AI-heavy teams will pay for an independent run receipt. Distinguish the developer-user from a buyer with budget.

## Tasks

1. Use `docs/PRODUCT_SPEC.md`, `AW-VFY-011`, `AW-VFY-012`, and `docs/research/agentswatch_deep_dive_2026_09/07_BUYERS_ICP_AND_WTP.md`.
2. Create an ICP screen for 5–30 developer teams: number of meaningful coding-agent runs per week, one/multiple agent vendors, current PR/review policy, local/dirty worktrees, review cost, procurement constraints, security sensitivities and actual payer.
3. Prepare an outreach shortlist framework for at least 15 eligible contacts across >=2 coding-agent ecosystems. Never invent names, interviews, replies or permissions. Contact people only with an explicitly authorized outreach workflow.
4. Prepare a neutral interview script: ask for last real false-Done/rework case, current tool demonstration, time spent, outcome, existing solution, severity, budget owner, buying trigger and what would cause refusal. Avoid leading "would you use AI verifier?" questions.
5. Design baseline-vs-AgentsWatch blinded evaluation: record whether reviewer already knew the finding, whether the receipt changes an actual review/rework/merge/evidence decision, added tool time and reviewer minutes saved. Use a single comparable worksheet per participant.
6. Set commercial tests for team flat-rate and per-seat pricing. Suggested €79–199/team/month is a **hypothesis only**. Request observable paid-pilot/procurement commitment rather than "interest".
7. Document data privacy/consent for repository/CI evidence and non-collection of credentials, private code and full chat by default.
8. Define kill rules: low decision impact, high false-positive burden, no identified payer, or native/Qodo alternative preferred after realistic side-by-side use.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/**`
- optional compact `.ai/runs/**`

## Avoid

- `src/**`, `tests/**`, SaaS/billing/auth implementation
- contacting external people, collecting private repository access or creating purchase commitments without explicit authorization
- treating founder dogfood or a free enthusiastic demo as paid WTP

## Required output

`docs/research/agentswatch_market_validation_2026_11/buyer_discovery_protocol.md` with:
- ICP exclusion/inclusion table, interviewer script and minimal per-run evaluation record
- recruitment channel hypotheses, privacy/access checklist, pricing options
- sample ROI formula with clearly labeled inputs (reviewer minutes saved × runs × loaded rate minus overhead)
- thresholds consistent with AW-VFY-011 (>=10 real evaluations, >=3 decision-changing cases, >=3 continued-use requests) and AW-VFY-012 (>=3 real commercial commitments)

## Definition of done

The study can be executed by a second person without inventing findings. This prep is **not** evidence that either external or commercial gate passed.
