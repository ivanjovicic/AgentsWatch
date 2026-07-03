# AgentsWatch Discovery Inbox

This folder stores durable findings noticed during a run but not safely resolved inside that run's owned scope.

Use `../DISCOVERY_RECORD_TEMPLATE.md` and `../../docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`.

## Naming

Use `AW-DISC-<AREA>-<NNN>.md`.

Common areas include runtime, validation, docs, prompt, queue, context, security, architecture, product, and performance.

## Required behavior

- Capture a meaningful finding instead of expanding the current task.
- Check existing open records before creating another record.
- Link duplicates rather than creating parallel work.
- Give each triaged record one primary owner and one disposition.
- Add a focused follow-up prompt for actionable work, or record why none is needed.
- Link the originating run log, queue row, prompt, and final commit or pull request.
- Keep evidence summaries compact and local.

## Completion rule

A run is discovery-complete only when every finding is created and routed, updated and routed, linked as a duplicate, rejected with a reason, recorded as a no-op with a reason, or explicitly recorded as none found.

## Manual review until CLI support exists

Review records by status, severity, primary owner, age, missing prompt or queue links, missing validation, and likely duplicates.
