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

## Follow-up review: digest-omission downgrade (2026-11-06)

The P1 and P2 review fixes at `21a2281f734f8cb87116bdcb4f86e013506caec8` passed exact-head GitHub
CI [42243960199](https://github.com/ivanjovicic/AgentsWatch/actions/runs/42243960199):
162/162 on Linux and 162/162 on Windows, zero build warnings/errors.

A further code-level review found that `RunReceiptJson.EnsureStoredDigest` rejected a
*mismatching non-null* digest but accepted a **missing or null** `evidenceDigest`.
Removing the digest after editing the receipt was therefore an integrity downgrade:
a mutation command could silently create a new digest over altered acceptance data.

Red-before regression:
- `MissingReceiptEvidenceDigest_CannotReblessTamperedAcceptanceOrRewrite`
  has two variants (`evidenceDigest` removed or explicitly null).
- [Run 42248717280](https://github.com/ivanjovicic/AgentsWatch/actions/runs/42248717280),
  Ubuntu: 162 passed, **2 failed** at intermediate `66a8a373aefe`.
  Both variants unexpectedly returned `Ok=true` rather than rejecting the altered
  receipt (test line 582). The test did not rely on an independently attested CI source.

Fix:
- `EnsureStoredDigest` now rejects absent/blank digests as well as mismatches;
  the evidence/claims/drift mutation entry points consistently call this guard.
- Existing tests that deliberately create modified, valid-digest fixtures now
  recompute the fixture digest explicitly. This reflects the known limitation
  that anyone controlling local content and its checksum can also forge both.
- The no-digest test asserts failed evidence/claims/drift operations and
  byte-for-byte preservation of the tampered receipt.
- Legacy or incomplete receipts without the stored checksum cannot be
  automatically upgraded by a mutation command. No unsafe auto-backfill.

Full exact-final-head CI verification must be linked in the PR review before any
merge recommendation; this log does not claim tests passed until that succeeds.

Residual limitations remain unchanged: an unkeyed local checksum is not independent
attestation, validation source labels are user-controlled, and semantic acceptance
is not authenticated. AW-VFY-010 remains the next independent dogfood gate.
