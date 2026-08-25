# AgentsWatch Build Validation Plan

Last aligned: 2026-08-25  
Status: Gate 0 active

## Current known evidence

Latest known CI evidence already shows:

```text
restore: PASS
build: PASS
tests: FAIL
```

The current blocker is not an unknown solution/project-reference problem. It is the known Git status parser failure documented in `BOOTSTRAP_NEXT_STEPS.md`.

Current prompts:

```text
AW-VFY-001 — fix/harden git parser and make full test gate green
AW-VFY-002 — CLI smoke and close Gate 0
```

Canonical queue:

`docs/prompt_queues/verification_mvp_2026_08_25.md`

## Gate validation commands

Run from repository root:

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln --configuration Release --no-restore
dotnet test AgentsWatch.sln --configuration Release --no-build
```

During parser work, targeted tests may be used first, but Gate 0 requires the full test command above.

## CLI smoke after tests are green

Use disposable directories/repositories and validate:

```bash
dotnet run --project src/AgentsWatch.Cli -- --help
dotnet run --project src/AgentsWatch.Cli -- --version
dotnet run --project src/AgentsWatch.Cli -- optimize "Analyze the whole repo and fix everything"
dotnet run --project src/AgentsWatch.Cli -- init
dotnet run --project src/AgentsWatch.Cli -- status
```

Required smoke contexts:

- temporary empty directory for `init`;
- repeated `init` to verify no overwrite;
- clean temporary git repo for `status`;
- dirty temporary git repo for `status`;
- non-git directory for graceful status behavior.

Use shell-appropriate command syntax; do not assume `&&` on PowerShell.

## Current failure-specific validation

AW-VFY-001 must add/verify cases for:

- staged/unstaged modifications;
- add/delete;
- rename;
- untracked;
- filenames with spaces;
- no fixed-width prefix corruption.

Prefer lossless machine-safe git porcelain parsing, including NUL-delimited output where appropriate.

## Gate 0 completion evidence

Record:

```text
restore: pass/fail
build Release: pass/fail
full tests Release: pass/fail
CLI help/version: pass/fail
CLI optimize: pass/fail
CLI init temp/no-overwrite: pass/fail
CLI status clean/dirty/non-git: pass/fail
CI: pass/fail
files changed:
remaining risk:
```

## Completion rule

Do not start `AW-VFY-003` RunContract runtime work until Gate 0 is green or an explicit equivalent exception is documented and accepted.

## Stop rules

Stop and report rather than expanding scope if:

- validation failure is infrastructure/environmental and reproducibly unrelated to repo code;
- fixing a smoke failure requires a broad unrelated redesign;
- current code differs materially from the documented parser root cause;
- proposed fix weakens Git evidence correctness just to make a test pass.
