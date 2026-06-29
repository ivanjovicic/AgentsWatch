# AgentsWatch Risk Scoring Model

Last aligned: 2026-06-29  
Status: draft scoring contract

## Purpose

Define transparent risk scoring before implementation.

AgentsWatch risk scoring must be explainable. The MVP should not use opaque AI scoring.

---

## Risk levels

### Low

Use when:

- one or two low-risk files changed;
- docs-only or test-only change;
- validation evidence exists;
- no sensitive/config/build files touched;
- scope matches prompt.

### Medium

Use when:

- multiple source files changed;
- validation is partial;
- CLI behavior changed;
- adapter detection changed;
- report format changed;
- tests were not updated but runtime behavior changed.

### High

Use when:

- build/project/solution files changed;
- CI workflow changed;
- config/secrets patterns changed;
- command execution behavior changed;
- file-system write behavior changed;
- validation is missing;
- more than 20 files changed;
- final claims do not match actual diff.

---

## Scoring inputs

Inputs:

- changed file paths;
- file statuses;
- validation status;
- prompt run mode;
- token budget;
- claimed outcome;
- actual diff categories;
- test file changes;
- sensitive file patterns.

---

## File category rules

| Category | Examples | Default risk |
|---|---|---|
| Docs only | `docs/**`, `README.md` | Low |
| Tests only | `tests/**` | Low/Medium |
| CLI command | `src/AgentsWatch.Cli/**` | Medium |
| Git execution | `src/AgentsWatch.Git/**` | High |
| Reports | `src/AgentsWatch.Reports/**` | Medium |
| Config parser | config-related source | High |
| CI/build | `.github/**`, `*.sln`, `*.csproj` | High |
| Security/privacy | secret patterns, redaction | High |

---

## Validation modifiers

- Passing targeted validation can reduce risk by one level.
- Missing validation keeps Medium or High risk unchanged.
- Failed validation should usually be High until fixed.
- Environment-blocked validation must be recorded, not hidden.

---

## Claims-vs-actual modifiers

Raise risk when:

- tests claimed but no test files changed;
- validation claimed but no evidence exists;
- docs claimed but no docs changed;
- runtime fix claimed but only docs changed;
- no runtime change claimed but source files changed.

---

## Output format

```text
Risk: Medium
Reasons:
- CLI command file changed
- validation not run
- no test file changed
Required follow-up:
- add targeted command test
```

---

## MVP rule

Always show risk reasons. Never output only a numeric score.
