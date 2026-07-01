# Run Log Evidence Lint Prompt

Use this before broad new feature work, after evidence backfills, or when queue status looks ahead of proof.

```text
Use only this repository:
ivanjovicic/AgentsWatch

Prompt ID:
AW-EVIDENCE-LINT-001

Run mode:
docs/evidence

Token budget:
low-medium

Goal:
Check AgentsWatch queue rows and recent `.ai/runs` logs for evidence, score, validation, and mistake-learning gaps.

Read first:
- docs/AGENT_SHARED_OPERATING_STANDARD.md
- docs/AGENT_RUN_LOG_ENFORCEMENT.md
- docs/ai/learning/MISTAKE_LEDGER.md
- .ai/RUN_LOG_TEMPLATE.md
- .ai/runs/README.md
- docs/prompt_queues/PROMPT_QUEUE_ROUTER.md

Inspect only:
- prompt queue rows marked Done, Blocked, or Needs evidence sync;
- `.ai/runs` logs referenced by those rows;
- recent docs/evidence commits if needed to verify paths.

Checks:
1. Done row has run-log path or explicit fallback.
2. Referenced run-log path exists.
3. Run log has model/client fields or `unknown-not-exposed`.
4. Run log has elapsed/phase fields or `unknown-not-recorded`.
5. Run log has validation run or skipped reason.
6. Completion score obeys caps.
7. Docs-only audit does not claim runtime feature/fix.
8. Mistakes observed are classified or explicitly `none`.
9. Repeated mistake includes a prevention update or documented no-op.
10. Gate 0 feature work is not marked runtime-complete without restore/build/test/smoke evidence.

Allowed edits:
- downgrade misleading rows to `Needs evidence sync`;
- add compact backfill logs using proven facts only;
- update `MISTAKE_LEDGER.md` if a repeated evidence mistake is confirmed;
- add a follow-up prompt when facts cannot be proven.

Avoid:
- runtime code edits;
- broad docs rewrites;
- inventing model names, timings, validation results, or CI status.

Validation:
- git diff --check
- verify changed run-log paths exist

Final response:
- rows checked
- rows fixed/downgraded
- logs created
- mistake IDs
- validation
- residual risk
- commit SHA
```
