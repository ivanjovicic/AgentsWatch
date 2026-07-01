# Agent Run Log Enforcement Gate

Last aligned: 2026-07-01  
Status: mandatory closure gate for non-trivial prompts  
Scope: `ivanjovicic/AgentsWatch`

## Purpose

AgentsWatch must dogfood the same evidence and mistake-learning loop it is designed to productize.

Hard rule:

```text
No complete run log = no high-confidence Done row.
No classified mistake = no learning-complete run.
```

## Applies when

Use this gate for every non-trivial run:

- CLI/runtime implementation;
- tests and validation;
- prompt queue/status edits;
- docs/evidence/audit work;
- product/spec changes;
- dogfood runs.

Tiny typo-only edits may skip a dedicated run log only if the completion row says why.

## Start-of-run requirement

Before editing runtime files, broad docs, or prompt queues, create or plan:

```text
.ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
```

Minimum start stub:

```text
Prompt ID:
Queue:
Agent/tool:
Model provider:
Model name/id:
Model mode/settings:
Client/IDE:
Run mode:
Token budget:
Started from queue status:
Local collision check:
Relevant prior mistakes read:
How this run avoids prior mistakes:
```

Use `unknown-not-exposed` for model fields that are not visible. Do not guess.

## Done-row blocker

A prompt must not be marked `Done` unless either:

1. a compact `.ai/runs/<date>-<prompt-id>-evidence.md` log exists and is referenced; or
2. the queue row includes `Run log: fallback <reason>` with the same required fields.

A row with only commit SHA, tests, and residual risk should be `Needs evidence sync` or capped.

## Evidence completion score cap

| Situation | Maximum score |
|---|---:|
| Target run log created, referenced, validated | 95-100% |
| Useful audit completed, target run logs still missing | 75% |
| Queue rows updated, no durable `.ai/runs` evidence | 70% |
| Model/timing fields missing and not `unknown-*` | 65% |
| Docs/evidence change without path verification | 85% |
| Prompt required logs but only reported missing | 60% |
| Mistake observed but not classified | 80% |
| Repeated mistake without rule/prompt/test/queue/lint update | 75% |
| Docs-only audit claimed as runtime feature/fix | 70% |
| Validation claimed without command or skip reason | 80% |

Backfill prompts cannot claim 100% if residual risk says target evidence is still missing.

## Mistake learning gate

Before starting, read `docs/ai/learning/MISTAKE_LEDGER.md` and record relevant IDs in the run log.

Before completion, classify every observed mistake as:

```text
new mistake with a mistake card
repeated mistake with a rule/prompt/test/queue/lint update
false alarm with explanation
```

Run log must include:

```text
## Mistakes observed

- Mistake ID:
- New or repeated:
- Root cause:
- Prevention added:
- Existing rule that should have prevented it:
- Did this run update a rule/prompt/test/queue/lint:
```

If none: `Mistakes observed: none`.

## Docs-only vs runtime

| Run mode | Allowed completion claim |
|---|---|
| `docs/evidence`, `docs/audit`, `review-only` | Documentation/process/review only; no runtime feature/fix claim |
| `implementation`, `tests` | Requires code/test evidence in run log |
| `validation-only` | Reports validation state only |
| `evidence backfill` | Repairs logs/queue rows; may not change runtime |

## Required final fields

Use `.ai/RUN_LOG_TEMPLATE.md`. Never leave required fields blank.

AgentsWatch validation fields:

```text
Validation run: dotnet test AgentsWatch.sln — passed
Validation not run: not run - <reason>
CI: No GitHub Actions evidence found via connector
```

## Completion row shape

```text
Done <percent>% (YYYY-MM-DD, <agent/lane>, commit `<sha>`)
Model: <provider>/<model or unknown-not-exposed> via <client/tool>
Validation: <exact command or docs-only validation>
Run log: .ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
Mistakes: <AW-MISTAKE-* IDs or none>
Waste: <categories or none recorded>
Missed: <missed work or none known>
Follow-up: <prompt ID or none>
Residual risk: <one sentence>
```

## Final response requirement

Mention:

- run-log path;
- mistake IDs or `none`;
- commit SHA;
- validation run or skipped reason;
- biggest waste category;
- residual risk;
- next prompt, if selected.
