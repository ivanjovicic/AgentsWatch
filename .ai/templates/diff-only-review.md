# Diff-only Review Prompt Template

Repository:

Run mode: diff-only review  
Token budget: low

## Review scope

Review only:

- changed files in commit/range: `<sha-or-range>`;
- listed diff hunks;
- validation evidence included in the run report.

Do not inspect the whole repo unless a changed file references a missing symbol or contract.

## Return

1. blocking issues;
2. missed tests;
3. risky scope creep;
4. claimed-vs-actual mismatch;
5. follow-up prompt if needed.

## Stop rules

Stop if review requires unrelated repo areas or a second implementation run.
