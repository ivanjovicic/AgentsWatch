# DISC-001 — Capture run discoveries

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_discovery_and_self_improvement.md`  
Run mode: docs/evidence  
Token budget: low  
Permission mode: docs_only  
Gate: after any non-trivial run has evidence or a final response

## Goal

Extract meaningful findings that were noticed during the run but were not safely resolved inside the run's owned scope.

## Read first

- originating `.ai/runs/*-evidence.md` file;
- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`;
- `.ai/discoveries/README.md`;
- existing open discovery records with matching category, title, or path.

## Inspect only

- one originating run log;
- its linked final evidence or compact handoff when required;
- likely duplicate discovery records.

Do not inspect runtime code unless the run log lacks enough evidence to describe the finding.

## Task

1. Read `What was missed`, `Residual risk`, `Mistakes observed`, `Follow-up prompt`, and any out-of-scope notes.
2. Extract only meaningful findings with durable value.
3. For each finding, search for an existing discovery with the same root issue.
4. Create a new record from `.ai/DISCOVERY_RECORD_TEMPLATE.md`, or update/link the existing record.
5. Record source run, evidence summary, category, severity, confidence, affected paths/contracts, and why it was outside scope.
6. Do not route or implement unrelated runtime work beyond an initial safe classification.
7. Update the originating run log with discovery IDs and capture status.

## Validation

- every captured finding links to the originating run;
- no same-root duplicate record is created;
- no secret-bearing raw output or full diff is copied;
- `git diff --check` when files are changed.

## Final evidence

- run reviewed;
- discoveries created;
- discoveries updated;
- duplicates linked;
- findings intentionally ignored and reason;
- validation;
- residual risk;
- next prompt: usually `DISC-002` when untriaged findings remain.

## Stop rules

Stop and mark `NeedsEvidence` when the run only contains speculation, the affected area is unknown, or accurate capture would require broad repository analysis.
