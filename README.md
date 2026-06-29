# AgentsWatch

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

It helps developers run smaller, safer, cheaper AI coding tasks by splitting broad prompts, limiting scope, tracking git diffs, recording validation evidence, and generating compact handoff summaries.

## Core promise

```text
Spend fewer tokens. Merge safer AI code.
```

Practical target:

```text
Reduce AI coding-agent token waste by 30-50% on typical multi-file tasks by splitting prompts, limiting scope, tracking diffs, and using compact handoff summaries.
```

Use higher savings claims only for oversized repo-analysis prompts.

## MVP scope

Start with a local CLI. Do not start with SaaS, billing, cloud sync, deep IDE integration, or automatic code editing.

Initial commands:

```bash
agentswatch init
agentswatch optimize <prompt-file-or-text>
agentswatch task split <prompt-file>
agentswatch start <task-id>
agentswatch finish <task-id>
agentswatch report
agentswatch handoff
agentswatch review-diff <commit-or-range>
agentswatch validate
agentswatch status
```

## Repository layout

```text
src/
  AgentsWatch.Cli/
  AgentsWatch.Core/
  AgentsWatch.Git/
  AgentsWatch.LanguageAdapters/
  AgentsWatch.Reports/
tests/
  AgentsWatch.Tests/
docs/
.ai/templates/
```

## Development principles

- Local-first CLI before dashboard or SaaS.
- Git, markdown, and file-system behavior before LLM/API integrations.
- Universal repo behavior before language-specific adapters.
- Token budget and scope limiter for every non-trivial task.
- Investigation-only first for uncertain bugs.
- Diff-only review after implementation commits.
- Compact handoff summaries instead of long chat history.

## Current status

Planning and skeleton stage. See:

- `docs/PRODUCT_SPEC.md`
- `docs/CLI_SPEC.md`
- `docs/MVP_ROADMAP.md`
- `docs/PROMPT_OPTIMIZATION_PLAYBOOK.md`
- `docs/prompt_queues/agentwatch_mvp.md`
