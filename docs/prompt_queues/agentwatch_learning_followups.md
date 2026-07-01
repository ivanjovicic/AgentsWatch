# AgentsWatch Learning Follow-up Queue

Last aligned: 2026-06-30  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: AgentsWatch mistake-learning follow-ups  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/prompt_queues/agentwatch_learning_followups.md`  
Parent queue: [agentwatch_mvp.md](agentwatch_mvp.md)

## Purpose

Hold missing or later-phase AgentsWatch mistake-learning prompts that should not bloat the first MVP skeleton.

## Read first

- `../MISTAKE_LEARNING_SPEC.md`
- `../CLI_LEARNING_ADDENDUM.md`
- `../PRODUCT_SPEC.md`
- `../MVP_ROADMAP.md`
- `agentwatch_mvp.md`

## Rules

- Local-first only.
- Do not upload mistake ledgers, source code, diffs, prompts, or run logs.
- Do not add dashboard/SaaS/team behavior from this queue.
- Mistake learning must use local files before database/UI work.
- Repeated mistakes must produce a prevention rule, prompt, test, lint, or documented no-op.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-LEARN-001 | Ready after AW-LIFECYCLE-001 and AW-PRIVACY-001 | Define and generate local mistake-learning files/templates. |
| AW-LEARN-002 | Ready after AW-003 and AW-LEARN-001 | Implement `agentswatch mistakes list/check` over local markdown ledgers and run logs. |
| AW-LEARN-003 | Ready after AW-009 and AW-LEARN-002 | Implement evidence/learning lint checks for Done rows, run logs, score caps, and repeated mistakes. |
| AW-LEARN-004 | Ready after dogfood evidence | Run a mistake rollup over dogfood logs and generate prevention updates. |
| AW-LEARN-005 | Backlog | Add `mistakes add --from-run` once check/list behavior is stable. |
| AW-LEARN-006 | Backlog | Add exportable safe aggregate mistake summary without source-code or diff content. |
