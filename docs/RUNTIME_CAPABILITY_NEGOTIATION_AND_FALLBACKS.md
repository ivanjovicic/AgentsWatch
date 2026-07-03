# AgentsWatch Runtime Capability Negotiation and Fallbacks

Last aligned: 2026-07-03  
Status: L1 runtime compatibility contract; no implementation implied

## Purpose

Define how AgentsWatch determines what it can safely observe, verify, advise, or enforce in a specific coding-agent session.

## Core rule

```text
Do not select behavior by provider or model name alone.
Select behavior from the effective runtime profile.
```

A declared configuration is not enough. AgentsWatch must distinguish:

- what the adapter says it supports;
- what the user configured;
- what the environment actually exposes;
- what was observed during this run;
- what evidence remains missing.

## Effective runtime profile

```text
EffectiveRuntimeProfile
  ProfileSchemaVersion
  ProfileId
  DetectedAtUtc
  RepositoryIdentity
  WorkspaceIdentity
  ToolProfile
  SurfaceProfile
  ModelProfiles[]
  ObservationCapabilities
  ActionCapabilities
  PermissionProfile
  EnvironmentProfile
  VersionControlProfile
  UsageTelemetryProfile
  InstructionProfile
  ControlCapabilities
  DataHandlingProfile
  CapabilitySources[]
  Conflicts[]
  BlindSpots[]
  Confidence
```

## Capability value model

Each capability uses a state plus provenance.

```text
CapabilityState
  Native          — officially exposed by the host tool/surface
  Wrapped         — observed or controlled because AgentsWatch owns a process/wrapper
  Derived         — inferred from git, filesystem, CI, or correlated evidence
  UserSupplied    — supplied or attested by the user
  Unavailable     — confirmed unavailable in this profile
  Unknown         — not established
  Conflicted      — sources disagree
```

Provenance:

```text
CapabilitySource
  AdapterDeclaration
  ToolConfiguration
  ManagedPolicy
  CommandLine
  EnvironmentProbe
  HookHandshake
  ProcessOwnership
  ProviderExport
  GitEvidence
  CiEvidence
  UserInput
```

Confidence:

```text
Verified
High
Medium
Low
Unknown
```

A user-configured setting is not `Verified` until the effective behavior or supported configuration surface confirms it.

## Feature support modes

```text
Full
  Required observation and control surfaces exist.
  Known material bypasses are absent or explicitly covered.

Guarded
  Useful observation/control exists, but gaps or equivalent bypass paths remain.

Advisory
  AgentsWatch can classify risk and recommend actions but cannot enforce.

PostHoc
  AgentsWatch can analyze completed logs, git, CI, PR, or artifacts.

Manual
  The user must provide claims, logs, or results.

Unavailable
  The feature cannot provide a meaningful result in the current profile.
```

A feature decision must never silently upgrade. It may downgrade when a hook fails, environment changes, permission changes, or an adapter becomes unhealthy.

## Support decision model

```text
FeatureSupportDecision
  CapabilityId
  RequestedOperation
  EffectiveMode
  RequiredCapabilities[]
  SatisfiedCapabilities[]
  MissingCapabilities[]
  ConflictedCapabilities[]
  BlindSpots[]
  ConfidenceCap
  FallbackPlan
  UserActionRequired[]
  UnsafeToProceed
  DecisionReason
```

## Runtime detection sequence

### Step 1 — Identify repository and workspace

Detect:

- repository root or no-git state;
- workspace roots;
- worktree identity;
- branch, HEAD, merge base, dirty state;
- remote/cloud task and PR identity when available;
- host and container paths.

Stop or downgrade when two roots map ambiguously to the same path.

### Step 2 — Identify tool and surface

Record separately:

```text
Provider
Product/tool
Client version
Surface: CLI | IDE | Desktop | Cloud | CI | SDK | Chat
Interactive/headless
Local/remote
Process owned by AgentsWatch: yes/no
```

Do not map `Codex`, `Claude`, `Gemini`, `Copilot`, or another provider to one static profile.

### Step 3 — Identify model roles

Record zero or more model profiles:

```text
ModelProfile
  Provider
  ModelIdOrAlias
  ResolvedModelId if available
  Version/snapshot if available
  Role: Planner | Editor | Reviewer | Validator | Coordinator | Worker
  ReasoningMode/effort
  ContextBudget
  UsageTelemetryAvailability
  AutomaticRoutingPossible
```

When the exact model is hidden or automatically routed, record `Unknown` or alias plus routing risk.

### Step 4 — Handshake with event adapters

Each runtime adapter returns:

```text
CanObserveSessionLifecycle
CanObservePrompts
CanObservePreTool
CanObservePostTool
CanObserveCommands
CanObserveExitCodes
CanObserveFileReads
CanObserveFileWrites
CanObserveApprovals
CanObserveUsage
CanObserveCompaction
CanObserveSubagents
CanObserveWorktrees
CanObserveCwdChanges
CanObserveEnvironmentSetup
CanObserveNetworkDecisions
CanBlockTool
CanRewriteTool
CanPause
CanResume
CanStop
CanCreateCheckpoint
EventSchemaStability
```

The adapter must also run a lightweight health/handshake test where possible. A configured hook that does not fire is not treated as active.

### Step 5 — Resolve effective permissions

Separate permissions by operation:

```text
FilesystemRead
FilesystemWrite
Delete
CommandExecute
NetworkEgress
PackageInstall
Browser
McpTools
GitCommit
GitPush
PullRequestWrite
SecretsRead
ExternalServiceWrite
InfrastructureChange
ProductionChange
PrivilegeEscalation
```

Resolution precedence is adapter-specific. AgentsWatch stores the sources and conflict, not one universal precedence assumption.

### Step 6 — Identify execution environment

Record:

- OS, architecture, shell;
- local host, WSL, container, SSH, cloud, or CI;
- image/setup/maintenance script hashes;
- network mode;
- mount roots;
- state persistence and cache state;
- toolchain versions;
- simulator/browser/service dependencies;
- environment owner and trust level.

### Step 7 — Compute support modes

For each requested AgentsWatch capability, evaluate required observation/control/evidence and choose the strongest honest mode.

### Step 8 — Persist the profile

Write a compact profile into the run evidence. Any later profile change creates a new profile revision and invalidates assumptions that depend on the changed field.

## Capability requirements and fallbacks

### AW-CAP-028 — Flight Recorder

Required for Full:

- O4+ event observation;
- stable run/session/workspace identity;
- command/file/action outcomes;
- adapter health.

Fallbacks:

```text
O6 -> Full lifecycle including subagent/worktree/compaction
O4/O5 -> Guarded observed-tool timeline
O2/O3 -> PostHoc git/log timeline
O1 -> Manual final-response record
O0 -> Unavailable
```

Confidence cap must state whether file reads, hidden context assembly, browser/computer use, and equivalent tool paths are observable.

### AW-CAP-029 — Trust Ledger

Requirements are claim-specific.

Examples:

```text
Claim: file changed
  Preferred: git/file hash evidence
  Fallback: user-supplied patch

Claim: tests passed
  Preferred: CI/check bound to commit or captured command result
  Fallback: user attestation -> Low confidence

Claim: UI works
  Preferred: automated UI test or reproducible manual evidence
  Build success alone is insufficient

Claim: no unrelated files changed
  Requires start/end ownership-aware file evidence
```

Trust Ledger remains useful in PostHoc mode even without agent integration.

### AW-CAP-030 — Context Snapshot and Resume

Requirements:

- task/run source;
- repository/environment state where relevant;
- target adapter semantics for export.

Fallbacks:

- unsupported tool -> generic markdown handoff;
- unknown model context budget -> conservative compact export;
- managed policy conflict -> report and avoid overwrite;
- chat-only -> copy-ready manual pack;
- cloud agent -> include setup/branch/environment references.

### AW-CAP-031 — Rules Compiler and Drift Detector

Requirements:

- target format adapter;
- scope/precedence model;
- ownership marker or non-destructive merge strategy.

Fallbacks:

```text
Exact target semantics -> generated target file
Partial semantics -> generate supported fields + loss report
No target adapter -> generic AGENTS.md/human policy
Managed target -> lint only, no write
Read-only workspace -> proposed patch/output only
```

### AW-CAP-032 — Cost and Loop Guard

Subcapabilities are decided separately:

```text
RepeatedActionDetection
ProgressDeltaDetection
TokenUsage
MonetaryCost
QuotaPercentage
LiveWarning
LivePause
LiveStop
CheckpointBeforeStop
```

Fallbacks:

- no usage telemetry -> time/event/action metrics only;
- provider plan percentage only -> do not infer token count or money;
- local model -> wall-clock/compute proxy, no fabricated provider price;
- cloud opaque task -> PostHoc loop analysis;
- no process ownership -> recommendation only;
- no checkpoint -> prohibit automatic stop.

### AW-CAP-033 — Policy Firewall

Requirements by enforcement class:

```text
PE1 Preflight: requested operation input
PE2 Approval advice: request/approval event or explicit CLI preflight
PE3 Hook blocking: supported pre-tool hook plus coverage declaration
PE4 Wrapper enforcement: AgentsWatch owns command/tool execution
PE5 Environment enforcement: supported OS/container/network policy integration
```

Fallbacks:

- no hook/wrapper -> Advisory;
- cloud task -> lint provider config/workflow and audit results;
- incomplete hook coverage -> Guarded with bypass disclosure;
- admin policy conflict -> stricter effective policy wins, conflict shown;
- unknown effective permission -> fail closed only when the user selected enforce mode; otherwise Advisory with warning.

### AW-CAP-034 — Multi-Agent Coordinator

Coordinator mode selection:

```text
NativeWorktree
ManagedWorktree
CloudBranchPR
SharedWorkspaceOwnership
MessageOnly
Unavailable
```

Fallbacks:

- no subagent API -> generate worker task packs only;
- no worktree support but branches/PRs exist -> CloudBranchPR;
- no git -> message/status coordination only;
- shared workspace -> ownership/conflict warnings, no isolation claim;
- singleton resource -> serialize workers with a resource lease.

### AW-CAP-035 — PR Review Debt Reducer

Fallbacks:

- PR/check API available -> Full/GitHub-integrated report;
- local git range -> local review packet;
- dirty worktree -> uncommitted review packet with lower provenance;
- no git -> manual file manifest/diff import;
- checks inaccessible -> `Unavailable`, not failed;
- fork secrets unavailable -> environment-blocked category.

### AW-CAP-036 — Regression Canary

Requirements:

- stable test task and success criteria;
- complete comparison profile;
- repeated runs where stochastic behavior matters.

Fallbacks:

- hidden model routing -> compare tool profile, not pure model;
- environment changed -> mark confounded;
- no usage telemetry -> omit cost metric;
- chat-only -> manual benchmark pack;
- cloud setup changed -> invalidate direct baseline comparison.

### AW-CAP-037 — Runtime compatibility negotiation

This cross-cutting capability produces the profile and decisions above. Until implemented, all advanced feature support remains manually declared/specification-only.

## Static versus dynamic capabilities

### Static

Usually known from adapter/tool version:

- event names exposed;
- target rule format;
- supported surfaces;
- platform support;
- documented permission concepts.

### Dynamic

Must be determined per run:

- active hooks;
- current approval mode;
- effective network access;
- current workspace/worktree;
- model alias resolution;
- process ownership;
- secret availability;
- provider outage or missing telemetry;
- cloud cache/setup state;
- user-granted escalation.

A static declaration cannot override a failed dynamic handshake.

## Conflict resolution

Examples:

```text
Tool says workspace-write, OS mount is read-only
  -> effective write unavailable

Project policy allows network, managed policy denies
  -> effective network denied

Hook configured, handshake/event missing
  -> observation capability Unknown/Unavailable

Model alias says auto, runtime reports resolved model
  -> retain alias and resolved identity

Provider says tests passed, CI for another commit passed
  -> claim remains Missing/Contradicted for current commit

User selected YOLO/full access, sandbox blocks path
  -> requested broad, effective restricted
```

## Profile change handling

A new profile revision is required when:

- model or reasoning mode changes;
- permission/approval mode changes;
- cwd/worktree changes;
- session moves local to cloud or cloud to local;
- container/image/setup changes;
- network mode changes;
- hook/adapter health changes;
- secret scope changes;
- repository HEAD/base changes materially.

Feature decisions after the change use the new revision. Reports show which events belong to each revision.

## User-facing compatibility report

Planned command:

```bash
agentswatch compatibility detect
agentswatch compatibility explain <capability-id>
agentswatch compatibility compare <profile-a> <profile-b>
agentswatch compatibility export --format markdown|json
```

Example output:

```text
Runtime profile: RP-2026-07-03-001
Tool/surface: Claude Code CLI local
Model: configured alias; resolved model unavailable
Workspace: git worktree / project-A
Environment: macOS sandbox
Permissions: read/write workspace; network allowlist; no production credentials
Observation: O6 except screen/browser content

AW-CAP-028 Flight Recorder: Full
AW-CAP-029 Trust Ledger: Full for git/build/test claims; Manual for visual behavior
AW-CAP-032 Live Stop: Guarded
AW-CAP-033 Policy Firewall: Guarded PE3
Known blind spot: equivalent action paths outside supported hook coverage
```

## Configuration contract

Possible future local config:

```yaml
compatibility:
  detection: auto
  fail_on_unknown_enforcement: true
  allow_manual_overrides: true
  require_override_reason: true
  cache_profile_minutes: 10

profiles:
  production:
    max_policy_enforcement: advisory
    require_human_approval: true
    prohibit_live_stop_without_checkpoint: true

adapters:
  codex:
    enabled: true
  claude-code:
    enabled: true
```

Manual overrides cannot claim higher confidence than the evidence supports. They record author, reason, and expiry.

## Security rules

- Capability detection must not read secret values.
- Environment probes are allow-listed and non-destructive.
- Do not enumerate the entire host filesystem.
- Do not call remote APIs unless the user enabled the integration.
- Do not automatically weaken vendor sandbox settings.
- Do not turn on YOLO/full-access modes.
- Do not install hooks without explicit user action and a preview.
- A missing compatibility profile blocks enforce-mode features.
- A profile marked production-affecting blocks autonomous actions.

## Test strategy

Required fixture categories:

- local CLI rich hooks;
- local CLI no hooks;
- IDE local;
- cloud PR agent;
- headless CI;
- chat-only;
- read-only filesystem;
- workspace-write/no-shell;
- shell/no-network;
- network/no-secrets;
- WSL/native Windows;
- container/nested container;
- remote SSH;
- worktree;
- monorepo/multi-root;
- no-git;
- model switch;
- multi-model architect/editor;
- managed policy conflict;
- adapter failure mid-run.

## Proof gate

AW-CAP-037 may reach:

- L2 when schema, detectors, and decision engine exist;
- L3 when fixture tests cover all required categories;
- L4 when black-box CLI scenarios run on Linux and Windows and at least two real tool adapters provide handshake evidence;
- L5 when dogfood demonstrates correct downgrades/fallbacks across local, cloud/PR, and chat/manual workflows;
- L6 only after packaged independent verification confirms that unsupported modes are never advertised as Full.
