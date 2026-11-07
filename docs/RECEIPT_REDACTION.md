# Receipt redaction and digest integrity

Status: normative for stored receipts, their projections, `.delta.json` sidecars, and the `agentswatch finish` CLI summary.

## When redaction runs

The Evidence Gate evaluates findings and the decision against in-memory receipt values (including raw `CommandDisplay` and change paths). After that evaluation, and before `evidenceDigest` is computed and the canonical JSON is written, sentinel-like secret values are replaced with `[redacted]`.

Redaction walks parsed JSON **string values** only. It must not run assignment-style `\S+` patterns against a serialized document: that can consume closing quotes, commas, and braces when a secret is the last token in a JSON string.

Markdown receipts, handoff summaries, and CLI banners (including the successful `finish` summary) are projections of the stored receipt. They must not re-introduce raw command text or paths from an earlier in-memory copy. Absolute workspace paths are not required in displayed artifact locations; `finish` prints a repository-relative path when it can, then applies the same redaction policy.

The finish sidecar `.agentwatch/runs/<run-id>.delta.json` is written from the raw in-memory finish record (so first-pass evaluation can still see original paths) but the bytes stored on disk are passed through the same JSON-string redaction before the file is moved into place.

## Privacy boundary

Protected by this redaction pass:

- canonical `.agentwatch/runs/<run-id>.json`
- Markdown and handoff projections regenerated from that receipt
- CLI output printed from the stored receipt, including the successful `finish` summary (change counters, HEAD transition, attribution categories, identifiers, and decision)
- `finish` warning and error text, after the same sentinel-like substitution used for receipt strings
- artifact locations shown by `finish` (`Receipt:` / `Projection:`), formatted as repository-relative paths when they fall under the workspace root
- `.agentwatch/runs/<run-id>.delta.json` at persist time

Not rewritten by this pass, and still treated as local-sensitive (do not distribute):

- `.agentwatch/active-runs/<run-id>.json` start baselines, which may contain raw worktree paths from `start` time
- any operator logs, terminals, or copies made outside AgentsWatch

The successful `finish` banner is rendered from `result.Receipt` (already redacted) rather than the raw in-memory `result.Record`. It does not recompute or mutate the receipt or its digest.

This document does **not** claim that every raw input is absent from all AgentsWatch storage, or that every possible secret shape is recognized. Only the artifacts listed above are in scope, and only documented sentinel-like patterns are replaced.

## What is redacted

Redaction is substring replacement inside string values for sentinel-like shapes, including:

- invented privacy-test tokens matching `AWSENTINEL-TOKEN-…`
- common secret shapes (cloud access-key prefixes, `Bearer` credentials, `password=` / `token=` / `api_key=` style assignments)

Only the matching span is replaced, so a path such as `notes/<token>.txt` becomes `notes/[redacted].txt`.

## Scope replay after path redaction

Scope drift is evaluated on raw paths, then paths are stored in redacted form. A later `evidence check` recomputes scope from the stored (redacted) paths, which may no longer match a narrow `avoidPaths` or `ownedPaths` pattern. When a run-interval or further-changed path already contains `[redacted]`, or will after redaction, the first gate fail-closes with `AWD004` (`NeedsReview`) so finish and later checks stay aligned. Pre-existing unchanged dirty paths are still excluded from scope drift. Raw paths are not persisted as the fix.

## Digest semantics

`evidenceDigest` is `sha256` over the canonical JSON **with the digest field omitted**, after redaction. Integrity verification therefore answers: “does this published receipt still match the bytes that were signed?” It does not preserve or authenticate the raw operator input that was redacted.

Consequences:

1. A stored receipt that contains `[redacted]` and a matching digest is intact as published.
2. Editing the redacted text, or restoring a secret into the JSON, fails digest verification.
3. Redaction is idempotent: applying it to an already-redacted receipt does not change the digest.
4. Mandatory-validation and criterion-binding comparisons treat a raw command and its redacted form as the same command, so a later `evidence check` of a stored (redacted) receipt does not spuriously report a missing validation.

Contracts are not rewritten by receipt redaction. Do not put secrets in `--validate` or `--accept-validation` text.
