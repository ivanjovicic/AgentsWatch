# AgentsWatch 90-Day Execution Plan

Last aligned: 2026-08-25  
Status: tactical execution plan

## 90-day goal

In 90 days, AgentsWatch should be a trustworthy local CLI that can take a bounded task contract, record one external coding-agent run, attribute the actual repository changes, verify evidence/scope/common claims, and produce a reusable run receipt.

The purpose of the 90-day period is to prove the verification loop, not to maximize feature count.

## Primary outcome

```text
Task -> Contract -> Start baseline -> Agent -> Finish delta -> Receipt -> Verification
```

## Workstreams

| Workstream | Goal |
|---|---|
| Gate 0 | Make CI green and prove CLI smoke. |
| Contract | Define/lint canonical RunContract v1. |
| Attribution | Correctly isolate run changes from pre-existing dirty work. |
| Receipt | Persist canonical RunReceipt v1 and Markdown projection. |
| Verification | Evidence, scope drift and common claims checks. |
| Adapters | Universal git first, then .NET and Flutter validation hints. |
| Dogfood | Collect 30 useful receipts and measure real catches/false positives. |
| Packaging | Keep global-tool install and local workflow simple. |

## Weeks 1–2 — Close Gate 0 and establish contracts

Tasks:

- fix/harden git porcelain parsing;
- add parser edge-case tests;
- rerun restore/build/test;
- run CLI smoke in temporary directories/repos;
- update Gate 0 evidence;
- implement `RunContract v1` model/schema/storage/lint.

Exit criteria:

- CI green;
- CLI smoke proven;
- valid/invalid contract fixtures tested;
- `.agentwatch/contracts/<id>.json` is stable and canonical.

## Weeks 3–4 — Start-run baseline

Tasks:

- extract application use cases/ports from CLI where needed;
- implement `agentswatch start`;
- record branch/HEAD/staged/unstaged/untracked baseline;
- add atomic active-run persistence;
- handle clean/dirty repositories;
- refuse overlapping active run by default;
- add attribution-oriented tests.

Exit criteria:

- a dirty repository can start safely;
- pre-existing files are clearly recorded;
- no source contents are unnecessarily persisted;
- start state can drive a later delta comparison.

## Weeks 5–6 — Finish-run attribution and receipt

Tasks:

- implement `agentswatch finish`;
- compute attributable delta;
- handle pre-existing unchanged and changed-further files;
- represent attribution ambiguity;
- support add/delete/rename/untracked cases;
- implement `RunReceipt v1`;
- generate Markdown report from JSON receipt;
- generate compact handoff.

Exit criteria:

- tests prove no false attribution for a pre-existing dirty file;
- tests prove attributable edits are captured;
- receipt survives without chat history;
- Markdown can be regenerated from JSON.

## Weeks 7–8 — Evidence and Scope gates

Tasks:

- add validation evidence model/import path;
- implement `evidence check`;
- enforce mandatory validation requirements;
- implement `drift check`;
- test owned/avoid paths cross-platform;
- add decision reasons and audit-friendly override design if needed.

Exit criteria:

- missing mandatory validation prevents `Done`;
- scope drift identifies exact paths/rules;
- pre-existing unchanged dirty files do not create drift;
- unknown/ambiguous states remain explicit.

## Weeks 9–10 — Claims verification and adapter hardening

Tasks:

- implement initial structured claim types;
- verify `TestsAdded`, `DocsOnly`, `BackendUnchanged`, `MigrationAdded`, `ValidationPassed`, `NoUnrelatedChanges`;
- harden universal git adapter;
- refine .NET and Flutter validation suggestions;
- avoid broad automatic validation.

Exit criteria:

- at least one synthetic unsupported claim is caught per claim family;
- claim failures explain the exact supporting/missing evidence;
- verification works without any LLM provider key.

## Weeks 11–12 — Dogfood and product proof

Tasks:

- dogfood AgentsWatch on itself;
- dogfood on at least one .NET repo;
- dogfood on at least one Flutter repo;
- collect toward 30 useful receipts;
- record false positives/false negatives/ambiguities;
- measure setup friction and handoff usefulness;
- test global tool packaging/install;
- decide next phase from evidence.

Exit criteria:

- 30 useful receipts or a documented reason why the workflow is not sticky enough to reach that number;
- at least one real unsupported-claim catch;
- at least one real scope-drift catch;
- at least one real missing-evidence block;
- no known silent false attribution in covered cases;
- decision recorded for MCP/GitHub check/dashboard priorities.

## Success criteria

AgentsWatch is successful at day 90 if:

- the CLI is installable locally;
- Contract -> Start -> Finish -> Receipt -> Verify works end to end;
- dirty-worktree attribution is covered by tests and dogfood;
- receipts are useful without full session history;
- verification catches real agent mistakes or missing evidence;
- dogfood users can understand every status decision;
- the next investment decision is based on receipts, not feature enthusiasm.

## Do not do in first 90 days

- SaaS;
- billing/OAuth;
- hosted dashboard;
- generic agent runtime;
- cloud workspaces;
- visual orchestration;
- generic token/cost dashboard as the main product;
- complex empirical router;
- automatic merge/release/deploy;
- broad integration marketplace;
- deep vendor-specific session capture before the vendor-neutral receipt model is stable.

## Queue

Execution is governed by:

`docs/prompt_queues/verification_mvp_2026_08_25.md`
