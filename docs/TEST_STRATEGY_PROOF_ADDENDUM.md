# AgentsWatch Test Strategy Proof Addendum

Last aligned: 2026-07-03  
Status: mandatory addition to `TEST_STRATEGY.md`

## Why this addendum exists

The original test strategy defines useful test layers, but product proof also requires requirement traceability, black-box scenarios, CI artifacts, package verification, and claims governance.

## Required proof additions

For each user-visible capability:

1. stable capability ID;
2. observable acceptance criteria IDs;
3. targeted automated tests;
4. at least one black-box scenario;
5. failure/negative scenario;
6. CI result linked to the same commit;
7. safety/privacy proof when relevant;
8. registry/matrix maturity update;
9. dogfood proof for usefulness claims;
10. clean-install proof for release support.

## Test-source vs test-result rule

```text
Test source exists != test passed.
Test passed on another commit != proof for this commit.
Green build != every feature verified.
Coverage percentage != requirement coverage.
```

## Additional test categories

- CLI process tests with exit code/stdout/stderr assertions;
- temporary git-repository lifecycle tests;
- path traversal/outside-write tests;
- no-network tests;
- fake-secret redaction tests;
- proof-manifest schema and commit-match tests;
- capability maturity calculation tests;
- discovery deduplication/owner/routing tests;
- false-proof regression tests;
- package checksum and isolated-install smoke;
- mutation tests for critical deterministic rule engines after normal tests stabilize.

## Coverage policy

Code coverage is diagnostic and helps find untested branches. It must not be used alone to claim a feature works.

Requirement coverage is primary:

```text
accepted criterion -> named test/scenario -> executed result -> proof artifact
```

## Regression policy

Every confirmed defect in:

- validation honesty;
- path safety;
- claim maturity;
- discovery routing;
- evidence completeness;
- packaging/install;
- risk/prompt classification

must produce a regression test or an explicit reason why deterministic automation is not possible.

## CI evidence

CI should retain:

- TRX files;
- smoke transcripts;
- exit-code summary;
- environment metadata;
- package/checksum;
- clean-install transcript;
- proof manifest.

## Release test gate

A stable release requires all mandatory tests/scenarios for advertised capabilities, safety/privacy checks, package/install proof, and independent verification.
