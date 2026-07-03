# Discovery CLI Contract

Last aligned: 2026-07-03  
Status: planned; implementation blocked by Gate 0

## Commands

```bash
agentswatch discover add --from-run <run-id>
agentswatch discover list [--status <status>]
agentswatch discover reconcile <run-id|discovery-id|--all-open>
agentswatch discover prompt <discovery-id>
agentswatch discover close <discovery-id>
agentswatch lint discoveries
agentswatch finish <task-id> --learn --reconcile
agentswatch next --from-discoveries
```

## Deterministic behavior

The first implementation should work without an external model:

- parse structured run-log sections;
- create local discovery records;
- generate stable IDs;
- match likely duplicates by normalized title, category, and affected paths;
- validate required fields and links;
- generate prompts from local templates;
- emit queue candidates in truthful Planned or Blocked states.

Optional model assistance may later suggest category, confidence, owner, or wording, but deterministic rules and user-editable files remain authoritative.

## Finish integration

`finish --learn --reconcile` should:

1. write or validate run evidence;
2. extract discovery candidates;
3. update or create local records;
4. report unresolved or unowned items;
5. generate prompt candidates when configured;
6. refuse learning-complete status when reconciliation is missing.

## Output

Commands should return compact summaries with IDs and paths, not full run logs or diffs.

## Gate

Implement through `AW-DISC-001` to `AW-DISC-007` only after restore, build, test, CLI-smoke, and owning dependency evidence permit runtime work.
