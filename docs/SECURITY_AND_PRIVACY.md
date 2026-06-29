# AgentsWatch Security and Privacy Guide

Last aligned: 2026-06-29  
Status: draft guardrails

## Principle

AgentsWatch is local-first. The MVP must not upload source code, prompts, diffs, reports, secrets, or run history to a cloud service.

## MVP privacy rules

- No cloud sync.
- No account required.
- No telemetry by default.
- No LLM provider keys required.
- No automatic source upload.
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

## Network access

MVP commands should work offline except commands that explicitly interact with git remotes or future integrations.

Any future network feature must be opt-in and visible.

## Future integrations

Before adding GitHub, LLM, or cloud integrations, document:

- what data is sent;
- why it is needed;
- how users disable it;
- where credentials are stored;
- how reports are redacted.

## Security issue handling

If a security issue is found:

1. do not paste secrets into issues or reports;
2. create a minimal reproduction without secrets;
3. rotate exposed credentials if any were committed;
4. document the mitigation in a security note.
