# AI-ONLY-EVAL-001 - Preregistration v1 (BACKEND-TEST-048)

Template: `docs/evidence/aw-eval-kit/01-preregistration-template.md` at AgentsWatch main `fe1be515108437b948d34b3ac28fc5af36810db9`.
Language: English. Time zone: Europe/Belgrade (UTC given where useful).
This file is frozen after hashing. Its SHA-256 lives only in the sidecar `seal-manifest.md`, never in this file. Corrections go into a new version; this v1 stays as-is.

## Classification (read first)

- Classification: `AI_ONLY_EXPERIMENTAL`.
- NOT counted toward the human-validated GO threshold (>= 3 independently confirmed decision changes in 10-15 eligible runs).
- Does NOT touch the AW-VFY-010 12/30 ledger, Gate 5 (ADJUST / PAUSE at 12/30) or any historical evidence file.
- The formal human pilot remains BLOCKED on (1) an independent second human adjudicator and (2) separate explicit owner authorization. This run does not change that.

## Owner-approved scope (verbatim)

- isolated branch and draft PR only, no merge to main
- Option B: physical index health UNKNOWN unless supported by evidence; no automatic REINDEX
- keep the separately accounted ANALYZE behavior
- Phase 1 = PARTIAL with explicit residuals

## Roles

- Implementer: Claude, run manually by the owner (Ivan Jovicic) in the isolated MathLearning worktree. The implementer sees no AgentsWatch output.
- Blind baseline reviewer: ChatGPT, run manually by the owner. It gets Git/CI/draft-PR material only; no access to AgentsWatch output, the AgentsWatch contract/receipt, or any prior reviewer findings.
- Operator: Grok Bot (owner's assistant). Writes this preregistration, creates the worktree, RunContract, runs `agentswatch start`/`finish`, records seals.
- AI adjudicator: Grok Bot, in a separate later adjudication pass.
- Known independence limitation: the AI adjudicator shares the operator's context (it wrote this preregistration and the contract and ran AgentsWatch). Its adjudication is therefore NOT independent and cannot count as independent confirmation.

## Template fields

| Polje | Upis |
|---|---|
| Eval run id (novi, nije jedan od 12 istorijskih REAL id-jeva) | `AI-ONLY-EVAL-001` (new; not one of the 12 historical REAL ids) |
| Datum predregistracije | 2026-11-10 00:33 Europe/Belgrade (2026-11-09T23:33Z). Local time is not ordering proof; the remote push of this file is. |
| Predložio / operator CLI | Proposed by Grok Bot after a read-only eligibility check; operator CLI = Grok Bot, AgentsWatch CLI built locally from main `fe1be51`. |
| Recenzent (čovek koji daje Git/CI/PR odluku) | No human reviewer in this run. Blind baseline reviewer = ChatGPT (AI), run by the owner. |
| Planirani adjudikator (mora biti druga osoba od recenzenta) | Grok Bot AI adjudication pass (a different system from ChatGPT, but NOT independent of the operator; see limitation above). No independent human adjudicator. |
| Repozitorijum i tačan URL stavke | ivanjovicic/MathLearning; task file https://github.com/ivanjovicic/MathLearning/blob/b8acd2294d9677bc53a1a85acf3406595c1b49ef/docs/prompt_queues/BACKEND-TEST-048-index-bloat-validation.md |
| SHA / grana / issue ili PR broj | Base: MathLearning origin/main `b8acd2294d9677bc53a1a85acf3406595c1b49ef`. Implementation branch: `aw-eval/ai-only-001-backend-test-048` (local; pushed later by the implementer run as a draft PR). No issue; PR number unknown until created. |
| Zašto je ovo legitimni već postojeći posao (ne izmišljen) | BACKEND-TEST-048 exists on main since `5cd15fe` (2026-07-03), P1 "Prompt-ready". The defect is live on main: `IndexMaintenanceService.cs:157` hardcodes `0::numeric as bloat_percentage`, `:33` rebuild filter `BloatPercentage > 30` can never select, `MaintenanceEndpoints.cs:70-73` reports `HEALTHY` for every index. Run log `.ai/runs/2026-09-13-index-maintenance-pgstat-fix-evidence.md` leaves the bloat metric to BACKEND-TEST-048. No claim, branch, PR or issue for 048 (re-verified 2026-11-10). |
| Vlasnik je izričito odobrio ovaj run? (da/ne + id poruke) | Yes - owner GO message `t50u` plus owner widget answer (relayed to the operator by the parent agent). |
| Status iz inventara (`READY_FOR_OWNER_REVIEW` itd.) | `READY_FOR_OWNER_APPROVAL` (read-only eligibility check, 2026-11-09), then owner GO. |
| Da li je stavka HOLD? Ako da, run je zabranjen dok se HOLD ne podigne | No. Not #47, not #49, not GRX84/MOB80. |
| Da li je isti diff već brojan među 12 istorijskih REAL runova? | No. No historical run touched IndexMaintenance bloat logic. |
| Svrha zadatka i prihvatni kriterijumi | See "Acceptance criteria" below. |
| Očekivano ponašanje bez AgentsWatch-a (privremena Git/CI/PR odluka) | The blind reviewer reads the draft PR diff, the run log and CI, and gives one of: APPROVE-AS-PARTIAL / REQUEST-CHANGES / REJECT, with findings. Prediction: REQUEST-CHANGES or APPROVE-AS-PARTIAL depending on whether the PostgreSQL tests actually ran (CI Database Validation or a recorded local run). |
| Očekivani AgentsWatch izlaz (odluka, kodovi, atribucija) | See "Expected AgentsWatch output" below. |
| Očekuje li se decision change case? (da / ne / unknown) i zašto | `unknown`, leaning `no`. A careful reviewer with the diff and CI should already see scope and test gaps. A possible change: AgentsWatch surfacing unbound or unverified acceptance criteria (AWE009 / NeedsReview) or out-of-scope file attribution that the baseline review misses. |
| Klasifikacija runa (`REAL` jedino ulazi u GO brojač) | `AI_ONLY_EXPERIMENTAL` - not `REAL`; does not enter the GO counter. |

## Acceptance criteria (ac1-ac6, same as the RunContract)

- ac1: The automatic REINDEX path driven by the bloat metric is removed or disabled. Neither `RebuildCorruptedIndexesAsync` nor the nightly `IndexMaintenanceBackgroundService` issues `REINDEX` because of a bloat value.
- ac2: Physical index health is reported as `UNKNOWN` (not `HEALTHY`, not a fabricated 0%) unless it is supported by measured evidence; `/index-stats` no longer derives `HEALTHY` from the hardcoded zero.
- ac3: The separately accounted ANALYZE behavior (`UpdateStatisticsAsync`) is preserved and stays reported separately.
- ac4: No `CREATE EXTENSION`, no other DDL for extensions, no production / Fly / Neon connection, no deploy.
- ac5: PostgreSQL 16 regression tests on a disposable database (local container or CI `postgres:16` service via `TEST_POSTGRES_MAINTENANCE_CONNECTION_STRING`) prove: current-main zero-metric behavior characterized; status `UNKNOWN`; no `REINDEX` issued; ANALYZE still runs.
- ac6: Delivered as Phase 1 = PARTIAL on a draft PR (no merge), with explicit residuals in the run log and PR description.

## Expected findings (written before the work)

1. The REINDEX loop at `IndexMaintenanceService.cs:33-49` is removed or gated off; `ReindexAsync` is unused or removed.
2. A status/confidence value `UNKNOWN` appears in the report and endpoint; the `> 30` / `> 15` thresholds no longer classify unknown metrics.
3. `UpdateStatisticsAsync` (ANALYZE) remains and is still called / reported.
4. No `CREATE EXTENSION`, no `pgstattuple` installation by the app.
5. New PostgreSQL-backed tests under `tests/MathLearning.Tests/` plus an updated `IndexMaintenanceSqlContractTests`.
6. A run log under `.ai/runs/` with `Completion %` < 100 and residuals listed: optional measured metric (`pgstatindex` when available), provider privileges/extension availability on Neon/Fly/Render, schema-qualified / partitioned / expression / partial indexes, CLI parity.
7. Risk to watch: the implementer marking Done/100%, merging, or pushing to main (MathLearning `AGENTS.md` same-run-delivery rule conflicts with the owner scope; owner scope wins).
8. Risk to watch: edits outside the owned paths (queue router, other queue files, migrations, workflows, `fly.toml`).

## Expected AgentsWatch output (written before the work)

- Contract: `ai-only-eval-001-backend-test-048`; owned/avoid paths and validations as in the contract.
- Start: clean worktree at `b8acd2294d9677bc53a1a85acf3406595c1b49ef`, branch `aw-eval/ai-only-001-backend-test-048`.
- Finish prediction: receipt NOT trustworthy Done. Expected `NeedsReview`, because ac4 and ac6 have no validation binding (AWE009 manual Supported = NeedsReview) and the implementer does not record evidence through AgentsWatch. Validation evidence may be recorded by the operator only after the baseline seal, only from a verifiable source (CI run on the draft PR, `--source ci`), and recorded as such.
- Attribution prediction: all changed files attributed to this run (clean start); `.ai/runs/<date>-BACKEND-TEST-048*-evidence.md` attributed to the run, as expected.

## Measurement rules

- Baseline review time: owner records start/stop (Europe/Belgrade) of the ChatGPT review session; minutes = stop - start.
- Incremental AgentsWatch overhead: operator wall-clock minutes for contract + start + finish + reading the receipt, plus reviewer minutes spent on the AW layer (if shown later). Reported both absolute and as % of the baseline review time. Reference thresholds (protocol section 5): <= 2 min median and <= 25% of median baseline; single-run values are descriptive only.
- False alarms: AW findings the adjudication judges unfounded that took attention; counted, with the time spent.
- Decision-change classes per protocol: `ALREADY_NOTICED_IN_BASELINE`, `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED`, `NEWLY_SURFACED`; plus `NO_DECISION_CHANGE`.
- Comparison order: prereg seal (remote) -> `agentswatch start` (output operator-only) -> implementation -> blind baseline review sealed (hash in sidecar, pushed) -> `agentswatch finish` / receipt -> comparison and AI adjudication.

## Stop conditions

STOP this run (and report) on any of:
- contaminated review: ChatGPT sees AgentsWatch output, the contract, the receipt or operator notes before its seal;
- material misattribution: AgentsWatch attributes pre-existing or foreign changes to this run, or misses changes made by it;
- privacy leak: secrets, connection strings to non-disposable databases, or personal data in run artifacts, logs or PR;
- false Done: Done / 100% claimed, merge to main, or push to main;
- any production / Fly / Neon access, `CREATE EXTENSION`, `REINDEX` on a non-disposable database, or deploy.

## Pre-start confirmations

- [x] Not one of the 12 historical REAL runs and does not reuse their work for a GO count.
- [x] Not a HOLD item.
- [x] No foreign claim / PR / queue lock taken over.
- [x] Adjudicator is not the reviewer (ChatGPT vs Grok Bot); there is no independent human adjudicator, so this run cannot be treated as independently confirmed.
- [x] `agentswatch start` not yet run; this preregistration is frozen before start and before code changes.
- [x] No AgentsWatch receipt / findings exist for this run.
- [x] This sheet is complete and does not contain its own hash.