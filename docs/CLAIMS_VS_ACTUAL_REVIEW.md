# AgentsWatch Claims vs Actual Review

Last aligned: 2026-07-03

## Purpose

Check whether agent, documentation, release, and product claims match the actual repository diff and commit-bound proof.

Never use final chat text as proof by itself.

## Inputs

- claim text;
- capability ID;
- required maturity;
- changed files/diff stat;
- implementation path;
- targeted test results;
- black-box scenario results;
- CI/proof bundle;
- dogfood evidence for usefulness claims;
- release/package evidence when applicable.

## Review checklist

| Claim | Evidence to check |
|---|---|
| tests added | test files changed and IDs map to acceptance criteria |
| tests pass | TRX/CI result for same commit |
| docs updated | owning docs changed and links valid |
| CLI command changed | CLI/core implementation path changed |
| command works | black-box process scenario passes |
| git parsing changed | Git implementation + parser/integration tests |
| report format changed | report code/docs + golden result |
| validation passed | exact command/CI evidence exists |
| no runtime change | only docs/templates/evidence changed |
| risk reduced | regression/safety proof or explicit evidence exists |
| capability supported | registry/matrix maturity meets required level |
| cross-platform | required scenario passes on every named OS |
| package works | pack + checksum + isolated install smoke |
| token/time saved | measured paired dogfood benchmark with quality guardrail |
| privacy/local-first | dedicated negative tests/audit, not architecture statement alone |

## Maturity review

```text
L0 Idea
L1 Specified
L2 Implemented
L3 Test-backed
L4 CI-verified
L5 Dogfood-verified
L6 Release-verified
```

A claim must not use stronger wording than the current capability level.

Examples:

```text
Claim: init is implemented
Actual: CLI code path exists
Result: L2 match, not verified
```

```text
Claim: init is safe and idempotent
Actual: no temp-directory/no-overwrite execution evidence
Result: mismatch; requires integration and black-box proof
```

```text
Claim: tests pass
Actual: test source exists, no result for current commit
Result: mismatch
```

```text
Claim: reduces tokens by 40%
Actual: one anecdotal dogfood run and estimated values
Result: unsupported; use directional wording only
```

## Output format

```text
Claim:
Capability ID:
Required maturity:
Available maturity:
Actual evidence:
Commit match: yes/no
Result: Match | Partial | Mismatch
Risk:
Allowed replacement wording:
Required follow-up/discovery:
```

## Rules

- Evidence must match the claimed commit/release.
- Test source is not test execution proof.
- Green CI is not proof for a capability unless relevant tests/scenarios are linked.
- Failed/skipped stages remain visible.
- Docs-only work cannot claim runtime behavior.
- Value claims require benchmark evidence.
- Release claims require proof bundle and independent verification.
