# AgentsWatch Test and Proof Matrix

Last aligned: 2026-07-03

## Purpose

Keep validation focused on the highest-risk behavior while proving each capability through the correct combination of tests, scenarios, CI, dogfood, and release evidence.

Use with:

- `PROOF_AND_VERIFICATION_STRATEGY.md`;
- `FEATURE_CAPABILITY_REGISTRY.md`;
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`;
- `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`.

## Required coverage

| Area / capability | Unit/fixture | Integration/golden | Black-box/safety | Current proof gap |
|---|---|---|---|---|
| Help/version AW-CAP-001/002 | output/version contract | CLI process | help/version/unknown scenarios | CI run result needed |
| Init AW-CAP-003 | template rules | temp-dir idempotency/no-overwrite | path escape/outside-write | tests not implemented |
| Risk/optimize AW-CAP-004/005/006 | broad/scoped/boundary | stable output golden | optimize process/input errors | unit source exists; execution/goldens missing |
| Git/status AW-CAP-007/008 | parser states | temp git repos | non-git/path spaces/binary | limited unit source; integration missing |
| Project detection AW-CAP-009/010 | stack fixtures | mixed repo | deterministic suggestions/no auto-exec | .NET/Flutter test source only |
| Start/finish AW-CAP-012/013 | evidence rules | temp repo lifecycle | missing-evidence and NotRun scenarios | not implemented |
| Reports/handoff/review AW-CAP-014/015/016 | formatter | golden markdown/JSON | no full diff/log, length budget | not implemented |
| Validation/profile AW-CAP-017/018 | command classification | controlled process | fail/timeout/cancel/secret output | not implemented |
| Claims/evidence AW-CAP-019/021 | deterministic rules | bad/good fixture suite | false-proof regression scenarios | manual/spec only |
| Mistake learning AW-CAP-020 | parser/dedup/rollup | fixture logs | repeated mistake prevention gate | manual/spec only |
| Discovery AW-CAP-022/023 | record/parser/routing | run-to-record lifecycle | duplicate/security/closure scenarios | manual/spec only |
| Packaging AW-CAP-025 | package metadata | pack/install | checksum + isolated help/version | CI workflow added; result needed |
| Privacy AW-CAP-026 | redaction/path rules | fixture repositories | no-network/outside-write/secret/binary | dedicated suite missing |
| Proof bundle AW-CAP-027 | schema/maturity rules | artifact validation | commit/checksum/claim mismatch | CI generation added; validator missing |

## Test identifiers

Use:

```text
AW-UT-*    unit
AW-IT-*    integration
AW-GOLD-*  golden
AW-SAFE-*  safety/privacy
AW-REG-*   regression
AW-SCN-*   black-box scenario
AW-DOG-*   dogfood benchmark
```

Include capability/acceptance IDs in test metadata or names where practical.

## Default validation

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln --configuration Release --no-restore
dotnet test AgentsWatch.sln --configuration Release --no-build
```

For current CLI changes also run matching black-box scenarios.

## Rules

- Use temp directories for file-system tests.
- Do not write to the real user home/repository.
- Unit tests do not require network or provider credentials.
- Keep fixtures tiny and synthetic.
- Test source without an executed result does not establish L3.
- Coverage percentage is diagnostic, not capability proof.
- Critical deterministic rules should eventually receive mutation testing.
- Every accepted bug/process failure gets a regression test or explicit non-automatable reason.
- Cross-platform claims require the same required scenario on each named OS.

## Done rule

A command is not done merely because code and tests exist. It needs:

1. registry/traceability row;
2. observable acceptance criteria;
3. targeted automated tests;
4. relevant black-box scenario;
5. matching CI evidence;
6. safety/privacy proof where applicable;
7. discovery/follow-up for remaining gaps.
