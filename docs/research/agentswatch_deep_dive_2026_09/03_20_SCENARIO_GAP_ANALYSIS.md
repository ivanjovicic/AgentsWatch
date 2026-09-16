# 24-Scenario Adversarial Gap Analysis

Full table: `data/scenario_gap_matrix.csv`.

## Summary

The analysis contains 24 scenarios, including all 20 requested cases plus four additional edge cases.

The main conclusion is deliberately narrower than “AgentsWatch catches more bugs”. Generic Git/CI/review already handles many scenarios well enough. AgentsWatch only earns credit where it changes risk, reviewer effort, auditability, completion decisions, reproducibility or cross-vendor consistency.

## Highest-value scenarios

1. **Repository dirty before run** — a deliberate pre-run baseline prevents old dirt from being silently attributed to the run.
2. **Same pre-existing dirty file changed further** — start/end fingerprints can distinguish unchanged pre-existing dirt from changed-further state, while retaining ambiguity when exact causality cannot be proven.
3. **Agent claims tests passed but required validation was incomplete** — a validation contract can block `Done` when required evidence is absent.
4. **Agent changes outside owned/avoid paths** — scope findings can operate on run-interval changes rather than raw final dirty state.
5. **Two agent vendors execute comparable tasks** — one RunContract/RunReceipt schema can normalize evidence across vendors.
6. **Audit without full agent chat** — a compact receipt can preserve repository/evidence facts without storing the entire conversational transcript.
7. **User overrides failed/unknown verification** — an explicit override reason can remain auditable.
8. **Concurrent writer or formatter modifies the workspace** — AgentsWatch can improve safety by returning `Ambiguous` rather than pretending causal certainty.

## Low-differentiation scenarios

Rename/delete visibility, untracked-file visibility, generic “validation failed”, a migration file appearing in the diff, and a final summary omitting a changed file are already visible through Git/CI/review products.

AgentsWatch may normalize these into a receipt, but they are not moat claims.

## Scenario-level rule

A finding counts as incremental value only if the normal baseline — Git + CI + PR review + native vendor evidence + relevant review tooling — would not have surfaced it early or consistently enough.

## Causality boundary

With Git-only start/end evidence, AgentsWatch can often prove:

> repository state changed inside the recorded run interval.

It cannot always prove:

> the selected coding agent caused every byte of that change.

Concurrent humans, background formatters, generators and multiple agents can make causal attribution impossible. Those cases must become `Ambiguous / NeedsReview`.

## Commercial implication

Dirty-worktree handling by itself is not a company. Its value rises only when combined with required evidence, scope/claim checks and a portable completion receipt.
