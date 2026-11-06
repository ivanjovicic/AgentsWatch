# PR #10 review-fix evidence (control-flow P1s)

- Prompt: GitHub review on draft PR #10 (https://github.com/ivanjovicic/AgentsWatch/pull/10), review https://github.com/ivanjovicic/AgentsWatch/pull/10#pullrequestreview-5799626978, inline https://github.com/ivanjovicic/AgentsWatch/pull/10#discussion_r4492058460
- Branch: `fix/aw-evidence-trust-gate-20261106` at parent `93e3dd730b34b9c0a045dcc39fdec0cb3cc87bcd`
- Run mode: implementation (review-fix only; PR left draft, not merged, no GitHub comments posted)
- Model/client: unknown-not-exposed; timing: unknown-not-recorded
- Timezone: Europe/Belgrade

## Findings addressed

1. A still-valid edit to the stored RunContract after finish (same id/task/acceptance, linter-valid, e.g. mandatory command changed from `dotnet test` to `echo ok`) could reinterpret old evidence and reach Done. The previous gate compared contractId, taskId, linter validity and acceptance text only.
2. `evidence check` / `EvidenceGate.Apply` could return Done without Scope Drift ever running. Drift was an optional CLI step; Apply only consumed already-persisted AWD findings.

## Red-before proof (tests added first, no production change)

`dotnet test tests/AgentsWatch.Tests -c Release --filter FullyQualifiedName~ValidContractChangedAfterFinish_IsRejected|FullyQualifiedName~EvidenceCheck_WithoutDriftCheck_DoesNotDone_WhenOutsideOwnedAndAvoidTouched`

Both failed with:

```text
Assert.NotEqual() Failure: Values are equal
Expected: Not Done
Actual:       Done
```

- `ValidContractChangedAfterFinish_IsRejected` at EvidenceGateTests.cs:185
- `EvidenceCheck_WithoutDriftCheck_DoesNotDone_WhenOutsideOwnedAndAvoidTouched` at EvidenceGateTests.cs:215
- Failed 2, Passed 0, Total 2

Scenarios match the review: (1) finish under `dotnet test` + `feature works` + `test log`, rewrite the same contract to require `echo ok`, mark AC Supported, import matching `echo ok` Pass with `test log`; (2) finish -> evidence record/check with outside-owned `extra.txt` and avoid-path `docs/note.md`, no `drift check` invocation.

## Fix

- Capture `RunContractJson.ComputeDigest` (SHA-256 of canonical contract JSON) on the receipt at finish as `ContractDigest`.
- `EvidenceGate.Apply` rejects Done when the loaded contract digest is missing or differs (`AWE001`).
- `EvidenceGate.Apply` always recomputes scope drift (`ScopeDriftChecker.Check`) and replaces AWD findings, so a positive decision cannot precede a scope check.

## Gate (executed, Europe/Belgrade)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 158, Skipped 0, Total 158 (156 prior + 2 regressions).

## Known remaining limits (not closed by this fix)

- `ValidationSource.CI` is still user-supplied, not authenticated.
- A locally recomputed SHA-256 digest is not attestation.
- `ExpectedEvidence` matching remains substring-based.
- Receipts finished before this change have no `ContractDigest` and cannot reach Done (fail-closed).
- Falsification coverage still belongs in AW-VFY-010. No claim of independently trusted agent execution.
