# AW-VFY-008/009 review-fix delivery evidence

- Prompt: user review + deliver to main (not AW-VFY-010)
- Range reviewed: `8cabed7..8928c28` (006/007 follow-up, 008, 009) plus this fix commit
- Run mode: implementation (focused defect fixes)
- Model/client: unknown-not-exposed; timing: unknown-not-recorded
- Basis before this fix: local HEAD `8928c28`, origin/main `d078295`, CI run 42192082705 green (79/79)
- Timezone: Europe/Belgrade

## Claim value contract

Boolean claim values are `true`/`yes`/`1` and `false`/`no`/`0` (trim, case-insensitive).

- `false`/`no`/`0` is evaluated as the negated affirmative check: Supported <-> Unsupported; Unknown stays Unknown, with a `negated claim (...=false)` reason prefix.
- Non-boolean values are rejected at `claims record` (not stored) and treated as Unknown in `EvaluateOne` so already-persisted invalid values remain explainable on `check`.

This matches the structured claim contract (a boolean assertion about the run) better than silently ignoring `value`.

Tests: `FalseValue_IsEvaluatedAsNegatedClaim`; `NonBooleanValue_IsRejectedAtRecordAndUnknownAtEvaluate`.

## Review findings — fixed

1. `value: false` ignored (009 self-review). Fixed as negation; tested.
2. CLI `claims record --replace` on a middle id could report the wrong claim if lookup used last-match instead of the written id. `EvidenceCommandResult.RecordedClaim` is set from the persisted id; CLI prints that row. Tested in `Record_AllocatesIds_RefusesOverwrite_AndCliChecksFalseClaims` (replace c1 while c2 exists).
3. Overwrite-refusal test used a non-boolean value (`again`), which would now fail at the boolean gate before the id clash. Test now uses `false`.
4. No test that `evidence check` after `claims check` keeps AWK findings. Added `EvidenceCheck_AfterClaimsCheck_PreservesAwkFindings`.
5. Finding preservation across the other pair of writers: `DriftCheck_AfterClaimsCheck_PreservesAwkAndClaimsCheckPreservesAwd` (AWD/AWK survive each other's strip-and-rebuild).
6. Windows separators: `PathClassifier` already goes through `RepositoryPath.Normalize` (`\` -> `/`). Locked with InlineData for `tests\FooTests.cs`, `src\Services\Billing.cs`, `docs\guide.md`.

## Review findings — accepted (no code change)

- Decision order: Failed > Blocked > NeedsApproval > NeedsReview (unsupported AC, AWD001-003, AWK001) > NeedsEvidence (missing validation, unknown AC, AWK002) > Done. AWD004/AWR002 ambiguous also downgrade Done to NeedsReview. If the run is already NeedsEvidence (e.g. missing mandatory validation), AWK001/AWD001-003 findings are preserved but do not override the decision to NeedsReview. Same pattern as AWD004 + missing validation.
- Finish recovery from `8cabed7` is sound: orphan delta (record present, receipt missing) is removed and finish continues; receipt `WriteNew` failure deletes the just-written delta and keeps the active baseline. If that cleanup itself fails, retry still uses the orphan-delta path. Accepted leftover risk: an unreadable leftover delta could surface as a later IO error.
- AWE006 matching remains substring / case-insensitive field contains, not a typed catalog.
- Path matching is ordinal (case-sensitive). `src/**` does not match a file named `src`.
- No command runner; real CLI acceptance criteria stay Unknown, so Done stays rare without a later AC evaluator.
- Backend heuristic treats leftover compiled-language files (including `.cs` not otherwise classified) as Backend; `contest.cs` is Backend. No Flutter/Dart-specific frontend rule (`lib/*.dart` may classify as Backend).
- Empty-delta `BackendUnchanged` is Supported (nothing changed), not Unknown.
- Modified (not added/untracked/renamed-to) migration files do not support `MigrationAdded`.
- `UserDeclared` Pass still supports `ValidationPassed` because it is structured labeled evidence.
- No LLM/prose claim extraction (in scope to avoid).
- Git porcelain paths are `/`; backslash inputs are normalized only at RepositoryPath/PathClassifier boundaries.

## Gate (executed, Europe/Belgrade)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 150, Skipped 0, Total 150.

Counts vs previous 143: +4 claim facts (false value, non-boolean reject, AWK after evidence, AWK/AWD cross-preservation) and +3 PathClassifier Windows-separator cases.

## Queue / router

AW-VFY-001..009 treated as Done after this delivery commit. AW-VFY-010 remains Ready now (requires dogfood) and was not started. CI on the pushed head is recorded after `git push` (not claimed here).
