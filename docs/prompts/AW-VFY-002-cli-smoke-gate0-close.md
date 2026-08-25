# AW-VFY-002 — CLI smoke and Gate 0 closure

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md`  
Status: Ready after AW-VFY-001  
Run mode: validation-first, minimal hardening only  
Budget: low  
Gate: AW-VFY-001 full test gate passes

## Read only

- `AGENTS.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/CLI_SPEC.md`
- `src/AgentsWatch.Cli/Program.cs`
- existing CLI/init tests if any

## Task

Prove the existing CLI skeleton end to end in disposable directories/repositories and close Gate 0.

Do not add verification MVP features in this prompt.

## Required smoke cases

Validate:

```text
agentswatch --help
agentswatch --version
agentswatch optimize <small inline prompt>
agentswatch init
agentswatch status
```

For `init`:

- use a temporary directory;
- verify expected `.ai` / `.agentwatch` paths;
- run `init` twice;
- verify existing user files are not overwritten;
- add/adjust automated temp-directory tests if coverage is missing.

For `status`:

- test a clean temporary git repo;
- test a dirty temporary git repo;
- test non-git behavior and either harden it minimally or record a precise blocker if the current contract is not satisfied.

## Owned paths

- `src/AgentsWatch.Cli/**`
- existing init/status supporting code only if needed
- `tests/AgentsWatch.Tests/**` for CLI/init/status smoke coverage
- Gate 0 evidence/status docs

## Avoid

- `contract`, `start`, `finish`, `receipt`, evidence/drift/claims features;
- prompt optimizer redesign;
- token economy work;
- dashboard/SaaS.

## Stop rules

- If smoke exposes an unrelated architectural problem, record and queue it rather than broad refactoring.
- If environment blocks a smoke case, record exact command/error and keep Gate 0 open.

## Validation

Run targeted smoke/tests, then:

```bash
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

If packaging/global-tool smoke is practical without publishing, validate `dotnet pack` or local tool install only if it remains within this prompt's scope.

## Expected evidence

- every smoke command and result;
- temp-dir/no-overwrite evidence;
- clean/dirty/non-git status result;
- full build/test result;
- files changed;
- explicit statement whether Gate 0 is closed.

## Completion rule

Only promote AW-VFY-003 when Gate 0 is fully green or when the queue/docs are updated with an explicit equivalent accepted exception.
