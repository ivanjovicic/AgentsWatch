# AW-VFY-006 — RunReceipt v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-005  
Run mode: implementation  
Budget: medium  
Gate: attributable RunDelta proven

## Read only

- `AGENTS.md`
- `docs/DATA_MODEL.md` — RunReceipt
- `docs/ARCHITECTURE.md`
- `docs/CLI_SPEC.md` / `docs/COMMAND_CONTRACTS.md` — finish/receipt/handoff
- current run baseline/delta implementation
- `src/AgentsWatch.Reports/**`
- directly related tests

## Task

Implement canonical `RunReceipt v1` persistence and derive Markdown run report/handoff from the structured receipt.

## Required canonical receipt fields

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

Fields may be empty/unknown where later prompts will populate them, but the schema and semantics must be explicit and stable enough for AW-VFY-007/008/009.

## Required outputs

```text
.agentwatch/runs/<run-id>.json   # canonical
.ai/runs/<run-id>.md             # generated projection
.ai/handoffs/<run-id>.md         # compact generated projection
```

## Rules

- JSON is the source of truth;
- Markdown must be generated from the structured model, not hand-maintained as independent truth;
- report must distinguish attributable vs pre-existing/ambiguous files;
- no validation result may be synthesized from prose;
- full chat history/source contents/full command logs are excluded by default;
- failed Markdown generation must not corrupt an already valid canonical receipt;
- schema version must round-trip.

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
- LLM summarization.

## Required tests

- receipt JSON round trip;
- schema version behavior;
- attributable/pre-existing/ambiguous change projection;
- Markdown generated from receipt;
- handoff generated from receipt;
- no validation claim when validation list is empty;
- unknown agent/model/tool handled cleanly;
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
- example canonical JSON shape (compact/redacted);
- report/handoff sections;
- end-to-end smoke result;
- full test result;
- fields intentionally left for evidence/scope/claims prompts.
