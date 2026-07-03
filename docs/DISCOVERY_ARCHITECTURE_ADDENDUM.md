# Discovery Architecture Addendum

Last aligned: 2026-07-03

AgentsWatch adds a local discovery lane after each run report:

```text
run evidence
  -> discovery capture
  -> duplicate check
  -> classification and owner routing
  -> documentation or prompt candidate
  -> queue link
  -> next supervised run
```

Phase 1 uses markdown under `.ai/discoveries/`.

Future local storage may add `discoveries`, `discovery_links`, `discovery_events`, and `prompt_candidates` tables.

Runtime implementation is divided into the `AW-DISC-*` prompts in `prompt_queues/agentwatch_discovery_and_self_improvement.md` and remains gated by bootstrap validation.

The discovery lane records and routes work outside the active task. It does not expand the current task or claim that queued work has been implemented.
