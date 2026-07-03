# DISC-002 — Reconcile discovery inbox

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_discovery_and_self_improvement.md`  
Run mode: docs/evidence  
Token budget: low-medium  
Permission mode: docs_only  
Gate: one or more discovery records are in Inbox or NeedsEvidence

## Goal

Deduplicate, classify, assign ownership, route, and give every reviewed discovery a clear disposition.

## Read first

- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`;
- `.ai/discoveries/README.md`;
- discovery records being reviewed;
- `../ai/learning/MISTAKE_LEDGER.md`;
- `../prompt_queues/PROMPT_QUEUE_ROUTER.md`;
- only the owning queue/doc needed for routing.

## Inspect only

- selected Inbox, NeedsEvidence, or stale records;
- likely duplicates;
- the minimal canonical owner doc or queue.

## Task

For each selected record:

1. Normalize title, category, affected paths, and evidence signature.
2. Search discoveries, mistake cards, risks, queue rows, and prompt IDs for the same root issue.
3. Link duplicates and choose one canonical record.
4. Set category, severity, confidence, status, primary owner, gate/dependencies, and recommended validation.
5. Route confirmed documentation gaps to `DISC-003`.
6. Route actionable findings needing prompts to `DISC-004`.
7. Route uncertain findings to a new investigation-only prompt candidate.
8. Route repeated agent behavior to the mistake ledger as well as the primary discovery owner.
9. Record a no-op or rejection reason when action is not justified.
10. Do not mark a blocked item Ready merely because a prompt exists.

## Validation

- every reviewed item has one primary owner;
- every actionable item has a prompt target or documented reason none is needed;
- every duplicate links to its canonical record;
- Gate 0 and dependency statuses remain truthful;
- `git diff --check` when files are changed.

## Final evidence

- discoveries reviewed;
- status changes;
- duplicates merged/linked;
- owners assigned;
- items routed to docs, queues, prompts, risks, or mistakes;
- unresolved evidence gaps;
- validation;
- next prompts.

## Stop rules

Stop and leave an item in NeedsEvidence when classification requires broad code analysis, sensitive-path review, or evidence that is not present in the originating run.
