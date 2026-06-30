# AgentsWatch Zero-Waste Execution Protocol

Last aligned: 2026-06-29  
Status: mandatory execution protocol

## Purpose

This protocol controls how an agent executes a prompt after the prompt has been accepted.

It exists to prevent token waste during execution, not only during prompt writing.

## Execution principle

Every action must answer one question:

```text
Does this move the current prompt closer to validated completion?
```

If the answer is no, stop or record why the action is still required.

---

## Phase 0 — Pre-run gate

Before reading implementation files, verify:

- prompt ID exists;
- queue exists;
- run mode is one value only;
- token budget exists;
- Gate status is known;
- owned paths are named;
- avoid paths are named;
- validation command is named;
- evidence format is named.

If any item is missing, do not execute. Rewrite or split the prompt first.

---

## Phase 1 — Minimal context loading

Read only:

1. `AGENTS.md`;
2. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`;
3. owning prompt/queue;
4. `docs/CONTEXT_INDEX.md`;
5. one relevant contract doc;
6. target file(s).

Do not follow every link in `DOCS_INDEX.md`.

Do not read old audit files unless the task is an audit or evidence review.

---

## Phase 2 — Discovery budget

Low budget discovery:

```text
max searches: 2
max files opened after search: 5
max unrelated results inspected: 0
```

Medium budget discovery:

```text
max searches: 4
max files opened after search: 12
max unrelated results inspected: 2
```

If the target is not found within budget, stop and create a better investigation prompt.

---

## Phase 3 — Work decision

Before editing, state internally:

```text
Target file:
Why this file:
Contract controlling behavior:
Expected validation:
Risk level:
```

If there is no contract, write or update the contract before implementation.

If there is no validation path, use investigation/docs-evidence mode instead of implementation mode.

---

## Phase 4 — Patch discipline

Allowed:

- one focused behavior change;
- one focused test class;
- one focused docs update;
- one queue status correction;
- one evidence file.

Forbidden:

- touching unrelated modules;
- changing docs to hide missing validation;
- broad cleanup;
- changing architecture and implementation in the same run;
- modifying multiple queues without evidence.

After two failed write/patch attempts, stop and record waste.

---

## Phase 5 — Validation discipline

Run validation in dependency order.

For Gate 0:

```text
restore -> build -> test -> CLI smoke
```

For feature work:

```text
targeted tests -> solution build -> broader tests if needed
```

Never claim a later step passed if an earlier prerequisite failed.

If validation cannot run, record:

```text
Validation not run:
Reason:
Risk:
Follow-up prompt:
```

---

## Phase 6 — Waste checkpoint

Before final response, answer:

```text
What did I inspect that was not needed?
What action failed or was blocked?
What should the next agent avoid?
What rule or prompt can prevent this next time?
```

If any answer exists, update evidence and add an optimized prompt or rule update.

---

## Phase 7 — Final evidence

A run is incomplete unless it records:

- what was done;
- what was missed;
- validation run/not run;
- waste found;
- waste cause;
- docs/rules updated;
- optimized prompt added or reason none is needed;
- completion percentage;
- residual risk;
- commit SHA.

---

## Mandatory stop conditions

Stop immediately if:

- prompt violates Gate 0;
- scope expands beyond budget;
- owned paths are unclear;
- avoid paths are missing;
- validation is undefined;
- tool blocks the same action twice;
- same search fails twice;
- same command fails twice for same reason;
- implementation requires another subsystem;
- user intent changes mid-run and invalidates the queue.

## Success definition

A stopped run with precise evidence is better than a long run with hidden uncertainty.
