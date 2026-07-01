# Agent Mistake Card Template

Status: copyable template  
Last aligned: 2026-07-01

Use this template when a run discovers a new agent mistake or repeats a known mistake.

Mistake cards belong in:

```text
docs/ai/learning/MISTAKE_LEDGER.md
```

## Copyable mistake card

```text
### AW-MISTAKE-<AREA>-<NNN> — <short title>

Severity: P0 | P1 | P2
Status: Open | Mitigated | Watching | Retired | False alarm
First seen:
Repeated in:

Problem:
- What did the agent do wrong?
- What evidence proves it?

Impact:
- What did this cost: time, tokens, wrong status, bad validation, product risk, release risk?

Root cause:
- Why did the agent make this mistake?
- Was the prompt broad, queue stale, validation missing, template unclear, or rule not enforced?

Prevention:
- Which rule/prompt/test/queue/lint/template was changed?
- If no change was made, why not?

Next check:
- What should the next agent verify to prove this mistake did not repeat?
```

## Severity guide

```text
P0 = can cause unsafe release decisions, broken validation gates, false runtime claims, or product-trust risk
P1 = repeated wasted time/context, misleading Done status, missing evidence, validation claim without proof
P2 = local inefficiency, stale wording, missing convenience prompt, minor docs ambiguity
```

## Required run-log cross-reference

Every run log that observes a mistake should include:

```text
Relevant prior mistakes read:
- AW-MISTAKE-...

## Mistakes observed

- Mistake ID:
- New or repeated:
- Root cause:
- Prevention added:
- Existing rule that should have prevented it:
- Did this run update a rule/prompt/test/queue/lint:
```

If no mistakes were observed:

```text
## Mistakes observed

- none
```
