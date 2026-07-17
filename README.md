# AgentsWatch

AgentsWatch is a local-first, vendor-neutral control and evidence plane for AI coding agents.

It sits above Codex, Cursor, Claude Code, Copilot, Devin, OpenHands, and similar tools. External agents execute coding work; AgentsWatch converts roadmap intent into bounded run contracts, verifies what actually changed, and learns which execution path works best for the repository.

## Core promise

```text
Turn roadmap intent into verified change — across any coding agent.
```

Supporting promise:

```text
Spend fewer tokens. Prove every change. Do not repeat avoidable mistakes.
```

Token and cost savings are product targets to measure through dogfood evidence, not published claims yet.

## Differentiated product loop

```text
Roadmap item or prompt
  -> bounded run contract
  -> external coding agent
  -> Agent Run Receipt
  -> claims/diff/validation evidence gate
  -> scope and roadmap drift result
  -> learning and next route
```

The first product is not another coding agent, cloud sandbox, scheduler, or visual workflow engine.

See:

- `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`
- `docs/PRODUCT_SPEC.md`
- `docs/MVP_ROADMAP.md`
- `docs/prompt_queues/agentwatch_differentiation.md`

## Current runtime

Implemented commands:

```bash
agentswatch init
agentswatch optimize <prompt-file-or-text>
agentswatch status
```

The repository is still in planning/skeleton stage. Runtime work must remain validation-first.

## Planned differentiated commands

Run contract and receipt:

```bash
agentswatch start <task-id>
agentswatch finish <task-id>
agentswatch contract check <file>
agentswatch contract build <roadmap-item-or-prompt>
agentswatch receipt create <run-id>
agentswatch receipt check <run-id>
```

Evidence and drift:

```bash
agentswatch evidence check <run-id>
agentswatch drift check <run-id>
agentswatch handoff <run-id>
```

Validation economy and learning:

```bash
agentswatch validate --suggest
agentswatch validate --profile
agentswatch mistakes list
agentswatch mistakes check <run-log>
agentswatch rollup mistakes --last 5
```

Roadmap and routing later:

```bash
agentswatch roadmap check
agentswatch roadmap next
agentswatch roadmap review
agentswatch route suggest
```

## Signature capabilities

### Roadmap Contract Compiler

Turns vague roadmap items into machine-checkable intent, acceptance criteria, dependencies, owned paths, avoid paths, permission mode, validation and stop rules.

### Agent Run Receipt

Produces a compact vendor-neutral record of what an agent was asked to do, what it changed, what it validated, what it claimed, what was missed and what should happen next.

### Evidence and Drift Gate

Compares:

```text
roadmap intent
vs acceptance criteria
vs agent claims
vs actual diff
vs validation evidence
```

### Counterfactual Learning

Proposes the smaller prompt, narrower context, cheaper route and validation sequence that should have been used after failed, expensive or drifting runs.

### Project-Local Empirical Router

Later recommends the cheapest sufficient model/tool using comparable outcomes from this repository, with confidence, reasons and an `unknown` result when evidence is insufficient.

## Post-prompt logging rule

Every agent run should leave compact evidence and one learning note.

See:

- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`
- `docs/MISTAKE_LEARNING_SPEC.md`
- `docs/CLI_LEARNING_ADDENDUM.md`
- `docs/prompts/LOG-001-post-prompt-run-log.md`
- `docs/prompts/LOG-002-mistake-pattern-review.md`
- `docs/prompts/LOG-003-flutter-agent-run-review.md`

## Supervised autopilot rule

AgentsWatch may sequence prompts, but should not run uncontrolled continuous autopilot in MVP.

External tools execute. AgentsWatch contracts, verifies and learns.

See:

- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- `docs/prompts/AUTO-001-design-supervised-autopilot-queue.md`
- `docs/prompts/AUTO-002-generate-tool-prompt-envelope.md`
- `docs/prompts/AUTO-003-review-queued-agent-run.md`
- `docs/prompts/AUTO-004-manual-assisted-queue-runbook.md`

## Agent safety rule

Agents may suggest risky actions, but must not execute them without an explicit approval gate.

See:

- `docs/AGENT_RISK_BOUNDARIES.md`
- `docs/AGENT_PERMISSION_MODEL.md`
- `docs/prompts/SEC-001-agent-risk-boundary-audit.md`

## Bootstrap warning

The next runtime work must be:

1. run `AW-VAL-001` build validation;
2. run `AW-VAL-002` CLI smoke validation;
3. review validation evidence;
4. only then implement the Agent Run Receipt spine.

See:

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/prompt_queues/bootstrap_validation.md`

## Repository layout

```text
src/
  AgentsWatch.Cli/
  AgentsWatch.Core/
  AgentsWatch.Git/
  AgentsWatch.LanguageAdapters/
  AgentsWatch.Reports/
tests/
  AgentsWatch.Tests/
docs/
.ai/templates/
```

## Development principles

- Local-first CLI before dashboard or SaaS.
- Evidence before autonomy.
- Cross-vendor contracts before deep vendor integration.
- External agents execute; AgentsWatch supervises and verifies.
- Git, markdown and file-system evidence before cloud services.
- Compact receipts instead of full chat history.
- Compact command profiles instead of full terminal logs.
- Deterministic findings before opaque scores.
- Explainable routing before automatic routing.
- One learning note after every agent run.
- Risky actions require explicit approval gates.
- No dashboard until at least 30 useful dogfood receipts exist.

## De-prioritized

AgentsWatch should not initially build:

- proprietary coding-agent runtime;
- cloud workspaces;
- generic background-agent manager;
- generic schedules or playbooks;
- visual workflow canvas;
- full session archive;
- CI/CD, incident or production orchestration;
- automatic merge/release;
- integration marketplace;
- exact token accounting without provider data.

## Commercial trial rule — post-MVP

AgentsWatch may later offer a permanent free tier plus a time-limited or usage-limited Pro trial.

License checks must not upload repository source code, prompts, diffs, validation output, receipts, command logs or learning history.

See:

- `docs/TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- `docs/prompt_queues/agentwatch_trial_licensing.md`

## Current strategy documents

- `docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md`
- `docs/PRODUCT_SPEC.md`
- `docs/MVP_ROADMAP.md`
- `docs/FEATURE_PORTFOLIO_REVIEW_2026_06_30.md`
- `docs/FEATURE_SELECTION_SPEC.md`
- `docs/ROADMAP_DRIVEN_AGENT_OS.md`
- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`
- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- `docs/prompt_queues/agentwatch_differentiation.md`
