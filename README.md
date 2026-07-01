# AgentsWatch

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

It helps developers run smaller, safer, cheaper AI coding tasks by splitting broad prompts, limiting scope, tracking git diffs, recording validation evidence, profiling expensive commands, logging post-prompt lessons, and generating compact handoff summaries.

## Core promise

```text
Spend fewer tokens. Merge safer AI code.
```

Practical target:

```text
Reduce AI coding-agent token waste by 30-50% on typical multi-file tasks by splitting prompts, limiting scope, tracking diffs, learning from run logs, and using compact handoff summaries.
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

Planned command profiler commands:

```bash
agentswatch run -- <command>
agentswatch validate --suggest
agentswatch validate --profile
```

Mistake-learning commands planned after the core run/report spine:

```bash
agentswatch mistakes list
agentswatch mistakes check <run-log>
agentswatch rollup mistakes --last 5
agentswatch lint evidence
agentswatch lint learning
```

## Post-prompt logging rule

Every agent run should leave compact evidence and one learning note.

See:

- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`
- `docs/MISTAKE_LEARNING_SPEC.md`
- `docs/CLI_LEARNING_ADDENDUM.md`
- `docs/prompts/LOG-001-post-prompt-run-log.md`
- `docs/prompts/LOG-002-mistake-pattern-review.md`
- `docs/prompts/LOG-003-flutter-agent-run-review.md`

## Supervised autopilot rule

AgentsWatch may sequence prompts, but should not run uncontrolled continuous autopilot in MVP.

See:

- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- `docs/prompts/AUTO-001-design-supervised-autopilot-queue.md`
- `docs/prompts/AUTO-002-generate-tool-prompt-envelope.md`
- `docs/prompts/AUTO-003-review-queued-agent-run.md`
- `docs/prompts/AUTO-004-manual-assisted-queue-runbook.md`

## Agent safety rule

Agents may suggest risky actions, but must not execute them without an explicit approval gate.

See:

- `docs/AGENT_RISK_BOUNDARIES.md`
- `docs/AGENT_PERMISSION_MODEL.md`
- `docs/prompts/SEC-001-agent-risk-boundary-audit.md`

## Bootstrap warning

The initial skeleton was created through GitHub file writes, so the next work must be validation-first:

1. run `AW-VAL-001` build validation;
2. run `AW-VAL-002` CLI smoke validation;
3. review validation evidence;
4. only then continue runtime feature work.

See:

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/RISK_REGISTER.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/prompt_queues/bootstrap_validation.md`

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
- Compact command summaries instead of full terminal logs.
- One learning note after every agent run.
- Risky actions require explicit approval gates.
- Supervised prompt sequencing before continuous autopilot.

## Current status

Planning and skeleton stage. See:

- `docs/PRODUCT_SPEC.md`
- `docs/CLI_SPEC.md`
- `docs/CLI_LEARNING_ADDENDUM.md`
- `docs/MVP_ROADMAP.md`
- `docs/FEATURE_PORTFOLIO_REVIEW_2026_06_30.md`
- `docs/FEATURE_SELECTION_SPEC.md`
- `docs/MISTAKE_LEARNING_SPEC.md`
- `docs/MISTAKE_LEARNING_ROADMAP_ADDENDUM.md`
- `docs/TEST_STRATEGY.md`
- `docs/ARCHITECTURE.md`
- `docs/PROMPT_OPTIMIZATION_PLAYBOOK.md`
- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`
- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`
- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- `docs/AGENT_RISK_BOUNDARIES.md`
- `docs/AGENT_PERMISSION_MODEL.md`
- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/prompt_queues/bootstrap_validation.md`
- `docs/prompt_queues/agentwatch_mvp.md`
- `docs/prompt_queues/agentwatch_foundation_followups.md`
- `docs/prompt_queues/agentwatch_feature_selection.md`
- `docs/prompt_queues/agentwatch_learning_followups.md`
- `docs/prompt_queues/agentwatch_testing.md`
- `docs/samples/README.md`
