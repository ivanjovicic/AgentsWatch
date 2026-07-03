# AgentsWatch Community Opportunity Validation Queue

Last aligned: 2026-07-03  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: product discovery, compatibility, prototypes, and evidence for AW-CAP-028 through AW-CAP-037

## Purpose

Convert repeated community problems into validated, bounded product opportunities without confusing online discussion with market proof, assuming equal tool support, or starting live integrations too early.

## Read first

- `../AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`
- `../POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`
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

- Social/forum/issue frequency is a problem signal, not market size.
- Keep AW-CAP-028 through AW-CAP-037 at L1 until runtime code exists.
- Never infer support from model/provider name alone.
- Interviews and real examples precede broad implementation.
- Runtime profile and fallback decisions precede provider-specific live features.
- Import-only and dry-run prototypes precede live observation, enforcement, process control, agent spawning, or GitHub posting.
- Do not require source upload or hidden telemetry.
- Unknown provider events, prices, permissions, or capabilities remain unknown.
- Every advancing idea needs success metrics, kill criteria, compatibility modes, and a proof plan.

## Discovery prompts

| ID | Status | Prompt | Purpose |
|---|---|---|---|
| OPP-RSCH-001 | Ready | `../prompts/OPP-001-user-interview-synthesis.md` | Validate pain, workflow, workarounds, and adoption intent. |
| OPP-RSCH-002 | Ready | `../prompts/OPP-002-adapter-feasibility.md` | Map stable local logs/hooks/exports and blind spots. |
| OPP-RSCH-003 | Ready | `../prompts/OPP-003-competitive-substitutes.md` | Identify substitutes, differentiation, and kill conditions. |
| OPP-RSCH-004 | Ready | `../prompts/OPP-004-runtime-compatibility-audit.md` | Audit one tool/surface/permission/environment combination and assign honest support modes. |
| OPP-RSCH-005 | Ready after interviews | — | Re-score opportunity map using evidence rather than desk research. |
| OPP-RSCH-006 | Ready after interviews/compatibility | — | Select one free wedge and one Pro prototype with feasible inputs and fallbacks. |

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

## Prototype prompts

Every prototype must consume an `EffectiveRuntimeProfile` or use the generic/manual profile explicitly.

| ID | Gate | Capability | Purpose |
|---|---|---|---|
| OPP-PROT-001 | Core config/file safety + rule target profile | AW-CAP-031 | Rules Compiler and Drift Detector prototype. |
| OPP-PROT-002 | Run/handoff model + target profile | AW-CAP-030 | Manual context snapshot and fresh-session resume pack. |
| OPP-PROT-003 | Event foundation + support decision | AW-CAP-028 | Import-only Flight Recorder timeline. |
| OPP-PROT-004 | Evidence grades + support decision | AW-CAP-029 | Deterministic claim-to-evidence verifier. |
| OPP-PROT-005 | Event/usage profile | AW-CAP-032 | Offline loop and budget analyzer. |
| OPP-PROT-006 | Permission/environment profile + threat model | AW-CAP-033 | Policy dry-run and explain mode. |
| OPP-PROT-007 | VCS/coordination profile | AW-CAP-034 | Worktree/branch/shared-workspace planner; no agent spawning. |
| OPP-PROT-008 | Git/PR/evidence profile | AW-CAP-035 | Local or cloud PR review-evidence packet. |
| OPP-PROT-009 | Complete comparison profile | AW-CAP-036 | Small regression canary suite. |

## Dogfood and advancement prompts

| ID | Gate | Purpose |
|---|---|---|
| OPP-DOG-001 | Rules prototype | Test onboarding a second tool and rule-loss/drift detection. |
| OPP-DOG-002 | Context prototype | Paired fresh-session resume comparison across at least two target surfaces. |
| OPP-DOG-003 | Timeline/trust prototype | Verify supported, contradicted, missing, and not-observed claims. |
| OPP-DOG-004 | Loop prototype | Measure detections and false positives with exact/partial/no usage telemetry. |
| OPP-DOG-005 | Policy dry-run | Evaluate enforcement class, bypass disclosure, and approval burden. |
| OPP-DOG-006 | Coordination planner | Run local-worktree, cloud-branch, and shared-workspace scenarios. |
| OPP-DOG-007 | PR packet | Compare local, fork, stacked, and cloud-agent review cases. |
| OPP-DOG-008 | Compatibility engine | Verify correct Full/Guarded/Advisory/PostHoc/Manual/Unavailable modes across required surfaces. |

## Live/enforcement prompts — blocked initially

| ID | Blocked until | Purpose |
|---|---|---|
| OPP-LIVE-001 | useful low-noise offline loop dogfood + process ownership/checkpoint proof | Live loop warning/optional stop wrapper. |
| OPP-LIVE-002 | compatibility engine + threat model + independent security review | Optional policy execution broker. |
| OPP-LIVE-003 | coordination-mode dogfood + stable adapter + exact workspace identity | Controlled worker launching. |
| OPP-LIVE-004 | local/cloud review packet dogfood + explicit approval flow | Draft/post GitHub review output. |
| OPP-LIVE-005 | two stable event adapters + privacy proof + handshake/downgrade | Live provider event ingestion. |

## Advancement scorecard

An opportunity advances only when:

```text
Problem recognized by target users
Real examples available
Prototype input available locally or through documented export
Effective runtime profile can be established
Support mode and fallback are honest
Output changes a real decision
False-positive burden acceptable
Privacy boundary credible
Maintenance cost understood
Differentiation still exists
Proof plan executable
```

Use `Advance`, `Revise`, `Park`, or `Reject`; do not leave every idea indefinitely planned.

## Current recommended sequence

```text
OPP-RSCH-001
-> OPP-RSCH-002
-> OPP-RSCH-003
-> OPP-RSCH-004 for candidate tools/surfaces
-> choose one free and one Pro wedge
-> AW-CAP-037 runtime profile/decision foundation
-> event foundation
-> import-only/dry-run prototypes
-> cross-surface dogfood
-> live behavior only after compatibility and safety proof
```
