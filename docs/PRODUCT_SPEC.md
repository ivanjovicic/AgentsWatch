# AgentsWatch Product Spec

Last aligned: 2026-07-02  
Status: planning/specification

## Description

AgentsWatch is an AI coding-agent supervisor and token optimizer for developers.

It does not replace Codex, Cursor, Claude Code, Copilot, or ChatGPT. It sits above them and helps developers run smaller, safer, cheaper coding-agent tasks.

## Core promise

```text
Spend fewer tokens. Merge safer AI code.
```

Practical claim:

```text
Reduce AI coding-agent token waste by 30-50% on typical multi-file tasks by splitting prompts, limiting scope, tracking diffs, and using compact handoff summaries.
```

Use stronger claims such as `up to 70%+` only for oversized repo-analysis prompts.

## Problem

AI coding agents often waste tokens and create risk because they:

- inspect too many files;
- repeat searches;
- repeat slow validation commands after small changes;
- paste large terminal logs back into model context;
- mix investigation, implementation, tests, docs, and review in one run;
- continue after the prompt should stop;
- edit unrelated files;
- claim tests were added when no test file changed;
- claim backend was changed when only frontend changed;
- rely on long chat history instead of handoff summaries.

## Target users

Primary:

- solo developers using Cursor, Codex, Claude, Copilot, or ChatGPT heavily;
- developers working across .NET, React, Flutter, Python, Node, or mixed repos;
- power users with usage limits or high AI spend;
- developers who want reviewable AI-agent history.

Later:

- small teams reviewing AI-assisted pull requests;
- maintainers who want AI run evidence;
- managers who want policy/risk visibility.

## Product layers

1. Local CLI — first product, works through git, markdown, shell commands, and local config.
2. Local dashboard — optional after CLI proves value.
3. Team/SaaS edition — later only after local usage is proven.

## MVP feature list

MVP 1:

- `.ai` folder generator;
- prompt optimizer;
- prompt splitter;
- scope limiter;
- git diff tracker;
- basic risk scoring;
- markdown run report;
- changelog generator.

MVP 2:

- acceptance-criteria checker;
- claimed-vs-actual diff checker;
- validation runner;
- handoff summary generator;
- token waste report;
- diff-only review prompt generator;
- command profiler / fast validation advisor.

## Command profiler principle

The Command Profiler / Fast Validation Advisor is a planned docs-first epic. It should help agents pick faster language-specific validation commands and avoid large terminal logs.

Rule:

```text
Profile commands locally. Show agents only compact command evidence.
```

See `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`.

## Commercial trial and licensing — post-MVP

AgentsWatch may later offer a permanent free tier plus a time-limited or usage-limited Pro trial.

Commercial protection must follow these truths:

- local files and generated output shown to the user cannot be made impossible to copy;
- premium implementation details should not be shipped as editable plaintext files when avoidable;
- commercial enforcement should use server-signed feature entitlements and a short-lived offline-capable lease;
- user source code, prompts, diffs, validation output, reports, and run history must remain local by default;
- license activation/refresh calls must be visible and must not upload repository content;
- expiration must never encrypt, delete, corrupt, or hold user-owned repository/report data hostage;
- licensing runtime work starts only after CLI MVP validation and dogfood evidence.

Recommended direction:

```text
small permanent free tier
+ 14-day or usage-limited Pro trial
+ server-signed lease
+ OS-protected local license storage
+ central feature entitlement gates
+ compiled/embedded premium local logic
+ optional remote premium algorithms only where stronger IP protection justifies the privacy/online tradeoff
```

See:

- `docs/TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- `docs/prompt_queues/agentwatch_trial_licensing.md`

## Non-goals for v1

Do not start with:

- SaaS;
- billing;
- runtime license enforcement or DRM before CLI value is proven;
- cloud sync;
- deep IDE integration;
- automatic code editing;
- perfect token counting;
- perfect AI acceptance-criteria inference;
- uploading command logs or run history by default.
