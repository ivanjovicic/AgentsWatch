# Discovery Data Model Addendum

Last aligned: 2026-07-03

## Markdown phase

Canonical record:

```text
.ai/discoveries/<discovery-id>.md
```

Required logical fields:

```text
DiscoveryId
Title
Status
Category
Severity
Confidence
FoundInRunId
FoundWhileDoing
EvidenceSummary
AffectedPathsOrContracts
OutOfScopeReason
DuplicateOfDiscoveryId
PrimaryOwner
CanonicalDocTarget
QueueTarget
PromptTarget
GateDependencies
RecommendedValidation
Disposition
CreatedAt
LastReviewedAt
ResolvedBy
```

## Link model

A discovery may link to many runs, prompts, queue rows, documents, mistake cards, risks, commits, and pull requests. One discovery remains the canonical record for the same root issue.

## JSON phase

Optional local sidecars may use:

```text
.agentwatch/discoveries/<discovery-id>.json
.agentwatch/discovery-events.jsonl
.agentwatch/prompt-candidates.jsonl
```

Each serialized record should include a schema version.

## SQLite phase

Planned local tables:

- `discoveries`;
- `discovery_links`;
- `discovery_events`;
- `prompt_candidates`.

## Invariants

- one primary owner per triaged discovery;
- duplicate records point to one canonical discovery;
- resolved records retain their history;
- actionable open records have a next action or documented no-op;
- source-containing evidence stays local;
- markdown remains importable into later JSON/SQLite storage without losing IDs or links.
