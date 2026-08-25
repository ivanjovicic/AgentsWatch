# AW-VFY-009 — Claims vs Diff vs Validation v1

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-008  
Run mode: implementation  
Budget: medium  
Gate: Scope Drift proven

## Read only

- `AGENTS.md`
- `docs/PRODUCT_SPEC.md` — claims verification
- `docs/DATA_MODEL.md` — AgentClaim/Finding
- `docs/COMMAND_CONTRACTS.md` — `claims check`
- current receipt/evidence/drift code and direct tests

## Task

Implement deterministic verification for the first structured claim classes.

Initial claim types:

```text
TestsAdded
DocsOnly
BackendUnchanged
MigrationAdded
ValidationPassed
NoUnrelatedChanges
```

Claims may be entered/imported structurally for MVP. Do not add an LLM provider merely to extract prose claims in this prompt.

## Verification rules

Examples:

### TestsAdded
Supported only when attributable changes include test files according to explicit project/path heuristics. Unknown is acceptable if test-location detection is uncertain.

### DocsOnly
Unsupported when attributable runtime/config/test/code files exist outside defined documentation patterns.

### BackendUnchanged
Use explicit stack/path classification. If repository structure is unknown, return Unknown rather than fabricate support.

### MigrationAdded
Supported when attributable migration files are added according to configured/adapter patterns.

### ValidationPassed
Supported only by structured validation evidence with acceptable source/status. Agent prose alone is unsupported.

### NoUnrelatedChanges
Use Scope Drift findings/contract ownership. If scope attribution is ambiguous, return Unknown/NeedsReview rather than Supported.

## Architecture rule

Claim verification must consume existing structured Contract/RunDelta/Validation/Scope evidence. Do not duplicate Git parsing or invent a second truth model.

## Output

Implement `agentswatch claims check <run-id>` or equivalent use case.

Each claim result must contain:

- claim type/value;
- Supported/Unsupported/Unknown;
- reason;
- evidence references;
- related paths when relevant.

Unsupported claims should affect run decision according to a simple explainable rule, usually `NeedsEvidence` or `NeedsReview` rather than opaque scoring.

## Owned paths

- `src/AgentsWatch.Core/**`
- stack/path classification helpers only as minimally required
- `src/AgentsWatch.Cli/**` for command wiring
- receipt/report projection
- `tests/AgentsWatch.Tests/**`

## Avoid

- natural-language claim extraction with LLM;
- broad static analysis;
- agent quality score/ranking;
- token/cost features;
- dashboard/MCP.

## Required tests

For each initial claim type:

- one supported case;
- one unsupported case;
- one Unknown/ambiguous case where meaningful.

Also test:

- validation prose without evidence does not support ValidationPassed;
- pre-existing unrelated dirty file does not make NoUnrelatedChanges false unless attributable;
- scope ambiguity flows into claim result;
- results persist/project through RunReceipt.

## Validation

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Run synthetic end-to-end receipts with deliberately false claims.

## Expected evidence

- rule table for each claim type;
- supported/unsupported/unknown examples;
- test results;
- full validation result;
- explicit limits before future semantic/LLM claim extraction.
