# AW-RUNTIME-COMPATIBILITY-001 Evidence

Prompt ID: AW-RUNTIME-COMPATIBILITY-001  
Queue: `community_opportunity_validation.md`  
Agent/tool: ChatGPT with web research and GitHub connector  
Model: GPT-5.5 Thinking  
Run mode: research + architecture/product contract + documentation  
Date: 2026-07-03

## Goal

Determine whether all planned AgentsWatch capabilities apply equally across models and coding tools, including different permissions, local/remote/cloud environments, IDE/CLI/CI/chat surfaces, VCS workflows, and validation constraints; update the development system accordingly.

## Executive result

No. Concepts are broadly reusable, but operational support is profile-dependent.

The effective runtime must be derived from:

```text
model role and identity
+ host tool
+ interaction surface
+ observable events
+ effective permissions
+ execution environment
+ repository/delivery workflow
```

Every advanced capability must select:

```text
Full | Guarded | Advisory | PostHoc | Manual | Unavailable
```

## Primary official sources reviewed

### Claude Code

- settings, permissions, managed policy, sandbox, and hooks;
- pre/post tool and permission-request lifecycle;
- session/compaction/subagent/worktree/current-directory/file-change events;
- platform and sandbox exceptions/limitations.

### OpenAI Codex

- local permission profiles and sandbox behavior;
- CLI/IDE/cloud/worktree/headless surfaces;
- hook lifecycle and documented incomplete interception boundary;
- cloud setup/maintenance/cache and agent-phase network behavior;
- branch/worktree handoff and ignored-file limitations.

### GitHub Copilot coding agent

- ephemeral GitHub Actions-based environment;
- repository setup workflow and runner constraints;
- branch/PR/check delivery rather than local process ownership;
- fork/private dependency/credential limitations.

### Gemini CLI

- layered configuration precedence;
- default/auto-edit/plan/YOLO approval modes;
- checkpointing;
- plan/implementation model routing.

### Cline

- separate read/edit/command/browser/MCP permissions;
- model-produced command safety classification;
- broad YOLO mode;
- shadow-git checkpoints and storage/performance implications.

### Aider

- ask/code/architect modes;
- planner/editor model separation;
- additional request/cost and role attribution needs.

## Main findings

1. **Model identity does not determine permissions or visibility.** The same model may run chat-only, locally, in an IDE, cloud, CI, or through an orchestrator.
2. **Tool identity is still insufficient.** One product may expose materially different local, IDE, cloud, and headless surfaces.
3. **Configured permission is not effective permission.** Managed policies, mounts, sandbox behavior, platform support, network rules, and credential scope may restrict it.
4. **Observation and enforcement are separate.** Post-tool events can record outcomes without blocking actions; pre-tool hooks may still have uncovered equivalent paths.
5. **Cloud/PR and local-worktree coordination are different products modes.** They require CloudBranchPR versus LocalWorktree profiles.
6. **Trust Ledger is the most portable feature.** It can use independent git/CI evidence even when agent events are unavailable.
7. **Live Flight Recorder, Loop Stop, and Policy enforcement are the least portable.** They require hooks, wrappers, process ownership, or environment controls.
8. **Usage metrics are not normalized facts.** Exact tokens, cached tokens, quota percentages, currency cost, and local compute proxies must remain distinct.
9. **Regression Canary requires the whole profile.** A model comparison is invalid or confounded when tool, permissions, environment, network, prompt/rules, or role composition changed.
10. **Missing evidence must remain missing.** It cannot be converted into success, failure, or absence of behavior.

## Documentation added

- `docs/MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`;
- `docs/RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`;
- `docs/COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`;
- `docs/COMPATIBILITY_IMPLEMENTATION_BACKLOG.md`;
- `docs/COMPATIBILITY_INDEX.md`;
- `docs/prompts/OPP-004-runtime-compatibility-audit.md`;
- `.ai/discoveries/AW-DISC-COMPAT-001.md`.

## Documentation updated

- `docs/ADAPTER_SPEC.md` — expanded from stack adapters into ten composable adapter families;
- `docs/FEATURE_CAPABILITY_REGISTRY.md` — added AW-CAP-037 at L1;
- `docs/FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md` — mapped compatibility scenarios and proof-invalidating conditions;
- `docs/MVP_ROADMAP.md` — added runtime compatibility gate before event/live integrations;
- `docs/prompt_queues/community_opportunity_validation.md` — prototypes now require runtime profiles/fallbacks;
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md` — routes advanced work through compatibility negotiation;
- `docs/DOCS_INDEX.md` — indexed compatibility contracts.

## New capability

```text
AW-CAP-037 Runtime capability negotiation and fallback planning
Surface: compatibility detect/explain/compare/export
Maturity: L1 Specified
```

No runtime detector, adapter handshake, permission resolver, compatibility CLI, or enforcement behavior was implemented in this run.

## Adapter architecture change

Adapter families now include:

1. runtime tool;
2. surface;
3. model metadata;
4. event/telemetry;
5. permission;
6. environment;
7. repository/VCS/CI;
8. technology stack;
9. rule format;
10. usage/cost.

They compose into `EffectiveRuntimeProfile`. Stack adapters no longer imply security, runtime, or event capabilities.

## Acceptance coverage

Fifty scenarios were specified, including:

- chat-only/manual;
- rich hooks and no-hook wrappers;
- local IDE;
- cloud agent and CI/fork PR;
- read-only, write/no-shell, shell/no-network, network/no-credentials;
- broad/YOLO and managed-policy conflict;
- native Windows/WSL, containers, nested sandbox, remote SSH, cloud cache;
- local worktree, cloud branch/PR, shared workspace, monorepo, no-git, dirty checkout;
- generated/binary repositories;
- planner/editor and auto-routed models;
- hook failure, incomplete hook coverage, and mid-run downgrade;
- lossy rule export and context compaction;
- UI/simulator/resource constraints;
- offline/proxy/credential/production cases;
- usage telemetry gaps and justified validation reruns.

These are contracts, not executed tests.

## Product and roadmap decisions

- AW-CAP-037 becomes a prerequisite for advanced live integrations.
- Low-risk Rules Compiler and manual Context Snapshot may begin with a generic/manual profile.
- Flight Recorder starts import-only.
- Policy starts dry-run with an explicit enforcement class.
- Multi-agent coordination selects local-worktree, cloud-branch/PR, shared-workspace, message-only, or unavailable mode.
- Live stop requires process ownership or supported stop API, plus a checkpoint and opt-in.
- Canaries store the whole comparison profile and mark confounders.

## Validation performed

- checked official contracts across materially different tool types;
- separated model, tool, surface, permission, environment, and VCS dimensions;
- added negative/downgrade scenarios;
- registered only L1 capability maturity;
- updated proof rules so advanced capabilities cannot mature without compatibility decisions;
- preserved local-first/no-hidden-network behavior;
- recorded one P1 architecture discovery.

## Validation not performed

- no code was changed;
- no build/test/CI was needed for the documentation-only scope;
- no real provider hook handshake was executed;
- no enterprise-managed environment was tested;
- no cross-surface dogfood was performed;
- PR #5 remains stacked on PR #4, so no separate Actions run is expected until retargeted to `main`.

## Residual risks

- provider contracts change rapidly;
- documented behavior may differ from implementation bugs;
- enterprise products may expose additional managed restrictions;
- stable log/export schemas still need adapter feasibility verification;
- 50 scenarios require executable fixtures or documented manual verification;
- compatibility detection itself can be wrong and therefore needs confidence/provenance and downgrade behavior.

## Next recommended work

1. Retarget PR #5 after PR #4 merges and obtain green CI.
2. Run OPP-004 audits for candidate local, IDE, cloud/PR, and chat/manual tools.
3. Implement COMPAT-001 Runtime Profile schema after the core evidence spine is ready.
4. Implement COMPAT-002 support decision/fallback engine.
5. Add generic/manual, environment, permission, and git/no-git adapters.
6. Add two materially different local adapters and one cloud/PR adapter.
7. Execute cross-surface dogfood before any live/enforcing claim.
