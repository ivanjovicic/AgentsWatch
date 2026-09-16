# AgentsWatch Data Model

Last aligned: 2026-09-16  
Status: active MVP contract

## Core rule

Machine-readable structured state is canonical.

```text
JSON = source of truth
Markdown = human-readable projection
```

Verification logic must never depend on parsing free-form Markdown.

## Evidence wording rule

AgentsWatch records **run-interval repository evidence**.

A change observed between the recorded start and finish of a run is not automatically proven to be authored by the selected AI agent. Humans, hooks, formatters, generators or another process may write concurrently.

Use causal language only when executor instrumentation proves it.

Otherwise classify evidence explicitly as:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Ambiguity must remain visible and may force `NeedsReview`.

## Storage phases

### MVP — JSON + Markdown projection

Canonical machine data:

```text
.agentwatch/contracts/<contract-id>.json
.agentwatch/active-runs/<run-id>.json
.agentwatch/runs/<run-id>.json
```

Human projections:

```text
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

### Later — optional indexes/learning

Only after receipts are trustworthy and externally valuable:

```text
.agentwatch/command-history.jsonl
.agentwatch/learning-events.jsonl
.agentwatch/mistake-patterns.json
```

SQLite is optional later only when stable schemas/query needs justify it.

## Schema version rule

Every persisted contract, baseline and receipt contains:

```text
schemaVersion
```

Readers reject unsupported future versions instead of silently misparsing them.

# RunContract v1

RunContract is a **verification normalization layer**, not a replacement for GitHub/Jira/Linear/task-management metadata.

Default flow should import/normalize an existing issue, roadmap item or prompt and ask only for missing verification-specific information.

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
sourceTaskRef?
createdAtUtc
```

Rules:
- implementation contracts require non-empty intent and acceptance criteria;
- validation requirements are explicit;
- incomplete contracts produce lint findings rather than invented scope;
- generated Markdown is not authoritative.

# RunBaseline v1

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
executorMetadata?
```

## WorktreeState

Preserve staged, unstaged and untracked state separately.

```text
porcelainVersion
staged[]
unstaged[]
untracked[]
stateFingerprint
```

Changed-file evidence should support:

```text
path
oldPath?
status
contentOrDiffFingerprint
```

Do not persist full source contents merely to calculate state comparison when hashes/diff fingerprints are sufficient.

# RunDelta v1

Produced by comparing the start baseline with finish repository evidence.

Canonical naming should avoid causal overclaim:

```text
runIntervalChanges[]
preExistingUnchangedChanges[]
preExistingChangedFurther[]
attributionAmbiguities[]
endBranch
endHeadCommitSha
```

## RunIntervalChange

```text
path
oldPath?
status
addedLines?
deletedLines?
classification
classificationReason
executorAttribution?
```

Classification values:

```text
RunIntervalChange
PreExistingUnchanged
PreExistingChangedFurther
Ambiguous
```

Rules:
- raw end-of-run dirty state is never equivalent to run-interval change;
- concurrent-writer ambiguity is preserved;
- scope/claims checks can use high-confidence run-interval evidence but must surface material ambiguity separately;
- `executorAttribution` is optional and requires a trustworthy executor-specific evidence source.

# ValidationEvidence v1

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
sourceRef?
evidenceDigest?
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

Source examples:

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
- secret-looking values are redacted before summaries persist;
- `Pass` must never be inferred only from agent prose.

# AgentClaim v1

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

Semantic claims such as `BugFixed` may be advisory later but must not become deterministic fact without evidence.

# Finding v1

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

# AcceptanceCriterionResult v1

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

Unknown is preferable to fabricated semantic certainty.

# RunReceipt v1

Canonical path:

```text
.agentwatch/runs/<run-id>.json
```

RunReceipt is a **compact evidence artifact**, not a workflow/session transcript.

Minimum canonical fields:

```text
schemaVersion
runId
contractId
taskId
startedAtUtc
finishedAtUtc
executorMetadata?
startRepositoryState
endRepositoryState
runDelta
validations[]
claims[]
acceptanceCriteria[]
findings[]
decision
missedWork[]
evidenceDigest?
```

Do **not** require these as canonical audit fields:

```text
learningNote
nextPrompt
routeSuggestion
optimizationAdvice
verboseSessionNarrative
```

Those belong in optional downstream handoff/learning projections.

## RunDecision

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

Optional auditable override:

```text
overriddenBy
overrideReason
overriddenAtUtc
```

Rules:
- no numeric score upgrades status by itself;
- mandatory validation missing => cannot be `Done`;
- material unresolved attribution ambiguity affecting scope/acceptance normally prevents high-confidence `Done`;
- every non-Done decision exposes reasons.

# Markdown projections

Generated from structured data:

```text
.ai/runs/<run-id>.md
.ai/handoffs/<run-id>.md
```

Run projection should contain concise evidence sections:
- contract intent;
- run-interval changes;
- pre-existing state/ambiguities;
- validation;
- claims/support status;
- scope findings;
- acceptance criteria;
- decision/reasons;
- missed work.

Handoff may separately include:
- learning note;
- next prompt;
- suggested follow-up.

Those are not verification truth.

# LearningEvent — post-receipt

Only after receipt attribution/evidence and external value are trustworthy:

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

# Trust rule

Deterministic/core evidence:
- repository state/fingerprints;
- path/scope rules;
- validation results/provenance;
- narrow claim checks;
- evidence hashes/references.

AI-assisted only:
- free-text claim extraction;
- semantic acceptance analysis;
- explanation;
- suggested next step.

AI output must never silently upgrade `Unknown` / `NeedsReview` to `Supported` / `Done`.

## Compatibility rule

Every human-readable report must be regenerable from canonical JSON verification state without losing core evidence.

No downstream checker may require information that exists only in Markdown prose.

Latest strategy guardrails:
`DEEP_DIVE_DECISIONS_2026_09_16.md`
