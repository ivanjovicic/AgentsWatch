# AW-VFY-003 evidence: RunContract v1

- Prompt: AW-VFY-003; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Gate basis: Gate 0 green locally (AW-VFY-002). GitHub CI confirmation still pending (nothing pushed); user explicitly chose to proceed.
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## RunContract v1 shape (`src/AgentsWatch.Core/Contracts/RunContract.cs`)

schemaVersion(int=1), contractId, taskId, intent, acceptanceCriteria[], ownedPaths[], avoidPaths[],
permissionMode(read-only|workspace-write|approval-required), runMode(implementation|investigation|validation|docs),
validationContract{requirements[{id,command,mandatory}]}, stopRules[], expectedEvidence[],
sourceTaskRef?{system,reference,url?} (opaque, no tracker dependency), createdAtUtc.
JSON camelCase, kebab-case string enums, nulls omitted. No PM metadata (assignee/priority/labels/status...).

## Storage / schema

- `.agentwatch/contracts/<contract-id>.json` via `FileRunContractStore` (FileMode.CreateNew: never silently overwrites; `--force` explicit).
- ID: explicit `--id` or deterministic slug of taskId (lowercase, `[a-z0-9._-]`, no `..`); invalid IDs rejected before any path is built.
- Deserialize reads schemaVersion first; missing/non-int/unsupported -> AWC001 with clear reason; malformed JSON -> AWC000.

## Lint rules (implementation mode = error; other modes downgrade content rules to warning)

AWC000 parse error; AWC001 unsupported schema; AWC002 invalid contractId; AWC003 missing taskId; AWC004 intent;
AWC005 acceptance criteria; AWC006 unsafe path pattern (empty, whitespace, absolute/drive, `..`, forbidden chars - not a glob engine);
AWC007 no mandatory validation; AWC008 stop rules; AWC009 expected evidence; AWC010 no ownedPaths (warning);
AWC011 owned/avoid identical overlap; AWC012 validation requirement missing id/command.

## Architecture

Application use case `ContractService` + `IRunContractStore` port in Core; thin `Cli/ContractCommand.cs` adapter; Program.cs only routes.

## Files

- new: `src/AgentsWatch.Core/Contracts/RunContract.cs`, `src/AgentsWatch.Core/Contracts/ContractService.cs`, `src/AgentsWatch.Cli/ContractCommand.cs`, `tests/AgentsWatch.Tests/RunContractTests.cs`
- changed: `src/AgentsWatch.Cli/Program.cs` (route + help), queue/prompt status

## Tests

23 new (valid, missing intent/AC/validation/stop/evidence, investigation warnings, 6 unsafe paths, overlap, unsupported schema incl. JSON 99/missing/malformed, JSON round trip, optional sourceTaskRef, stable path + no overwrite + force, deterministic IDs, unsafe explicit id, no PM metadata, CLI end-to-end).

## Gate (executed)

- restore exit 0; build Release 0 errors; test Release: Failed 0, Passed 38, Skipped 0, Total 38.

## CLI smoke (temp git repo)

- create AW-DEMO-1 full flags -> written, Valid: yes, exit 0; check aw-demo-1 -> exit 0
- create AW-DEMO-1 again -> `refusing to overwrite`, exit 1
- create AW-DEMO-2 draft with `../x` -> written as draft, Valid: no (AWC005, 006, 007, 008, 009), exit 0; check -> exit 1

## Limitations

Source-task import adapters (GitHub/Jira/Linear/prompt files) are later; model holds only an opaque reference and does not own task-management state. `contract create --file` import not implemented. Glob semantics deferred to AW-VFY-008.

Commit SHA (change): d828b4e