# AgentsWatch Dogfood Runbook

Last aligned: 2026-06-29  
Status: operational dogfood guide

## Purpose

Define exactly how to dogfood AgentsWatch after Gate 0 passes.

Dogfood should produce evidence, not just opinions.

---

## Prerequisite

Do not run dogfood as product proof until:

- build/test evidence exists;
- CLI smoke evidence exists;
- `init`, `status`, and `optimize` work enough for manual use.

---

## Dogfood run steps

1. Choose one real repo task.
2. Save the rough prompt.
3. Run or manually apply prompt optimization.
4. Save generated split prompts.
5. Run an AI agent with the investigation prompt.
6. Record files inspected if known.
7. Run implementation prompt if investigation gives a clear root cause.
8. Record changed files.
9. Record validation.
10. Generate handoff summary.
11. Generate diff-only review prompt.
12. Record what AgentsWatch caught or prevented.

---

## Dogfood evidence file

Save under:

```text
examples/dogfood/<yyyy-mm-dd>-<repo>-<task>.md
```

Template:

```text
Repo:
Task:
Raw prompt:
Risk before:
Optimized split:
Files inspected:
Files changed:
Validation run:
Validation skipped:
Handoff reused:
Issue caught:
Scope creep prevented:
Token waste note:
Follow-up:
```

---

## Good first dogfood tasks

Use small tasks first:

- docs broken-reference audit;
- CLI help output improvement;
- init no-overwrite hardening;
- git parser edge-case test;
- report format test.

Avoid first:

- dashboard;
- GitHub integration;
- SaaS;
- major refactor;
- multi-repo implementation.

---

## Success criteria

AgentsWatch dogfood is useful when it produces:

- a better prompt than the raw prompt;
- a smaller task split;
- a run report that explains actual changes;
- a handoff that can replace chat history;
- a diff-only review prompt;
- at least one concrete risk or waste insight.
