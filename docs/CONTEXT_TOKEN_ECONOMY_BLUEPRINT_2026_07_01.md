# Context Token Economy Blueprint — 2026-07-01

Status: design blueprint, no runtime implementation yet  
Target repo: `ivanjovicic/AgentsWatch`  
Goal: reduce coding-agent token waste without reducing correctness, safety, validation, or developer control.

## Design principle

AgentsWatch should not make agents blind. It should make context intentional.

The goal is not "use fewer tokens at any cost". The goal is:

```text
right context + right time + right freshness + right validation > less context blindly
```

## Inspiration patterns

This blueprint combines patterns seen in successful agent/coding tools and recent agent-context research:

- repo map over full-file context;
- layered project memory;
- prompt-cache-friendly stable prefixes;
- explicit cache breakpoints / cache hit metrics;
- stale-context guardrails;
- lossless summaries with pointers back to original evidence;
- diff-only and owned-path-first review;
- task packs instead of whole-doc discovery.

## High-impact improvements

### 1. Context packs by task type

Replace broad read-first lists with named packs.

Examples:

| Pack | Purpose | Includes | Excludes |
|---|---|---|---|
| `pack.backend.auth-risk` | auth/security prompt | auth endpoints, auth tests, risk rules, exact queue row | UI docs, performance docs |
| `pack.flutter.ui-test` | targeted UI tests | one screen, one widget, test matrix, quality gate | backend docs, unrelated queues |
| `pack.evidence.repair` | run-log/status repair | queue row, run log, template, validator, mistake ledger | runtime code by default |
| `pack.agentwatch.gate0` | AgentsWatch bootstrap | build plan, router, validation queue, risk register | product feature specs |

Rule: an agent chooses one pack before reading files. Extra files require a scope expansion note.

### 2. Repo map instead of full repository reads

AgentsWatch should generate a compact repository map with:

- file path;
- public classes/functions/commands;
- signatures only;
- owning prompt queue if known;
- last changed commit/date if cheaply available;
- risk tags from docs or queue names.

The repo map is allowed in context. Full files are loaded only after selecting a small target set.

### 3. Stable prefix / variable suffix prompt shape

Prompt text should be split into:

```text
STATIC PREFIX:
- invariant agent rules
- repo rules
- task pack contract
- validation contract
- output contract

VARIABLE SUFFIX:
- exact user request
- changed files
- latest queue row
- fresh command output
- current error text
```

Never put timestamps, branch-specific diffs, or volatile tool output inside the stable prefix.

### 4. Cache-aware metrics

Each non-trivial run log should be able to record:

```text
Static prefix reused: yes/no/unknown
Prompt cache expected: high/medium/low/unknown
Cache breaker found: none / timestamp in prefix / dynamic tool output in prefix / oversized static prefix / unknown
Cached input tokens: <number or unknown>
Uncached input tokens: <number or unknown>
Output tokens: <number or unknown>
```

If the platform does not expose cached token metrics, record `unknown-not-exposed` instead of guessing.

### 5. Stale-context guard

Every retrieved context item should be labeled:

```text
fresh: current file from working tree or default branch
recent: latest run log / queue update from same day
stale: historical doc, old audit, old evidence, or unknown commit freshness
```

Rules:

- stale docs may explain history but cannot prove current behavior;
- stale code snippets must not override current files;
- mixed current + stale context requires the agent to name which source won;
- a prompt cannot claim Done based only on stale evidence.

### 6. Lossless summary pointers

Summaries save tokens only if they point back to exact evidence.

Allowed shape:

```text
Summary: registration failure cleanup exists
Pointer: src/MathLearning.Api/Endpoints/AuthEndpoints.cs:<line or symbol>
Validation pointer: AuthMobileRegistrationAtomicityTests
Confidence: high/medium/low
Freshness: current/recent/stale
```

Forbidden shape:

```text
Summary says it is fixed, trust summary.
```

### 7. Context budget gates

Every prompt gets one of four budgets:

| Budget | Max discovery | Max files opened before edit | Use case |
|---|---:|---:|---|
| XS | 1-2 files | 2 | queue row/status fix |
| S | 3-5 files | 5 | targeted test or doc repair |
| M | 6-10 files | 10 | feature fix with tests |
| L | explicit approval / split | 15+ only with reason | architecture audit |

Crossing the budget requires a run-log entry:

```text
Scope expansion: yes
Why: <specific blocker>
Files added: <list>
```

### 8. Diff-first review

Review prompts must start with changed files and diffs, not the whole repo.

Default review order:

1. changed files list;
2. queue row / prompt contract;
3. touched tests;
4. touched docs;
5. only then related source files.

### 9. Token waste ledger separate from mistake ledger

Some waste is not a mistake. Track it separately so agents do not overcorrect.

Waste examples:

- broad docs read;
- repeated same search;
- opening full files where symbol map was enough;
- stale evidence reconciliation;
- overlong final response;
- duplicate queue/status update;
- running broad test command before targeted command.

### 10. Negative cache: what not to read again

Run logs should record:

```text
Do not reread next time:
- file/path: reason
- doc/path: stale/superseded by X
- command: too broad; use Y instead
```

AgentsWatch can later surface this as a pre-run warning.

### 11. Context fingerprint

Every run should be able to compute a cheap fingerprint:

```text
Context fingerprint:
- prompt pack: pack.evidence.repair@v1
- queue row: hash/commit
- files opened: sorted list hash
- static prefix version: hash
```

Value: compare similar runs and identify why one consumed more context.

### 12. Progressive disclosure commands

Future CLI should prefer progressive context:

```text
agentswatch context plan <prompt-id>
agentswatch context map --changed
agentswatch context expand <symbol-or-file>
agentswatch context stale-check
agentswatch tokens report
```

The CLI should not dump everything by default.

### 13. Small-model preflight

Before asking an expensive coding model to implement, run a cheap/local/static preflight where possible:

- prompt lint;
- owned path collision;
- likely files from repo map;
- stale doc warning;
- budget estimate;
- validation command suggestion.

The expensive model receives the result, not the whole discovery process.

### 14. Output token guard

Long final answers waste output tokens and future context.

Final response should default to:

```text
Done:
- changed files
- validation
- missed/risk
- next prompt
```

Only include long explanations when the user explicitly asks for them or when safety/architecture requires detail.

### 15. Evidence compaction cadence

After every 5 meaningful run logs:

1. create compact rollup;
2. mark older logs as summarized-by rollup;
3. keep originals, but future agents read rollup first;
4. open originals only when exact proof is needed.

## What not to do

Do not reduce tokens by:

- skipping tests;
- skipping current code inspection;
- trusting stale docs;
- deleting evidence logs;
- hiding residual risk;
- marking lower confidence as Done;
- removing safety/security/privacy checks;
- forcing one generic prompt for every task.

## MVP implementation order

1. Add metrics to `TOKEN_WASTE_METRICS.md`.
2. Add prompt queue for context/token-economy hardening.
3. Add context-pack spec.
4. Add repo-map output contract.
5. Add stale-context check spec.
6. Add CLI command contracts only after Gate 0 validation.
7. Dogfood on MathLearning Flutter and backend before public savings claims.

## Success criteria

AgentsWatch can claim improvement only when it has dogfood evidence for:

- lower files-inspected/files-changed ratio on comparable tasks;
- fewer repeated searches;
- more handoff/rollup reuse;
- fewer stale-context mistakes;
- stable or improved validation pass rate;
- no increase in missed-work or false-Done rows.
