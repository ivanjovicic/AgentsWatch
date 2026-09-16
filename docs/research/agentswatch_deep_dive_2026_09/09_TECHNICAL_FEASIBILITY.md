# Technical Feasibility Audit

## Current architecture

The existing project split is sufficient for the MVP and does not require a rewrite:
- `AgentsWatch.Cli`
- `AgentsWatch.Core`
- `AgentsWatch.Git`
- `AgentsWatch.LanguageAdapters`
- `AgentsWatch.Reports`

The main gap is implementation maturity, not architecture.

## Difficulty by phase

| Phase | Difficulty | Main risk |
|---|---|---|
| Gate 0 parser/smoke | low | current parser bug |
| RunContract v1 | low-medium | schema/UX friction |
| Start baseline | medium | lossless index/worktree/untracked capture |
| Finish interval delta | medium-high | changed-further/rename/ambiguity semantics |
| RunReceipt v1 | medium | stable schema and provenance |
| Validation evidence | medium | trusted source/import semantics |
| Scope drift | medium | path/glob/cross-platform correctness |
| Deterministic claims | medium | narrow claims easy; semantic claims not |
| 30-run dogfood | medium | experiment quality more than coding |

## Current Gate 0

The Git parser bug is straightforward but important: current parsing trims significant porcelain whitespace before slicing path/status fields.

Preferred direction:
- machine-safe NUL-delimited porcelain, e.g. `git status --porcelain=v1 -z -uall`;
- preserve index/worktree status exactly;
- avoid fixed-width parsing after string trimming;
- tests for spaces, rename, staged/unstaged, untracked, dirty-at-start and changed-further cases.

This is necessary correctness work, **not moat**.

## Important technical boundary

Do not infer causal authorship that repository evidence cannot prove.

When concurrent writers or background processes make authorship uncertain, the correct result is `Ambiguous`.

## What is not required for MVP

- hosted service;
- database;
- message broker;
- microservices;
- LLM provider;
- vector database;
- background agent runtime;
- generic telemetry backend.

## Technical moat assessment

Engineering difficulty alone is not a moat. It becomes defensible only if reliable attribution/evidence semantics are trusted, adopted and integrated into real team workflows.
