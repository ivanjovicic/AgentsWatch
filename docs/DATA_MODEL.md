# AgentsWatch Data Model

Last aligned: 2026-08-25  
Status: active MVP contract

## Core rule

Machine-readable structured state is canonical from the verification MVP onward.

```text
JSON = source of truth
Markdown = human-readable projection
```

Do not make verification logic depend on parsing free-form Markdown.

## Storage phases

### Phase 1 — JSON + Markdown projection

Canonical machine data:

```text
.agentwatch/contracts/<contract-id>.json
.agentwatch/active-runs/<run-id>.json
.agentwatch/runs/<run-id>.json
```

Human-readable projection:

```text
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
.ai/STATUS.md
.ai/CHANGELOG_AI.md
```

### Phase 2 — JSONL indexes / command evidence

When needed:

```text
.agentwatch/command-history.jsonl
.agentwatch/learning-events.jsonl
.agentwatch/mistake-patterns.json
```

### Phase 3 — SQLite

Only after schemas stabilize and queries justify it:

```text
.agentwatch/agentswatch.db
```

## Schema-version rule

Every persisted contract, baseline and receipt must contain:

```text
schemaVersion
```

Readers must reject unsupported future versions clearly rather than silently misparse them.

## RunContract v1

Minimum canonical shape:

```text
schemaVersion
contractId
taskId
intent
acceptanceCriteria[]
ownedPaths[]
avoidPaths[]
permissionMode
runMode
validationContract
stopRules[]
expectedEvidence[]
createdAtUtc
```

Optional later fields:

```text
dependencies[]
riskGates[]
budgetGuidance
routeSuggestion
sourceRoadmapItem
```

Rules:

- implementation contracts require non-empty intent and acceptance criteria;
- `ownedPaths` / `avoidPaths` may be empty only when explicitly justified by contract type;
- validation requirements are explicit;
- incomplete contracts produce lint findings;
- generated Markdown is not authoritative.

## RunBaseline v1

Persisted by `agentswatch start` under:

```text
.agentwatch/active-runs/<run-id>.json
```

Minimum fields:

```text
schemaVersion
runId
contractId
taskId
startedAtUtc
branch
headCommitSha
worktreeState
agent
model
tool
```

### WorktreeState

Must preserve enough evidence to distinguish pre-existing changes from run-attributable changes.

Minimum logical fields:

```text
porcelainVersion
staged[]
unstaged[]
untracked[]
stateFingerprint
```

Each tracked changed-file record should support:

```text
path
oldPath
status
contentOrDiffFingerprint
```

Do not store full source-file contents merely to calculate attribution when hashes/diff fingerprints are sufficient.

## RunDelta v1

Produced by comparing start baseline with end repository evidence.

```text
attributableChanges[]
preExistingUnchangedChanges[]
preExistingChangedFurther[]
attributionAmbiguities[]
endBranch
endHeadCommitSha
```

### AttributableChange

```text
path
oldPath
status
addedLines?
deletedLines?
attribution
attributionReason
```

Suggested attribution values:

```text
Attributable
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Rules:

- raw end-of-run dirty state is never automatically equivalent to attributable changes;
- ambiguous attribution remains explicit;
- scope and claims checks operate on attributable changes, while ambiguities are surfaced separately.

## ValidationEvidence v1

```text
validationId
runId
commandDisplay
status
startedAtUtc?
finishedAtUtc?
durationMs?
exitCode?
outputSummary?
firstErrorLine?
source
```

Status values:

```text
Pass
Fail
NotRun
BlockedByEnvironment
TimedOut
Killed
Unknown
```

Source values may include:

```text
AgentsWatchCommand
ImportedAgentResult
UserDeclared
CI
Unknown
```

Rules:

- user-declared evidence is labeled as such;
- full stdout/stderr is not stored by default;
- secret-looking values are redacted before summaries are persisted;
- `Pass` must not be invented from an agent prose claim without a corresponding evidence source.

## AgentClaim v1

```text
claimId
runId
claimType
rawText?
source
value
```

Initial deterministic claim types:

```text
TestsAdded
DocsOnly
BackendUnchanged
MigrationAdded
ValidationPassed
NoUnrelatedChanges
```

Claims may initially be entered/imported structurally. LLM extraction is optional later.

## Finding v1

Common shape for contract/evidence/scope/claim findings:

```text
findingId
runId?
contractId?
category
severity
status
message
paths[]
evidenceRefs[]
ruleId
```

Suggested categories:

```text
ContractIncomplete
AttributionAmbiguous
MissingValidation
ValidationFailed
ScopeOutsideOwnedPaths
AvoidPathTouched
UnsupportedClaim
AcceptanceCriterionUnsupported
RiskApprovalRequired
```

Finding status:

```text
Supported
Unsupported
Unknown
NeedsReview
```

## AcceptanceCriterionResult v1

```text
criterionId
text
status
evidenceRefs[]
reason
```

Status:

```text
Supported
Unsupported
Unknown
```

MVP may require explicit/manual mappings where semantics cannot be verified deterministically. Unknown is preferable to fabricated certainty.

## RunReceipt v1

Canonical path:

```text
.agentwatch/runs/<run-id>.json
```

Minimum fields:

```text
schemaVersion
runId
contractId
taskId
startedAtUtc
finishedAtUtc
agent
model
tool
startRepositoryState
endRepositoryState
runDelta
validations[]
claims[]
acceptanceCriteria[]
findings[]
decision
missedWork[]
learningNote
nextPrompt
```

### RunDecision

```text
status
reasons[]
override?
```

Status values:

```text
Done
NeedsEvidence
NeedsReview
NeedsApproval
Blocked
Failed
```

Override, if supported later:

```text
overriddenBy
overrideReason
overriddenAtUtc
```

Rules:

- no numeric score alone upgrades status;
- mandatory validation missing => cannot be `Done`;
- unresolved attribution ambiguity that affects scope/acceptance should normally prevent high-confidence `Done`;
- all non-Done decisions expose reasons.

## Markdown projections

Generated from structured data:

```text
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

The Markdown report should include concise sections for:

- contract intent;
- attributable changes;
- pre-existing changes/ambiguities;
- validation;
- claims and support status;
- scope findings;
- acceptance criteria;
- decision and reasons;
- missed work;
- learning note;
- next prompt.

## CommandProfile — later

Command profiling remains useful after the verification spine works.

Potential shape:

```text
commandId
runId
workingDirectory
commandDisplay
commandHash
startedAtUtc
finishedAtUtc
durationMs
exitCode
stdoutBytes
stderrBytes
status
firstErrorLine
outputSummary
suggestedByAgentsWatch
```

Do not let command-profiling work block RunContract/RunReceipt/Evidence implementation.

## LearningEvent — post-receipt MVP

```text
learningEventId
runId
category
message
ruleCandidate
scope
createdAtUtc
accepted
confidence
evidenceCount
expiresAtUtc?
```

Learning is only trustworthy after receipt attribution and evidence are trustworthy.

## Compatibility rule

Every human-readable report must be fully regenerable from the canonical JSON models without losing the core verification state.

No downstream checker may require information that exists only in Markdown prose.
