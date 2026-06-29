# AgentsWatch Agent Patch Playbook

Last aligned: 2026-06-29

## Purpose

Keep file edits small, safe, and reviewable.

AgentsWatch currently has many docs contracts and an unverified CLI skeleton. Broad rewrites are risky until Gate 0 is complete.

## Patch rules

1. Read the current file segment before editing.
2. Prefer one small file or one section per commit.
3. Use stable anchors such as headings, type names, or command names.
4. Do not replace a whole file unless the file is short and fully owned by the prompt.
5. Re-read after patching if the change is important.
6. Stop after two failed patch attempts and write a handoff.

## Runtime code rule

Before Gate 0 passes:

- docs/evidence changes are allowed;
- build/test fixes are allowed;
- new product features are blocked;
- broad refactors are blocked.

After Gate 0 passes:

- change the smallest command/service needed;
- add targeted tests;
- avoid touching unrelated CLI commands.

## Good patch candidates

- one docs section;
- one test file;
- one parser method;
- one CLI command handler;
- one report formatter;
- one adapter detector.

## Bad patch candidates

- rewrite all CLI commands;
- move all projects;
- redesign report formats while implementing commands;
- change config, report, data model, and command behavior in one run;
- update docs and runtime broadly without validation.

## Before commit

Check:

```bash
git diff --stat
git diff --check
```

Then run the narrowest relevant validation.

## Handoff after failed patch

```text
Prompt:
File:
Anchor tried:
Why patch failed:
Current diff:
Smallest next edit:
Validation status:
```
