# AgentsWatch Product Form Factors, Installation, and Delivery Plan

Last aligned: 2026-07-03  
Status: product and delivery plan; no implementation implied

## Executive decision

AgentsWatch should be **one product with one shared application core and multiple optional interfaces/components**.

It should not become separate, independently implemented applications for Claude, Codex, Copilot, Cline, Cursor, Gemini, or other tools.

```text
AgentsWatch Product
  Shared Core and Contracts
  Local CLI — required first interface
  Runtime/Tool Adapters — optional and capability-negotiated
  Local Background Service — optional later
  Local Dashboard — optional later
  IDE Extensions — thin optional clients later
  GitHub Action — optional CI distribution
  GitHub App — optional team integration
  Team Server — optional hosted or self-hosted metadata layer later
```

Every component must reuse the same:

- domain and application services;
- runtime compatibility model;
- evidence and trust model;
- policy and redaction rules;
- report formats;
- capability registry and proof rules.

No interface may reimplement core risk, evidence, compatibility, or policy logic independently.

## Product identity

Public product name:

```text
AgentsWatch
```

Component names may describe installation surfaces:

```text
AgentsWatch CLI
AgentsWatch Local Service
AgentsWatch Dashboard
AgentsWatch GitHub Action
AgentsWatch GitHub App
AgentsWatch Team Server
AgentsWatch VS Code Extension
```

These are components/editions of the same product, not unrelated products.

## Canonical architecture

```text
                        Optional Team Server
                    metadata, policy, audit, licensing
                               ▲
                               │ explicit opt-in metadata sync
                               │
GitHub Action/App ─────── Local/CI Application API ─────── IDE thin clients
                               │
                         AgentsWatch Core
                               │
          evidence / context / compatibility / policy / reports
                               │
              adapters / git / filesystem / process / CI
                               │
                    local repository and artifacts
```

## Component plan

### 1. AgentsWatch Core

Role:

- shared domain/application logic;
- no direct console, UI, GitHub, or cloud dependency;
- local-first contracts;
- capability negotiation;
- evidence, context, policy, loop, and review projections.

Distribution:

- initially internal .NET assemblies used by the CLI;
- not necessarily a separately marketed package;
- stable public/plugin contracts only after real adapter demand.

### 2. AgentsWatch CLI — first and required product

The CLI is the first installable product and remains the universal interface for local use and CI.

Initial command family:

```bash
agentswatch init
agentswatch status
agentswatch optimize
agentswatch verify
agentswatch report
agentswatch rules lint
agentswatch context snapshot
agentswatch compatibility detect
```

Later command families are introduced only when their capability rows and proof gates permit them.

Properties:

- runs on the developer machine or CI runner;
- no account required for core local functionality;
- no network required by default;
- reads only the selected repository/artifact roots;
- writes local markdown/JSON evidence;
- adapters are loaded through the same executable or supported plugin mechanism;
- unknown tools receive generic/manual/post-hoc fallbacks.

### 3. Runtime and tool adapters

Adapters are not separate user-facing applications.

They may be delivered as:

- built-in adapters for stable/common surfaces;
- optional signed adapter packages later;
- project/team configuration selecting enabled adapters;
- generic manual, process-wrapper, git/CI, and no-git fallbacks.

Examples:

```text
Claude Code local adapter
Codex local adapter
Codex/GitHub cloud evidence adapter
Generic process wrapper
Generic chat/manual adapter
GitHub PR/check adapter
```

Adapter installation must not silently:

- install provider hooks;
- enable telemetry;
- broaden permissions;
- turn on network access;
- enable YOLO/full-access modes;
- upload repository data.

Hook/config installation requires an explicit preview and user action.

### 4. Optional local background service

The local background service is deferred until import-only and command-wrapper workflows prove value.

Possible process name:

```text
agentswatch service
```

Responsibilities:

- receive supported hook/runtime events;
- maintain a local append-only event journal;
- update run and compatibility profiles;
- provide local notifications;
- support live loop warnings and checkpoints where permitted;
- expose a loopback-only local API for dashboard/IDE clients.

Default behavior:

- not installed or started automatically by the first MVP;
- visible process and status;
- explicit enable/disable;
- local-only binding by default;
- configurable retention;
- no cloud sync unless separately enabled;
- no live process termination without explicit feature configuration and proof gates.

The CLI must remain useful when the service is absent.

### 5. Optional local dashboard

The dashboard is a local viewer/controller over the same core and storage.

Possible command:

```bash
agentswatch dashboard
```

Possible local address:

```text
http://127.0.0.1:<configured-port>
```

Initial pages:

- runs and tasks;
- changed files and risk;
- validation and claims/evidence;
- context snapshots;
- compatibility profile and blind spots;
- loop/usage findings;
- policies and adapter health;
- PR evidence packets.

Rules:

- local-only by default;
- not required for CLI or CI usage;
- must call application use cases rather than duplicate logic;
- must not become a mandatory cloud login surface.

### 6. Optional IDE extensions

IDE extensions are deferred thin clients.

First candidate:

```text
VS Code extension
```

Possible later clients:

- Visual Studio;
- JetBrains Rider/IntelliJ platform.

Responsibilities:

- show current run, scope, validation, risk, and compatibility state;
- invoke approved CLI/local API commands;
- link findings to files;
- provide local notifications and context-pack actions.

Non-responsibilities:

- reimplementing evidence/policy/risk logic;
- storing a second incompatible history;
- silently controlling the coding agent;
- becoming required for core value.

### 7. GitHub Action — first team/viral distribution

The GitHub Action should precede the full GitHub App.

Example target usage:

```yaml
- name: Verify AI-assisted change
  uses: agentswatch/verify-pr@v1
```

Responsibilities:

- install/run a pinned AgentsWatch CLI version;
- inspect the current PR commit/range;
- consume build/test/check artifacts where available;
- generate a commit-bound evidence packet;
- publish a check summary or artifact according to configured permissions;
- operate with fork/secret/permission restrictions honestly.

Initial mode:

- report/artifact first;
- comment/check posting only when explicitly configured;
- public repositories may be the free adoption channel;
- private/team automation may be paid later.

### 8. GitHub App — team integration after Action/manual proof

The GitHub App is not required for the first product.

Responsibilities later:

- organization/repository installation;
- PR/check ingestion;
- draft or publish review/evidence checks with explicit policy;
- organization policy distribution;
- team audit metadata;
- connection to Team Server when enabled.

Rules:

- least-privilege permissions;
- clear data-access explanation;
- no source upload by default;
- local CLI remains independent of GitHub App authentication;
- no automatic merge/deploy;
- Marketplace listing only after manual/Action workflow proves value.

### 9. Optional Team Server

The Team Server is deferred until local CLI and PR workflow demonstrate paid demand.

Deployment forms:

```text
AgentsWatch Cloud — hosted SaaS
AgentsWatch Team Server — customer-managed/self-hosted later
Hybrid — local evidence, synced metadata only
```

Initial server scope should be metadata-first:

- organization/team/project identity;
- policy versions;
- capability/support summaries;
- commit/run/evidence hashes;
- validation/risk summaries;
- adapter/tool versions;
- audit history;
- licensing/entitlement metadata.

Not uploaded by default:

- source code;
- full prompts;
- full diffs;
- raw terminal output;
- secrets;
- full local event journals;
- private screenshots/assets.

Self-hosted support is an enterprise hypothesis and should not be built before paid team demand exists.

## Installation plan

### Phase I — .NET global/local tool

First supported installation:

```bash
dotnet tool install --global AgentsWatch.Cli
```

Pinned per-repository/team installation:

```bash
dotnet new tool-manifest
dotnet tool install AgentsWatch.Cli
dotnet tool run agentswatch --version
```

Use cases:

- early adopters;
- dogfood;
- CI proof;
- fast release iteration.

Limitation:

- requires an appropriate .NET runtime/SDK environment.

### Phase II — standalone self-contained executables

After CLI behavior and release proof stabilize, publish signed/checksummed standalone builds for:

```text
Windows x64 and arm64 where demand exists
Linux x64 and arm64 where demand exists
macOS x64 and arm64
```

Possible artifact forms:

- ZIP/tar archive;
- single-file executable where practical;
- installer only when it provides clear value.

Every runtime identifier must have:

- checksum;
- clean-install proof;
- help/version/smoke proof;
- capability/limitation snapshot;
- platform-specific known limitations.

### Phase III — package managers

After standalone release stability:

```text
WinGet or Scoop for Windows
Homebrew for macOS/Linux
NuGet/.NET tool remains supported
```

Package-manager publication must reference the same immutable release artifacts and checksums.

Do not maintain package channels that cannot be tested and updated reliably.

### Phase IV — GitHub Action distribution

Publish a versioned Action that pins or verifies the AgentsWatch CLI artifact.

Requirements:

- immutable major/minor tags and commit references;
- dependency/supply-chain review;
- fork/permissions scenarios;
- no hidden source upload;
- artifact/check retention policy;
- exact commit linkage.

### Phase V — optional service/dashboard installers

Only after the local service/dashboard exist:

- CLI-managed installation and upgrade where possible;
- service remains optional;
- platform service registration is explicit;
- uninstall preserves user-owned evidence by default;
- local API binding and authentication are documented.

### Phase VI — IDE marketplace distribution

Only after a thin IDE client has value:

- VS Code Marketplace first candidate;
- extension declares required local CLI/service version;
- no duplicated core logic;
- extension functions degrade safely when CLI/service is absent.

## Repository-local data plan

Canonical configuration/artifacts should converge on one product directory. The current repository uses `.ai` contracts; a future migration to `.agentwatch` must be an explicit compatibility decision, not an unannounced rename.

Possible long-term layout:

```text
.agentwatch/
  config.yml
  policy.yml
  runs/
  evidence/
  context/
  events/
  discoveries/
```

Until migration is approved, current `.ai` paths remain authoritative where documented.

Migration requirements:

- backward-compatible read;
- explicit migration command;
- no data loss;
- no overwrite of user-owned files;
- release notes and rollback guidance.

## One-product rules

1. One capability registry covers all components.
2. One runtime compatibility engine selects support modes.
3. One evidence schema links local, CI, and cloud metadata.
4. One policy model is compiled/adapted per surface.
5. One release version identifies compatible component versions where possible.
6. CLI functionality never depends on an IDE extension.
7. Local functionality never depends on a Team Server account.
8. Optional components must degrade safely when unavailable.
9. Adapters expose capability differences rather than forking the product.
10. A cloud edition does not replace or silently change local-first defaults.

## Commercial packaging hypothesis

### Community/Free

- local CLI core;
- Rules Compiler/basic drift report;
- basic context/evidence output;
- public-repository GitHub Action allowance;
- generic/manual adapters.

### Solo Pro

- advanced local history/dashboard;
- Context Resume;
- advanced Trust Ledger;
- Loop/usage analysis;
- regression canaries;
- supported premium adapters where justified.

### Team

- private-repository Action/App workflows;
- shared policies;
- PR evidence checks;
- team audit/history;
- metadata dashboard;
- organization compatibility reports.

### Enterprise

- SSO/RBAC;
- longer audit retention;
- managed policies;
- self-hosted/hybrid option if validated;
- custom adapter/support/SLA.

Pricing and feature boundaries remain hypotheses until user and willingness-to-pay evidence exists.

## Delivery roadmap

```text
Stage 0  Current CLI skeleton and proof
Stage 1  Useful local CLI and repository artifacts
Stage 2  Verified .NET tool release
Stage 3  Standalone executables and package-manager distribution
Stage 4  GitHub Action and PR Evidence workflow
Stage 5  Optional local service and dashboard
Stage 6  Thin IDE extension
Stage 7  GitHub App and Team beta
Stage 8  Hosted metadata Team Server
Stage 9  Enterprise self-hosted/hybrid only after demand
```

Stages may overlap for research, but a later stage must not become a dependency of an earlier local stage.

## Acceptance gates

### Local CLI gate

- clean install;
- no account/network requirement;
- local evidence and report value;
- Linux/Windows proof;
- safe behavior in dirty/no-git/read-only scenarios.

### Standalone distribution gate

- per-platform clean install;
- signatures/checksums;
- no missing runtime dependency surprises;
- upgrade/uninstall documentation.

### GitHub Action gate

- commit-bound evidence;
- fork and permission handling;
- no hidden uploads;
- useful report before automatic posting;
- public and private repository policy defined.

### Local service/dashboard gate

- explicit install/start/stop;
- local-only default;
- bounded storage/retention;
- CLI remains functional without it;
- no unsupported live monitoring claim.

### IDE extension gate

- thin client only;
- version compatibility and degraded mode;
- no duplicate policy/evidence implementation;
- clear local data behavior.

### Team Server gate

- paid team demand;
- data-minimization contract;
- tenant isolation;
- auth/RBAC;
- sync failure/offline behavior;
- export/delete/retention controls;
- no requirement to upload source.

## Non-goals

- separate full applications per provider;
- mandatory always-running daemon;
- mandatory desktop GUI;
- mandatory cloud account;
- hidden agent monitoring;
- automatic broad-permission enablement;
- cloud-only source of truth;
- microservices before product-market evidence;
- simultaneous support for every package manager/IDE/provider;
- self-hosted enterprise platform before customer demand.
