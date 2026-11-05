# AW-QA-001 — Adversarial receipt benchmark preparation

Repository: `ivanjovicic/AgentsWatch`  
Owning queue: `docs/prompt_queues/verification_mvp_2026_08_25.md` (support track)  
Status: Ready only after AW-VFY-006 yields stable RunReceipt v1; must be complete before AW-VFY-010 executes the 30-run evaluation  
Run mode: QA design and **safe fixture preparation**, no product expansion  
Context budget: low/medium; one fixture/test plan

## Goal

Make AW-VFY-010 rigorous and repeatable rather than 30 anecdotal success reports.

## Steps

1. Review `AW-VFY-004` through `AW-VFY-010`, current Git/Core test semantics, the approved RunReceipt schema and `docs/TEST_MATRIX.md`.
2. Define >=30 distinct case slots across AgentsWatch, one .NET and one Flutter repository. Only assign real repos after access/permission is confirmed; do not modify production or main branches.
3. Include all adversarial must-catch cases: pre-existing dirty unchanged; dirty changed-further; existing/new untracked; add/delete/rename; same-path staged+unstaged; branch/HEAD switch; concurrent writer ambiguity; missing test run; test failure; CI evidence for wrong SHA; blocked environment; out-of-scope path; avoid-path touch; false DocsOnly; false TestsAdded; false ValidationPassed; unsupported broad BugFixed claim.
4. Specify for every slot: baseline repo state, RunContract requirements, agent task, expected machine evidence, expected run decision, privacy/cleanup controls, independent reviewer judgment, and baseline workflow result (Git+CI+PR+native/Qodo if available).
5. Define measurement: high-confidence true/false findings, material attribution errors, Unknown/Ambiguous, receipt capture overhead, reviewer minutes used/saved, changed decision, setup friction and repeat use. No invented token savings.
6. Specify failure severity: any material false attribution or unsupported Done is release-blocking; benign ambiguity must not be counted as false author claim; missing measurement is Unknown, not success.
7. Include a 5–10 run batch review cadence, independent adjudication of disagreements and a stop rule for excessive false-positive burden. Separate product failures from fixture mistakes.
8. Use disposable branches/temporary repos and synthetic files for deliberate failures. No destructive operations in Trendplus/MathLearning or production data.

## Owned paths

- `docs/research/agentswatch_market_validation_2026_11/**`
- optional `tests/**` only for **non-production, explicitly authorized** test fixtures after runtime gates and stable schema
- compact `.ai/runs/**` evidence

## Avoid

- `src/**` runtime changes
- automatic merge/release, mass CI retries, production credentials or customer code
- scoring a run as useful just because the receipt is readable

## Output and completion

Create `docs/research/agentswatch_market_validation_2026_11/adversarial_30_run_protocol.md`, with 30-row fixture matrix and adjudication rubric. Mark AW-QA-001 Done only after the benchmark plan is independently reviewed and references the actual RunReceipt version. Its completion does not replace AW-VFY-010 dogfood or later real buyers.
