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

Both new fields are omitted when null. Historical receipts therefore serialize byte-identically,
keep their original `evidenceDigest`, and are never rewritten or re-digested. A receipt without
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

Receipt hashes are checksums, not signatures. Loading a receipt whose `executorIdentity.status`
is `verified`, or whose `authorshipVerified` is `true`, fails with `InvalidDataException`
(`receipt show` exits 1) instead of displaying the claim as fact.

## Comparison

- Compared pairs: declared `agent` vs log `Agent/tool`; declared `model` vs log `Model name/id`.
- Not compared (no comparable counterpart): declared `tool`, log `Model provider`, log `Client/IDE`.
  They are recorded and displayed only.
- Normalization: Unicode NFKC, every whitespace run collapsed to one space, trim, invariant
  lower-case. Placeholders (`unknown`, `n/a`, `na`, `none`, `null`, `-`, `?`, and any `unknown-*`
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
- does not exist, is not a regular file, or exceeds 64 KiB.

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
