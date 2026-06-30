# Roadmap-Driven Agent OS

Last aligned: 2026-06-30  
Status: product/architecture plan; docs-only

## Vision

AgentsWatch can become a roadmap-oriented agent operating system.

The user writes or imports a roadmap. AgentsWatch turns that roadmap into epics, acceptance criteria, prompt queues, validation plans, execution evidence, and self-correction loops.

Core idea:

```text
Roadmap is the source of truth. Agents execute only through scoped, validated, evidence-producing prompts.
```

## Product promise

```text
Turn a roadmap into safe agent execution without losing control of scope, cost, or evidence.
```

AgentsWatch should not replace coding agents. It should supervise them by deciding:

- what should be done next;
- what should not be done yet;
- which prompt should be generated;
- which model/tool is appropriate;
- which files are owned or avoided;
- which validation command proves the work;
- whether the run should stop, split, retry, or escalate.

## Roadmap-to-execution pipeline

```text
Roadmap
  -> Epics
  -> Milestones
  -> Work items
  -> Prompt queue
  -> Agent run
  -> Validation evidence
  -> Run report
  -> Handoff
  -> Self-review
  -> Roadmap status update
```

Each stage must produce compact evidence, not long chat history.

## Roadmap file shape

MVP should support markdown first:

```text
.ai/roadmap/ROADMAP.md
.ai/roadmap/EPICS.md
.ai/roadmap/MILESTONES.md
.ai/roadmap/DECISIONS.md
.ai/roadmap/RISKS.md
```

Optional JSON later:

```text
.agentwatch/roadmap.json
```

## Roadmap item contract

Every roadmap item should have:

```text
ID:
Title:
Why:
Status:
Priority:
Gate:
Owner:
Run mode:
Token budget:
Acceptance criteria:
Owned paths:
Avoid paths:
Validation:
Risks:
Next prompt:
```

If these fields are missing, AgentsWatch should generate an investigation/planning prompt, not implementation.

## Execution rules

Roadmap-driven execution must follow these rules:

- do not implement directly from vague roadmap text;
- generate an investigation or planning prompt first when scope is uncertain;
- split broad roadmap items into one-mode prompts;
- never skip Gate 0 or validation gates;
- never run dashboard/SaaS/cloud work before roadmap gates allow it;
- never claim completion without validation evidence or blocked reason;
- never paste full logs into prompts or reports;
- use command profile summaries instead of terminal log dumps.

## Agent self-supervision loop

After each run, AgentsWatch should ask:

```text
Did the prompt stay in scope?
Were only owned files changed?
Was validation run or honestly blocked?
Were tests missed?
Did the agent inspect too many files?
Did the agent run broad commands unnecessarily?
Should the next step be retry, split, review, or stop?
```

The answer updates:

- run report;
- handoff summary;
- roadmap status;
- risk register;
- next prompt.

## Model/tool selection

AgentsWatch should recommend model/tool classes, not hard-code vendors.

| Work type | Recommended capability | Budget |
|---|---|---|
| Roadmap parsing | strong reasoning, broad summarization | medium |
| Prompt splitting | strong instruction following | low/medium |
| One-file implementation | fast coding model | low |
| Multi-file refactor | strong coding + reasoning | medium/high |
| Debug/root cause | strong reasoning + code search | medium |
| Diff-only review | precise code reviewer | low |
| Validation/log triage | compact summarizer | low |
| Architecture planning | strongest reasoning model | high |

Selection rule:

```text
Use the cheapest model that can satisfy the risk level and evidence requirement.
```

Escalate only when:

- root cause is unknown after scoped investigation;
- code crosses multiple subsystems;
- security/auth/data-loss risk appears;
- validation fails twice for non-obvious reasons;
- roadmap item has ambiguous acceptance criteria.

## Safety gates

Roadmap-driven automation must stop for human review when:

- auth/security/secrets are involved;
- migrations or data-loss risks are involved;
- billing/SaaS/cloud/network behavior is involved;
- public API contracts change;
- generated prompts would exceed context budget;
- an agent wants to inspect whole repo;
- an agent wants to execute full suite/CI without justification.

## MVP scope

MVP should be docs/CLI-first:

1. Roadmap import/check command design.
2. Roadmap item linting.
3. Roadmap-to-epic prompt generation.
4. Epic-to-prompt queue generation.
5. Next prompt selector.
6. Run report updates roadmap item status.
7. Self-review prompt after each run.

Do not start with autonomous background execution.
Do not start with SaaS.
Do not start with deep IDE integration.

## Proposed commands

```bash
agentswatch roadmap check
agentswatch roadmap split <roadmap-file>
agentswatch roadmap next
agentswatch roadmap prompt <item-id>
agentswatch roadmap status
agentswatch roadmap review
```

All commands should be local-first and markdown-first.

## Prompt set

Use these prompt files:

```text
docs/prompts/ROAD-001-roadmap-to-execution-plan.md
docs/prompts/ROAD-002-roadmap-item-to-prompt-queue.md
docs/prompts/ROAD-003-roadmap-run-self-review.md
```

## Fit with AW-011

AW-011 command profiling becomes one feedback signal for roadmap execution:

- if full validation is slow, recommend targeted validation next time;
- if a command fails repeatedly, create an investigation prompt;
- if logs are large, summarize locally before agent review;
- if command strings contain secret-looking values, redact or refuse before storing.

## Privacy/storage rule

Roadmap data, prompt queues, command profiles, and run reports are local-first.

If command profiling stores command text, it must store a redacted display command and either redact or refuse secret-looking raw command strings before persistence.

Command history JSONL belongs to the JSON sidecar phase, not markdown-only Phase 1.