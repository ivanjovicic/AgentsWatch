# AgentsWatch Agent Rulebook

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

## Source of truth

1. Current code and tests.
2. `README.md`.
3. `docs/AGENT_SHARED_OPERATING_STANDARD.md`.
4. `docs/AGENT_RUN_LOG_ENFORCEMENT.md`.
5. `.ai/RUN_LOG_TEMPLATE.md` and `.ai/runs/README.md`.
6. `docs/ai/learning/MISTAKE_LEDGER.md`.
7. `docs/DOCS_INDEX.md`.
8. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
9. `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md` when the user asks for the next prompt.
10. `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` and `docs/PROMPT_LINT_CHECKLIST.md`.
11. `docs/ZERO_WASTE_EXECUTION_PROTOCOL.md`.
12. `docs/AGENT_RUN_EVIDENCE_STANDARD.md`.
13. `docs/WASTE_LEARNING_LOOP.md`.
14. `docs/PROMPT_BATCH_REVIEW_POLICY.md`.
15. `docs/AGENT_OPERATING_SYSTEM.md`.
16. `docs/CONTEXT_INDEX.md`.
17. Bootstrap validation docs while Gate 0 is incomplete.
18. Product contracts: `docs/CLI_SPEC.md`, `docs/COMMAND_CONTRACTS.md`, `docs/CLI_UX_OUTPUT_SPEC.md`, `docs/CONFIG_REFERENCE.md`, `docs/REPORT_FORMATS.md`, `docs/DATA_MODEL.md`, `docs/ADAPTER_SPEC.md`.
19. Prompt queues under `docs/prompt_queues/`.

If documents disagree, current code/tests and committed `.ai/runs` evidence win over planning notes or chat history.

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

Use `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` and `docs/AGENT_SHARED_OPERATING_STANDARD.md` as hard authority for:

- green/yellow/red prompt classification;
- token budgets;
- file read/edit/search limits;
- run mode enforcement;
- prompt split rules;
- stop rules;
- final evidence requirements.

Reject or rewrite prompts that fail lint.

## Run evidence and learning rule

Every non-trivial run must leave realistic `.ai/runs/<date>-<prompt-id>-evidence.md` evidence before it is considered complete.

The agent must record:

- model/client metadata or `unknown-not-exposed`;
- elapsed/phase timing or `unknown-not-recorded`;
- what was done;
- what was missed;
- files inspected;
- files changed;
- validation run or why it did not run;
- where time/tokens were wasted;
- why waste happened;
- relevant prior mistakes read;
- mistakes observed or `none`;
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
5. update `docs/ai/learning/MISTAKE_LEDGER.md`;
6. add or update a lint/test prompt;
7. record why no rule or prompt update was needed.

A prompt cannot be marked high-confidence `Done` unless it references a run log or explicit fallback, and the score obeys `docs/AGENT_RUN_LOG_ENFORCEMENT.md`.

## Prompt batch review rule

After 3-5 important prompt, queue, rule, evidence, or agent-workflow commits, run `docs/PROMPT_BATCH_REVIEW_POLICY.md` before continuing to add more prompt-system changes.

Batch review must check:

- broken references;
- stale queue statuses;
- prompts marked Ready despite blocked gates;
- missing validation evidence;
- contradiction between `AGENTS.md`, `DOCS_INDEX.md`, shared standard, run-log gate, router, queues, and prompt files;
- missing follow-up prompts for discovered issues.

If review finds more than three unrelated issues, add follow-up prompts instead of fixing everything in one run.

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

- repository;
- prompt id;
- queue;
- run mode;
- token budget;
- scope limiter;
- owned paths;
- avoid paths;
- stop rules;
- validation;
- expected evidence;
- relevant prior mistakes read;
- handoff summary when split or blocked.

Use investigation-only first when root cause is unknown. Use diff-only review after implementation commits.

## Working order

1. Read `docs/AGENT_SHARED_OPERATING_STANDARD.md`.
2. Read `docs/AGENT_RUN_LOG_ENFORCEMENT.md` and choose/create the `.ai/runs` path.
3. Read `docs/ai/learning/MISTAKE_LEDGER.md` and select relevant mistake IDs.
4. Read `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
5. Lint the prompt with `docs/PROMPT_LINT_CHECKLIST.md`.
6. Apply `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` limits.
7. Follow `docs/ZERO_WASTE_EXECUTION_PROTOCOL.md` during execution.
8. If the user asks for the next prompt, use `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`.
9. Read `docs/CONTEXT_INDEX.md` and the owning queue.
10. If Gate 0 is incomplete, select from `docs/prompt_queues/bootstrap_validation.md`.
11. Otherwise select one Ready prompt from the owning queue.
12. Inspect only the relevant docs/files.
13. Make the smallest safe change.
14. Add targeted tests when runtime behavior changes.
15. Run narrow validation when possible.
16. Record validation honestly.
17. Record run evidence using `.ai/RUN_LOG_TEMPLATE.md`.
18. Apply `docs/WASTE_LEARNING_LOOP.md` and `docs/ai/learning/MISTAKE_LEDGER.md`.
19. If the run belongs to a 3-5 important prompt-system commit batch, apply `docs/PROMPT_BATCH_REVIEW_POLICY.md`.
20. Mark prompt `Done`, `Blocked`, or `Needs evidence sync` with run-log path or fallback.
21. Commit and push to `main` unless the user requests another flow.

## Validation defaults

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

Do not claim validation passed unless it was actually run or CI evidence is available.
