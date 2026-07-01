# AgentsWatch Testing Prompt Queue

Last aligned: 2026-06-30  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: AgentsWatch testing and quality gates  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/prompt_queues/agentwatch_testing.md`  
Parent docs: [../TEST_STRATEGY.md](../TEST_STRATEGY.md), [agentwatch_mvp.md](agentwatch_mvp.md)

## Purpose

Provide detailed test implementation prompts for AgentsWatch functionality.

## Rules

- Do not add tests before the target implementation exists unless the prompt is explicitly docs/spec first.
- Prefer small fixture-based tests over real external repositories.
- Do not hit the network by default.
- Do not include real secrets, real private code, or generated dependency folders in fixtures.
- Golden outputs must use fixed clocks and deterministic ordering.
- Tests must cover both happy path and failure path.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-TEST-001 | Ready after AW-VAL-001 | Add skeleton test project validation and first CLI help/version tests. |
| AW-TEST-002 | Ready after init implementation | Test `agentswatch init` idempotency, dry-run, overwrite safety, and path safety. |
| AW-TEST-003 | Ready after git tracker implementation | Test clean/dirty/untracked/renamed/deleted/binary-file git fixtures. |
| AW-TEST-004 | Ready after report implementation | Add golden tests for markdown run report and handoff summary. |
| AW-TEST-005 | Ready after risk scoring implementation | Test high-risk file patterns and missing-test risk reasons. |
| AW-TEST-006 | Ready after validation adapter implementation | Test Flutter/.NET/React/Python/Node detection and validation suggestions. |
| AW-TEST-007 | Ready after mistake-learning implementation | Test mistake ledger, mistake check, rollups, and evidence lint failures. |

## Validation

```bash
dotnet test
```

Use narrower filters once test projects and traits exist.
