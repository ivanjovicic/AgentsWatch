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
not run - <reason>       # validation skipped or blocked
```

Mistake-learning placeholders:

```text
Relevant prior mistakes read: none
How this run avoids prior mistakes: none
Mistakes observed: none
```

If a mistake is observed, use an existing `AW-MISTAKE-*` ID from `docs/ai/learning/MISTAKE_LEDGER.md` or add a new card using `docs/ai/learning/MISTAKE_CARD_TEMPLATE.md`.

AgentsWatch validation examples:

```text
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
git diff --check
docs-only: verified linked paths exist
CI: No GitHub Actions evidence found via connector
```
