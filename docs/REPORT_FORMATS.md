# AgentsWatch Report Formats

Last aligned: 2026-08-25  
Status: active projection contract

## Core rule

Reports are human-readable projections of canonical structured state.

```text
.agentwatch/runs/<run-id>.json = source of truth
.ai/runs/<run-id>.md = generated run report
.ai/handoffs/<run-id>.md = generated compact handoff
```

Do not store information only in Markdown when verification logic needs it later.

## Run report

Path:

```text
.ai/runs/<run-id>.md
```

Recommended template:

```markdown
# AgentsWatch Run Receipt — <run-id>

Task: <task-id>
Contract: <contract-id>
Started: <timestamp>
Finished: <timestamp>
Agent/tool/model: <known values or unknown>
Decision: <Done|NeedsEvidence|NeedsReview|NeedsApproval|Blocked|Failed>

## Intent

<intent>

## Acceptance criteria

- [supported|unsupported|unknown] <criterion> — <reason/evidence>

## Repository attribution

Start branch/commit: <branch> / <sha>
End branch/commit: <branch> / <sha>

Attributable changes:
- `<status>` `<path>`

Pre-existing unchanged changes:
- `<status>` `<path>`

Pre-existing changed further:
- `<status>` `<path>` — <attribution reason>

Attribution ambiguities:
- `<path>` — <reason>

## Validation evidence

- `<status>` `<command/display>` — source: <source>, <summary>

## Claims

- `<claim type>`: <supported|unsupported|unknown> — <reason>

## Scope findings

- <none or exact path + matched rule/reason>

## Other findings

- <category/severity/status/reason>

## Decision reasons

- <reason>

## Missed work

- <item or none>

## Learning note

<specific compact note or none>

## Next prompt

<next minimal prompt or none>
```

Rules:

- do not dump raw full terminal logs;
- do not dump source contents;
- do not present agent claims as facts without support status;
- visibly distinguish attributable vs pre-existing/ambiguous files;
- decision reasons must match canonical receipt findings;
- `unknown` is allowed and should remain visible.

## Handoff

Path:

```text
.ai/handoffs/<run-id>.md
```

Target: roughly 10–20 lines for a normal run.

Recommended shape:

```markdown
# Handoff — <task-id>

Decision: <status>
Intent: <one line>
Attributable files: <compact list/count>
Validation: <compact evidence summary>
Unsupported/unknown claims: <compact summary>
Scope findings: <compact summary>
Acceptance gaps: <compact summary>
Residual risk: <one line>
Learning note: <one line>
Next minimal prompt: <one line>
```

Do not copy full chat/session history.

## Status projection

`.ai/STATUS.md` may summarize:

- active run ID if any;
- latest receipt ID;
- latest decision;
- open verification findings;
- next safe prompt.

It is a convenience projection, not canonical state.

## AI changelog projection

`.ai/CHANGELOG_AI.md` may contain compact entries generated from completed receipts:

```markdown
## <date> — <task-id>

Decision: <status>
Attributable changes:
- <summary>

Validation:
- <summary>

Findings:
- <summary>

Follow-up:
- <next prompt or none>
```

## Command-profile section — later

When command profiling exists, receipt/report may project compact command evidence:

- command display;
- duration;
- exit/status;
- byte counts if useful;
- first useful error line;
- output summary.

Full stdout/stderr stays excluded by default.

Command-profile work must not become a prerequisite for the first RunReceipt/Evidence implementation.

## Learning section — later expansion

One compact learning note may be included from the start.

Sophisticated mistake pattern/expiry/confidence sections should be added only after trustworthy dogfood receipts exist.

Avoid generic notes such as `be better`. Prefer evidence-backed, scoped statements.

## Compatibility rule

Every report/handoff/status/changelog projection must be regenerable from canonical structured models or clearly labeled as supplemental user-authored text.

Downstream verification must never require scraping these Markdown files.
