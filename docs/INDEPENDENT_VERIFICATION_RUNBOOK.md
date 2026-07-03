# AgentsWatch Independent Verification Runbook

Last aligned: 2026-07-03  
Status: release-candidate verification contract

## Purpose

A release candidate should be verifiable by a reviewer who did not implement the change and does not depend on the developer's workspace or chat history.

## Reviewer inputs

- immutable commit/tag;
- packaged `.nupkg` and SHA-256 checksum;
- proof bundle;
- capability registry and traceability matrix;
- acceptance scenario definitions;
- known limitations;
- release notes.

## Clean environment

Use one of:

- fresh CI runner;
- clean virtual machine/container with supported .NET SDK/runtime;
- dedicated temporary local user/tool path.

Do not use previously built binaries or global tool installations unless the test explicitly verifies upgrade behavior.

## Verification sequence

1. Verify package checksum.
2. Record OS, architecture, .NET SDK/runtime, git version, and timestamp.
3. Install tool into an isolated tool path.
4. Run help and version scenarios.
5. Run current supported command scenarios from `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`.
6. Run negative/error scenarios.
7. Run file-system safety checks in temporary repos.
8. Run no-network/privacy checks where supported.
9. Compare outputs with documented anchors/golden results.
10. Verify proof-manifest commit/version/artifact consistency.
11. Compare advertised features with the capability registry/matrix.
12. Record pass/fail/blocked for every required capability.
13. Open discoveries for mismatches instead of editing evidence to hide them.

## Required release-candidate results

```text
Verifier:
Verification ID:
Commit/tag:
Package:
Package SHA-256:
Environment:
Capabilities reviewed:
Scenarios passed:
Scenarios failed:
Scenarios blocked:
Privacy/safety result:
Manifest integrity result:
Claims-vs-evidence result:
Known limitations confirmed:
Discoveries created:
Decision: Accept | Reject | Conditional
```

## Independence rules

- Reviewer should not accept developer explanations as a substitute for artifacts.
- Reviewer may inspect source after black-box execution, but should record that step separately.
- Failed or blocked scenarios remain visible.
- A conditional decision must state exact missing evidence and follow-up IDs.
- The same person may perform verification for early internal milestones, but public-release verification should use a different reviewer or isolated automated job when possible.

## Black-box safety checks

Verify:

- no writes outside selected temporary repository/tool path;
- existing user files are preserved;
- commands do not upload repository artifacts by default;
- secret-like fixture values are not printed unexpectedly;
- binary contents are not inlined;
- invalid input returns documented exit code and clear error;
- interrupted/failed operations leave recoverable local state.

## Claims audit

For each release-note claim:

```text
Claim
Capability ID
Required maturity
Observed evidence
Result: Match | Partial | Mismatch
Required correction
```

Reject or downgrade claims that exceed the traceability matrix.

## Release decision

Accept only when:

- all advertised capabilities meet their required maturity;
- mandatory acceptance/safety scenarios pass;
- package and proof bundle match the released commit;
- no unresolved P0/P1 release blocker exists;
- known limitations are explicit;
- installation and basic use work without source checkout.

## Evidence storage

Save under:

```text
artifacts/proof/<commit>/independent-verification/
```

or attach the signed/immutable report to the release/CI run.

Do not include private repositories, prompts, source, or raw logs unless explicitly approved.
