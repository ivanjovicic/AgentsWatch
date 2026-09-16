# AW-VFY-006 — RunReceipt v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-005  
Run mode: implementation  
Budget: medium  
Gate: run-interval delta proven

## Read only

- `AGENTS.md`
- `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`
- `docs/DATA_MODEL.md` — RunReceipt
- `docs/ARCHITECTURE.md`
- `docs/CLI_SPEC.md` / `docs/COMMAND_CONTRACTS.md` — finish/receipt/handoff
- current run baseline/delta implementation
- `src/AgentsWatch.Reports/**`
- directly related tests

## Task

Implement canonical `RunReceipt v1` persistence as a compact evidence artifact and derive Markdown run report/handoff from structured data.

## Required canonical receipt fields

```text
schemaVersion
runId
contractId
taskId
startedAtUtc
finishedAtUtc
executorMetadata if known
startRepositoryState
endRepositoryState
runDelta
validations[]
claims[]
acceptanceCriteria[]
findings[]
decision
missedWork[]
evidenceDigest/reference if implemented
```

Fields may be empty/unknown where later prompts will populate them, but schema semantics must be explicit enough for AW-VFY-007/008/009.

Do **not** require these as canonical verification fields:

```text
learningNote
nextPrompt
routeSuggestion
optimizationAdvice
verboseSessionNarrative
```

Those belong in optional handoff/learning projections downstream of trustworthy verification evidence.

## Required outputs

```text
.agentwatch/runs/<run-id>.json   # canonical verification evidence
.ai/runs/<run-id>.md             # generated evidence projection
.ai/handoffs/<run-id>.md         # compact generated handoff; may contain non-canonical next-step text
```

## Rules

- JSON is the verification source of truth;
- Markdown must be generated from structured data, not hand-maintained as independent truth;
- report distinguishes run-interval changes, pre-existing state and ambiguity;
- do not call run-interval evidence `agent-authored` unless executor-specific provenance proves it;
- no validation result may be synthesized from prose;
- full chat history/source contents/full command logs are excluded by default;
- failed Markdown generation must not corrupt an already valid canonical receipt;
- schema version must round-trip;
- optional handoff learning/next-step text must not affect verification status.

## Owned paths

- `src/AgentsWatch.Core/**`
- `src/AgentsWatch.Reports/**`
- `src/AgentsWatch.Cli/**` only for receipt/show/handoff wiring
- local storage code
- `tests/AgentsWatch.Tests/**`

## Avoid

- implementing Evidence Gate logic beyond placeholder/default decision fields;
- scope/claims verification logic;
- SQLite;
- dashboard/MCP;
- LLM summarization;
- routing/learning system.

## Required tests

- receipt JSON round trip;
- schema version behavior;
- run-interval/pre-existing/ambiguous change projection;
- Markdown generated from receipt;
- handoff generated from receipt;
- no validation claim when validation list is empty;
- unknown executor/model/tool handled cleanly;
- learning/nextPrompt not required for canonical receipt validity;
- write ordering/failure safety where practical.

## Validation

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Run one end-to-end temporary-repo smoke:

```text
contract -> start -> edit -> finish -> receipt show -> inspect JSON/Markdown
```

## Expected evidence

- final RunReceipt v1 schema;
- compact/redacted example canonical JSON;
- report/handoff sections and canonical-vs-projection boundary;
- end-to-end smoke result;
- full test result;
- fields intentionally left for evidence/scope/claims prompts.
