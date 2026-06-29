# AgentsWatch Bootstrap Validation Queue

Last aligned: 2026-06-29  
Target repo: `ivanjovicic/AgentsWatch`

Purpose: reduce risk from the initial skeleton by validating build, tests, CLI smoke, and evidence before adding more runtime features.

## Read first

- `../../AGENTS.md`
- `../BUILD_VALIDATION_PLAN.md`
- `../RISK_REGISTER.md`
- `../ARCHITECTURE.md`

## Rules

- Do not add new product features until `AW-VAL-001` and `AW-VAL-002` are complete.
- Fix only build/test/smoke failures in this queue.
- If validation is blocked by local SDK/environment, record it clearly instead of changing runtime code.
- Keep every prompt low-budget and narrow.

## Active prompts

| ID | Status | Prompt file | Purpose |
|---|---|---|---|
| AW-VAL-001 | Ready | `../prompts/AW-VAL-001-build-validation.md` | Verify restore/build/test for the skeleton. |
| AW-VAL-002 | Ready | `../prompts/AW-VAL-002-cli-smoke.md` | Verify CLI help/version/optimize/status smoke behavior. |
| AW-VAL-003 | Ready | `../prompts/AW-VAL-003-evidence-review.md` | Review validation evidence and decide if feature work is safe. |
| AW-VAL-004 | Ready | `../prompts/AW-VAL-004-init-hardening.md` | Harden init command only after validation is known. |

## Exit criteria

Bootstrap validation is complete when:

- restore/build/test results are recorded;
- CLI smoke results are recorded;
- CI or validation evidence review is recorded;
- remaining risks are moved to `docs/RISK_REGISTER.md` or a follow-up prompt;
- `docs/prompt_queues/agentwatch_mvp.md` can continue at `AW-002` or later.
