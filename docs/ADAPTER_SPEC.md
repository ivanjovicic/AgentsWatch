# AgentsWatch Adapter Specification

Last aligned: 2026-07-03  
Status: L1 target contract

## Purpose

Adapters make AgentsWatch useful across technology stacks, coding-agent tools, model arrangements, permission systems, execution environments, and delivery workflows without putting provider-specific behavior into the domain core.

## Core correction

Adapters are not only language/stack adapters.

AgentsWatch requires composable adapter families:

```text
Runtime tool adapter
Surface adapter
Model metadata adapter
Event/telemetry adapter
Permission adapter
Environment adapter
Repository/VCS/CI adapter
Technology-stack adapter
Rule-format adapter
Usage/cost adapter
```

The same model can run in different tools and surfaces. The same tool can run locally, in an IDE, in CI, or in a cloud container. Therefore adapters compose into an `EffectiveRuntimeProfile`; one provider name must not select all behavior.

See:

- `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`;
- `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`;
- `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`.

## Adapter principles

1. Domain logic depends on normalized contracts, not provider SDKs.
2. Adapters declare capabilities and blind spots explicitly.
3. Static declarations are verified by a dynamic handshake where possible.
4. Missing events mean `not observed`, not `did not happen`.
5. Unknown adapters fall back to universal git/file/manual behavior.
6. An adapter must not silently enable permissions, hooks, network, telemetry, or cloud sync.
7. Provider instruction files are advisory unless the target documents enforcement.
8. Adapter failures downgrade feature support rather than fabricate continuity.
9. Model identity, tool identity, and environment identity remain separate.
10. Local-first and redaction rules apply to every adapter.

## Adapter families

### 1. Runtime tool adapter

Describes the coding tool/product and its supported surfaces.

```text
IRuntimeToolAdapter
  AdapterId
  ToolId
  SupportedVersions
  SupportedSurfaces
  Detect(context)
  GetStaticCapabilities()
  Handshake(runtime)
```

Examples:

- Claude Code CLI;
- Codex CLI;
- Codex IDE;
- Codex cloud;
- GitHub Copilot cloud agent;
- Gemini CLI;
- Cline IDE/TUI/CLI;
- Aider terminal;
- generic chat/manual.

A surface-specific adapter may share common provider parsing code, but capability declarations remain surface-specific.

### 2. Surface adapter

Normalizes interaction mode:

```text
ChatOnly
LocalInteractiveCli
LocalIde
DesktopLocal
HeadlessCli
Sdk
CloudBackground
CiPullRequest
BrowserComputerUse
```

Owns:

- process ownership;
- interactive approval availability;
- local versus remote execution;
- pause/resume/stop availability;
- editor context availability;
- task/branch/PR identity.

### 3. Model metadata adapter

Owns only model-related facts:

- configured model/alias;
- resolved model/version when exposed;
- role: planner/editor/reviewer/worker;
- context budget where documented;
- reasoning mode/effort;
- usage metric availability;
- automatic routing possibility;
- edit/tool format hints.

It does not own permissions or tool-event capability.

For multi-model workflows, emit multiple model profiles and role lineage.

### 4. Event and telemetry adapter

Normalizes events into `AgentEventEnvelope`.

Capability declaration:

```text
CanObserveSessionLifecycle
CanObservePreTool
CanObservePostTool
CanObserveCommands
CanObserveExitCodes
CanObserveFileReads
CanObserveFileWrites
CanObserveApprovals
CanObserveCompaction
CanObserveSubagents
CanObserveWorktrees
CanObserveCwdChanges
CanObserveUsage
CanObserveEnvironmentSetup
CanObserveNetworkDecisions
SchemaStability
```

Rules:

- preserve provider raw event reference and adapter version;
- avoid relying on undocumented transcript internals as stable contracts;
- record out-of-order, duplicate, corrupt, and dropped events;
- run a hook/event handshake when possible;
- emit adapter-health changes into the run.

### 5. Permission adapter

Normalizes configured and effective operation rights:

```text
ReadFile
WriteFile
DeleteFile
ExecuteCommand
NetworkEgress
InstallPackage
UseBrowser
UseMcp
Commit
Push
WritePullRequest
ReadSecretReference
CallAuthenticatedService
ChangeInfrastructure
ChangeProduction
EscalatePrivilege
```

Every normalized permission includes:

- state: Allowed, Denied, ApprovalRequired, Unknown;
- source and precedence;
- scope/target;
- whether it is configured or empirically effective;
- expiry/session boundary.

A provider's `safe`, `auto`, `YOLO`, `full access`, or similar label is retained as provider metadata but is not the normalized security decision.

### 6. Environment adapter

Detects and records:

- OS, architecture, shell;
- local host, WSL, container, remote SSH, cloud, or CI;
- host/container/remote path mapping;
- workspace roots and mounts;
- image/setup/maintenance script identity;
- network mode/proxy/allowlist;
- state persistence and cache;
- toolchain versions;
- simulator/browser/service dependencies;
- credential classes, never raw values;
- trust class: development, test, staging, production-affecting.

Environment adapters must use non-destructive allow-listed probes.

### 7. Repository, VCS, and CI adapter

Owns:

- repository and worktree identity;
- branch/HEAD/base/merge base;
- clean/dirty/untracked state;
- local diff and file status;
- fork/PR/check/workflow identity;
- shallow/partial checkout state;
- CODEOWNERS/ownership metadata where configured;
- commit-bound evidence.

Initial adapters:

- universal git CLI;
- local no-git manifest fallback;
- GitHub PR/check adapter later;
- generic CI artifact import later.

### 8. Technology-stack adapter

Adds stack-specific knowledge without owning runtime permissions or provider events.

May provide:

- project detection;
- validation suggestions;
- high-risk paths;
- likely test folders;
- affected-project hints;
- report labels;
- post-run learning signals.

Adapters should not execute commands by default.

### 9. Rule-format adapter

Compiles canonical AgentsWatch policy/context into target formats.

Must declare per field:

```text
Exact
Equivalent
Weaker
AdvisoryOnly
Unsupported
ConflictWithManagedPolicy
```

Owns:

- target file shape;
- scope and precedence;
- generated-region ownership;
- target size limits;
- loss report;
- target-specific nesting behavior.

Does not claim that textual instructions equal hard enforcement.

### 10. Usage and cost adapter

Normalizes only metrics the provider/tool exposes:

```text
InputTokens
OutputTokens
CachedTokens
QuotaPercentage
CurrencyCost
RequestCount
ToolCallCount
WallClock
LocalComputeProxy
```

Each metric records:

- raw unit/value;
- measured versus estimated;
- source;
- time window;
- model/role attribution;
- confidence.

Unknown prices remain unknown. Quota percentage is not converted into token or currency estimates without a documented mapping.

## Composition and precedence

Adapters do not override one another arbitrarily.

```text
Tool/surface adapter declares possible capabilities
Environment adapter constrains what can work
Permission adapter resolves allowed operations
Event adapter proves observation availability
VCS/CI adapter supplies independent evidence
Stack adapter recommends validation
Rule adapter exports configuration
Usage adapter supplies measured consumption
```

Example:

```text
Tool declares workspace writes
Container mount is read-only
=> effective write denied

Project rule allows network
Managed policy denies network
=> effective network denied, conflict recorded

Hook is configured
Handshake fails
=> event observation downgraded

Model alias is auto-routed
Resolved identity unavailable
=> model-specific canary confidence capped
```

## Generic adapter capability contract

```csharp
public interface IAgentsWatchAdapter
{
    string AdapterId { get; }
    string AdapterVersion { get; }
    AdapterKind Kind { get; }
    AdapterStability Stability { get; }
    bool CanHandle(AdapterDetectionContext context);
}

public interface IRuntimeCapabilityProvider : IAgentsWatchAdapter
{
    StaticCapabilityDeclaration GetStaticCapabilities();
    Task<CapabilityHandshakeResult> HandshakeAsync(
        RuntimeProbeContext context,
        CancellationToken cancellationToken);
}

public interface IEnvironmentDetector : IAgentsWatchAdapter
{
    Task<EnvironmentDetectionResult> DetectAsync(
        EnvironmentProbeContext context,
        CancellationToken cancellationToken);
}

public interface IPermissionResolver : IAgentsWatchAdapter
{
    PermissionResolution Resolve(PermissionResolutionContext context);
}
```

## Universal repository adapter

Always available where git exists.

Detects:

- repository root;
- changed files;
- clean/dirty worktree;
- staged and untracked files;
- branch, HEAD, merge base where available;
- worktree path.

Suggested evidence commands:

```bash
git status --short -uall
git diff --stat
git diff
git diff --cached
git rev-parse HEAD
git merge-base <base> HEAD
```

High-risk patterns:

- many files changed;
- runtime changes without matching tests;
- config/secrets changed;
- generated/binary files changed;
- ownership boundary exceeded;
- evidence commit mismatch.

No-git fallback:

- file manifest and hashes;
- user-provided diff;
- command evidence;
- lower proof confidence;
- no worktree or PR claims.

## Stack adapters

### .NET

Detects:

- `*.sln`, `*.slnx`, `*.csproj`;
- `Directory.Build.props`;
- `Directory.Packages.props`;
- C# source/test projects.

Validation ladder:

```bash
dotnet restore
dotnet build
dotnet test <targeted-project-or-filter>
dotnet test
```

Environment adaptations:

- restore can be blocked by network/private feed credentials;
- Windows/WSL/container SDK and path behavior are distinct;
- database/integration tests may require services or credentials;
- migrations, deployment projects, signing, and production config elevate risk.

High-risk patterns:

- migrations;
- auth/security;
- dependency injection composition;
- public API contracts;
- configuration;
- project references;
- package lock/version changes.

### Flutter

Detects:

- `pubspec.yaml`;
- `lib/`, `test/`, integration tests;
- Dart source;
- Android/iOS/web/desktop platform folders.

Validation ladder:

```bash
flutter analyze
flutter test <targeted-test>
flutter test
```

Adaptations:

- widget/UI behavior needs widget/integration/manual evidence;
- simulator/emulator/device/browser is a resource/environment capability;
- platform build can require macOS/Xcode or Android tooling;
- navigation, provider/state, persistence, and platform changes require different validation;
- singleton emulators may require worker serialization.

Token-saving rules:

- no whole-app inspection for one widget;
- targeted tests before broad suite;
- broad platform/integration validation only when justified.

### React/TypeScript

Detects:

- `package.json`;
- TypeScript/TSX;
- Vite/Next/other config;
- test/lint scripts.

Suggested validation is derived from actual package scripts, not assumed blindly:

```text
build
lint
typecheck
targeted tests
full tests
```

Adaptations:

- package-manager and lockfile detection;
- browser/E2E dependencies;
- environment variables and generated clients;
- monorepo affected-package scope;
- private registries and network constraints.

High-risk patterns:

- API clients/contracts;
- route/auth guards;
- state management;
- dependency/lock changes;
- environment/config;
- generated clients.

### Python

Detects:

- `pyproject.toml`, lockfiles, requirements;
- source/tests;
- environment/tool configs.

Suggested validation from configured tools:

```text
pytest targeted/full
ruff or configured linter
mypy/pyright or configured type checker
```

Adaptations:

- venv/container/interpreter identity;
- native dependencies;
- network/private indexes;
- notebooks and generated data;
- background jobs/services;
- migrations/auth/IO.

### Node/JavaScript

Detects package manager, scripts, JS module mode, workspace layout, and tests.

Adaptations mirror React where applicable and must account for:

- npm/pnpm/yarn/bun differences;
- lockfile and lifecycle scripts;
- network/install permissions;
- native modules;
- monorepo workspaces.

### Mixed/monorepo

Multiple stack adapters may match.

Composition rules:

- detect roots/packages;
- calculate affected boundaries;
- merge risk findings;
- deduplicate validation;
- preserve cross-stack contracts;
- avoid broad validation unless dependency graph justifies it.

## Adapter output shape

```text
Runtime:
  tool/surface/version
  model roles
  observation level
  process ownership

Permissions:
  configured/effective rights
  approvals
  network/credential/production class

Environment:
  OS/runtime/local-remote-cloud
  workspace/worktree
  image/setup/cache/network

Repository:
  git/no-git
  commit/base/dirty state
  roots/ownership

Stacks:
  detected projects
  targeted validation
  risks

Support decisions:
  capability -> Full|Guarded|Advisory|PostHoc|Manual|Unavailable
  blind spots
  fallback
  confidence cap
```

## Unknown and conflicting adapters

When detection is uncertain:

- report `Unknown`;
- use universal git/file/manual behavior;
- prohibit enforce mode;
- do not invent commands;
- generate a focused adapter discovery;
- preserve conflicting sources.

## Adapter versioning

Every adapter output includes:

- adapter ID/version;
- supported tool version range;
- schema version;
- detection timestamp;
- source stability;
- last successful handshake;
- known limitations.

Tool updates trigger a new handshake. A capability removed by a tool update must downgrade immediately.

## MVP and implementation order

1. Keep current universal git and stack detection.
2. Implement runtime profile schema.
3. Add static generic/manual and local-wrapper adapters.
4. Add environment and permission fixtures.
5. Add two official hook/event adapters.
6. Add one cloud/PR evidence adapter.
7. Add rule-format adapters.
8. Add usage adapters only for documented metrics.

No live provider adapter is required for the first Rules Compiler or manual Context Snapshot prototype.
