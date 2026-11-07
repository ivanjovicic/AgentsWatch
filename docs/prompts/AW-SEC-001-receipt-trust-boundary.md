# AW-SEC-001 — Receipt privacy and trust-boundary review

Repository: `ivanjovicic/AgentsWatch`  
Queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Parallel support; not the next claimable task  
Depends on: current `docs/RECEIPT_REDACTION.md` and merged PRs #12 and #13  
Run mode: documentation / threat review  
Budget: medium

## Purpose

Produce a current privacy and trust-boundary review of what AgentsWatch
persists and prints. This is not a penetration-test prompt and not a request
for exploit work. It records residual risk against the documented redaction
contract.

## In-scope surfaces

- Stored receipts and Markdown projections
- Finish CLI stdout/stderr after PR #13
- Evidence / accept command banners
- Logs and local `.ai/runs/` write-ups
- Path redaction and `AWD004` interaction
- Start-baseline persistence (currently not a redacted surface; say so plainly)
- Digest stability after redaction (`evidenceDigest`)

## Out of scope

- Writing exploits, attack PoCs, or payloads
- Changing runtime code unless a later authorized fix prompt is opened
- Production SQL, deploy, or hosted auth
- Inventing incidents or customer reports

## Known facts to verify rather than re-discover

- PR #12 added JSON string-value redaction and kept manual `Supported` as
  `AWE009` / `NeedsReview`.
- PR #13 made finish summaries render from the redacted receipt. The
  post-merge finish-output check on `edbbbe4…` reported 0 invented-sentinel
  hits on stored artifacts and finish stdout/stderr.
- Documented residual limits: unexpected exceptions from `Program.Main` may
  still print unredacted text; artifact paths outside the workspace may not
  become repo-relative before `SafeText`.
- Heuristic redaction can over-redact (AWD004) or miss a novel secret shape.

## Owned paths

- `docs/security/RECEIPT_TRUST_BOUNDARY_REVIEW.md` (create if absent)
- updates to `docs/RISK_REGISTER.md` only if an existing row must be dated;
  do not invent severity scores
- coverage-matrix status pointer

## Avoid paths

- `src/**` / `tests/**` in this prompt
- historical receipts
- publishing real tokens, customer paths, or previously used sentinel values
- a second queue or a claim that AW-VFY-010 is complete

## Acceptance criteria

- Review lists persistence and display surfaces, current controls, residual
  limits, and recommended follow-ups without exploit steps.
- Distinguishes independent verification (not available for manual Supported)
  from heuristic redaction (best-effort privacy).
- Does not overwrite `docs/RECEIPT_REDACTION.md` historical sections; may add
  a dated cross-link.

## STOP conditions

- Request for exploit reproduction steps.
- Need to “fix” residual limits inside this review prompt.
- Need to mark security work as proof that dogfood is done.
