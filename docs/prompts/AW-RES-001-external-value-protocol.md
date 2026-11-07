# AW-RES-001 — External-value interview protocol (research only)

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Parallel research; required input to AW-VFY-011, not a substitute for it  
Depends on: a clear statement of current product promises (receipts, evidence
gate, AWE009 / NeedsReview)  
Run mode: research / protocol design only  
Budget: medium

## Purpose

Design the interview and comparison protocol that AW-VFY-011 will later
execute with real external agent-heavy users. This prompt collects no customer
quotes, invents no evaluations, and does not start AW-VFY-011.

## What the protocol must compare against

Native vendor logs, Git, CI, PR review, and current independent-review tools
(including Qodo where relevant). AgentsWatch has to change a real decision,
not merely add another tracking view.

## Questions the protocol should force later interviewers to answer

- Did a receipt change a merge, rewrite, or acceptance decision?
- Did unverified `Supported` / `NeedsReview` (AWE009) increase or reduce trust?
- Were false positives or ambiguity more expensive than the catch?
- Was cross-vendor independence valued?
- Which baseline already covered the case?

## Owned paths

- `docs/research/AW-RES-001-external-value-protocol.md` (create if absent)

## Avoid paths

- Runtime code
- Contacting customers or fabricating quotes
- Marking AW-VFY-011 ready-to-execute before AW-VFY-010 completes
- SaaS / billing / team-admin design

## Acceptance criteria

- A reusable interview script, consent/privacy notes, and a scoring sheet that
  can record “no decision-changing value” as a valid outcome.
- Explicit rule: zero invented respondents.
- AW-VFY-011 remains HOLD.

## STOP conditions

- Pressure to invent “3 teams asked to keep using it”.
- Pressure to start external outreach from this research prompt without owner
  authorization naming who is contacted.
