# AgentsWatch Risk Register

Last aligned: 2026-06-29  
Status: active during bootstrap

## Current risk summary

| ID | Risk | Level | Why it matters | Mitigation |
|---|---|---|---|---|
| R-001 | Handwritten `.sln` may not build | High | Initial solution was created through GitHub contents API, not `dotnet new sln`. | Run `dotnet restore/build/test`; recreate solution locally if needed. |
| R-002 | Project references may be wrong | High | CLI, test, git/report projects depend on correct relative paths. | Validate with `dotnet build`; fix references before adding features. |
| R-003 | CLI smoke behavior unverified | High | `init`, `optimize`, and `status` were written without runtime execution. | Run smoke commands from `BUILD_VALIDATION_PLAN.md`. |
| R-004 | CI workflow unverified | Medium | GitHub Actions may fail due to path, SDK, or solution issues. | Check first CI run and fix only CI/build issues. |
| R-005 | Git parser too naive | Medium | `git status --short` parsing may not handle rename/copy paths correctly. | Add tests for deleted/renamed/copied/untracked paths. |
| R-006 | Prompt optimizer too heuristic | Medium | Risk analyzer can over/under-classify prompts. | Keep output transparent; add tests for common prompt patterns. |
| R-007 | `status` command assumes git repo | Medium | Running outside a git repo can fail. | Add graceful error or `--no-git` behavior in hardening pass. |
| R-008 | Init command may need overwrite policy | Medium | Existing `.ai` files should never be overwritten accidentally. | Add tests for no-overwrite behavior and later explicit `--force`. |
| R-009 | No packaging validation yet | Low | Tool may not pack/install as global tool yet. | Add `dotnet pack`/local tool install later, after build passes. |
| R-010 | Dashboard/SaaS scope creep | High | Building dashboard too early delays useful CLI. | Keep dashboard prompts blocked until CLI MVP evidence exists. |

---

## Required risk rule

Before implementing new runtime features, complete bootstrap validation:

1. build and test solution;
2. run CLI smoke commands;
3. verify CI result;
4. update `docs/prompt_queues/agentwatch_mvp.md` or validation queue with evidence.

---

## Risk report format

```text
Risk checked:
Evidence:
Files inspected:
Files changed:
Validation run:
Remaining risk:
Follow-up prompt:
```

---

## High-risk file categories

Treat these as high risk in the MVP:

- solution/project files;
- CI workflows;
- command dispatch in `Program.cs`;
- git command execution;
- file-system writes under `.ai` and `.agentwatch`;
- future validation command runner;
- future SQLite storage;
- future GitHub API integration.
