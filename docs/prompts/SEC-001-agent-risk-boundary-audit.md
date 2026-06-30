# SEC-001 — Agent risk boundary audit

Repository: `ivanjovicic/AgentsWatch`  
Prompt ID: `SEC-001`  
Run mode: review-only  
Token budget: low  
Permission mode: read_only  
Gate: security policy review only

## Purpose

Audit one prompt, roadmap item, or agent run for risky autonomous behavior.

## Minimum read

- item being audited;
- `docs/AGENT_RISK_BOUNDARIES.md`;
- `docs/AGENT_PERMISSION_MODEL.md`.

## Task

Decide whether the agent is allowed to proceed, needs approval, or must stop.

Check:

1. Does it touch sensitive paths?
2. Does it involve dangerous commands?
3. Does it involve secrets or command output?
4. Does it change auth/security/billing/migrations/infra?
5. Does it send data externally?
6. Does it attempt deploy/merge/release?
7. Does it exceed owned paths?
8. Is validation named?

## Output

```text
Risk result: safe/needs-approval/blocked
Permission mode:
Reasons:
Required approval:
Safer alternative:
Next prompt:
```

## Stop rules

Stop if:

- the requested action is destructive;
- secrets may be exposed;
- production systems may be changed;
- approval is vague or missing.
