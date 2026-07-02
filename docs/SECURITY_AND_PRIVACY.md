# AgentsWatch Security and Privacy Guide

Last aligned: 2026-07-02  
Status: draft guardrails

## Principle

AgentsWatch is local-first. The MVP must not upload source code, prompts, diffs, reports, secrets, command logs, or run history to a cloud service.

## Agent risk boundaries

Agents may suggest risky actions, but must not execute them without an explicit approval gate.

Authoritative policy docs:

- `docs/AGENT_RISK_BOUNDARIES.md`
- `docs/AGENT_PERMISSION_MODEL.md`
- `docs/prompts/SEC-001-agent-risk-boundary-audit.md`

Default posture:

- read-only first;
- local-first;
- minimal scope;
- no hidden network calls;
- no destructive operations;
- no secret exposure;
- no production changes;
- no autonomous merge/deploy/release;
- evidence before completion.

## MVP privacy rules

- No cloud sync.
- No account required.
- No telemetry by default.
- No LLM provider keys required.
- No automatic source upload.
- No automatic command-output upload.
- No automatic PR comments.
- No hidden network calls.

## Sensitive file handling

AgentsWatch should treat these as sensitive by default:

```text
.env
*.env
*.key
*.pem
secrets*.json
appsettings.Production.json
*.pfx
*.p12
```

Behavior:

- flag them as high risk if changed;
- do not include content in reports;
- include only path and risk reason;
- later allow user-configured redaction patterns.

## Report safety

Markdown reports should avoid full secret values, tokens, cookies, private keys, or connection strings.

If a command output contains sensitive-looking values, future versions should redact them before writing reports.

## Command output safety

AW-011 command profiling must follow these rules:

- do not write full stdout/stderr into markdown reports by default;
- do not paste full command logs into generated prompts by default;
- store duration, exit code, byte counts, and compact summaries instead;
- store only the first useful error line when needed;
- redact secret-looking values before writing `OutputSummary` or `FirstErrorLine`;
- do not persist or display raw secret-looking command strings;
- optional raw logs must be local-only and explicitly requested;
- raw logs must be size-limited and easy to delete;
- command profiles must not perform hidden network calls.

Sensitive-looking output examples to redact:

```text
password=...
Authorization: Bearer ...
connection strings
API keys
private keys
cookies
JWT tokens
```

## Network access

MVP commands should work offline except commands that explicitly interact with git remotes or future integrations.

Any future network feature must be opt-in and visible.

## Commercial licensing network exception — post-MVP

A future paid trial or Pro edition may use visible licensing network calls for:

- activation;
- short-lived lease refresh;
- device deactivation/recovery;
- revocation checks;
- signing-key rotation metadata;
- product edition and feature entitlement verification.

Licensing calls may send only the minimum commercial metadata needed, such as:

- license/trial/account identifier;
- product edition;
- client version;
- random installation identifier or derived device-binding hash;
- activation/lease nonce;
- usage allowance checkpoint where explicitly documented.

Licensing calls must not send:

- repository source code;
- file contents;
- raw prompts;
- git diffs;
- validation output;
- command logs;
- reports;
- run history;
- secrets;
- raw machine hardware inventory.

Rules:

- no hidden activation, refresh, or telemetry call;
- first activation and periodic lease behavior must be documented;
- licensing failure must not delete or encrypt user data;
- expired users retain read/export access to their own existing artifacts;
- client stores only public verification keys, never licensing private signing keys;
- protected local storage and signed entitlements are required; a plaintext local `trialEndsAt` is not authoritative;
- anti-copy protections are deterrence, not guaranteed secrecy.

See:

- `docs/TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- `docs/prompt_queues/agentwatch_trial_licensing.md`

## Future integrations

Before adding GitHub, LLM, licensing, or cloud integrations, document:

- what data is sent;
- why it is needed;
- how users disable or avoid it where feasible;
- where credentials are stored;
- how reports are redacted;
- what continues to work offline;
- what happens when the service is unavailable.

## Security issue handling

If a security issue is found:

1. do not paste secrets into issues or reports;
2. create a minimal reproduction without secrets;
3. rotate exposed credentials if any were committed;
4. document the mitigation in a security note.
