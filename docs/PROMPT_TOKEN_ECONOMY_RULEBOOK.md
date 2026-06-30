# AgentsWatch Prompt Token Economy Rulebook

Last aligned: 2026-06-29  
Status: hard rules for prompt writing and execution

## Purpose

This is the strict rulebook for writing and executing prompts without wasting AI-agent tokens.

AgentsWatch exists to reduce waste. Its own prompts must be more disciplined than ordinary coding-agent prompts.

## Non-negotiable rule

A prompt that cannot name its scope, stop rules, validation, and expected evidence is not ready to run.

Do not “let the agent figure it out” unless the run mode is explicitly `investigation-only`.

---

## Severity levels

### Green prompt

Allowed to run.

Requirements:

- one repo;
- one task;
- one run mode;
- named files/folders or a strict discovery budget;
- validation named;
- stop rules present;
- final evidence format present.

### Yellow prompt

Must be split or tightened first.

Signs:

- scope is understandable but broad;
- validation exists but is generic;
- owned paths are missing;
- avoid paths are missing;
- task mixes docs and implementation.

### Red prompt

Do not run.

Signs:

- “analyze the whole repo”;
- “fix everything”;
- “make production-ready” without gates;
- combines investigation, implementation, tests, docs, review, and release;
- no validation;
- no stop rules;
- asks to continue huge chat history;
- asks for dashboard/SaaS before gates allow it;
- asks for runtime features before Gate 0 validation.

---

## Token budgets

### Low budget

Use for:

- validation-only;
- one bug;
- one command;
- one parser;
- one report formatter;
- docs/evidence update;
- diff-only review.

Limits:

```text
Max files to inspect: 8
Max files to edit: 3
Max broad searches: 2
Max validation commands: 3
Max commits: 1-3 small commits
```

### Medium budget

Use for:

- one feature slice;
- prompt optimizer expansion;
- git report end-to-end;
- one adapter plus tests.

Limits:

```text
Max files to inspect: 15
Max files to edit: 6
Max broad searches: 4
Max validation commands: 5
Must summarize before continuing if scope expands
```

### High budget

Use only for:

- documentation audit;
- release evidence;
- architecture planning;
- migration planning;
- broad review without implementation.

Limits:

```text
Implementation edits: forbidden unless explicitly scoped
Summarize every 10 files inspected
Stop if a smaller prompt can continue the work
```

---

## Discovery rules

When exact files are unknown, use staged discovery.

### Stage 1 — Find candidates

Allowed:

- search filenames;
- search exact command names;
- search exact class/function names;
- inspect queue/router/context docs.

Stop after:

```text
2 searches for low budget
4 searches for medium budget
```

### Stage 2 — Confirm ownership

Before editing, answer:

```text
Why this file?
What contract doc controls it?
What tests prove it?
What file must not be touched?
```

### Stage 3 — Edit only owned paths

If the necessary file is outside owned paths, stop and hand off.

---

## Read limits

Do not read docs because they are interesting. Read only because they are required.

Default read set:

```text
AGENTS.md
PROMPT_QUEUE_ROUTER.md
CONTEXT_INDEX.md
owning prompt file or queue
one relevant contract doc
```

Add more only if the current prompt names them or the first read proves they are needed.

---

## Edit limits

### Allowed edit shapes

Good:

- one command handler;
- one parser;
- one formatter;
- one test class;
- one queue status update;
- one evidence file.

Bad:

- rewrite Program.cs and all services together;
- change CLI, reports, config, adapters, and docs in one run;
- change roadmap while implementing runtime behavior;
- update many docs to hide missing validation;
- create feature code before Gate 0.

---

## Run mode enforcement

### validation-only

Allowed:

- run validation;
- inspect failures;
- fix build/test/smoke failures only;
- record evidence.

Forbidden:

- new features;
- refactors;
- dashboard/SaaS;
- unrelated docs expansion.

### investigation-only

Allowed:

- search;
- read targeted files;
- inspect tests;
- produce root cause and plan.

Forbidden:

- edit files;
- commit;
- mark Done.

### implementation

Allowed:

- edit owned runtime files;
- add tests;
- update docs only if behavior changed.

Forbidden:

- broad research;
- unrelated cleanup;
- multiple feature areas.

### tests

Allowed:

- add or fix tests for existing behavior;
- make small extraction for testability only if justified.

Forbidden:

- new product behavior;
- broad refactor.

### docs/evidence

Allowed:

- update docs, prompts, evidence, risk status.

Forbidden:

- runtime code changes.

### diff-only review

Allowed:

- review changed files only;
- compare claims to diff;
- identify missed tests and risks.

Forbidden:

- whole-repo review;
- new implementation.

---

## Prompt split rules

Split immediately if a prompt contains more than one of these:

- find root cause;
- implement fix;
- add tests;
- update docs;
- review diff;
- package/release;
- dogfood/report.

Default split:

```text
001-investigate-only
002-implement-minimal-fix
003-add-targeted-tests
004-diff-only-review
005-record-evidence
```

---

## Stop rules

Stop and report if:

- file inspection budget is exceeded;
- edit budget is exceeded;
- root cause is unknown after discovery;
- required validation cannot run;
- the task enters a blocked gate;
- another subsystem becomes necessary;
- the same validation failure repeats twice;
- the prompt asks for hidden or unsupported claims;
- the agent needs long chat history instead of a handoff.

Stopping is success when it prevents waste.

---

## Required prompt header

Every serious prompt must start with:

```text
Repository:
Prompt ID:
Queue:
Run mode:
Token budget:
Gate:
Owned paths:
Avoid paths:
Validation:
Stop rules:
```

If this header is missing, rewrite the prompt before running it.

---

## Forbidden prompt phrases

Reject or rewrite prompts that contain:

- “analyze everything”;
- “whole repo”;
- “fix all issues”;
- “make production-ready”;
- “also add dashboard”;
- “also add SaaS”;
- “skip tests”;
- “validation optional”;
- “do as much as possible”;
- “continue from chat history”;
- “mark done if it looks okay”.

---

## Final answer requirements

Every run must end with:

```text
Prompt ID:
Run mode:
Token budget used:
Files inspected:
Files changed:
Validation run:
Validation not run:
Commit SHA:
Completion %:
Missed:
Follow-up:
Residual risk:
Token waste avoided:
```

If any field is unknown, write `unknown` and explain why.

---

## Completion scoring

Do not score above:

```text
35% if evidence-only and validation is missing
59% if implementation is partial
79% if validation is missing for runtime changes
89% if tests are missing but follow-up exists
94% if CI/local validation is not both clear enough for the task
```

Use 95-100% only when implementation, tests/evidence, docs, and residual risk are all handled.

---

## Token waste report

Every non-trivial run must report:

```text
Files inspected:
Files changed:
Inspection/change ratio:
Broad searches used:
Prompt split used: yes/no
Handoff used: yes/no
Whole-repo review avoided: yes/no
Largest waste avoided:
Next token-saving improvement:
```

---

## AgentsWatch-specific override

Gate 0 is currently incomplete.

Until restore/build/test and CLI smoke evidence exist, the only safe implementation-adjacent prompt is:

```text
AW-VAL-001 — Build validation
```

All feature/productization prompts are blocked unless they are docs-only and do not claim runtime completion.
