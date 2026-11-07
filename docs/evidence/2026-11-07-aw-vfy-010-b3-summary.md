# AW-VFY-010 Batch 3 sanitized summary

Date: 2026-11-07 (Europe/Belgrade)
Product SHA under study: AgentsWatch `8e0bd8e7a9ab9b479a6f5a4cda21a63cc5fc4809` (merge of PR #15).
Main CI after PR #15: run `42371110378`, 193/193.
CLI: Release build of that SHA.
This summary does not reproduce invented sentinel values or host-absolute transcripts.

Parent program AW-VFY-010 remains **In progress**. Useful REAL count after this batch: **11 of 30**.

## Executed REAL runs

1. **B3-R1** `aw-vfy-010-b3-r1-20261107t122421z-e6589a` (AgentsWatch). Kind REAL.
   Purpose: redact unexpected exception messages from `Program.Main` and add a focused test.
   Start/end HEAD `8e0bd8e` (interval files uncommitted at finish).
   Attribution: run-interval `Program.cs` and `TrustBoundaryAndRedactionTests.cs`; clean at start.
   Decision after manual Supported: NeedsReview + AWE009 (finish-at-close NeedsEvidence). Class TP.
   Digest `sha256:0042bd8d4ba88e7b4967bb6ffbea9a82a816315d1ae2c2de09e5ef86dd117b15`.
   Local tests after the change: 194/194.

2. **B3-R2** `aw-vfy-010-b3-r2-20261107t122625z-de848e` (MathLearning). Kind REAL.
   Purpose: investigate leaderboard guide versus XP-reset timing, week start, and Hangfire job registration. Not issue #47.
   Start/end HEAD `b8acd2294d9677bc53a1a85acf3406595c1b49ef`.
   Attribution: run-interval investigation note under `.ai/runs/`; four AgentsWatch init files pre-existing unchanged.
   Decision after manual Supported: NeedsReview + AWE009. Class TP.
   Digest `sha256:4855bebb8554a65c158b0e9b65b68ba4b7cfa4353da130379466557accf39dda`.
   Findings: guide hourly cadence is wrong (delay until next UTC midnight); Monday week start matches code; LeaderboardJobs stubs are not registered.

3. **B3-R3** `aw-vfy-010-b3-r3-20261107t122746z-d47952` (MathLearning). Kind REAL.
   Purpose: correct the guide cadence and exercise a distinct tracked dirty-start.
   Start/end HEAD `b8acd22`.
   Attribution: run-interval `LEADERBOARD_SYSTEM_GUIDE.md`; planted tracked `LeaderboardJobs.cs` comment pre-existing unchanged.
   Decision after manual Supported: NeedsReview + AWE009. Class TP.
   Digest `sha256:402b026ae7634891a9dfd29a6f34880fd12d7e04f83db184b13ab4de822b8a88`.

No FIXTURE, NEGATIVE_CONTROL, REPLAY, DUPLICATE, or UNKNOWN rows were added.

## Confusion additions this batch

TP=3, FP=0, TN=0, FN=0, Unknown=0. No unsafe false-Done, no material attribution error, no serious privacy leak.

## Reviewer overhead

Unknown (not independently timed).

## Retainable changes

Separate draft PRs (not merged from this batch): AgentsWatch Program.Main redaction; MathLearning leaderboard guide cadence correction. Planted dirty comments are not proposed for retention.
