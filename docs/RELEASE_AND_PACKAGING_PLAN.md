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
- `INDEPENDENT_VERIFICATION_RUNBOOK.md`;
- `PRODUCT_FORM_FACTORS_INSTALLATION_AND_DELIVERY_PLAN.md`.

## Product packaging principle

AgentsWatch is one product with a shared core and optional components.

```text
Required first interface: AgentsWatch CLI
Optional later components: adapters, local service, dashboard, IDE clients, GitHub Action/App, Team Server
```

Each component must reuse the same capability registry, runtime compatibility model, evidence contracts, and release-proof rules.

Do not publish separate provider-specific products or allow optional interfaces to reimplement core logic.

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

Initial assets:

- NuGet package;
- SHA-256 checksum;
- release notes;
- installation instructions;
- proof manifest/bundle reference;
- capability matrix snapshot;
- independent verification summary;
- examples that do not expose private data.

Later, after standalone packaging is proven, add per-platform archives/executables from the same immutable release.

### Stage 5 — NuGet publish

Only after:

- release candidate accepted;
- clean install passes;
- no-overwrite/privacy rules tested;
- versioning policy confirmed;
- dogfood supports any usefulness claims included in release messaging.

### Stage 6 — Standalone self-contained builds

Only after the CLI contract is stable enough to support platform-specific release maintenance.

Candidate runtime identifiers:

```text
win-x64
win-arm64 when demand exists
linux-x64
linux-arm64 when demand exists
osx-x64
osx-arm64
```

Possible formats:

- ZIP or tar archive;
- self-contained executable;
- single-file executable where compatible with required features;
- installer only when it materially improves installation or service registration.

Required proof for every published runtime identifier:

- build from the tagged commit;
- checksum and signature where supported;
- clean-machine or clean-container install;
- help/version and required black-box scenarios;
- platform-specific compatibility profile;
- uninstall/upgrade guidance;
- known runtime limitations.

A successful NuGet tool release is not proof that every standalone platform package works.

### Stage 7 — Package-manager distribution

After standalone artifacts prove stable:

```text
WinGet or Scoop for Windows
Homebrew for macOS/Linux
NuGet/.NET tool remains supported
```

Rules:

- manifests reference immutable release artifacts and checksums;
- package-manager version matches AgentsWatch release version;
- installation smoke is automated where practical;
- do not add a channel that cannot be maintained and tested;
- a delayed package-manager update must be visible rather than silently serving an older incompatible build.

### Stage 8 — GitHub Action package

The GitHub Action should be the first team/CI distribution before the full GitHub App.

Example target:

```yaml
- name: Verify AI-assisted change
  uses: agentswatch/verify-pr@v1
```

Packaging requirements:

- pin or verify an immutable AgentsWatch CLI artifact;
- use commit-bound evidence;
- support forks and restricted permissions safely;
- avoid hidden source/log upload;
- declare all required token permissions;
- publish report/artifact before automatic comment/check modes;
- retain exact Action commit, CLI version, and proof manifest.

Action versioning must avoid mutable behavior hidden behind a tag without auditable commit history.

### Stage 9 — Optional local service and dashboard

Only after service/dashboard runtime exists and has dogfood evidence.

Packaging requirements:

- optional installation;
- explicit start/stop/status;
- loopback-only local API by default;
- bounded retention and storage location;
- CLI remains usable when service/dashboard is absent;
- uninstall preserves user-owned evidence unless explicitly removed;
- service registration/privileges are previewed and opt-in;
- platform-specific service behavior tested independently.

The first CLI release must not install or start a daemon automatically.

### Stage 10 — Thin IDE extension

First candidate: VS Code Marketplace, only after a useful local API/CLI interaction exists.

Requirements:

- thin client over AgentsWatch CLI/local API;
- compatible CLI/service version declared;
- safe degraded mode when local component is absent;
- no duplicated evidence, policy, risk, or compatibility engine;
- transparent local data access;
- no mandatory cloud login.

JetBrains/Visual Studio clients remain later hypotheses based on demand.

### Stage 11 — GitHub App and Team beta

Only after the manual and GitHub Action PR workflows prove value.

Requirements:

- least-privilege installation permissions;
- tenant/repository isolation;
- explicit posting and data-access policy;
- private/public/fork scenario coverage;
- connection to the optional Team Server only when enabled;
- local CLI remains independent of App auth.

### Stage 12 — Team Server editions

Possible later delivery forms:

```text
Hosted AgentsWatch Cloud
Customer-managed/self-hosted Team Server
Hybrid local evidence plus metadata sync
```

Blocked until paid team demand and data-minimization requirements are validated.

The server should receive metadata by default, not source, full prompts, full diffs, raw terminal logs, secrets, or complete local event journals.

Self-hosted packaging additionally requires:

- deployment/upgrade/rollback model;
- backup and retention guidance;
- SSO/RBAC and tenant isolation;
- support matrix;
- vulnerability/patch process;
- independent security review appropriate to claims.

## Versioning draft

```text
0.1.0 — bootstrap CLI skeleton; only capabilities proven by bundle may be advertised
0.2.0 — prompt optimizer/task split after verification
0.3.0 — run reports/handoff after verification
0.4.0 — review/claims/discovery proof after verification
0.5.0 — dogfood-ready local CLI
1.0.0 — stable documented CLI contract with release verification
```

Version number alone does not imply capability maturity or availability in every component/platform.

Optional components may have their own package version when technically necessary, but compatibility with the shared core must be explicit and machine-readable.

## Component compatibility manifest

A future multi-component release should publish:

```text
AgentsWatch product version
CLI version
Core/schema version
Adapter versions
Local service API version
Dashboard compatibility range
IDE extension compatibility range
GitHub Action CLI version
Team Server API/sync version
```

An interface must fail or degrade clearly when the installed core/service version is incompatible.

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
- [ ] runtime/platform support matrix included where relevant;
- [ ] release claims certified;
- [ ] dogfood linked for value claims;
- [ ] security/privacy reviewed;
- [ ] known limitations and unresolved risks visible;
- [ ] independent verification Accept or documented Conditional with no release blocker;
- [ ] optional component compatibility versions match;
- [ ] no optional daemon, hook, telemetry, or cloud sync enabled silently.

## Release notes template

```markdown
# AgentsWatch <version>

Commit/tag: `<sha/tag>`
Package SHA-256: `<hash>`
Proof bundle: `<artifact/reference>`
Independent verification: `<result/reference>`

## Distributed components

| Component | Version/artifact | Required/optional |
|---|---|---|
| AgentsWatch CLI | | Required/primary |
| Standalone packages | | Optional |
| GitHub Action | | Optional |
| Local service/dashboard | | Optional |
| IDE extension | | Optional |
| Team Server | | Optional |

## Supported capabilities

| Capability ID | Capability | Evidence level | Supported runtime profiles |
|---|---|---|---|
| | | | |

## Highlights

-

## Validation

- restore: pass/fail
- build: pass/fail
- tests: pass/fail
- black-box scenarios: pass/fail/blocked
- safety/privacy: pass/fail/blocked
- clean install: pass/fail
- platform/component matrix: pass/fail/blocked

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
- an optional component claims compatibility with an untested core version;
- a platform package was not clean-install tested;
- a service/hook/cloud integration is enabled without explicit user action;
- P0/P1 release blocker remains open.
