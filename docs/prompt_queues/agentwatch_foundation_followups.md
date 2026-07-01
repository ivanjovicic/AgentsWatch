# AgentsWatch Foundation Follow-up Queue

Last aligned: 2026-06-30  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: AgentsWatch CLI foundation follow-ups  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/prompt_queues/agentwatch_foundation_followups.md`  
Parent queue: [agentwatch_mvp.md](agentwatch_mvp.md)

## Purpose

Hold foundational prompts for AgentsWatch CLI behavior that are not part of mistake-learning but are important before broad usage.

## Read first

- `../PRODUCT_SPEC.md`
- `../CLI_SPEC.md`
- `../MVP_ROADMAP.md`
- `../FEATURE_PORTFOLIO_REVIEW_2026_06_30.md`
- `agentwatch_mvp.md`

## Rules

- Local-first only.
- Do not start SaaS, billing, auth, cloud sync, or dashboard implementation here.
- Do not run broad runtime work before validation gates pass.
- Prefer small CLI/core prompts with explicit tests.
- Do not collect or upload source code, prompts, diffs, validation output, or run logs by default.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-SCOPE-001 | Ready | Lock AgentsWatch MVP scope and v1 non-goals before implementation expansion. |
| AW-LIFECYCLE-001 | Ready after AW-SCOPE-001 | Define task/run lifecycle, ids, statuses, and local file outputs. |
| AW-PRIVACY-001 | Ready after AW-SCOPE-001 | Define local privacy/no-telemetry policy. |
| AW-CONFIG-001 | Ready after AW-LIFECYCLE-001 and AW-PRIVACY-001 | Define config schema, feature flags, paths, validation commands, and risk patterns. |
| AW-DOGFOOD-001 | Ready after AW-003/AW-005/AW-006 | Dogfood AgentsWatch on MathLearning and record before/after evidence. |
| AW-PACKAGE-001 | Backlog after CLI MVP evidence | Define .NET global tool and self-contained packaging plan. |
