# AW-VFY-010 — 30-receipt dogfood proof

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-009  
Run mode: dogfood / evidence collection / targeted fixes only  
Budget: staged; do not run as one giant context-heavy session  
Gate: Contract -> Attribution -> Receipt -> Evidence -> Scope -> Claims works end to end

## Purpose

Prove whether AgentsWatch creates enough real value to justify integrations, learning, routing, or a dashboard.

This is not a feature-expansion prompt. It is a structured product proof program executed in small batches.

## Repositories

Use at minimum:

- AgentsWatch itself;
- one real .NET repository;
- one real Flutter repository.

Do not copy private source into AgentsWatch artifacts. Store only the local structured evidence allowed by the product privacy contract.

## Target sample

Collect at least 30 useful receipts across comparable task types where practical:

- docs-only;
- test-only;
- small bug fix;
- .NET endpoint/service change;
- Flutter widget change;
- Flutter provider/state change;
- investigation/fix split;
- intentionally or naturally drifting change;
- run with missing validation;
- run with pre-existing dirty worktree.

## Batch rule

Run in batches of 5–10 receipts. After each batch:

1. review false positives/false negatives;
2. review attribution ambiguities;
3. record user friction;
4. fix only high-confidence defects that invalidate the next batch;
5. do not add unrelated features.

## Metrics

For each receipt capture at minimum:

```text
task type
contract completeness
clean/dirty start
attribution result/ambiguity
attributable file count
scope findings
unsupported/unknown claims
validation evidence status
final decision
manual acceptance/rejection
retry count
handoff reused yes/no
setup/friction note
```

Secondary where available:

```text
validation duration
command breadth
token/cost provider data
```

Do not fabricate token/cost data.

## Required real proof events

Before declaring the MVP validated, obtain at least:

- one real unsupported agent claim caught;
- one real scope-drift case caught;
- one real missing-validation/evidence block;
- multiple dirty-at-start runs with no observed false attribution;
- evidence that the receipt/handoff is useful without full chat history.

## Quality review

Classify each notable finding as:

```text
TruePositive
FalsePositive
TrueNegative
FalseNegative
Unknown/Ambiguous
```

Any silent false attribution is a priority defect and should block broader product expansion until understood.

## Deliverables

Create/update a compact dogfood evidence report containing:

- 30-run summary table or equivalent structured rollup;
- real catches;
- false positives/negatives;
- attribution ambiguity rate;
- user-friction findings;
- most valuable verification capability;
- least valuable/noisy capability;
- top 3 product changes justified by evidence;
- explicit next-phase recommendation.

## Decision gate

Choose the next phase only from evidence.

Candidate directions:

1. MCP exposure if contract/start/finish/verify is already valuable in daily agent workflows;
2. GitHub/PR check if review/evidence is the strongest value;
3. Validation Economy if repeated broad validation is a measured problem;
4. Mistake Learning if repeated failure patterns are well-supported;
5. empirical routing only if enough comparable cross-agent outcomes exist;
6. local dashboard only if users repeatedly need aggregate receipt views.

## Avoid

- pre-building all candidate directions;
- changing product positioning based on one run;
- publishing token/time saving percentages without reliable data;
- turning dogfood into a giant refactor program.

## Completion rule

AW-VFY-010 is Done only when the evidence report supports a concrete go/adjust/stop decision for the next product phase.
