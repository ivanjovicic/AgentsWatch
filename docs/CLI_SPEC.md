# AgentsWatch CLI Spec

Last aligned: 2026-06-29  
Status: planning/specification

## Recommended stack

Start as a `.NET global tool` because it is cross-platform and fits the expected developer stack.

Suggested solution layout:

```text
AgentsWatch.sln
src/AgentsWatch.Cli
src/AgentsWatch.Core
src/AgentsWatch.Git
src/AgentsWatch.LanguageAdapters
src/AgentsWatch.Reports
```

## CLI commands

```bash
agentswatch init
agentswatch optimize <prompt-file-or-text>
agentswatch task new
agentswatch task split <prompt-file>
agentswatch next
agentswatch start <task-id>
agentswatch finish <task-id>
agentswatch report
agentswatch handoff
agentswatch review-diff <commit-or-range>
agentswatch validate
agentswatch status
```

## Command behavior

### `agentswatch init`

Creates local files:

```text
.ai/
  config.yml
  tasks/
  runs/
  STATUS.md
  CHANGELOG_AI.md
  REVIEW_CHECKLIST.md
.agentwatch/
  agentswatch.db
```

### `agentswatch optimize`

Input: rough prompt.

Output:

- risk level;
- estimated waste causes;
- token budget;
- scope limiter;
- suggested split;
- generated markdown prompts.

### `agentswatch start`

Records:

- task id;
- optional tool/model;
- start time;
- git branch;
- start commit;
- `git status --short -uall`.

### `agentswatch finish`

Records:

- end time;
- changed files;
- `git diff --stat`;
- validation result;
- risk score;
- missed tests;
- handoff summary.

### `agentswatch review-diff`

Generates a review prompt scoped only to changed files in the commit/range.

## Config file

Root file example:

```yaml
project:
  name: Example Project
  types:
    - flutter

paths:
  root: .

validation:
  flutter:
    - flutter analyze
    - flutter test

risk:
  high:
    - "**/Auth/**"
    - "**/Security/**"
    - "**/Migrations/**"
    - "**/pubspec.yaml"
    - "**/*Provider.dart"
  medium:
    - "lib/services/**"
    - "lib/navigation/**"
    - "lib/offline/**"
    - "lib/widgets/ui/**"
```

## Language adapters

Universal adapter first:

- git status;
- git diff;
- changed files;
- prompt generation;
- risk report;
- changelog;
- handoff summary.

Stack adapters later:

- Flutter;
- .NET;
- React/TypeScript;
- Python;
- Node.

## Report outputs

Markdown first:

```text
.ai/runs/2026-06-29-001.md
.ai/CHANGELOG_AI.md
.ai/STATUS.md
```

Optional JSON later:

```text
.agentwatch/runs/2026-06-29-001.json
```
