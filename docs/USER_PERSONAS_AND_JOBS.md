# AgentsWatch User Personas and Jobs To Be Done

Last aligned: 2026-06-29  
Status: product discovery draft

## Purpose

Clarify who AgentsWatch is for and what job it helps them complete.

---

## Persona 1 — Solo AI-heavy developer

Profile:

- uses Cursor, Codex, Claude Code, Copilot, ChatGPT;
- works on side projects or client projects;
- hits usage limits or worries about token spend;
- wants agents to stop wandering through the repo.

Jobs:

- split broad prompts into smaller runs;
- reduce repeated context;
- keep handoffs short;
- see what the agent actually changed;
- avoid missing tests.

Success moment:

```text
I pasted a messy prompt, AgentsWatch split it into safe tasks, and the next agent run touched only the right files.
```

---

## Persona 2 — Senior developer reviewing AI work

Profile:

- reviews AI-assisted PRs;
- cares about tests, scope creep, and risk;
- does not want to read huge chat logs.

Jobs:

- compare final agent claims with actual diff;
- see changed files and risk reasons;
- detect test/validation mismatch;
- generate diff-only review prompt;
- decide whether a PR is safe to merge.

Success moment:

```text
I spotted that the agent claimed tests were added, but no test files changed.
```

---

## Persona 3 — Consultant/freelancer using AI for client work

Profile:

- works across multiple repos/stacks;
- needs audit trail for client confidence;
- cannot upload private code to random services.

Jobs:

- keep local run reports;
- avoid cloud sync by default;
- produce evidence of validation;
- make handoff notes for next session;
- prove what changed without exposing secrets.

Success moment:

```text
I can send a clean report of what changed, what was tested, and what risk remains.
```

---

## Persona 4 — Small team adopting AI agents

Profile:

- multiple developers use AI agents;
- team wants consistency;
- PRs need policy checks.

Jobs:

- standardize prompts;
- standardize validation evidence;
- flag high-risk files;
- create PR risk reports;
- enforce “no Done without evidence”.

Success moment:

```text
Every AI-assisted PR has the same risk/evidence checklist.
```

---

## Jobs To Be Done summary

When I use an AI coding agent, I want to:

1. turn a broad prompt into small safe prompts;
2. limit which files the agent reads and edits;
3. capture what actually changed;
4. verify tests and validation honestly;
5. generate a short handoff;
6. review only the diff;
7. avoid leaking secrets;
8. understand token waste and scope creep.

---

## Product implication

The first product should not be a dashboard.

The first product should be a local CLI that creates:

- optimized prompts;
- scoped task files;
- run reports;
- handoff summaries;
- diff-only review prompts;
- claims-vs-actual findings.
