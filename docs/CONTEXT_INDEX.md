# AgentsWatch Context Index

Last aligned: 2026-06-29

Use this file to choose the smallest useful context before running an agent.

## Always read

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/AGENT_OPERATING_SYSTEM.md`
- owning prompt queue

## Need the next prompt fast

Read:

- `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`

Use when:

- user asks “what next?”;
- user wants a copy-ready prompt;
- Gate 0 status is unclear.

## Bootstrap validation

Read:

- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`
- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/VALIDATION_EVIDENCE_2026_06_29.md`
- `docs/prompt_queues/bootstrap_validation.md`

Use when:

- build/test/smoke validation is incomplete;
- `.sln` or `.csproj` files change;
- CI/workflow files change.

## CLI command work

Read:

- `docs/CLI_SPEC.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/CONFIG_REFERENCE.md`
- `docs/REPORT_FORMATS.md`
- `docs/TEST_MATRIX.md`
- `docs/AGENT_COMMAND_PLAYBOOK.md`

Use when:

- adding or changing CLI commands;
- changing file-system writes;
- changing validation command behavior.

## Prompt optimizer work

Read:

- `docs/PROMPT_OPTIMIZATION_PLAYBOOK.md`
- `docs/PROMPT_RULES.md`
- `docs/PROMPT_QUALITY_CHECKLIST.md`
- `docs/COMPLETION_ANALYTICS.md`
- `docs/TOKEN_WASTE_METRICS.md`

Use when:

- changing prompt risk scoring;
- generating prompt splits;
- writing handoff or diff-only review prompts.

## Git evidence and reports

Read:

- `docs/DATA_MODEL.md`
- `docs/REPORT_FORMATS.md`
- `docs/ADAPTER_SPEC.md`
- `docs/RISK_SCORING_MODEL.md`
- `docs/PROMPT_EVIDENCE_TEMPLATE.md`

Use when:

- changing git parsing;
- changing run reports;
- changing risk findings;
- changing handoff summaries.

## Adapters

Read:

- `docs/ADAPTER_SPEC.md`
- `docs/TEST_MATRIX.md`
- `docs/SECURITY_AND_PRIVACY.md`

Use when:

- adding .NET, Flutter, React/TypeScript, Python, or Node detection;
- suggesting validation commands;
- flagging high-risk file categories.

## Product planning

Read:

- `docs/PRODUCT_SPEC.md`
- `docs/ULTRA_ROADMAP.md`
- `docs/90_DAY_EXECUTION_PLAN.md`
- `docs/ROADMAP_VALIDATION_GATES.md`
- `docs/DOGFOOD_PLAN.md`
- `docs/ISSUE_BACKLOG.md`

Use when:

- changing roadmap;
- adding public positioning;
- deciding dashboard or SaaS readiness.

## Security/privacy

Read:

- `SECURITY.md`
- `docs/SECURITY_AND_PRIVACY.md`

Use when:

- reports may include command output;
- config/risk rules mention secrets;
- future integrations send data outside the local machine.
