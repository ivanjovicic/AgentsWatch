# Roadmap-Driven Agent OS

Last aligned: 2026-07-17  
Status: product/architecture plan; docs-only

## Vision

AgentsWatch is a roadmap-oriented control and evidence layer for external coding agents.

The user writes or imports a roadmap. AgentsWatch does not directly become the coding agent. It compiles each roadmap item into a bounded run contract, routes it to an external agent, verifies the result, and updates roadmap status from evidence.

Core idea:

```text
Roadmap is the source of intent.
Agent Run Receipts are the source of execution truth.
```

## Product promise

```text
Turn roadmap intent into verified change without losing control of scope, cost, risk, or evidence.
```

## Differentiated pipeline

```text
Roadmap item
  -> Contract Completeness Check
  -> Bounded Run Contract
  -> Model/tool route recommendation
  -> External coding agent
  -> Agent Run Receipt
  -> Claims/diff/validation gate
  -> Scope and roadmap drift findings
  -> Counterfactual learning
  -> Roadmap status update
  -> Next route
```

Every stage must produce compact, local evidence rather than depend on full chat history.

## Roadmap storage

Markdown first:

```text
.ai/roadmap/ROADMAP.md
.ai/roadmap/EPICS.md
.ai/roadmap/MILESTONES.md
.ai/roadmap/DECISIONS.md
.ai/roadmap/RISKS.md
.ai/queue/AUTOPILOT_QUEUE.md
```

Machine-readable sidecars later:

```text
.agentwatch/roadmap.json
.agentwatch/contracts/<contract-id>.json
.agentwatch/runs/<run-id>.json
```

## Roadmap item contract

Every executable item must include:

```text
ID
Title
Why / intent
Status
Priority
Dependencies
Gate
Owner
Permission mode
Run mode
Token budget
Acceptance criteria
Owned paths
Avoid paths
Validation contract
Risk rules
Stop rules
Expected evidence
```

If required fields are missing, AgentsWatch generates an investigation/planning prompt, not an implementation prompt.

## Contract completeness

The contract checker should answer:

- Is the requested outcome measurable?
- Are dependencies and gates known?
- Are owned and avoid paths explicit enough?
- Is the permission mode appropriate?
- Is validation named?
- Is the expected evidence sufficient to prove completion?
- Does the item combine multiple run modes?
- Does the item require a human product or risk decision?

Output:

```text
Complete
NeedsPlanning
NeedsApproval
Blocked
```

## Agent Run Receipt

Each run must produce a vendor-neutral receipt containing:

```text
roadmap/contract id
prompt id
agent/model/tool
permission mode
run mode
start/end git evidence
files inspected when available
files changed
commands and compact profiles
validation evidence
agent claims
missed work
risk findings
scope drift
learning note
next prompt
```

The receipt must remain useful without access to the original agent session.

## Evidence and drift gate

After a run, compare:

```text
roadmap intent
vs acceptance criteria
vs agent claims
vs changed files
vs validation evidence
```

Detect:

- claimed work without supporting files;
- required deliverables missing;
- changed files outside owned paths;
- skipped dependency or approval gates;
- broad validation unrelated to the diff;
- roadmap items marked Done without evidence.

The score summarizes findings but never replaces them.

## Roadmap status rules

Suggested statuses:

```text
Planned
Ready
Running
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
Done
Skipped
```

Only receipt evidence may move runtime work to `Done`.

An agent's final statement alone is not sufficient.

## Model/tool route recommendation

AgentsWatch recommends capability classes first:

| Work type | Initial route |
|---|---|
| Roadmap planning | strong reasoning |
| One-file implementation | fast coding model |
| Multi-file refactor | strong coding + reasoning |
| Root-cause investigation | reasoning + code search |
| Diff review | precise reviewer |
| Validation triage | compact low-cost summarizer |
| Security boundary review | cautious read-only reviewer |

Initial rule:

```text
Use the cheapest route that satisfies the risk and evidence requirements.
```

Later, replace generic routing with project-local empirical evidence from comparable receipts.

If evidence is insufficient, return `unknown` and a safe fallback.

## Counterfactual learning

After failed, expensive, or drifting runs, answer:

```text
What smaller prompt should have been used?
What files were actually needed?
What model/tool class may have been sufficient?
What validation sequence should have been used?
What specific mistake should not be repeated?
```

Accepted rules must have:

- evidence count;
- repository/task scope;
- confidence;
- created/last-used run;
- expiry or deprecation behavior;
- human acceptance for high-impact rules.

Do not create permanent rules from one ambiguous run.

## Validation economy

Roadmap execution should use command-profile evidence to:

- recommend targeted validation before broad suites;
- detect repeated commands;
- estimate avoidable command time;
- compact error output;
- create an investigation prompt after repeated failure;
- prevent full logs from entering future prompts.

## Safety gates

Stop for human review when:

- auth, security or secrets are involved;
- migrations or data-loss risk are involved;
- billing, cloud, deployment or production behavior changes;
- public API contracts change;
- owned paths cannot be determined;
- context budget would require a whole-repo scan;
- an agent requests destructive commands or autonomous merge/release.

## Supervised execution levels

```text
Level 0 — generate next prompt only
Level 1 — generate prompt, route suggestion and evidence checklist
Level 2 — use an official opt-in connector
Level 3 — continuous autopilot
```

Level 3 remains blocked until contracts, receipts, evidence gates, risk rules, rollback and stop behavior are proven.

## Integration principle

```text
External product executes.
AgentsWatch contracts, verifies and learns.
```

Preferred integration order:

1. MCP tools.
2. Codex skill/plugin.
3. Cursor preflight/postflight adapter.
4. GitHub check or agent app.
5. Superplane preflight/postflight component.
6. Devin and OpenHands receipt import.

Do not build another coding-agent runtime, cloud sandbox, visual workflow canvas, generic scheduler, or full-session archive.

## MVP commands

```bash
agentswatch roadmap check
agentswatch contract check <file>
agentswatch contract build <roadmap-item-or-prompt>
agentswatch start <task-id>
agentswatch finish <task-id>
agentswatch receipt create <run-id>
agentswatch receipt check <run-id>
agentswatch evidence check <run-id>
agentswatch drift check <run-id>
agentswatch roadmap review
agentswatch roadmap next
```

## Proof requirement

Before adding a dashboard or empirical router, collect at least 30 useful comparable receipts from AgentsWatch and MathLearning dogfood runs.

Measure:

- contract completeness;
- scope drift;
- evidence completeness;
- retries;
- validation breadth/duration;
- repeated mistakes;
- whether accepted learning rules improve later comparable runs.

See:

- `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`
- `docs/PRODUCT_SPEC.md`
- `docs/MVP_ROADMAP.md`
- `docs/prompt_queues/agentwatch_differentiation.md`
