# Evidence Workflow Discoverability Hardening Evidence

Prompt ID: ad-hoc-evidence-workflow-discoverability-hardening  
Queue: docs/evidence/latest follow-up  
Agent/tool: ChatGPT GitHub connector  
Model provider: OpenAI  
Model name/id: GPT-5.5 Thinking  
Client/IDE: ChatGPT web  
Run mode: docs/evidence  
Token budget: medium  
Relevant prior mistakes read: AW-MISTAKE-EVIDENCE-001, AW-MISTAKE-GATE-001  
Elapsed time: unknown-not-recorded  
Phase time breakdown: unknown-not-recorded

## Files inspected

- `.github/workflows/agent-evidence-validation.yml`
- `docs/DOCS_INDEX.md`

## Files changed

- `.github/workflows/agent-evidence-validation.yml`
- `docs/DOCS_INDEX.md`
- `.ai/runs/2026-07-01-evidence-workflow-discoverability-hardening-evidence.md`

## What was done

- Added `python -m py_compile scripts/validate_agent_evidence.py` before running the validator workflow.
- Indexed `docs/prompt_queues/agent_evidence_validation_followups_2026_07_01.md` in `docs/DOCS_INDEX.md`.

## What was missed

- Timeout was not added to AgentsWatch workflow because the first broader workflow update was blocked; compile smoke was still added.
- Did not run GitHub Actions or local commands in this connector-only session.

## Validation not run

- not run - connector-only docs/workflow update, no local checkout.

## Mistakes observed

- Mistake ID: AW-MISTAKE-EVIDENCE-001
- New or repeated: repeated risk mitigated
- Root cause: validator/workflow queue was created but needed stronger discoverability and compile smoke
- Prevention added: docs index entry and workflow compile smoke
- Existing rule that should have prevented it: evidence validation gate
- Did this run update a rule/prompt/test/queue: yes, workflow and index

## Completion %

88%

## Residual risk

- Workflow has not been executed yet.
- AgentsWatch workflow still lacks timeout.

## Commit SHA

- `69da9f7`, `435e670`
