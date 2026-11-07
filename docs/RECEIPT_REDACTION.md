# Receipt redaction and digest integrity

Status: normative for stored receipts and their projections.

## When redaction runs

The Evidence Gate evaluates findings and the decision against in-memory receipt values (including raw `CommandDisplay` and change paths). After that evaluation, and before `evidenceDigest` is computed and the canonical JSON is written, sentinel-like secret values are replaced with `[redacted]`.

Markdown receipts, handoff summaries, and CLI banners are projections of the stored receipt. They must not re-introduce raw command text or paths from an earlier in-memory copy.

## What is redacted

Redaction is substring replacement for sentinel-like values, including:

- invented privacy-test tokens matching `AWSENTINEL-TOKEN-…`
- common secret shapes (cloud access-key prefixes, `Bearer` credentials, `password=` / `token=` / `api_key=` style assignments)

Only the matching span is replaced, so a path such as `notes/<token>.txt` becomes `notes/[redacted].txt`.

## Digest semantics

`evidenceDigest` is `sha256` over the canonical JSON **with the digest field omitted**, after redaction. Integrity verification therefore answers: “does this published receipt still match the bytes that were signed?” It does not preserve or authenticate the raw operator input that was redacted.

Consequences:

1. A stored receipt that contains `[redacted]` and a matching digest is intact as published.
2. Editing the redacted text, or restoring a secret into the JSON, fails digest verification.
3. Redaction is idempotent: applying it to an already-redacted receipt does not change the digest.
4. Mandatory-validation and criterion-binding comparisons treat a raw command and its redacted form as the same command, so a later `evidence check` of a stored (redacted) receipt does not spuriously report a missing validation.

Raw secret material is not retained in the canonical receipt. Operators who need the original command or path must keep that record outside AgentsWatch.

Contracts are not rewritten by receipt redaction. Do not put secrets in `--validate` or `--accept-validation` text.
