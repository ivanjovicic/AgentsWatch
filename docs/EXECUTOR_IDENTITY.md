# Executor identity trust boundary (AW-FIX-EXECUTOR-001)

Status: normative for executor identity in RunReceipt v1.

```text
Observed changes != proven authorship.
Declared identity != verified identity.
Matching claims  != independent proof.
```

AgentsWatch records who the operator *says* executed a run and, optionally, what an
implementation run log *says*. It compares the two deterministically. It never proves
who authored the changes, and it never assigns `VERIFIED`.

## Receipt fields (additive, optional)

| Field | Meaning |
|---|---|
| `executorMetadata` | **declaredExecutor**: what the operator passed to `start --agent/--tool/--model`. Unchanged. |
| `observedExecutorEvidence` | Present only when `finish --executor-log` was used: `sourceKind` (`implementation-run-log`), workspace-relative `sourcePath`, `sourceSha256` of the file bytes, `format`, and at most `agentTool`, `modelProvider`, `modelName`, `clientIde` (or a `problem` when the content is unusable). |
| `executorIdentity` | `status`, `authorshipVerified` (always `false`), `reason`, `comparedFields`, `conflictingFields`, `sourceRefs`. |

Both new fields are omitted when null. A historical receipt therefore re-serializes to the same JSON
text after line-ending normalization (CRLF/LF and the trailing newline are ignored, so this is not an
original-bytes comparison). It keeps its stored `evidenceDigest`, and it is never rewritten or re-digested:
`receipt show` and `handoff` leave the file's bytes unchanged. On a different OS from the one that
produced it, strict verification of that stored digest needs AW-FIX-DIGEST-001. A receipt without
`executorIdentity` is displayed as `Identity status: UNKNOWN` with reason "not assessed".
New receipts always carry `executorIdentity`; it is covered by `evidenceDigest` and preserved by
`evidence`/`accept`/`claims` re-evaluation.

## Statuses

| Status (JSON / display) | Meaning |
|---|---|
| `unknown` / `UNKNOWN` | Not enough usable data to compare (no declaration, unusable log, or log without a declaration). |
| `declared-only` / `DECLARED_ONLY` | Only the operator declaration is usable (no log, or no comparable field in the log). |
| `consistent-unverified` / `CONSISTENT_UNVERIFIED` | Declaration and log agree on every compared field. Agreement is **not** verification. |
| `conflict` / `CONFLICT` | Declaration and log disagree on at least one compared field. |
| `verified` / `VERIFIED` | Reserved for a future trusted provenance mechanism. Never assigned. |

Receipt hashes are checksums, not signatures: anyone who edits a receipt can recompute a valid digest.
Loading a receipt therefore fails with `InvalidDataException` (`receipt show` exits 1; `evidence`,
`accept`, `claims` and `drift` refuse it) when:

- `executorIdentity.status` is not a JSON string exactly equal to one of `unknown`, `declared-only`,
  `consistent-unverified`, `conflict` (case-sensitive, no surrounding whitespace). This rejects
  `verified` in any casing, comma-separated lists such as `unknown, verified` or `conflict,verified`,
  numeric strings such as `"4"`, JSON numbers, and any other spelling that the default enum converter
  would otherwise bind to `Verified` or to an undefined value;
- after binding, the status is `Verified` or is not a defined value (defense in depth);
- `authorshipVerified` is `true`; or
- `executorIdentity` contains a duplicate key (for example a shadowed earlier `"status": "verified"`),
  or the receipt contains more than one `executorIdentity` property. System.Text.Json binds the last
  duplicate, so an earlier one would otherwise be invisible to validation and to the digest.

Each of these refusals is covered by tests for `receipt show` and for every receipt mutation
(`evidence record`, `evidence check`, `accept record`, `claims record`, `claims check`,
`drift check`): exit 1, the `executorIdentity` error on stderr, and the receipt file byte-identical
(`Negative_ForgedVerifiedReceipt_IsRefusedByEveryMutationCommand`). Identity fields are assessed once,
at `finish`, and every mutation keeps `executorMetadata`, `observedExecutorEvidence` and
`executorIdentity` unchanged (`IdentityFields_ArePreservedThroughEveryMutationCommand`).

The claim is refused instead of being displayed as fact.

## Comparison

- Compared pairs: declared `agent` vs log `Agent/tool`; declared `model` vs log `Model name/id`.
- Not compared (no comparable counterpart): declared `tool`, log `Model provider`, log `Client/IDE`.
  They are recorded and displayed only.
- Normalization (comparison only; stored and displayed values keep their original spelling): Unicode
  NFKC, every whitespace run collapsed to one space, trim, invariant lower-case. A fullwidth
  `Ｃｏｄｅｘ` therefore matches `Codex`; a format character such as U+200B is not removed by NFKC
  and makes the values differ (`CONFLICT`), never equal. Placeholders (`unknown`, `n/a`, `na`, `none`, `null`, `-`, `?`, and any `unknown-*`
  such as `unknown-not-exposed`) are treated as absent.
- Missing fields are never a conflict. Matching is exact after normalization, so `Claude` vs
  `Claude Code` is reported as `CONFLICT` for a human to judge rather than guessed consistent.
- Git commit author, GitHub PR author, and workspace names are not used as identity evidence.

## Finding

`AWI001` / `executor-identity-conflict` / Warning / NeedsReview is added when the status is
`CONFLICT`. It is distinct from `AWR002` (ambiguous Git change attribution). It is deliberately
outside the decision ordering (`docs/DECISION_ORDERING.md`): neither `CONFLICT` nor
`CONSISTENT_UNVERIFIED` changes `Done`/`NeedsReview`/`NeedsEvidence`. A conflict alone does
not invalidate the run or its evidence (it is not a contamination verdict).

## `finish --executor-log <relative-path>`

The log is read once, read-only, before anything is written. The CLI exits `2` and keeps the
active run (nothing is written, so the finish can be retried) when the path:

- is absolute or rooted, or contains a `..` segment;
- passes through any symbolic link or junction (any component), or resolves outside the workspace;
- is under `.agentwatch/` or `.ai/handoffs/`, or is an AgentsWatch-generated projection
  (starts with `# AgentsWatch Run Receipt` / `# Handoff - ` or carries the
  "Generated from `.agentwatch/runs/" marker);
- does not exist, is not a regular file, or exceeds 64 KiB. On Unix the file type is checked before the
  file is opened, so a FIFO, socket or device is refused without blocking (a FIFO would otherwise block
  in `open`). Windows has no filesystem FIFOs; device names such as `NUL` resolve outside the workspace
  and are refused by the path check. The FIFO test is Unix-only and is reported as Skipped on Windows.

Limits of the path confinement:

- A **hard link** inside the workspace to a file outside it is the same file under a second name and is
  read like any regular file. It cannot be told apart with the public .NET APIs on both OSes, and a
  link-count check would also refuse legitimate files, so it is a documented limitation, pinned by
  `Limitation_ExecutorLog_HardLinkToOutsideFile_IsReadAsRegularFile` on both OSes. Creating one needs
  write access to the workspace; only the five header fields are extracted, and the result is still
  only a declarative source.
- The type check and the open are two steps; a file swapped for a FIFO between them is not prevented
  (requires concurrent local write access to the workspace).

Refusal messages never echo the supplied path. Content problems do not refuse the finish; they
record `UNKNOWN` with the reason: NUL bytes, invalid UTF-8, missing or non-`v2`
`Evidence format`, duplicate identity header fields, control characters, or values over 200
characters.

Only `Key: value` lines before the first `## ` section are read, and only for `Evidence format`,
`Agent/tool`, `Model provider`, `Model name/id`, `Client/IDE`. Narrative, other headers (including
any self-declared "verified" line), and other `.ai/runs` files are never read or interpreted.
Nothing in the log is executed, and nothing is sent to the network. Stored values pass through the
standard receipt redaction before the digest; CLI, Markdown and handoff output render from the
redacted receipt.

## Display

`finish`, `receipt show` (and the `.ai/runs/<run-id>.md` projection) and `handoff` show:

```text
Executor declared: Claude
Additional source: Codex / OpenAI / gpt-6
Additional source client: ChatGPT Work
Additional source ref: evidence/impl-run.md (sha256:...)
Identity status: CONFLICT
Authorship verified: NO
Reason: Operator declaration conflicts with implementation log metadata. Neither source independently proves who authored the changes. Conflicting field(s): agent.
```

`Authorship verified` is always `NO`. The Git run-interval change display is unchanged.

Every identity value in these lines (declared agent/tool/model, the log fields, the log path and the
pre-existing `Executor:` line) is rendered with control (Cc), format (Cf: bidi overrides and isolates
such as U+202E/U+2066, zero-width characters) and line/paragraph separator (U+2028/U+2029) code points
shown as `<U+XXXX>`, so a value cannot reorder, hide or break a rendered line. This is rendering only:
the receipt stores the exact value. Changed-file paths in the run-delta lists are rendered by older code
and are not covered by this rule.
