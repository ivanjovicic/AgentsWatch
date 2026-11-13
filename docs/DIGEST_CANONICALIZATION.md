# Digest canonicalization (AW-FIX-DIGEST-001)

Status: normative for `RunReceipt.evidenceDigest` and `contractDigest` (receipt and active-run baseline).
Schema: unchanged (`schemaVersion` 1). No new receipt fields and no digest-algorithm marker.

## Defect

Before this fix both digests were `sha256` over `JsonSerializer.Serialize(..., RunContractJson.Options)`.
`WriteIndented = true` writes line breaks with `Environment.NewLine`. The same receipt therefore had
two different digests: CRLF on Windows and LF on Linux/macOS. A receipt or contract digest captured on
one OS failed verification on the other:

- `receipt show` warned "evidence digest mismatch";
- every mutation (`evidence record|check`, `accept record`, `claims record|check`, `drift check`)
  refused with "receipt evidence digest does not match its contents";
- `evidence check` raised the "stored contract does not match the digest captured when the run
  finished" finding.

## Canonical form (written for every new or re-signed record)

1. Serialize the record with the existing `RunContractJson.Options` (indented, camelCase, nulls omitted,
   kebab-case enums). For receipts, `evidenceDigest` is omitted (set to null), as before.
2. Replace every CRLF (`\r\n`) in the serializer output with LF (`\n`). There is no trailing newline.
3. Encode as UTF-8 **without** a BOM.
4. Digest = `"sha256:"` + lowercase hex of SHA-256 over those bytes.

The implementation is `AgentsWatch.Core.CanonicalJsonDigest`. This canonical form is byte-for-byte what
pre-fix builds already produced on Linux/macOS, so every existing LF digest is already canonical.

### Why line-break normalization cannot hide a content change

System.Text.Json escapes every control character inside string values: a CR or LF in data is written
as the two characters `\r` or `\n`, never as a raw byte. The only raw CR/LF bytes in the serialized text
are the indentation layout. Mapping CRLF to LF is a bijection between two layouts of the same content, so
two records have the same canonical digest only if their serialized content is identical. This is covered
by the test `X05_NewlineInsideStringValue_IsContent_NotLayout`.

## Accepted legacy forms (verification only)

`VerifyDigest`, `EnsureStoredDigest` and the contract-digest check in the Evidence Gate accept a stored
digest when it equals one of exactly two digests, **both recomputed from the current content**:

| Form | Produced by | Written by new code? |
|---|---|---|
| `CanonicalLf` | pre-fix Linux/macOS builds, and all builds after this fix | yes |
| `LegacyCrlf` (same JSON, LF→CRLF) | pre-fix Windows builds | **no**, accepted on read only |

The comparison is between the **stored digest value** and these two recomputed values; nothing else is
accepted as a digest. The layout of the receipt **file** plays no part in it: the file is parsed, bound
to the model and re-serialized before hashing, so a file saved with CRLF, LF, a BOM or as compact JSON
verifies as long as its bound content is unchanged (see "Integrity scope" below). What is never accepted
is a stored digest computed any other way, for example over the raw file bytes, over a lone-CR or
compact serialization, or with a BOM included in the hashed bytes.

Tamper detection is not weakened. Both accepted digests are functions of the record being verified, so
an edited field changes both and the record fails verification. Accepting two layouts of the same content
does not accept different content. A stored digest copied from another receipt or contract also fails.

`RunReceiptJson.MatchDigest` reports which form matched (`CanonicalLf`, `LegacyCrlf`, `None`).

## Historical records

No historical receipt, contract or evidence file is migrated, regenerated or re-digested. A legacy
Windows receipt keeps its CRLF digest until the user runs a command that already mutated receipts
before this fix. That command re-signs the receipt in the canonical form (existing behavior: every
mutation recomputes the digest). The digest captured in `contractDigest` at `start` is stored evidence
and is kept verbatim. It is verified against the current contract in either accepted form and is never
rewritten.

Receipt and contract **files** are still written with the platform line ending. The digest covers the
canonical serialization, not the file bytes, so line-ending conversion of the file (for example
`core.autocrlf`) does not affect verification.

## Integrity scope: canonical content, not file bytes

Two different properties are easy to confuse:

- **Integrity of the canonical content** (what the digest guarantees): the receipt, as bound to the
  model and re-serialized canonically, is the same content the digest was computed over. Any change to
  a bound field value is detected.
- **Byte identity of the JSON file** (what the digest does not guarantee): the file may be re-encoded,
  re-indented, have its line endings converted or carry extra properties and still verify. Byte identity
  of a file is only asserted where a test or a refusal path compares the bytes explicitly (for example:
  a refused mutation leaves the receipt byte-identical).

## Read paths (owner decision, 2026-11-10)

On a digest mismatch (edited content, or a missing/replaced digest):

- `receipt show` exits **1**. It still renders the receipt for inspection, but the first stdout line is
  `UNTRUSTED - evidence digest mismatch: ...` and stderr carries
  `error: UNTRUSTED receipt: evidence digest mismatch; ...`.
- `handoff` exits **1**, writes nothing (neither `.ai/runs/<id>.md` nor `.ai/handoffs/<id>.md`) and leaves
  any existing projection file untouched. stderr:
  `error: UNTRUSTED receipt: evidence digest mismatch; refusing to write a handoff or receipt projection.`

This is a behavior change ordered by the owner. Before it, both commands only warned and exited 0, and
`handoff` wrote projections from the untrusted receipt. Receipts that verify in either accepted form are
unaffected (exit 0, no banner).

Mutation commands (`evidence record|check`, `accept record`, `claims record|check`, `drift check`) were
already refusing: exit 1, empty stdout, stderr
`error: receipt is invalid: receipt evidence digest does not match its contents.`, receipt file
byte-identical. Note that on success these commands also exit 1 while the decision is `NeedsEvidence`
(the exit code reports the decision); success and refusal are told apart by stderr, not by the exit code.
On success they re-write the receipt (canonical digest; file written with the platform line ending), so
the file bytes of a successful `check` are not guaranteed to stay identical.

## Limits

- The digest is an integrity checksum, not a signature. Anyone who can edit the receipt can recompute
  a valid digest. Authenticating receipts is out of scope.
- **Unknown properties** in the receipt file are ignored by the binder and are therefore outside the
  digest: adding one does not fail verification and it is dropped on the next re-sign.
- **Duplicate properties** bind the last occurrence (System.Text.Json default). Only that bound value is
  covered; an earlier duplicate is invisible to the digest. Changing the last one is detected.
  (Characterized by `D10_DigestCoversBoundContent_NotFileBytes_DocumentedLimits`.)
  **Exception (AW-FIX-EXECUTOR-001):** a duplicate key inside `executorIdentity`, and a second
  `executorIdentity` property in the receipt, are refused on load (`InvalidDataException`; covered by
  `Negative_DuplicateExecutorIdentityKeys_AreRejected`: `receipt show` exits 1 and the evidence check
  refuses), see `docs/EXECUTOR_IDENTITY.md`. All other duplicates are still last-wins; that remains a
  residual.
- The full M1 regression (a digest computed over raw `Environment.NewLine` output instead of the canonical
  form) can only surface where `Environment.NewLine` is CRLF, i.e. in the `windows-latest` CI job; on Linux
  the raw output is already LF. The canonicalization itself is covered on every OS by
  `D09_CanonicalJsonDigest_NormalizesLayout_IndependentOfTheRunningOs` with explicit CRLF/LF inputs.
- A receipt re-signed by a post-fix build verifies on pre-fix Linux/macOS builds (same LF form), but a
  pre-fix **Windows** build will report a mismatch. It fails closed.

Tests: `tests/AgentsWatch.Tests/CrossPlatformDigestTests.cs`. Its fixtures under
`tests/AgentsWatch.Tests/Fixtures/digest/` were produced by the unchanged `fe1be51` CLI on Windows and
Linux and are kept byte-exact by `.gitattributes`.
