# AgentsWatch Discovery and Self-Improvement Loop

Last aligned: 2026-07-03  
Status: mandatory product/workflow contract; runtime implementation gated by Gate 0

## Purpose

AgentsWatch must preserve useful findings that appear during a task even when they are outside that task's owned scope.

The system must not silently lose:

- bugs or risks noticed outside the current task;
- stale or contradictory documentation;
- missing tests or validation gaps;
- prompt, queue, router, context-pack, or policy gaps;
- repeated agent waste or failure patterns;
- architecture or product follow-ups that are useful but unsafe to implement now.

Core rule:

```text
Do not fix unrelated work inside the current task.
Do not lose it either.
Capture -> deduplicate -> classify -> route -> generate follow-up -> verify closure.
```

## Current gap this contract closes

AgentsWatch already records `missed work`, `mistakes`, `waste`, `learning notes`, and `next prompt`. Those fields are useful, but they do not create a durable, queryable lifecycle for every out-of-scope finding.

Without a dedicated discovery lifecycle:

- findings can remain buried in one run log;
- the same issue can be rediscovered repeatedly;
- canonical docs may never be updated;
- queues can omit the required follow-up;
- a next prompt may be vague, duplicated, or never routed;
- later agents must reread old reports to reconstruct context.

## Mandatory run lifecycle

Every non-trivial run uses this closure sequence:

```text
1. Execute only the owned scope.
2. Capture every meaningful out-of-scope finding.
3. Reconcile findings against existing discoveries, docs, queues, prompts, risks, and mistake cards.
4. Classify each finding.
5. Route it to exactly one primary owner.
6. Create or update a copy-ready follow-up prompt when action is required.
7. Link the finding, owner, and prompt from the run log.
8. Mark the run discovery-complete only when every finding has a disposition.
```

A run can be task-complete but must not be marked learning-complete when discovery reconciliation is missing.

## What counts as a discovery

Create a discovery for a meaningful observation that is not safely resolved within the current prompt.

Categories:

```text
Bug
Risk
SecurityPrivacy
DataLoss
ValidationGap
TestGap
DocumentationGap
PromptGap
QueueGap
ContextGap
ArchitectureGap
ProductOpportunity
PerformanceWaste
ToolingGap
RepeatedMistake
Other
```

Do not create a discovery for:

- formatting preferences with no durable value;
- speculative ideas with no evidence or expected value;
- an issue already tracked with no new evidence;
- work that belongs to and is completed inside the current task;
- secrets, raw logs, full diffs, or sensitive source content.

## Discovery record contract

Canonical markdown path:

```text
.ai/discoveries/<discovery-id>.md
```

Required fields:

```text
Discovery ID:
Title:
Status:
Category:
Severity:
Confidence:
Found in run:
Found while doing:
Evidence summary:
Affected paths/contracts:
Why out of scope:
Duplicate of:
Primary owner:
Canonical doc target:
Queue target:
Prompt target:
Gate/dependencies:
Recommended validation:
Disposition:
Created:
Last reviewed:
Resolved by:
```

Use `.ai/DISCOVERY_RECORD_TEMPLATE.md`.

## Status model

```text
Inbox
Triaged
Planned
Blocked
NeedsEvidence
Ready
InProgress
Resolved
Duplicate
Rejected
Stale
```

Transitions:

```text
Inbox -> Triaged
Triaged -> Planned | Ready | Blocked | NeedsEvidence | Duplicate | Rejected
Planned | Ready -> InProgress
InProgress -> Resolved | Blocked | NeedsEvidence
Any open state -> Stale after review
Stale -> Triaged | Rejected | Resolved
```

No discovery may be deleted merely because it is inconvenient. Close it with a disposition.

## Severity and confidence

Severity:

```text
P0 — immediate safety, privacy, data-loss, false release/validation, or destructive risk
P1 — important correctness, repeated waste, queue/router, evidence, or architecture gap
P2 — useful improvement, test/docs gap, maintainability or performance issue
P3 — optional idea with limited current impact
```

Confidence:

```text
Confirmed
Probable
Possible
Unknown
```

`Possible` and `Unknown` findings should usually produce an investigation-only prompt, not an implementation prompt.

## Primary routing owner

Each discovery has exactly one primary owner:

| Finding | Primary owner |
|---|---|
| Repeated agent behavior or waste | `docs/ai/learning/MISTAKE_LEDGER.md` |
| Product/runtime work | owning implementation queue |
| Missing validation or test | validation/testing queue |
| Stale/contradictory canonical knowledge | owning canonical doc |
| Prompt quality or missing prompt | `docs/prompts/` and owning queue |
| Queue/router drift | owning queue or `PROMPT_QUEUE_ROUTER.md` |
| Security/privacy concern | security/privacy doc and gated prompt |
| Uncertain issue | investigation-only prompt |

Secondary links are allowed, but one owner must be accountable for closure.

## Automatic reconciliation policy

AgentsWatch should automate low-risk bookkeeping and preparation as far as possible.

Safe to automate locally after Gate 0 and the relevant command is implemented:

- create discovery records from structured run-log findings;
- assign IDs, timestamps, category, and default status;
- search for likely duplicates using IDs, normalized titles, paths, and categories;
- suggest severity, confidence, owner, queue, and gate;
- generate copy-ready investigation/implementation/test/docs/review prompts;
- append queue candidates in `Planned` or `Blocked` state;
- link run logs, discoveries, prompts, and queue rows;
- report stale, unowned, duplicate, or unresolved discoveries;
- produce rollups and metrics;
- mark a discovery `NeedsEvidence` when claims lack proof.

Require explicit approval before automation:

- edits canonical product, security, architecture, or policy documents;
- changes a queue item to `Ready` when risk or dependency is uncertain;
- changes severity to or from P0/P1;
- edits runtime code, tests, CI, packaging, release, deployment, or licensing;
- closes a security/privacy/data-loss discovery;
- commits, pushes, opens a PR, merges, deploys, or releases.

Never automate:

- unrelated runtime fixes hidden inside the current task;
- bypassing Gate 0, permission, validation, or approval rules;
- deletion of unresolved findings;
- upload of source code, prompts, diffs, run logs, or discovery records by default.

## Reconciliation algorithm

For each finding:

```text
1. Normalize title, category, affected paths, and evidence signature.
2. Search open discoveries, mistake ledger, risk register, owning queues, and prompt IDs.
3. If same root issue exists, update and link it instead of duplicating it.
4. If evidence is insufficient, set NeedsEvidence and generate investigation-only prompt.
5. If blocked by Gate 0 or dependencies, set Blocked and record the exact gate.
6. If actionable and safe, set Planned or Ready according to queue policy.
7. Create/update canonical-doc candidate only when the finding changes durable knowledge.
8. Generate the smallest useful prompt with owned/avoid paths, stop rules, validation, and evidence output.
9. Link all artifacts from the originating run log.
```

## Prompt generation rules

A generated prompt must include:

```text
Repository
Prompt ID
Source discovery IDs
Queue
Run mode
Token budget
Read first
Inspect only
Owned paths
Avoid paths
Do not edit
Task
Acceptance criteria
Stop rules
Validation
Required evidence
Expected discovery reconciliation
```

Default mapping:

| Confidence / category | Prompt mode |
|---|---|
| Unknown or Possible | investigation-only |
| Confirmed bug with known minimal fix | implementation |
| TestGap | tests |
| DocumentationGap | docs/evidence |
| QueueGap or PromptGap | docs/evidence |
| SecurityPrivacy or DataLoss | read-only investigation first |
| Completed implementation | diff-only review |

## Canonical documentation promotion

Not every finding belongs in broad documentation.

Promote a finding when it is:

- durable knowledge future agents need;
- a rule, invariant, gate, ownership boundary, or accepted decision;
- a confirmed limitation or required operational procedure;
- a repeated mistake that needs prevention;
- a status change in a canonical queue or router.

Keep one-off evidence in the discovery/run log and link it rather than bloating canonical docs.

When promotion is required:

```text
1. identify the canonical owner document;
2. draft the smallest update;
3. update the docs index/router only when navigation changes;
4. record source discovery IDs;
5. validate links and contradictions;
6. mark the discovery resolved only after the canonical update is committed or explicitly rejected.
```

## Completion gate

Every run log must contain:

```text
Out-of-scope discoveries:
Discovery reconciliation:
Discoveries created:
Discoveries updated:
Duplicates linked:
Canonical docs updated:
Follow-up prompts generated:
Queue rows created/updated:
Unresolved discoveries:
```

Allowed completion values:

```text
none found
reconciled
blocked - <reason>
not run - <reason>
```

Completion score caps:

| Situation | Maximum score |
|---|---:|
| Meaningful finding mentioned but no discovery record/disposition | 75% |
| Discovery exists but has no owner or route | 80% |
| Actionable discovery has no prompt or documented no-op | 85% |
| Canonical-doc change needed but not routed | 80% |
| Discovery reconciliation completed and linked | normal score rules |

## Review cadence

Run reconciliation:

- after every non-trivial run;
- immediately for P0/P1 findings;
- after 3-5 prompt-system commits;
- after five meaningful run logs;
- before closing a milestone, release, or dogfood cycle.

Run missed-discovery audit when:

- run logs contain `missed`, `residual risk`, or `follow-up` text without discovery IDs;
- the same issue appears in two run logs;
- a canonical doc changes without a source finding or task;
- a queue item has no source evidence;
- an agent final response mentions unrelated work that is not tracked.

## Metrics

Track locally:

```text
findings captured
findings reconciled
duplicate rate
unowned discoveries
open by severity/status
median age to triage
median age to resolution
findings converted to prompts
prompts completed
repeated rediscovery count
canonical-doc promotions
stale findings closed
```

The most important quality metric is:

```text
Repeated rediscovery of the same untracked issue should trend toward zero.
```

## Planned CLI commands

Runtime implementation starts only after Gate 0 evidence allows feature work.

```bash
agentswatch discover add --from-run <run-id>
agentswatch discover list [--status <status>]
agentswatch discover reconcile <run-id|discovery-id|--all-open>
agentswatch discover promote <discovery-id> --to-doc <path>
agentswatch discover prompt <discovery-id>
agentswatch discover close <discovery-id> --disposition <text>
agentswatch lint discoveries
agentswatch finish <task-id> --learn --reconcile
agentswatch next --from-discoveries
```

## Implementation order

```text
1. Gate 0 restore/build/test/CLI smoke evidence.
2. Init support for discovery folders/templates.
3. Markdown discovery parser/writer and ID generation.
4. Run-log extraction and deterministic reconciliation.
5. Duplicate detection and routing suggestions.
6. Prompt generation and queue candidate output.
7. Discovery lint and completion gate.
8. Rollup/stale audit.
9. Optional connector-assisted execution only after permission/evidence gates are proven.
```

## Privacy

Discovery data is project-local by default.

Do not upload:

- source code;
- full prompts or diffs;
- command output;
- run logs;
- discovery records;
- mistake ledgers.

Store compact evidence summaries and paths, not secret-bearing raw content.

## Related prompts

- `docs/prompts/DISC-001-capture-run-discoveries.md`
- `docs/prompts/DISC-002-reconcile-discovery-inbox.md`
- `docs/prompts/DISC-003-promote-discovery-to-docs.md`
- `docs/prompts/DISC-004-generate-follow-up-prompts.md`
- `docs/prompts/DISC-005-audit-missed-discoveries.md`
- `docs/prompts/DISC-006-close-stale-discoveries.md`
- `docs/prompt_queues/agentwatch_discovery_and_self_improvement.md`
