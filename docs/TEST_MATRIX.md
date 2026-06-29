# AgentsWatch Test Matrix

Last aligned: 2026-06-29

## Purpose

Keep testing focused on the highest-risk CLI behavior.

## Required test areas

| Area | Required coverage |
|---|---|
| Init | creates folders, does not overwrite existing files, works in temp directory |
| Optimize | broad prompt is high risk, scoped prompt is low risk, missing scope/stop/validation is detected |
| Git parser | clean, modified, added, deleted, renamed, and untracked files |
| Reports | run report includes changed files, validation, missed work, follow-up, and risk |
| Handoff | compact summary includes relevant files, validation, next prompt, residual risk |
| Diff review | generated prompt is scoped to changed files only |
| Adapters | detects .NET, Flutter, React/TypeScript, Python, and Node projects |
| Config | missing config, minimal config, unknown fields, output path overrides |

## Default validation

```bash
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

## Targeted validation examples

```bash
dotnet test --filter PromptRiskAnalyzer
dotnet test --filter GitStatusParser
dotnet test --filter ProjectTypeDetector
dotnet test --filter Init
```

## Rules

- Use temp directories for file-system tests.
- Do not write to the real user home directory.
- Do not require network access for unit tests.
- Do not require LLM provider credentials for unit tests.
- Keep fixture repositories small.

## Done rule

A command is not done until it has targeted tests or an explicit follow-up prompt for missing tests.
