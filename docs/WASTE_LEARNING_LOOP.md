# AgentsWatch Waste Learning Loop

Last aligned: 2026-06-29

## Purpose

Turn every wasted step into a better rule, better prompt, or better queue decision.

If the same mistake happens twice, the documentation or prompt system is failing.

## Loop

```text
1. Run prompt.
2. Record evidence.
3. Identify waste.
4. Find root cause.
5. Update rule/doc/queue.
6. Add optimized prompt.
7. Mark follow-up.
8. Reuse learning in next run.
```

## What counts as waste

Waste is any action that used time/tokens without moving the task closer to safe completion.

Common examples:

- reading broad docs when a context index existed;
- starting feature work before Gate 0;
- retrying a blocked write with the same shape;
- asking for whole-repo analysis when diff-only review was enough;
- running a command after an earlier required command failed;
- claiming validation without evidence;
- editing docs but not updating `DOCS_INDEX.md`;
- adding a plan without a runnable next prompt.

## Root cause categories

Use one:

- prompt missing scope;
- prompt missing stop rule;
- prompt missing validation;
- queue status misleading;
- docs reference stale;
- command environment unknown;
- tool limitation not recognized;
- output too large;
- no evidence template used;
- no router consulted.

## Required remediation

For every major waste item, choose one:

| Waste cause | Required remediation |
|---|---|
| wrong queue selected | update router or queue status |
| stale docs reference | fix reference and update docs governance if needed |
| repeated blocked write | split into smaller files or record stop rule |
| missing validation | update prompt with validation command |
| broad scope | add scope limiter or split prompt |
| missed follow-up | add optimized follow-up prompt |
| missing evidence | add evidence entry and template reference |

## Optimized prompt generation

A good optimized follow-up prompt should be shorter and more specific than the original.

It must name:

- exact queue;
- exact run mode;
- exact owned paths;
- what not to inspect;
- what not to edit;
- validation command;
- evidence output.

## Review cadence

After every 5 completed runs, review evidence entries and ask:

1. Which waste category repeats?
2. Which prompt caused the most wasted work?
3. Which docs were missing or stale?
4. Which command failed most often?
5. Which rule should become mandatory?

## Product implication

This loop is not only development hygiene. It is a core AgentsWatch product feature.

Future CLI commands should support it directly:

```text
agentswatch finish --retrospective
agentswatch waste report
agentswatch prompt improve
```
