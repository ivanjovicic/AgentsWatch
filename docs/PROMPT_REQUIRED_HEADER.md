# AgentsWatch Required Prompt Header

Last aligned: 2026-06-29

## Purpose

A prompt without a strict header wastes tokens because the agent must infer scope, validation, and evidence rules.

## Required header

Every non-trivial prompt must start with:

```text
Repository:
Prompt ID:
Queue:
Run mode:
Token budget:
Gate:
Owned paths:
Avoid paths:
Validation:
Stop rules:
Evidence output:
```

## Field rules

### Repository

Must be exact.

Example:

```text
Repository: ivanjovicic/AgentsWatch
```

### Prompt ID

Must map to a queue or prompt file.

Example:

```text
Prompt ID: AW-VAL-001
```

### Queue

Must name the queue file.

Example:

```text
Queue: docs/prompt_queues/bootstrap_validation.md
```

### Run mode

Use exactly one:

```text
validation-only
investigation-only
implementation
tests
docs/evidence
review-only
diff-only review
```

### Token budget

Use one:

```text
low
medium
high
```

### Gate

Must state whether Gate 0 or another gate blocks the task.

### Owned paths

Files the agent may edit.

### Avoid paths

Files the agent must not edit.

### Validation

Exact command or explicit reason validation is not applicable.

### Stop rules

At least two stop rules required.

### Evidence output

Must require done/missed/waste/follow-up/residual risk.

## Enforcement

If the header is missing, rewrite the prompt before execution.
