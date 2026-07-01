# AgentsWatch Token Waste Metrics

Last aligned: 2026-07-01  
Status: draft measurement contract

## Purpose

Define how AgentsWatch talks about token waste without making unsupported claims.

AgentsWatch does not make models cheaper per token. It reduces waste by making agent runs smaller, scoped, fresh, and evidence-driven.

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
- whole-repo reviews avoided;
- validation commands run;
- validation commands skipped;
- scope expansion events;
- stopped runs due to budget.

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
Review scope: diff-only/changed-files/whole-repo
Context pack: <pack or unknown>
Repo map used: yes/no
Stale context items: <n or unknown>
Static prefix reused: yes/no/unknown-not-exposed
Cached input tokens: <n or unknown-not-exposed>
Output tokens: <n or unknown-not-exposed>
Largest waste source: <text>
Next token-saving improvement: <text>
```

---

## Claims policy

Safe claim:

```text
AgentsWatch reduces AI coding-agent token waste by splitting prompts, limiting scope, tracking diffs, avoiding stale context, using compact repo maps, and generating handoffs.
```

Conditional claim:

```text
30-50% less token waste on typical multi-file tasks, when scope limiting, cache-aware prompt shape, repo-map selection, and handoff reuse are applied.
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
- cache-aware metrics where exposed;
- stale-context count;
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
- producing shorter prompts that cause broader tool exploration later.
