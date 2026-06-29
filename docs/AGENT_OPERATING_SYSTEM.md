# AgentsWatch Agent Operating System

Last aligned: 2026-06-29  
Status: canonical agent workflow

## Purpose

This document adapts the strongest AI-agent workflow rules from `Mathlearning-Mobile-App` to AgentsWatch.

AgentsWatch is itself a product for supervising agents, so its repo must be stricter than a normal app repo.

## Source ideas adapted

From MathLearning:

- token budget and scope limiter;
- investigation-only mode;
- split prompts into investigation, implementation, tests, and review;
- long-task control loop;
- shell-neutral command blocks;
- evidence-first final responses;
- completion percentage, missed work, follow-up, residual risk.

AgentsWatch-specific additions:

- bootstrap validation comes before feature work;
- local-first/privacy-first product rules;
- markdown report contracts before SQLite/dashboard;
- CLI command safety before automation;
- no cloud/SaaS scope until roadmap gates allow it.

---

## Required read order

For every non-trivial task, read:

1. `AGENTS.md`
2. `docs/CONTEXT_INDEX.md`
3. `docs/BOOTSTRAP_NEXT_STEPS.md` if Gate 0 is incomplete
4. the owning prompt queue
5. the relevant product contract doc
6. this file if agent behavior is unclear

For broad, multi-step, validation, review, or handoff work, also read:

- `docs/PROMPT_RULES.md`
- `docs/PROMPT_QUALITY_CHECKLIST.md`
- `docs/AGENT_COMMAND_PLAYBOOK.md`
- `docs/AGENT_LONG_TASK_PLAYBOOK.md`
- `docs/PROMPT_EVIDENCE_TEMPLATE.md`

---

## Run modes

Use exactly one primary run mode per agent run:

```text
validation-only
investigation-only
implementation
tests
review-only
docs/evidence
diff-only review
```

If a task needs more than one mode, split it.

---

## Default control loop

```text
1. Select one prompt.
2. Confirm bootstrap/roadmap gate status.
3. State run mode, token budget, scope, stop rules.
4. Inspect only the smallest useful docs/files.
5. Make the smallest safe change, or stop with evidence.
6. Run narrow validation when available.
7. Record validation honestly.
8. Commit only owned files.
9. Final response includes commit SHA, completion %, missed, follow-up, residual risk.
```

---

## Bootstrap override

While Gate 0 is incomplete, validation prompts override MVP feature prompts.

Required order:

1. `AW-VAL-001`
2. `AW-VAL-002`
3. `AW-VAL-003`
4. `AW-VAL-004`
5. then continue MVP prompts

Do not implement new CLI features before build/test/smoke evidence exists.

---

## Done means evidence exists

A prompt is not Done because code exists.

A prompt is Done only when the repo has:

- implemented or documented result;
- validation evidence or explicit blocked reason;
- risk note;
- follow-up prompt if incomplete;
- pushed commit on `main` or documented reason push was not possible.
