# AW-VFY-007 — Validation evidence and Evidence Gate v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-006  
Run mode: implementation  
Budget: medium  
Gate: RunReceipt v1 proven

## Read only

- `AGENTS.md`
- `docs/PRODUCT_SPEC.md` — Evidence Gate
- `docs/DATA_MODEL.md` — ValidationEvidence/Finding/RunDecision
- `docs/COMMAND_CONTRACTS.md` — `evidence check`
- current RunContract/RunReceipt/application-use-case code
- directly related tests

## Task

Implement structured validation evidence and a deterministic Evidence Gate that can prevent unsupported completion.

## Required behavior

Support/import validation evidence with explicit source and status:

```text
Pass
Fail
NotRun
BlockedByEnvironment
TimedOut
Killed
Unknown
```

Source must be explicit, e.g. AgentsWatch command, CI, imported agent result, user-declared, unknown.

Implement `agentswatch evidence check <run-id>` or equivalent application use case that evaluates the contract + receipt and produces findings/decision reasons.

## Minimum deterministic rules

- unsupported schema or missing required receipt data -> not Done;
- mandatory validation missing/NotRun/Unknown -> `NeedsEvidence` unless contract explicitly allows otherwise;
- validation Fail -> `Failed` or `NeedsReview` according to a clear deterministic rule;
- BlockedByEnvironment -> `Blocked`/`NeedsEvidence`, never silently Pass;
- expected evidence missing -> finding;
- acceptance criterion without required support -> `Unsupported` or `Unknown`;
- approval-required risk unresolved -> `NeedsApproval` if such contract metadata already exists.

## Critical rules

- agent prose such as `all tests pass` is not validation evidence by itself;
- user-declared validation must remain labeled `UserDeclared`;
- every non-Done decision must expose reasons and evidence references;
- no numeric score may independently upgrade status;
- `unknown` is valid and preferable to false certainty.

## Owned paths

- `src/AgentsWatch.Core/**`
- `src/AgentsWatch.Cli/**` for evidence command wiring
- receipt/storage/report projection only where needed
- `tests/AgentsWatch.Tests/**`

## Avoid

- scope drift implementation;
- claims verification beyond treating prose claims as non-evidence;
- command profiler/full validation runner;
- LLM semantic judging;
- dashboard/MCP.

## Required tests

- mandatory validation Pass -> evidence requirement satisfied;
- validation missing -> cannot Done;
- validation Fail -> deterministic failure decision;
- BlockedByEnvironment;
- user-declared evidence remains labeled;
- expected evidence missing;
- unknown acceptance support;
- decision reasons persist/project to Markdown.

## Validation

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Smoke one receipt with missing validation and one with explicit passing evidence.

## Expected evidence

- validation evidence shape/source policy;
- Evidence Gate rule IDs;
- decision matrix;
- tests/smoke results;
- full validation result;
- intentionally deferred semantic/command-runner work.
