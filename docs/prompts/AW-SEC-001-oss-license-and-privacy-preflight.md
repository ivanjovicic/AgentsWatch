# AW-SEC-001 — OSS distribution, evidence privacy and security preflight

Repository: `ivanjovicic/AgentsWatch`  
Owning queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Ready now for documentation; closure required before public external installer/package or AW-VFY-011 repository-evidence collection  
Run mode: audit/design, no automatic license selection or production launch  
Context budget: low

## Goal

Avoid turning a local-first verifier into an unsafe data exfiltration/command-execution path or distributing a purported OSS product without explicit rights.

## Steps

1. Check current `README.md`, `SECURITY.md`, `docs/SECURITY_AND_PRIVACY.md`, `AGENTS.md`, current CLI and CI. Verify whether a root `LICENSE` file exists on the exact branch being prepared; public repo != automatically open source.
2. Present a concise **owner decision** for distribution: closed-source test, permissive OSS, or copyleft/open-core; list tradeoffs and obligations. Do not insert/select a license without authorization.
3. Map evidence data that may reveal personal data, proprietary paths, source, secrets, command stdout/stderr, environment variables, provider tokens or private repo metadata; define minimum collection and retention defaults.
4. Threat-model CLI commands, Git invocations, future validation command execution, local filesystem writes/path traversal, malicious repository filenames, symlinks and crafted receipt JSON. Require fail-closed behavior and explicit user permission before executing tests provided by a third party.
5. Define safe sharing/export rules: redact absolute paths/secrets, do not upload full source/chat/logs by default, no telemetry by default, immutable provenance refs with no false SLSA/compliance claim.
6. Recommend short-lived external-evaluation onboarding checklist: OS/.NET support, reproducible install/rollback, consent, scoped repo access, export/deletion procedure, contact for security issues. Do not implement hosted auth/SSO.
7. Prioritize as P0 external-evaluation blocker vs post-validation improvement; identify only evidence-backed findings.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/**`
- `SECURITY.md` or `docs/SECURITY_AND_PRIVACY.md` only for factual, approved, low-risk corrections
- `.ai/runs/**` audit evidence

## Avoid

- `src/**`, `tests/**`, hosted services, telemetry, shipping an installer
- license addition/selection without explicit owner decision
- uploading customer repository content or secrets

## Required output

`docs/research/agentswatch_market_validation_2026_11/security_distribution_preflight.md` with a threat table, evidence-field retention decision, 8–12 point external-evaluation safety checklist, GPL/permissive/private option comparison and clearly marked owner choices.

## Definition of done

Security/distribution blockers are explicit and assigned, not implicitly waived. No claim of security certification or release readiness without supporting tests/approval.
