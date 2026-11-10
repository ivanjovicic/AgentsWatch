# AW-FIX-EXECUTOR-001 Evidence

Evidence format: v2
Prompt ID: AW-FIX-EXECUTOR-001
Queue: owner-authorized corrective task (not a verification_mvp_2026_08_25 queue row; see Claim record)
Agent/tool: Grok Bot executor subagent
Model provider: unknown-not-exposed
Model name/id: unknown-not-exposed
Model mode/settings: unknown-not-exposed
Client/IDE: Grok Bot box + Windows Shell on DESKTOP-V877DAD
Run mode: implementation (test-first, fail-closed)
Token budget: medium
Elapsed time: unknown-not-recorded
Phase time breakdown: unknown-not-recorded
Commit SHA: see the draft PR head (a file cannot contain its own commit SHA); base fe1be515108437b948d34b3ac28fc5af36810db9

This log is itself a declarative source. Under the rule it implements, it does not prove who authored the change.

## Claim record

- Authorization: explicit owner GO for this corrective task only (spec `AW-FIX-EXECUTOR-001 - Executor Identity & Provenance Trust Boundary`, sha256 `9cc5514f13a14e2d235bb4e8ef7e6d66959d9d0525c44304e2dc38ccb09baa4e`, 304 lines).
- `AGENTS.md` working order says to "select exactly one currently claimable prompt" from `docs/prompt_queues/verification_mvp_2026_08_25.md` via the router. The canonical docs define no lease/lock mechanism and no path for an owner-authorized corrective task.
- I did not invent a queue entry and did not change the router or the queue. The claim is recorded transparently and minimally as: (1) this evidence log; (2) the dedicated branch `fix/aw-fix-executor-001` from `origin/main` `fe1be51`; (3) one draft PR. No existing claim, branch, or worktree was taken over. Before starting, there were no open PRs or remote branches for this task.
- Gate 5 ADJUST/PAUSE stays in force for everything else. This task does not start AW-VFY-010 B5, AW-VFY-011 or AW-VFY-012, and does not touch the AW-VFY-010 counters (12/30).
- Worktree: `C:\Users\Ivan\source\repos\_aw-vfy-010\aw-fix-executor-001` (new, disposable).

## What changed

- `executorIdentity` (status, authorshipVerified=false, reason, compared/conflicting fields, source refs) and `observedExecutorEvidence` (bounded v2 header fields, workspace-relative path, SHA-256) are added as optional RunReceipt v1 fields, omitted when null.
- `ExecutorIdentityEvaluator`: pure, deterministic; statuses UNKNOWN / DECLARED_ONLY / CONSISTENT_UNVERIFIED / CONFLICT. VERIFIED is reserved and never assigned. Loading a receipt that claims it, or claims authorshipVerified=true, is rejected.
- `finish <run-id> --executor-log <relative-path>`: read-only and workspace-confined. Refuses absolute paths, `..`, symlinks/junctions, `.agentwatch/`, AgentsWatch projections, non-regular files and files over 64 KiB (exit 2, active run kept). Malformed content records UNKNOWN.
- `AWI001` / `executor-identity-conflict` finding (Warning/NeedsReview). It is outside decision ordering and distinct from AWR002.
- An identity block in `finish`, `receipt show` / `.ai/runs` projection and `handoff`; `Authorship verified: NO` is always shown.
- Docs: `docs/EXECUTOR_IDENTITY.md` (new) plus one-line pointers in COMMAND_CONTRACTS, CLI_SPEC, DATA_MODEL, DECISION_ORDERING and README.

## Files changed

- src/AgentsWatch.Core/Runs/ExecutorIdentity.cs (new)
- src/AgentsWatch.Core/Runs/RunReceipt.cs
- src/AgentsWatch.Core/Runs/RunDelta.cs
- src/AgentsWatch.Cli/FinishCommand.cs
- src/AgentsWatch.Cli/Program.cs
- src/AgentsWatch.Reports/ReceiptProjections.cs
- tests/AgentsWatch.Tests/ExecutorIdentityFlowTests.cs (new)
- tests/AgentsWatch.Tests/ExecutorIdentityEvaluatorTests.cs (new)
- tests/AgentsWatch.Tests/Fixtures/v1-receipt-pre-executor-identity.json (new; produced by the unmodified fe1be51 CLI, sha256 031c51103085827f911a0033b00e9c29fd247d043ba8e2eee6125e140737b854)
- tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj (copy fixtures to output)
- docs/EXECUTOR_IDENTITY.md (new), docs/COMMAND_CONTRACTS.md, docs/CLI_SPEC.md, docs/DATA_MODEL.md, docs/DECISION_ORDERING.md, README.md
- .ai/runs/2026-11-10-aw-fix-executor-001-evidence.md (this log)

## Validation

Validation run (local, Windows, .NET SDK 10.0.201 targeting net8.0, Release):
- RED behavioral (src identical to fe1be51; only the flow tests, fixture and csproj item added): `dotnet test tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj -c Release --no-build --filter FullyQualifiedName~ExecutorIdentityFlowTests` returned exit 1: 27 total, 27 failed, 0 passed. Causes: `finish` rejected `--executor-log` (usage, exit 2); receipts had no `executorIdentity`; show/handoff had no identity block; forged VERIFIED receipts loaded. T10 failed only on the missing display: the historical fixture already deserialized, verified its digest and re-serialized identically.
- RED compile (evaluator/parser unit tests added, src still identical to fe1be51): `dotnet build tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj -c Release` returned exit 1, error CS0246 `ObservedExecutorEvidence` not found.
- GREEN targeted: `--filter FullyQualifiedName~ExecutorIdentity` returned exit 0: 42 total, 42 passed.
- GREEN full gate: `dotnet restore AgentsWatch.sln` exit 0; `dotnet build AgentsWatch.sln --configuration Release` exit 0 (0 warnings, 0 errors); `dotnet test AgentsWatch.sln --configuration Release` exit 0: 239 passed, 0 failed, 0 skipped.
- `git diff --check` clean.
- `python scripts/validate_agent_evidence.py`: pre-existing baseline on fe1be51 is FAIL (30 FAIL / 236 WARN, all in older queue rows and logs). This log adds no new finding.
- First pushed head `61d1ee6`: local Windows full gate exit 0 (239/239) at that exact SHA. CI run 42790834828 had windows-latest success and ubuntu-latest failure: 1 test, T10 `VerifyDigest` false.
- Root cause (pre-existing, not introduced here): `RunReceiptJson.ComputeDigest` hashes indented JSON whose newline follows `Environment.NewLine`. The Windows-produced fixture's stored digest `sha256:159bb5a1...` equals SHA-256 of its canonical JSON in CRLF form. On Linux the same unchanged content hashes as LF `sha256:6a9b7d31...`. Reproduced on Linux (box, .NET SDK 8.0.426) with unmodified fe1be51 Core: the same mismatch. `RunContractJson.Options` is untouched by this PR.
- T10 correction (my own wrong test assumption, not a change to product behavior): T10 now checks the digest platform-independently (stored digest == SHA-256 of the unchanged content in its producing CRLF form). It keeps the strict `VerifyDigest`/`EnsureStoredDigest`/no-mismatch-warning checks where `Environment.NewLine` matches the producer. Every other T10 assertion is unchanged.
- After the correction, the full Release gate on Linux (box, .NET SDK 8.0.426) exited 0: 239/239 passed. Windows was re-run on the final head. GitHub CI for the final head is recorded in the PR body, so this file does not need a follow-up commit.

Validation not run: no production or deploy steps (none apply). The file-symlink sub-case of T09 runs only where the OS allows unprivileged file symlinks (Linux CI). On Windows without symlink privilege, the directory-junction case is exercised instead.

## Mistakes observed

- none matching an AW-MISTAKE-* ledger ID. My own test-assumption error (T10 assumed same-platform digest verification) is recorded under Validation.

## Missed work

- none in scope. Follow-ups are listed under Residual risk and need owner approval.

## Follow-up

- Owner decision: whether `AWI001` CONFLICT should become a NeedsReview decision input. This PR keeps the decision rules unchanged, as the spec requires.
- Owner decision (pre-existing defect, found here, not fixed): receipt digests depend on the producing OS newline, so a receipt written on Windows fails `VerifyDigest` on Linux and vice versa. Normalizing would change the digest semantics of every historical receipt. Per the spec, I stopped and am reporting it instead of migrating.

## Residual risk

- The log is a declarative source even when its SHA-256 matches. CONSISTENT_UNVERIFIED and CONFLICT are statements about agreement between claims, not about authorship.
- Exact matching after normalization reports `Claude` vs `Claude Code` as CONFLICT. This is deliberately conservative, not a guess.
- Path checks run before the read. A filesystem race between check and read is outside the threat model: it is a local, operator-run CLI.
- Older AgentsWatch binaries ignore the new fields when reading a new receipt, and their digest check then fails. Forward compatibility of older binaries is not provided.

## Addendum 2026-11-10 (after independent review of head 5df365b; earlier text above is left as written)

- Correction 1, reserialization wording: the text above ("re-serialized identically"), `docs/EXECUTOR_IDENTITY.md`, `docs/DATA_MODEL.md` ("keep their exact JSON") and the PR body said or implied that the historical receipt re-serializes byte-identically and that its original digest verifies. That overstated what the tests show:
  - T10 compares the re-serialized text with the original only after CRLF→LF normalization and `TrimEnd('\n')`. That is equality after line-ending normalization, not original-bytes identity.
  - Strict `VerifyDigest`/`EnsureStoredDigest` and the no-mismatch-warning check run only when `Environment.NewLine` is CRLF (Windows). On Linux T10 checks only that the stored digest equals SHA-256 of the same content in CRLF form.
  - What is byte-checked is that `receipt show`/`handoff` leave the fixture file's bytes unchanged (SHA-256 before = after).
  - The docs and the T10 comments were corrected in a follow-up commit. Test logic and product code are unchanged.
- Correction 2, fixture SHA-256: the value given in "Files changed" above (`031c5110…b854`) is **not** the hash of the committed fixture. Facts:
  - The committed blob (`git show <head>:tests/AgentsWatch.Tests/Fixtures/v1-receipt-pre-executor-identity.json`) has SHA-256 `f6c056ae8d4ca02835897d7c689844292c6c38b4ce4f9c5c9c2406b3e46528ed`. It has LF line endings, because Git on the Windows machine (`core.autocrlf=true`) normalized the file at commit time and the repo had no `.gitattributes` rule for it.
  - `031c51103085827f911a0033b00e9c29fd247d043ba8e2eee6125e140737b854` is the hash of the bytes the fe1be51 CLI wrote on Windows: CRLF line breaks plus one final lone LF, from `Serialize(...)` with `Environment.NewLine` + `"\n"`. Those bytes are still present, unmodified, in the Windows worktree `C:\Users\Ivan\source\repos\_aw-vfy-010\aw-fix-executor-001` (verified 2026-11-10 with `Get-FileHash`). They are reproducible from the committed blob by converting every LF to CRLF except the final newline.
  - Other forms of the same blob do not give `031c5110…`: all-CRLF `1f8a9b4e…`, LF without the final newline `342487f6…`, BOM + CRLF `b961a069…`.
  - So `031c5110…` is verifiable only on that machine or through the stated transform, not from a fresh checkout. The authoritative hash of the committed fixture is `f6c056ae…`.
  - The test logic does not depend on either file hash: it deserializes the content and compares digests.
- Neither correction changes product code or test logic. The other review findings (VERIFIED bypass via a comma enum list, FIFO, hard links, Unicode, test gaps, accept/claims) await an owner decision and are not implemented here.

## Addendum 2026-11-10 (owner-approved blocking fix: VERIFIED bypass)

- Finding (independent review of head 5df365b): the load check matched only the exact string `verified` (ignoring case). The default `JsonStringEnumConverter` also binds comma-separated lists and numeric strings: `"unknown, verified"` and `"4"` became `Verified`; `"conflict,verified"` and `"7"` became undefined values. An attacker can recompute a valid evidence digest whenever the value round-trips through the serializer.
- RED (commit `ee576f8`, test only, `src/` unchanged): `dotnet test tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj -c Release --no-build --filter FullyQualifiedName~Negative_ForgedIdentityStatusVariant` → **exit 1, 11 total, 8 failed, 3 passed** on both Linux (box, .NET 8) and Windows (DESKTOP-V877DAD).
  - Loaded with no error, which is a bypass: `"unknown, verified"`, `"4"` and `"declared-only, consistent-unverified"`, all three with a recomputed valid digest; also `"conflict,verified"` and `"7"`, undefined values that cannot be serialized, so they had no valid digest.
  - Rejected only by a generic `JsonException`, not by the identity rule: `4`, `7` (JSON numbers) and `"declared-only,verified"`.
  - Passed already (exact-casing variants of `verified`): `"Verified"`, `"VERIFIED"`, `"vErIfIeD"`.
- Fix (commit `bc07d9f`):
  - On the raw JSON, `executorIdentity.status` must be a string exactly equal to `unknown`, `declared-only`, `consistent-unverified` or `conflict` (ordinal).
  - After binding, `Verified`, any undefined value and `authorshipVerified=true` are rejected (defense in depth).
  - `docs/EXECUTOR_IDENTITY.md` now describes this rule.
  - No custom converter, no schema change, and nothing changes for receipts written by this version.
- GREEN: full Release gate on Linux (box) exit 0, 250/250. Windows and CI results for the final head are in the PR body.
- Other review findings (FIFO, hard links, Unicode, test gaps, accept/claims) await an owner decision and are not implemented.
