# AgentsWatch Roadmap Validation Gates

Last aligned: 2026-06-29  
Status: roadmap control document

## Purpose

This document prevents AgentsWatch from growing too fast in the wrong direction.

Each phase must pass its gate before the next phase starts.

---

## Gate 0 — Bootstrap safety

Required before CLI feature expansion.

Must have:

- solution restore/build/test verified;
- CLI smoke verified;
- CI evidence checked;
- bootstrap risks updated;
- no unresolved high-risk build failures.

Blocked if:

- `.sln` or project references are broken;
- tests do not run;
- CLI entry point fails;
- CI failure root cause is unknown.

---

## Gate 1 — CLI foundation

Required before token optimizer expansion.

Must have:

- `init` safe and tested;
- `status` works in a git repo;
- `optimize` works for a rough prompt;
- reports folders are created safely;
- no user files are overwritten accidentally.

Blocked if:

- file-system writes are unsafe;
- commands only work on one OS path style;
- CLI has no test coverage;
- command failures are unclear.

---

## Gate 2 — Prompt optimizer value

Required before dashboard planning.

Must have:

- broad prompts are classified as high risk;
- prompt splitter creates 3-4 useful tasks;
- generated prompts include run mode, budget, scope limiter, stop rules, and validation;
- handoff summary format is useful in a real follow-up prompt;
- diff-only review prompt prevents whole-repo review.

Blocked if:

- output prompts are generic;
- risk reasons are not explainable;
- user still needs to rewrite every generated prompt manually;
- token waste report has no concrete signals.

---

## Gate 3 — Git evidence trust

Required before team/PR workflow.

Must have:

- changed files are parsed correctly;
- run reports show start/end state;
- high-risk files are flagged;
- tests changed/not changed is detected;
- claimed-vs-actual mismatch is detected;
- validation status is recorded honestly.

Blocked if:

- report can claim success without evidence;
- renamed/deleted/untracked files are mishandled;
- runtime changes without tests are not visible;
- high-risk categories are hidden.

---

## Gate 4 — Dogfood evidence

Required before dashboard implementation.

Must have:

- at least 5 real run reports;
- at least 2 real repositories used;
- at least 2 handoff summaries reused;
- at least 1 missed-test or scope-creep issue caught;
- developer can explain the time/token savings qualitatively.

Blocked if:

- tool is only demo data;
- reports are too noisy to read;
- handoffs are not reused;
- user does not trust the risk output.

---

## Gate 5 — Dashboard decision

Required before building local dashboard.

Start dashboard only if:

- CLI run reports are useful;
- user wants visual history;
- markdown output is becoming hard to browse;
- local SQLite model is stable enough.

Do not start dashboard if:

- CLI commands are unstable;
- reports are still changing heavily;
- no real dogfood data exists;
- dashboard would delay core CLI value.

---

## Gate 6 — Team/SaaS decision

Required before cloud/team work.

Start team/SaaS only if:

- local CLI has regular real usage;
- PR review use case is clear;
- users want shared history or policy checks;
- pricing hypothesis is validated.

Do not start team/SaaS if:

- no one uses the CLI manually;
- token savings are not visible;
- risk reports are not trusted;
- GitHub integration is being built only because it sounds exciting.

---

## Per-feature done checklist

Every feature must include:

- one clear user value;
- one command or output changed;
- tests or explicit reason tests are not applicable;
- risk level;
- validation evidence;
- docs update if behavior changed;
- next follow-up prompt if incomplete.

---

## Roadmap stop rule

Stop roadmap expansion if the next idea does not improve one of these:

- lower token waste;
- safer AI code review;
- clearer run evidence;
- faster handoff to next agent;
- better scoped prompts;
- more trustworthy validation.
