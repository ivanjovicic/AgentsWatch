# AgentsWatch

AgentsWatch is a local-first, vendor-neutral verification and evidence layer for AI coding agents.

External agents such as Codex, Claude Code, Cursor, Copilot, Devin, OpenHands, and similar tools execute coding work. AgentsWatch does not replace them. It turns roadmap intent into a machine-checkable run contract, records what actually changed, verifies claims against git and validation evidence, and produces a vendor-neutral run receipt.

## Core promise

```text
Turn roadmap intent into verified change — across any coding agent.
```

Supporting promise:

```text
Trust the diff, not the agent summary.
```

Token, time, and cost efficiency remain useful secondary metrics. They are not the primary product wedge and no savings percentage should be published without dogfood evidence.

## Product loop

```text
Roadmap item / issue / prompt
  -> Run Contract
  -> external coding agent
  -> start/end repository evidence
  -> Agent Run Receipt
  -> claims/diff/validation gate
  -> scope and acceptance-criteria findings
  -> Done / NeedsEvidence / NeedsReview / NeedsApproval / Blocked
  -> learning and next route
```

## What AgentsWatch is not

AgentsWatch is not another:

- coding-agent runtime;
- cloud sandbox;
- multi-agent session manager;
- generic scheduler;
- visual workflow engine;
- generic token/cost dashboard;
- full chat/session archive;
- CI/CD or release orchestrator.

Those capabilities are increasingly provided by agent vendors and engineering platforms. AgentsWatch should integrate with them and specialize in independent, reviewable verification.

## MVP wedge

The first credible product is deliberately narrow:

```text
Task -> Contract -> Agent -> Verified Receipt
```

MVP capabilities:

1. local workspace initialization;
2. canonical machine-readable `RunContract v1`;
3. run start baseline with pre-existing dirty-worktree attribution;
4. run finish delta;
5. canonical `RunReceipt v1` plus Markdown projection;
6. validation evidence capture;
7. claims-vs-diff checks;
8. scope drift checks;
9. explainable evidence findings and status;
10. compact handoff and one learning note.

## Current runtime

Implemented today:

```bash
agentswatch init
agentswatch optimize <prompt-file-or-text>
agentswatch status
```

The current repository is still a skeleton/prototype. The core contract -> run -> receipt -> verification spine is not implemented yet.

The latest known GitHub CI evidence shows:

- restore: pass;
- build: pass;
- tests: fail in `GitStatusParserTests` because the current parser trims the fixed-width git porcelain prefix before slicing the path.

The first implementation prompt must fix and harden git status parsing, rerun the complete build/test gate, then run CLI smoke validation before feature expansion.

See:

- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/prompt_queues/verification_mvp_2026_08_25.md`

## Canonical local artifacts

Machine-readable data is canonical from MVP start:

```text
.agentwatch/
  contracts/<contract-id>.json
  runs/<run-id>.json
```

Human-readable projections and handoffs remain git-friendly Markdown:

```text
.ai/
  runs/<run-id>.md
  handoffs/<run-id>.md
```

Rule:

```text
JSON = source of truth
Markdown = human-readable projection
```

Do not require downstream verification logic to parse free-form Markdown.

## Architecture direction

AgentsWatch remains a local-first modular monolith:

```text
CLI / future MCP
      |
Application use cases
      |
Contract | Run | Evidence | Learning
      |
Domain models
      |
Git | Local storage | Validation/stack adapters
```

Current projects remain useful boundaries:

```text
src/
  AgentsWatch.Cli/
  AgentsWatch.Core/
  AgentsWatch.Git/
  AgentsWatch.LanguageAdapters/
  AgentsWatch.Reports/
tests/
  AgentsWatch.Tests/
```

After Gate 0, application use cases and ports should be introduced before feature growth so CLI logic does not accumulate in `Program.cs`.

## MVP integration scope

Start with:

- universal git behavior;
- .NET adapter;
- Flutter adapter.

React/TypeScript, Node, Python, MCP, GitHub checks, and vendor-specific adapters come after the verification spine works in dogfood.

## Dogfood gate

Use AgentsWatch on AgentsWatch itself and at least one real application repository.

Before building a dashboard or sophisticated empirical router, collect at least 30 useful receipts across comparable task types and measure:

- contract completeness;
- attributable changed files;
- scope drift;
- evidence completeness;
- validation breadth/duration;
- retries;
- repeated mistakes;
- acceptance/rejection of agent claims.

## Product principles

- Verification before observability breadth.
- Evidence before autonomy.
- Cross-vendor contracts before deep vendor integration.
- Canonical structured data before derived reports.
- Git attribution before scope scoring.
- Deterministic findings before LLM interpretation.
- Local-first and no telemetry by default.
- Compact evidence instead of full session capture.
- Explainable status decisions; no opaque score may decide completion alone.
- Risky actions require explicit approval gates.
- No dashboard until receipt dogfood proves what should be visualized.

## De-prioritized

Do not prioritize before the verification MVP is proven:

- proprietary coding-agent execution;
- cloud workspaces;
- generic parallel-agent management;
- generic schedules/playbooks;
- visual workflow canvas;
- full conversation history;
- generic token dashboard as the core product;
- automatic merge/release;
- SaaS/billing/team administration;
- complex model routing without comparable local evidence;
- large integration marketplace.

## Canonical strategy documents

Read these first:

1. `README.md`
2. `docs/PRODUCT_SPEC.md`
3. `docs/MVP_ROADMAP.md`
4. `docs/ARCHITECTURE.md`
5. `docs/DATA_MODEL.md`
6. `docs/COMMAND_CONTRACTS.md`
7. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
8. `docs/prompt_queues/verification_mvp_2026_08_25.md`

Historical token-economy, productization, and older queue documents remain useful research/context, but they must not override the current verification-first roadmap.
