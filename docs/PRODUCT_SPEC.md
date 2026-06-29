# AgentsWatch Product Spec

Last aligned: 2026-06-29  
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
- diff-only review prompt generator.

## Non-goals for v1

Do not start with:

- SaaS;
- billing;
- cloud sync;
- deep IDE integration;
- automatic code editing;
- perfect token counting;
- perfect AI acceptance-criteria inference.
