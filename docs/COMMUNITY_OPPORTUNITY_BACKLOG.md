# AgentsWatch Community Opportunity Backlog

Last aligned: 2026-07-03  
Status: issue-ready discovery and implementation slices

## Rules

- Community reports prove repeated pain, not market demand.
- Discovery issues precede implementation issues.
- Import-only and dry-run issues precede live or enforcing issues.
- Every issue names capability IDs and a proof action.
- Do not implement these slices on main until Gate 0 and owning dependencies pass.

## Research and validation

### OPP-001 — Conduct control-plane problem interviews

Capabilities: AW-CAP-028 through AW-CAP-036  
Labels: `research`, `product-discovery`, `community-opportunity`

Task:

- interview at least 12 coding-agent users;
- cover solo power users, team developers, and maintainers;
- collect examples for context loss, false completion, usage waste, scope expansion, multi-agent coordination, and review debt;
- record current workaround and willingness-to-try/pay signal.

Acceptance:

- anonymized interview notes;
- at least five users for each opportunity that advances;
- explicit rejected/weak opportunities;
- prioritization score updated with evidence.

Stop: no runtime implementation.

### OPP-002 — Build source/adapter feasibility matrix

Capabilities: AW-CAP-028, AW-CAP-030, AW-CAP-032, AW-CAP-034  
Labels: `research`, `adapters`, `architecture`

Task:

- document local logs, hooks, session exports, event formats, and wrapper options for at least Claude Code and Codex;
- classify each input as stable, experimental, or unsupported;
- list privacy and maintenance risks.

Acceptance:

- capability matrix completed;
- no claim based on undocumented hidden data;
- import-only source selected for first prototype;
- unsupported fields remain explicit.

### OPP-003 — Competitive substitute review

Capabilities: AW-CAP-028 through AW-CAP-036  
Labels: `research`, `product`

Task:

- compare vendor-native and independent tools for memory, usage tracking, hooks, policy, multi-agent worktrees, and PR review;
- identify why AgentsWatch would be adopted instead of or alongside them.

Acceptance:

- substitutes listed by problem;
- differentiation statement for each advancing opportunity;
- kill recommendation where a substitute fully solves the need.

## Shared event foundation

### OPP-010 — Define normalized agent event schema

Capabilities: AW-CAP-028, AW-CAP-032, AW-CAP-034  
Dependencies: main Gate 0; OPP-002.

Task:

- implement versioned event envelope and initial event types;
- support extension fields and stable local identities;
- add JSON serialization fixtures.

Acceptance:

- two synthetic provider fixtures normalize into the same model;
- duplicate, out-of-order, and invalid events tested;
- unknown fields preserved;
- no command execution during import;
- redaction state recorded.

### OPP-011 — Implement import-only event journal

Capabilities: AW-CAP-028  
Dependencies: OPP-010.

Acceptance:

- repeated import does not duplicate events;
- corrupt input does not corrupt journal;
- paths cannot escape selected artifact root;
- no network access.

### OPP-012 — Add adapter capability declaration

Capabilities: AW-CAP-028, AW-CAP-030, AW-CAP-032, AW-CAP-034

Task:

- declare observability for commands, exits, files, usage, compaction, subagents, approvals, and worktrees;
- show unsupported data in reports.

Acceptance:

- absence of an event is not treated as proof that behavior did not happen;
- source/version and blind spots are visible.

## Flight Recorder and Trust Ledger

### OPP-020 — Generate local run timeline

Capabilities: AW-CAP-028  
Dependencies: OPP-011.

Acceptance:

- deterministic markdown and JSON;
- parent/subagent lineage visible;
- missing events visible;
- full source/output omitted by default;
- secret fixtures absent from human-facing output.

### OPP-021 — Verify basic completion claims

Capabilities: AW-CAP-029

Task:

- support tests/build/command/file/package claims;
- classify Supported, Contradicted, Missing, or NotVerifiable.

Acceptance:

- failed test contradicts `tests passed`;
- no test event returns Missing;
- changed-file claim uses git evidence;
- false-completion fixtures included.

### OPP-022 — Link trust results into proof bundle

Capabilities: AW-CAP-027, AW-CAP-029

Acceptance:

- missing referenced artifact fails validation;
- commit/hash mismatch fails linkage;
- maturity cannot exceed linked evidence.

## Context portability and rules compiler

### OPP-030 — Manual context snapshot

Capabilities: AW-CAP-030  
Dependencies: basic run/report model.

Task:

- generate bounded snapshot with goals, non-goals, decisions, scope, validation, risk, and next action.

Acceptance:

- deterministic markdown/JSON;
- fixed size budget;
- unknown values remain unknown;
- secret values excluded.

### OPP-031 — Fresh-session resume experiment

Capabilities: AW-CAP-030

Task:

- compare baseline fresh session and AgentsWatch resume pack on the same task/commit;
- measure corrective prompts, time, and validated outcome.

Acceptance:

- at least five paired runs;
- quality does not decline;
- limitations recorded;
- no general savings claim from a small sample.

### OPP-032 — Canonical rules compiler

Capabilities: AW-CAP-031

Task:

- define canonical repository agent-policy source;
- export `AGENTS.md`, `CLAUDE.md`, generic handoff, and one additional target;
- add drift lint.

Acceptance:

- deterministic golden outputs;
- user-owned files not overwritten;
- unsupported target concepts warned;
- contradictions detected.

## Cost and Loop Guard

### OPP-040 — Offline action fingerprinting

Capabilities: AW-CAP-032  
Dependencies: OPP-010/011.

Task:

- fingerprint commands, searches, file reads, and edits;
- detect repeated no-progress windows and edit oscillation.

Acceptance:

- same failed command without relevant change detected;
- relevant code change adjusts comparison;
- false-positive fixtures exist;
- every finding is explained.

### OPP-041 — Provider-neutral budget ledger

Capabilities: AW-CAP-032

Acceptance:

- known token/quota/cost/time events stored with source/confidence;
- unknown prices remain unknown;
- measured and estimated values are distinct;
- unexplained consumption is reported without invented cause.

### OPP-042 — Checkpoint and stop recommendation

Capabilities: AW-CAP-032

Task:

- recommend Continue, Ask, Checkpoint, SwitchStrategy, or Stop;
- do not terminate a process.

Acceptance:

- recommendation includes evidence and threshold;
- checkpoint is resumable;
- dismissal can be recorded for tuning.

### OPP-043 — Live warning wrapper spike

Capabilities: AW-CAP-032  
Dependencies: useful offline dogfood; safety review.

Acceptance:

- warnings only in the first spike;
- process cleanup tested;
- interruption retains checkpoint;
- explicit opt-in required.

## Policy Firewall

### OPP-050 — Policy schema and linter

Capabilities: AW-CAP-033

Task:

- define path, operation, network, and instruction-trust rules;
- implement conflict and precedence explanation.

Acceptance:

- deterministic result;
- conflicts reported;
- platform/path fixtures;
- no execution.

### OPP-051 — Path and command dry-run

Capabilities: AW-CAP-033

Acceptance:

- path traversal and symlink-boundary scenarios tested;
- repository instruction cannot override user policy;
- result names matched rule and confidence;
- sensitive fixture paths can be denied.

### OPP-052 — Threat model and independent review plan

Capabilities: AW-CAP-033

Acceptance:

- assets, actors, boundaries, abuse cases, and residual risk documented;
- wording avoids complete-protection claims;
- enforcing implementation remains blocked until review.

## Multi-Agent Worktree Coordinator

### OPP-060 — Worktree ownership planner

Capabilities: AW-CAP-034

Task:

- create planned worktree, branch, and owned-path assignments;
- detect overlaps and invalid roots;
- do not spawn agents.

Acceptance:

- deterministic plan;
- overlapping write ownership blocked or explicitly overridden;
- base commit recorded;
- dry-run does not mutate repository.

### OPP-061 — Worker bootstrap and status files

Capabilities: AW-CAP-034

Acceptance:

- exact canonical worktree path;
- worker scope, stop rules, and evidence requirements;
- parent reads compact status without transcript;
- worker lineage preserved.

### OPP-062 — Integration readiness check

Capabilities: AW-CAP-034

Acceptance:

- flags wrong-worktree edits, overlapping files, incomplete dependencies, missing validation, and likely conflicts;
- two-worker and five-worker fixtures;
- no automatic merge;
- cleanup never removes uncommitted work.

## PR Review Debt Reducer

### OPP-070 — Local range evidence packet

Capabilities: AW-CAP-035

Task:

- analyze local commit range;
- list requested scope, files, tests, validation, risk paths, and reviewer questions.

Acceptance:

- every file listed;
- high-risk files prioritized;
- missing evidence distinct from failure;
- no LLM or network required.

### OPP-071 — Link agent runs to PR evidence

Capabilities: AW-CAP-029, AW-CAP-035

Acceptance:

- exact commit linkage;
- claims checked against run/CI evidence;
- mismatched commit fails trust linkage.

### OPP-072 — Maintainer dogfood study

Capabilities: AW-CAP-035

Task:

- compare normal diff/stat review with review packet on fixtures and approved real examples.

Acceptance:

- reviewer time, missed findings, false positives, and usefulness recorded;
- no replacement-human-review claim.

## Regression Canary

### OPP-080 — Define small canary suite

Capabilities: AW-CAP-036

Task:

- create tiny deterministic tasks for context precision, scope, validation, completion integrity, and usage where observable.

Acceptance:

- inputs and success criteria versioned;
- results comparable across client/model versions;
- unavailable metrics remain unknown.

### OPP-081 — Canary comparison report

Capabilities: AW-CAP-036

Acceptance:

- baseline/candidate environment recorded;
- differences separated from statistical conclusions;
- no regression claim from one noisy run.

## Priority order

```text
OPP-001 -> OPP-002 -> OPP-003
-> OPP-010 -> OPP-011 -> OPP-012
-> OPP-020/021
-> OPP-030/032
-> OPP-040/041
-> remaining dry-run prototypes
-> dogfood
-> live or enforcing slices only after evidence
```
