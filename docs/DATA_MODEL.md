# AgentsWatch Data Model

Last aligned: 2026-07-05  
Status: run manifest v1 implemented on feature branch; wider model planned

## Purpose

Define local, versioned data shapes so CLI commands, reports, adapters, and future storage do not invent incompatible models.

Principles:

- local-first;
- JSON/Markdown before SQLite;
- evidence provenance and unknown states preserved;
- no full chat history or raw terminal output by default;
- no absolute local repository root in shareable run artifacts;
- schema changes must be versioned and validated.

## Storage phases

### Current foundation

Human report:

```text
.ai/runs/<task-id>.md
```

Machine sidecar:

```text
.agentwatch/runs/<task-id>.json
```

### Next local event/evidence files

```text
.agentwatch/command-history.jsonl
.agentwatch/evidence/<task-id>-pr-evidence.json
.ai/evidence/<task-id>-pr-evidence.md
```

### Future SQLite

Only after JSON/report contracts and migration rules stabilize:

```text
.agentwatch/agentswatch.db
```

## RunManifest schema v1.0

Implemented shape:

```text
SchemaVersion
TaskId
Title
StartedAt
FinishedAt
Status
ValidationStatus
StartBranch
StartCommitSha
EndBranch
EndCommitSha
AllowedPaths
ChangedFiles
OutOfScopeFiles
Warnings
```

JSON field names use camel case.

### Lifecycle status

```text
InProgress
Finished
```

`Finished` means the Git/scope capture was closed. It does not mean acceptance criteria, build, tests, CI, UI, database, or runtime behavior passed.

### Validation status

Schema values:

```text
NotRun
Pass
Fail
BlockedByEnvironment
Unknown
```

The current foundation writes only `NotRun`.

Future `Pass`/`Fail` values require linked command, CI, adapter, or explicit user-entered evidence.

### Git identity

Start/end identities use full hexadecimal object IDs:

```text
40 characters — SHA-1 repositories
64 characters — SHA-256 repositories
```

Arbitrary revision/command strings are not valid persisted object IDs.

### ChangedFile

```text
Path
Status
AddedLines optional
DeletedLines optional
```

Current normalized statuses:

```text
added
modified
deleted
type-changed
unmerged
renamed
copied
untracked
unknown/provider status
```

Current foundation lists the final path for rename/copy events. Rich old/new path lineage is a later schema extension.

### AllowedPaths

Repository-relative glob patterns.

Current matcher supports:

```text
*   within one path segment
?   one non-separator character
**  zero or more nested path segments
```

An empty list means unrestricted scope. It does not mean a scope check passed; it means out-of-scope evaluation is not applicable.

### Warnings

Current warning examples:

- branch changed during run;
- non-AgentsWatch uncommitted changes at finish;
- scope unrestricted/not evaluated;
- validation not captured.

Warnings must be deterministic evidence notes, not model-generated prose presented as fact.

## Machine/human artifact ownership

Managed run artifacts:

```text
.agentwatch/runs/*.json
.ai/runs/*.md
```

They are:

- excluded from coding-agent change attribution;
- ignored when deciding whether only previous managed evidence makes a baseline dirty;
- still protected from accidental overwrite by run ID/lifecycle rules.

Other `.ai` and `.agentwatch` files are not automatically excluded.

## Future CommandEvidence

Planned for `agentswatch run -- <command>`:

```text
SchemaVersion
CommandId
RunId optional
CommandDisplay redacted
CommandHash
CommandRedactionApplied
CommandRefusedReason
WorkingDirectoryIdentity
StartedAt
FinishedAt
DurationMs
StartCommitSha
EndCommitSha
ExitCode
StdoutBytes
StderrBytes
Status
FirstErrorLine redacted
OutputSummary redacted
SuggestedByAgentsWatch
```

Command statuses:

```text
Pass
Fail
BlockedByEnvironment
TimedOut
Cancelled
Killed
Refused
Unknown
```

Rules:

- command execution explicit;
- no source/output upload;
- raw stdout/stderr not persisted by default;
- secret-looking values redacted before display/storage;
- exit code 0 supports only the observed command result, not every agent claim;
- command evidence links to immutable Git state when available.

## Future Claim and Trust Ledger

```text
ClaimId
RunId
Text or structured statement
ClaimType
EvidenceLinks
Assessment
Reason
Confidence
AssessedAt
```

Assessments:

```text
SUPPORTED
PARTIALLY_SUPPORTED
CONTRADICTED
MISSING_EVIDENCE
STALE_EVIDENCE
NOT_OBSERVED
NOT_VERIFIABLE
SKIPPED
```

Rules:

- missing observation is not contradiction;
- stale evidence identifies the mismatched object/commit;
- confidence reflects evidence quality;
- model self-confidence is not evidence.

## Future PR Evidence Packet

```text
PacketId
RunId
BaseObjectId
HeadObjectId
DeclaredTask
DeclaredScope
ChangedFiles
OutOfScopeFiles
Claims
CommandEvidence
ValidationEvidence
CiEvidence
EvidenceObjectMatches
RiskFindings
ReviewerActions
GeneratedAt
```

The current run foundation is only a prerequisite and does not implement this packet.

## Future ContextSnapshot

```text
SnapshotId
TaskId
SourceObjectId
Goal
Constraints
Decisions
Completed
Pending
RelevantFiles
FailedApproaches
ValidationSummary
OpenRisks
TargetExports
LossReport
```

## Future runtime compatibility references

Runtime-specific evidence should reference:

```text
EffectiveRuntimeProfileId
ProfileRevision
SupportMode
CapabilityProvenance
BlindSpots
Fallback
```

Do not embed assumptions from a provider/model name directly into evidence truth.

## Data integrity rules

- unknown schema versions rejected;
- required fields validated during load/save;
- task IDs path-safe;
- start/end object IDs validated;
- new manifests written without overwrite;
- updates written through temporary file and atomic replacement;
- finished runs require finish time, end branch, and end object ID;
- no silent conversion of unknown values into success;
- migrations must preserve user-owned evidence and support rollback/export.

## Privacy rules

Default persisted/shared artifacts exclude:

- absolute repository root;
- source contents;
- full diffs;
- full prompts/chat history;
- raw stdout/stderr;
- secrets;
- cloud credentials;
- private organization identity unless explicitly supplied.

Paths inside the repository may be listed because scope/change review requires them; future redaction policy may mask sensitive path segments.

## Compatibility rule

Every Markdown projection must preserve enough structure to convert to JSON/SQLite without losing:

- identity/schema;
- lifecycle/timestamps;
- immutable Git identities;
- declared scope;
- changes and scope findings;
- validation status/provenance;
- warnings/unknowns;
- future command and claim evidence links.
