# Dirty-Worktree Attribution Deep Dive

## Precise problem

The useful question is not “who wrote every line?” It is:

> **What repository delta occurred inside this explicitly recorded run boundary, given that the repository may already have staged, unstaged and untracked changes?**

## What a safe baseline should capture

At minimum:
- branch and HEAD;
- staged/index state;
- unstaged/worktree state;
- untracked file set;
- rename/delete semantics;
- content/diff fingerprints needed for start/end comparison;
- optional submodule state where supported.

A machine-safe NUL-delimited porcelain contract is preferable to trimmed line parsing.

## Edge cases

### Pre-existing dirty file unchanged
High-confidence classification: `PreExistingUnchanged`.

### Pre-existing dirty file changed further
Often detectable from start/end fingerprints as `PreExistingChangedFurther`.
Exact hunk/actor causality can remain ambiguous.

### New untracked file during interval
High confidence that the file appeared in the interval; not necessarily proof that the chosen agent created it if other actors were active.

### Rename/delete
Git can represent these reliably if parsing preserves machine-safe semantics.

### Partial staging
Index and worktree must be captured separately. A flattened status summary is insufficient.

### Binary files
Hashes can prove state change; semantic diff is limited.

### Submodules
Record gitlink/submodule state explicitly. Parent status cannot prove internal authorship.

### Generated/formatter changes
Interval-attributable but actor/intent may be ambiguous.

### Concurrent human/process/second agent
Git snapshots cannot prove causal actor. Result must be `Ambiguous` unless stronger executor instrumentation exists.

## Technical feasibility

Reliable **interval attribution** is technically achievable for many common Git cases. Reliable **causal authorship attribution** is not universally achievable from Git alone.

This distinction should be reflected in naming and docs.

## Commercial value

Dirty-state attribution is strongest as a foundation for:
- scope-drift checks;
- claims-vs-delta checks;
- required evidence;
- portable receipt/audit.

It is unlikely to justify a standalone business by itself.

## Candidate product-safety targets

These are hypotheses, not industry standards:
- material false attribution: target zero observed in dogfood;
- ambiguity should be surfaced rather than guessed;
- a false blocking decision caused by bad attribution is a major defect.
