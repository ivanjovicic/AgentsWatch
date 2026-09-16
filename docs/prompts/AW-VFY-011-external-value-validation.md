# AW-VFY-011 — External Value Validation

Status: Ready after `AW-VFY-010` and reviewed 30-run dogfood evidence
Type: validation/research only

## Objective

Determine whether AgentsWatch's independent RunContract/RunReceipt verification changes real engineering review decisions outside the founder's own repositories.

Do not add product features merely to make the study easier.

## Required cohort

Recruit at least 10 external developers or engineering teams that:
- use coding agents on real repositories;
- perform real review/validation before accepting agent changes;
- can compare AgentsWatch with their current baseline workflow.

Include at least two agent ecosystems/vendors across the cohort where practical.

## Baseline to compare against

For every participant record current use of:
- Git diff/status/history;
- CI/tests;
- PR review;
- native agent session/log/evidence features;
- existing code-review/governance tooling.

## Evaluation procedure

Use real coding-agent work where permitted.

For each evaluated run record:
- task context;
- agent/vendor;
- contract completeness;
- attributable delta result;
- validation evidence;
- claims/scope findings;
- whether AgentsWatch found something the existing workflow did not surface early enough;
- whether a reviewer changed merge/rework/evidence decision;
- false-positive findings;
- attribution ambiguity;
- participant trust in the receipt;
- request for continued use: yes/no + reason.

Do not claim a catch merely because AgentsWatch displayed information already obvious to the participant.

## Required output

Create a structured evidence report with:

1. participant/cohort summary;
2. baseline workflow matrix;
3. evaluated-run table;
4. decision-changing finding table;
5. false-positive/ambiguity table;
6. continued-use requests;
7. strongest validated buyer/job;
8. strongest reason the thesis may fail;
9. Gate 6 decision: PASS / FAIL / NARROW;
10. exact next recommendation.

## Pass candidate

- >=10 external real evaluations;
- >=3 external users/teams request continued use;
- >=3 real evidence/scope/claim findings changed review/rework/merge/evidence decisions;
- no observed material false attribution;
- finding noise remains low enough that reviewers do not ignore the product;
- at least one use case clearly adds value beyond Git + CI + PR review + native vendor logs.

## Kill / narrow signal

- baseline tools solve the problem well enough;
- receipt is interesting but not decision-changing;
- cross-vendor independence is not valued in the target segment;
- false positives/ambiguity exceed practical reviewer tolerance;
- users want unrelated orchestration/runtime features instead of verification.

## Stop rules

- Do not implement SaaS, auth, billing or dashboard work.
- Do not invent user feedback.
- Do not count founder dogfood as external evidence.
- Do not lower evidence standards to force a PASS.
- If access to real external repos is unavailable, use the smallest safe shared/representative evaluation and mark the limitation explicitly.
