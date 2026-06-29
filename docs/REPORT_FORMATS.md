# AgentsWatch Report Formats

Last aligned: 2026-06-29  
Status: draft contract

## Purpose

AgentsWatch should produce small, readable markdown reports first. JSON and SQLite can come later.

Reports must be useful without the original chat history.

---

## Run report

Path:

```text
.ai/runs/<yyyy-mm-dd>-<sequence>-<task-id>.md
```

Template:

```markdown
# AgentsWatch Run Report — <task-id>

Started: <timestamp>
Finished: <timestamp>
Tool: <optional>
Model: <optional>
Run mode: <investigation-only|implementation|tests|review|docs>
Token budget: <low|medium|high>
Risk: <Low|Medium|High>

## Scope

Inspect only:
- <path>

Do not inspect:
- <path>

Owned paths:
- <path>

## Git evidence

Start commit: `<sha>`
End commit: `<sha or uncommitted>`
Branch: `<branch>`

Changed files:
- `<status>` `<path>`

## Validation

- `<command>`: pass/fail/not run

## Claims vs actual

Claimed:
- <claim>

Actual diff:
- <evidence>

Mismatches:
- <none or issue>

## Missed

- <missed item or none>

## Follow-up

- <next prompt or none>

## Token waste report

Files inspected: <n or unknown>
Files changed: <n>
Repeated searches: <n or unknown>
Broad commands avoided: <n or unknown>
Largest waste source: <text>
Next token-saving improvement: <text>
```

---

## Handoff summary

Path:

```text
.ai/runs/<run-id>-handoff.md
```

Template:

```markdown
# Handoff — <task-id>

Task:
Status:
Relevant files:
Files changed:
Root cause:
Validation run:
Validation blocked:
Do not inspect next:
Next minimal prompt:
Residual risk:
```

Keep handoff summaries short. Target 10-20 lines.

---

## Diff-only review prompt

Path:

```text
.ai/generated/<run-id>-diff-review.md
```

Template:

```markdown
# Diff-only Review Prompt

Repository:
Commit/range:
Run mode: diff-only review
Token budget: low

Review only:
- changed files in this commit/range
- validation evidence from the run report

Do not inspect the whole repo unless a changed file references a missing symbol or contract.

Return:
1. blocking issues
2. missed tests
3. risky scope creep
4. claimed-vs-actual mismatch
5. follow-up prompt if needed
```

---

## Status file

Path:

```text
.ai/STATUS.md
```

Should include:

- latest run id;
- latest commit;
- latest validation status;
- open risks;
- next prompt.

---

## AI changelog

Path:

```text
.ai/CHANGELOG_AI.md
```

Entry template:

```markdown
## <date> — <task-id>

Changed:
- <summary>

Validation:
- <command/result>

Risk:
- <low|medium|high>

Follow-up:
- <prompt or none>
```
