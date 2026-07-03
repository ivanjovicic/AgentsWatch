# AW-DISC-VALIDATION-001 — Gate 0 proof exposed parser and smoke-runner defects

Discovery ID: AW-DISC-VALIDATION-001  
Status: InProgress  
Category: ValidationGap  
Severity: P1  
Confidence: Confirmed  
Found in run: AW-PROOF-SYSTEM-001  
Created: 2026-07-03

## Evidence

- CI run `28649826676`: restore/build passed on Linux and Windows; tests failed because Git porcelain leading status whitespace was removed, turning `README.md` into `EADME.md`.
- CI run `28650352024`: all eight tests passed on Linux; smoke outputs and expected exit codes were correct, but PowerShell returned the last intentional unknown-command exit code and marked the step failed.

## Fixes

- Preserve Git porcelain columns and trim only CR line endings.
- Add CRLF/index-status regression coverage.
- End the smoke script with success after asserting the expected error code.
- Pin SDK selection to .NET 8 with `global.json`.

## Reconciliation

Primary owner: Gate 0 validation and proof queue  
Queue target: `bootstrap_validation.md`, `agentwatch_proof_and_verification.md`  
Validation target: CI run for commit `f7332b2a71ca502e53e5516150bc93a767a73a13`  
Resolved by: pending green CI/proof artifacts

## Links

Pull request: `https://github.com/ivanjovicic/AgentsWatch/pull/4`
