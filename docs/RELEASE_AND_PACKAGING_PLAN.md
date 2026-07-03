# AgentsWatch Release and Packaging Plan

Last aligned: 2026-07-03  
Status: draft, blocked until Gate 0 passes

## Purpose

Define how AgentsWatch becomes installable and how every release proves which capabilities are actually supported.

Use with:

- `PROOF_AND_VERIFICATION_STRATEGY.md`;
- `FEATURE_CAPABILITY_REGISTRY.md`;
- `FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`;
- `PROOF_BUNDLE_SPEC.md`;
- `INDEPENDENT_VERIFICATION_RUNBOOK.md`.

## Prerequisite

Do not publish a release until:

- restore/build/test verified for the release commit;
- required CLI smoke/acceptance scenarios verified;
- risk register updated;
- advertised capabilities meet required maturity;
- proof bundle matches the packaged commit;
- no unresolved P0/P1 release blocker exists.

## Release stages

### Stage 0 — Local dev run

```bash
dotnet run --project src/AgentsWatch.Cli -- --help
```

Proof:
- exact commit and environment recorded;
- help/version/current command smoke transcripts.

### Stage 1 — Local pack

```bash
dotnet pack src/AgentsWatch.Cli/AgentsWatch.Cli.csproj --configuration Release
```

Proof:
- package exists;
- package ID/version match contracts;
- SHA-256 checksum recorded.

### Stage 2 — Isolated tool install

```bash
dotnet tool install --tool-path <temp-path> --add-source <package-dir> AgentsWatch.Cli
```

Proof:
- no source checkout required;
- installed help/version and required scenarios pass;
- tested package checksum matches Stage 1.

### Stage 3 — Release candidate

Required:

- complete proof bundle;
- capability matrix snapshot;
- known limitations;
- independent verification result;
- release-claim certification.

### Stage 4 — GitHub release

Assets:

- NuGet package;
- SHA-256 checksum;
- release notes;
- installation instructions;
- proof manifest/bundle reference;
- capability matrix snapshot;
- independent verification summary;
- examples that do not expose private data.

### Stage 5 — NuGet publish

Only after:

- release candidate accepted;
- clean install passes;
- no-overwrite/privacy rules tested;
- versioning policy confirmed;
- dogfood supports any usefulness claims included in release messaging.

## Versioning draft

```text
0.1.0 — bootstrap CLI skeleton; only capabilities proven by bundle may be advertised
0.2.0 — prompt optimizer/task split after verification
0.3.0 — run reports/handoff after verification
0.4.0 — review/claims/discovery proof after verification
0.5.0 — dogfood-ready local CLI
1.0.0 — stable documented CLI contract with release verification
```

Version number alone does not imply capability maturity.

## Release checklist

- [ ] release commit/tag is immutable;
- [ ] restore passes;
- [ ] build passes;
- [ ] tests pass with retained results;
- [ ] required acceptance/safety scenarios pass;
- [ ] package created;
- [ ] checksum created and verified;
- [ ] isolated install tested;
- [ ] proof manifest commit/version/package match;
- [ ] capability registry/matrix snapshot included;
- [ ] release claims certified;
- [ ] dogfood linked for value claims;
- [ ] security/privacy reviewed;
- [ ] known limitations and unresolved risks visible;
- [ ] independent verification Accept or documented Conditional with no release blocker.

## Release notes template

```markdown
# AgentsWatch <version>

Commit/tag: `<sha/tag>`
Package SHA-256: `<hash>`
Proof bundle: `<artifact/reference>`
Independent verification: `<result/reference>`

## Supported capabilities

| Capability ID | Capability | Evidence level |
|---|---|---|
| | | |

## Highlights

-

## Validation

- restore: pass/fail
- build: pass/fail
- tests: pass/fail
- black-box scenarios: pass/fail/blocked
- safety/privacy: pass/fail/blocked
- clean install: pass/fail

## Known limitations

-

## Value evidence

- Dogfood/benchmark references or `no public efficiency claim`.

## Upgrade notes

-
```

## Failure rule

Do not publish or silently remove evidence when:

- package and manifest commits differ;
- required scenario fails/skips;
- release notes advertise an unproven capability;
- checksum/clean install fails;
- proof bundle omits a required failed stage;
- P0/P1 release blocker remains open.
