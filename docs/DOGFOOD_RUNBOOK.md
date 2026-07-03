# AgentsWatch Dogfood Runbook

Last aligned: 2026-07-03  
Status: operational dogfood and value-proof guide

## Purpose

Define how to dogfood AgentsWatch so the result is reproducible evidence, not an anecdote.

Use with `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`.

Dogfood answers whether the tool is useful on real work. Deterministic tests and acceptance scenarios still prove command correctness.

## Prerequisites

Do not use dogfood as product proof until:

- build/test evidence exists for the tested commit;
- CLI smoke evidence exists;
- required capability rows identify what is being tested;
- the commands used are implemented enough for the scenario;
- raw evidence can be stored without exposing private source/prompts/logs.

Early manual dogfood may still be recorded as exploratory evidence, but must not establish L5 or public claims.

## Dogfood modes

### Exploratory

Use to find usability gaps and discoveries.

- one workflow may be enough;
- no percentage claims;
- result remains directional.

### Capability verification

Use to move a capability toward L5.

- capability ID and acceptance criteria are named;
- deterministic proof already exists;
- at least two real repositories when applicable;
- failures are retained.

### Paired benchmark

Use for efficiency/value claims.

- baseline and AgentsWatch-assisted variants;
- same commit, intended outcome, validation target, and comparable model/tool settings;
- follow the minimum samples and calculation rules in the benchmark methodology.

## Run steps

1. Choose repository commit and task category.
2. Name capability IDs and acceptance criteria under evaluation.
3. Decide exploratory, capability, or paired mode.
4. Save the raw task safely.
5. Define success/validation before starting.
6. For paired mode, define baseline and assisted variants.
7. Run the workflow without changing the measurement protocol mid-pair.
8. Record files inspected/changed when observable.
9. Record searches/commands and repeats when observable.
10. Record validation and task outcome.
11. Record scope/evidence mistakes caught or introduced.
12. Record discoveries and follow-up prompts.
13. Score quality with the common reviewer rubric.
14. Store limitations and confounders.
15. Update capability maturity only when requirements are met.

## Evidence location

```text
examples/dogfood/<date>-<repo>-<task>-<variant>.md
examples/benchmarks/<benchmark-id>/
```

## Evidence template

```text
Dogfood/benchmark ID:
Pair ID:
Mode:
Variant: exploratory | baseline | assisted
Repository and commit:
Capability IDs:
Acceptance criteria:
Task category:
Raw task:
Final prompt(s):
Tool/model/settings or unknown-not-exposed:
Environment:
Start/end or unknown-not-recorded:
Measured token/time/cost fields:
Estimated fields clearly labeled:
Files inspected:
Files changed:
Searches/commands:
Repeated actions:
Validation:
Task outcome:
Acceptance result:
Scope violations:
Evidence/claim mistakes:
Discoveries:
Human interventions:
Reviewer rubric:
Residual risk:
Limitations:
```

## Good first dogfood tasks

- docs broken-reference audit;
- CLI help/version proof;
- init no-overwrite hardening;
- git parser edge-case test;
- report format/golden test;
- discovery reconciliation on one run;
- evidence/claim lint on synthetic bad logs.

Avoid as first proof:

- dashboard/SaaS;
- major refactor;
- multi-repo implementation;
- tasks with no objective validation;
- tasks where token/time values must be guessed.

## Success criteria

A useful dogfood run produces:

- a validated task outcome;
- clear capability/acceptance mapping;
- compact reproducible evidence;
- at least one useful scope, risk, evidence, or learning result;
- honest failures/limitations;
- routed discoveries;
- no unsupported maturity/percentage claim.

## Claim gate

One dogfood run may show an example, but cannot prove a general percentage saving.

Public token/time/cost claims require the sample, paired design, measured values, quality guardrail, and independent calculation review defined in `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`.
