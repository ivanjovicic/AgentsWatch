# AgentsWatch Runtime Compatibility Implementation Backlog

Last aligned: 2026-07-03  
Status: issue-ready L1 backlog

## Purpose

Implement AW-CAP-037 before advanced community capabilities assume equal behavior across models, tools, permissions, or environments.

## Global gates

- Main Gate 0 must pass.
- Core run/evidence spine must exist before live adapters.
- All detection is local and non-destructive by default.
- No adapter may silently enable hooks, permissions, network, telemetry, or broad-access modes.
- Unknown/conflicted capability blocks enforce mode.
- Every issue must add or map executable `COMP-*` scenarios.

---

## COMPAT-001 — Runtime profile domain schema

Capability: AW-CAP-037  
Priority: P0

Task:

- implement immutable/versioned profiles for model, tool, surface, observation, permission, environment, VCS/delivery, and data handling;
- add capability provenance, confidence, blind spots, and profile revisions.

Acceptance:

- JSON round trip stable;
- unknown and conflicted states preserved;
- multiple model roles supported;
- no raw credential values;
- schema migration/version tests;
- maps COMP-050.

## COMPAT-002 — Feature support decision engine

Dependencies: COMPAT-001

Task:

- produce exactly one of Full, Guarded, Advisory, PostHoc, Manual, Unavailable;
- attach required/missing/conflicting capabilities, confidence cap, and fallback.

Acceptance:

- no silent upgrade;
- non-Full always has reason/fallback;
- Full lists evidence sources;
- deterministic decisions;
- table-driven tests for AW-CAP-028 through AW-CAP-036.

## COMPAT-003 — Generic/manual adapter

Dependencies: COMPAT-001/002

Task:

- support chat-only and user-supplied artifacts;
- generate generic context/rules outputs and post-hoc evidence intake.

Acceptance:

- COMP-001 passes;
- live observation/control remains unavailable;
- manual claims never exceed low/user-attested confidence.

## COMPAT-004 — Local process-wrapper adapter

Dependencies: command profiler/run spine

Task:

- detect process ownership, command exit/duration, environment, and checkpoint availability;
- do not install provider hooks.

Acceptance:

- COMP-003 passes;
- child cleanup/cancellation tests;
- file-read visibility remains Unknown;
- live stop unavailable without checkpoint policy.

## COMPAT-005 — Environment detector foundation

Task:

- identify OS, architecture, shell, local/container/WSL/remote/CI hints, workspace roots, git/worktree, and read-only mounts where safely detectable.

Acceptance:

- Linux and Windows fixtures;
- COMP-013, 014, 016, 018, 022 pass or have fixture contracts;
- no broad filesystem scan;
- host/container paths remain distinct.

## COMPAT-006 — Permission resolution model

Task:

- normalize read/write/delete/command/network/package/browser/MCP/git/PR/credential/service/production rights;
- support Allowed, Denied, ApprovalRequired, Unknown and multiple sources.

Acceptance:

- configured versus effective state distinct;
- managed/project conflict retained;
- COMP-007 through 012 and 037/038/045 pass;
- provider labels remain metadata, not independent proof.

## COMPAT-007 — Adapter capability handshake

Task:

- define static declaration plus dynamic handshake/health;
- downgrade on missing hook/event;
- emit profile revision when health changes.

Acceptance:

- configured-but-not-firing fixture;
- mid-run failure fixture;
- COMP-029, 031, 047 pass;
- no false continuity.

## COMPAT-008 — Universal git/no-git repository profile

Task:

- enrich git detection with branch/HEAD/base/merge base/worktree/dirty/pre-existing changes;
- add no-git manifest fallback.

Acceptance:

- COMP-018, 022, 023, 039 pass;
- stacked PR merge-base semantics tested;
- no automatic git initialization.

## COMPAT-009 — Rule-format capability/loss model

Capability link: AW-CAP-031

Task:

- define Exact, Equivalent, Weaker, AdvisoryOnly, Unsupported, ConflictWithManagedPolicy;
- add target size/precedence/ownership metadata.

Acceptance:

- COMP-012, 021, 032, 033 pass;
- no overwrite of managed/user-owned files;
- loss report deterministic.

## COMPAT-010 — Usage metric normalization

Capability link: AW-CAP-032

Task:

- normalize exact tokens, cached tokens, request counts, quota percentages, currency, wall-clock, and local compute proxies without unsafe conversion.

Acceptance:

- COMP-025, 026, 041, 042 pass;
- raw units preserved;
- unknown pricing stays unknown;
- multi-model role attribution.

## COMPAT-011 — Compatibility CLI

Dependencies: COMPAT-001/002/005/006/008

Planned commands:

```bash
agentswatch compatibility detect
agentswatch compatibility explain <capability-id>
agentswatch compatibility compare <profile-a> <profile-b>
agentswatch compatibility export --format markdown|json
```

Acceptance:

- Linux/Windows black-box tests;
- JSON/markdown agreement;
- no write/network by default;
- clear blind spots/fallbacks;
- COMP-050 passes.

## COMPAT-012 — Profile revision and mid-run downgrade

Task:

- create new profile revisions for model, permission, cwd/worktree, surface, environment, network, credential class, hook health, and repository state changes.

Acceptance:

- events linked to correct revision;
- later capabilities downgrade safely;
- COMP-026, 028, 031, 047 pass.

## COMPAT-013 — First rich local hook adapter

Gate:

- official documented event surface;
- privacy review;
- event foundation.

Task:

- implement one supported local tool adapter with session/tool/approval/subagent lifecycle where available.

Acceptance:

- handshake fixtures;
- schema/version declaration;
- incomplete event paths disclosed;
- COMP-002, 029, 030 pass;
- no Full policy claim when interception is incomplete.

## COMPAT-014 — Second local tool adapter

Purpose:

- prove normalized model across a materially different event/permission system.

Acceptance:

- same domain events/support engine reused;
- provider-specific code isolated;
- cross-adapter fixture comparison;
- no lowest-common-denominator loss hidden.

## COMPAT-015 — Cloud/PR evidence adapter

Task:

- ingest task/session/environment/branch/PR/check metadata for one cloud coding-agent flow;
- no local process-control assumptions.

Acceptance:

- COMP-005, 006, 017, 019, 039 pass;
- setup versus agent phase separated;
- commit/check binding exact;
- unavailable logs/checks explicit.

## COMPAT-016 — Remote/container path identity

Task:

- model host, container, WSL, remote, and repository logical paths;
- prevent unsafe path conflation.

Acceptance:

- COMP-013, 014, 015, 016 pass;
- path traversal/junction/symlink fixtures;
- evidence shows execution location.

## COMPAT-017 — Resource lease model

Capability link: AW-CAP-034

Task:

- coordinate singleton emulators, ports, dev servers, databases, and external test resources separately from file ownership.

Acceptance:

- COMP-035 passes;
- waiting is not a loop by default;
- lease expiry/recovery defined;
- no automatic production-resource lease.

## COMPAT-018 — Compatibility proof-bundle integration

Task:

- include runtime profile hash, revisions, support decisions, adapter versions/health, blind spots, and fallback outcomes in proof bundles.

Acceptance:

- mismatch/missing profile fails advanced feature certification;
- release claim cannot exceed support mode;
- independent verifier can reproduce decision from artifacts.

## COMPAT-019 — Cross-surface dogfood matrix

Gate: prior fixtures green.

Required dogfood:

- one local rich-event tool;
- one local wrapper/no-hook flow;
- one IDE/local flow;
- one cloud/PR flow;
- one chat/manual flow;
- one read-only/no-network constrained flow.

Acceptance:

- correct downgrades and fallbacks observed;
- blind spots understandable to users;
- no unsupported Full result;
- adapter maintenance burden recorded.

## COMPAT-020 — Independent compatibility verification

Gate: release candidate.

Acceptance:

- clean-install profile detection;
- tool update/removal scenario;
- no network/credential leak;
- Linux/Windows verification;
- unsupported environment uses safe fallback;
- public compatibility claims match the tested matrix.

## Recommended order

```text
COMPAT-001
-> COMPAT-002
-> COMPAT-003/005/006/008
-> COMPAT-007/009/010
-> COMPAT-011/012
-> first rich local adapter
-> second local adapter
-> cloud/PR adapter
-> path/resource/proof integration
-> dogfood
-> independent verification
```

## Live-feature gate

Flight Recorder live ingestion, live Loop Guard, Policy enforcement, controlled worker launching, and automated cloud integration remain blocked until the compatibility engine can correctly downgrade and produce a safe fallback for all required scenarios.
