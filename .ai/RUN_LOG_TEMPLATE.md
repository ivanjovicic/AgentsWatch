# Compact Run Log Template

Copy this file into:

```text
.ai/runs/<yyyy-mm-dd>-<prompt-id>-evidence.md
```

Do not commit this template as the run log. Fill the copied file.

Repo: `ivanjovicic/AgentsWatch`

```text
# <Prompt ID> Evidence

Prompt ID:
Queue:
Agent/tool:
Model provider:
Model name/id:
Model mode/settings:
Client/IDE:
Run mode:
Token budget:
Actual context:
Started from queue status:
Local collision check:
Relevant prior mistakes read:
How this run avoids prior mistakes:
Elapsed time:
Phase time breakdown:

## Files inspected

-

## Files changed

-

## Commands run

-

## What was done

-

## What was missed

-

## Out-of-scope discoveries observed

- Discovery candidate:
- Category:
- Evidence summary:
- Why outside current task:

Use `none found` when there were no meaningful findings.

## Discovery reconciliation

- Reconciliation status: reconciled | blocked - <reason> | not run - <reason>
- Discoveries created: none
- Discoveries updated: none
- Duplicates linked: none
- Primary owners assigned: none
- Canonical docs updated: none
- Follow-up prompts generated: none
- Queue rows created or updated: none
- Unresolved discoveries: none

## Validation run

-

## Validation not run

-

## Waste categories

-

## Mistakes observed

- Mistake ID:
- New or repeated:
- Root cause:
- Prevention added:
- Existing rule that should have prevented it:
- Did this run update a rule/prompt/test/queue/lint:

## Where time/context was wasted

-

## Why waste happened

-

## What the next agent should avoid

-

## Docs/rules updated to prevent repeat

-

## Queue updated

-

## New optimized prompt added

-

## Follow-up prompt

-

## Completion %

-

## Residual risk

-

## Commit SHA

-
```

Required placeholder values when unknown:

```text
unknown-not-exposed      # model/client value not visible
unknown-not-recorded     # timing/phase value not captured
none                     # truly none
none found               # no meaningful out-of-scope discovery
reconciled               # every discovery has a disposition
blocked - <reason>       # reconciliation could not finish
not run - <reason>       # validation or reconciliation skipped
```

Mistake-learning placeholders:

```text
Relevant prior mistakes read: none
How this run avoids prior mistakes: none
Mistakes observed: none
```

Discovery placeholders:

```text
Out-of-scope discoveries observed: none found
Reconciliation status: reconciled
Discoveries created: none
Discoveries updated: none
Duplicates linked: none
Primary owners assigned: none
Follow-up prompts generated: none
Unresolved discoveries: none
```

If a mistake is observed, use an existing `AW-MISTAKE-*` ID from `docs/ai/learning/MISTAKE_LEDGER.md` or add a new card using `docs/ai/learning/MISTAKE_CARD_TEMPLATE.md`.

If a meaningful out-of-scope finding is observed, use `.ai/DISCOVERY_RECORD_TEMPLATE.md` and `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`. Do not hide unrelated implementation inside the current task.

A run may be task-complete but is not learning-complete while discovery reconciliation is missing.

AgentsWatch validation examples:

```text
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
git diff --check
docs-only: verified linked paths exist
docs-only: discovery records, prompts, and queue links reconciled
CI: No GitHub Actions evidence found via connector
```
