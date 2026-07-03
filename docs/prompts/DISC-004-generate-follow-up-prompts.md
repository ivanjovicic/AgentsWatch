# DISC-004 — Generate follow-up prompts from discoveries

Repository: `ivanjovicic/AgentsWatch`  
Queue: `agentwatch_discovery_and_self_improvement.md`  
Run mode: docs/evidence  
Token budget: low-medium  
Permission mode: docs_only  
Gate: one or more reconciled discoveries require action

## Goal

Generate the smallest copy-ready prompts and truthful queue candidates needed to resolve actionable discoveries.

## Read first

- selected discovery records;
- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`;
- `../PROMPT_TOKEN_ECONOMY_RULEBOOK.md`;
- `../PROMPT_LINT_CHECKLIST.md`;
- `../prompt_queues/PROMPT_QUEUE_ROUTER.md`;
- the owning queue and one relevant contract document.

## Inspect only

- selected discoveries sharing one owner or dependency chain;
- owning queue;
- direct target contracts or paths needed to scope the prompt.

## Task

1. Group only discoveries that have the same run mode, owner, gate, and validation path.
2. Split unrelated or multi-mode work into separate prompts.
3. Choose investigation-only for unknown or possible findings.
4. Include repository, prompt ID, source discovery IDs, queue, run mode, token budget, read-first files, inspect-only paths, owned paths, avoid paths, task, acceptance criteria, stop rules, validation, required evidence, and discovery reconciliation output.
5. Add or update the owning queue row in Planned, Blocked, or Ready state according to current gates.
6. Link prompt and queue row from every source discovery.
7. Avoid recreating a prompt or queue item that already owns the same root issue.
8. Do not execute the generated prompt in this run.

## Validation

- prompt passes `PROMPT_LINT_CHECKLIST.md`;
- prompt has one primary run mode;
- every source discovery is linked;
- queue state matches dependencies and Gate 0;
- no duplicate prompt ID or same-root queue row exists;
- `git diff --check`.

## Final evidence

- discoveries converted to prompts;
- prompt files created or updated;
- queue rows created or updated;
- discoveries left without prompts and reason;
- validation;
- residual risk;
- recommended execution order.

## Stop rules

Stop and create an investigation prompt rather than an implementation prompt when the root cause, target paths, or validation path is uncertain. Stop and split when more than one primary run mode is required.
