# AgentsWatch Community and Market Opportunity Validation Queue

Last aligned: 2026-07-05  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: product discovery, market validation, compatibility, prototypes, and evidence for AW-CAP-028 through AW-CAP-037

## Purpose

Convert repeated coding-agent problems into validated, bounded product opportunities without:

- confusing online discussion with product-market fit;
- assuming equal tool support;
- building infrastructure before repeat use/payment;
- starting live integrations too early;
- drifting into a generic AI code reviewer.

## Current product decision

The first market-facing hypothesis is:

```text
PR Evidence Packet + Trust Ledger
```

Public outcome hypothesis:

```text
Know what your coding agent changed, executed, tested, and missed.
```

The broader control-plane architecture remains valid, but market validation begins with completion integrity and review evidence.

## Read first

- `../MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`
- `../PR_EVIDENCE_MARKET_VALIDATION_RUNBOOK.md`
- `../AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`
- `../POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`
- `../PRODUCT_SPEC.md`
- `../MVP_ROADMAP.md`
- `../MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`
- `../RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md`
- `../COMPATIBILITY_ACCEPTANCE_SCENARIOS.md`
- `../ADAPTER_SPEC.md`
- `../COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`
- `../COMMUNITY_OPPORTUNITY_ARCHITECTURE_ADDENDUM.md`
- `../COMMUNITY_OPPORTUNITY_BACKLOG.md`
- `../COMPATIBILITY_IMPLEMENTATION_BACKLOG.md`
- `../FEATURE_CAPABILITY_REGISTRY.md`
- `../PROOF_AND_VERIFICATION_STRATEGY.md`
- `PROMPT_QUEUE_ROUTER.md`

## Hard rules

- Social/forum/issue frequency is a problem signal, not market size or willingness to pay.
- Adjacent paid products prove a neighboring budget category, not demand for AgentsWatch.
- Keep AW-CAP-028 through AW-CAP-037 at L1 until runtime code and required proof exist.
- Never infer support from model/provider name alone.
- Real examples and manual-assisted validation precede broad implementation.
- Runtime profile and fallback decisions precede provider-specific live features.
- Manual, import-only, and dry-run prototypes precede live observation, enforcement, process control, agent spawning, or GitHub posting.
- Do not require source upload or hidden telemetry.
- Unknown provider events, prices, permissions, or capabilities remain unknown.
- Every advancing idea needs success metrics, kill criteria, compatibility modes, and a proof plan.
- Do not build Team Server, broad GitHub App, dashboard, or daemon before validated demand makes them necessary.
- Do not position AgentsWatch as a generic model-based bug reviewer.

## Market validation prompts

| ID | Status/Gate | Capability | Purpose |
|---|---|---|---|
| OPP-MKT-001 | Ready | AW-CAP-035/AW-CAP-029 | Recruit first 5 reviewers/maintainers and collect real AI-assisted PR cases. |
| OPP-MKT-002 | OPP-MKT-001 | AW-CAP-035/AW-CAP-029 | Manually generate PR Evidence Packets using the runbook. |
| OPP-MKT-003 | After 5 cases | AW-CAP-035/AW-CAP-029 | Review false positives, packet length, missing inputs, and reviewer decision impact. |
| OPP-MKT-004 | After 10 cases | AW-CAP-035/AW-CAP-029 | Decide whether author preflight or reviewer packet is the stronger primary surface. |
| OPP-MKT-005 | After 20 cases | AW-CAP-035/AW-CAP-029 | Test automation demand and paid/budgeted pilot wording. |
| OPP-MKT-006 | After 30 cases | AW-CAP-035/AW-CAP-029 | Produce Advance/Revise/Park/Reject report and exact next implementation slice. |

Required runbook: `../PR_EVIDENCE_MARKET_VALIDATION_RUNBOOK.md`.

Directional Gate M criteria:

```text
>= 30% of packets change a real review action
>= 50% of participants request or perform repeat use
>= 3 teams request CI/GitHub automation
>= 2 teams accept a paid or explicitly budgeted pilot
```

These are internal thresholds, not external benchmarks.

## General discovery prompts

| ID | Status | Prompt | Purpose |
|---|---|---|---|
| OPP-RSCH-001 | Ready | `../prompts/OPP-001-user-interview-synthesis.md` | Validate pain, workflow, workarounds, buyer, and adoption intent. |
| OPP-RSCH-002 | Ready | `../prompts/OPP-002-adapter-feasibility.md` | Map stable local logs/hooks/exports and blind spots. |
| OPP-RSCH-003 | Ready | `../prompts/OPP-003-competitive-substitutes.md` | Identify substitutes, differentiation, and kill conditions. |
| OPP-RSCH-004 | Ready | `../prompts/OPP-004-runtime-compatibility-audit.md` | Audit one tool/surface/permission/environment combination and assign honest support modes. |
| OPP-RSCH-005 | Ready after Gate M/interviews | — | Re-score opportunity map using AgentsWatch-specific evidence rather than desk research. |
| OPP-RSCH-006 | Ready after Gate M/compatibility | — | Select the smallest automation slice and one secondary free wedge. |

## Core evidence/PR prototype prompts

Every prototype must preserve commit identity and unknown/not-observed states.

| ID | Gate | Capability | Purpose |
|---|---|---|---|
| OPP-PR-001 | Core git/run evidence | AW-CAP-035 | Define deterministic PR Evidence Packet schema and Markdown/JSON outputs. |
| OPP-PR-002 | OPP-PR-001 | AW-CAP-029/AW-CAP-019 | Define claim extraction and exact status vocabulary. |
| OPP-PR-003 | Validation runner/evidence | AW-CAP-029/AW-CAP-035 | Bind command/test/CI evidence to commit and classify stale evidence. |
| OPP-PR-004 | OPP-PR-001/002/003 | AW-CAP-035 | Implement local range-based PR Evidence command. |
| OPP-PR-005 | OPP-PR-004 + Gate M automation demand | AW-CAP-035 | GitHub Action artifact/job summary, no automatic comments initially. |
| OPP-PR-006 | Action dogfood + explicit approval | AW-CAP-035 | Optional check/comment publishing. |

## Compatibility foundation prompts

| ID | Gate | Capability | Purpose |
|---|---|---|---|
| OPP-COMPAT-001 | Main Gate 0 | AW-CAP-037 | Implement versioned runtime profile schema. |
| OPP-COMPAT-002 | OPP-COMPAT-001 | AW-CAP-037 | Implement support decision and fallback engine. |
| OPP-COMPAT-003 | OPP-COMPAT-001/002 | AW-CAP-037 | Generic/manual adapter and chat-only fallback. |
| OPP-COMPAT-004 | Core evidence/command spine | AW-CAP-037 | Environment, permission, git/no-git, and process-ownership detectors. |
| OPP-COMPAT-005 | OPP-COMPAT-001/002/004 | AW-CAP-037 | Compatibility CLI and Linux/Windows black-box scenarios. |
| OPP-COMPAT-006 | Fixture matrix green | AW-CAP-037 | First rich local event adapter. |
| OPP-COMPAT-007 | First adapter stable | AW-CAP-037 | Second materially different local adapter. |
| OPP-COMPAT-008 | Core PR evidence | AW-CAP-037 | Cloud/PR evidence adapter. |

Detailed issue slices live in `../COMPATIBILITY_IMPLEMENTATION_BACKLOG.md`.

## Event foundation prompts

| ID | Gate | Purpose |
|---|---|---|
| OPP-EVENT-001 | Main Gate 0 + OPP-RSCH-002 + runtime schema | Define normalized event schema and two synthetic adapters. |
| OPP-EVENT-002 | OPP-EVENT-001 | Implement idempotent import-only local event journal. |
| OPP-EVENT-003 | OPP-EVENT-001 | Add adapter capability declaration, handshake, and blind-spot reporting. |
| OPP-PRIV-001 | Before real logs | Add redaction, path-boundary, corrupt-input, and no-network fixtures. |

## Secondary prototype prompts

Every runtime-specific prototype consumes an `EffectiveRuntimeProfile` or uses the generic/manual profile explicitly.

| ID | Gate | Capability | Purpose |
|---|---|---|---|
| OPP-PROT-001 | Core PR evidence manual format | AW-CAP-035/AW-CAP-029 | Manual/local PR Evidence Packet and Trust Ledger. |
| OPP-PROT-002 | Run/handoff model + target profile | AW-CAP-030 | Manual context snapshot and fresh-session resume pack. |
| OPP-PROT-003 | Core config/file safety + rule target profile | AW-CAP-031 | Rules Compiler, target-loss report, and Drift Detector. |
| OPP-PROT-004 | Event/usage profile | AW-CAP-032 | Offline loop/waste analyzer; no exact billing or process stopping. |
| OPP-PROT-005 | Event foundation + support decision | AW-CAP-028 | Import-only Flight Recorder timeline. |
| OPP-PROT-006 | Permission/environment profile + threat model | AW-CAP-033 | Policy dry-run and effective-permission explain mode. |
| OPP-PROT-007 | VCS/coordination profile | AW-CAP-034 | Workspace/ownership/stale-state planner; no agent spawning. |
| OPP-PROT-008 | Complete comparison profile | AW-CAP-036 | Small regression canary suite. |

## Dogfood and advancement prompts

| ID | Gate | Purpose |
|---|---|---|
| OPP-DOG-001 | PR packet/Trust Ledger | Compare reviewer baseline and packet-assisted decisions on real PRs. |
| OPP-DOG-002 | Rules prototype | Test onboarding a second tool and rule-loss/drift detection. |
| OPP-DOG-003 | Context prototype | Paired fresh-session resume comparison across at least two target surfaces. |
| OPP-DOG-004 | Timeline/trust prototype | Verify supported, contradicted, missing, stale, skipped, and not-observed claims. |
| OPP-DOG-005 | Loop prototype | Measure detections and false positives with exact/partial/no usage telemetry. |
| OPP-DOG-006 | Policy dry-run | Evaluate enforcement class, bypass disclosure, and approval burden. |
| OPP-DOG-007 | Coordination diagnostics | Run local-worktree, cloud-branch, shared-workspace, stale-state, and duplicate-work scenarios. |
| OPP-DOG-008 | Compatibility engine | Verify correct Full/Guarded/Advisory/PostHoc/Manual/Unavailable modes across required surfaces. |

## Live/enforcement prompts — blocked initially

| ID | Blocked until | Purpose |
|---|---|---|
| OPP-LIVE-001 | useful low-noise offline loop dogfood + process ownership/checkpoint proof | Live loop warning/optional stop wrapper. |
| OPP-LIVE-002 | compatibility engine + threat model + independent security review | Optional policy execution broker. |
| OPP-LIVE-003 | coordination diagnostic demand + stable adapter + exact workspace identity | Controlled worker launching. |
| OPP-LIVE-004 | local/Action PR Evidence dogfood + explicit approval flow | Draft/post GitHub review output. |
| OPP-LIVE-005 | two stable event adapters + privacy proof + handshake/downgrade | Live provider event ingestion. |

## Infrastructure blockers

Do not begin these merely because they are on the roadmap:

| Infrastructure/component | Required evidence first |
|---|---|
| Standalone/package-manager expansion | repeated CLI use and release maintenance capacity |
| GitHub Action | at least 3 teams request automation |
| Local dashboard | users struggle with accumulated local history and request visual analysis |
| Background service/daemon | live events/warnings cannot be delivered usefully on demand |
| GitHub App | Action/manual workflow has paying users and documented limitations |
| Team Server/SaaS | at least 2 paid/budgeted pilots and a shared-metadata requirement |
| Self-hosted enterprise | signed customer requirement and support/security budget |

## Advancement scorecard

An opportunity advances only when:

```text
Problem recognized by target users
Real examples available
Prototype input available locally or through documented export
Effective runtime profile can be established where needed
Support mode and fallback are honest
Output changes a real decision
Repeat use exists
False-positive burden acceptable
Privacy boundary credible
Maintenance cost understood
Differentiation still exists
Payment/budget signal exists before team infrastructure
Proof plan executable
```

Use `Advance`, `Revise`, `Park`, or `Reject`; do not leave every idea indefinitely planned.

## Current recommended sequence

```text
finish core run/git/validation evidence spine
-> define manual PR Evidence Packet and Trust Ledger
-> execute OPP-MKT-001 through OPP-MKT-006 on 30 real PRs
-> implement smallest local automation justified by repeated findings
-> Context Snapshot/Resume
-> Rules Compiler/target-loss report
-> AW-CAP-037 runtime profile/decision foundation
-> offline Loop/Waste Analyzer
-> import-only event timeline
-> GitHub Action only after automation demand
-> live behavior only after compatibility, privacy, safety, and user-value proof
```
