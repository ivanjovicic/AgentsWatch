# AgentsWatch Agent Command Playbook

Last aligned: 2026-06-29  
Status: command safety source-of-truth

## Purpose

Prevent shell-specific command failures and keep validation evidence clear.

Adapted from the MathLearning command playbook, but focused on .NET CLI, git, markdown reports, and AgentsWatch bootstrap validation.

---

## First rule: identify the shell

Use one command per line in shared docs unless a shell is explicitly named.

| Shell | Rule |
|---|---|
| Windows PowerShell 5.1 | Do not use `&&`; run one command at a time. |
| PowerShell 7+ | Prefer one command per line. |
| Bash / Git Bash / WSL / macOS | `&&` is allowed, but not preferred in shared prompts. |
| Windows CMD | `&&` works, but shared docs should stay shell-neutral. |

---

## Bootstrap validation commands

Run in this order:

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

If one fails, stop and report it. Do not run later commands as proof of success.

---

## CLI smoke commands

After build/test pass:

```bash
dotnet run --project src/AgentsWatch.Cli -- --help
dotnet run --project src/AgentsWatch.Cli -- --version
dotnet run --project src/AgentsWatch.Cli -- optimize "Analyze the whole repo and fix everything"
dotnet run --project src/AgentsWatch.Cli -- status
```

If `status` fails outside a git repo, record it as behavior to harden, not as proof that all CLI commands fail.

---

## Git defaults

Inspect state:

```bash
git status --short -uall
git diff --stat
git diff -- <path>
```

Check whitespace:

```bash
git diff --check
```

Prefer explicit adds:

```bash
git add docs/REPORT_FORMATS.md tests/AgentsWatch.Tests/FooTests.cs
```

Avoid unless explicitly requested:

```bash
git reset --hard
git clean -fd
git push --force
git rebase main
```

---

## Targeted test commands

```bash
dotnet test --filter PromptRiskAnalyzer
dotnet test --filter GitStatusParser
dotnet test --filter ProjectTypeDetector
dotnet test --filter Init
```

Run the narrowest test that proves the change first.

---

## Pack/install commands later

Use only after Gate 0 passes:

```bash
dotnet pack src/AgentsWatch.Cli/AgentsWatch.Cli.csproj
dotnet tool install --global --add-source <local-package-folder> AgentsWatch.Cli
```

Do not add packaging claims until pack/install are verified.

---

## Failure reporting

When a command fails, report:

```text
Command:
Exit/result:
Relevant output:
Likely cause:
Fix attempted:
Next command:
```

Never hide failed validation in a final answer.
