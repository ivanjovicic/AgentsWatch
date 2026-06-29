# AgentsWatch Claims vs Actual Review

Last aligned: 2026-06-29

## Purpose

Check whether an agent's final claims match the actual repository diff and validation evidence.

This is a core AgentsWatch feature and should also be used manually during development.

## Inputs

- final agent response;
- changed files;
- git diff stat;
- validation output;
- prompt evidence row;
- run report if available.

## Review checklist

| Claim | Evidence to check |
|---|---|
| tests added | files under `test/` or `tests/` changed |
| docs updated | files under `docs/` changed |
| CLI command changed | `src/AgentsWatch.Cli/` changed |
| git parsing changed | `src/AgentsWatch.Git/` changed |
| report format changed | `src/AgentsWatch.Reports/` or `docs/REPORT_FORMATS.md` changed |
| adapter behavior changed | `src/AgentsWatch.LanguageAdapters/` or `docs/ADAPTER_SPEC.md` changed |
| validation passed | command output or CI status exists |
| no runtime change | only docs/templates changed |
| risk reduced | risk register or evidence file changed |

## Mismatch examples

```text
Claim: tests added
Actual: no test files changed
Finding: missing-test claim mismatch
```

```text
Claim: build validated
Actual: no build output and no CI status
Finding: validation evidence missing
```

```text
Claim: init hardened
Actual: docs changed only, no CLI/tests changed
Finding: implementation not present
```

## Output format

```text
Claim reviewed:
Actual evidence:
Match: yes/no/partial
Risk:
Required follow-up:
```

## Rule

Never use final chat text as proof by itself. Evidence must come from diff, tests, CI, reports, or a dated validation evidence file.
