# AgentsWatch Sample Workflow

Last aligned: 2026-06-29  
Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/agentwatch_samples/README.md`

This folder shows how AgentsWatch should work before the CLI exists.

Use these files as manual dogfood examples:

| File | Purpose |
|---|---|
| `raw-task.md` | Example broad prompt that should not be sent directly to an agent. |
| `001-investigate.md` | Optimized investigation-only prompt. |
| `002-implement.md` | Minimal implementation prompt that uses the investigation handoff. |
| `003-tests.md` | Targeted test prompt. |
| `004-review.md` | Diff-only review prompt. |
| `sample-run-report.md` | Example report AgentsWatch should generate after a run. |
| `sample-handoff-summary.md` | Compact handoff for the next agent run. |

Manual workflow:

```text
1. Write raw task.
2. Optimize/split it into 001-004 prompts.
3. Run 001 investigation-only.
4. Save handoff.
5. Run 002 implementation using only handoff files.
6. Run 003 tests.
7. Run 004 diff-only review.
8. Write run report and changelog.
```

Rules:

- Do not paste the full chat into the next run.
- Do not review the whole repo when a diff-only review is enough.
- Do not let implementation start before root cause is known for uncertain bugs.
- Keep each prompt under a clear token budget.
