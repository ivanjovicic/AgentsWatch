# Agent Permission Model

Last aligned: 2026-06-30  
Status: draft policy contract

## Purpose

Define what an agent can do without asking, what requires approval, and what must be refused.

## Permission modes

### `read_only`

Agent may:

- inspect scoped files;
- summarize docs/code;
- produce plans;
- generate prompts;
- suggest commands.

Agent may not:

- edit files;
- run mutating commands;
- create commits or PRs;
- send data externally.

Default for uncertain tasks.

### `docs_only`

Agent may:

- edit markdown docs in owned paths;
- generate prompt files;
- update local planning files;
- create docs-only PRs.

Agent may not:

- change runtime code;
- change CI/CD;
- change secrets/config for production;
- merge without explicit instruction.

### `local_code`

Agent may:

- edit scoped runtime/test files;
- run targeted local validation;
- produce run reports;
- propose commits.

Agent may not:

- deploy;
- change production config;
- change infrastructure;
- run destructive commands;
- send code/logs externally.

### `elevated_risk`

Needed for:

- auth/security;
- migrations;
- billing/payments;
- CI/CD;
- deployment/infrastructure;
- public API contracts;
- secrets/config.

Requires explicit approval before edits and before risky commands.

### `blocked`

Must refuse or stop:

- destructive commands without explicit approval;
- secret exposure;
- hidden network calls;
- production deploys without approval;
- autonomous merge/release;
- commands outside repo scope.

## Prompt header field

Prompts should include:

```text
Permission mode: read_only|docs_only|local_code|elevated_risk|blocked
```

If omitted, default to `read_only` for investigation and `docs_only` for docs-only tasks.

## Runtime enforcement idea

Later CLI commands can enforce this through config:

```yaml
agent_policy:
  default_permission_mode: read_only
  require_approval_for:
    - ci_cd
    - auth_security
    - migrations
    - billing_payments
    - deployment_infra
    - secrets
    - external_network
    - destructive_commands
  block_by_default:
    - production_deploy
    - force_push
    - raw_secret_storage
    - hidden_telemetry
```

## Approval token idea

For high-risk actions, the agent should require an explicit approval phrase in the prompt or command invocation.

Example:

```text
Approved action: update GitHub Actions workflow for build validation only.
```

Approval must be specific. Generic approval such as `do whatever is needed` is not enough.
