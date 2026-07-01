# AgentsWatch Run Logs

Status: mandatory evidence directory  
Last aligned: 2026-07-01

Each non-trivial agent run should create one compact log here:

```text
.ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
```

Use `.ai/RUN_LOG_TEMPLATE.md`.

## Naming rules

- Use lowercase prompt IDs where practical.
- Keep names compact and searchable.
- One prompt gets one run log.
- Backfills must say `Run mode: evidence backfill`.

Examples:

```text
.ai/runs/2026-07-01-aw-val-001-evidence.md
.ai/runs/2026-07-01-prompt-system-audit-evidence.md
.ai/runs/2026-07-01-cross-repo-agent-standard-sync-evidence.md
```

## Required evidence

A log must record:

- prompt ID and queue;
- model/client metadata or `unknown-not-exposed`;
- elapsed/phase timing or `unknown-not-recorded`;
- files inspected and changed;
- validation run or skipped reason;
- what was done and missed;
- waste categories;
- relevant prior mistakes read;
- mistakes observed;
- docs/rules/queue/prompt updates;
- follow-up prompt;
- completion percent;
- residual risk;
- commit SHA.

## Learning rule

Before starting, read `docs/ai/learning/MISTAKE_LEDGER.md` and name relevant mistake IDs.

Before marking Done, every observed mistake must be one of:

```text
new mistake with a mistake card
repeated mistake with a rule/prompt/test/queue/lint update
false alarm with explanation
```

## Score cap reminder

A docs-only audit, missing validation, missing model/timing, missing run log, or unclassified mistake caps completion according to `docs/AGENT_RUN_LOG_ENFORCEMENT.md`.
