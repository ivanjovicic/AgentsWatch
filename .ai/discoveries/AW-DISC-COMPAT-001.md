# AW-DISC-COMPAT-001 — Planned capabilities require runtime negotiation, not provider-name assumptions

Discovery ID: AW-DISC-COMPAT-001  
Status: Planned  
Category: ArchitectureRisk  
Severity: P1  
Confidence: Confirmed  
Found in run: AW-RUNTIME-COMPATIBILITY-001  
Found while doing: cross-model, cross-tool, permission, and environment research  
Created: 2026-07-03  
Last reviewed: 2026-07-03

## Finding

The planned AgentsWatch concepts are broadly reusable, but their observation, verification, and enforcement depth is not equal across models, tools, surfaces, permissions, environments, and VCS/delivery workflows.

A model/provider name is insufficient because the same model may run in:

- chat-only mode;
- a local CLI;
- an IDE;
- a headless wrapper;
- a cloud ephemeral environment;
- CI/PR automation;
- a multi-model planner/editor workflow.

The effective behavior depends on:

```text
ModelProfile
+ HostToolProfile
+ SurfaceProfile
+ ObservationProfile
+ PermissionProfile
+ ExecutionEnvironmentProfile
+ RepositoryDeliveryProfile
= EffectiveRuntimeProfile
```

## Risks if not fixed

- Flight Recorder presented as observing actions it cannot see;
- advisory policy described as enforcement;
- missing events interpreted as absence of behavior;
- cloud/PR agents modeled as local worktrees;
- usage percentages converted into false token/cost estimates;
- canary differences attributed to a model when tool/environment changed;
- live stop attempted without process ownership or checkpoint;
- managed policy, remote paths, no-network, missing credentials, or read-only mounts ignored;
- provider updates silently remove a capability while AgentsWatch retains a Full claim.

## Required system response

Introduce AW-CAP-037 Runtime capability negotiation and fallback planning.

Every advanced feature must select exactly one support mode:

```text
Full
Guarded
Advisory
PostHoc
Manual
Unavailable
```

Every non-Full result needs a reason and fallback where one exists. Full/Guarded results require capability provenance, blind spots, and a confidence cap.

## Affected capabilities

- AW-CAP-028 Flight Recorder;
- AW-CAP-029 Trust Ledger;
- AW-CAP-030 Context/Session Rescue;
- AW-CAP-031 Rules Compiler;
- AW-CAP-032 Cost/Loop Guard;
- AW-CAP-033 Policy Firewall;
- AW-CAP-034 Multi-Agent Coordinator;
- AW-CAP-035 PR Review Debt Reducer;
- AW-CAP-036 Regression Canary;
- AW-CAP-037 Runtime Compatibility.

## Canonical documentation

- `docs/COMPATIBILITY_INDEX.md`;
- `docs/MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`;
- `docs/RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`;
- `docs/COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`;
- `docs/ADAPTER_SPEC.md`;
- `docs/COMPATIBILITY_IMPLEMENTATION_BACKLOG.md`.

## Reconciliation

Duplicate of: none  
Primary owner: AW-CAP-037 / `COMPATIBILITY_IMPLEMENTATION_BACKLOG.md`  
Queue target: `docs/prompt_queues/community_opportunity_validation.md`  
Prompt target: `docs/prompts/OPP-004-runtime-compatibility-audit.md`  
Dependencies: main Gate 0, core evidence spine, non-destructive environment probes  
Recommended validation: 50 compatibility scenarios, Linux/Windows black-box tests, two local adapters, one cloud/PR flow, one manual/chat flow

## Disposition

Action: make runtime negotiation a prerequisite for advanced integrations  
Reason: confirmed architectural requirement across current tool contracts  
Resolved by: pending implementation and proof

## Links

Run log: `.ai/runs/2026-07-03-AW-RUNTIME-COMPATIBILITY-001-evidence.md`
