# AgentsWatch Config Reference

Last aligned: 2026-06-29  
Status: draft contract

## Purpose

`agentswatch.yml` or `.ai/config.yml` controls project metadata, validation suggestions, risk rules, and output locations.

The config must stay optional. AgentsWatch should work with sensible defaults when no config exists.

---

## Default location

Preferred generated file:

```text
.ai/config.yml
```

Optional root alias later:

```text
agentswatch.yml
```

If both exist, root `agentswatch.yml` should override `.ai/config.yml`.

---

## Minimal config

```yaml
project:
  name: Example Project
  types:
    - dotnet

paths:
  root: .

outputs:
  tasks: .ai/tasks
  runs: .ai/runs
  generated: .ai/generated
  status: .ai/STATUS.md
  changelog: .ai/CHANGELOG_AI.md
```

---

## Full draft config

```yaml
project:
  name: Example Project
  types:
    - dotnet
    - react

paths:
  root: .
  backend: src/Example.Api
  frontend: client

budgets:
  default: low
  low:
    max_files_to_inspect: 8
    max_files_to_edit: 3
  medium:
    max_files_to_inspect: 15
    max_files_to_edit: 6
  high:
    summarize_every_files: 10

validation:
  dotnet:
    - dotnet build
    - dotnet test
  react:
    - npm run build
    - npm test

risk:
  high:
    - "**/Migrations/**"
    - "**/*Auth*"
    - "**/*Security*"
    - "**/appsettings*.json"
  medium:
    - "src/**"
    - "lib/**"
    - "client/src/**"

outputs:
  tasks: .ai/tasks
  runs: .ai/runs
  generated: .ai/generated
  status: .ai/STATUS.md
  changelog: .ai/CHANGELOG_AI.md

privacy:
  redact:
    - "*.env"
    - "*.key"
    - "secrets*.json"
```

---

## Required parser behavior

- Missing config should not fail commands.
- Unknown fields should be ignored with a warning later, not fail immediately.
- Paths should support Windows and Unix separators.
- Generated config must not include secrets.
- `init` must not overwrite user config unless an explicit force flag exists later.

---

## Validation rules

Before implementing config parsing, add tests for:

- no config file;
- minimal config;
- unknown fields;
- Windows paths;
- output path overrides;
- no overwrite behavior.
