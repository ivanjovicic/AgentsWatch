# AgentsWatch Issue-Ready Backlog

Last aligned: 2026-06-29  
Status: issue creation source

## Purpose

Provide issue-sized tasks that can become GitHub issues, PRs, or agent prompts.

Each item should stay small enough for one focused run.

---

## Gate 0 / Bootstrap

### ISSUE-001 — Verify build/test

Labels: `validation`, `bootstrap`, `high-priority`

Task:

- run restore/build/test;
- fix only build/test failures;
- record evidence.

Acceptance:

- validation evidence file updated;
- no feature work included.

### ISSUE-002 — Verify CLI smoke

Labels: `validation`, `cli`, `bootstrap`

Task:

- run help/version/optimize/status smoke checks;
- fix only smoke failures;
- record evidence.

Acceptance:

- CLI smoke evidence recorded;
- follow-up for any behavior gap.

---

## CLI foundation

### ISSUE-010 — Harden init no-overwrite behavior

Labels: `cli`, `filesystem`, `tests`

Task:

- make init idempotent;
- preserve existing files;
- add temp-directory tests.

Acceptance:

- tests prove no overwrite;
- output lists created/preserved paths.

### ISSUE-011 — Extract init command from Program.cs

Labels: `cli`, `architecture`

Task:

- move init behavior into dedicated command class;
- keep CLI output unchanged;
- add tests if extraction changes behavior.

Acceptance:

- `Program.cs` command dispatch stays simple;
- init tests still pass.

### ISSUE-012 — Status command non-git handling

Labels: `cli`, `git`, `ux`

Task:

- handle non-git folder gracefully;
- show clear status note;
- do not throw raw exception.

Acceptance:

- non-git test exists;
- UX output includes `Git: not detected` or equivalent.

---

## Prompt optimizer

### ISSUE-020 — Expand prompt risk analyzer cases

Labels: `prompt-optimizer`, `tests`

Task:

- add tests for broad scan, mixed modes, missing validation, missing stop rules, long chat continuation.

Acceptance:

- each case maps to transparent waste cause;
- high-risk broad prompts recommend split.

### ISSUE-021 — Generate scope limiter block

Labels: `prompt-optimizer`, `ux`

Task:

- generate inspect/avoid/do-not-edit lists;
- allow placeholders when paths are unknown.

Acceptance:

- optimized prompt includes scope limiter;
- no invented repo-specific paths.

### ISSUE-022 — Generate task split files

Labels: `prompt-optimizer`, `filesystem`

Task:

- implement `task split` output;
- avoid overwrite by default.

Acceptance:

- four markdown prompts created;
- tests check filenames and required sections.

---

## Git evidence and reports

### ISSUE-030 — Harden git status parser

Labels: `git`, `tests`

Task:

- support modified, added, deleted, renamed, copied, and untracked status.

Acceptance:

- parser tests cover each status;
- renamed paths are represented clearly.

### ISSUE-031 — Run report writer end-to-end

Labels: `reports`, `filesystem`

Task:

- write `.ai/runs/<run-id>.md`;
- include validation, risk, missed, follow-up.

Acceptance:

- report matches `REPORT_FORMATS.md`;
- test uses temp directory.

### ISSUE-032 — Handoff summary generator command

Labels: `handoff`, `reports`

Task:

- generate compact handoff from latest report.

Acceptance:

- 10-20 line target;
- includes next prompt and residual risk.

---

## Claims vs actual

### ISSUE-040 — Detect tests-claimed mismatch

Labels: `claims-vs-actual`, `risk`

Task:

- if final claim says tests were added but no test files changed, flag mismatch.

Acceptance:

- test covers mismatch;
- finding includes follow-up prompt.

### ISSUE-041 — Detect validation claim without evidence

Labels: `claims-vs-actual`, `validation`

Task:

- flag claims of validation pass without command output or CI status.

Acceptance:

- report records `validation evidence missing`.

---

## Packaging and dogfood

### ISSUE-050 — Pack CLI as local tool

Labels: `packaging`, `release`

Task:

- run `dotnet pack`;
- document local install command;
- record evidence.

Acceptance:

- pack output exists;
- install instructions verified or risk documented.

### ISSUE-051 — Dogfood on AgentsWatch

Labels: `dogfood`, `evidence`

Task:

- run one real AgentsWatch-assisted workflow on this repo;
- save run report, handoff, and review prompt.

Acceptance:

- example report exists;
- missed/token-waste notes recorded.

### ISSUE-052 — Dogfood on MathLearning

Labels: `dogfood`, `cross-repo`

Task:

- use AgentsWatch workflow on a MathLearning prompt;
- collect report and handoff.

Acceptance:

- evidence saved under examples;
- value/risk notes recorded.
