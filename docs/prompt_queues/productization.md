# AgentsWatch Productization Prompt Queue

Last aligned: 2026-06-29  
Target repo: `ivanjovicic/AgentsWatch`

Purpose: turn productization docs into small, safe implementation and evidence tasks.

## Read first

- `../../AGENTS.md`
- `../DOCS_INDEX.md`
- `../COMMAND_CONTRACTS.md`
- `../CLI_UX_OUTPUT_SPEC.md`
- `../MVP_EPICS_AND_ACCEPTANCE.md`
- `../ISSUE_BACKLOG.md`
- `../ROADMAP_VALIDATION_GATES.md`

## Rules

- Gate 0 validation still comes first.
- Do not implement feature work until build/test/smoke evidence exists.
- Each prompt must map to one command, one report format, or one evidence artifact.
- Prefer tests before broad refactor.
- Keep dashboard/SaaS blocked until dogfood evidence exists.

---

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| PROD-001 | Ready after Gate 0 | Align `--help` output with `CLI_UX_OUTPUT_SPEC.md`. |
| PROD-002 | Ready after Gate 0 | Add temp-directory tests for `agentswatch init`. |
| PROD-003 | Ready after Gate 0 | Make `status` handle non-git directories gracefully. |
| PROD-004 | Ready after Gate 0 | Expand prompt optimizer tests from `MVP_EPICS_AND_ACCEPTANCE.md`. |
| PROD-005 | Ready after Gate 0 | Implement token waste report fields in run report model. |
| PROD-006 | Ready after Gate 0 | Add claims-vs-actual mismatch tests. |
| PROD-007 | Ready after Gate 0 | Add first dogfood evidence example. |
| PROD-008 | Ready after Gate 0 | Add release packaging evidence after `dotnet pack`. |

---

## PROD-001 — Help output UX alignment

Run mode: implementation  
Token budget: low

Task: align CLI `--help` output with `docs/CLI_UX_OUTPUT_SPEC.md`.

Owned paths:

- `src/AgentsWatch.Cli/`
- CLI smoke tests if available

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

Stop if build/test status is still unknown.

---

## PROD-002 — Init temp-directory tests

Run mode: tests  
Token budget: low

Task: add tests proving `agentswatch init` creates expected files and does not overwrite existing files.

Owned paths:

- `tests/AgentsWatch.Tests/`
- command extraction only if required for testability

---

## PROD-003 — Status non-git behavior

Run mode: implementation  
Token budget: low

Task: make `agentswatch status` show a clear message outside git repos.

Expected UX anchor:

```text
Git: not detected
```

Do not add external integrations.

---

## PROD-004 — Prompt optimizer test expansion

Run mode: tests  
Token budget: low

Task: add tests for broad scope, mixed modes, missing validation, missing stop rules, missing scope limiter, and long chat continuation.

---

## PROD-005 — Token waste report fields

Run mode: implementation  
Token budget: medium

Task: add token waste report fields to report contracts/model, following `docs/TOKEN_WASTE_METRICS.md`.

---

## PROD-006 — Claims-vs-actual tests

Run mode: tests  
Token budget: medium

Task: add tests for claims mismatch detection.

Cases:

- tests claimed but no test files changed;
- validation claimed without evidence;
- runtime fix claimed but only docs changed.

---

## PROD-007 — First dogfood evidence

Run mode: docs/evidence  
Token budget: low

Task: add first real dogfood evidence after one validated run.

Use `docs/DOGFOOD_RUNBOOK.md`.

---

## PROD-008 — Packaging evidence

Run mode: validation/evidence  
Token budget: low

Task: after build/test pass, run pack/local tool install and record evidence.

Use `docs/RELEASE_AND_PACKAGING_PLAN.md`.
