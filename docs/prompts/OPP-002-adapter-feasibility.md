# OPP-002 — Agent event adapter feasibility

Repository: `ivanjovicic/AgentsWatch`  
Queue: `community_opportunity_validation.md`  
Run mode: investigation-only  
Token budget: medium  
Capabilities: AW-CAP-028, AW-CAP-030, AW-CAP-032, AW-CAP-034

## Goal

Determine which local, documented, maintainable inputs AgentsWatch can use for Claude Code, Codex, and one additional coding-agent tool without relying on hidden APIs or uploading source.

## Scope

Inspect only:

- official documentation;
- public schemas and stable file formats;
- documented hooks/events;
- user-exported session or usage artifacts;
- explicit process-wrapper possibilities;
- tiny sanitized fixtures.

Avoid:

- reverse engineering credentials;
- undocumented private endpoints;
- collecting hidden reasoning;
- real private user logs;
- runtime implementation.

## Required capability matrix

For each adapter, determine:

```text
Can observe session start/resume
Can observe commands
Can observe exit codes
Can observe file reads
Can observe file writes
Can observe validation
Can observe token/usage events
Can observe compaction
Can observe approvals
Can observe subagents and lineage
Can observe worktree/cwd
Can export/resume context
Can stop or warn live
Source stability: Stable | Experimental | Unknown
Privacy risk
Maintenance risk
Missing evidence behavior
```

## Work

1. Identify exact documented data source.
2. Record format/version and sample fields.
3. Separate import-only from live-hook possibilities.
4. Identify gaps that cannot be observed.
5. Propose the smallest common event subset.
6. Define sanitized fixture requirements.
7. Recommend first and second adapters.
8. Create discoveries for unstable or unsafe dependencies.

## Validation

- every claimed field has a documented source;
- unsupported data remains unsupported;
- absence of an event is not treated as absence of behavior;
- no secrets or private source appear in fixtures;
- no runtime feature is marked implemented.

## Output

Adapter matrix, common event subset, source stability, privacy/maintenance risks, recommended import-only prototype, rejected approaches, and next prompt.
