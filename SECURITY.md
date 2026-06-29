# Security Policy

AgentsWatch is currently in bootstrap/MVP planning.

## Supported versions

No stable release exists yet. Security-sensitive findings should target the current `main` branch.

## Reporting a vulnerability

Do not paste secrets, tokens, private keys, or private source code into public issues.

For now, open a minimal GitHub issue that describes:

- the affected command or document;
- the risk category;
- safe reproduction steps without secrets;
- whether credentials or private data may have been exposed.

If a secret was committed or written to a report:

1. rotate the secret;
2. remove it from generated reports;
3. add or update a redaction rule;
4. document the mitigation.

## MVP security principles

- Local-first by default.
- No cloud sync in MVP.
- No telemetry by default.
- No hidden network calls.
- No LLM provider credentials required.
- Do not include secret values in reports.

See also `docs/SECURITY_AND_PRIVACY.md`.
