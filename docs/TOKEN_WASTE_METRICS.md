# AgentsWatch Token Waste Metrics

Last aligned: 2026-07-01  
Status: draft measurement contract

## Purpose

Define how AgentsWatch talks about token waste without making unsupported claims.

AgentsWatch does not make models cheaper per token. It reduces waste by making agent runs smaller, scoped, fresh, state-aware, and evidence-driven.

---

## MVP-safe metrics

Track these first:

- files inspected;
- files changed;
- ratio of inspected to changed files;
- repeated searches;
- broad commands avoided;
- prompt split count;
- handoff summaries reused;
- long chat history avoided;
- whole-repo reviews avoided;
- validation commands run;
- validation commands skipped;
- scope expansion events;
- stopped runs due to budget;
- negative-cache entries created.

---

## Cache-aware metrics

Track when available from the model/API/tooling. If unavailable, record `unknown-not-exposed` rather than guessing.

```text
Static prefix reused: yes/no/unknown-not-exposed
Prompt cache expected: high/medium/low/unknown-not-exposed
Cached input tokens: <number or unknown-not-exposed>
Uncached input tokens: <number or unknown-not-exposed>
Output tokens: <number or unknown-not-exposed>
Cache breaker: none / timestamp-in-prefix / dynamic-tool-output-in-prefix / changed-rules-prefix / unknown-not-exposed
```

Use these metrics to improve prompt shape, not to claim exact savings without evidence.

---

## Context freshness metrics

Every important context item should be treated as one of:

```text
fresh: current file from working tree/default branch
recent: same-day queue/evidence/status update
stale: historical audit, old evidence, old summary, or unknown freshness
```

Track:

- stale files opened;
- stale docs used only for history;
- stale docs incorrectly treated as current proof;
- current source that overrode stale evidence;
- stale-context mistakes found during review.

---

## Context pack metrics

A run should name the pack it used.

```text
Context pack: pack.backend.auth-risk / pack.flutter.ui-test / pack.evidence.repair / custom
Pack fit: exact / too broad / too narrow
Extra files added: <n>
Scope expansion: yes/no
Expansion reason: <reason or none>
```

Value: compare which packs reliably lead to fewer reads without worse validation.

---

## Skill-pack and state-ownership metrics

From earlier planning, skill docs and state ownership should prevent wrong-layer reads.

```text
Skill/context pack used: yes/no
State owner identified before broad read: yes/no/not-applicable
State owner: backend / local-cache / display-only / config / filesystem / git / external-service / unknown
Wrong-layer files opened: <n or unknown>
Owner-path read first: yes/no/not-applicable
```

Value: detects when agents waste tokens by reading UI for backend-owned bugs, backend for display-only work, or broad docs before the owning path.

---

## Feature-profile metrics

Feature-profile gating prevents future/disabled feature docs from entering context.

```text
Feature profile: core/reports/handoff/review/risk/validation/adapters/learning/lint/metrics/dogfood/dashboard/team/cloud/custom/unknown
Disabled/future feature docs opened: <n>
Feature docs avoided: <n or unknown>
Profile mismatch: yes/no
```

Value: prevents core/local MVP prompts from loading dashboard/team/cloud context by default.

---

## Queue lifecycle metrics

Until `agentwatch next/run/finish/report` exists, run logs should simulate the lifecycle.

```text
Lifecycle step simulated: next/run/finish/report/unknown
Next prompt chosen by router: yes/no
Run stayed within selected pack: yes/no
Finish wrote evidence: yes/no
Report included token waste summary: yes/no
```

Value: makes token savings part of normal agent flow instead of a separate afterthought.

---

## Repository-map metrics

When a repo map exists, track whether it prevented full-file reads.

```text
Repo map used: yes/no
Symbols selected from map: <n or unknown>
Full files avoided: <n or unknown>
Map stale: yes/no/unknown
```

Do not count savings from repo-map use unless the task still passes validation or review.

---

## Derived metrics

### Inspect-to-change ratio

```text
files inspected / files changed
```

High ratio may indicate wasted context, unless the run is intentionally an audit.

### Handoff reuse

```text
handoff reused: yes/no
```

Value: shows whether long chat history was avoided.

### Review narrowing

```text
review scope: diff-only / changed-files / whole repo
```

Value: shows whether review stayed focused.

### Scope creep prevented

```text
scope expansion blocked: yes/no
reason: <reason>
```

Value: shows safety improvement.

### Cache-breaker rate

```text
runs with cache breaker / cache-aware runs
```

High rate means prompt prefixes are unstable.

### Stale-context rate

```text
stale context items / total context items
```

High rate means the agent is spending tokens on old state and increasing correctness risk.

### Wrong-layer read rate

```text
wrong-layer files opened / files inspected
```

High rate means state ownership was not identified early enough.

### Negative-cache reuse

```text
negative-cache entries reused / negative-cache entries available
```

High reuse means agents avoid repeated irrelevant reads.

---

## Reporting shape

```text
Token waste report:
Files inspected: <n or unknown>
Files changed: <n>
Inspect/change ratio: <value or unknown>
Repeated searches: <n or unknown>
Broad commands avoided: <n or unknown>
Handoff reused: yes/no
Long chat history avoided: yes/no/unknown
Review scope: diff-only/changed-files/whole-repo
Context pack: <pack or unknown>
State owner: <owner or unknown>
Wrong-layer files opened: <n or unknown>
Feature profile: <profile or unknown>
Repo map used: yes/no
Stale context items: <n or unknown>
Static prefix reused: yes/no/unknown-not-exposed
Cached input tokens: <n or unknown-not-exposed>
Output tokens: <n or unknown-not-exposed>
Negative-cache entries: <n>
Largest waste source: <text>
Next token-saving improvement: <text>
```

---

## Claims policy

Safe claim:

```text
AgentsWatch reduces AI coding-agent token waste by splitting prompts, limiting scope, tracking diffs, avoiding stale context, using compact repo maps, applying state-ownership filters, and generating handoffs.
```

Conditional claim:

```text
30-50% less token waste on typical multi-file tasks, when scope limiting, cache-aware prompt shape, repo-map selection, state-ownership filtering, context packs, and handoff reuse are applied.
```

Use conditional claims only with dogfood evidence.

Do not claim exact savings until measured on real runs.

---

## Evidence required for public claims

Before publishing savings claims, collect:

- at least 5 real runs;
- before/after prompt examples;
- files inspected vs changed;
- handoff reuse evidence;
- review scope narrowing;
- context-pack names;
- state-owner labels;
- wrong-layer read counts;
- feature profile used;
- cache-aware metrics where exposed;
- stale-context count;
- negative-cache entries created/reused;
- user notes about repeated context avoided;
- validation pass/fail status for each run.

---

## Anti-goals

Do not reduce tokens by:

- skipping current code inspection;
- skipping validation;
- hiding uncertainty;
- deleting durable evidence;
- trusting stale summaries;
- marking docs-only work as runtime proof;
- removing safety/privacy/security checks;
- loading all previous conversations into every run;
- producing shorter prompts that cause broader tool exploration later.
