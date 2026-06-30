# AgentsWatch Agent Rulebook

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

## Source of truth

1. Current code and tests.
2. `README.md`.
3. `docs/DOCS_INDEX.md`.
4. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
5. `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md` when the user asks for the next prompt.
6. `docs/AGENT_OPERATING_SYSTEM.md`.
7. `docs/CONTEXT_INDEX.md`.
8. Bootstrap validation docs while Gate 0 is incomplete.
9. Product contracts: `docs/CLI_SPEC.md`, `docs/COMMAND_CONTRACTS.md`, `docs/CLI_UX_OUTPUT_SPEC.md`, `docs/CONFIG_REFERENCE.md`, `docs/REPORT_FORMATS.md`, `docs/DATA_MODEL.md`, `docs/ADAPTER_SPEC.md`.
10. Prompt queues under `docs/prompt_queues/`.

## Product rules

- Build local CLI first.
- Do not start with SaaS, billing, cloud sync, or dashboard before roadmap gates allow it.
- Universal git/markdown/file-system behavior comes before language adapters.
- Risk scoring must stay heuristic and explainable.
- Keep prompts small and split broad work.
- Markdown report contracts come before SQLite/dashboard work.
- No hidden telemetry or network calls in MVP.

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
2. If the user asks for the next prompt, use `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`.
3. Read `docs/CONTEXT_INDEX.md` and the owning queue.
4. If Gate 0 is incomplete, select from `docs/prompt_queues/bootstrap_validation.md`.
5. Otherwise select one Ready prompt from the owning queue.
6. Inspect only the relevant docs/files.
7. Make the smallest safe change.
8. Add targeted tests when runtime behavior changes.
9. Run narrow validation when possible.
10. Record validation honestly.
11. Mark prompt `Done`, `Blocked`, or `Needs evidence sync`.
12. Commit and push to `main` unless the user requests another flow.

## Validation defaults

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

Do not claim validation passed unless it was actually run or CI evidence is available.
