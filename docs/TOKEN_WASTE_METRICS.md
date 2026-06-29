# AgentsWatch Token Waste Metrics

Last aligned: 2026-06-29  
Status: draft measurement contract

## Purpose

Define how AgentsWatch talks about token waste without making unsupported claims.

AgentsWatch does not make models cheaper per token. It reduces waste by making agent runs smaller, scoped, and evidence-driven.

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
review scope: diff-only / whole repo
```

Value: shows whether review stayed focused.

### Scope creep prevented

```text
scope expansion blocked: yes/no
reason: <reason>
```

Value: shows safety improvement.

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
Review scope: diff-only/whole-repo
Largest waste source: <text>
Next token-saving improvement: <text>
```

---

## Claims policy

Safe claim:

```text
AgentsWatch reduces AI coding-agent token waste by splitting prompts, limiting scope, tracking diffs, and generating handoffs.
```

Conditional claim:

```text
30-50% less token waste on typical multi-file tasks, when scope limiting and handoff reuse are applied.
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
- user notes about repeated context avoided.
