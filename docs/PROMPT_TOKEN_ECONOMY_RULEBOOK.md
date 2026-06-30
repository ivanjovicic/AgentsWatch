# AgentsWatch Prompt Token Economy Rulebook

Last aligned: 2026-06-30  
Status: full anti-waste rules; use quick rules by default

## Purpose

This is the full rulebook for writing and executing agent prompts without wasting tokens.

Default behavior:

```text
Use docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md for normal runs.
Use this full rulebook only when writing, changing, or auditing prompt-system rules.
```

## Non-negotiable rules

A prompt is not ready unless it has:

- one task;
- one primary run mode;
- a token budget;
- scope or discovery limit;
- stop rules;
- validation or an explicit blocked reason;
- expected evidence.

Do not “let the agent figure it out” unless the run mode is explicitly `investigation-only`.

## Context budget

```text
Low budget: read up to 3 docs before first action
Medium budget: read up to 5 docs before first action
High budget: read up to 8 docs before first action
```

If more docs are needed, stop and explain why before reading more.

Do not read docs because they are interesting. Read only because they are required.

## Default read set

Most prompts should start with:

```text
1. owning prompt or queue section
2. one relevant contract doc
3. one implementation file or focused design doc
```

Add optional docs only when the task proves they are needed.

## Severity levels

### Green prompt

Allowed to run.

Requirements:

- one repo;
- one task;
- one run mode;
- named files/folders or a strict discovery budget;
- validation named or blocked reason named;
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
- no validation or blocked reason;
- no stop rules;
- asks to continue huge chat history;
- asks for dashboard/SaaS before gates allow it;
- asks for runtime features before Gate 0 validation.

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
Max docs before first action: 3
Max files to inspect: 8
Max files to edit: 3
Max broad searches: 2
Max validation commands: 3
Max commits: 1-2 small commits
```

### Medium budget

Use for:

- one feature slice;
- prompt optimizer expansion;
- git report end-to-end;
- one adapter plus tests.

Limits:

```text
Max docs before first action: 5
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
Max docs before first action: 8
Implementation edits: forbidden unless explicitly scoped
Summarize every 10 files inspected
Stop if a smaller prompt can continue the work
```

## Run modes

Use exactly one primary run mode:

```text
validation-only
investigation-only
implementation
tests
docs/evidence
review-only
diff-only review
```

Split the prompt if it needs more than one mode.

## Edit limits

Good edit shapes:

- one command handler;
- one parser;
- one formatter;
- one test class;
- one queue status update;
- one evidence file.

Bad edit shapes:

- change CLI, reports, config, adapters, and docs in one run;
- rewrite Program.cs and all services together;
- change roadmap while implementing runtime behavior;
- update many docs to hide missing validation;
- create feature code before Gate 0.

## Required prompt header

For medium/high-budget prompts:

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

For low-budget prompts, the header may be compact as long as the same information is present somewhere in the prompt.

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

## Final answer requirements

Low-budget runs may use compact final output:

```text
Prompt ID:
Files changed:
Validation:
Result:
Missed:
Next:
Risk:
```

Medium/high-budget runs should use full output:

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

If any field is unknown, write `unknown` and explain briefly.

## Token waste report

Use this only for non-trivial medium/high-budget runs:

```text
Files inspected:
Files changed:
Inspection/change ratio:
Broad searches used:
Prompt split used: yes/no
Handoff used: yes/no
Whole-repo review avoided: yes/no
Large logs avoided: yes/no
Largest waste avoided:
Next token-saving improvement:
```

Low-budget runs may summarize token waste in one line.

## Command-output rule

```text
Command profile summary beats command log paste.
```

Record duration, exit code, byte counts, and compact error signatures. Do not include full stdout/stderr unless a later explicit debug/export workflow requires it.

## AgentsWatch-specific override

Gate 0 is currently incomplete.

Until restore/build/test and CLI smoke evidence exist, the only safe implementation-adjacent prompt is:

```text
AW-VAL-001 — Build validation
```

All feature/productization prompts are blocked unless they are docs-only and do not claim runtime completion.