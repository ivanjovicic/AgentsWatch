# AW-RES-002 — Buyer discovery, ICP/ROI design, and commercial-commitment protocol

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Parallel research; required input to AW-VFY-012, not a substitute for it  
Depends on: AW-RES-001 protocol draft recommended but not a hard blocker  
Run mode: research / outreach plan only; no source or billing changes  
Budget: medium

## Purpose

Design how later commercial conversations will distinguish a real buyer
commitment from polite interest, and prepare the ICP, recruitment, ROI and
pricing instruments needed before AW-VFY-011/012 execute. This prompt does not
obtain commitments, does not contact people, and does not start AW-VFY-011 or
AW-VFY-012. Those gates remain HOLD. Planned commercial thresholds are
unchanged.

## Part A — ICP, recruitment, ROI and pricing (restored from PR #9)

1. Use `docs/PRODUCT_SPEC.md`, `docs/prompts/AW-VFY-011-external-value-validation.md`,
   `docs/prompts/AW-VFY-012-commercial-design-partner-validation.md`, and
   `docs/research/agentswatch_deep_dive_2026_09/07_BUYERS_ICP_AND_WTP.md`.
2. Create an ICP screen for 5–30 developer teams: number of meaningful
   coding-agent runs per week, one/multiple agent vendors, current PR/review
   policy, local/dirty worktrees, review cost, procurement constraints,
   security sensitivities and actual payer. Distinguish the developer-user
   from a buyer with budget.
3. Prepare an outreach shortlist **framework** for at least 15 eligible
   contacts across >=2 coding-agent ecosystems. Never invent names,
   interviews, replies or permissions. Contact people only with an explicitly
   authorized outreach workflow.
4. Prepare a neutral interview script: ask for the last real false-Done/rework
   case, current tool demonstration, time spent, outcome, existing solution,
   severity, budget owner, buying trigger and what would cause refusal. Avoid
   leading "would you use an AI verifier?" questions.
5. Design baseline-vs-AgentsWatch evaluation recording: whether the reviewer
   already knew the finding, whether the receipt changes an actual
   review/rework/merge/evidence decision, added tool time and reviewer minutes
   saved. Use a single comparable worksheet per participant. Do not claim
   reviewer benefit was measured until those fields are actually filled by a
   later authorized evaluation.
6. Set commercial tests for two pricing hypotheses: team flat-rate and
   per-seat. Suggested €79–199/team/month is a **hypothesis only**. Request
   observable paid-pilot/procurement commitment rather than "interest".
7. Document data privacy/consent for repository/CI evidence and non-collection
   of credentials, private code and full chat by default.
8. Define kill rules: low decision impact, high false-positive burden, no
   identified payer, or native/Qodo alternative preferred after realistic
   side-by-side use.
9. Include a sample ROI formula with clearly labeled inputs:
   (reviewer minutes saved × runs × loaded rate) minus overhead.
   Do not invent numeric savings.

## Part B — What later counts as a commitment (kept from the earlier PR #14 draft)

Counts only when AW-VFY-012 later records it from a real buyer:

- paid pilot / design-partner agreement or equivalent written commitment
- identified budget owner
- payment reason tied to verification / review / governance value

Does not count:

- internal enthusiasm
- invented quotes
- same-account PR comments
- a plan or backlog file
- packaging ideas from open PR #8 or similar future-gated strategy docs

Thresholds stay consistent with the canonical queue and are not met by this
prep: AW-VFY-011 still requires >=10 real evaluations, >=3 decision-changing
cases, >=3 continued-use requests; AW-VFY-012 still requires >=3 real
commercial commitments. This document does not change those gates.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/buyer_discovery_protocol.md`
- optional compact `.ai/runs/**`

## Avoid

- `src/**`, `tests/**`, SaaS/billing/auth implementation
- contacting external people, collecting private repository access or creating
  purchase commitments without explicit authorization
- treating founder dogfood or a free enthusiastic demo as paid willingness to pay
- marking AW-VFY-012 complete or ready-to-execute before AW-VFY-011 passes

## Acceptance criteria

- ICP inclusion/exclusion table, interviewer script, >=15-candidate recruitment
  framework (without invented names), privacy/access checklist, two pricing
  hypotheses, ROI formula with labeled inputs, and commitment/disqualifier
  definitions exist.
- The study can be executed later by a second person without inventing findings.
- Zero invented respondents or commitments.
- AW-VFY-011 and AW-VFY-012 remain HOLD.

## STOP conditions

- Pressure to name fictional design partners, contacts, or pricing wins.
- Pressure to implement billing or team administration now.
- Pressure to start outreach without owner authorization naming who is contacted.
