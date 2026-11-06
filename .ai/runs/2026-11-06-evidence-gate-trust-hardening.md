# 2026-11-06 — Evidence Gate invariants and adversarial regression

- Run mode: implementation + analysis, bounded review fix (not AW-VFY-010 dogfood completion).
- Source base SHA: `058283bc0452a40ab3e731f3083ff1848f61055d`.
- Goal: prevent false `Done` from missing/substituted acceptance criteria, mismatched contract, cross-run validation, mandatory command mismatch and contradictory pass/exit code.
- Owned paths: `src/AgentsWatch.Core/Runs/EvidenceGate.cs`, `tests/AgentsWatch.Tests/EvidenceGateTests.cs`, `README.md`, `AGENTS.md`, `docs/prompts/AW-VFY-010-dogfood-30-receipts.md`.
- No production mutation; main not changed.

## Added regression tests

1. `RemovedAcceptanceCriteria_CannotTurnEvidenceGateGreen`
2. `SubstitutedAcceptanceCriteria_CannotTurnEvidenceGateGreen`
3. `CrossRunEvidence_IsRejectedAtImport_AndIgnoredIfAlreadyStored`
4. `MatchingIdWithDifferentCommand_DoesNotSatisfyMandatoryValidation`
5. `PassingStatusWithNonzeroExitCode_IsContradictoryEvidence`
6. `InvalidContractModifiedAfterFinish_CannotProduceDone`

## Gate truth at creation

- Did not execute .NET locally; branch submitted to GitHub PR CI for an exact-SHA Release build and full test suite.
- Therefore **NOT VERIFIED** pending the recorded workflow result. No PASS claimed.
- No 30-run dogfood performed; AW-VFY-010 remains open.
- Source declaration remains user-controlled; signed/independent provenance not implemented.
- Semantic acceptance checks and receipt attestation remain unproven.

## Next steps

1. Check full PR CI on exact head; fix failures before any merge recommendation.
2. Independent review of false-Done scenarios and decision semantics.
3. Run AW-VFY-010 adversarial matrix and 30 real receipts; do not treat these six unit cases as real receipts.
4. Validate external reviewer value before expanding feature footprint.
