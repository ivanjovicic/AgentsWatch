# Raw Task Sample

Source: migrated from `ivanjovicic/Mathlearning-Mobile-App/docs/agentwatch_samples/raw-task.md`

This is an intentionally broad prompt. AgentsWatch should flag it as high risk and split it before sending it to a coding agent.

```text
Fix the Admin Workers page. The Run Now button does not work. Check frontend, backend, logs, worker configuration, UI feedback, performance, and tests. Clean up anything related and make it production-ready.
```

## Expected AgentsWatch assessment

```text
Original prompt risk: HIGH
Estimated waste causes:
- broad multi-feature scope
- frontend + backend + logs + performance + tests in one run
- no owned paths
- no avoid paths
- no stop rules
- no investigation-only phase
- vague “production-ready” wording

Recommended split:
1. investigation-only
2. minimal implementation
3. targeted tests
4. diff-only review

Recommended budget: low for investigation, medium for implementation if backend is involved.
```

Do not send the raw prompt directly to an agent.
