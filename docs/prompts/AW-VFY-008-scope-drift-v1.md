# AW-VFY-008 — Scope Drift v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-007  
Run mode: implementation  
Budget: medium  
Gate: Evidence Gate proven

## Read only

- `AGENTS.md`
- `docs/PRODUCT_SPEC.md` — Scope Drift
- `docs/DATA_MODEL.md` — RunDelta/Finding
- `docs/COMMAND_CONTRACTS.md` — `drift check`
- current path/contract/receipt code and direct tests

## Task

Implement deterministic scope verification against RunContract path boundaries using **run-attributable changes**, not raw final repository dirtiness.

## Required checks

- attributable path outside `ownedPaths`;
- attributable path matching `avoidPaths`;
- rename where old/new path crosses boundaries;
- path matching behavior on Windows/Unix separators;
- attribution ambiguity that affects scope => explicit `NeedsReview`/ambiguity finding rather than a guessed drift result.

## Critical correctness rule

A file dirty before `start` and unchanged during the run must not create scope drift for the current run.

A pre-existing file changed further during the run must be evaluated according to its attributable delta/context and surfaced clearly.

## Path semantics

Use one explicit path-normalization/glob policy and test it. Do not scatter ad-hoc string-prefix checks across commands.

The MVP does not need an enterprise policy engine. It needs predictable repository-relative path matching.

## Output

Implement `agentswatch drift check <run-id>` or reusable equivalent and persist findings in the canonical receipt when appropriate.

Each finding must include:

```text
rule/category
exact path (and oldPath for rename when relevant)
matched owned/avoid pattern or missing ownership reason
attribution context
severity/status
```

## Owned paths

- `src/AgentsWatch.Core/**`
- minimal reusable path-matching utility location
- `src/AgentsWatch.Cli/**` for command wiring
- receipt/report code only where findings are projected
- `tests/AgentsWatch.Tests/**`

## Avoid

- claims verification;
- broad security policy engine;
- generic scoring system;
- LLM path classification;
- dashboard/MCP.

## Required tests

- inside owned path -> no drift;
- outside owned path -> drift;
- avoid path touched -> finding;
- pre-existing unchanged outside-owned file -> no run drift;
- pre-existing changed-further outside-owned file -> correct finding/context;
- ambiguous attribution -> review finding;
- rename inside->outside / outside->inside as applicable;
- slash normalization;
- overlapping patterns/edge cases defined by chosen matching policy.

## Validation

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Smoke one scoped contract/run with an intentional unrelated file change.

## Expected evidence

- path matching semantics;
- rule IDs and finding examples;
- dirty-at-start regression proof;
- test/smoke results;
- full validation result;
- known path-pattern limitations.
