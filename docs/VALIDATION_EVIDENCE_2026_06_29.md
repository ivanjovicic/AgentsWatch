# AgentsWatch Gate 0 Validation Evidence

Last aligned: 2026-06-29  
Scope: `AW-VAL-001` and `ROAD-001` bootstrap safety evidence

## Summary

Gate 0 is **not complete** yet.

Reason: executable `dotnet restore/build/test` and CLI smoke commands could not be run in the current environment because the local runtime available to this pass does not include the .NET SDK. GitHub status/workflow evidence is also not available for the current `main` commit through the accessible GitHub status/workflow queries.

No runtime features were added in this pass.

---

## Current main commit checked

```text
1db4be9e7e8224e7cf8f8abc3115192defc899c7
```

Commit title:

```text
Add security policy
```

---

## Commands requested

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

## Validation evidence

| Check | Status | Evidence |
|---|---|---|
| `dotnet restore AgentsWatch.sln` | Not run | Current execution environment has no local .NET SDK available. Requires local machine or CI. |
| `dotnet build AgentsWatch.sln` | Not run | Current execution environment has no local .NET SDK available. Requires local machine or CI. |
| `dotnet test AgentsWatch.sln` | Not run | Current execution environment has no local .NET SDK available. Requires local machine or CI. |
| CLI `--help` | Not run | Requires local .NET runtime. |
| CLI `--version` | Not run | Requires local .NET runtime. |
| CLI `optimize` smoke | Not run | Requires local .NET runtime. |
| CLI `status` smoke | Not run | Requires local .NET runtime and git repo context. |
| Combined status checks for current commit | No statuses returned | GitHub combined status for `1db4be9e7e8224e7cf8f8abc3115192defc899c7` returned an empty status list. |
| Workflow runs for current commit | No workflow runs returned | Available commit workflow query returned `workflow_runs: []` for the current commit. |

---

## Static preflight performed

This pass inspected the most likely build/test failure points without changing runtime code.

| Area | Static result |
|---|---|
| `AgentsWatch.sln` | Contains CLI, Core, Git, LanguageAdapters, Reports, and Tests projects. |
| `Directory.Build.props` | Defines `TargetFramework=net8.0`, nullable enabled, implicit usings enabled, latest language version. |
| CLI project | `Program.cs` exposes `init`, `optimize`, `status`, `--help`, and `--version` paths. |
| Core project | `PromptRiskAnalyzer` uses C# collection expressions/raw strings that require modern C#; covered by `LangVersion=latest`. |
| Git project | `GitStatusParser` and `GitSnapshotReader` are present. |
| Test project | Uses xUnit packages and references Core, LanguageAdapters, Git, and Reports projects. |
| CI workflow | `.github/workflows/ci.yml` exists and runs restore/build/test on .NET 8 for push and pull request. |

No obvious static `.sln`/`.csproj` path mismatch was found during this pass, but this is **not** a substitute for `dotnet restore/build/test`.

---

## Gate 0 decision

`ROAD-001` and `AW-VAL-001` remain **open**.

Do not continue to new runtime feature work until at least one of these is true:

1. local validation is run and recorded;
2. GitHub Actions CI run is visible and green;
3. validation failure is found and fixed with a narrow build/test/smoke fix.

---

## Remaining bootstrap risks

- `R-001`: handwritten `.sln` may not build.
- `R-002`: project references may be wrong despite static paths looking plausible.
- `R-003`: CLI smoke behavior is unverified.
- `R-004`: CI workflow is unverified for the current commit.
- `R-007`: `status` command behavior outside a git repo is still unverified.

---

## Next prompt

```text
Repository: ivanjovicic/AgentsWatch

Use docs/prompts/AW-VAL-001-build-validation.md.
Do not add features.
Run restore/build/test locally or through CI.
Commands:
- dotnet restore AgentsWatch.sln
- dotnet build AgentsWatch.sln
- dotnet test AgentsWatch.sln
Fix only build/test failures.
Record validation output in docs/VALIDATION_EVIDENCE_2026_06_29.md or a new dated evidence file.
Then run AW-VAL-002 CLI smoke validation.
```

---

## Token optimization applied

- Run mode: validation/evidence only.
- Runtime feature work: skipped.
- Scope: requested validation commands, current commit status/workflow evidence, static build-critical files, and validation docs.
- Stop reason: no executable .NET SDK available in this environment and no CI run evidence returned for current `main`.
