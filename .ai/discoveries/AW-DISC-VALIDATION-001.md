# AW-DISC-VALIDATION-001 — Gate 0 proof exposed parser and smoke-runner defects

Discovery ID: AW-DISC-VALIDATION-001  
Status: Resolved  
Category: ValidationGap  
Severity: P1  
Confidence: Confirmed  
Found in run: AW-PROOF-SYSTEM-001  
Created: 2026-07-03  
Last reviewed: 2026-07-03

## Evidence

- CI run `28649826676`: restore/build passed on Linux and Windows; tests failed because Git porcelain leading status whitespace was removed, turning `README.md` into `EADME.md`.
- CI run `28650352024`: parser tests passed, but PowerShell returned the expected unknown-command exit code after assertions and falsely failed the smoke step.
- CI run `28650547744`: Linux and Windows restore/build/test/smoke passed; package/checksum/isolated installation passed.

## Fixes

- Preserve Git porcelain columns and trim only CR line endings.
- Add CRLF/index-status regression coverage.
- End the smoke script successfully after asserting expected exit code 2.
- Pin SDK selection to .NET 8 with `global.json`.

## Resolution proof

- Linux: 8 tests executed, 8 passed; all smoke cases passed.
- Windows: 8 tests executed, 8 passed; all smoke cases passed.
- package: `AgentsWatch.Cli.0.1.0.nupkg`.
- SHA-256: `3bc0a9b2acb20c200ffd749186a2697ac95e42a8497647c36234d1a79d330288`.
- checksum verification and isolated tool install: Pass.

## Reconciliation

Primary owner: Gate 0 validation and proof queue  
Queue target: `bootstrap_validation.md`, `agentwatch_proof_and_verification.md`  
Validation evidence: `docs/VALIDATION_EVIDENCE_2026_07_03.md`  
Resolved by: workflow run `28650547744`

## Links

Pull request: `https://github.com/ivanjovicic/AgentsWatch/pull/4`
