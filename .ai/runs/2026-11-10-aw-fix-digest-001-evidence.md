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

## Mistakes observed

none
