# AgentsWatch Roadmap Execution Queue

Last aligned: 2026-06-29  
Target repo: `ivanjovicic/AgentsWatch`

Purpose: execute the ultra roadmap in small, low-risk, token-optimized prompts.

## Read first

- `../../AGENTS.md`
- `../ULTRA_ROADMAP.md`
- `../90_DAY_EXECUTION_PLAN.md`
- `../ROADMAP_VALIDATION_GATES.md`
- `../BUILD_VALIDATION_PLAN.md`
- `../RISK_REGISTER.md`
- `bootstrap_validation.md`
- `agentwatch_mvp.md`

## Rules

- Bootstrap validation comes before feature expansion.
- Do not start dashboard/SaaS prompts until roadmap gates allow it.
- Every prompt must have run mode, token budget, scope limiter, stop rules, and validation.
- Prefer one command/output improvement per prompt.
- If a gate fails, write a fix prompt instead of skipping the gate.

---

## Active roadmap prompts

| ID | Status | Purpose |
|---|---|---|
| ROAD-001 | Ready | Complete Gate 0 bootstrap safety evidence. |
| ROAD-002 | Ready | Convert AW-VAL validation results into risk-register updates. |
| ROAD-003 | Ready | Harden CLI init after Gate 0 passes. |
| ROAD-004 | Ready | Implement start/finish run snapshot model after CLI foundation is safe. |
| ROAD-005 | Ready | Implement markdown run report writing. |
| ROAD-006 | Ready | Implement task split command with generated markdown prompts. |
| ROAD-007 | Ready | Improve prompt risk analyzer with more test cases. |
| ROAD-008 | Ready | Implement handoff summary command. |
| ROAD-009 | Ready | Implement diff-only review prompt command. |
| ROAD-010 | Ready | Add dogfood examples after first real run reports exist. |

---

## ROAD-001 — Gate 0 bootstrap safety evidence

Run mode: validation-only  
Token budget: low

Task: complete Gate 0 from `ROADMAP_VALIDATION_GATES.md`.

Owned docs:

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/prompt_queues/bootstrap_validation.md`

Required evidence:

- restore/build/test result;
- CLI smoke result;
- CI or validation evidence review;
- remaining bootstrap risks.

Do not add new product features.

---

## ROAD-002 — Validation results to risk register

Run mode: docs/evidence  
Token budget: low

Task: after validation runs, update `docs/RISK_REGISTER.md` with the real result.

Required update:

- mark resolved risks;
- add new risks discovered;
- add next prompt for unresolved risks;
- do not hide skipped validation.

---

## ROAD-003 — Init hardening

Run mode: implementation  
Token budget: low

Prerequisite: Gate 0 passed or blockers are documented.

Task: harden `agentswatch init`.

Required behavior:

- no overwrite by default;
- creates all expected folders/files;
- temp-directory tests;
- clear console output.

---

## ROAD-004 — Start/finish run snapshots

Run mode: implementation  
Token budget: medium

Task: implement start/end run snapshot capture.

Required behavior:

- write start metadata;
- write end metadata;
- capture branch, commit, changed files;
- handle clean worktree;
- handle untracked files.

---

## ROAD-005 — Markdown run reports

Run mode: implementation  
Token budget: medium

Task: write `.ai/runs/<id>.md` reports.

Required sections:

- task;
- mode;
- budget;
- files changed;
- validation;
- risk;
- missed;
- follow-up;
- token waste report.

---

## ROAD-006 — Task split command

Run mode: implementation  
Token budget: medium

Task: implement `agentswatch task split <prompt-file>`.

Required output:

- `001-investigate-only.md`;
- `002-implement-minimal-fix.md`;
- `003-add-tests.md`;
- `004-diff-only-review.md`.

---

## ROAD-007 — Prompt risk analyzer expansion

Run mode: implementation  
Token budget: low

Task: improve risk analyzer patterns and tests.

Add cases for:

- broad repo scans;
- missing stop rules;
- mixed modes;
- missing validation;
- long chat continuation;
- no scope limiter;
- review whole repo vs diff-only review.

---

## ROAD-008 — Handoff command

Run mode: implementation  
Token budget: low

Task: implement `agentswatch handoff` from latest run report.

Output:

```text
Task:
Status:
Relevant files:
Files changed:
Root cause:
Validation run:
Validation blocked:
Do not inspect next:
Next minimal prompt:
Residual risk:
```

---

## ROAD-009 — Diff-only review command

Run mode: implementation  
Token budget: low

Task: implement `agentswatch review-diff <commit-or-range>`.

Required behavior:

- read changed files;
- create review prompt;
- forbid whole-repo scan by default;
- include missed-test checklist.

---

## ROAD-010 — Dogfood examples

Run mode: docs/evidence  
Token budget: low

Task: add `examples/` after real reports exist.

Examples:

- rough prompt;
- optimized prompt split;
- run report;
- handoff summary;
- diff-only review prompt.
