# AgentsWatch CLI Learning Addendum

Last aligned: 2026-06-30  
Status: planning/specification addendum  
Parent spec: [CLI_SPEC.md](CLI_SPEC.md)  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/AGENTWATCH_CLI_LEARNING_ADDENDUM.md`

## Purpose

This addendum defines the CLI surface for AgentsWatch mistake learning without expanding the first CLI skeleton too much.

The core CLI spec remains local-first. These commands are phased in after the basic task/run/report spine exists.

## Command group

Mistake-learning commands should live under a clear command group:

```bash
agentswatch mistakes list
agentswatch mistakes add
agentswatch mistakes check
agentswatch rollup mistakes --last 5
agentswatch lint evidence
agentswatch lint learning
```

Do not add cloud, SaaS, or remote policy behavior to these commands in the local MVP.

## Phase order

### Phase A — file generation

Add to `agentswatch init` only after lifecycle and privacy docs are accepted:

```text
.ai/RUN_LOG_TEMPLATE.md
.ai/learning/MISTAKE_LEDGER.md
.ai/learning/MISTAKE_CARD_TEMPLATE.md
.ai/learning/ROLLUP_TEMPLATE.md
.ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md
```

If the repo already has `docs/ai/learning/*`, do not duplicate without confirmation. Report detected existing learning docs and suggest linking.

### Phase B — check/list commands

```bash
agentswatch mistakes list
agentswatch mistakes check <run-log>
```

Minimum behavior:

- list known mistake IDs and titles;
- verify the run log has `Relevant prior mistakes read`;
- verify the run log has `Mistakes observed`;
- verify observed mistakes are classified as new, repeated, false alarm, or none.

### Phase C — add/update ledger command

```bash
agentswatch mistakes add --from-run <run-log>
```

Minimum behavior:

- create a new mistake card stub;
- require severity, status, root cause, prevention, next check;
- avoid duplicate IDs;
- preserve manual editing as the source of truth.

### Phase D — rollup and lint commands

```bash
agentswatch rollup mistakes --last 5
agentswatch lint evidence
agentswatch lint learning
```

Minimum behavior:

- summarize repeated mistakes across recent run logs;
- flag Done rows without run logs;
- flag completion-score/residual-risk contradictions;
- flag repeated mistakes without prevention updates;
- generate a next prevention prompt when needed.

## Data model additions

If using SQLite later, add tables after markdown works:

```text
mistakes
mistake_occurrences
prevention_actions
learning_rollups
```

Markdown remains canonical for early MVP because it is transparent, reviewable, and easy for agents to read.

## Validation examples

Docs/file-only implementation:

```bash
git diff --check
```

CLI command implementation:

```bash
dotnet test --filter Mistakes
dotnet test --filter EvidenceLint
```

Adjust test names to the actual future AgentsWatch solution.

## Non-goals

- Do not implement SaaS learning database.
- Do not upload mistake ledgers.
- Do not collect source code or diffs remotely.
- Do not block agents through a remote policy engine.
- Do not infer exact token usage unless provider data exists.
- Do not make mistake learning depend on a dashboard.
