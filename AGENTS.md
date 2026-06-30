# AgentsWatch Agent Rulebook

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

## Source of truth

1. Current code and tests.
2. `README.md`.
3. `docs/DOCS_INDEX.md`.
4. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
5. `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md` when the user asks for the next prompt.
6. `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`.
7. `docs/PROMPT_LINT_CHECKLIST.md`.
8. `docs/ZERO_WASTE_EXECUTION_PROTOCOL.md`.
9. `docs/AGENT_RUN_EVIDENCE_STANDARD.md`.
10. `docs/WASTE_LEARNING_LOOP.md`.
11. `docs/AGENT_OPERATING_SYSTEM.md`.
12. `docs/CONTEXT_INDEX.md`.
13. Bootstrap validation docs while Gate 0 is incomplete.
14. Product contracts: `docs/CLI_SPEC.md`, `docs/COMMAND_CONTRACTS.md`, `docs/CLI_UX_OUTPUT_SPEC.md`, `docs/CONFIG_REFERENCE.md`, `docs/REPORT_FORMATS.md`, `docs/DATA_MODEL.md`, `docs/ADAPTER_SPEC.md`.
15. Prompt queues under `docs/prompt_queues/`.

## Product rules

- Build local CLI first.
- Do not start with SaaS, billing, cloud sync, or dashboard before roadmap gates allow it.
- Universal git/markdown/file-system behavior comes before language adapters.
- Risk scoring must stay heuristic and explainable.
- Keep prompts small and split broad work.
- Markdown report contracts come before SQLite/dashboard work.
- No hidden telemetry or network calls in MVP.

## Token economy rule

Every non-trivial prompt must pass `docs/PROMPT_LINT_CHECKLIST.md` before execution.

Use `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` as the hard authority for:

- green/yellow/red prompt classification;
- token budgets;
- file read/edit/search limits;
- run mode enforcement;
- prompt split rules;
- stop rules;
- final evidence requirements.

Reject or rewrite prompts that fail lint.

## Run evidence and learning rule

Every non-trivial run must leave realistic evidence before it is considered complete.

The agent must record:

- what was done;
- what was missed;
- files inspected;
- files changed;
- validation run or why it did not run;
- where time/tokens were wasted;
- why waste happened;
- docs/rules updated to prevent repeat;
- optimized prompt added or reason none was needed;
- follow-up prompt;
- residual risk;
- commit SHA.

For every meaningful issue, waste item, blocker, stale reference, unclear rule, or repeated failure, the agent must do at least one of:

1. update an existing docs rule;
2. add a new rule to the relevant playbook;
3. update the prompt queue;
4. add a new optimized prompt;
5. record why no rule or prompt update was needed.

## Bootstrap rule

Gate 0 is not complete until restore/build/test and CLI smoke evidence exist.

Until then, work must prioritize:

1. `AW-VAL-001` build validation;
2. `AW-VAL-002` CLI smoke validation;
3. `AW-VAL-003` validation evidence review;
4. `AW-VAL-004` init hardening.

Do not add new CLI features before build/test/smoke evidence exists.

## Required prompt fields

Every non-trivial task must include:

- run mode;
- token budget;
- scope limiter;
- owned paths;
- avoid paths;
- stop rules;
- validation;
- handoff summary when split or blocked.

Use investigation-only first when root cause is unknown. Use diff-only review after implementation commits.

## Working order

1. Read `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
2. Lint the prompt with `docs/PROMPT_LINT_CHECKLIST.md`.
3. Apply `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` limits.
4. Follow `docs/ZERO_WASTE_EXECUTION_PROTOCOL.md` during execution.
5. If the user asks for the next prompt, use `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`.
6. Read `docs/CONTEXT_INDEX.md` and the owning queue.
7. If Gate 0 is incomplete, select from `docs/prompt_queues/bootstrap_validation.md`.
8. Otherwise select one Ready prompt from the owning queue.
9. Inspect only the relevant docs/files.
10. Make the smallest safe change.
11. Add targeted tests when runtime behavior changes.
12. Run narrow validation when possible.
13. Record validation honestly.
14. Record run evidence using `docs/AGENT_RUN_EVIDENCE_STANDARD.md`.
15. Apply `docs/WASTE_LEARNING_LOOP.md`: update docs/rules/queue or add an optimized prompt for discovered waste.
16. Mark prompt `Done`, `Blocked`, or `Needs evidence sync`.
17. Commit and push to `main` unless the user requests another flow.

## Validation defaults

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

Do not claim validation passed unless it was actually run or CI evidence is available.
