# AgentsWatch MVP Prompt Queue

Last aligned: 2026-08-25  
Status: **superseded historical queue**

This queue described the older MVP framing centered on a local agent supervisor/token optimizer and the AW-002/AW-011 sequence.

That ordering is no longer current product authority.

## Current product direction

AgentsWatch is now focused on:

```text
Task -> RunContract -> Start baseline -> external agent -> attributable delta -> RunReceipt -> Evidence/Scope/Claims verification
```

The active queue is:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

The router is:

```text
docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
```

## Historical mapping

The useful intent of older prompts is retained as follows:

| Old area | Current handling |
|---|---|
| AW-002 init hardening | Gate 0 / AW-VFY-002 if still needed. |
| AW-003 git diff/run report | Replaced by AW-VFY-004/005/006 with dirty-worktree-safe attribution and canonical receipts. |
| AW-004 basic risk scoring | Secondary; revisit after Evidence/Scope/Claims proof. |
| AW-005 prompt optimizer/split | Existing helper remains secondary; do not expand before verification spine. |
| AW-006 handoff | Implemented as receipt projection in AW-VFY-006. |
| AW-007 diff-only review | Optional post-MVP workflow after receipt verification. |
| AW-008 validation runner | Validation evidence comes first in AW-VFY-007; command execution/profiling later. |
| AW-009 claims-vs-actual | Replaced by AW-VFY-009 after trustworthy attribution/scope evidence exists. |
| AW-010 dashboard | Blocked until AW-VFY-010 dogfood proof. |
| AW-011 command profiler/validation economy | Post-dogfood candidate, not current MVP prerequisite. |

## Rule

Do not execute prompts from this file as current work unless they are explicitly re-promoted by a newer canonical queue.

The old content was intentionally removed from active selection to prevent agents from implementing the previous token-optimizer-first roadmap by mistake.
