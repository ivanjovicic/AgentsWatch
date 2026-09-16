# Dogfood and External Validation Review

## 30-run dogfood design

AW-VFY-010 should deliberately cover a heterogeneous corpus rather than 30 easy happy-path runs.

Required coverage:
- clean worktree;
- dirty worktree;
- same pre-existing dirty file changed further;
- docs-only task;
- backend task;
- tests-only task;
- database/migration task;
- refactor;
- validation failure/blocked environment;
- intentional scope violation;
- .NET;
- Flutter;
- multiple agent vendors where practical.

For every run record:
- what the agent claimed;
- what Git/CI/native logs already showed;
- what AgentsWatch added;
- whether the finding changed a review/rework/merge/evidence decision;
- false positive?;
- false attribution?;
- ambiguity?;
- verification overhead time;
- estimated reviewer time saved;
- whether the reviewer would use it next run.

## Candidate precision/error targets

These are product hypotheses, not industry standards:
- material false attribution: target **0 observed** in dogfood;
- deterministic blocking-finding precision: target >95%;
- false blocking should be rare (<1–2% candidate threshold);
- ambiguous causality should become `NeedsReview`, not a fabricated fail/pass.

## AW-VFY-011 review

The existing prompt is directionally strong.

Strengthen the baseline comparison to include, where applicable:
- Qodo;
- GitHub/Cursor/Claude native evidence/hooks;
- participant’s actual Git/CI/PR process.

The key pass signal is not “useful” or “interesting”. It is:

> **AgentsWatch changed a real merge/rework/evidence decision that the baseline did not surface early or consistently enough.**

Target cohort: 10–20 external developers/teams, ideally 5–30 developer agent-heavy teams and at least two agent ecosystems.

## AW-VFY-012 review

Only run after external value passes.

Count as commercial evidence:
- paid pilot;
- invoice-ready commitment;
- signed paid design-partner agreement;
- procurement/security review tied to an agreed commercial next step.

Do not count:
- GitHub stars;
- survey “would pay” answers;
- compliments;
- free ongoing usage.

## Recommended 90-day evidence chain

`Gate 0 -> minimal evidence spine -> 30 adversarial receipts -> external decision-change study -> paid design-partner ask`

Do not insert dashboard/SaaS/integration breadth before these gates.
