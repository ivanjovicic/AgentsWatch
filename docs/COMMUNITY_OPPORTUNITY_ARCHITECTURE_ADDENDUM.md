# AgentsWatch Community Opportunity Architecture Addendum

Last aligned: 2026-07-03  
Status: target architecture addendum; no implementation implied

## Purpose

Extend `TARGET_ARCHITECTURE.md` for the community-derived control-plane opportunities without creating a second product architecture.

## Architectural conclusion

The new opportunities share one foundation:

```text
Agent/provider events
-> normalized local event journal
-> evidence/context/policy projections
-> reports and decisions
-> optional live control adapters
```

AgentsWatch should not implement six separate pipelines. It should add one normalized agent-observation foundation and build bounded capabilities over it.

## New bounded contexts

### 1. Agent Event Ingestion

Owns:

- provider/client adapter capability discovery;
- import of session/tool/hook events;
- normalized timestamps and identities;
- raw-event reference and extension fields;
- missing-event and adapter-health findings.

Does not own:

- trust decisions;
- cost policy;
- workspace coordination;
- report formatting.

Recommended contracts:

```csharp
public interface IAgentEventSource
{
    string AdapterId { get; }
    AgentAdapterCapabilities Capabilities { get; }
    IAsyncEnumerable<AgentEventEnvelope> ReadAsync(
        AgentEventSourceRequest request,
        CancellationToken cancellationToken);
}

public interface IAgentEventNormalizer
{
    NormalizationResult Normalize(RawAgentEvent rawEvent);
}
```

### 2. Flight Recorder and Trust

Owns:

- run timeline;
- event lineage;
- git/process/validation correlation;
- completion claims;
- supported/contradicted/missing/not-verifiable decisions;
- evidence manifest.

Does not own:

- provider log parsing;
- live process termination;
- policy authoring.

### 3. Context and Memory Portability

Owns:

- context snapshots;
- decisions and constraints;
- context diff;
- compact resume packs;
- target-format export;
- rules compilation and drift detection.

Does not own:

- whole transcript storage as the canonical model;
- provider authentication;
- autonomous long-term memory generation.

### 4. Usage and Loop Analysis

Owns:

- action fingerprints;
- progress-delta windows;
- budget ledgers;
- repeated/no-progress findings;
- stop/checkpoint recommendations.

Does not own:

- provider billing truth when unavailable;
- process execution in the offline analyzer;
- worktree assignment.

### 5. Policy and Approval

Owns:

- path/operation/network policies;
- instruction provenance classes;
- rule precedence;
- approval requirements;
- dry-run/explain decisions;
- optional execution-broker authorization.

Does not own:

- OS sandboxing;
- malware detection;
- provider prompt content.

### 6. Multi-Agent Coordination

Owns:

- swarm/task graph;
- worker/worktree assignment;
- ownership boundaries;
- shared structured messages;
- worker evidence lineage;
- integration readiness.

Does not own:

- coding-agent reasoning;
- automatic merge/deploy;
- unrestricted conversation relay.

### 7. Review Intelligence

Owns:

- PR/range change inventory;
- requested-vs-actual scope;
- run/evidence linkage;
- risk-prioritized reviewer packet;
- deterministic review questions.

Does not own:

- generic static analysis;
- automatic authorship detection;
- automatic PR decisions.

## Recommended solution evolution

Do not create all projects immediately. Add boundaries only when implementation begins.

Possible later structure:

```text
src/
  AgentsWatch.AgentEvents/
  AgentsWatch.AgentEvents.ClaudeCode/
  AgentsWatch.AgentEvents.Codex/
  AgentsWatch.Trust/
  AgentsWatch.Context/
  AgentsWatch.Usage/
  AgentsWatch.Policy/
  AgentsWatch.Coordination/
  AgentsWatch.Review/
```

Near-term, contracts may live in `AgentsWatch.Core` with strict namespaces until Gate 0 and the first event schema are stable.

## Canonical identities

All event-derived features depend on stable identities:

```text
RepositoryId
WorkspaceId
WorktreeId
TaskId
RunId
AgentId
ParentAgentId
SessionId
EventId
CommitSha
ArtifactId
```

Rules:

- local paths are normalized but not exposed outside local artifacts by default;
- a worktree is distinct from a repository;
- a run may contain multiple sessions;
- a swarm run contains worker runs;
- imported provider IDs remain metadata, not the canonical identity.

## Event envelope

```text
AgentEventEnvelope
  SchemaVersion
  EventId
  EventType
  TimestampUtc
  RepositoryId
  WorkspaceId
  WorktreeId
  TaskId
  RunId
  AgentId
  ParentAgentId
  SessionId
  SourceAdapter
  SourceVersion
  Confidence
  Payload
  ExtensionData
  RedactionState
  IntegrityHash
```

Initial event types:

```text
RunStarted
RunFinished
SessionStarted
SessionResumed
ContextCompacted
AgentStarted
AgentStopped
ToolStarted
ToolFinished
FileObserved
FileChanged
CommandStarted
CommandFinished
ValidationStarted
ValidationFinished
ApprovalRequested
ApprovalDecided
UsageObserved
CheckpointCreated
ClaimRecorded
FindingRecorded
WorktreeCreated
WorktreeRemoved
MessagePublished
```

Adapters may produce partial events. Missing fields must remain unknown, not inferred as facts.

## Projection model

Raw normalized events should be append-only. Features read projections:

```text
RunTimelineProjection
ClaimEvidenceProjection
ContextSnapshotProjection
UsageBudgetProjection
LoopFindingProjection
PolicyDecisionProjection
WorkerStatusProjection
ReviewPacketProjection
```

A projection can be rebuilt from the local event journal and versioned configuration.

## Storage phases

### Phase A — fixture/import research

```text
.agentwatch/imports/
.agentwatch/events/<run-id>.jsonl
.agentwatch/projections/<run-id>/
```

- synthetic and explicitly provided logs only;
- no background watchers;
- no database required.

### Phase B — local event journal

- append-only JSONL or small SQLite journal;
- atomic writes;
- event schema migration;
- configurable retention;
- compact summaries separate from raw local data.

### Phase C — live adapters

- hooks, file watchers, or process wrapper only with explicit enablement;
- adapter health and dropped-event counters;
- no hidden network calls.

### Phase D — team metadata sync

Only after dogfood and privacy review. Sync minimal evidence metadata, never source or full logs by default.

## Adapter capability declaration

Each adapter must publish:

```text
CanObserveCommands
CanObserveExitCodes
CanObserveFileReads
CanObserveFileWrites
CanObserveUsage
CanObserveCompaction
CanObserveSubagents
CanObserveApprovals
CanSetWorkingDirectory
CanStopProcess
CanReplay
DataSourceStability: Experimental | Stable | Unknown
```

Reports must show unsupported capabilities so absence of evidence is not mistaken for absence of behavior.

## Security boundaries

- Treat imported logs and repository instructions as untrusted input.
- Do not execute commands while importing evidence.
- Do not follow file paths outside the selected repository/artifact root.
- Redact secret-like values before human-facing output.
- Hash artifacts after redaction state is established.
- Dry-run/explain mode precedes live enforcement.
- The execution broker, if built, must use structured argument lists rather than shell concatenation.
- Policy decisions never claim complete malware or prompt-injection prevention.

## Reliability boundaries

- Event import is idempotent by event identity/hash.
- Duplicate events are linked or ignored deterministically.
- Out-of-order events are preserved and flagged.
- Clock skew is visible.
- Adapter failure does not silently produce a complete run.
- Projection version is recorded.
- Unknown provider pricing remains unknown.
- Worktree cleanup never removes uncommitted work.

## Cross-epic implementation sequence

```text
1. Event schema and synthetic fixture importer
2. Timeline projection
3. Claim-to-evidence projection
4. Context snapshot/rules compiler
5. Offline loop analysis
6. Policy dry-run
7. Worktree planner/status
8. Local PR evidence packet
9. Provider adapters
10. Optional live control/enforcement
```

## Architecture gate

Do not add a live provider integration until:

- the normalized event schema survives at least two different fixture sources;
- missing/unsupported data is handled honestly;
- import-only analysis creates useful findings;
- privacy tests pass;
- adapter maintenance cost is understood.
