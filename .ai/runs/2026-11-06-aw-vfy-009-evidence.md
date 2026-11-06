# AW-VFY-009 evidence: Claims vs Diff vs Validation v1

- Prompt: AW-VFY-009; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Also includes prior step-1 tests (outside+avoid, evidence-check preserves AWD) committed as `d1603d4`.
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## Claim rules

Uses Contract + RunDelta (RunIntervalChange and PreExistingChangedFurther only) + ValidationEvidence + ScopeDriftChecker. No extra git parsing.

- TestsAdded: Supported if any attributable path classifies as Test (folder test/tests/spec/__tests__, PascalCase *Test/*Tests, `_test.` / `.test.` / `.spec.`); Unsupported if attributable files exist but none match; Unknown if no high-confidence attributable files.
- DocsOnly: Supported if every attributable file is Docs (`docs/**`, README/CHANGELOG/LICENSE, .md/.rst/.adoc); Unsupported if any non-docs file; Unknown if none attributable.
- BackendUnchanged: Unsupported if any Backend file (api/server/backend/controllers/services/repositories folders, or leftover .cs/.fs/.java/.go/.py/.rb/.kt); Supported if all files are known non-backend or nothing attributable; Unknown if any path is Other (unrecognized).
- MigrationAdded: Supported if an attributable added/untracked/renamed-to file matches migration heuristics (`migrations/`, prisma/supabase/alembic, `*Migration.cs`, `.sql` under a migration path); Unsupported if attributable files exist but none added migrations; Unknown if none attributable.
- ValidationPassed: Supported by structured Pass whose source is not Unknown; Unsupported if no validations (prose is not enough) or any Fail/TimedOut/Killed; Unknown if evidence exists but none Passed (e.g. status Unknown).
- NoUnrelatedChanges: uses ScopeDriftChecker; Unsupported on AWD001/002/003; Unknown on AWD004; Supported otherwise. PreExistingUnchanged dirt does not count.

Unknown claim types -> Unknown. Ids: empty -> cN; existing id requires --replace.

Unsupported claims: finding AWK001, Done becomes NeedsReview. Unknown claims: AWK002, Done becomes NeedsEvidence.

## Tests

PathClassifier theories. Each initial type: supported, unsupported, unknown. Extra: ValidationPassed Fail; prose without evidence; NoUnrelatedChanges with pre-existing unchanged outside file (still Supported) and ambiguous outside (Unknown); persist/project + unknown type; id allocation/overwrite refusal + CLI.

Step 1 (008): OutsideOwnedAndAvoided_EmitsBothFindings; EvidenceCheck_AfterDriftCheck_PreservesAwdFindings — both passed without production changes.

## Gate (executed)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 143, Skipped 0, Total 143.

## Smoke (temp repo)

finish after editing `src/app.cs` and `extra.txt`. `claims record DocsOnly` -> Unsupported (non-docs files). `claims record ValidationPassed --source agent-prose --text "all tests pass"` -> Unsupported (no structured validation). Decision NeedsEvidence.

## Self-review gaps

- No LLM/prose extraction (in scope to avoid).
- `value: false` is stored but not treated as a negated claim.
- Backend heuristic treats remaining compiled-language files as backend; `contest.cs` is Backend, not a test (PascalCase *Test required). Flutter/Dart widgets in `lib/` may be classified Backend via no dedicated frontend rule for `.dart`.
- Empty-delta BackendUnchanged is Supported (nothing changed), not Unknown.
- Modified (not added) migration files do not support MigrationAdded.
- No separate test that `evidence check` after `claims check` keeps AWK findings (same keep-non-AWE mechanism as AWD, which is now tested).
- UserDeclared Pass still supports ValidationPassed (structured, labeled).

Commit SHA (change): 9451849

