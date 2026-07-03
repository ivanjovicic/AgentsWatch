# AgentsWatch Learning Follow-up Queue

Last aligned: 2026-07-03  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: AgentsWatch mistake-learning follow-ups  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/prompt_queues/agentwatch_learning_followups.md`  
Parent queue: [agentwatch_mvp.md](agentwatch_mvp.md)  
Related queue: [agentwatch_discovery_and_self_improvement.md](agentwatch_discovery_and_self_improvement.md)

## Purpose

Hold missing or later-phase AgentsWatch mistake-learning prompts that should not bloat the first MVP skeleton.

Mistakes and discoveries are related but different:

- a mistake records repeatable agent/process behavior;
- a discovery records useful work, risk, or knowledge found outside the active task;
- one finding may update both systems, but every item still needs one primary owner.

## Read first

- `../MISTAKE_LEARNING_SPEC.md`
- `../CLI_LEARNING_ADDENDUM.md`
- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`
- `../PRODUCT_SPEC.md`
- `../MVP_ROADMAP.md`
- `agentwatch_mvp.md`
- `agentwatch_discovery_and_self_improvement.md`

## Rules

- Local-first only.
- Do not upload mistake ledgers, discovery records, source code, diffs, prompts, or run logs.
- Do not add dashboard/SaaS/team behavior from this queue.
- Mistake learning and discovery tracking must use local files before database/UI work.
- Repeated mistakes must produce a prevention rule, prompt, test, lint, queue update, or documented no-op.
- Meaningful out-of-scope findings must be reconciled into the discovery inbox instead of remaining only in a run log.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-LEARN-001 | Ready after AW-LIFECYCLE-001 and AW-PRIVACY-001 | Define and generate local mistake-learning files/templates. |
| AW-LEARN-002 | Ready after AW-003 and AW-LEARN-001 | Implement `agentswatch mistakes list/check` over local markdown ledgers and run logs. |
| AW-LEARN-003 | Ready after AW-009 and AW-LEARN-002 | Implement evidence/learning lint checks for Done rows, run logs, score caps, repeated mistakes, and missing discovery reconciliation. |
| AW-LEARN-004 | Ready after dogfood evidence | Run a mistake rollup over dogfood logs and generate prevention updates. |
| AW-LEARN-005 | Backlog | Add `mistakes add --from-run` once check/list behavior is stable. |
| AW-LEARN-006 | Backlog | Add exportable safe aggregate mistake summary without source-code, diff, or discovery content. |
| AW-LEARN-007 | Ready as docs/evidence workflow | Use DISC-005 to audit recent run logs for findings that were mentioned but never captured or routed. |
| AW-LEARN-008 | Blocked until AW-DISC-003/006 | Enforce discovery reconciliation from `finish`, evidence lint, and learning-complete status. |
