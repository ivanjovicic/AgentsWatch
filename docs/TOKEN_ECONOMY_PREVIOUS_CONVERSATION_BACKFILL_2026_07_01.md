# Token Economy Previous Conversation Backfill — 2026-07-01

Status: backfilled from prior planning conversations  
Scope: AgentsWatch token/context economy docs  
Purpose: preserve useful token-saving ideas from earlier discussions that were not yet fully represented in the newer industry research docs.

## Backfilled ideas

### 1. Repo-specific skill docs

Earlier planning established a reusable skill-doc shape:

```text
When to use
Files to inspect
Rules/invariants
Common mistakes
Required tests
Done criteria
```

This is now represented in `docs/CONTEXT_PACKS.md` as context-pack schema with added token controls:

```text
Max files before expansion
State ownership check
Output mode
Freshness rule
Done blocker
```

Why it saves tokens: agents read a narrow task-specific skill pack instead of rediscovering the same file sets and rules.

### 2. State ownership map as a context filter

Earlier MathLearning discussions repeatedly showed that agents waste tokens by inspecting the wrong layer first:

- UI files for backend-owned economy bugs;
- backend docs for display-only UI polish;
- old audit docs for current runtime behavior;
- broad state-management docs when a single owner path is enough.

Backfilled rule:

```text
Before broad discovery, identify the state owner.
Then read the owner path first.
```

Ownership categories:

- backend-authoritative;
- local cache;
- display-only;
- config;
- file system;
- git/repository state;
- external service.

### 3. Queue lifecycle commands

Earlier AgentsWatch planning included a lightweight flow:

```text
agentwatch next
agentwatch run
agentwatch finish
agentwatch report
```

Backfilled meaning:

- `next`: choose the smallest safe next prompt and context pack;
- `run`: execute within budget;
- `finish`: write evidence, token report, validation, negative cache;
- `report`: summarize cost, waste, result, next action.

Until implemented, run logs should simulate this flow.

### 4. Feature-profile gating

Earlier feature-selection planning introduced feature packages such as:

```text
core
reports
handoff
review
risk
validation
adapters
learning
lint
metrics
dogfood
dashboard
team
cloud
```

Backfilled token-economy rule:

```text
Do not load docs/specs for disabled or future-only feature packages unless the selected prompt needs them.
```

Example: a core/local MVP prompt should not read dashboard/team/cloud docs by default.

### 5. Batch review as token compaction checkpoint

Earlier policy required a review after 3-5 important prompt/rule/queue/evidence commits.

Backfilled token-economy interpretation:

- batch review catches broken links and stale statuses;
- it also creates a natural compaction point;
- after a batch, produce a short rollup so future agents read one rollup before opening all logs.

### 6. Zero-waste domain playbooks

Earlier Access/ERP cutoff planning used a low-waste pattern:

```text
clone/report layer first -> validate counts -> preserve base semantics -> change defaults only after proof
```

Backfilled generic pattern:

```text
Prefer isolated query/report/adapter layer first.
Avoid broad base-layer rewrites before counts/tests prove need.
```

Why it saves tokens: agents do not inspect/change every base table or core file when a reporting layer can localize the change.

### 7. Prompt anatomy from earlier planning

Earlier prompt-system notes required prompts to include:

- goal;
- scope;
- exact files to read;
- non-goals;
- invariants;
- acceptance criteria;
- test plan;
- output format.

Backfilled token-economy addition:

- token budget;
- context pack;
- max files before expansion;
- expected output mode;
- cache profile.

### 8. Negative cache from repeated mistakes

Earlier runs repeatedly showed token waste from rereading stale docs or retrying the same broad searches.

Backfilled rule:

```text
Every non-trivial run should record one thing not to reread or rerun next time, if such waste occurred.
```

Example:

```text
Do not reread next time:
- docs/old_audit.md: superseded by docs/current_status.md
- whole repo grep: use context pack pack.backend.auth-risk first
```

## Files updated by this backfill

- `docs/CONTEXT_PACKS.md`
- `docs/TOKEN_WASTE_METRICS.md`
- `docs/TOKEN_ECONOMY_INDUSTRY_RESEARCH_2026_07_01.md`
- `docs/prompt_queues/token_economy_industry_followups_2026_07_01.md`
- `docs/DOCS_INDEX.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`

## Anti-goal

Do not turn every old idea into always-loaded context. Backfilled ideas should live in indexed docs and context packs, not bloat `AGENTS.md`.
