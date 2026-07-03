# AgentsWatch Discovery and Self-Improvement Queue

Last aligned: 2026-07-03  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: durable out-of-scope findings and automatic learning follow-ups  
Parent router: `PROMPT_QUEUE_ROUTER.md`

## Purpose

Turn findings noticed during agent work into durable records, canonical documentation updates, focused prompts, and routed queue work without expanding the active task.

## Read first

- `../DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`
- `../DISCOVERY_INDEX.md`
- `../AGENT_RUN_LOG_ENFORCEMENT.md`
- `../AGENT_RUN_EVIDENCE_STANDARD.md`
- `../WASTE_LEARNING_LOOP.md`
- `../ai/learning/MISTAKE_LEDGER.md`
- `PROMPT_QUEUE_ROUTER.md`

## Rules

- Capture and route unrelated findings; do not implement them inside the current task.
- Reuse an existing discovery, mistake card, risk, prompt, or queue row when it has the same root issue.
- Every actionable discovery needs a primary owner and either a focused prompt or a documented no-op.
- Unknown or low-confidence issues start with investigation-only prompts.
- P0/P1 security, privacy, data-loss, release, or evidence findings require explicit review.
- Runtime CLI work in this queue remains blocked until Gate 0 restore/build/test/CLI smoke evidence exists.
- Local-first only; do not upload discovery records, run logs, prompts, diffs, or source content.

## Workflow prompts usable now

These prompts are docs/evidence workflows and may be used manually without claiming runtime automation:

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| DISC-001 | Ready | `../prompts/DISC-001-capture-run-discoveries.md` | Extract meaningful out-of-scope findings from one run. |
| DISC-002 | Ready after DISC-001 or when inbox has items | `../prompts/DISC-002-reconcile-discovery-inbox.md` | Deduplicate, classify, assign owners, and route findings. |
| DISC-003 | Ready for a reconciled documentation gap | `../prompts/DISC-003-promote-discovery-to-docs.md` | Promote durable knowledge to the correct canonical document. |
| DISC-004 | Ready for actionable reconciled findings | `../prompts/DISC-004-generate-follow-up-prompts.md` | Generate focused prompts and queue candidates. |
| DISC-005 | Ready when missed work lacks discovery IDs or after five runs | `../prompts/DISC-005-review-untracked-findings.md` | Find discoveries that were mentioned but never captured or routed. |
| DISC-006 | Ready during periodic maintenance | `../prompts/DISC-006-close-stale-discoveries.md` | Review stale/duplicate/resolved records and close with evidence. |

## Runtime implementation prompts

| ID | Status | Purpose |
|---|---|---|
| AW-DISC-001 | Blocked until AW-VAL-001/002 and init-hardening gate | Add discovery folders/templates to `agentswatch init` with non-overwrite tests. |
| AW-DISC-002 | Blocked until AW-DISC-001 | Implement markdown discovery model, parser, writer, and deterministic ID generation. |
| AW-DISC-003 | Blocked until AW-003 run reports and AW-DISC-002 | Extract structured discoveries from a run log and link them back to the run. |
| AW-DISC-004 | Blocked until AW-DISC-003 | Implement deterministic duplicate detection and owner/queue suggestions. |
| AW-DISC-005 | Blocked until AW-005 and AW-DISC-004 | Generate copy-ready prompts from reconciled discoveries. |
| AW-DISC-006 | Blocked until AW-DISC-005 | Add `agentswatch lint discoveries` and completion-gate checks. |
| AW-DISC-007 | Blocked until dogfood evidence exists | Add rollup, stale-item audit, and discovery metrics. |
| AW-DISC-008 | Backlog after supervised connector gates | Integrate discovery candidates into assisted queue execution with approval gates. |

## Exit criteria

The docs/manual slice is complete when:

- every non-trivial run template has discovery fields;
- a local inbox and record template exist;
- the router and learning queue point here;
- prompt files cover capture, reconciliation, documentation promotion, prompt generation, untracked-finding review, and stale closure;
- a real run log demonstrates the lifecycle.

The runtime slice is complete only when commands are implemented, tested, and dogfooded with evidence. Docs alone must not be described as runtime automation.
