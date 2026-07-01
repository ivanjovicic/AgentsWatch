# Cross Repo Agent Standard Sync Evidence

Prompt ID: ad-hoc-cross-repo-agent-standard-sync  
Queue: docs/evidence/cross-repo standards  
Agent/tool: ChatGPT GitHub connector  
Model provider: OpenAI  
Model name/id: GPT-5.5 Thinking  
Model mode/settings: reasoning model, ChatGPT web with GitHub connector  
Client/IDE: ChatGPT web  
Run mode: docs/evidence  
Token budget: high  
Actual context: high  
Started from queue status: user requested comparing MathLearning backend, Flutter, and AgentsWatch docs and aligning useful agent/prompt rules  
Local collision check: not applicable - GitHub connector docs-only cross-repo update  
Relevant prior mistakes read: AW-MISTAKE-EVIDENCE-001, AW-MISTAKE-GATE-001, AW-MISTAKE-AUDIT-001, AW-MISTAKE-CONTEXT-001  
How this run avoids prior mistakes: added missing hard run-log gate, template, mistake ledger, lint prompt, rollup prompt, and shared standard to AgentsWatch  
Elapsed time: unknown-not-recorded  
Phase time breakdown: unknown-not-recorded

## Files inspected

- `AGENTS.md`
- `docs/DOCS_INDEX.md`
- `docs/AGENT_OPERATING_SYSTEM.md`
- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `docs/WASTE_LEARNING_LOOP.md`
- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/MISTAKE_LEARNING_SPEC.md`
- matching Flutter/backend process docs through GitHub connector

## Files changed in this repo

- `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `.ai/runs/README.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`
- `docs/ai/learning/MISTAKE_CARD_TEMPLATE.md`
- `docs/ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md`
- `docs/ai/prompts/RUN_LOG_EVIDENCE_LINT_PROMPT.md`
- `docs/ai/prompts/CROSS_REPO_AGENT_STANDARD_SYNC_PROMPT.md`
- `.ai/runs/2026-07-01-cross-repo-agent-standard-sync-evidence.md`

## Commands run

- GitHub fetch_file / create_file via connector.

## What was done

- Added AgentsWatch version of the shared cross-repo operating standard.
- Added the same strict `.ai/runs` hard gate used by MathLearning repos.
- Added run log template and run-log README.
- Added AgentsWatch mistake ledger and mistake-card template.
- Added lint and rollup prompts for evidence/mistake learning.
- Added cross-repo standard sync prompt.

## What was missed

- Did not update full `AGENTS.md` / `DOCS_INDEX.md` references in this pass.
- Did not run local `git diff --check` because this used GitHub connector writes.
- Did not run restore/build/test because no runtime code changed.

## Validation run

- Source docs were fetched before writing.
- GitHub create_file returned commit SHAs.

## Validation not run

- `git diff --check` not run locally.
- `dotnet restore`, `dotnet build`, and `dotnet test` not run; docs-only change.

## Waste categories

- AgentsWatch had strong productized learning specs but lacked matching hard-gate files;
- one large shared-standard write was blocked and rewritten as a smaller safer file;
- full index/rulebook reference sync deferred to focused follow-up.

## Mistakes observed

- Mistake ID: AW-MISTAKE-EVIDENCE-001
- New or repeated: repeated risk mitigated
- Root cause: evidence existed in broad docs/specs but not in the same `.ai/runs` hard gate shape
- Prevention added: run-log enforcement, template, run-log README, mistake ledger, lint prompt, rollup prompt
- Existing rule that should have prevented it: AgentsWatch run evidence standard and mistake learning spec
- Did this run update a rule/prompt/test/queue/lint: yes, added rule docs and lint/rollup prompts

## Where time/context was wasted

- Checking missing hard-gate paths one by one.
- Retrying a blocked large write.

## Why waste happened

- AgentsWatch had product design for mistake learning but not the exact dogfood file structure used by the MathLearning repos.

## What the next agent should avoid

- Do not treat AgentsWatch docs-only specs as runtime implementation.
- Do not start feature work before Gate 0 restore/build/test/smoke evidence is current.
- Use `.ai/RUN_LOG_TEMPLATE.md` for every non-trivial run.

## Docs/rules updated to prevent repeat

- `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `.ai/runs/README.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`
- `docs/ai/learning/MISTAKE_CARD_TEMPLATE.md`
- `docs/ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md`
- `docs/ai/prompts/RUN_LOG_EVIDENCE_LINT_PROMPT.md`
- `docs/ai/prompts/CROSS_REPO_AGENT_STANDARD_SYNC_PROMPT.md`

## Queue updated

- none

## New optimized prompt added

- `docs/ai/prompts/CROSS_REPO_AGENT_STANDARD_SYNC_PROMPT.md`
- `docs/ai/prompts/RUN_LOG_EVIDENCE_LINT_PROMPT.md`
- `docs/ai/prompts/AGENT_MISTAKE_ROLLUP_PROMPT.md`

## Follow-up prompt

- Focused AGENTS/DOCS_INDEX reference sync across all three repos.

## Completion %

82%

## Residual risk

- Shared standard and hard-gate files exist, but full `AGENTS.md` and `DOCS_INDEX.md` references are not fully synced yet.
- Gate 0 runtime validation remains separate and was not run in this docs-only sync.

## Commit SHA

Related AgentsWatch commits: `3562102`, `f0c75e1`, `b1fb75b`, `5e502c6`, `0fc6a12`, `18e0bf9`, `75502d5`, `fa23aa1`, `990508e`.
