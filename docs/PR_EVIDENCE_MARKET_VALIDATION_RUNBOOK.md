# AgentsWatch PR Evidence Market Validation Runbook

Last aligned: 2026-07-05  
Status: validation plan; does not prove product value until executed

## Goal

Test whether an AgentsWatch PR Evidence Packet changes real review decisions, saves measurable work, creates repeat use, and supports a paid pilot.

This runbook validates the first market-facing hypothesis:

```text
AgentsWatch verifies what a coding agent changed, executed, tested, and missed.
```

## Scope

Analyze 30 real AI-assisted pull requests across at least:

- 5 repositories;
- 3 teams or independent maintainers;
- 2 technology stacks;
- 2 coding-agent tools where available;
- both accepted and problematic PRs where possible.

A PR may be public or private. Private source/content must not be copied into shared research artifacts without explicit permission.

## Exclusions

Do not use:

- synthetic-only PRs as the main sample;
- only AgentsWatch's own repository;
- only successful PRs;
- only one developer's workflow;
- PRs where the reviewer already authored the changes;
- unverifiable marketing examples presented as real user evidence.

## Participant roles

For each PR record:

- author or agent operator;
- reviewer/maintainer;
- team/organization where permitted;
- coding agent/tool/surface;
- repository and stack category;
- privacy/export permission.

Do not record personal or organization identifiers in published summaries unless explicitly approved.

## Evidence inputs

Use only inputs available with permission:

- task/issue description;
- declared scope;
- PR diff and commit range;
- local run report;
- observed commands and exit codes;
- build/test output summaries;
- CI/check results;
- artifacts and commit hashes;
- agent completion message/claims;
- reviewer comments and final decision.

Mark every unavailable input as unavailable. Do not infer that an action did not occur merely because it was not observed.

## Required PR Evidence Packet

### 1. Identity

```text
study_case_id
repository_category
pr_number_or_private_alias
base_commit
head_commit
analysis_timestamp
runtime_profile_summary
```

### 2. Declared task and scope

- requested outcome;
- acceptance criteria;
- allowed paths;
- explicitly excluded work;
- declared validation plan.

### 3. Actual change inventory

- changed files;
- additions/deletions where permitted;
- generated/binary files;
- files outside declared scope;
- major behavior/config/schema changes.

### 4. Agent claims

Extract concrete claims such as:

- implemented behavior;
- tests/build run;
- test result;
- service/UI behavior observed;
- migration applied;
- no unrelated changes;
- production/readiness claim.

Avoid interpreting general confidence language as a concrete claim unless it clearly asserts an outcome.

### 5. Observed evidence

- git/diff evidence;
- commands and exit codes;
- relevant stdout/stderr findings;
- build/test artifacts;
- CI/check status;
- artifact/commit match;
- screenshots/manual attestations where applicable;
- missing evidence.

### 6. Claim classification

Use exactly one primary status:

```text
SUPPORTED
PARTIALLY_SUPPORTED
CONTRADICTED
MISSING_EVIDENCE
STALE_EVIDENCE
NOT_OBSERVED
NOT_VERIFIABLE
SKIPPED
```

Optional confidence:

```text
HIGH
MEDIUM
LOW
```

Confidence must reflect evidence quality, not model confidence.

### 7. Reviewer action list

Keep the list short and actionable:

- inspect specific file/behavior;
- rerun specific validation;
- request missing evidence;
- clarify scope;
- reject stale evidence;
- accept evidence as sufficient;
- no additional action.

## Study procedure

### Step 1 — baseline review

Before showing the AgentsWatch packet, record:

- review start/end time;
- initial reviewer decision;
- issues/questions identified;
- evidence the reviewer manually inspected;
- confidence in the decision.

Possible decision labels:

```text
APPROVE
REQUEST_CHANGES
NEEDS_EVIDENCE
NEEDS_DISCUSSION
DEFER
```

### Step 2 — generate packet

Generate the packet manually or with the current prototype.

Record:

- operator time;
- data sources used;
- unavailable inputs;
- tool/version;
- manual judgments required.

### Step 3 — packet-assisted review

Show the packet to the reviewer and record:

- new issues/questions;
- removed concerns;
- changed decision;
- additional review time;
- confusing/noisy findings;
- findings considered useful;
- findings considered obvious or redundant.

### Step 4 — outcome interview

Ask:

1. Did this change what you reviewed or the order of review?
2. Did it reveal missing/stale/contradicted evidence?
3. Did it save time or add time?
4. Which section would you remove?
5. Which section must be automated?
6. Would you run this on the next AI-assisted PR?
7. Would your team install a CLI or GitHub Action for it?
8. Who would own the budget?
9. Would you join a free design-partner pilot?
10. Would you pay for a pilot, and what outcome would justify payment?

Do not treat polite interest as purchase intent.

## Metrics

### Primary metrics

```text
review_action_change_rate
repeat_use_rate
missing_or_stale_evidence_detection_rate
false_positive_rate
paid_or_budgeted_pilot_count
```

### Secondary metrics

```text
baseline_review_minutes
packet_generation_minutes
packet_assisted_review_minutes
net_review_minutes_saved_or_added
claims_per_pr
supported_claim_percentage
unverified_claim_percentage
out_of_scope_file_rate
commit_mismatch_rate
automation_request_count
```

### Qualitative outcomes

- strongest recurring problem;
- most useful packet section;
- most ignored packet section;
- reasons for refusing installation;
- privacy/security objections;
- existing substitute tools;
- buyer and budget owner;
- preferred pricing model.

## Directional advance criteria

Advance to automated local/GitHub workflow when:

```text
>= 30% of packets change a real review action
>= 50% of participants request or perform repeat use
>= 3 teams request CI/GitHub automation
>= 2 teams accept a paid or explicitly budgeted pilot
```

Also require:

- false positives are considered tolerable;
- packet generation can be automated from stable inputs;
- privacy boundaries are acceptable;
- the report adds value beyond standard GitHub/CI views.

These thresholds are internal product decisions, not external benchmarks.

## Revise conditions

Revise the product/output when:

- users value only one small section;
- reports are useful but too long;
- packet generation costs more time than review savings;
- reviewers need stack-specific evidence that the generic report misses;
- missing data is the dominant result;
- users prefer author preflight over reviewer-facing reports;
- willingness to pay exists only for onboarding/consulting.

## Park/reject conditions

Park or reject when:

- fewer than 3 of 5 initial users recognize the problem;
- fewer than 20% of packets alter any review action;
- repeat use is below 25%;
- reviewers consider the output obvious/redundant;
- false positives create more work than they save;
- required evidence cannot be collected reliably;
- users reject local/CI integration on privacy grounds;
- no team accepts a budgeted or paid pilot after the full study;
- the only valued feature is generic AI bug review.

## Data handling

- use study aliases for private repositories;
- retain only minimum evidence required for analysis;
- never publish source, full diffs, full logs, prompts, secrets, or organization names without permission;
- record consent and retention period;
- allow participants to request deletion;
- separate raw private data from aggregated findings;
- store source hashes/identifiers only when useful and permitted.

## Study log template

```markdown
# PR Evidence Study Case <id>

## Identity
- Repository category:
- Stack:
- Agent/tool/surface:
- Base/head commit:
- Privacy level:

## Baseline review
- Initial decision:
- Time:
- Issues found:
- Evidence inspected:

## Claims
| Claim | Evidence | Status | Confidence |
|---|---|---|---|
| | | | |

## Scope/diff findings
-

## Validation/commit findings
-

## Reviewer actions
-

## Packet-assisted outcome
- Decision before:
- Decision after:
- Time saved/added:
- New issue/evidence request:
- Useful sections:
- Noisy sections:

## Adoption signal
- Would repeat:
- Requests automation:
- Free pilot:
- Paid/budgeted pilot:
- Buyer/budget owner:

## Follow-up
-
```

## Aggregate decision report

After 10, 20, and 30 PRs publish an internal report containing:

- sample composition;
- metric table;
- recurring findings;
- false positives;
- privacy/integration objections;
- substitute products/workflows;
- pricing/buyer notes;
- Advance/Revise/Park/Reject decision;
- exact next smallest implementation slice.

Do not publish percentage claims externally unless methodology, sample limitations, and review are documented.
