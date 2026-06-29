# AgentsWatch Agent Rulebook

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

## Source of truth

1. Current code and tests.
2. `README.md`.
3. `docs/PRODUCT_SPEC.md`.
4. `docs/CLI_SPEC.md`.
5. `docs/MVP_ROADMAP.md`.
6. `docs/PROMPT_OPTIMIZATION_PLAYBOOK.md`.
7. `docs/ARCHITECTURE.md`.
8. `docs/prompt_queues/agentwatch_mvp.md`.

## Product rules

- Build local CLI first.
- Do not start with SaaS, billing, cloud sync, or dashboard.
- Universal git/markdown/file-system behavior comes before language adapters.
- Risk scoring must stay heuristic and explainable.
- Keep prompts small and split broad work.

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

1. Select one Ready prompt from `docs/prompt_queues/agentwatch_mvp.md`.
2. Claim it locally as `In progress`.
3. Inspect current code and tests.
4. Make the smallest safe change.
5. Add targeted tests.
6. Run narrow validation when possible.
7. Mark prompt `Done`, `Blocked`, or `Needs evidence sync`.
8. Commit and push to `main` unless the user requests another flow.

## Validation defaults

```bash
dotnet build AgentsWatch.sln
dotnet test tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj
```

Do not claim validation passed unless it was actually run or CI evidence is available.
