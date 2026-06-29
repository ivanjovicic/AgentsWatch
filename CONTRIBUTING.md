# Contributing to AgentsWatch

Thanks for improving AgentsWatch.

## Current project stage

AgentsWatch is in bootstrap and CLI MVP planning. Build validation must come before new runtime feature work.

Read first:

- `README.md`
- `AGENTS.md`
- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/ROADMAP_INDEX.md`
- `docs/ROADMAP_VALIDATION_GATES.md`

## Development rules

- Keep changes small.
- Do not add dashboard/SaaS/cloud work before roadmap gates allow it.
- Add tests for new CLI behavior.
- Prefer markdown report contracts before storage/database changes.
- Keep risk scoring transparent and explainable.
- Do not add hidden telemetry or network calls.

## Validation

Default validation:

```bash
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

For bootstrap work, follow:

```text
docs/prompts/AW-VAL-001-build-validation.md
docs/prompts/AW-VAL-002-cli-smoke.md
```

## Pull request checklist

- [ ] Scope is small and clear.
- [ ] Tests added or missing tests documented.
- [ ] Build/test evidence included.
- [ ] Docs updated when behavior changed.
- [ ] No secrets or private data in reports.
- [ ] No dashboard/SaaS scope creep.
