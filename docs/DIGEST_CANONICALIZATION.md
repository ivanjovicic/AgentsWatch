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

Nothing else is accepted: no lone CR, no BOM, no compact JSON, no other line-break placement.

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

## Read paths

`receipt show` and `handoff` both print `warning: evidence digest mismatch; canonical receipt may have
been edited.` to stderr when the stored digest matches neither form. The exit code stays 0, as it already
was for `show`. Before this fix, `handoff` printed no warning.

## Limits

- The digest is an integrity checksum, not a signature. Anyone who can edit the receipt can recompute
  a valid digest. Authenticating receipts is out of scope.
- A receipt re-signed by a post-fix build verifies on pre-fix Linux/macOS builds (same LF form), but a
  pre-fix **Windows** build will report a mismatch. It fails closed.

Tests: `tests/AgentsWatch.Tests/CrossPlatformDigestTests.cs`. Its fixtures under
`tests/AgentsWatch.Tests/Fixtures/digest/` were produced by the unchanged `fe1be51` CLI on Windows and
Linux and are kept byte-exact by `.gitattributes`.
