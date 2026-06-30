# Supervised Autopilot Queue

Last aligned: 2026-06-30  
Status: product/architecture plan; docs-only

## Purpose

Define a safe way for AgentsWatch to execute multiple prompts in sequence without letting agents take risky actions on their own.

Core idea:

```text
Autopilot means supervised prompt sequencing, not uncontrolled autonomous execution.
```

## Feasibility

A fully automatic autopilot depends on the external tool:

- some coding agents can run multiple tasks or background sessions;
- some IDE agents require manual prompt submission;
- some tools expose CLI/API/integration points;
- some tools only support human-driven UI flows.

AgentsWatch should not rely on UI automation or browser/keyboard hacks.

MVP should support a tool-agnostic queue that can be used in three modes:

```text
manual_queue     -> user copies/runs the next prompt manually
assisted_queue   -> AgentsWatch prepares next prompt and approval checklist
connector_queue  -> future integration opens tasks/PRs where an official API supports it
```

## Non-goal

AgentsWatch must not become an uncontrolled bot that clicks through Codex, Cursor, or any IDE UI.

Do not implement:

- hidden UI automation;
- credential scraping;
- browser session hijacking;
- unofficial remote Codex/Cursor wrappers;
- bypassing approval dialogs;
- autonomous deploy/merge/release loops.

## Queue item contract

Each queued prompt must have:

```text
ID:
Title:
Status:
Permission mode:
Run mode:
Token budget:
Model/tool recommendation:
Gate:
Owned paths:
Avoid paths:
Prompt file:
Validation:
Stop rules:
Approval required:
Depends on:
Produces:
```

If any required field is missing, the queue item is not executable.

## Queue statuses

```text
Planned
Ready
Running
NeedsReview
NeedsApproval
Blocked
Failed
Done
Skipped
```

Only `Ready` items may be selected for execution.

## Execution loop

```text
1. Load queue.
2. Select first Ready item whose dependencies are Done.
3. Check permission mode and risk boundaries.
4. Generate tool-specific prompt envelope.
5. Require approval if the item is elevated risk.
6. Execute manually, assisted, or through an approved connector.
7. Capture evidence: changed files, validation, report, handoff.
8. Run self-review.
9. Update queue status.
10. Stop unless next item is safe and explicitly allowed by autopilot policy.
```

## Autopilot levels

### Level 0 — manual queue

AgentsWatch only generates the next prompt.

User copies it into Codex/Cursor/Claude/Copilot manually.

Allowed now.

### Level 1 — assisted queue

AgentsWatch prepares:

- next prompt;
- model/tool recommendation;
- validation command suggestion;
- approval checklist;
- expected output/evidence shape.

User still starts the agent manually.

Allowed after prompt queue docs are stable.

### Level 2 — supervised connector queue

AgentsWatch may open a task through an official integration or connector.

Required gates:

- explicit user enablement;
- per-tool config;
- scoped repo permissions;
- no secrets in prompt;
- risk boundary check;
- no elevated-risk task without approval;
- evidence required after run.

### Level 3 — continuous autopilot

AgentsWatch selects and starts the next safe item automatically.

Blocked for MVP.

May be considered only after:

- robust queue state exists;
- permission model is enforced;
- command profiling and validation evidence are reliable;
- rollback/stop behavior is tested;
- external integrations are official and opt-in;
- human approval gates are proven.

## Codex/Cursor handling

AgentsWatch should treat Codex, Cursor, Claude Code, Copilot, and similar tools as execution targets, not as trusted controllers.

Tool-specific integration should be adapter-based:

```text
Queue item -> Prompt envelope -> Tool target -> Evidence collector -> Self-review
```

Never assume a tool can safely accept an unbounded queue.

## Prompt envelope

Every tool-targeted prompt should include:

```text
AgentsWatch queue item:
Permission mode:
Run mode:
Scope:
Owned paths:
Avoid paths:
Validation:
Stop rules:
Approval required:
Final response format:
```

## Stop conditions

Autopilot must stop when:

- validation fails;
- changed files exceed owned paths;
- sensitive paths are touched;
- command output may contain secrets;
- permission mode becomes elevated risk;
- dependencies are not Done;
- the next prompt would need more context than budget allows;
- the agent asks to inspect the whole repo;
- the agent asks to deploy, merge, release, force push, or change production;
- the user has not explicitly enabled the next autopilot step.

## Model/tool selection

Autopilot should recommend model/tool class, not hard-code vendor names.

```text
planning          -> strong reasoning model
one-file fix      -> fast coding model
debug/root cause  -> strong reasoning + code search
multi-file change -> strong coding model with medium budget
diff review       -> reviewer model, low budget
validation triage -> cheap summarizer, low budget
security boundary -> cautious reviewer, read-only
```

Rule:

```text
Use the cheapest model/tool that can satisfy the queue item's risk and evidence requirements.
```

## Evidence requirement

A queue item is not Done unless there is:

- changed-file summary or docs-only note;
- validation result or blocked reason;
- risk result;
- missed work;
- next prompt or completion note.

## Recommended storage

Markdown first:

```text
.ai/queue/AUTOPILOT_QUEUE.md
.ai/queue/RUNBOOK.md
.ai/queue/HANDOFF.md
```

JSON later:

```text
.agentwatch/autopilot-queue.json
.agentwatch/autopilot-runs.jsonl
```

## Security rule

Supervised autopilot inherits:

- `docs/AGENT_RISK_BOUNDARIES.md`;
- `docs/AGENT_PERMISSION_MODEL.md`;
- `docs/COMMAND_PROFILE_PRIVACY_AND_STORAGE_POLICY.md`;
- `docs/SECURITY_AND_PRIVACY.md`.

If policies conflict, the safer policy wins.

## MVP recommendation

Implement documentation and prompt generation first:

```text
AUTO-001 — design queue contract
AUTO-002 — generate prompt envelopes
AUTO-003 — self-review after queued run
AUTO-004 — manual/assisted queue runbook
```

Do not implement continuous autopilot in MVP.
