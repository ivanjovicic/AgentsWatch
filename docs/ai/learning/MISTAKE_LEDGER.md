# AgentsWatch Agent Mistake Ledger

Status: active agent-learning memory  
Last aligned: 2026-07-01  
Scope: `ivanjovicic/AgentsWatch`

## Purpose

This ledger tracks repeated agent mistakes in the AgentsWatch repo.

`.ai/runs/` logs explain one run. This ledger captures patterns so future agents do not rediscover them from scattered queue rows, specs, audits, and commits.

A run is not learning-complete until every observed mistake is classified as:

```text
new mistake with a mistake card
repeated mistake with a rule/prompt/test/queue/lint update
false alarm with explanation
```

## How agents must use this file

Before starting a non-trivial prompt:

1. Read this ledger.
2. Pick only mistake IDs relevant to the current prompt.
3. Write them in the run log under `Relevant prior mistakes read`.
4. Explain how the run will avoid repeating them.

Before marking a prompt Done:

1. Add a new mistake card if a new mistake was found.
2. Update an existing card if a known mistake repeated.
3. Add or update a rule, prompt, test, queue row, or validation/lint prompt for repeated mistakes.
4. If no update is needed, write the reason in the run log.

## Severity

| Severity | Meaning | Required action |
|---|---|---|
| P0 | Broken validation gate, false runtime/release claim, privacy/local-first violation, or unsafe product direction | rule + test/lint/prompt update required immediately |
| P1 | Wrong Done status, missing evidence, misleading audit, wasted context, stale router | rule/prompt/queue update required |
| P2 | Local inefficiency, stale doc reference, minor template gap | prompt/template update or documented no-op |

## Status values

```text
Open | Mitigated | Watching | Retired | False alarm
```

---

## Known mistakes

### AW-MISTAKE-EVIDENCE-001 — Run evidence exists only in broad docs, not `.ai/runs`

Severity: P1  
Status: Open  
First seen: AgentsWatch early docs/evidence before `.ai/runs` hard gate

Problem:
Agents recorded validation/audit evidence in broad docs such as `VALIDATION_EVIDENCE_*` or audit files, but did not always create a compact per-prompt `.ai/runs/<prompt-id>-evidence.md` log.

Impact:
Later agents must reconstruct what happened from multiple docs. This wastes context and weakens completion scoring.

Root cause:
AgentsWatch had an evidence standard but not the same hard run-log gate used by MathLearning Flutter/backend.

Prevention:
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md` added.
- `.ai/RUN_LOG_TEMPLATE.md` and `.ai/runs/README.md` added.
- Queue rows must reference run logs or explicit fallback.

Next check:
New AgentsWatch prompt-system or runtime runs should create `.ai/runs` logs before Done.

---

### AW-MISTAKE-GATE-001 — Feature work can outrun Gate 0 validation

Severity: P0  
Status: Watching  
First seen: AgentsWatch bootstrap planning docs

Problem:
A prompt can start product/feature work before restore/build/test/CLI smoke evidence is current.

Impact:
The repo can accumulate feature specs or runtime code before the CLI skeleton is proven runnable.

Root cause:
Roadmap/prompt queues can be attractive even while bootstrap validation remains incomplete.

Prevention:
- `AGENTS.md` Gate 0 rule.
- `docs/AGENT_SHARED_OPERATING_STANDARD.md` AgentsWatch-specific Gate 0 rule.
- Prompt router must keep validation prompts first until evidence is current.

Next check:
Before any implementation prompt, verify build/test/smoke evidence or mark feature prompt Blocked.

---

### AW-MISTAKE-AUDIT-001 — Docs-only audit treated as product/runtime completion

Severity: P1  
Status: Open  
First seen: migrated planning/spec docs and productization audits

Problem:
Docs-only audits, roadmap additions, or prompt queues can be interpreted as actual CLI/product implementation.

Impact:
Stakeholders may believe AgentsWatch can lint/evaluate/optimize real runs when only the design or queue exists.

Root cause:
Prompt status language did not always separate `docs-only audit`, `prompt-ready`, `runtime-fixed`, and `test-validated`.

Prevention:
- Use statuses from `docs/AGENT_SHARED_OPERATING_STANDARD.md`.
- Docs-only work must not claim runtime completion.
- Completion score caps apply when validation/runtime proof is missing.

Next check:
Prompt queues should not mark runtime functionality Done unless tests or CLI smoke evidence exist.

---

### AW-MISTAKE-CONTEXT-001 — Broad docs reads before context index/router

Severity: P2  
Status: Watching  
First seen: prompt-system organization work

Problem:
Agents can read many docs when `docs/CONTEXT_INDEX.md`, router, or quick rules would identify the smallest useful set.

Impact:
Wasted context and slower runs.

Root cause:
AgentsWatch has many planning docs and product specs; without a strict read budget, agents over-read.

Prevention:
- Use token budget from `docs/AGENT_SHARED_OPERATING_STANDARD.md`.
- Prefer `docs/CONTEXT_INDEX.md` and prompt router before broad docs.
- Stop and split if more than the budget is needed.

Next check:
Run logs should record docs inspected and the inspection/change ratio.

---

## Add new mistake card

Use `docs/ai/learning/MISTAKE_CARD_TEMPLATE.md` and IDs:

```text
AW-MISTAKE-<AREA>-<NNN>
```

Areas: `EVIDENCE`, `GATE`, `AUDIT`, `VALIDATION`, `CONTEXT`, `QUEUE`, `SCOPE`, `CLAIM`, `PRODUCT`, `PRIVACY`.
