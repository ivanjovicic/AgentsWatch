# AgentsWatch Popular Feature Opportunity Map — July 2026

Last aligned: 2026-07-05  
Status: prioritization hypothesis informed by external problem evidence; not product-market-fit proof

## Goal

Rank product opportunities by:

- repeated user pain;
- strategic fit with a local-first evidence product;
- differentiation from existing AI code-review products;
- ability to prove value without heavy infrastructure;
- cross-tool feasibility;
- realistic path to repeat usage and payment.

Use with:

- `AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md`;
- `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md`.

## Evidence rule

External research, issue trackers, forums, Reddit/Hacker News studies, and adjacent paid products can establish a problem/budget signal.

They do not establish:

- willingness to install AgentsWatch;
- repeat usage;
- willingness to pay;
- time saved;
- acceptable false-positive rate;
- sustainable adapter cost.

Those require AgentsWatch-specific interviews, dogfood, pilots, and payment evidence.

## Scoring model

Each opportunity is scored directionally from 1 to 5.

| Factor | Weight | Meaning |
|---|---:|---|
| Pain intensity | 25% | How costly, risky, or frustrating is the problem? |
| Repetition/evidence signal | 20% | Does it appear across studies, issues, tools, and communities? |
| Strategic fit | 20% | Does it strengthen the evidence/control product? |
| Differentiation | 15% | Can AgentsWatch offer a vendor-neutral evidence advantage? |
| Time to useful prototype | 10% | Can a narrow local/manual prototype prove value? |
| Monetization potential | 10% | Is the outcome valuable to solo/team buyers? |

## Revised ranked opportunities

| Rank | Opportunity | Directional score | Recommended role |
|---:|---|---:|---|
| 1 | PR Evidence Packet, Completion Integrity, and Trust Ledger | 4.85 | First market-facing product and core trust moat |
| 2 | Context Snapshot, Memory Portability, and Session Rescue | 4.60 | Strong solo retention and cross-tool value |
| 3 | Agent Rules Compiler, Target-Loss Report, and Drift Detector | 4.45 | Deterministic free/open adoption wedge |
| 4 | Offline Cost/Loop/Waste Analyzer | 4.20 | High-frequency Pro value after evidence spine |
| 5 | Runtime Compatibility Detect/Explain | 4.15 | Required horizontal foundation and trust feature |
| 6 | Flight Recorder Import and Replay Timeline | 4.10 | Evidence enrichment; import-first, live later |
| 7 | Policy Dry Run and Effective-Permission Report | 3.95 | Team/security value without false enforcement claims |
| 8 | Multi-Agent Workspace/Ownership Diagnostics | 3.70 | Emerging power-user/team workflow |
| 9 | Agent Regression Canary | 3.55 | Differentiated reliability feature after repeat use |
| 10 | OSS AI Contribution Evidence Profile | 3.45 | Maintainer-focused profile of PR Evidence |
| 11 | Agent Workspace Health Monitor | 3.10 | Supporting maintenance feature |
| Do not lead | Generic model-based AI bug reviewer | 2.30 strategic fit | Crowded adjacent market, weak differentiation |

Scores are product decisions, not statistical market estimates.

## 1. PR Evidence Packet, Completion Integrity, and Trust Ledger

### User problem

Developers and reviewers cannot reliably answer:

```text
What did the agent actually change, execute, test, verify, skip, and only claim?
```

Repeated public reports describe:

- phantom tool/test execution claims;
- skipped validation gates;
- exit-code-only success claims despite warnings;
- unverified file edits;
- inferred or partial results presented as verified;
- stale CI/test evidence attached to a newer change;
- agent PR volume increasing review debt.

Representative evidence:

- https://github.com/anthropics/claude-code/issues/32650
- https://arxiv.org/abs/2601.15195
- https://arxiv.org/abs/2602.09185
- https://arxiv.org/abs/2606.13468
- https://arxiv.org/abs/2607.01904

### Product concept

A local/CI evidence packet containing:

- declared task and scope;
- actual changed files;
- files outside scope;
- observed commands and exit codes;
- relevant stdout/stderr findings;
- build/test artifacts;
- CI status and commit identity;
- agent claims;
- claim-to-evidence links;
- missing/stale/contradicted evidence;
- remaining reviewer actions.

Claim statuses:

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

### MVP

```bash
agentswatch verify
agentswatch evidence report
agentswatch pr evidence <range>
agentswatch claims check <run-or-range>
```

### Why it ranks first

- strongest repeated trust/review signal;
- useful with git/command/CI evidence before rich hooks;
- provider-neutral;
- direct value to both author and reviewer;
- creates foundations for team, policy, context, and loop features;
- differentiates from generic AI review by verifying the work rather than only reviewing the code.

### Success metrics

- reports that change a real review action;
- missing/stale evidence discovered;
- review minutes saved or added;
- false-positive rate;
- repeat usage;
- requests for CI/GitHub automation;
- paid pilot acceptance.

## 2. Context Snapshot, Memory Portability, and Session Rescue

### User problem

Context is lost when:

- a session reaches limits;
- compaction drops decisions or constraints;
- conversation history becomes invalid;
- a provider/model becomes unavailable or expensive;
- a developer switches CLI, IDE, computer, account, or provider;
- multiple team members use different agents.

Representative evidence:

- https://github.com/anthropics/claude-code/issues/34556
- https://github.com/anthropics/claude-code/issues/40524
- https://github.com/anthropics/claude-code/issues/63147
- https://arxiv.org/abs/2602.22402
- https://arxiv.org/abs/2605.11032

### Product concept

A canonical, versioned task checkpoint containing:

- goal and constraints;
- completed/pending work;
- decisions and rationale;
- relevant files/symbols;
- failed approaches;
- observed validation;
- risks and open questions;
- source hash;
- target-specific loss report.

### MVP

```bash
agentswatch context snapshot <task-id>
agentswatch context diff <a> <b>
agentswatch context export --target <agent>
agentswatch resume <task-id> --target <agent>
```

### Target-loss statuses

```text
REPRESENTED
SHORTENED
DROPPED
UNSUPPORTED
CONFLICTED
SENSITIVE_OMISSION
```

### Success metric

Time and corrective prompts needed to resume the same task in a fresh session or another tool without losing critical decisions.

## 3. Agent Rules Compiler, Target-Loss Report, and Drift Detector

### User problem

Repository instructions are copied across vendor-specific formats, drift, contradict one another, and have different effective strength.

### Product concept

One canonical rule source compiled into supported targets, with explicit semantic loss:

```text
EXACT
EQUIVALENT
WEAKER
ADVISORY_ONLY
UNSUPPORTED
CONFLICT_WITH_MANAGED_POLICY
```

### MVP

```bash
agentswatch rules lint
agentswatch rules compile
agentswatch rules diff
agentswatch rules export --target all
```

### Why it is the strongest free wedge

- deterministic and understandable;
- useful without live agent control;
- local and shareable in public repos;
- low infrastructure cost;
- naturally introduces compatibility and policy concepts;
- avoids pretending textual instructions equal mechanical enforcement.

### Success metric

Contradictory/stale rules found, target losses reported, and time required to onboard another agent tool.

## 4. Offline Cost/Loop/Waste Analyzer

### User problem

Developers discover waste after quota, time, or budget is consumed.

Repeated signals include:

- repeated identical commands/searches;
- retries without relevant state change;
- edit oscillation;
- no-progress waits;
- duplicate parallel work;
- sudden provider-reported quota drain;
- runs ending without a checkpoint.

### Product concept

Analyze only observable waste first:

- repeated action fingerprints;
- relevant-state delta between retries;
- duplicate worker scope;
- wall-clock no-progress periods;
- repeated validation after no meaningful change;
- checkpoint need.

Keep separate:

```text
exact tokens
cached tokens
quota percentage
currency cost
wall-clock time
local compute/storage
unknown
```

### MVP

```bash
agentswatch loops report <run-id>
agentswatch waste report <run-id>
agentswatch checkpoint recommend <run-id>
```

### Success metric

Actionable redundant-work findings, measured wall-clock savings, and low false-positive rate.

Do not initially claim exact provider billing reconstruction or automatic process termination.

## 5. Runtime Compatibility Detect/Explain

### User problem

The same feature is not equally observable or enforceable across chat, local CLI, IDE, cloud/PR, CI, read-only, container, remote, and worktree setups.

### Product concept

Resolve an `EffectiveRuntimeProfile` and classify every capability:

```text
Full | Guarded | Advisory | PostHoc | Manual | Unavailable
```

### MVP

```bash
agentswatch compatibility detect
agentswatch compatibility explain <capability>
agentswatch compatibility compare <profile-a> <profile-b>
```

### Why it matters

It prevents false universal-support claims and makes evidence/policy/context behavior explainable.

### Success metric

Correct support/fallback decisions across the compatibility fixture matrix and real dogfood profiles.

## 6. Flight Recorder Import and Replay Timeline

### User problem

Users cannot reconstruct what occurred across commands, edits, approvals, subagents, worktrees, and compaction events.

### Product concept

A normalized local event journal and replayable timeline.

### MVP order

1. synthetic fixture import;
2. user-supplied session/log import;
3. generic process-wrapper events;
4. first rich local adapter;
5. second materially different adapter;
6. live collection only after privacy and compatibility proof.

### Success metric

Useful event coverage, missing-event transparency, adapter maintenance cost, and evidence added to the Trust Ledger.

## 7. Policy Dry Run and Effective-Permission Report

### User problem

Permission systems are tool-specific, coarse, and often rely on textual rules that are not mechanically enforced.

### Product concept

Explain:

- intended policy;
- configured versus effective permissions;
- enforcement class;
- bypass paths;
- approval requirement;
- unsupported surfaces.

### MVP

```bash
agentswatch policy lint
agentswatch policy explain <command-or-operation>
agentswatch policy dry-run -- <command>
```

### Product rule

Do not publicly call this a firewall before PE3/PE4/PE5 enforcement proof exists.

## 8. Multi-Agent Workspace/Ownership Diagnostics

### User problem

Parallel agents can:

- use the wrong checkout/current directory;
- duplicate work;
- ignore heartbeats or handoffs;
- leave stale halt/lock state;
- require humans to relay every decision;
- confuse persistent and one-shot workers.

Representative evidence:

- https://github.com/anthropics/claude-code/issues/53610
- https://github.com/anthropics/claude-code/issues/28300
- https://github.com/openai/codex/issues/23095
- https://github.com/openai/codex/issues/21027

### Initial product concept

Diagnostics and planning, not full orchestration:

```bash
agentswatch coordination plan
agentswatch workspace verify
agentswatch ownership check
agentswatch stale detect
agentswatch duplicate-work report
agentswatch integrate check
```

### Success metric

Wrong-workspace edits, duplicate work, stale states, conflicts, and human coordinator interventions avoided.

## 9. Agent Regression Canary

### User problem

A model/client/tool update may become more expensive, less careful, or more edit-first without an objective baseline.

### Product concept

Compare the complete runtime profile:

- model/version and role composition;
- tool/version and surface;
- permissions;
- environment/toolchain;
- repository commit;
- prompt/rules;
- network mode;
- repetitions;
- scope, tests, evidence, cost/usage, and retries.

### Product rule

A changed tool, permission, environment, or planner/editor composition is a confounded comparison, not a pure model regression.

## 10. OSS AI Contribution Evidence Profile

A maintainer-focused profile of PR Evidence checking:

- issue linkage and requested scope;
- duplicate topic/PR signals;
- reproduction and test evidence;
- AI-assistance disclosure policy;
- changed-file/complexity budget;
- contributor understanding checklist.

This is not a separate architecture.

## 11. Agent Workspace Health Monitor

Supporting commands:

```bash
agentswatch doctor
agentswatch storage report
agentswatch worktrees clean --dry-run
```

Useful, but not a primary reason to adopt AgentsWatch.

## Competitive boundary: generic AI reviewer

The generic AI code-review category is real and monetized, but crowded.

AgentsWatch should not lead with model-based bug-finding superiority.

| Generic AI reviewer | AgentsWatch target |
|---|---|
| Reviews final diff | Verifies task/run/evidence lineage |
| Produces model findings | Links claims to independent proof |
| Suggests tests | Shows which tests ran and for which commit |
| Often repository snapshot focused | Includes scope, commands, context, CI, and missing evidence |
| Competes on review intelligence | Competes on completion integrity and auditability |

External reviewers/static analyzers may become evidence sources later.

## Recommended portfolio strategy

### Free/open adoption wedge

- Rules Compiler and target-loss/drift report;
- basic context snapshot/export;
- basic local PR/run evidence report;
- `agentswatch doctor`;
- public-repository GitHub Action allowance later.

### Pro solo-developer value

- advanced Context Resume;
- Trust Ledger history/comparison;
- offline Loop/Waste Analyzer;
- supported premium adapters where justified;
- regression canaries later.

### Team/enterprise value

- private PR Evidence automation;
- shared policy/rules packs;
- team audit/history;
- organization compatibility reports;
- multi-agent diagnostics;
- signed/tamper-evident evidence later;
- policy enforcement only after declared enforcement proof.

## Recommended execution order

### Now: evidence-first validation

1. complete the current core run/evidence spine;
2. define the PR Evidence/Trust Ledger output contract;
3. manually generate reports for real AI-assisted PRs;
4. run the 30-PR market experiment;
5. implement the narrow local PR Evidence prototype;
6. build Context Snapshot/Resume;
7. build Rules Compiler and target-loss report;
8. implement runtime compatibility schema/decision engine.

### After repeat use

9. offline Loop/Waste Analyzer;
10. local event/session import timeline;
11. GitHub Action for users requesting automation;
12. first materially different live adapters.

### After paid team demand

13. Policy Dry Run/Explain;
14. team history/audit metadata;
15. multi-agent diagnostics;
16. GitHub App;
17. Regression Canary.

### Only after safety and runtime proof

18. live loop warnings/checkpoints;
19. enforced execution policy;
20. full multi-agent orchestration;
21. hosted/self-hosted Team Server expansion.

## First experiment thresholds

For 30 real AI-assisted PRs:

```text
At least 30% of reports change a review action.
At least 50% of test users request or perform repeat use.
At least 3 teams request automated GitHub/CI execution.
At least 2 teams accept a paid or explicitly budgeted pilot.
```

These are internal directional thresholds, not market benchmarks.

## Kill criteria

Pause, revise, or reject an opportunity when:

- fewer than three of five target users recognize the problem;
- PR Evidence reports rarely change review action;
- output duplicates existing GitHub/CI views without added value;
- users do not repeat usage;
- users will not provide/generate required local evidence;
- required logs/events are too unstable for a maintainable adapter;
- false positives create more work than the feature saves;
- the solution requires source upload by default;
- no team accepts even a small paid/budgeted pilot;
- a vendor-native feature solves the problem across providers;
- the idea pulls AgentsWatch into being a full IDE, generic AI reviewer, or autonomous coding agent.
