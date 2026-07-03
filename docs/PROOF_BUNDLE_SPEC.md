# AgentsWatch Proof Bundle Specification

Last aligned: 2026-07-03  
Status: release and CI evidence contract

## Purpose

A proof bundle is an immutable, commit-bound collection of artifacts showing what was built, tested, executed, packaged, and verified.

It must allow a reviewer to validate claims without trusting chat history or manually reconstructed terminal output.

## Canonical artifact layout

```text
artifacts/proof/<commit-sha>/
  proof-manifest.json
  capability-matrix.md
  environment/
    dotnet-info.txt
    runner.txt
  build/
    build-summary.txt
  tests/
    *.trx
    test-summary.md
    coverage/                 # when enabled
  smoke/
    help.txt
    version.txt
    optimize.txt
    status.txt
    exit-codes.json
  acceptance/
    scenario-results.json
    scenario-summary.md
  safety/
    safety-summary.md
  package/
    *.nupkg
    SHA256SUMS.txt
  dogfood/
    references.md
  claims/
    claims-vs-evidence.md
  known-limitations.md
```

Not every early build has every folder. The manifest must explicitly mark required, present, skipped, blocked, and not-applicable artifacts.

## Proof manifest

Minimum JSON shape:

```json
{
  "schemaVersion": 1,
  "repository": "ivanjovicic/AgentsWatch",
  "commitSha": "<full sha>",
  "branchOrTag": "<branch-or-tag>",
  "version": "<package version or unknown-not-built>",
  "generatedAtUtc": "<ISO-8601>",
  "runner": {
    "os": "<name/version>",
    "architecture": "<arch>",
    "dotnetSdk": "<version>"
  },
  "stages": {
    "restore": "Pass|Fail|Blocked|NotRun",
    "build": "Pass|Fail|Blocked|NotRun",
    "test": "Pass|Fail|Blocked|NotRun",
    "smoke": "Pass|Fail|Blocked|NotRun",
    "acceptance": "Pass|Fail|Blocked|NotRun",
    "safety": "Pass|Fail|Blocked|NotRun",
    "pack": "Pass|Fail|Blocked|NotRun",
    "cleanInstall": "Pass|Fail|Blocked|NotRun"
  },
  "capabilities": [
    {
      "id": "AW-CAP-001",
      "claimedLevel": "L4",
      "evidence": ["smoke/help.txt", "tests/help.trx"],
      "result": "Pass|Fail|Partial|NotRun"
    }
  ],
  "artifacts": [
    {
      "path": "package/AgentsWatch.Cli.<version>.nupkg",
      "sha256": "<hash>"
    }
  ],
  "knownLimitations": ["<text>"],
  "dogfoodReferences": ["<path-or-url>"],
  "generator": "CI|local"
}
```

## Integrity requirements

- `commitSha` must equal the commit that produced the binaries/tests.
- Package checksum must be generated after packaging and verified during clean-install proof.
- CI artifacts must be retained according to project policy.
- A release proof bundle should be attached to the release or referenced by immutable CI run.
- Manifests may summarize but must not replace raw TRX, scenario output, or package checksum evidence.
- Failed or blocked stages remain in the bundle; do not delete them to make the bundle appear green.

## CI bundle

Every pull request should eventually produce at least:

- restore/build/test status;
- TRX test files;
- basic help/version/optimize/status smoke transcripts;
- tested commit/environment metadata;
- changed capability list when available.

After packaging is enabled, main/tag builds additionally produce:

- `.nupkg`;
- checksum;
- package metadata;
- isolated install smoke;
- proof manifest.

## Local bundle

A developer may generate a local proof bundle when CI is unavailable, but must include:

- exact commit SHA and dirty-working-tree status;
- commands executed;
- environment details;
- raw test result files;
- reason CI was unavailable;
- `generator: local`.

A dirty working tree cannot establish release verification unless the bundle also records a patch/diff and is explicitly labeled non-release evidence.

## Capability maturity calculation

The proof bundle should compute or validate maturity, not accept arbitrary labels.

Examples:

- implementation path only -> maximum L2;
- passing unit test linked to capability -> maximum L3;
- required CI and black-box scenario pass -> maximum L4;
- dated real-repo dogfood evidence -> maximum L5;
- packaged clean-install verification -> maximum L6.

## Required validation failures

Proof-bundle validation fails when:

- commit is missing or mismatched;
- claimed capability is absent from registry/matrix;
- claimed maturity exceeds available evidence;
- referenced artifact is missing;
- checksum does not match;
- a required stage is silently omitted;
- failed test/scenario is reported as pass;
- release bundle lacks clean-install evidence;
- value claim lacks dogfood references.

## Privacy

Do not include by default:

- repository source;
- full diffs;
- user prompts;
- command output containing secrets;
- private run logs;
- private discovery records.

Use compact transcripts, synthetic fixtures, hashes, summaries, and paths.

## Retention

Recommended:

- pull-request bundles: retain long enough for review/regression diagnosis;
- main-branch bundles: retain for each accepted milestone;
- release bundles: retain permanently with release artifacts;
- failed bundles: retain sufficiently to diagnose recurring failures.

## Current implementation state

This specification is L1. CI currently runs restore/build/test only and does not yet generate the complete bundle. The proof queue owns incremental implementation.
