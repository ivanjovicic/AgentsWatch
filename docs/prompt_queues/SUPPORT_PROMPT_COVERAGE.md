# Support-prompt coverage and open-PR classification

Last aligned: 2026-11-07

This file is supporting context for the canonical queue
`verification_mvp_2026_08_25.md`. It is not a second active queue.

## Next-task reminder

The only next claimable task is `AW-VFY-010-LEDGER`. Support prompts below may
run in parallel as docs/research if the owner assigns them, but they never
outrank the router.

## Support-prompt coverage matrix

| Prompt | File | Primary coverage now | Next-task eligible? |
|---|---|---|---|
| AW-QA-001 | `../prompts/AW-QA-001-receipt-quality-protocol.md` | Quality protocol, current-semantics predicate catalog, restored adversarial must-catch list and measurement rubric | No (parallel support) |
| AW-SEC-001 | `../prompts/AW-SEC-001-receipt-trust-boundary.md` | OSS/licensing/distribution/privacy preflight **and** receipt persist/display review | No (parallel support) |
| AW-RES-001 | `../prompts/AW-RES-001-external-value-protocol.md` | Dated competitive baseline, >=8 falsifiable scenarios, copy/kill tests, **and** later interview script | No (parallel research; 011 remains HOLD) |
| AW-RES-002 | `../prompts/AW-RES-002-commercial-design-partner-protocol.md` | ICP (5–30 developers), >=15-candidate framework, ROI formula, two pricing hypotheses, **and** commitment definition | No (parallel research; 012 remains HOLD) |

These prompts are not a second runtime queue. They do not change Gate 6 or
Gate 7 thresholds and do not start AW-VFY-011 or AW-VFY-012.

## What the first PR #14 draft actually carried vs dropped

Compared with PR #9 files
`AW-QA-001-receipt-benchmark-preflight.md`,
`AW-SEC-001-oss-license-and-privacy-preflight.md`,
`AW-RES-001-competitive-baseline-verification.md` and
`AW-RES-002-buyer-discovery-preflight.md` (branch
docs/aw-queue-precision-2026-11-05 @ `66ec1409010e7e21ae47eeb8b73bbd3b004cb00b`).
The first PR #14 commit (`4057da827c2bb118736961f5a8d7163494f52532`) did **not**
preserve all useful PR #9 content. This corrective commit restores the gaps
called out in review `5811734643`. Honesty below is about both drafts.

### AW-QA-001

Carried in the first draft and kept: support-track placement; no runtime
expansion; protocol aligned to current AWE009 / NeedsReview,
`--accept-validation` binding, finish-CLI redaction, and `AWD004`; explicit
statement that this does not replace or complete AW-VFY-010.

Restored here from PR #9: adversarial must-catch catalog; per-slot expected
evidence/decision/baseline-workflow fields; measurement list (including
reviewer minutes as Unknown if unmeasured); 5–10 run batch cadence;
independent adjudication; fixture-vs-product-failure split; disposable-repo
rule.

Intentionally not restored: “must be complete before AW-VFY-010 executes”
(obsolete; 010 is already in progress). Optional `tests/**` fixture
implementation remains unauthorized here (protocol/docs only). Output path
stays `docs/qa/RECEIPT_EVIDENCE_PROTOCOL.md` rather than
`docs/research/agentswatch_market_validation_2026_11/adversarial_30_run_protocol.md`
to avoid a second queue-like research tree for QA.

### AW-SEC-001

Carried in the first draft and kept: receipt persist/display review against
`RECEIPT_REDACTION.md`; PR #12/#13 controls; residual `Program.Main` and
out-of-repo path limits; no exploit steps.

Dropped by the first draft and restored here from PR #9: LICENSE existence
check (public repo ≠ OSS); owner distribution decision
(closed/permissive/copyleft/open-core) without auto-selecting a license;
consent/retention defaults; exfiltration and command-execution threat model;
tool install/sharing safeguards; 8–12 point external-evaluation safety
checklist; required-before-public-installer / before AW-VFY-011
repository-evidence collection.

### AW-RES-001

Carried in the first draft and kept: later interview/comparison script;
AWE009 / NeedsReview questions; no invented customer feedback; 011 HOLD.

Dropped by the first draft and restored here from PR #9: dated primary-source
competitor matrix (Confirmed/Partial/Unknown/Contradicted; no inferred “No”);
at least eight falsifiable baseline scenarios (dirty unchanged,
dirty changed-further, untracked/rename, missing mandatory tests, stale
validation after HEAD change, forbidden path, unsupported “all tests pass”,
concurrent writer ambiguity); copyability / simplest competing implementation;
kill/stop rule if the baseline already catches the issue at equal or lower
reviewer effort; required outputs `competitive_baseline.md` and `sources.md`.

### AW-RES-002

Carried in the first draft and kept: definition of a concrete later
commitment vs polite interest; no invented quotes or pricing wins; 012 HOLD;
no SaaS/billing implementation.

Dropped by the first draft and restored here from PR #9: 5–30 developer ICP
screen; >=15-candidate outreach framework across >=2 ecosystems (no invented
names); neutral buyer interview; blinded/side-by-side evaluation worksheet;
ROI formula with labeled inputs; two pricing hypotheses (team flat-rate and
per-seat; €79–199/team/month as hypothesis only); privacy/consent notes;
kill rules; explicit consistency with unchanged AW-VFY-011/012 thresholds.

Do not merge PR #9 as-is; its router/queue text still presents AW-VFY-001 as
next and predates Gates 0–4 closing.

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
- Public installer / LICENSE selection until the owner explicitly decides
