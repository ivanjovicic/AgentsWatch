# AgentsWatch Community Opportunity Validation Queue

Last aligned: 2026-07-03  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: product discovery, feasibility, prototypes, and evidence for AW-CAP-028 through AW-CAP-036

## Purpose

Convert repeated community problems into validated, bounded product opportunities without confusing online discussion with market proof or starting live integrations too early.

## Read first

- `../AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`
- `../POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`
- `../COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`
- `../COMMUNITY_OPPORTUNITY_ARCHITECTURE_ADDENDUM.md`
- `../COMMUNITY_OPPORTUNITY_BACKLOG.md`
- `../FEATURE_CAPABILITY_REGISTRY.md`
- `../PROOF_AND_VERIFICATION_STRATEGY.md`
- `PROMPT_QUEUE_ROUTER.md`

## Hard rules

- Social/forum/issue frequency is a problem signal, not market size.
- Keep AW-CAP-028 through AW-CAP-036 at L1 until runtime code exists.
- Interviews and real examples precede broad implementation.
- Import-only and dry-run prototypes precede live observation, enforcement, process control, agent spawning, or GitHub posting.
- Do not require source upload or hidden telemetry.
- Unknown provider events, prices, or capabilities remain unknown.
- Every advancing idea needs success metrics, kill criteria, and a proof plan.

## Discovery prompts

| ID | Status | Prompt | Purpose |
|---|---|---|---|
| OPP-RSCH-001 | Ready | `../prompts/OPP-001-user-interview-synthesis.md` | Validate pain, workflow, workarounds, and adoption intent. |
| OPP-RSCH-002 | Ready | `../prompts/OPP-002-adapter-feasibility.md` | Map stable local logs/hooks/exports and blind spots. |
| OPP-RSCH-003 | Ready | `../prompts/OPP-003-competitive-substitutes.md` | Identify substitutes, differentiation, and kill conditions. |
| OPP-RSCH-004 | Ready after interviews | — | Re-score opportunity map using evidence rather than desk research. |
| OPP-RSCH-005 | Ready after interviews | — | Select one free wedge and one Pro-value prototype. |

## Foundation prompts

| ID | Gate | Purpose |
|---|---|---|
| OPP-EVENT-001 | Main Gate 0 + OPP-RSCH-002 | Define normalized event schema and two synthetic adapters. |
| OPP-EVENT-002 | OPP-EVENT-001 | Implement idempotent import-only local event journal. |
| OPP-EVENT-003 | OPP-EVENT-001 | Add adapter capability declaration and blind-spot reporting. |
| OPP-PRIV-001 | Before real logs | Add redaction, path-boundary, corrupt-input, and no-network fixtures. |

## Prototype prompts

| ID | Gate | Capability | Purpose |
|---|---|---|---|
| OPP-PROT-001 | Core config/file safety | AW-CAP-031 | Rules Compiler and Drift Detector prototype. |
| OPP-PROT-002 | Run/handoff model | AW-CAP-030 | Manual context snapshot and fresh-session resume pack. |
| OPP-PROT-003 | Event foundation | AW-CAP-028 | Import-only Flight Recorder timeline. |
| OPP-PROT-004 | Event foundation | AW-CAP-029 | Deterministic claim-to-evidence verifier. |
| OPP-PROT-005 | Event foundation | AW-CAP-032 | Offline loop and budget analyzer. |
| OPP-PROT-006 | Threat model | AW-CAP-033 | Policy dry-run and explain mode. |
| OPP-PROT-007 | Git/worktree fixtures | AW-CAP-034 | Worktree ownership planner; no agent spawning. |
| OPP-PROT-008 | Git/evidence spine | AW-CAP-035 | Local PR review-evidence packet. |
| OPP-PROT-009 | Stable metrics | AW-CAP-036 | Small regression canary suite. |

## Dogfood and advancement prompts

| ID | Gate | Purpose |
|---|---|---|
| OPP-DOG-001 | Rules prototype | Test onboarding a second agent tool and rule-drift detection. |
| OPP-DOG-002 | Context prototype | Paired fresh-session resume comparison. |
| OPP-DOG-003 | Timeline/trust prototype | Verify known supported, contradicted, and missing claims. |
| OPP-DOG-004 | Loop prototype | Measure useful detections and false-positive rate. |
| OPP-DOG-005 | Policy dry-run | Evaluate safety findings versus approval/noise burden. |
| OPP-DOG-006 | Worktree planner | Run two-worker and five-worker synthetic/real independent tasks. |
| OPP-DOG-007 | PR packet | Maintainer/reviewer comparison study. |

## Live/enforcement prompts — blocked initially

| ID | Blocked until | Purpose |
|---|---|---|
| OPP-LIVE-001 | useful low-noise offline loop dogfood | Live loop warning wrapper. |
| OPP-LIVE-002 | threat model + independent security review | Optional policy execution broker. |
| OPP-LIVE-003 | worktree planner dogfood + stable agent adapter | Controlled worker launching. |
| OPP-LIVE-004 | local review packet dogfood + explicit approval flow | Draft/post GitHub review output. |
| OPP-LIVE-005 | two stable event adapters + privacy proof | Live provider event ingestion. |

## Advancement scorecard

An opportunity advances only when:

```text
Problem recognized by target users
Real examples available
Prototype input available locally
Output changes a real decision
False-positive burden acceptable
Privacy boundary credible
Maintenance cost understood
Differentiation still exists
Proof plan executable
```

Use `Advance`, `Revise`, `Park`, or `Reject`; do not leave every idea indefinitely marked planned.

## Current recommended sequence

```text
OPP-RSCH-001
-> OPP-RSCH-002
-> OPP-RSCH-003
-> choose between Rules Compiler and Context Snapshot as first wedge
-> event foundation
-> Flight Recorder + Trust Ledger
-> offline Loop Guard
-> Policy/Worktree/PR dry-run prototypes
-> dogfood
-> live behavior only after evidence
```
