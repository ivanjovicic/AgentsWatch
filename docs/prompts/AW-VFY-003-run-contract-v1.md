# AW-VFY-003 — RunContract v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-002  
Run mode: implementation  
Budget: medium  
Gate: Gate 0 closed

## Read only

- `AGENTS.md`
- `docs/PRODUCT_SPEC.md`
- `docs/ARCHITECTURE.md`
- `docs/DATA_MODEL.md` — RunContract
- `docs/CLI_SPEC.md` — contract commands
- `docs/COMMAND_CONTRACTS.md` — contract commands
- exact current Core/CLI/test files needed for implementation

## Task

Implement the canonical `RunContract v1` model, JSON persistence, deterministic lint, and the minimum CLI surface needed to create/check contracts.

## Required contract fields

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

Use explicit enums/types where they improve deterministic validation without overengineering.

## Required behavior

- canonical JSON path: `.agentwatch/contracts/<contract-id>.json`;
- supported schema version is explicit;
- invalid/unsupported versions fail clearly;
- implementation contracts require intent + acceptance criteria + validation/stop/evidence fields;
- path patterns are validated enough to reject clearly invalid/unsafe values without pretending to implement a full glob engine here;
- creation must not overwrite an existing contract ID silently;
- `contract check` returns deterministic findings with rule IDs/reasons;
- no LLM/provider key is required.

`contract create` may start with a practical deterministic input form if natural-language compilation would introduce provider scope. The important deliverable is the stable contract and lint spine, not an AI text generator.

## Architecture requirement

Do not put the domain/application logic directly into a growing `Program.cs` switch body. Introduce the smallest reusable application service/use case and storage abstraction appropriate for later CLI/MCP reuse.

## Owned paths

- `src/AgentsWatch.Core/**`
- `src/AgentsWatch.Cli/**`
- a minimal local-storage implementation location consistent with current solution boundaries
- `tests/AgentsWatch.Tests/**`
- documentation only if implementation reveals a required contract correction

## Avoid

- start/finish lifecycle;
- receipt/evidence/drift/claims implementation;
- SQLite;
- LLM integration;
- generic DI/framework expansion unless truly needed;
- token optimizer work.

## Tests

Cover at minimum:

- valid implementation contract;
- missing intent;
- missing acceptance criteria;
- missing validation requirement;
- missing stop/evidence fields;
- unsupported schema version;
- JSON round trip;
- no overwrite of existing contract;
- stable path generation.

## Validation

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Also run CLI smoke for the new contract commands against a temporary repository/workspace.

## Expected evidence

- final RunContract v1 shape;
- storage path and schema-version behavior;
- lint rule list;
- tests and CLI examples;
- full validation result;
- explicit limitations deferred to later prompts.

## Completion rule

Do not promote AW-VFY-004 until contract persistence/lint is deterministic and the full test gate is green.
