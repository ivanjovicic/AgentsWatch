# AgentsWatch MVP Roadmap

Last aligned: 2026-07-17  
Status: planning/specification

## Strategy

Do not compete with Codex, Cursor, Devin, OpenHands, GitHub agents, or Superplane on agent execution, cloud sandboxes, generic scheduling, visual workflows, or parallel sessions.

Build the missing control/evidence layer:

```text
Roadmap item
  -> bounded run contract
  -> external coding agent
  -> Agent Run Receipt
  -> evidence/drift gate
  -> learning and next route
```

First product:

```text
AgentsWatch CLI — local roadmap-to-verified-change control plane for coding agents.
```

## Gate 0 — prove the current skeleton

Must complete before new runtime features:

1. `AW-VAL-001` restore/build/test validation.
2. `AW-VAL-002` CLI smoke validation.
3. Resolve bootstrap failures.
4. Confirm local-only file behavior.
5. Record first valid run report.

Definition of done:

- solution builds;
- tests pass or have honest blocked evidence;
- `init`, `optimize`, and `status` work in a temporary repo;
- no files are written outside expected local paths.

## Phase 1 — Run spine and Agent Run Receipt

Goal: produce a trustworthy, vendor-neutral receipt for one external agent run.

Priority order:

1. Task/run lifecycle: `start`, `finish`, `status`.
2. Git start/end snapshots.
3. Run contract file.
4. Changed-file and scope evidence.
5. Validation evidence capture.
6. Agent Run Receipt generation.
7. Compact handoff and next prompt.
8. Evidence lint for required receipt fields.

Minimum receipt:

```text
run id
roadmap/prompt id
agent/model/tool
permission mode
run mode
owned/avoid paths
start/end commit
files inspected if available
files changed
validation
claims
missed work
risk
learning note
next prompt
```

Definition of done:

- works with a manually pasted Codex/Cursor/Claude final response;
- works on Flutter and .NET dogfood repos;
- generates a useful receipt without full chat history;
- does not mark runtime work `Done` without validation or blocked reason.

## Phase 2 — Roadmap Contract Compiler

Goal: turn vague roadmap items into safe executable contracts.

Features:

1. `agentswatch roadmap check`.
2. Contract completeness checker.
3. Acceptance-criteria normalizer.
4. Dependency and gate checker.
5. Owned/avoid path resolver with manual confirmation.
6. Permission mode and risk classification.
7. Run-mode and token-budget recommendation.
8. Next minimal prompt generator.

Definition of done:

- incomplete items create investigation/planning prompts;
- implementation prompts require acceptance criteria, validation and stop rules;
- broad items are split into one-mode queue items;
- generated contracts are deterministic enough to lint.

## Phase 3 — Evidence and Drift Gate

Goal: verify actual work instead of trusting the agent summary.

Features:

1. Claims-vs-diff checker.
2. Claims-vs-validation checker.
3. Roadmap acceptance-criteria coverage.
4. Scope Drift Score.
5. Evidence Score with explainable reasons.
6. Dependency violation detection.
7. `Done`, `NeedsEvidence`, `NeedsReview`, `NeedsApproval`, `Blocked` decisions.
8. Roadmap status update from receipt evidence.

Example findings:

```text
Claim: tests added
Actual: no test file changed
Result: NeedsEvidence
```

```text
Owned paths: lib/features/profile/**
Actual change: lib/auth/session_provider.dart
Result: Scope drift
```

Definition of done:

- common claims are checked against file patterns and validation;
- score explanations list supporting and missing evidence;
- no opaque score decides status by itself;
- users can override with an auditable reason.

## Phase 4 — Validation Economy

Goal: reduce repeated commands, broad validation and large logs.

Features:

1. Command Profiler / Fast Validation Advisor.
2. Targeted validation ladder by changed files and adapter.
3. Repeated-command detection.
4. Avoidable command-time estimate.
5. Compact error signatures.
6. Output size/context proxy.
7. Investigation prompt after repeated failure.

Definition of done:

- recommends targeted validation before broad validation;
- avoids full output in receipts;
- records why a broader command is justified;
- learns repo-specific validation rules from accepted evidence.

## Phase 5 — Counterfactual Learning

Goal: turn failed or expensive runs into specific better future behavior.

Features:

1. Learning events from receipts.
2. Mistake pattern recurrence counts.
3. Rule candidates with evidence.
4. Confidence, scope and expiry/deprecation.
5. Counterfactual next prompt.
6. Counterfactual context set.
7. Counterfactual model/tool class.
8. Counterfactual validation ladder.
9. Measure whether applying a rule improved a comparable later run.

Definition of done:

- rules are specific, scoped and reviewable;
- stale or contradicted rules can expire;
- generic advice is rejected;
- future preflight uses only accepted relevant rules.

## Phase 6 — Cross-agent history and empirical router

Goal: recommend the cheapest sufficient route using evidence from this repository.

Features:

1. Normalize runs across manual, Codex, Cursor, Claude, Copilot, Devin and OpenHands sources.
2. Task-type classification.
3. Comparable-run grouping.
4. Quality/evidence outcome tracking.
5. Retry and scope-drift tracking.
6. Provider token/cost import when available.
7. Model/tool route recommendation.
8. Confidence and fallback explanation.

Definition of done:

- recommendation never relies on one run;
- users can inspect supporting runs;
- insufficient evidence returns `unknown`, not a guess;
- routing remains vendor-neutral.

## Phase 7 — Thin integrations

Only after the internal contract and receipt model are stable.

Preferred order:

1. MCP server for preflight, receipt, evidence and route tools.
2. Codex skill/plugin.
3. Cursor background-agent adapter.
4. GitHub check/agent app.
5. Superplane preflight/postflight component.
6. OpenHands and Devin import adapters.

Integration rule:

```text
External product executes. AgentsWatch contracts, verifies and learns.
```

## Phase 8 — Local dashboard

Only after at least 30 dogfood receipts.

Pages:

- roadmap confidence;
- run receipts;
- evidence and drift findings;
- validation economy;
- mistake patterns;
- agent/model comparisons;
- router confidence;
- settings and privacy.

Do not build a visual workflow canvas.

## Later team edition

Only after local cross-agent evidence proves useful.

Possible features:

- shared policy packs;
- signed receipt export;
- PR evidence checks;
- team-level comparable-run analysis;
- role-based approvals;
- optional hosted metadata without source upload.

## Explicitly de-prioritized

- proprietary coding-agent runtime;
- cloud workspaces;
- generic parallel-agent manager;
- generic schedules and automations;
- generic knowledge/playbook library;
- full conversation archive;
- visual workflow builder;
- production deployment orchestration;
- broad incident/infrastructure automation;
- hundreds of integrations;
- automatic merge/release;
- exact token accounting without provider data.

## Dogfood proof plan

Use AgentsWatch and MathLearning.

Collect at least 30 comparable receipts covering:

- Flutter widget change;
- Flutter provider/state change;
- .NET endpoint/service change;
- test-only change;
- docs-only change;
- bug investigation;
- diff-only review.

Track:

- contract completeness;
- scope drift;
- evidence completeness;
- validation duration and breadth;
- retries;
- repeated mistakes;
- accepted learning rules;
- route recommendation accuracy when enough evidence exists.

## Current priority order

1. Gate 0 validation.
2. Task/run lifecycle.
3. Agent Run Receipt.
4. Evidence lint.
5. Roadmap Contract Compiler.
6. Claims/diff/validation gate.
7. Scope Drift and Evidence scores.
8. Validation Economy.
9. Counterfactual Learning.
10. Cross-agent empirical router.
11. Thin integrations.
12. Local dashboard.

See `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`.