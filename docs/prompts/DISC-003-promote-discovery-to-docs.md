# DISC-003 — Promote a discovery to canonical documentation

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_discovery_and_self_improvement.md`  
Run mode: docs/evidence  
Token budget: low  
Permission mode: docs_only  
Gate: a reconciled discovery represents durable knowledge

## Goal

Move confirmed durable knowledge from a discovery record into the smallest correct canonical document without bloating unrelated documentation.

## Read first

- selected discovery record;
- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`;
- `../DOCS_GOVERNANCE.md`;
- the one canonical owner document;
- `../DOCS_INDEX.md` only when navigation changes.

## Inspect only

- selected discovery;
- one canonical owner document;
- direct references that must stay consistent.

## Task

1. Confirm the discovery is reconciled, supported by evidence, and not a duplicate.
2. Confirm that the finding is durable knowledge rather than one-run evidence.
3. Add the smallest accurate canonical update.
4. Include the discovery ID where provenance is useful.
5. Update the docs index, router, or queue only when ownership, navigation, or status actually changes.
6. Check for contradictions with current code/tests, AGENTS.md, and higher-priority contracts.
7. Update the discovery with target document, disposition, validation, and resolving commit or pull-request placeholder.
8. Generate a separate implementation or test prompt when documentation does not resolve the underlying work.

## Validation

- referenced paths exist;
- no duplicate canonical rule was added;
- no docs-only change claims runtime behavior is implemented;
- links and queue states are consistent;
- `git diff --check`.

## Final evidence

- discovery ID;
- canonical document updated;
- durable rule or knowledge added;
- secondary links updated;
- implementation or test work still required;
- validation;
- residual risk;
- next prompt.

## Stop rules

Stop and return the discovery to NeedsEvidence when the durable statement cannot be supported. Stop and split when more than one unrelated canonical document needs substantive changes.
