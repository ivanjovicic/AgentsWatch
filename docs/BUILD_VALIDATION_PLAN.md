# AgentsWatch Build Validation Plan

Last aligned: 2026-06-29  
Status: required before new runtime features

## Why this exists

The initial repository skeleton was created through GitHub file writes, not through a local `dotnet new` / `dotnet build` workflow. That means the code can be structurally useful but still risky until build, restore, test, and CLI smoke commands are verified.

Do not add major new runtime features until this plan passes.

---

## Validation order

Run from repository root.

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

Then run CLI smoke checks:

```bash
dotnet run --project src/AgentsWatch.Cli -- --help
dotnet run --project src/AgentsWatch.Cli -- --version
dotnet run --project src/AgentsWatch.Cli -- optimize "Analyze the whole repo and fix everything"
dotnet run --project src/AgentsWatch.Cli -- status
```

If running on PowerShell, prefer one command per line. Do not chain with `&&` unless using a shell that supports it.

---

## What to fix first

If validation fails, fix in this order:

1. solution/project reference issues;
2. compile errors;
3. nullable/type errors;
4. test package or test discovery errors;
5. CLI command parsing smoke errors;
6. CI workflow path/version errors.

Do not add new commands before `restore`, `build`, `test`, `--help`, `--version`, and `optimize` smoke checks pass.

---

## CI validation

GitHub Actions file:

```text
.github/workflows/ci.yml
```

Expected CI steps:

1. checkout;
2. setup .NET 8;
3. restore;
4. build Release;
5. test Release.

If CI fails but local passes, inspect CI logs before changing code.

---

## Completion evidence format

When this plan is completed, record:

```text
Validation run:
- dotnet restore AgentsWatch.sln: pass/fail
- dotnet build AgentsWatch.sln: pass/fail
- dotnet test AgentsWatch.sln: pass/fail
- CLI smoke: pass/fail
- CI: pass/fail or not checked

Fixes made:
- <files>

Residual risk:
- <remaining issue or none>
```

---

## Stop rules

Stop and report instead of continuing if:

- NuGet restore fails for infrastructure/network reasons twice;
- solution file is broken enough that recreating it with `dotnet new sln` is safer;
- a test needs a large refactor unrelated to build validation;
- a command failure comes from missing local SDK rather than repo code;
- fixing validation would require changing product scope.
