# PR #10 review-fix evidence (receipt integrity P1/P2)

- Prompt: GitHub review #5801945868 on draft PR #10 (https://github.com/ivanjovicic/AgentsWatch/pull/10#pullrequestreview-5801945868) at parent head `86318d62eb1fcd8f207acf78caf73dd52906e7e5`
- Branch: `fix/aw-evidence-trust-gate-20261106`
- Run mode: implementation (review-fix only; PR left draft, not merged, no GitHub comments posted)
- Model/client: unknown-not-exposed; timing: unknown-not-recorded
- Timezone: Europe/Belgrade

## Findings addressed

1. P1: Removing `acceptanceCriteria[].status` from receipt JSON defaults the enum to `Supported`. Evidence record/check did not verify the stored `evidenceDigest` before recomputing it, so a matching Pass could persist `Done`.
2. P2: `runDelta: {}` passed the top-level non-null check; automatic `ScopeDriftChecker.Check` then threw `NullReferenceException` on null change collections.

## Red-before proof (tests added first, no production change)

`dotnet test tests/AgentsWatch.Tests -c Release --filter FullyQualifiedName~MissingAcceptanceStatus_WithStaleDigest_CannotProduceDoneOrRewrite|FullyQualifiedName~EmptyRunDeltaObject_DoesNotThrow_AndIsRejectedWithoutRewrite`

1. `MissingAcceptanceStatus_WithStaleDigest_CannotProduceDoneOrRewrite` — `Assert.NotEqual() Failure: Values are equal / Expected: Not 0 / Actual: 0` (CLI `evidence record` returned success/`Done` after deleting AC status and keeping the stale digest). Line 549.
2. `EmptyRunDeltaObject_DoesNotThrow_AndIsRejectedWithoutRewrite` — `Assert.Null() Failure` with `System.NullReferenceException` at `ScopeDriftChecker.Check` (`ScopeDrift.cs:27`) via `EvidenceGate.Apply` (`EvidenceGate.cs:97`) / `EvidenceService.Check`. Line 571.

Failed 2, Passed 0, Total 2.

Scenarios match the review: finish under `feature works` + `dotnet test` + `test log`; (1) strip `acceptanceCriteria[0].status`, keep `evidenceDigest`, run `evidence record --command "dotnet test" --status pass --source user-declared --summary "test log"`; (2) set `runDelta` to `{}` and run `evidence check`.

## Fix

- Deserialize structurally requires each acceptance criterion `status` and the four `runDelta` change arrays. A missing status is rejected (not treated as Supported).
- `evidence`/`claims`/`drift` mutation loads call `RunReceiptJson.EnsureStoredDigest`: a present but mismatched digest is rejected and the file is not rewritten. `receipt show` still reads and warns (existing test).
- `EvidenceGate` skips automatic drift when collections are null and records AWE001; `ScopeDriftChecker` also treats null lists as empty so evaluation cannot throw.

## Gate (executed, Europe/Belgrade)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 162, Skipped 0, Total 162 (160 prior + 2 regressions).

## Known remaining limits

- A locally recomputed SHA-256 digest is not attestation; a writer who can rewrite both body and digest is outside this check.
- `ValidationSource.CI` remains user-supplied. `ExpectedEvidence` matching remains substring-based.
- Semantic acceptance may still be marked Supported when the field is present.
- AW-VFY-010 dogfood and independent reviewer approval remain open. No claim of independently trusted agent execution.
