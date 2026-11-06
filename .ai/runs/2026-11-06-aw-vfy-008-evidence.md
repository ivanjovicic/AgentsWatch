# AW-VFY-008 evidence: Scope Drift v1

- Prompt: AW-VFY-008; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Gate basis: AW-VFY-007 + 006/007 follow-up done locally (`8cabed7`/`cce86c1`, 83/83). Not pushed.
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## Path matching (`RepositoryPath`)

- Normalize: `\` -> `/`, collapse `//`, strip leading `./` and surrounding slashes.
- Ordinal (case-sensitive) match.
- Globs: `**` any path including slashes (`**/` also matches zero directories), `*` any non-slash sequence, `?` one non-slash char.
- No brace expansion or character classes. First matching pattern in list order wins for reporting.

## Rules

- AWD001 ScopeOutsideOwnedPaths: attributable path matches no ownedPaths (skipped when ownedPaths is empty/unbounded).
- AWD002 AvoidPathTouched: attributable path or rename oldPath matches avoidPaths (reported even if also owned).
- AWD003 RenameCrosses: old vs new differ in owned membership.
- AWD004 AmbiguousScope: Ambiguous classification that would affect scope (outside owned, avoid, or rename crossing); NeedsReview finding, no guessed AWD001/002.
- Evaluated classes: RunIntervalChange and PreExistingChangedFurther. PreExistingUnchanged never creates drift.
- Findings replaced wholesale on each check (AWD* stripped then rewritten) so checks are idempotent.
- EvidenceGate keeps AWD* findings/reasons; Done + AWD001/002/003 becomes NeedsReview. Missing validation still yields NeedsEvidence (higher-priority gap).

Finding payload: rule/category, Paths (new + oldPath), message includes matched pattern or "no pattern matched", attribution class/status, severity/status.

## Tests

RepositoryPath: separator/`./` normalization; glob accept/reject including `Src/**` vs `src` case; first-match order.
ScopeDrift (temp git repos): inside owned; outside owned; avoid even when owned; pre-existing unchanged outside (no drift); pre-existing changed-further outside (AWD001 + context); ambiguous outside (AWD004, not AWD001); ambiguous inside owned (no AWD); rename inside->outside and outside->inside; slash pattern `src\**`; empty owned + avoid still applies; overlapping owned patterns; idempotent re-check + missing/invalid id; missing contract; CLI check + usage errors + Markdown projection.

## Gate (executed)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 120, Skipped 0, Total 120.

## Smoke (temp repo)

Contract owned `src/**`, avoid `src/secret/**`. Start clean, edit `src/app.cs` and `extra.txt`, finish (2 RunIntervalChange), `drift check`: AWD001 for `extra.txt` only (attribution RunIntervalChange); exit 1 because decision remains NeedsEvidence (no validation recorded).

## Self-review gaps

- COMMAND_CONTRACTS also mentions "elevated-risk path patterns when configured"; RunContract has no such field and this prompt's required-check list does not include it. Not implemented.
- Matching is case-sensitive; Windows case-insensitive filesystems can therefore diverge from the matcher.
- `src/**` does not match a file named exactly `src`.
- Drift trusts the receipt delta; it does not re-read git.
- No dedicated test for avoid+outside both applying (owned `src/**`, avoid `docs/**`, change `docs/x` would be AWD001+AWD002). Avoid-when-owned is tested.
- No test that `evidence check` after `drift check` preserves AWD findings (intended via EvidenceGate keeping non-AWE rules, not separately proven).

Commit SHA (change): f91a7f4

