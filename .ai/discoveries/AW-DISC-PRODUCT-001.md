# AW-DISC-PRODUCT-001 — Discovery automation is not implemented yet

Discovery ID: AW-DISC-PRODUCT-001  
Status: Blocked  
Category: ProductOpportunity  
Severity: P1  
Confidence: Confirmed  
Found in run: AW-DISCOVERY-SYSTEM-001  
Found while doing: review of AgentsWatch self-improvement and out-of-scope learning behavior  
Created: 2026-07-03  
Last reviewed: 2026-07-03

## Evidence summary

The repository has strong learning and evidence documents, while the current CLI exposes only `init`, `optimize`, and `status`. Discovery capture, reconciliation, prompt generation, and lint behavior are now specified but not implemented as runtime commands.

## Affected paths or contracts

- `src/AgentsWatch.Cli/Program.cs`
- `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`
- `docs/DISCOVERY_CLI_CONTRACT.md`
- `docs/prompt_queues/agentwatch_discovery_and_self_improvement.md`

## Reason it was not handled in the active task

Gate 0 restore/build/test/CLI-smoke evidence is incomplete, and repository rules prohibit new CLI feature work before that gate.

## Reconciliation

Duplicate of: none  
Primary owner: `docs/prompt_queues/agentwatch_discovery_and_self_improvement.md`  
Canonical document target: `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`  
Queue target: `AW-DISC-001` through `AW-DISC-007`  
Prompt target: owning AW-DISC queue rows  
Gate or dependencies: AW-VAL-001, AW-VAL-002, init hardening, run-report groundwork  
Recommended validation: targeted tests per AW-DISC slice, then solution build/test and CLI smoke

## Disposition

Action: queue  
Reason: implement in gated, testable slices after bootstrap evidence  
Resolved by: none

## Links

Run log: `.ai/runs/2026-07-03-AW-DISCOVERY-SYSTEM-001-evidence.md`  
Mistake IDs: `AW-MISTAKE-DISCOVERY-001`  
Queue row: `docs/prompt_queues/agentwatch_discovery_and_self_improvement.md`  
Prompt file: none; queue defines implementation slices  
Commit or pull request: `https://github.com/ivanjovicic/AgentsWatch/pull/4`
