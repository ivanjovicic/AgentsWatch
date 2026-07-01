# AgentsWatch Mistake Learning Spec

Last aligned: 2026-06-30  
Status: planning/specification  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/AGENTWATCH_MISTAKE_LEARNING_SPEC.md`

## Purpose

AgentsWatch should not only record agent runs. It should help agents and developers learn from repeated mistakes.

Logs alone are not learning. The product needs a closed loop:

```text
mistake -> classification -> prevention rule/prompt/test/lint -> verification that it did not repeat
```

This spec turns the MathLearning run-log and mistake-ledger dogfood practice into a reusable AgentsWatch product capability.

## Core promise

```text
Turn AI-agent mistakes into reusable prevention rules.
```

The first implementation must stay local-first and file-based. No cloud, telemetry, SaaS, or source-code upload is required.

## Learning loop model

AgentsWatch mistake learning has five layers:

1. `.ai/runs/<run>.md` — what happened in one run.
2. `.ai/learning/MISTAKE_LEDGER.md` or `docs/ai/learning/MISTAKE_LEDGER.md` — compact memory of known mistake patterns.
3. `.ai/learning/<date>-mistake-rollup.md` — periodic rollup of the last N runs.
4. Playbooks / prompt queues / templates — prevention rules that change future behavior.
5. Validation/lint checks — mechanical gates that block known evidence mistakes.

A run is not learning-complete until every observed mistake is classified as:

- new mistake with a mistake card;
- repeated mistake with a rule/prompt/test/lint update;
- false alarm with explanation.

## Required local files

`agentswatch init` should eventually be able to create or offer these files:

```text
.ai/
  RUN_LOG_TEMPLATE.md
  learning/
    MISTAKE_LEDGER.md
    MISTAKE_CARD_TEMPLATE.md
    ROLLUP_TEMPLATE.md
  prompts/
    AGENT_MISTAKE_ROLLUP_PROMPT.md
```

If a repository already has `docs/ai/learning/*`, AgentsWatch should detect it and avoid duplicating the system unless the user asks to migrate or link it.

## Mistake ledger

The ledger is the compact memory of repeated agent mistakes.

Each mistake card should contain:

```text
ID:
Title:
Severity: P0 | P1 | P2
Status: Open | Mitigated | Watching | Retired | False alarm
First seen:
Repeated in:
Root cause:
Impact:
Prevention rule:
Fixed by:
Next check:
```

Example categories:

- `MISTAKE-EVIDENCE-*` — missing or misleading run evidence.
- `MISTAKE-SCOPE-*` — prompt scope creep or unrelated files edited.
- `MISTAKE-VALIDATION-*` — claimed validation without command evidence.
- `MISTAKE-CLAIM-*` — final claim contradicts changed files.
- `MISTAKE-CONTEXT-*` — repeated broad reads or ignored handoff.
- `MISTAKE-QUEUE-*` — queue/router status drift.

## Run log mistake card

Every non-trivial run log should include:

```text
Relevant prior mistakes read:
How this run avoids prior mistakes:

## Mistakes observed

- Mistake ID:
- New or repeated:
- Root cause:
- Prevention added:
- Existing rule that should have prevented it:
- Did this run update a rule/prompt/test/lint/queue:
```

If no mistakes were observed:

```text
Mistakes observed: none
```

## Repeated mistake rule

If the same mistake category appears twice, AgentsWatch should require one prevention action before the run can be marked learning-complete:

- update a playbook/rule;
- update a prompt template;
- update a prompt queue;
- add a targeted test;
- add or update a lint/validation check;
- document why no new prevention action is needed.

Recording the mistake without a prevention action is not enough.

## Rollup workflow

AgentsWatch should support a rollup workflow after either:

- the last 5 meaningful run logs; or
- 3 repeated mistake categories; or
- one high-severity evidence/release mistake.

Rollup output:

```text
Runs reviewed:
Repeated mistakes:
New mistake cards added:
Rules/prompts/tests/lints updated:
Still-open risks:
Next prevention prompt:
```

The rollup should prefer the compact mistake ledger over reading every historical run in future sessions.

## CLI command candidates

Do not add all of these to v1 skeleton at once. Add them by phase.

Phase 1 / early CLI:

```bash
agentswatch mistakes list
agentswatch mistakes add
agentswatch mistakes check
```

Phase 2 / trust layer:

```bash
agentswatch rollup mistakes --last 5
agentswatch lint evidence
agentswatch lint learning
```

Later:

```bash
agentswatch mistakes suggest-rules
agentswatch mistakes export
```

## Evidence lint checks

A future `agentswatch lint evidence` should flag:

- Done queue row without run-log path;
- run-log path referenced but missing;
- model/client metadata blank instead of `unknown-not-exposed`;
- elapsed time blank instead of `unknown-not-recorded`;
- validation claimed but no command/evidence recorded;
- residual risk says target evidence missing while completion is 100%;
- mistake observed but not classified;
- repeated mistake without prevention update or documented no-op.

## Privacy and safety

Mistake learning is local-first.

Do not upload:

- source code;
- prompts;
- diffs;
- validation output;
- mistake ledger;
- run logs.

Future SaaS/team features must explicitly separate source-containing artifacts from safe aggregate metadata.

## What not to build yet

Do not build these before local file-based learning works:

- cloud mistake database;
- team leaderboard of mistakes;
- automatic agent blocking by remote policy;
- model-specific token analytics;
- IDE extension;
- automatic code editing.

## MVP definition for mistake learning

The mistake-learning slice is useful when AgentsWatch can:

1. create mistake ledger/template files;
2. make run logs include relevant prior mistakes and observed mistakes;
3. list known mistakes;
4. check that a run log classified mistakes;
5. produce a rollup from recent run logs;
6. flag at least three evidence-quality mistakes mechanically.

This should be added after the core CLI spine exists, but before dashboard/SaaS work.
