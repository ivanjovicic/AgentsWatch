# AW-VFY-010 batch 2 (sanitized summary)

Date: 2026-11-07 (Europe/Belgrade).

CLI under test: AgentsWatch `main` at `852ecd1f2aec7c30172cadcf904e399460ca00cd` (after PR #11 merge `8906526aba76082271e71b874ea4e672d1309c12`).

This file is a sanitized public summary for GitHub. It does not reproduce sentinel values, raw commands, or host file paths. Original local evidence notes and original receipts are not attached and were not modified for this pull request.

## Scope

Five real verification runs (R1–R5) plus two negative controls (N1–N2). Independent expected decisions were written before reading AgentsWatch output. Product worktrees were not pushed.

## Results

| ID | Role | Expected under contracted criteria | Observed decision | Classification |
| --- | --- | --- | --- | --- |
| R1 | Real (documentation) | Done | Done | TN |
| R2 | Real (privacy; expected non-Done) | Non-Done; leakage is a security finding | NeedsEvidence, plus leakage of an invented sentinel in stored JSON, Markdown, handoff, and CLI | TP (the run did not reach Done) and a confirmed projection leak |
| R3 | Real (documentation analysis) | Done | Done | TN |
| R4 | Real (scoped product change) | Done | Done | TN |
| R5 | Real (scoped product change) | Done | Done | TN |
| N1 | Control (Supported without a reason) | Reject / not Done | Rejected | TP |
| N2 | Control (Supported linked to a passing but unrelated validation) | Not a trustworthy Done | Done | FN (false Done) |

Run ids (receipts left as originally written; not bundled here):

- R1 `aw-vfy-010-b2-r1-20261107t054952z-fb635e`
- R2 `aw-vfy-010-b2-r2-20261107t055038z-30675b`
- R3 `aw-vfy-010-b2-r3-20261107t055709z-b9ec9f`
- R4 `aw-vfy-010-b2-r4-20261107t055238z-8e3bf4`
- R5 `aw-vfy-010-b2-r5-20261107t055246z-5189ed`
- N1 `aw-vfy-010-b2-n1-20261107t055113z-130c40`
- N2 `aw-vfy-010-b2-n2-20261107t055117z-f6583a`

## Confusion matrix

Treat “should stay out of trustworthy Done / should be caught” as the positive class for the controls and the privacy run; treat “Done is correct under the contracted criteria” as the negative class for R1, R3, R4, and R5.

| | Predicted catch / non-Done | Predicted Done |
| --- | --- | --- |
| Actually should be caught | TP 2 (R2, N1) | FN 1 (N2) |
| Actually should be Done | FP 0 | TN 4 (R1, R3, R4, R5) |

Combined: TP 2, FN 1, TN 4, FP 0. False-Done count: 1 (N2).

## Findings that this pull request addresses

1. **Projection leak (R2).** An invented sentinel appeared in canonical JSON, the Markdown receipt, the handoff, and CLI output because command display text and change paths were stored and printed verbatim. The corrective change redacts sentinel-like values after evaluation and before the evidence digest is computed. See `docs/RECEIPT_REDACTION.md`.
2. **Trust boundary (N2).** A manual `Supported` mark could be linked to any passing validation, including a validation that was only an unrelated environment probe declared at run level. The corrective change requires a per-criterion declared validation in the contract (`--accept-validation` bound to the preceding `--accept`). If that binding is missing, or a linked validation is not declared for that criterion, the gate records `AWE009` and the decision is `NeedsReview` (unverified), not trustworthy `Done`. Run-level `--validate` alone is not enough; implicit 1:1 pairing would have treated N2 as Done.

## R3 structured-validation check

Inspected the original canonical receipt `aw-vfy-010-b2-r3-20261107t055709z-b9ec9f` without modifying it.

A formatting verifier was executed during the playbook and exited `2`, but that result was **not** recorded as a structured validation. The receipt contains one structured validation: a documentation-locator check with status Pass and exit `0`. The decision is Done, with no `AWE003` finding.

Because there was no stored Fail paired with Done, this is not the extra Fail-vs-Done regression. The original receipt was left unchanged. Owner guidance that an out-of-contract extra check need not automatically reclassify that narrow documentation task to Failed still applies.

A separate regression in this pull request locks the existing fail-closed rule: any structured validation with status Fail still prevents Done (even when it is not the contract’s mandatory command). There is no “outside contract” marker on validations today; adding one is out of scope.

## References

- PR #11 (P1/P2 accept priority and Supported reason): merge `8906526aba76082271e71b874ea4e672d1309c12`, follow-up evidence note `852ecd1f2aec7c30172cadcf904e399460ca00cd`.
- This corrective branch: receipt redaction, `AWE009` unverified acceptance, sanitized batch 2 summary.
