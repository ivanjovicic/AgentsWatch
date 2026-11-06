# Evidence Gate decision ordering

Status: normative for the Evidence Gate in this repository.
Total order: **Failed > Blocked > NeedsApproval > NeedsReview > NeedsEvidence > Done**

The classifier inspects the finished finding set. The same findings always produce the same decision, regardless of the order in which individual checks appended those findings.

## First matching class wins

1. **Failed** — any `AWE003` (validation `Fail`, `TimedOut`, or `Killed`).
2. **Blocked** — any `AWE004` (blocked by environment).
3. **NeedsApproval** — any `AWE008`.
4. **NeedsReview** — scope-drift errors (`AWD001`, `AWD002`, `AWD003`), unsupported claims (`AWK001`), unsupported acceptance (`AWE007` with status Unsupported), or attribution ambiguity (`AWR002` / `AWD004`). This class outranks unknown acceptance, so drift and unsupported claims are not hidden behind a `NeedsEvidence` banner.
5. **NeedsEvidence** — incomplete receipt (`AWE001`), missing or unevaluated mandatory validation (`AWE002`), missing expected evidence (`AWE006`), unknown acceptance (`AWE007` with status Unknown), or unknown claims (`AWK002`).
6. **Done** — no blocking findings.

`Fail`, `TimedOut`, `Blocked`, `NeedsApproval`, and invalid receipts (failed digest/shape checks that refuse rewrite) stay non-Done. Historical receipts whose acceptance criteria are still Unknown do not become `Done` merely because this ordering changed; they remain `NeedsEvidence` unless a stronger class applies.

## Marking acceptance

Use `agentswatch accept record <run-id> --id <acN> --status supported|not-supported|unknown --evidence <validation-id>`.

- `Supported` requires one or more recorded validations that are `Pass` with exit `0` or no exit code. Evidence-free `Supported` is rejected.
- `NotSupported` requires a link to a recorded validation id.
- `Unknown` may omit `--evidence` (clears a previous mark when `--replace` is used).
- Tampered or digest-invalid receipts are rejected and not rewritten.
