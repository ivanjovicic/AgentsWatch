# Repository and CI State

Research cut-off: **2026-09-16**

## Current main

Audited `main` SHA:

`cfd2c7f87f052e61d7fd5fa11651f92a60803929`

Current executable remains a small .NET 8 prototype. Implemented CLI commands are:

- `init`
- `optimize`
- `status`
- help/version

The commercially relevant verification commands (`contract`, `start`, `finish`, `receipt`, `evidence`, `drift`, `claims`) are specified but not implemented.

## Latest CI truth

GitHub Actions run `35087436507` on the audited main SHA:

- restore: PASS
- Release build: PASS
- tests: FAIL
- total tests: 7
- passed: 6
- failed: 1

Failing test:

`AgentsWatch.Tests.GitStatusParserTests.Parse_ParsesModifiedAndUntrackedFiles`

Expected `README.md`; actual `EADME.md`.

## Root cause confirmed in current source

`GitStatusParser.Parse` splits using `StringSplitOptions.TrimEntries`. A porcelain line such as:

` M README.md`

loses the meaningful leading status space and becomes:

`M README.md`

`ParseLine` then slices `line[3..]`, dropping the first path character.

This is a direct code/CI observation, not a documentation assumption.

## Implemented vs planned

### Implemented
- process-based Git command execution;
- branch/HEAD/status snapshot;
- simple changed-file parser;
- project-type/validation suggestions;
- legacy prompt optimizer;
- init/status CLI.

### Planned
- RunContract v1;
- start baseline;
- run-interval delta;
- RunReceipt v1;
- ValidationEvidence;
- Evidence Gate;
- Scope Drift;
- claims checks;
- 30-run dogfood;
- external/commercial gates.

### Still hypotheses
- external teams need a separate verifier;
- vendor-neutrality is monetizable;
- run receipts change review decisions;
- dirty-state attribution is frequent enough to matter commercially;
- OSS/GitHub distribution works.

## Additional stale implementation wording

Current CLI help still says:

`AgentsWatch — AI coding-agent supervisor and token optimizer`

That wording conflicts with the current canonical verification-first product definition. It should be corrected in an authorized implementation/docs synchronization task, but this research commit does not modify production code.

## Conclusion

Gate 0 is genuinely open. Any market analysis that treats the verification spine as already built is wrong.
