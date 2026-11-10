# AW-FIX-DIGEST-001 Evidence

Evidence format: v2
Prompt ID: AW-FIX-DIGEST-001
Queue: owner-authorized corrective task (not a verification_mvp_2026_08_25 queue row; see Claim record)
Agent/tool: Grok Bot executor subagent
Model provider: unknown-not-exposed
Model name/id: unknown-not-exposed
Model mode/settings: unknown-not-exposed
Client/IDE: Grok Bot box (Linux) + Windows Shell on DESKTOP-V877DAD
Run mode: implementation (test-first, fail-closed)
Token budget: medium
Elapsed time: unknown-not-recorded
Phase time breakdown: unknown-not-recorded
Commit SHA: see the draft PR head (a file cannot contain its own commit SHA); base fe1be515108437b948d34b3ac28fc5af36810db9; RED commit 581f2fb

This log is itself a declarative source.

## Claim record

- Authorization: owner GO on 2026-11-10 for one narrow corrective draft PR, with no merge. Required scope: reproduce on unchanged code with RED tests; check both `RunReceiptJson.ComputeDigest` and `RunContractJson.ComputeDigest`; platform-independent canonicalization for new records; keep verifying existing Windows/Linux digests without changing historical receipts; check start/finish/show/handoff/evidence check and receipt mutations; tamper tests; strict T10 on both OSes.
- Same procedure as AW-FIX-EXECUTOR-001: `AGENTS.md` only defines claiming a prompt from `docs/prompt_queues/verification_mvp_2026_08_25.md`. No queue entry was invented and the router and queue are untouched. The claim is recorded in (1) this log, (2) the dedicated branch `fix/aw-fix-digest-001` from `origin/main` `fe1be51`, and (3) one draft PR. Before starting there was no branch, PR or worktree for this task.
- Base: `fe1be51` (current `origin/main`). Independent of PR #21: it does not contain #21 and does not depend on it.
- Worktree: `C:\Users\Ivan\source\repos\_aw-vfy-010\aw-fix-digest-001` (new). A Linux clone on the box was used for the Linux runs.
- Out of scope and untouched: AW-VFY-010 B5, AW-VFY-011/012, PR #22, model routing, counters, adjudication `362e762`, MathLearning `a7ec70f`, eval-001 hashes, and the 12 REAL runs.

## Finding (2026-11-10): no claim/lease procedure for corrective tasks

`AGENTS.md` and the router only describe claiming queue prompts. There is no lease, lock or expiry, and no path for an owner-authorized corrective task such as AW-FIX-EXECUTOR-001 or AW-FIX-DIGEST-001. Both tasks recorded their claim ad hoc in an evidence log, a dedicated branch and a draft PR. Risk: two agents can start the same corrective task, and a claim cannot be checked mechanically. Proposed follow-up, which needs an owner decision and is not done here: a short "corrective task claim" section in `AGENTS.md` (claim = evidence log + branch `fix/<task-id>` + draft PR, with a check for an existing branch/PR before starting). This finding is recorded here only. PR #21 is not widened for it.

## Reproduction (unchanged code, fe1be51)

Real fixtures were produced with the unchanged fe1be51 CLI (`contract create`, `start --agent synthetic-agent --tool synthetic-tool --model synthetic-model`, edit, `finish`), using synthetic values only:

- Windows (DESKTOP-V877DAD, `dotnet build src/AgentsWatch.Cli -c Release` at fe1be51) produced `windows-crlf`: stored evidenceDigest `sha256:cfc05948…c299a` (CRLF form), contractDigest `sha256:9f5d9cf8…15144`.
- Linux (box, .NET 8, same commit) produced `linux-lf`: evidenceDigest `sha256:ed996a3d…69398`, contractDigest `sha256:254e9491…40ce0`.
- With unchanged fe1be51 Core on Linux, the Windows receipt fails `VerifyDigest`. Its canonical LF digest is `sha256:8250f813…cafc7` and its LF contract digest is `sha256:d8c81a07…cf84`.

Inspected digests over serialized text:

- `RunReceiptJson.ComputeDigest`: `Serialize` uses `WriteIndented = true`, so `Environment.NewLine` makes it OS-dependent. **Affected.**
- `RunContractJson.ComputeDigest` (captured at `start`, copied into the receipt, compared in `EvidenceGate`): same options. **Affected.**
- `WorktreeState` fingerprint (`RunBaseline.cs`): built with explicit `\n` and `\0`. Not affected.
- `GitRepositoryStateReader` file hashing hashes raw file bytes within one run on one machine. Not a serialized-text digest; not affected.
- `ReceiptRedaction.RedactJsonDocument` uses a `Utf8JsonWriter` with `Indented = true`, but its output is re-parsed and never hashed directly. Not affected.

## RED (commit 581f2fb: tests, fixtures, csproj, .gitattributes; src identical to fe1be51)

Command on both OSes: `dotnet test tests/AgentsWatch.Tests/AgentsWatch.Tests.csproj -c Release --no-build --filter FullyQualifiedName~CrossPlatformDigestTests`, after `dotnet build AgentsWatch.sln -c Release` (exit 0, 0 errors).

- **Linux (box): exit 1, 36 total, 15 failed, 21 passed.** Failed:
  - D01 (windows-crlf): receipt does not verify on Unix.
  - D04 (windows-crlf): `show` printed "evidence digest mismatch".
  - D06 (windows-crlf): `evidence check` returned "receipt is invalid: receipt evidence digest does not match its contents."
  - D07 (windows-crlf) × 6 mutations (evidence-record, evidence-check, accept-record, claims-record, claims-check, drift-check): all refused with "does not match its contents".
  - X01 and X02 (windows-crlf): untampered legacy receipt does not verify. X01 and X02 (linux-lf): `handoff` prints no mismatch warning for a tampered receipt.
  - X03 (windows-crlf): line-ending-only rewrite does not verify.
  - X06 (windows-crlf): `evidence check` refused before the contract check.
- **Windows: exit 1, 36 total, 23 failed, 13 passed.** Failed:
  - D01 (linux-lf): does not verify on Win32NT.
  - D02 × 2 and D03 × 2: digests are CRLF form, not canonical, e.g. expected `sha256:8250f813…`, actual `sha256:cfc05948…`.
  - D04 (linux-lf): `show` warning.
  - D06 (linux-lf): "receipt is invalid".
  - D07 (linux-lf) × 6: refused. D07 (windows-crlf) × 3 (evidence-record, accept-record, claims-record): re-signed digest is CRLF form, not canonical.
  - X01 and X02 (linux-lf): untampered legacy receipt does not verify. X01 and X02 (windows-crlf): no handoff warning.
  - X03 (linux-lf); X06 (linux-lf).
  - X07: `start` contractDigest is not canonical (expected `sha256:73b3adf2…`, actual `sha256:bc7bbb65…`).
- Raw RED logs (outside the repo): Windows `C:\Users\Ivan\source\repos\_aw-vfy-010\aw-fix-digest-001-red-windows.txt`; Linux `/tmp/red-linux.txt` on the box.

Tests that pass on unchanged code are regression guards, not RED: D00 fixture byte-exactness; X04 digest swap; X05 newline-in-string; the same-OS halves of D01–D07 and X03/X06.

## Fix (commit 7ae6359)

- `src/AgentsWatch.Core/CanonicalJsonDigest.cs` (new). Canonical form: the existing serialization with CRLF→LF, UTF-8 without BOM, `"sha256:"` + lowercase hex. `Match` accepts exactly the canonical LF form and the legacy CRLF form, both recomputed from the current content.
- `RunReceiptJson.ComputeDigest` now writes the canonical form; `VerifyDigest`/`EnsureStoredDigest` accept LF or legacy CRLF; `MatchDigest` (new) reports which form matched.
- `RunContractJson.ComputeDigest` writes the canonical form; `MatchesDigest` (new) is used by `EvidenceGate` for the stored `contractDigest`, which is kept verbatim.
- `handoff` now prints the same digest-mismatch warning as `receipt show` (exit code unchanged, 0).
- Docs: `docs/DIGEST_CANONICALIZATION.md` (new); pointers in `docs/DATA_MODEL.md` and `docs/RECEIPT_REDACTION.md`.
- GREEN-phase test addition: `D08_MatchDigest_ReportsExactlyWhichAcceptedFormMatched`. It uses the new API, so it was not part of RED.
- Not changed: `schemaVersion` (still 1), receipt fields, file write format, any historical receipt, contract or evidence file.

## GREEN

- Linux (box, .NET SDK 8.0.426), head 7ae6359: `dotnet build AgentsWatch.sln -c Release` exit 0 (0 warnings, 0 errors); `dotnet test AgentsWatch.sln -c Release` **exit 0, 234/234** passed (197 pre-existing + 37 new).
- Windows (DESKTOP-V877DAD, .NET SDK 10.0.201 → net8.0), head 7ae6359: `dotnet restore` exit 0; `dotnet build AgentsWatch.sln --configuration Release` exit 0 (0 warnings, 0 errors); `dotnet test AgentsWatch.sln --configuration Release` **exit 0, 234/234**. `git status` clean, and the fixture SHA-256 values match exactly on the Windows checkout (`core.autocrlf=true`, protected by `.gitattributes -text`).
- `git diff --check` clean.
- CI for the exact PR head is listed in the PR body (a file cannot contain its own commit SHA).

## T10 (PR #21) strict on both OSes: approach

This PR is independent of #21, so it does not touch #21's T10. Plan, after this PR is merged with a separate owner GO:

1. On `fix/aw-fix-executor-001`, merge `origin/main` with a normal merge commit (no rebase, no force push). There is one trivial conflict in `docs/DATA_MODEL.md`: both PRs add one paragraph after the receipt field list; keep both.
2. Make T10 strict: remove the `Environment.NewLine == "\r\n"` guard, assert `VerifyDigest`, `EnsureStoredDigest` and `MatchDigest == LegacyCrlf` on every OS, and assert no "digest mismatch" warning from `show` on every OS.
3. Push normally and require exact-head CI on Windows and Ubuntu.

Proof that the plan works, with nothing pushed: a local throwaway branch on the box (`local/t10-strict-proof` = #21 head `1c26381` merged with this branch, plus the T10 patch) gave `dotnet test AgentsWatch.sln -c Release` **exit 0, 287/287** on Linux, including the strict T10. The same strict T10 failed on Ubuntu CI without this fix (run 42790834828, job 129743529802). The patch is 6 insertions and 15 deletions in `ExecutorIdentityFlowTests.cs`.

## Residuals

- A digest is an integrity checksum, not a signature: anyone who can edit a receipt can recompute a valid digest. Authentication is out of scope.
- Forward compatibility: a receipt re-signed by this version, or any new receipt, still verifies on pre-fix Linux/macOS builds (same LF form) but **not** on pre-fix Windows builds. They fail closed with a warning on show and a refusal on mutation.
- Legacy Windows receipts keep their CRLF digest until a mutation command re-signs them in the canonical form. This is existing re-sign-on-mutation behavior; nothing is migrated proactively.
- `handoff` warns but still writes the projection, consistent with `show`. Whether a mismatch should block `handoff` is an owner decision.
- Merge interaction with #21: one trivial `docs/DATA_MODEL.md` conflict, described above.
- Missing claim/lease procedure for corrective tasks: see the Finding above. It needs an owner decision.

## Mistakes observed

none
