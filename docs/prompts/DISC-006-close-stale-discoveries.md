# DISC-006 — Close stale discoveries

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_discovery_and_self_improvement.md`  
Run mode: docs/evidence  
Token budget: low  
Permission mode: docs_only  
Gate: periodic maintenance or discoveries with old review dates

## Goal

Review old discovery records and close, merge, re-triage, or reactivate them using current evidence.

## Read first

- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`;
- selected stale/open discovery records;
- linked run logs, prompts, queue rows, and resolving commits or pull requests;
- only the canonical owner documents needed to verify status.

## Inspect only

- records older than the chosen review threshold;
- direct linked evidence;
- likely duplicate records.

## Task

1. Verify whether the underlying issue is still open.
2. Mark Resolved only when linked evidence shows the work or durable documentation is complete.
3. Mark Duplicate and link the canonical record when the root issue matches another item.
4. Mark Rejected with a reason when the item is no longer justified.
5. Return uncertain items to Triaged or NeedsEvidence and add an investigation prompt when useful.
6. Refresh owner, queue, prompt, gate, validation, and last-reviewed fields.
7. Do not delete historical records.

## Validation

- every reviewed record has a current status and disposition;
- resolved records link to evidence;
- duplicates link to one canonical record;
- open actionable records retain an owner and next step;
- `git diff --check`.

## Final evidence

- records reviewed;
- resolved;
- duplicates;
- rejected;
- reactivated or returned for evidence;
- prompts/queues refreshed;
- validation;
- remaining uncertainty.

## Stop rules

Do not close a security, privacy, data-loss, validation, or release finding without explicit supporting evidence and review.
