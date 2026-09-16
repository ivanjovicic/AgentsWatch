# AW-VFY-003 — RunContract v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-002  
Run mode: implementation  
Budget: medium  
Gate: Gate 0 closed

## Read only

- `AGENTS.md`
- `docs/DEEP_DIVE_DECISIONS_2026_09_16.md`
- `docs/PRODUCT_SPEC.md`
- `docs/ARCHITECTURE.md`
- `docs/DATA_MODEL.md` — RunContract
- `docs/CLI_SPEC.md`
- `docs/COMMAND_CONTRACTS.md`
- exact current Core/CLI/test files needed for implementation

## Task

Implement canonical `RunContract v1` model, JSON persistence, deterministic lint and the minimum CLI/application surface needed to create/check contracts.

RunContract is a **verification normalization layer**, not another project-management system.

The MVP must support a practical deterministic creation path now while keeping the model ready to import/normalize existing issue/prompt/roadmap intent later.

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
sourceTaskRef?        # optional source issue/prompt/roadmap reference
createdAtUtc
```

Use explicit enums/types where they improve deterministic validation without overengineering.

## Required behavior

- canonical JSON path: `.agentwatch/contracts/<contract-id>.json`;
- supported schema version is explicit;
- invalid/unsupported versions fail clearly;
- implementation contracts require intent + acceptance criteria + validation/stop/evidence fields;
- source task reference is optional and does not bind Core to GitHub/Jira/Linear;
- path patterns reject clearly invalid/unsafe values without pretending to build a full glob engine here;
- creation never silently overwrites an existing contract ID;
- `contract check` returns deterministic findings with rule IDs/reasons;
- no LLM/provider key is required;
- creation UX should avoid forcing duplicate PM metadata unrelated to verification.

`contract create` may start with structured CLI/file input. Natural-language/task-system import is not required in this prompt.

The important output is the stable **verification contract + lint spine**.

## Architecture requirement

Do not put domain/application logic directly into a growing `Program.cs` switch body.

Introduce the smallest reusable application service/use case and storage abstraction appropriate for later CLI/MCP/importer reuse.

Do not add tracker-specific dependencies to the core model.

## Owned paths

- `src/AgentsWatch.Core/**`
- `src/AgentsWatch.Cli/**`
- minimal local-storage implementation consistent with current solution boundaries
- `tests/AgentsWatch.Tests/**`
- documentation only if implementation reveals a required contract correction

## Avoid

- start/finish lifecycle;
- receipt/evidence/drift/claims implementation;
- Jira/Linear/GitHub API integrations;
- SQLite;
- LLM integration;
- generic DI/framework expansion unless truly needed;
- token optimizer work.

## Tests

Cover at minimum:
- valid implementation contract;
- optional sourceTaskRef round trip;
- missing intent;
- missing acceptance criteria;
- missing validation requirement;
- missing stop/evidence fields;
- unsupported schema version;
- JSON round trip;
- no overwrite of existing contract;
- stable path generation;
- no requirement for unrelated PM metadata.

## Validation

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Also run CLI smoke for new contract commands against a temporary repository/workspace.

## Expected evidence

- final RunContract v1 shape;
- storage path and schema-version behavior;
- lint rule list;
- tests and CLI examples;
- full validation result;
- explicit limitation: source-task import adapters are later, but the contract model does not duplicate/own full task-management state.

## Completion rule

Do not promote AW-VFY-004 until contract persistence/lint is deterministic and the full test gate is green.
