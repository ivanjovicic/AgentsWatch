# Command Profile Privacy and Storage Policy

Last aligned: 2026-06-30  
Status: policy override for AW-011 implementation

## Purpose

This policy closes the two most important command-profile safety gaps before runtime implementation:

1. command strings can contain secrets;
2. command-history storage phase must be consistent.

## Command string policy

A profiled command may contain credentials in arguments or inline environment variables.

Examples:

```text
TOKEN=... agentswatch run -- curl ...
Authorization: Bearer ...
password=...
--api-key ...
```

AgentsWatch must not persist or display raw secret-looking command strings.

MVP rule:

```text
Persist only a redacted display command. Refuse or redact secret-looking raw command text before storage or report rendering.
```

## Required command fields

Use these fields instead of a single unsafe raw command field:

```text
CommandDisplay
CommandHash
CommandRedactionApplied
CommandRefusedReason
```

Optional local-only raw command storage is forbidden in MVP.

## Storage phase

Command history JSONL is not Phase 1 markdown storage.

It belongs to the JSON sidecar phase:

```text
.agentwatch/command-history.jsonl
```

Gate:

```text
Only write command-history JSONL after the Phase 2 JSON sidecar contract is accepted.
```

Before that, markdown reports may include manually entered compact command evidence only.

## Report rule

Reports should show:

```text
Command: dotnet test --filter <redacted-or-safe-filter>
Duration: 3.8s
Status: Pass
Output: compact summary only
```

Reports must not show:

- bearer tokens;
- API keys;
- passwords;
- cookies;
- private keys;
- connection strings;
- inline env secret values.

## Implementation stop rule

Stop implementation if command redaction cannot be made safe with simple deterministic rules.

Do not rely on an LLM to decide whether a command string contains secrets.
