# Example Rough Prompt — Broad Repo Fix

## Unsafe prompt

```text
Analyze the whole repo, make AgentsWatch production-ready, fix the CLI, improve docs, add tests, add packaging, and review everything.
```

## Why this is risky

- broad repo scope;
- multiple run modes mixed together;
- no token budget;
- no owned paths;
- no avoid paths;
- no validation commands;
- no stop rules;
- no definition of Done.

## Expected AgentsWatch response

AgentsWatch should classify this as high risk and recommend splitting it into:

1. investigation-only;
2. minimal implementation;
3. targeted tests;
4. diff-only review.
