# Support-prompt coverage and open-PR classification

Last aligned: 2026-11-07

This file is supporting context for the canonical queue
`verification_mvp_2026_08_25.md`. It is not a second active queue.

## Next-task reminder

The only next claimable task is `AW-VFY-010-LEDGER`. Support prompts below may
run in parallel as docs/research if the owner assigns them, but they never
outrank the router.

## Support-prompt coverage matrix

| Prompt | File | Primary coverage | Current-semantics adaptations | Overlap / non-duplication | Next-task eligible? |
|---|---|---|---|---|---|
| AW-QA-001 | `../prompts/AW-QA-001-receipt-quality-protocol.md` | Quality protocol and predicate catalog for contract, receipt, evidence gate, scope, claims | Manual `Supported` = AWE009 / NeedsReview; `--accept-validation` binds but does not certify Done; finish CLI uses the redacted receipt; start baselines are not a redacted persistence surface; `AWD004` when a path is or will be `[redacted]` | Complements the parent AW-VFY-010 adversarial list. Does not inventory existing runs (LEDGER) and does not execute batches (B3) | No (parallel support) |
| AW-SEC-001 | `../prompts/AW-SEC-001-receipt-trust-boundary.md` | Privacy and trust-boundary review of persist/display surfaces | Records PR #12/#13 controls and documented residual limits without exploit steps; distinguishes heuristic redaction from independent verification | Complements `RECEIPT_REDACTION.md`; does not replace it | No (parallel support) |
| AW-RES-001 | `../prompts/AW-RES-001-external-value-protocol.md` | Interview/comparison protocol for later AW-VFY-011 | No invented customer feedback; AWE009 / NeedsReview must appear in the script; baselines are Git+CI+PR+native/Qodo | Does not start AW-VFY-011 | No (parallel research; 011 remains HOLD) |
| AW-RES-002 | `../prompts/AW-RES-002-commercial-design-partner-protocol.md` | Buyer-commitment protocol for later AW-VFY-012 | No invented commitments or pricing wins; polite interest does not count | Does not start AW-VFY-012; does not implement SaaS/billing | No (parallel research; 012 remains HOLD) |

Useful content from draft PR #9 was preserved and rewritten against current
receipt/trust-boundary semantics. Do not merge PR #9 as-is; its router/queue
text still presents AW-VFY-001 as next and predates Gates 0–4 closing.

## Open PR classification (do not change, merge, or close these PRs)

Classification is documentation only.

| PR | Branch / note | Classification | Why | Salvage? |
|---|---|---|---|---|
| [#4](https://github.com/ivanjovicic/AgentsWatch/pull/4) | `feature/automatic-discovery-learning-loop` (July-era discovery/proof + early CI; many commits) | **Superseded** for parser/CI foundation; **future-gated** for discovery/learning loop | Current porcelain/CI spine landed through AW-VFY-001..009 on `main`. Learning/discovery remains downstream of trustworthy receipts and AW-VFY-011 | Historical proof/fixture ideas only; do not merge the branch |
| [#5](https://github.com/ivanjovicic/AgentsWatch/pull/5) | Research/product direction stacked on #4 | **Salvage** (research notes) + **future-gated** (commercial/team-server ideas) | Direction later narrowed by the 2026-09-16 deep-dive and verification-first queue. Must not override the canonical queue | Optional research reading; do not merge |
| [#6](https://github.com/ivanjovicic/AgentsWatch/pull/6) | Early start/finish/report foundation stacked on #5 | **Superseded** | Live start/finish/receipt behavior is AW-VFY-004..006 on `main`, including later accept-priority, redaction, and finish-CLI work (PRs #11–#13) | Do not port the old schema; do not merge |
| [#8](https://github.com/ivanjovicic/AgentsWatch/pull/8) | Trust-platform / Gateway / Team Server / SaaS expansion | **Future-gated** | SaaS/auth/billing/team administration stay behind AW-VFY-012. The PR’s older “next prompt” language (including `AW-VAL-001`) is stale and must not be promoted | Observe-Control-Verify-Learn framing may be reread later if gates pass; do not merge now |
| [#9](https://github.com/ivanjovicic/AgentsWatch/pull/9) | GitHub branch docs/aw-queue-precision-2026-11-05 @ `66ec1409010e7e21ae47eeb8b73bbd3b004cb00b`, base `0f8122995d75f3efbdaf73ef975e92f3155f11b6` | **Salvage** (four support prompts) + **superseded** as a queue/router update | About 53 commits behind current `main`. Body still says next is AW-VFY-001 and that CI is red. Updating that branch would require merging current `main` into files whose status text conflicts with the post-#13 queue | Support prompts rewritten in this tree; leave PR #9 open/unmodified |

Merged PRs that are not classified above: #11, #12, and #13 are on `main` and
are cited as corrective dogfood-era product work. They are not open.

## HOLD list (unchanged by support work)

- AW-VFY-011 and AW-VFY-012
- Flutter / MathLearning-Mobile dogfood items #5 and #6 unless the owner lifts them
- Independently verified `Done` from manual `Supported`
- Merge/close of PRs #4, #5, #6, #8, #9
- SaaS / Gateway / Team Server / billing implementation
- Cloud-agent execution, production SQL, deploy, and direct pushes to `main`
