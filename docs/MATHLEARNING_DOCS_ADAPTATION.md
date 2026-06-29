# MathLearning Docs Adaptation Note

Last aligned: 2026-06-29

## Purpose

This document records which MathLearning documentation ideas were reused and how they were adapted for AgentsWatch.

## Reused ideas

From `ivanjovicic/Mathlearning-Mobile-App`:

- token budget and scope limiter;
- investigation-only mode;
- split broad prompts into smaller runs;
- long-task control loop;
- shell-neutral command guidance;
- prompt quality checklist;
- prompt evidence template;
- completion percentage and missed-work tracking;
- patch discipline and retry limits;
- docs index / context index pattern.

## AgentsWatch adaptations

MathLearning is a Flutter learning app. AgentsWatch is a .NET CLI product.

Therefore, Flutter-specific rules were replaced with:

- `.NET restore/build/test` validation;
- CLI smoke validation;
- config/report/data-model contracts;
- local-first privacy rules;
- adapter detection rules;
- bootstrap validation gates;
- dogfood requirements before dashboard/SaaS.

## Added AgentsWatch-specific docs

- `docs/AGENT_OPERATING_SYSTEM.md`
- `docs/CONTEXT_INDEX.md`
- `docs/AGENT_COMMAND_PLAYBOOK.md`
- `docs/AGENT_LONG_TASK_PLAYBOOK.md`
- `docs/AGENT_PATCH_PLAYBOOK.md`
- `docs/PROMPT_RULES.md`
- `docs/PROMPT_QUALITY_CHECKLIST.md`
- `docs/PROMPT_EVIDENCE_TEMPLATE.md`
- `docs/COMPLETION_ANALYTICS.md`

## Rule

Use MathLearning docs as inspiration, not authority. AgentsWatch authority is current code/tests plus AgentsWatch docs and validation gates.
