# AgentsWatch Context Index

Last aligned: 2026-06-30

Use this file to choose the smallest useful context before running an agent.

## Default rule

```text
Start with the smallest context that can safely answer the prompt.
```

Do not read documents because they are interesting. Read them only because the current prompt needs them.

## Context tiers

### Tier 0 — almost always enough for small tasks

Read:

- owning prompt or queue section;
- one relevant contract doc;
- one implementation file or focused design doc.

Use for:

- docs/evidence updates;
- one command behavior change;
- one formatter/parser/report change;
- small prompt queue edits.

### Tier 1 — add only when the task proves it needs more

Optional docs:

- `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md` for normal anti-waste rules;
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md` when gate status matters;
- `docs/REPORT_FORMATS.md` when markdown reports or handoffs change;
- `docs/DATA_MODEL.md` when JSON/SQLite/storage shape changes;
- `docs/SECURITY_AND_PRIVACY.md` when command output, secrets, storage, or network behavior is involved.

### Tier 2 — full context for broad planning only

Read broader product docs only when changing roadmap, positioning, or architecture:

- `docs/PRODUCT_SPEC.md`;
- `docs/MVP_ROADMAP.md`;
- `docs/ARCHITECTURE.md`;
- `docs/ROADMAP_VALIDATION_GATES.md`.

### Tier 3 — prompt-system changes only

Read these only when changing prompt rules, lint rules, or agent workflow:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`;
- `docs/PROMPT_LINT_CHECKLIST.md`;
- `docs/PROMPT_RULES.md`;
- `docs/AGENT_OPERATING_SYSTEM.md`.

## Context budget

```text
Low budget: read up to 3 docs before first action
Medium budget: read up to 5 docs before first action
High budget: read up to 8 docs before first action
```

If more docs are required, stop and explain why before reading more.

## Need the next prompt fast

Minimum read:

- `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`.

Optional if unclear:

- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.

Use when:

- user asks “what next?”;
- user wants a copy-ready prompt;
- Gate 0 status is unclear.

## Bootstrap validation

Minimum read:

- `docs/prompt_queues/bootstrap_validation.md`;
- `docs/BUILD_VALIDATION_PLAN.md`.

Optional if failures or risks appear:

- `docs/RISK_REGISTER.md`;
- `docs/BOOTSTRAP_NEXT_STEPS.md`;
- `docs/VALIDATION_EVIDENCE_2026_06_29.md`.

Use when:

- build/test/smoke validation is incomplete;
- `.sln` or `.csproj` files change;
- CI/workflow files change.

## CLI command work

Minimum read:

- `docs/COMMAND_CONTRACTS.md`;
- the command implementation file.

Optional:

- `docs/CLI_UX_OUTPUT_SPEC.md` for console output changes;
- `docs/CONFIG_REFERENCE.md` for config changes;
- `docs/REPORT_FORMATS.md` for report/handoff changes;
- `docs/TEST_MATRIX.md` for test planning;
- `docs/AGENT_COMMAND_PLAYBOOK.md` only if command safety is unclear.

Use when:

- adding or changing CLI commands;
- changing file-system writes;
- changing validation command behavior.

## Command profiling and fast validation

Minimum read:

- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`;
- `docs/COMMAND_CONTRACTS.md`.

Optional:

- `docs/REPORT_FORMATS.md` for report/handoff integration;
- `docs/DATA_MODEL.md` for command history shape;
- `docs/SECURITY_AND_PRIVACY.md` for output redaction/log storage;
- `docs/CLI_UX_OUTPUT_SPEC.md` for console UX;
- `docs/ADAPTER_SPEC.md` for language-specific suggestions.

Use when:

- adding `agentswatch run -- <command>`;
- changing `agentswatch validate --suggest` behavior;
- storing command duration, exit code, or output summaries;
- recommending faster language-specific validation;
- deciding what terminal output is safe to include in reports.

Hard rule:

```text
Do not give the agent full terminal logs by default. Use compact command evidence.
```

## Prompt optimizer work

Minimum read:

- `docs/PROMPT_OPTIMIZATION_PLAYBOOK.md`;
- `docs/PROMPT_TOKEN_ECONOMY_QUICK_RULES.md`.

Optional:

- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` only when changing anti-waste rules;
- `docs/PROMPT_LINT_CHECKLIST.md` only when changing lint behavior;
- `docs/PROMPT_RULES.md` only when changing prompt templates;
- `docs/COMPLETION_ANALYTICS.md` or `docs/TOKEN_WASTE_METRICS.md` only when analytics change.

Use when:

- changing prompt risk scoring;
- generating prompt splits;
- writing handoff or diff-only review prompts.

## Git evidence and reports

Minimum read:

- `docs/REPORT_FORMATS.md`;
- `docs/DATA_MODEL.md`.

Optional:

- `docs/ADAPTER_SPEC.md` for stack-specific report labels;
- `docs/RISK_SCORING_MODEL.md` for risk categories;
- `docs/PROMPT_EVIDENCE_TEMPLATE.md` for evidence format changes.

Use when:

- changing git parsing;
- changing run reports;
- changing risk findings;
- changing handoff summaries.

## Adapters

Minimum read:

- `docs/ADAPTER_SPEC.md`.

Optional:

- `docs/TEST_MATRIX.md` for validation coverage;
- `docs/SECURITY_AND_PRIVACY.md` if adapter output may reveal secrets or command output.

Use when:

- adding .NET, Flutter, React/TypeScript, Python, or Node detection;
- suggesting validation commands;
- flagging high-risk file categories.

## Product planning

Minimum read:

- `docs/PRODUCT_SPEC.md`;
- `docs/MVP_ROADMAP.md`.

Optional:

- `docs/ULTRA_ROADMAP.md`;
- `docs/90_DAY_EXECUTION_PLAN.md`;
- `docs/ROADMAP_VALIDATION_GATES.md`;
- `docs/DOGFOOD_PLAN.md`;
- `docs/ISSUE_BACKLOG.md`.

Use when:

- changing roadmap;
- adding public positioning;
- deciding dashboard or SaaS readiness.

## Security/privacy

Minimum read:

- `docs/SECURITY_AND_PRIVACY.md`.

Optional:

- `SECURITY.md`;
- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md` for command profiling output.

Use when:

- reports may include command output;
- config/risk rules mention secrets;
- future integrations send data outside the local machine;
- command profiling stores stdout/stderr summaries or raw logs.