# AgentsWatch Documentation Audit — 2026-06-29

## Scope

Audit request: review the documentation, find missing or weak areas, and improve what is safe to improve before runtime feature work.

This was a docs-only audit. Runtime code was not changed.

## Documents inspected

Primary inspected/current docs:

- `AGENTS.md`
- `docs/DOCS_INDEX.md`
- `docs/AGENT_OPERATING_SYSTEM.md`
- `docs/CONTEXT_INDEX.md`
- `docs/AGENT_COMMAND_PLAYBOOK.md`
- `docs/AGENT_LONG_TASK_PLAYBOOK.md`
- `docs/PROMPT_RULES.md`
- `docs/PROMPT_QUALITY_CHECKLIST.md`
- `docs/PROMPT_EVIDENCE_TEMPLATE.md`
- `docs/COMPLETION_ANALYTICS.md`
- `docs/CLAIMS_VS_ACTUAL_REVIEW.md`
- `docs/MATHLEARNING_DOCS_ADAPTATION.md`

MathLearning references used for adaptation:

- token optimization playbook;
- long-task playbook;
- command playbook;
- prompt quality checklist;
- prompt playbook concepts.

## Issues found

### 1. `AGENTS.md` was stale

Problem: it still pointed mostly to early product docs and did not route agents through the new docs operating system, context index, or bootstrap validation order.

Fix: updated `AGENTS.md` to use:

- `docs/DOCS_INDEX.md`;
- `docs/AGENT_OPERATING_SYSTEM.md`;
- `docs/CONTEXT_INDEX.md`;
- bootstrap validation docs while Gate 0 is incomplete;
- product contract docs.

### 2. Broken prompt playbook reference

Problem: some docs referenced `docs/PROMPT_PLAYBOOK.md`, but that file had been intentionally replaced by `docs/PROMPT_RULES.md` after a larger file write was blocked.

Fix: replaced references in:

- `docs/AGENT_OPERATING_SYSTEM.md`;
- `docs/CONTEXT_INDEX.md`.

### 3. Claims-vs-actual review was not indexed

Problem: `CLAIMS_VS_ACTUAL_REVIEW.md` existed but was not visible in `DOCS_INDEX.md`.

Fix: indexed it under Agent workflow and prompts.

### 4. Docs governance was missing

Problem: there was no rule for source-of-truth ordering, broken references, and when docs must be updated.

Fix: added `docs/DOCS_GOVERNANCE.md`.

### 5. Audit evidence was missing

Problem: the repo had many docs, but no audit report explaining what was checked and what was fixed.

Fix: added this audit report.

## Improvements made

- Added/updated source-of-truth routing.
- Fixed broken prompt rules references.
- Added docs governance.
- Added audit evidence.
- Strengthened Gate 0-first workflow in `AGENTS.md`.

## Remaining gaps

These are intentionally not fixed in this docs-only pass:

1. Gate 0 build/test/smoke validation is still not complete.
2. Runtime code was not validated.
3. CI visibility for the current commit still needs real workflow evidence.
4. Prompt queues may still need status updates after validation runs.
5. README could be improved, but large README updates have been blocked before; prefer small README patches later.

## Recommended next prompt

```text
Repository: ivanjovicic/AgentsWatch

Use:
- AGENTS.md
- docs/AGENT_OPERATING_SYSTEM.md
- docs/CONTEXT_INDEX.md
- docs/AGENT_COMMAND_PLAYBOOK.md
- docs/prompts/AW-VAL-001-build-validation.md

Run mode: validation-only
Token budget: low

Do not add features.
Run:
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln

Fix only build/test failures.
Record validation evidence.
Then run AW-VAL-002 CLI smoke validation.
```

## Completion

Done 85% for documentation audit.

Tests: not run; docs-only pass through GitHub connector.
Missed: full automated link checker and local build/test validation.
Follow-up: AW-VAL-001 build validation.
Residual risk: docs are stronger, but runtime build status is still unknown.
