# AW-SEC-001 — OSS distribution, evidence privacy, and receipt trust-boundary preflight

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Parallel support; not the next claimable task  
Depends on: current `docs/RECEIPT_REDACTION.md` and merged PRs #12 and #13  
Run mode: audit/design; no automatic license selection, installer, or production launch  
Budget: medium

## Purpose

Avoid turning a local-first verifier into an unsafe data-exfiltration or
command-execution path, and avoid distributing a purported OSS product without
explicit rights. Also record residual receipt persist/display risk against the
documented redaction contract. This is not a penetration-test prompt and not a
request for exploit work. Closure of the distribution/privacy preflight is
required before a public external installer/package or AW-VFY-011
repository-evidence collection. AW-VFY-011 remains HOLD.

## Part A — OSS license, distribution and privacy preflight (restored from PR #9)

1. Check current `README.md`, `SECURITY.md`, `docs/SECURITY_AND_PRIVACY.md`,
   `AGENTS.md`, current CLI and CI. Verify whether a root `LICENSE` file exists
   on the exact branch being prepared. A public repo is not automatically
   open source.
2. Present a concise **owner decision** for distribution: closed-source test,
   permissive OSS, or copyleft/open-core; list tradeoffs and obligations.
   Do not insert or select a license without authorization.
3. Map evidence data that may reveal personal data, proprietary paths, source,
   secrets, command stdout/stderr, environment variables, provider tokens or
   private repo metadata; define minimum collection and retention defaults.
4. Threat-model CLI commands, Git invocations, future validation command
   execution, local filesystem writes/path traversal, malicious repository
   filenames, symlinks and crafted receipt JSON. Require fail-closed behavior
   and explicit user permission before executing tests provided by a third
   party.
5. Define safe sharing/export and tool-install safeguards: redact absolute
   paths/secrets, do not upload full source/chat/logs by default, no telemetry
   by default, immutable provenance refs with no false SLSA/compliance claim.
6. Recommend a short-lived external-evaluation onboarding checklist (8–12
   points): OS/.NET support, reproducible install/rollback, consent, scoped
   repo access, export/deletion procedure, contact for security issues.
   Do not implement hosted auth/SSO.
7. Prioritize as P0 external-evaluation blocker vs post-validation improvement;
   identify only evidence-backed findings.

## Part B — Receipt persist/display review (kept from the earlier PR #14 draft)

Verify rather than re-discover:

- Stored receipts and Markdown projections
- Finish CLI stdout/stderr after PR #13
- Evidence / accept command banners
- Logs and local `.ai/runs/` write-ups
- Path redaction and `AWD004` interaction
- Start-baseline persistence (currently not a redacted surface; say so plainly)
- Digest stability after redaction (`evidenceDigest`)
- PR #12 JSON string-value redaction and AWE009 / NeedsReview for manual `Supported`
- PR #13 finish summaries render from the redacted receipt; the post-merge
  finish-output check on `edbbbe4…` reported 0 invented-sentinel hits
- Documented residual limits: unexpected `Program.Main` exceptions; artifact
  paths outside the workspace may not be repo-relative
- Heuristic redaction can over-redact (AWD004) or miss a novel secret shape

Do not write exploits, attack PoCs, or payloads. Do not invent incidents.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/security_distribution_preflight.md`
- `docs/security/RECEIPT_TRUST_BOUNDARY_REVIEW.md`
- `SECURITY.md` or `docs/SECURITY_AND_PRIVACY.md` only for factual, approved,
  low-risk corrections
- coverage-matrix status pointer
- `.ai/runs/**` audit evidence

## Avoid

- `src/**` / `tests/**` in this prompt
- historical receipts
- publishing real tokens, customer paths, or previously used sentinel values
- license addition/selection without explicit owner decision
- hosted services, telemetry, shipping an installer
- a second queue or a claim that AW-VFY-010 or AW-VFY-011 is complete

## Acceptance criteria

- Preflight document includes a threat table, evidence-field retention
  decision, 8–12 point external-evaluation safety checklist, and
  GPL/permissive/private option comparison with owner choices clearly marked as undecided
  until the owner decides.
- Receipt-surface review lists persist/display controls, residual limits, and
  follow-ups without exploit steps.
- Distinguishes independent verification (not available for manual Supported)
  from heuristic redaction.
- Security/distribution blockers are explicit and assigned, not implicitly
  waived. No claim of security certification or release readiness.

## STOP conditions

- Request for exploit reproduction steps.
- Pressure to choose or commit a LICENSE without owner authorization.
- Need to mark security work as proof that dogfood or Gate 6 is done.
