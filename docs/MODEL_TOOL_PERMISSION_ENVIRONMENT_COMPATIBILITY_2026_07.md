# AgentsWatch Model, Tool, Permission, and Environment Compatibility — July 2026

Last aligned: 2026-07-03  
Status: compatibility research and product contract; no runtime support implied

## Executive answer

No. The planned AgentsWatch capabilities are **not equally available or equally enforceable** across all models, coding tools, permission levels, and execution environments.

The product concepts are broadly reusable, but their operational depth varies:

```text
The model influences reasoning, context, editing reliability, and cost.
The host tool determines which actions and events exist.
The session surface determines what AgentsWatch can observe or control.
Permissions determine what the agent may do.
The environment determines where actions execute and which evidence is available.
The repository and delivery workflow determine how completion can be proved.
```

AgentsWatch must never infer support from a model name alone. It must negotiate an **effective runtime profile** from all relevant dimensions and select one of these support modes:

```text
Full       — observable and enforceable through a supported adapter or wrapper
Guarded    — observable and partially enforceable; known bypasses or blind spots exist
Advisory   — AgentsWatch can evaluate/advise but cannot enforce
PostHoc    — analysis is possible only after logs, git, CI, or PR evidence exists
Manual     — the user must export or enter evidence
Unavailable — required evidence or control surface does not exist
```

## Why model name is not enough

The same underlying model can appear in:

- a chat-only website with no repository or shell access;
- a local CLI with filesystem and terminal tools;
- an IDE extension with open-file and selection context;
- an autonomous cloud agent working in an ephemeral container;
- a CI/GitHub agent limited to a branch and pull request;
- an orchestration tool that separates architect and editor models;
- a local model running fully offline;
- a managed enterprise installation with policy-enforced restrictions.

Conversely, one coding tool can support multiple models with different context limits, reasoning behavior, edit formats, tool-use reliability, prices, and latency.

Therefore:

```text
Compatibility = model profile
              + host-tool profile
              + surface profile
              + event/telemetry profile
              + permission profile
              + environment profile
              + repository/delivery profile
```

## Research observations from current tools

### Claude Code

Claude Code exposes a rich hook lifecycle, including pre/post tool calls, permission requests, compaction, session, subagent, worktree, configuration, current-directory, and file-change events. Some hooks can deny or rewrite tool calls, while others such as subagent start can inject context but cannot prevent creation.

Its sandbox behavior is platform- and configuration-dependent. Sandbox enforcement, filesystem paths, domains, sockets, local ports, simulator/XPC access, weaker nested-container modes, and enterprise lock settings can differ between macOS, Linux, WSL2, project settings, user settings, and managed settings.

Implication:

- Flight Recorder can be rich when hooks are enabled;
- Policy Firewall can be guarded/enforcing for intercepted tool paths;
- it must disclose hook gaps and sandbox escape/exception paths;
- iOS simulator, Docker, Playwright, Unix sockets, and nested-container workflows need environment-specific policies;
- subagent creation and worktree lifecycle need separate capability flags.

### OpenAI Codex

Codex currently spans CLI, IDE, app/local worktrees, cloud environments, non-interactive automation, and other surfaces. Local permission profiles distinguish read-only, workspace write, and unrestricted modes, with platform-specific caveats. Codex hooks expose several lifecycle events, but official documentation explicitly notes that current pre-tool interception is not a complete enforcement boundary and does not intercept every equivalent action path.

Codex cloud uses prepared/cached container environments; setup and agent phases have different behavior. Agent-phase internet is disabled by default and can be enabled with domain/method restrictions. Local, IDE, worktree, and cloud sessions therefore cannot share one static permission or evidence assumption.

Implication:

- local hook-enabled Codex may support guarded Flight Recorder and Policy Firewall;
- Codex cloud needs an environment/PR evidence adapter rather than local process control;
- worktree handoff must preserve environment identity and ignored-file behavior;
- cloud cache and setup/maintenance scripts become part of reproducibility evidence;
- internet-off and allow-listed modes require different validation and package-install expectations.

### GitHub Copilot cloud agent

The cloud agent works in its own ephemeral GitHub Actions-based environment, can explore code, change files, and run checks, and returns work through a branch/pull-request workflow. Repository setup is controlled by a special workflow and runner constraints. This is different from Copilot chat, IDE assistance, and local CLI operation.

Implication:

- local process termination and local filesystem policy are unavailable for a cloud task;
- Flight Recorder must ingest session logs, commits, checks, and PR evidence;
- Policy Firewall is mostly preconfigured workflow/repository policy plus post-hoc verification;
- Worktree Coordinator should map the cloud task to branch/PR isolation rather than assume local git worktrees;
- environment setup failure and unavailable private dependencies must be represented as environment evidence, not agent failure alone.

### Gemini CLI

Gemini CLI has layered system, user, project, environment, and command-line configuration; enterprise/system settings can override lower scopes. It supports multiple approval modes, including read-only planning, auto-edit, default approval, and command-line YOLO behavior. It also includes optional checkpointing and can route different models between planning and implementation.

Implication:

- rules compilation must preserve configuration precedence and identify admin-owned settings;
- model identity may change inside one task phase;
- a single run may need separate planner and implementer model records;
- approval mode changes the effective Policy Firewall mode;
- checkpoint availability affects rollback assurance.

### Cline

Cline exposes separate permissions for project/all-file reads and edits, safe/all commands, browser, and MCP. Its command safety decision is model-produced rather than a fixed deterministic allowlist. YOLO mode can auto-approve broad file, command, browser, and MCP activity. Cline checkpoints use a shadow Git repository and can include untracked files, but may create storage/performance costs for large repositories.

Implication:

- AgentsWatch must not equate the tool's `safe` label with independent policy proof;
- checkpoint evidence should be imported as a provider artifact, not confused with the user's main git history;
- YOLO sessions require a higher risk profile even when changes remain recoverable;
- large-repository storage and checkpoint frequency need environment-aware limits.

### Aider and other multi-model editing tools

Aider separates ask, code, and architect modes. Architect mode can use one model to propose changes and a second model to translate them into file edits, increasing requests, latency, and cost while potentially improving editing for models that reason well but edit poorly.

Implication:

- one logical task can contain multiple model roles;
- cost and canary reports must attribute planner and editor separately;
- Trust Ledger must link the edit evidence to the editor phase while retaining the architect proposal lineage;
- a read-only ask phase cannot be evaluated as though it had write permissions.

### Generic chat-only models

A chat interface may provide code suggestions but no structured tool events, repository identity, command execution, or direct file edits.

Implication:

- Context Pack, prompt lint, Rules Compiler, and manual claim review remain useful;
- live Flight Recorder, live Loop Guard, Policy enforcement, and Worktree coordination are unavailable;
- git/CI evidence can still support post-hoc Trust Ledger analysis after the user applies changes;
- the UI must say `Manual` or `PostHoc`, not imply invisible monitoring.

## Compatibility dimensions

### M — Model profile

AgentsWatch should record capabilities rather than rank vendors by name.

```text
M0 Unknown/general chat model
M1 Code-capable generation model
M2 Reliable structured-tool-use model
M3 Reasoning/planning-oriented model
M4 Editing/patch-specialized model
M5 Local/offline model
M6 Multi-model role composition
M7 Multimodal/computer-use model
```

Relevant properties:

- context-window and practical context budget;
- tool-calling and structured-output reliability;
- edit/patch format support;
- reasoning effort or mode;
- deterministic seed/settings when available;
- vision/computer-use availability;
- local versus hosted execution;
- provider usage/cost telemetry;
- model role: planner, editor, reviewer, validator, coordinator, worker;
- model/version mutability during a run.

Rules:

- do not infer permissions from the model;
- do not infer event availability from the model;
- do not compare canary results across different model roles as equivalent;
- record every model switch inside a run;
- separate model-reported usage from independently observed usage;
- treat provider aliases and automatic routing as potentially changing identities.

### T — Host tool and surface profile

```text
T0 Chat-only/manual
T1 Local interactive CLI
T2 IDE extension using local workspace
T3 Desktop app using local workspace
T4 Headless CLI/SDK/non-interactive process
T5 Cloud background agent
T6 CI/GitHub PR agent
T7 Agent orchestrator with subagents
T8 Browser/computer-use agent
T9 Custom MCP/tool-hosted agent
```

Surface affects:

- event lifecycle;
- process ownership;
- ability to pause/stop/resume;
- file and editor context;
- local versus remote paths;
- branch/worktree behavior;
- user approval UX;
- access to provider transcripts/logs;
- whether AgentsWatch can wrap the process.

### O — Observation profile

```text
O0 No observable events
O1 User-supplied final response only
O2 Git/diff/PR/CI evidence only
O3 Transcript or session-log export
O4 Post-tool events and command outcomes
O5 Pre- and post-tool lifecycle with approvals
O6 Full session/subagent/worktree/compaction lifecycle
```

Observation quality controls the maximum confidence of:

- Flight Recorder;
- Trust Ledger;
- Loop Guard;
- cost attribution;
- context-loss detection;
- multi-agent lineage.

An absent event means `not observed`, not `did not happen`.

### P — Permission profile

```text
P0 No repository access
P1 Repository metadata / PR read only
P2 Source read only
P3 Workspace file write
P4 Restricted command execution
P5 Broad command execution
P6 Restricted network/package access
P7 Secrets or authenticated service access
P8 Privileged host/container/cloud access
P9 Production-affecting access
```

Permissions are additive dimensions, not one linear switch. A session can have workspace writes but no shell, shell but no network, network but no secrets, or PR write without local filesystem control.

AgentsWatch must distinguish:

- configured permission;
- effective permission;
- observed action;
- requested escalation;
- approved escalation;
- denied escalation;
- provider sandbox boundary;
- AgentsWatch policy boundary.

### E — Execution environment profile

```text
E0 No execution environment / chat only
E1 Local host checkout
E2 Local sandbox
E3 Local container or dev container
E4 WSL2 / cross-OS environment
E5 Remote SSH development host
E6 Codespace or remote development container
E7 Cloud ephemeral agent environment
E8 CI runner
E9 Git worktree
E10 Multi-root workspace or monorepo
E11 Air-gapped / offline environment
E12 Corporate proxy / allow-listed network
E13 Simulator/emulator/browser-dependent environment
E14 Privileged infrastructure or production environment
```

Environment-specific evidence should include:

- OS, architecture, shell, container/runtime;
- local/remote/cloud identity;
- repository root and workspace roots;
- worktree/branch/commit;
- environment image or setup-script hash;
- installed toolchain versions;
- network mode and allowlist;
- credential availability class, never raw values;
- filesystem mount/read/write boundaries;
- simulator/browser/service dependencies;
- cache state and provenance;
- whether state survives the session.

### V — Repository and delivery profile

```text
V0 No VCS
V1 Local git checkout
V2 Dirty checkout with uncommitted work
V3 Git worktree
V4 Fork/branch contribution flow
V5 PR-only cloud delivery
V6 Monorepo/multi-root ownership
V7 Generated/binary-heavy repository
V8 Shallow clone or partial checkout
V9 Read-only mirror
```

This determines whether AgentsWatch can prove scope, calculate diffs, restore state, coordinate workers, or link evidence to a commit.

## Feature applicability matrix

Legend:

```text
F Full or near-full support is feasible
G Guarded/partial enforcement with declared blind spots
A Advisory only
H Post-hoc evidence analysis
M Manual workflow
N Not meaningfully available
```

| Capability | Local CLI with hooks | IDE local agent | Cloud/PR agent | CI/headless | Chat-only | Read-only session | No-git workspace |
|---|---:|---:|---:|---:|---:|---:|---:|
| Flight Recorder | F/G | G | H/G | F/G | M | G/H | G |
| Trust Ledger | F | F/G | F/H | F | H/M | F/H | G/M |
| Context Snapshot/Resume | F | F | G/H | G | M/F export | F | F |
| Rules Compiler/Drift | F | F | A/H | F | M | A/H | F |
| Cost/Usage Ledger | G | G | G/H | G | M | G | G |
| Loop Detection | F/G | G | H/G | F/G | M | H/G | G |
| Live Loop Stop | G | G | N/A | G if wrapper owns process | N | N | G |
| Policy Firewall dry-run | F | F | A/H | F | M | F/A | F |
| Policy enforcement | G/F by adapter | G | N/A or vendor policy only | G/F | N | N | G/F |
| Worktree Coordinator | F | F/G | branch/PR variant | F/G | N | planner only | N |
| PR Review Debt Reducer | F/H | F/H | F | F | H/M | F/H | N/M |
| Regression Canary | F | F/G | G | F | M | F for read tasks | F with non-git criteria |
| Workspace Doctor | F | F | H/A | F | N | A/H | F |

The matrix is a feasibility contract, not a statement that current code implements these modes.

## Required adaptations by planned capability

### 1. Agent Flight Recorder

Universal semantic goal:

```text
Record what can be observed and disclose what cannot.
```

Full requirements:

- stable run/session identity;
- pre/post tool or process events;
- command outcomes;
- file-change or git evidence;
- permission/approval events;
- timestamps and workspace identity.

Adaptations:

- rich-hook local tools: ingest structured lifecycle events;
- local CLI without hooks: wrap process and combine filesystem/git/process evidence;
- IDE-only surface: ingest extension events where available and use git/IDE state for gaps;
- cloud agent: ingest task/session logs, commits, PR, checks, and environment metadata;
- chat-only: accept final response and user-provided artifacts only;
- computer-use agents: record high-level screen/action evidence only when the platform exposes it; do not use hidden screen capture;
- privacy-restricted environment: store metadata/hashes and local references, not content.

Confidence cap:

- O0/O1 cannot produce `full timeline`;
- O2 can prove repository outcomes, not all agent actions;
- O4/O5 can prove observed tool paths only;
- O6 supports subagent/worktree/compaction lineage.

### 2. Trust Ledger

This is more portable than Flight Recorder because it can use independent evidence.

Evidence grades:

```text
E0 Agent statement only
E1 User attestation
E2 Git/diff artifact
E3 Command exit/output summary
E4 CI/check result bound to commit
E5 Reproducible black-box result
E6 Independently repeated result
```

Adaptations:

- chat-only: compare claims to later git/CI evidence;
- cloud PR agent: bind claims to PR head/check-run commit;
- dirty local checkout: separate pre-existing changes from run-owned changes;
- no-git workspace: use file manifests/hashes and command evidence;
- GUI/mobile application: require screenshot/UI-test/manual verification category rather than claiming runtime behavior from build success;
- production/infra: completion requires approval and environment evidence; never treat command success as safe deployment proof.

### 3. Context Snapshot and Session Rescue

Broadly applicable, but export is lossy and target-dependent.

Adaptations:

- target instruction-file precedence differs by tool;
- context-size budget differs by model and provider;
- IDE targets may use selected/open-file context;
- cloud agents need environment/setup/branch data;
- local models may need smaller context and no remote links;
- multimodal tasks need references to local assets without blindly embedding them;
- multi-model architect/editor workflows need role-specific snapshots;
- managed enterprise settings may override project-generated rules.

Every export must include:

```text
Represented fields
Dropped fields
Unsupported semantics
Target precedence assumptions
Content budget/truncation
Sensitive content omitted
Source snapshot/hash
```

### 4. Rules Compiler and Drift Detector

This feature is widely useful but not universally enforceable.

Adaptations:

- compile to target-specific scopes and precedence;
- distinguish advisory instruction files from hard permission settings;
- never overwrite admin/managed settings;
- detect nested rule files in monorepos;
- preserve user-owned text outside generated regions;
- warn when a target cannot express network, path, approval, or subagent constraints;
- compile separate profiles for local, cloud, CI, and read-only use;
- support a generic human-readable fallback for unsupported tools.

Required output status:

```text
Exact
Equivalent
Weaker
AdvisoryOnly
Unsupported
ConflictWithManagedPolicy
```

### 5. Cost and Loop Guard

Loop detection is more portable than cost accounting.

Adaptations:

- provider token metrics may be exact, estimated, plan-percentage-only, or unavailable;
- multi-model workflows require per-role attribution;
- cloud background work may lack real-time events and support only post-hoc detection;
- IDE tools may perform hidden context assembly not exposed to AgentsWatch;
- local models have compute/time/energy cost rather than provider token price;
- retry semantics differ between model calls, tool retries, network retries, test reruns, and human-requested reruns;
- cache hits, prompt caching, and auto-compaction must not be conflated with new work;
- repeated validation is legitimate after a relevant state change.

Live stop requirements:

- AgentsWatch owns/wraps the process, or the host exposes a supported stop API;
- a checkpoint can be created;
- subprocess cleanup is defined;
- user opt-in exists;
- the environment is not a remote opaque cloud task.

Fallback for opaque cloud sessions: `PostHoc loop/waste report` and provider-native cancellation guidance only.

### 6. Policy Firewall

This is the most environment-dependent feature.

Enforcement classes:

```text
PE0 Documentation/advice only
PE1 Preflight classification
PE2 Approval recommendation
PE3 Supported tool-hook blocking
PE4 Process-wrapper enforcement
PE5 OS/container/network enforcement integration
```

Rules:

- report the effective enforcement class on every decision;
- do not call PE3 a complete boundary when equivalent action paths bypass the hook;
- vendor sandbox and AgentsWatch policy are separate layers;
- remote/cloud agents may permit only configuration review and post-hoc auditing;
- read-only modes still need protection against sensitive reads and network exfiltration;
- broad local/YOLO/full-access modes increase risk but do not automatically mean every action is observable;
- production credentials elevate the policy profile regardless of model quality;
- simulator, Docker socket, Kubernetes, Terraform, browser, MCP, and package-manager access need dedicated operation categories.

### 7. Multi-Agent Worktree Coordinator

There are at least three distinct coordination models:

```text
LocalWorktree — one local git worktree per worker
CloudBranchPR — one remote branch/task/PR per worker
SharedWorkspace — multiple workers share one workspace with explicit path ownership
```

Adaptations:

- tools with native worktrees: import native worktree identity and lifecycle;
- subagents inheriting parent cwd: verify effective cwd before write work;
- cloud agents: use branch/PR isolation, not local path assumptions;
- non-git environments: only task/message coordination is available;
- shallow clones/partial checkouts: dependency and conflict forecasts have lower confidence;
- multi-root workspaces: ownership is per root and path namespace;
- ignored local setup files may not follow worktree/branch handoff;
- GUI applications with singleton emulators/servers need resource locks;
- shared databases and external services require lease/ownership records beyond file ownership.

### 8. AI PR Review Debt Reducer

This is highly portable when a commit/PR exists.

Adaptations:

- local uncommitted changes: analyze a synthetic range or working tree and label it unbound to commit;
- cloud agent: use session, PR, checks, and environment setup evidence;
- forks: distinguish contributor permissions and inaccessible CI/secrets;
- monorepos: apply ownership, CODEOWNERS, affected-project, and targeted validation rules;
- generated/binary changes: require generator/provenance evidence rather than text review;
- no CI permission: report checks as unavailable, not failed;
- stacked PRs: compare to the actual base branch/merge base;
- deployment/infrastructure PRs: require plan output and approval evidence.

### 9. Agent Regression Canary

Canaries are not valid when unrelated dimensions change silently.

Comparability key:

```text
Model and version
Tool and version
Surface
Permission profile
Environment image/toolchain
Repository commit
Prompt/rules snapshot
Network mode
Reasoning effort
Role composition
Randomness/repetition count
```

Allowed comparisons:

- same tool/environment, different model;
- same model/environment, different tool version;
- same model/tool, different permission profile;
- same complete profile over time.

Cross-tool results may be exploratory but should not be described as pure model comparisons.

### 10. Workspace Doctor and OSS Gatekeeper

Workspace Doctor requires local or exported environment state. Cloud agents support only environment-manifest review. OSS Gatekeeper is strongest in GitHub/CI flows but must account for forks, unavailable secrets, untrusted workflows, and maintainer-specific disclosure policies.

## Permission-aware behavior rules

### Read-only

Allowed AgentsWatch behavior:

- context/rules lint;
- repository and policy analysis;
- advisory validation plan;
- claim review from existing evidence;
- canary tasks that do not write.

Disallowed assumptions:

- inability to write means inability to execute;
- inability to edit means sensitive data cannot be read;
- missing tests mean tests failed.

### Workspace-write without shell

- diff/scope evidence is available;
- command/test proof is not available unless external CI runs;
- Trust Ledger caps validation claims;
- Policy focuses on path boundaries;
- Loop Guard watches edit cycles, not command cycles.

### Shell without network

- local build/tests may run;
- dependency installation/update may fail;
- environment readiness must distinguish missing dependency from code failure;
- package-lock changes need special policy.

### Network without secrets

- public dependencies and web access may work;
- private registries, services, and protected integration tests may not;
- failure must be classified as credential/environment blocked.

### Secrets/authenticated services

- record only secret class/name reference, never value;
- apply minimum-scope and purpose binding;
- require explicit policy for external side effects;
- mask logs and environment diffs;
- differentiate test credentials from production credentials.

### Privileged/production access

Default AgentsWatch mode:

```text
Advisory or approval-gated only
No autonomous deployment
No automatic destructive action
Mandatory checkpoint/plan/evidence
Human approval tied to exact action and target
```

## Environment-specific requirements

### Windows native versus WSL2

- normalize Windows/WSL path identities without conflating them;
- record shell and filesystem boundary;
- test symlink/junction/path-case behavior separately;
- sandbox support and command syntax may differ;
- one run should not silently switch environments.

### Container/dev container

- distinguish host path from container path;
- record image/devcontainer/setup hash;
- account for bind mounts, Docker socket, nested sandbox weakening, and ephemeral state;
- command evidence belongs to the container environment, not automatically the host.

### Remote SSH/Codespace

- event collector location must be explicit;
- local IDE events and remote command events may have different clocks/paths;
- network and credentials belong to the remote environment;
- local process wrapper cannot control an opaque remote process unless installed there.

### Cloud ephemeral agent

- use task/environment/branch/commit identity;
- import provider logs and checks;
- record setup phase separately from agent phase;
- record cache provenance and whether state survives;
- local live-stop/enforcement features are unavailable unless the provider exposes an API.

### CI runner

- headless and non-interactive;
- no approval dialog unless represented as a gate;
- permissions come from workflow/job/token configuration;
- fork PRs and protected secrets require distinct scenarios;
- every result must bind to workflow, job, attempt, commit, and artifact.

### Worktrees

- repository identity is shared; workspace identity is not;
- ownership and base commit are per worktree;
- ignored files and local services may be absent;
- branch checkout constraints and cleanup rules must be respected;
- one worker's evidence must not be attributed to another worktree.

### Multi-root/monorepo

- rules, adapters, ownership, validation, and risk are root/package scoped;
- nearest/nested instruction precedence must be retained;
- one global token or file budget may be misleading;
- affected-project calculation should precede broad validation.

### Air-gapped/corporate-restricted

- no network assumptions;
- no cloud licensing requirement for core local function;
- local model/provider and offline docs may be used;
- dependency/cache readiness becomes part of environment evidence;
- telemetry-disabled state must remain supported.

### Simulator/emulator/browser-dependent

- build success is not UI behavior proof;
- record simulator/browser/device version and availability;
- resource locks may serialize otherwise parallel agents;
- screenshots/manual checks are separate evidence types;
- sandbox exceptions for sockets/local ports/UI automation require explicit policy.

## Required architecture changes

AgentsWatch needs a runtime compatibility layer before advanced features:

```text
Detect declared configuration
+ observe effective environment
+ query adapter capabilities
+ record permissions and blind spots
= EffectiveRuntimeProfile

EffectiveRuntimeProfile
+ requested AgentsWatch capability
= SupportMode + ConfidenceCap + FallbackPlan
```

New core concepts:

```text
ModelProfile
HostToolProfile
SurfaceProfile
ObservationProfile
PermissionProfile
ExecutionEnvironmentProfile
RepositoryDeliveryProfile
AdapterCapabilityDeclaration
EffectiveRuntimeProfile
FeatureSupportDecision
FallbackPlan
```

## Required product behavior

Every advanced command should print or persist:

```text
Detected tool/surface/model
Environment and repository identity
Effective permissions
Observation level
Selected support mode
Known blind spots
Fallback used
Confidence cap
Unsafe or unavailable operations
```

Example:

```text
Capability: Policy Firewall
Tool: Codex CLI
Surface: local interactive
Environment: WSL2 sandbox
Observation: O5
Effective permission: workspace write, restricted network
Mode: Guarded
Blind spot: current PreToolUse coverage does not intercept every equivalent action path
Fallback: git/post-tool verification for uncovered changes
```

## Compatibility principles

1. **Never promise equal support across tools.** Promise explicit support levels.
2. **Never infer runtime capability from model name.** Detect the host/session.
3. **Never infer safety from read-only UI labels.** Inspect effective filesystem, shell, network, MCP, and secret access.
4. **Never infer absence from missing events.** Record `not observed`.
5. **Never compare canaries without a profile key.** Environment changes invalidate pure model conclusions.
6. **Never call advisory policy enforcement.** Display enforcement class.
7. **Never force cloud semantics into local-worktree concepts.** Use branch/PR coordination variants.
8. **Never treat provider checkpoints as equivalent to git commits.** Preserve provenance.
9. **Never treat setup failure as implementation failure without classification.** Environment readiness is separate.
10. **Always provide a safe fallback.** Post-hoc or manual support is better than a false Full claim.

## Recommended implementation order

```text
1. Runtime profile schema
2. Static adapter capability declarations
3. Environment/repository detector
4. Permission/effective-mode detector
5. Feature support decision and fallback planner
6. Compatibility report command
7. Fixture matrix for local/IDE/cloud/CI/chat/read-only/no-git
8. Only then implement provider-specific live adapters
```

## Research limitations

- Tool capabilities and configuration contracts can change rapidly.
- Enterprise-managed configurations may differ from public defaults.
- Some providers expose logs without a stable schema.
- Documentation describes intended behavior, not every implementation bug.
- The matrix identifies required product behavior; it does not prove current AgentsWatch support.
