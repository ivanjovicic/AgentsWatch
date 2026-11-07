# AW-RES-002 — Commercial design-partner protocol (research only)

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Parallel research; required input to AW-VFY-012, not a substitute for it  
Depends on: AW-RES-001 protocol draft recommended but not a hard blocker  
Run mode: research / protocol design only  
Budget: medium

## Purpose

Design how later commercial conversations will distinguish a real buyer
commitment from polite interest. This prompt does not obtain commitments and
does not start AW-VFY-012.

## What later counts (and does not count)

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

## Owned paths

- `docs/research/AW-RES-002-commercial-design-partner-protocol.md` (create if absent)

## Avoid paths

- Runtime code, SaaS, auth, billing implementation
- Drafting contracts as if a partner already exists
- Marking AW-VFY-012 complete or ready-to-execute before AW-VFY-011 passes

## Acceptance criteria

- Written definition of a concrete commitment, disqualifiers, and the evidence
  to keep (redacted) when AW-VFY-012 later runs.
- Explicit rule: zero invented commitments.
- AW-VFY-012 remains HOLD.

## STOP conditions

- Pressure to name fictional design partners or pricing wins.
- Pressure to implement billing or team administration now.
