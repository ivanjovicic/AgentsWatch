# AgentsWatch

AgentsWatch is a local-first, vendor-neutral **run-evidence and completion-verification layer for delegated coding-agent work**.

External agents such as Codex, Claude Code, Cursor, Copilot, Devin and OpenHands execute coding work. AgentsWatch does not replace them and does not try to become another generic AI code reviewer.

It normalizes verification intent, records repository state before and after a delegated run, checks required validation/scope/completion evidence, and produces a portable RunReceipt.

## Core promise

```text
Turn delegated coding work into independently verifiable evidence.
```

Supporting principle:

```text
Trust evidence, not the executor's confidence.
```

## Product loop

```text
existing task / issue / prompt
  -> verification RunContract
  -> pre-run repository baseline
  -> external coding agent
  -> run-interval repository delta
  -> validation evidence
  -> deterministic scope/claim/completion checks
  -> portable RunReceipt
  -> Done / NeedsEvidence / NeedsReview / NeedsApproval / Blocked / Failed
```

Important:

> A change observed during the recorded run interval is not automatically proven to be authored by the selected agent.

If humans, hooks, formatters, generators or another process may have written concurrently, AgentsWatch preserves `Ambiguous` / `NeedsReview` rather than inventing causal attribution.

## What AgentsWatch is not

AgentsWatch is not another:
- coding-agent runtime;
- generic AI code-review engine;
- cloud sandbox;
- multi-agent session manager;
- generic scheduler/orchestrator;
- token/cost dashboard;
- AI-code tracking dashboard;
- full chat/session archive;
- CI/CD or release orchestrator.

## Competitive boundary

As of September 2026, Cursor, GitHub, Qodo and adjacent vendors already expose increasingly strong AI-code tracking, session/audit, review and policy/governance capabilities.

Therefore the defensible hypothesis is **not** “better tracking” or “independent review” in the generic sense.

AgentsWatch must prove that a **cross-vendor run-evidence and completion-verification layer** produces decision-changing evidence that Git + CI + PR review + native vendor/Qodo workflows do not surface consistently enough.

Latest accepted guardrails:
`docs/DEEP_DIVE_DECISIONS_2026_09_16.md`

Latest deep-dive research:
`docs/research/agentswatch_deep_dive_2026_09/`

Deep-dive verdict: **CONTINUE, BUT NARROW WEDGE**. Current analytical score: **7.80/10**. This is a decision model, not product proof.

## MVP wedge

```text
Task/import -> verification Contract -> Agent -> interval evidence -> compact Verified Receipt
```

MVP capabilities:
1. local workspace initialization;
2. `RunContract v1` verification normalization/lint;
3. pre-run baseline with dirty-worktree evidence;
4. finish run-interval delta with explicit ambiguity;
5. compact `RunReceipt v1` JSON + Markdown projections;
6. validation evidence/provenance;
7. deterministic evidence gate;
8. scope drift checks;
9. narrow claims-vs-diff checks;
10. .NET + Flutter + universal Git behavior;
11. adversarial 30-run dogfood.

## Current runtime

Implemented and integrated in the CLI:

```bash
agentswatch init
agentswatch optimize "<prompt text or path>"
agentswatch status
agentswatch contract create <task-id> --intent <text> --accept <text> --validate <command> ...
agentswatch contract check <contract-id-or-json-file>
agentswatch start <contract-id>
agentswatch finish <run-id>
agentswatch receipt show <run-id>
agentswatch handoff <run-id>
agentswatch evidence record <run-id> --command <text> --status <status> --source <source>
agentswatch evidence check <run-id>
agentswatch drift check <run-id>
agentswatch claims record <run-id> --type <type> --value true --source <source>
agentswatch claims check <run-id>
```

Gate 0 is closed. On November 6, 2026, the main branch reached AW-VFY-009
(Contract, baseline, interval delta, receipt, Evidence Gate, Scope Drift, structured Claims).
The prior parser failure and earlier skeleton-only descriptions below this date are obsolete.
The GitHub CI on `ab3ebe62f1a23fd19a87caff98f8a267e598920c` completed successfully;
this does **not** establish dogfood, independent provenance or commercial readiness.

**Important remaining boundary:** validation evidence can be manually imported with a
declared source, and semantic acceptance criteria are not autonomously verified. The
current receipt hash is an integrity checksum, not a signed or trusted attestation.
Treat any `Done` verdict as bounded to the configured deterministic checks; it is
not proof that the agent independently executed every claimed action.

Next required gate: `AW-VFY-010`, 30 real/adversarially reviewed receipts, followed by
external user-value validation (011) and commercial validation (012). Do not start
hosted infrastructure or a dashboard as a substitute for these gates.

## Canonical artifacts

Machine-readable verification truth:

```text
.agentwatch/
  contracts/<contract-id>.json
  active-runs/<run-id>.json
  runs/<run-id>.json
```

Human projections:

```text
.ai/
  runs/<run-id>.md
  handoffs/<run-id>.md
```

Rule:

```text
JSON = verification source of truth
Markdown = human-readable projection / handoff
```

`learningNote`, `nextPrompt`, routing and optimization advice may appear later in handoff/learning outputs, but are **not mandatory canonical RunReceipt evidence**.

## Architecture direction

AgentsWatch remains a local-first modular monolith:

```text
CLI / future GitHub Check / MCP
      |
Application use cases
      |
Contract | Run evidence | Verification | optional later Learning
      |
Domain models
      |
Git | local storage | validation adapters
```

Initial stack coverage:
- universal Git behavior;
- .NET;
- Flutter.

Broader vendor/language integrations come only after verification and external-value proof.

## Dogfood gate

Before product expansion, collect at least 30 adversarial real receipts and measure:
- contract completeness;
- interval evidence correctness/ambiguity;
- unsupported claims;
- scope findings;
- missing evidence;
- false positives;
- material false attribution;
- verification overhead vs reviewer time saved;
- whether findings change decisions.

Success must include real catches and no observed material false attribution in the tested corpus.

## External-value gate

Before advanced learning, broad integrations, dashboard or team/SaaS packaging:
- >=10 real external evaluations;
- >=2 agent ecosystems represented where practical;
- >=3 external users/teams request continued use;
- >=3 real cases where the receipt changes a review/rework/merge/evidence decision;
- clear value beyond Git + CI + PR review + native vendor/Qodo alternatives.

First commercial ICP hypothesis:
**AI-heavy teams of roughly 5–30 developers**.

## Commercial gate

Before auth/billing/team administration:
- >=3 paid-pilot/design-partner/equivalent concrete commitments;
- payer/budget owner identified;
- payment tied to verification/review/governance outcomes.

## Packaging hypothesis

Leading hypothesis after value proof: **open core**.

The deterministic local verifier should be inspectable. Potential paid value belongs in organization policies, managed evidence, cross-repo controls, compliance/team features and support — but only after commercial validation.

## Product principles

- Evidence before autonomy.
- Run-interval evidence before causal claims.
- Unknown/Ambiguous before fabricated certainty.
- Deterministic verification before LLM interpretation.
- Verification normalization before task-management duplication.
- Compact receipt before session archive.
- External value before product breadth.
- Commercial proof before SaaS packaging.
- Local-first/no telemetry by default.

## Canonical strategy documents

Read these first:

1. `README.md`
2. `docs/PRODUCT_SPEC.md`
3. `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`
4. `docs/MVP_ROADMAP.md`
5. `docs/ARCHITECTURE.md`
6. `docs/DATA_MODEL.md`
7. `docs/COMMAND_CONTRACTS.md`
8. `docs/research/agentswatch_deep_dive_2026_09/00_EXECUTIVE_DECISION.md`
9. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
10. `docs/prompt_queues/verification_mvp_2026_08_25.md`
