# AgentsWatch Benchmark and Dogfood Methodology

Last aligned: 2026-07-03  
Status: canonical value-proof contract

## Purpose

AgentsWatch may claim that it reduces wasted context, repeated work, unsafe scope expansion, and validation mistakes only when those claims are supported by repeatable evidence.

Command correctness is proven by deterministic tests. Product usefulness is proven by controlled dogfood and benchmark runs. One does not replace the other.

## Claims this methodology can support

With sufficient evidence:

- prompts become narrower and more complete;
- broad tasks are split into safer phases;
- fewer unrelated files are inspected or changed;
- repeated searches/commands decrease;
- handoffs reduce repeated context loading;
- evidence and scope mistakes are caught;
- total measured tokens/time/cost may decrease for comparable solved tasks.

Do not claim a percentage improvement when tokens/time/cost were estimated rather than measured.

## Benchmark design

Use paired comparisons:

```text
A — baseline workflow without AgentsWatch guidance
B — AgentsWatch-assisted workflow
```

Use the same:

- repository commit;
- task statement and expected outcome;
- model/tool class and settings when controllable;
- environment;
- validation target;
- stopping condition;
- reviewer rubric.

Randomize A/B order across tasks when practical to reduce learning/order bias.

## Task portfolio

Use at least these categories:

1. one-file bug fix;
2. uncertain root-cause investigation;
3. multi-file implementation;
4. test-gap repair;
5. documentation/queue consistency task;
6. diff-only review;
7. validation failure triage;
8. discovery/out-of-scope follow-up workflow.

Do not build the claim from one unusually broad prompt.

## Minimum sample

For an internal directional claim:

- at least 5 comparable tasks across 2 repositories.

For a public percentage claim:

- at least 20 paired tasks;
- at least 3 repositories;
- more than one task category;
- report median, range, and failure count;
- disclose tool/model and measurement limitations.

Stronger claims require larger, more diverse samples.

## Required raw evidence per run

```text
Benchmark ID
Pair ID
Variant: baseline | assisted
Repository and commit
Task category
Raw prompt
Final scoped prompt(s)
Tool/model/settings or unknown-not-exposed
Start/end timestamps or unknown-not-recorded
Token input/output/cached values when exposed
Estimated values clearly labeled when used
Files inspected when observable
Files changed
Searches/commands executed when observable
Repeated searches/commands
Validation result
Task outcome
Acceptance criteria passed
Scope violations
Evidence mistakes
Human interventions
Discoveries created
Residual risk
Reviewer score
```

## Primary metrics

Prefer metrics that are directly observable:

- task solved: yes/no/partial;
- acceptance criteria passed;
- validation pass/fail;
- unrelated files inspected;
- unrelated files changed;
- changed-file count;
- repeated searches;
- repeated failed commands;
- full-repo scans;
- validation commands avoided or selected correctly;
- missing evidence fields;
- claims-vs-actual mismatches;
- discoveries lost vs captured;
- elapsed time when recorded;
- tokens/cost when provider exposes them.

## Derived metrics

```text
Solve rate = solved tasks / attempted tasks
Scope precision = relevant changed files / all changed files
Inspection precision = relevant inspected files / all inspected files
Repeat rate = repeated actions / all actions
Evidence completeness = required evidence fields present / required fields
Discovery capture rate = routed meaningful findings / meaningful findings observed
Token reduction = (baseline tokens - assisted tokens) / baseline tokens
Time reduction = (baseline time - assisted time) / baseline time
Cost per solved task = total measured cost / solved tasks
```

Do not compute precision when relevance cannot be reviewed reliably. Record unknown instead.

## Quality guardrail

Efficiency improvements are valid only if quality does not materially decline.

Compare:

- acceptance criteria;
- test/validation result;
- review findings;
- regressions introduced;
- unresolved risk;
- need for rework.

A lower-token failed task is not a success.

## Reviewer rubric

Score 0-2 for each:

| Dimension | 0 | 1 | 2 |
|---|---|---|---|
| Correctness | wrong/failed | partial | correct/validated |
| Scope discipline | broad/unrelated | minor drift | focused |
| Evidence honesty | misleading/missing | incomplete | complete/truthful |
| Safety | unsafe | unclear | boundaries respected |
| Reusability | no handoff | weak | compact useful handoff |
| Learning | finding lost | noted only | routed/prevention added |

Reviewer should be blind to baseline/assisted identity where practical.

## Dogfood repositories

Use:

- AgentsWatch itself;
- MathLearning;
- at least one additional small representative repository before public claims.

Use synthetic fixture repositories for deterministic tests, but not as the only usefulness evidence.

## Storage

Save evidence under:

```text
examples/dogfood/<date>-<repo>-<task>-<variant>.md
examples/benchmarks/<benchmark-id>/
  protocol.md
  pairs/
  raw-metrics.csv
  summary.md
  limitations.md
```

Do not commit private source, prompts, or logs without explicit approval. Redact or summarize as required.

## Statistical reporting

Report:

- sample size;
- solve rate by variant;
- median and interquartile range where possible;
- task-category breakdown;
- failures and excluded pairs;
- exact measurement availability;
- outliers without silently deleting them;
- known confounders.

Avoid presenting a mean alone for small skewed samples.

## Token-saving claim gate

A percentage claim may be published only when:

1. tokens are measured from provider/tool output, not guessed;
2. paired tasks are comparable;
3. solve quality is equal or better;
4. sample and repositories meet the stated minimum;
5. raw aggregate metrics and limitations are retained;
6. claim wording names the tested task population;
7. the capability registry links the dogfood evidence;
8. an independent review confirms calculations.

Until then use wording such as:

```text
Designed to reduce token and context waste.
Early dogfood evidence is being collected.
```

## Failure reporting

Record where AgentsWatch makes outcomes worse:

- prompt overhead exceeds savings;
- task splitting creates unnecessary latency;
- scope limits hide required dependencies;
- generated validation is incomplete;
- discovery routing creates low-value backlog noise;
- handoff omits critical context.

These failures should create discoveries and prevention prompts.
