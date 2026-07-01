# Shared Agent Operating Standard

Last aligned: 2026-07-01  
Scope: `ivanjovicic/AgentsWatch`  
Role: local-first AI coding-agent supervisor and prompt/token optimizer

This document aligns AgentsWatch with the MathLearning Flutter and MathLearning backend agent standards.

## Source-of-truth order

1. Current code and tests.
2. `AGENTS.md`.
3. This shared standard.
4. `docs/AGENT_RUN_LOG_ENFORCEMENT.md`.
5. `.ai/RUN_LOG_TEMPLATE.md` and `.ai/runs/README.md`.
6. `docs/ai/learning/MISTAKE_LEDGER.md`.
7. Prompt router / owning queue.
8. Architecture, product, roadmap, audit, and planning docs.

Current code/tests and committed evidence are stronger than chat memory or old queue rows.

## One prompt, one mode

Use one primary run mode:

```text
validation-only
investigation-only
implementation
tests
docs/evidence
review-only
diff-only review
```

Split broad prompts. Do not mix investigation, implementation, tests, docs, and review in one run unless the prompt explicitly scopes that combination.

## Required prompt fields

Every non-trivial prompt must name:

```text
Repository:
Prompt ID:
Queue:
Run mode:
Token budget:
Owned paths:
Avoid paths:
Validation:
Stop rules:
Expected evidence:
Relevant prior mistakes read:
```

## Token economy

Use the smallest useful context:

| Budget | Max docs before first action | Max files inspected | Max files edited |
|---|---:|---:|---:|
| Low | 3 | 8 | 3 |
| Medium | 5 | 15 | 6 |
| High | 8 | review-first | scoped only |

Stop and split when the prompt becomes whole-repo analysis, validation is unclear, or evidence would require reconstructing many old commits.

## Evidence gate

Every non-trivial run must produce or update:

```text
.ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
```

Use `.ai/RUN_LOG_TEMPLATE.md`. Required placeholders:

```text
unknown-not-exposed
unknown-not-recorded
not run - <reason>
none
```

A queue row cannot be high-confidence `Done` without a run-log path or explicit fallback.

## Score caps

| Situation | Maximum score |
|---|---:|
| Run log exists, is referenced, and validation is recorded | 95-100% |
| Useful audit completed, target logs missing | 75% |
| Queue row updated without durable run evidence | 70% |
| Model/timing missing and not `unknown-*` | 65% |
| Validation/path verification not run for docs/evidence | 85% |
| Docs-only audit described as runtime fix | 70% |
| Mistake observed but not classified | 80% |
| Repeated mistake without prevention update | 75% |

## Mistake-learning loop

A run is not learning-complete until every observed mistake is classified as:

```text
new mistake with a mistake card
repeated mistake with a rule/prompt/test/queue/lint update
false alarm with explanation
```

Before start, read `docs/ai/learning/MISTAKE_LEDGER.md`. Before Done, update the ledger/rule/prompt/test/queue/lint when a repeated mistake appears.

## Docs-only vs runtime truth

Use exact statuses:

```text
docs-only audit
prompt-ready
runtime-fixed
test-validated
Needs evidence sync
Blocked
```

A docs-only audit is not runtime proof.

## AgentsWatch-specific rule

Gate 0 validation comes before feature work. Until restore/build/test/CLI smoke evidence is current, feature prompts remain blocked unless docs-only and explicitly not runtime-complete.

## Mechanical evidence validation

For any prompt that touches prompt queues, `.ai/runs`, `AGENTS.md`, `docs/DOCS_INDEX.md`, run-log enforcement, run-log templates, mistake ledgers, evidence docs, or cross-repo standard docs, run:

```text
python scripts/validate_agent_evidence.py
```

For a focused repair/backfill pass, the agent may use:

```text
python scripts/validate_agent_evidence.py --referenced-run-logs-only
```

Record the command and result in the run log. If the agent cannot run local commands because it is using only a connector or remote file API, write:

```text
Validation not run: not run - connector-only docs update, no local checkout
```

Do not replace this mechanical check with manual reading unless the skip reason is explicit.

## Final response minimum

```text
Run log:
Mistake IDs:
Files changed:
Validation:
Commit SHA:
Completion %:
Residual risk:
Next prompt:
```
