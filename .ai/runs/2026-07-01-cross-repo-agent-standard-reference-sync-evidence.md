# Cross Repo Agent Standard Reference Sync Evidence

Prompt ID: ad-hoc-cross-repo-agent-standard-reference-sync  
Queue: docs/evidence/cross-repo standards  
Agent/tool: ChatGPT GitHub connector  
Model provider: OpenAI  
Model name/id: GPT-5.5 Thinking  
Client/IDE: ChatGPT web  
Run mode: docs/evidence  
Token budget: high  
Relevant prior mistakes read: AW-MISTAKE-EVIDENCE-001, AW-MISTAKE-GATE-001, AW-MISTAKE-AUDIT-001, AW-MISTAKE-CONTEXT-001  
Elapsed time: unknown-not-recorded  
Phase time breakdown: unknown-not-recorded

## Files inspected

- `AGENTS.md`
- `docs/DOCS_INDEX.md`
- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `docs/WASTE_LEARNING_LOOP.md`
- `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`

## Files changed

- `AGENTS.md`
- `docs/DOCS_INDEX.md`
- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `docs/WASTE_LEARNING_LOOP.md`
- `.ai/runs/2026-07-01-cross-repo-agent-standard-reference-sync-evidence.md`

## What was done

- Updated AgentsWatch first-read rulebook to include shared standard, run-log gate, template, run-log README, and mistake ledger.
- Updated DOCS_INDEX to list the new evidence and mistake-learning docs.
- Updated AGENT_RUN_EVIDENCE_STANDARD so `.ai/runs` is canonical, not future-only.
- Updated WASTE_LEARNING_LOOP to require mistake classification and ledger handoff for repeated mistakes.

## What was missed

- Local `git diff --check` not run.
- `dotnet restore/build/test` not run because no runtime code changed.
- Prompt queue rows not changed.

## Validation run

- Source docs fetched before editing.
- GitHub writes returned commit SHAs.

## Validation not run

- `git diff --check` not run locally.
- `dotnet test` not run; docs-only change.

## Mistakes observed

- Mistake ID: AW-MISTAKE-EVIDENCE-001
- New or repeated: repeated risk mitigated
- Root cause: AgentsWatch evidence standard still allowed broad docs as evidence and did not make `.ai/runs` clearly canonical
- Prevention added: updated AGENTS, DOCS_INDEX, AGENT_RUN_EVIDENCE_STANDARD, and WASTE_LEARNING_LOOP
- Existing rule that should have prevented it: AgentsWatch evidence standard and mistake learning spec
- Did this run update a rule/prompt/test/queue/lint: yes, rule/docs updates

## Completion %

88%

## Residual risk

- Local diff validation and runtime validation were not run because this was connector-based docs-only work.

## Commit SHA

AgentsWatch commits: `92d8f87`, `dcbbbda`, `b0adfa4`, `8ebcd67`.
