# AI-ONLY-EVAL-001 - Adjudication and final comparison v1 (BACKEND-TEST-048)

> **AI_ONLY_EXPERIMENTAL.** Not counted toward the human-validated GO threshold. Outside the AW-VFY-010 12/30 ledger (Gate 5 stays ADJUST / PAUSE at 12/30). Not an independent adjudication. The formal human pilot remains BLOCKED on (1) an independent second human adjudicator and (2) separate explicit owner authorization.

Kit: `docs/evidence/aw-eval-kit/03-independent-oracle-worksheet.md` and `04-final-comparison-worksheet.md` at AgentsWatch main `fe1be515108437b948d34b3ac28fc5af36810db9`, plus the measurement rules in `preregistration-v1.md`.
Language: English. Times: Europe/Belgrade (UTC where the source is UTC).

## 1. Independence limitation (read first)

- The AI adjudicator is Grok Bot, the same bot and context as the operator. It wrote the preregistration and the RunContract and ran `agentswatch start`/`finish`. This adjudication is therefore NOT independent.
- The implementer (Claude) and the blind reviewer (ChatGPT) were both run manually by the owner; their outputs reached the operator only as relayed by the owner (PR #50 on GitHub; review text in owner messages t53u/t54u).
- Kit 03 verdict on independence: `NOT_INDEPENDENT`. This run cannot count as an independently confirmed decision change under any outcome.

## 2. Inputs

| Input | Reference |
|---|---|
| Preregistration v1 | `aae1f16c70ef103787af2cab09f29a879ef553d6`, sha256 `617ede30d6095f7d0f96d1d8ee99930e1cd7b6514f7a233823a348238ce21c66` |
| Sealed baseline review v1 | `c8df1e654424629507d302812712d3007ac1cc3d`, sha256 `57465c1eb682b8727319c5238839fcd4555461ff00ed0347e1e539d1b5f1c083` (re-hashed from the commit before this adjudication: match) |
| Seal manifest rows 1-2 | `37d9360`, `2080f33` |
| Code | ivanjovicic/MathLearning `a7ec70f1f98df9b5add8e70bc1be9dc507d75ffe` (draft PR #50, open, not merged) vs base `b8acd2294d9677bc53a1a85acf3406595c1b49ef` |
| CI | Database Validation run 42723907114: success on `a7ec70f` (2026-11-10 00:52:19-00:57:06). Backend Agent System Validation run 42723907180: success on `a7ec70f` (00:52:19-00:52:39). |
| AgentsWatch receipt | run `ai-only-eval-001-backend-test-048-20261109t233612z-14def4`, receipt sha256 `fa1b3d8d78b43c084b5ef7aeb348d189afa0ab45ca1eb15f780500cb576f37f2`, finish stdout sha256 `b7367b689f80187fa8227360e8351af3352e029924e5c962f9e9493a48727e02` (operator-only copies; not published) |
| Operator event log | operator-only `event-log.txt` |

## 3. Ordering and integrity (kit 03 checklist)

| Question | Answer | Reason |
|---|---|---|
| Sealed provisional decision differs from final? | no | Final remains `ACCEPT_AS_PARTIAL`; the receipt adds nothing that changes it (section 6). |
| Change justified by a concrete AW finding? | no | No change. |
| Key finding classification | `ALREADY_NOTICED_IN_BASELINE` | See section 5. |
| Manifest hash matches original frozen bytes? | yes | Both frozen files re-hash to the manifest digests from their commits; no later diff. |
| `agentswatch start` before code changes? | yes | Start 2026-11-10 00:36:12 (receipt `startedAtUtc` 2026-11-09T23:36:12Z) on clean `b8acd22`; implementation commit authored 00:51:59; the PR run log states start 23:47:32Z (00:47:32). |
| Separate remote proof for prereg (before start) and baseline (before finish)? | yes | Prereg: GitHub activity 49639631760, 00:34:38. Baseline: activity 49643462044, 01:18:56; manifest row 2: 49643505141, 01:19:28. Finish: 01:20:11. |
| Reviewer saw start output, dirty counts, baseline metadata or receipt before the seal? | no | Start output stayed operator-only; the receipt did not exist until 01:20:42. The reviewer read MathLearning's own `.ai/runs` log in the PR diff (disclosed caveat, not AW output). |
| `finish` / receipt only after remote baseline-seal proof? | yes | 01:20:11 > 01:19:28. |
| One of the 12 historical REAL runs or their work? | no | |
| False alarm? | no material false alarm | Section 5. |
| Material false Done, privacy leak or misattribution by AW? | no | No Done claim (`needs-evidence`); no secrets/paths in receipt or projections; file attribution matches the PR diff exactly. Executor metadata issue: section 4. |

## 4. Attribution and the false-metadata finding

- Owner declaration (t54s262): "Kod je napisao Claude; Codex/OpenAI/gpt-6 u run logu je pogrešno." Recorded as an owner declaration. NOT independently verified: commit `a7ec70f` and PR #50 are authored by `ivanjovicic` with no co-author trailer, so Git cannot confirm the tool.
- **Finding F-META-1 (implementation artifact, false metadata):** `.ai/runs/2026-11-09-BACKEND-TEST-048-evidence.md:6-9` at `a7ec70f` states `Agent/tool: Codex`, `Model provider: OpenAI`, `Model name/id: gpt-6`, `Client/IDE: ChatGPT Work`. Per the owner declaration this is false. Severity: material for evidence integrity of the MathLearning run log (provenance claim is wrong), not for the code behaviour.
- **Detected by AgentsWatch: no.** AW does not read or cross-check run-log provenance. It was found by the operator during adjudication, after the receipt.
- The AW receipt's `executorMetadata` (`claude` / `owner-run-manual`) was typed by the operator at `start` from the preregistration. It now agrees with the owner declaration, but AW itself did not establish it.
- File-level attribution by AW is correct: 7 `RunIntervalChange` entries equal to the PR #50 diff, all inside the contract's owned paths, each marked "authorship not proven"; 0 pre-existing, 0 ambiguous; HEAD transition fast-forward `b8acd22 -> a7ec70f`.
- Review independence: implementer Claude vs reviewer ChatGPT is cross-vendor. Per the owner declaration the review is NOT contaminated. Classification stays `NOT_CONTAMINATED` with the disclosed caveats from the sealed review.

## 5. Per-finding classification

### 5.1 AgentsWatch receipt findings (12)

Rule applied: a "missing evidence" finding is not a false positive just because evidence exists elsewhere; AW was never given CI evidence (no `agentswatch evidence record` was run), so the findings are accurate statements about the receipt. The question is whether they told the reviewer anything the sealed baseline did not already establish.

| # | Code | Finding | Verified against code/CI | Classification | Material? |
|---|---|---|---|---|---|
| e1 | AWE002 | Mandatory v1 (`dotnet test ... IndexMaintenance|IndexBloat`) has no recorded evidence | True for the receipt. CI 42723907114 ran the suite on `a7ec70f` (success); the reviewer inspected the TRX (1285/1285) and reran 10/10 locally. | `ALREADY_NOTICED_IN_BASELINE` (baseline section 4 judged test evidence "delimično" with specific gaps) | no |
| e2 | AWE002 | Mandatory v2 (Release build) has no recorded evidence | True for the receipt. CI build passed; the reviewer reports a local build pass. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e3 | AWE006 | Expected evidence "draft PR diff ..." not backed | True for the receipt. The reviewer read the diff and confirmed head/base/draft state (section 3). | `ALREADY_NOTICED_IN_BASELINE` | no |
| e4 | AWE006 | Expected evidence "PostgreSQL 16 regression results ..." not backed | True for the receipt. Baseline confirmed both PG tests executed in CI and named what they do not prove. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e5 | AWE006 | Expected evidence "Release build result" not backed | Same as e2. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e6 | AWE006 | Expected evidence ".ai/runs log with Completion % < 100 and residuals" not backed | True for the receipt. Baseline read the log (Completion 65%, residuals) and flagged it stale (`:52-55`). | `ALREADY_NOTICED_IN_BASELINE` | no |
| e7 | AWE007 | ac1 (REINDEX path removed) unknown | Code: `ReindexAsync` removed; `RebuildCorruptedIndexesAsync` now only runs ANALYZE (`IndexMaintenanceService.cs:24-42`). Baseline confirmed removal and flagged the weak REINDEX regression test. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e8 | AWE007 | ac2 (UNKNOWN health) unknown | Code: `Status = "UNKNOWN"`, `BloatPercentage = null` (`:89, :164-165, :242-243, :255`). Baseline confirmed this and found the `bloatedIndexes = 0` hardcode. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e9 | AWE007 | ac3 (ANALYZE kept) unknown | Code: `UpdateStatisticsAsync` / `ANALYZE` at `:218`. Baseline confirmed. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e10 | AWE007 | ac4 (no CREATE EXTENSION / prod) unknown | Diff has no `CREATE EXTENSION`; contract test asserts it absent (`IndexMaintenanceSqlContractTests.cs:21`). Baseline confirmed. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e11 | AWE007 | ac5 (PG16 regression proof) unknown | Baseline: "delimično", no degraded fixture, no red result, REINDEX test cannot discriminate. | `ALREADY_NOTICED_IN_BASELINE` | no |
| e12 | AWE007 | ac6 (PARTIAL draft PR + residuals) unknown | PR #50 draft, open; run log 65%; residuals in log and PR body. Baseline confirmed PARTIAL and listed extra residuals. | `ALREADY_NOTICED_IN_BASELINE` | no |

Counts: `ALREADY_NOTICED_IN_BASELINE` 12; `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED` 0; `NEWLY_SURFACED` 0; `FALSE_POSITIVE` 0 material, 0 immaterial. Note: the six AWE007 findings are a mechanical consequence of no `accept record` entries; they carry no semantic judgement.

### 5.2 Baseline findings verified against code

| Baseline finding | Verified | Surfaced by AW? |
|---|---|---|
| major: REINDEX regression test cannot detect a reintroduced REINDEX (one-row `Questions_pkey`, OID compare) | Confirmed: `IndexBloatPostgresTests.cs:69-83, 119-123`; only string guard `DoesNotContain("REINDEX INDEX")` at `IndexMaintenanceSqlContractTests.cs:20`. The PG 16.15 OID/relfilenode measurement was not re-run: UNKNOWN. | no |
| major: `bloatedIndexes = 0` hardcoded | Confirmed: `MaintenanceEndpoints.cs:31`; locked by `MaintenanceEndpointContractTests.cs:127`. | no |
| minor: `success` always true; ANALYZE failure -> 500 | Confirmed `success = report.Errors.Count == 0` at `MaintenanceEndpoints.cs:29`, service no longer adds Errors (`IndexMaintenanceService.cs:24-42`). The 500 path was not executed: UNKNOWN. | no |
| minor: `unknownBloatIndexes` only counts indexes > 1 MB | Confirmed: filter at `IndexMaintenanceService.cs:145`, count at `MaintenanceEndpoints.cs:32`. | no |
| minor: legacy "Rebuild..." names | Confirmed (`:24`, POST `/rebuild-indexes`). | no |
| minor: two connections in a row | Confirmed (`:30-31`). | no |
| minor: stale run log | Confirmed (`.ai/runs/...-evidence.md:52-55`). | no |
| residual: CLI still shows HEALTHY for null; tool not in `MathLearning.slnx`; `openapi.yaml` text | Not re-verified in this pass: UNKNOWN. Author lists the CLI residual in PR body. | no |
| caveat: PG tests early-return outside CI | Confirmed (`IndexBloatPostgresTests.cs:13, 61, 86-87`). | no |

### 5.3 Semantic defects AgentsWatch missed

- `bloatedIndexes = 0` hardcoded at `MaintenanceEndpoints.cs:31` (contradicts Option B).
- Weak REINDEX regression test (cannot fail on a reintroduced REINDEX).
- CLI residual (`IndexMaintenanceTool`), outside the contract's owned paths and not built by CI.
- False run-log agent metadata (F-META-1).

This is expected: AgentsWatch is evidence/attribution tooling, not semantic code review. It did not claim to check these, and it did not.

## 6. Decision change

**NO.** The sealed decision was `ACCEPT_AS_PARTIAL` with two majors. The receipt (`needs-evidence`, 2 x AWE002, 4 x AWE006, 6 x AWE007) only says that no validation evidence and no criterion acceptance were recorded inside AgentsWatch. The reviewer had already verified CI and TRX directly, judged the evidence as partial, and confirmed PARTIAL status. Nothing in the receipt would move the decision to REQUEST_CHANGES or ACCEPT. Kit 03 verdict: `NO_DECISION_CHANGE` (and `NOT_INDEPENDENT`).

## 7. Timing and overhead (kit 04)

| Field | Value |
|---|---|
| Baseline review time (per reviewer) | 6 min (01:04-01:10) |
| AW `finish` (operator wall clock) | 31 s (01:20:11-01:20:42) |
| AW `start` | a few seconds; contract file written 00:35:56, start recorded 00:36:12. Exact duration not clocked: UNKNOWN |
| Contract authoring time | UNKNOWN (not clocked) |
| CLI build (one-off setup) | UNKNOWN (not clocked) |
| Reviewer minutes on the AW layer | UNKNOWN / not measured: the receipt was never shown to the reviewer |
| Start / end of AW review (kit 04) | UNKNOWN (no reviewer AW pass took place) |
| Setup + AW minutes (sum) | UNKNOWN (at least one term UNKNOWN) |
| Protocol steps (prereg, seals, manifests) | Protocol overhead, not AW-intrinsic. Push times 00:34:38, 00:35:13, 01:18:56, 01:19:28; drafting time UNKNOWN |
| Material false alarms | 0 |

## 8. AgentsWatch defects observed in this run

1. `finish` writes Markdown projections to `.ai/runs/<run-id>.md` and `.ai/handoffs/<run-id>.md` in the target repo, outside the ignored `.agentwatch/`. In MathLearning, `.ai/runs/` is the repo's own evidence directory with its own validator; a `git add -A` would commit AW output into a reviewer-visible PR. Both files are still untracked in the worktree, untouched, left for the owner.
2. Ignoring `.agentwatch/` needed an entry in `info/exclude`; for a linked worktree that file is the shared `MathLearning/.git/info/exclude`, so the entry affects every MathLearning worktree, including the primary checkout.
3. No validations reached the receipt: the implementer did not use AW and nobody ran `agentswatch evidence record` (CI could have been imported with `--source ci`). Result: `needs-evidence` by construction, even with green CI on the same head.
4. `executorMetadata` is operator-typed free text at `start`; AW cannot verify or cross-check it, and it does not compare it with run-log provenance (F-META-1 went undetected).

## 9. Verdict under the product rule

Product rule: "GO samo kada postoje proverljivi slučajevi promene odluke uz prihvatljiv dodatni trošak; inače NARROW ili STOP."

- This run produced no verifiable decision-change case. All 12 AW findings were already established by the sealed baseline review.
- The receipt correctly avoided a false Done and attributed files correctly, but per the kit, technically correct blocking of an unverified Done is not GO.
- Signal from this run: **NARROW / STOP, not GO.** It is one AI-only, non-independent data point; it does not decide the cohort and does not enter the 12/30 ledger or the human GO counter.

## 10. Final classification

- Run classification: `AI_ONLY_EXPERIMENTAL`; not eligible for the GO counter (AI-only, `NOT_INDEPENDENT`).
- Contamination: `NOT_CONTAMINATED` (per owner declaration; disclosed caveats stand).
- Adjudicator verdict: `NO_DECISION_CHANGE`.
- STOP conditions: none triggered by AgentsWatch. F-META-1 is a false-metadata finding in the implementation artifact, recorded here, not an AW misattribution.