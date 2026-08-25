# AgentsWatch Bootstrap Next Steps

Last aligned: 2026-08-25

## Current evidence

The skeleton is partially validated by GitHub Actions on `main`.

Latest known evidence:

```text
restore: PASS
build: PASS (0 warnings, 0 errors)
test: FAIL
```

Known failing test:

```text
AgentsWatch.Tests.GitStatusParserTests.Parse_ParsesModifiedAndUntrackedFiles
```

Known root cause:

- `git status --short` relies on fixed porcelain columns;
- `GitStatusParser.Parse` uses `StringSplitOptions.TrimEntries`;
- the leading status-column space is removed before `line[3..]` path slicing;
- ` M README.md` becomes `M README.md`, then path parsing returns `EADME.md`.

This means Gate 0 is not an unknown build-validation problem anymore. It is a known parser/test failure plus remaining CLI smoke validation.

## Required next order

1. `AW-VFY-001` — fix and harden git porcelain parsing; rerun restore/build/test.
2. `AW-VFY-002` — run CLI smoke and close Gate 0 if clean.
3. `AW-VFY-003` — implement RunContract v1.
4. continue the canonical verification queue.

Queue:

`docs/prompt_queues/verification_mvp_2026_08_25.md`

## Gate 0 definition of done

Gate 0 closes only when:

- restore passes;
- build passes;
- tests pass;
- CLI help/version/init/optimize/status smoke is recorded;
- init writes only expected local files;
- non-git/status behavior is either proven or explicitly queued as a blocker.

## Do not do before Gate 0 closes

Do not implement:

- RunContract runtime commands;
- start/finish lifecycle;
- receipts;
- evidence/drift/claims gates;
- MCP/dashboard/SaaS.

Docs-only alignment is already allowed and should use the verification-first canonical documents.

## Obsolete bootstrap instruction

The previous generic instruction to run `AW-VAL-001` as if no build evidence existed is superseded by `AW-VFY-001`.

Historical AW-VAL prompt files may remain for audit/history, but the prompt router must not select them as the current next task.
