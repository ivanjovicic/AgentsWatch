# AgentsWatch Agent Operating System

Last aligned: 2026-06-30  
Status: canonical agent workflow, not default context

## Purpose

This document defines the agent workflow for AgentsWatch.

Do not read this file for every small task. Read it when agent behavior, workflow, long-task control, or evidence rules are unclear.

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

## Read order

Default small task:

```text
1. owning prompt or queue section
2. one relevant contract doc
3. one implementation file or focused design doc
```

If gate status is unclear, add:

```text
docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
```

If anti-waste rules are needed, add:

```text
docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md
```

Read this operating-system file only if the run needs workflow clarification.

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
2. Confirm gate status only if relevant.
3. State run mode, token budget, scope, stop rules.
4. Inspect only the smallest useful docs/files.
5. Make the smallest safe change, or stop with evidence.
6. Run narrow validation when available.
7. Record validation honestly.
8. Commit only owned files.
9. Final response includes evidence, missed work, follow-up, residual risk.
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

Docs-only planning is allowed if it does not claim runtime completion.

---

## Done means evidence exists

A prompt is not Done because code exists.

A prompt is Done only when the repo has:

- implemented or documented result;
- validation evidence or explicit blocked reason;
- risk note;
- follow-up prompt if incomplete;
- pushed commit or documented reason push was not possible.

---

## Anti-waste override

If following a rule would require reading many extra docs for a small task, use the quick rules and stop at the smallest safe context.

Prefer:

```text
compact evidence > long reports
one contract doc > many background docs
targeted validation > full validation
handoff summary > chat history
command profile summary > command log paste
```
