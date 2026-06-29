# AgentsWatch Dogfood Plan

Last aligned: 2026-06-29

## Purpose

AgentsWatch should prove value on real repos before dashboard, SaaS, or paid features.

## Dogfood targets

Start with:

1. `ivanjovicic/AgentsWatch`
2. `ivanjovicic/Mathlearning-Mobile-App`
3. one .NET backend repo
4. one React/TypeScript repo if available

## First dogfood workflow

1. Write a rough prompt.
2. Run or manually apply prompt optimization.
3. Generate task split.
4. Run an AI agent with the investigation prompt.
5. Save run report.
6. Generate handoff summary.
7. Run implementation prompt.
8. Generate diff-only review prompt.
9. Record what was saved or caught.

## Evidence to collect

For each run:

- raw prompt;
- optimized prompt;
- files inspected if known;
- files changed;
- validation run;
- missed tests caught;
- scope creep prevented;
- handoff reused or not;
- follow-up prompt.

## Success threshold before dashboard

Do not build dashboard until there are at least:

- 5 real run reports;
- 2 repos used;
- 2 handoff summaries reused;
- 1 missed-test or scope-creep issue caught;
- 1 diff-only review that avoided whole-repo review.

## Example dogfood note

```text
Repo:
Task:
Raw prompt risk:
Optimized split:
Files changed:
Validation:
Waste avoided:
Issue caught:
Next improvement:
```
