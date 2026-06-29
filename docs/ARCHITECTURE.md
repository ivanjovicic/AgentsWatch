# AgentsWatch Architecture

Last aligned: 2026-06-29

## Goal

AgentsWatch starts as a local CLI that uses git, markdown files, config, and optional validation commands to supervise AI coding-agent runs.

## Project layers

```text
AgentsWatch.Cli
  command entry point and console UX

AgentsWatch.Core
  run modes, token budget, risk levels, prompt optimization models

AgentsWatch.Git
  git status/diff/snapshot access

AgentsWatch.LanguageAdapters
  project-type detection and validation command suggestions

AgentsWatch.Reports
  markdown run reports and handoff summaries
```

## Data flow

```text
raw prompt
  -> PromptRiskAnalyzer
  -> optimized prompt / suggested split
  -> agent run
  -> git snapshot / validation evidence
  -> markdown run report
  -> handoff summary / diff-only review prompt
```

## Local files

`agentswatch init` creates:

```text
.ai/config.yml
.ai/tasks/
.ai/runs/
.ai/STATUS.md
.ai/CHANGELOG_AI.md
.ai/REVIEW_CHECKLIST.md
.agentwatch/
```

## Design principles

- Markdown output first.
- Git evidence first.
- Heuristic risk scoring first.
- Local config before cloud settings.
- Prompt templates before LLM provider integration.
- Universal behavior before stack-specific behavior.

## Future storage

SQLite is planned for local history:

```text
.agentwatch/agentswatch.db
```

Tables later:

- projects;
- tasks;
- runs;
- changed_files;
- validations;
- risk_findings;
- generated_prompts;
- handoffs.
