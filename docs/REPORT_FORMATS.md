# AgentsWatch Report Formats

Last aligned: 2026-06-30  
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

## Command profile

Slowest commands:
- `<command>`: <duration>, <status>, <compact error signature or none>

Recommended next validation:
- `<command>`

Avoid by default:
- `<command or rule>`

Command-output policy:
- full stdout/stderr omitted from report
- first useful error line recorded only if present

## Claims vs actual

Claimed:
- <claim>

Actual diff:
- <evidence>

Mismatches:
- <none or issue>

## Missed

- <missed item or none>

## Learning

Mistakes:
- <category or none>

Token waste:
- <over-read/over-test/repeated failure/none>

Learning note:
- Next time, avoid <waste/mistake> by <specific rule>.

Do not repeat:
- <specific repeatable mistake or none>

## Follow-up

- <next prompt or none>

## Token waste report

Files inspected: <n or unknown>
Files changed: <n>
Repeated searches: <n or unknown>
Broad commands avoided: <n or unknown>
Large logs avoided: <n or unknown>
Largest waste source: <text>
Next token-saving improvement: <text>
```

---

## Post-prompt learning section rules

The learning section is controlled by `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`.

Rules:

- every run should produce one compact learning note;
- learning notes must be specific and repeatable;
- do not add vague rules such as `be better` or `read everything`;
- do not include full chat history;
- do not include full terminal logs;
- do not include secrets;
- add `Do not repeat` only for mistakes likely to happen again;
- prefer token-saving rules that reduce future reads, tests, logs, or prompt size.

Example:

```markdown
## Learning

Mistakes:
- OverRead
- FlutterStateRisk

Token waste:
- Read unrelated navigation docs for a provider-only change.

Learning note:
- Next time, for provider-only Flutter changes, inspect changed provider files and Flutter adapter only before reading navigation docs.

Do not repeat:
- Do not run full Flutter integration validation for a provider-only change unless persistence/navigation files changed.
```

---

## Command profile section rules

The command profile section is controlled by `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`.

Rules:

- include duration, status, and compact error signature;
- include stdout/stderr byte counts only when helpful;
- do not include full stdout/stderr by default;
- do not include secret-looking values;
- recommend the next smallest useful validation command;
- mark command evidence as `not available` if no command was profiled.

Example:

```markdown
## Command profile

Slowest commands:
- `dotnet test AgentsWatch.sln`: 42.3s, fail, `CS0246`
- `dotnet test --filter Git`: 3.8s, pass, none

Recommended next validation:
- `dotnet build --no-restore`
- `dotnet test --filter Git`

Avoid by default:
- full solution test unless project references or shared contracts changed

Command-output policy:
- full stdout/stderr omitted from report
- first useful error line recorded only if present
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
Command profile:
Learning note:
Do not repeat next:
Do not inspect next:
Next minimal prompt:
Residual risk:
```

Keep handoff summaries short. Target 10-20 lines.

Command profile in handoff should be one or two lines only, for example:

```text
Command profile: targeted `dotnet test --filter Git` passed in 3.8s; avoid full solution test unless project refs changed.
```

Learning note in handoff should be one line only, for example:

```text
Learning note: next Flutter prompt should stay provider-only; do not inspect navigation unless changed files require it.
```

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
- compact command profile evidence from the run report
- learning note and do-not-repeat items from the run report

Do not inspect the whole repo unless a changed file references a missing symbol or contract.
Do not request full command logs unless the compact error signature is insufficient.

Return:
1. blocking issues
2. missed tests
3. risky scope creep
4. claimed-vs-actual mismatch
5. repeated mistake or token waste pattern
6. follow-up prompt if needed
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
- latest command profile summary if available;
- latest learning note;
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

Command profile:
- <slowest command or targeted validation evidence, if available>

Learning:
- <one useful learning note>

Risk:
- <low|medium|high>

Follow-up:
- <prompt or none>
```
