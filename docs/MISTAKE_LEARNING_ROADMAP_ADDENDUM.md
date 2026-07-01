# AgentsWatch Mistake Learning Roadmap Addendum

Last aligned: 2026-06-30  
Status: planning addendum  
Parent roadmap: [MVP_ROADMAP.md](MVP_ROADMAP.md)  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/AGENTWATCH_MISTAKE_LEARNING_ROADMAP_ADDENDUM.md`

## Purpose

This addendum places mistake learning into the AgentsWatch roadmap without expanding the MVP into dashboard/SaaS work.

AgentsWatch should learn from mistakes through a closed loop:

```text
mistake -> classification -> prevention rule/prompt/test/lint -> verification that it did not repeat
```

## Phase placement

### Phase 0 — dogfood docs

Use real repos, especially MathLearning, to collect evidence of repeated agent mistakes:

- Done row without run log;
- model/client metadata missing;
- elapsed/phase timing missing;
- completion score contradicts residual risk;
- queue/router status drift.

### Phase 1 — local file templates

`agentswatch init` should create or detect:

```text
.ai/RUN_LOG_TEMPLATE.md
.ai/learning/MISTAKE_LEDGER.md
.ai/learning/MISTAKE_CARD_TEMPLATE.md
.ai/learning/ROLLUP_TEMPLATE.md
```

### Phase 2 — local mistake commands

Add:

```bash
agentswatch mistakes list
agentswatch mistakes check <run-log>
agentswatch mistakes add --from-run <run-log>
```

### Phase 3 — rollups and lint

Add:

```bash
agentswatch rollup mistakes --last 5
agentswatch lint evidence
agentswatch lint learning
```

## Gates

Do not start dashboard/SaaS/team mistake features until the local CLI can:

1. create/detect learning files;
2. validate run logs;
3. list and check mistake IDs;
4. catch repeated mistake without prevention update;
5. produce a local rollup;
6. dogfood on MathLearning or another real repo.

## Non-goals

- No cloud mistake database in MVP.
- No team leaderboard.
- No remote policy blocker.
- No source-code upload.
- No exact token analytics unless provider data is available.
