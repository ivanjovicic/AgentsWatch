# AgentsWatch Context Packs

Status: active docs/spec  
Last aligned: 2026-07-01  
Purpose: reduce token waste by replacing broad discovery with named, task-scoped context packs.

## Rule

A non-trivial agent run must choose one context pack before reading many files.

If the agent needs more than the pack budget, it must record:

```text
Scope expansion: yes
Pack: <pack-name>
Why: <specific blocker>
Files added: <paths>
```

Context packs do not replace current-code inspection. They only choose the smallest useful starting set.

## Shared pack schema

Each pack uses this shape:

```text
Pack: <name>
When to use: <task class>
Read first: <small list>
Avoid by default: <files/categories>
Max files before expansion: <n>
State ownership check: required/optional/not applicable
Validation defaults: <commands/checks>
Output mode: brief-done/review-table/evidence-row/full-analysis
Freshness rule: <fresh/recent/stale behavior>
Done blocker: <what prevents high-confidence Done>
```

This schema comes from the earlier repo-specific skill-doc pattern:

```text
When to use
Files to inspect
Rules/invariants
Common mistakes
Required tests
Done criteria
```

The pack version is optimized for token economy by adding max files, freshness, output mode, and expansion rules.

---

## pack.evidence.repair

When to use: fixing evidence rows, run logs, missing validation notes, false Done claims, stale queue statuses.

Read first:

- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `.ai/runs/README.md`
- exact queue row or evidence file being repaired
- `docs/ai/learning/MISTAKE_LEDGER.md`

Avoid by default:

- runtime source code;
- product roadmap docs;
- architecture docs not referenced by the evidence row;
- old run logs unless exact proof is needed.

Max files before expansion: 6

State ownership check: not applicable unless evidence makes runtime/data-ownership claims.

Validation defaults:

```bash
git diff --check
python scripts/validate_agent_evidence.py --referenced-run-logs-only
```

Output mode: evidence-row

Freshness rule: current queue row and current run log win over old summaries. Old logs are history, not proof of current status.

Done blocker: no high-confidence Done if the run log path is missing, validation is not run/declared, or residual risk contradicts completion percent.

---

## pack.agentwatch.gate0

When to use: restore/build/test/CLI smoke validation, bootstrap health, Gate 0 status.

Read first:

- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/VALIDATION_EVIDENCE_2026_06_29.md`
- `docs/BOOTSTRAP_NEXT_STEPS.md`
- `docs/ROADMAP_VALIDATION_GATES.md`
- exact AW-VAL prompt file if referenced

Avoid by default:

- productization docs;
- dashboard/SaaS docs;
- future feature specs;
- token economy research unless the task is about token economy.

Max files before expansion: 6

State ownership check: optional; use only if changing runtime config, filesystem writes, git status, or report ownership.

Validation defaults:

```bash
dotnet restore
dotnet build
dotnet test
```

Use the exact commands from the validation plan if they differ.

Output mode: brief-done

Freshness rule: latest command output wins. Historical validation evidence is only a baseline.

Done blocker: no Gate 0 Done without current restore/build/test/CLI smoke proof or explicit blocked evidence.

---

## pack.backend.auth-risk

When to use: backend auth, refresh tokens, mobile registration, idempotency, transactional cleanup, provider-specific EF behavior.

Read first:

- backend repo `AGENTS.md`
- backend `docs/DOCS_INDEX.md`
- backend auth endpoints/tests touched by the prompt
- backend `docs/ai/learning/MISTAKE_LEDGER.md`
- exact queue row for BACKEND/BACKEND2 auth work

Avoid by default:

- Flutter UI docs;
- unrelated backend performance docs;
- old static audits unless referenced by the queue row.

Max files before expansion: 8

State ownership check: required. Identify the authority for:

- identity user;
- refresh token;
- profile creation;
- welcome coins;
- retry/idempotency key;
- transaction boundary.

Validation defaults:

```bash
dotnet test --filter Auth
```

Prefer narrower test names when the queue row names them.

Output mode: review-table

Freshness rule: current code/tests override stale audit docs.

Done blocker: no Done if only in-memory tests exist for a relational/provider-specific risk and the residual risk is not documented.

---

## pack.flutter.ui-test

When to use: Flutter UI tests, widget tests, screen-level layout, accessibility, visual polish evidence.

Read first:

- Flutter repo `AGENTS.md`
- Flutter `docs/DOCS_INDEX.md`
- `docs/UI_QUALITY_GATE.md`
- `docs/AGENT_RESPONSIVE_DEBUG_PLAYBOOK.md`
- exact screen/widget/test files touched
- exact queue row

Avoid by default:

- backend docs unless API contract is directly involved;
- product marketing docs;
- old visual audits unless exact comparison is needed.

Max files before expansion: 7

State ownership check: optional for pure layout; required if UI displays coins, XP, rewards, auth state, pending offline operations, or backend-owned data.

Validation defaults:

```bash
flutter test <targeted_test_file>
flutter analyze
```

Output mode: review-table

Freshness rule: current widget/test files override old screenshots or old run logs.

Done blocker: no high-confidence visual/UI Done if there is only shared-component evidence and missing screen-level test coverage for the changed screen.

---

## pack.cross-repo.standard-sync

When to use: aligning AGENTS/shared standards/run-log/mistake-learning/docs-index rules across Flutter, backend, and AgentsWatch.

Read first:

- each repo `AGENTS.md`
- each repo `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- each repo `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- each repo `docs/DOCS_INDEX.md`
- each repo `docs/ai/learning/MISTAKE_LEDGER.md`

Avoid by default:

- runtime source code;
- product specs;
- historical completed archives unless checking contradictions.

Max files before expansion: 12 because this is intentionally cross-repo.

State ownership check: not applicable unless standards make runtime claims.

Validation defaults:

```bash
git diff --check
python scripts/validate_agent_evidence.py --referenced-run-logs-only
```

Run per repo if local checkout exists.

Output mode: review-table

Freshness rule: current AGENTS/shared standard/index files win. Old summaries are only handoff context.

Done blocker: no Done if one repo is updated and the others are left contradictory without a documented reason.

---

## pack.review.diff-only

When to use: reviewing recent commits, PRs, or agent changes.

Read first:

- changed filenames;
- diff/patch;
- exact queue row/prompt contract;
- touched tests;
- touched evidence logs.

Avoid by default:

- whole repo scans;
- unrelated architecture docs;
- old queues not touched by the diff.

Max files before expansion: changed files + 3 related files

State ownership check: required if the diff touches data authority, persistence, idempotency, auth, payments/rewards, or generated evidence status.

Validation defaults:

```bash
git diff --check
<targeted tests from changed files>
```

Output mode: review-table

Freshness rule: diff/current files win over previous summaries.

Done blocker: no review conclusion based on docs alone when the diff touches runtime behavior.

---

## pack.docs.index-sync

When to use: making new docs/queues discoverable in `DOCS_INDEX.md`, routers, README files, or governance docs.

Read first:

- new/changed doc or queue;
- `docs/DOCS_INDEX.md`;
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md` if queue-related;
- `docs/DOCS_GOVERNANCE.md` if source-of-truth ordering is affected.

Avoid by default:

- runtime code;
- unrelated queue files;
- old completed archives.

Max files before expansion: 5

State ownership check: not applicable.

Validation defaults:

```bash
git diff --check
```

Output mode: brief-done

Freshness rule: latest created doc/queue must be indexed or explicitly recorded as intentionally unindexed.

Done blocker: no Done if a new active queue/doc is orphaned without a router/index/addendum path.

---

## pack.security.boundary-check

When to use: security/privacy/local-first boundary checks, secret handling, outbound integrations, cloud/SaaS boundaries.

Read first:

- `docs/SECURITY_AND_PRIVACY.md`
- `docs/RISK_REGISTER.md`
- `docs/COMMAND_CONTRACTS.md` for affected command
- exact source/config files touched
- exact tests touched

Avoid by default:

- UI/product docs not related to the boundary;
- token economy docs unless the task is about reducing context safely.

Max files before expansion: 8

State ownership check: required. Identify local/private/cloud/external ownership.

Validation defaults:

```bash
dotnet test --filter Security
```

Use exact available test names; if no tests exist, document the gap.

Output mode: review-table

Freshness rule: current implementation/config wins over older security plan.

Done blocker: no Done if data boundary or secret handling is uncertain.

---

## pack.feature-profile.gating

When to use: feature selection, optional modules, init profiles, command gating, reducing context by loading only selected feature packages.

Read first:

- `docs/AGENTWATCH_FEATURE_SELECTION_SPEC.md`
- `docs/prompt_queues/agentwatch_feature_selection.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CONFIG_REFERENCE.md`
- affected command/spec files

Avoid by default:

- dashboard/team/cloud docs unless the selected profile includes them;
- future-only specs when the task is core/local MVP;
- unrelated adapter docs.

Max files before expansion: 7

State ownership check: required for config ownership and feature flag persistence.

Validation defaults:

```bash
dotnet test --filter Feature
```

If feature tests do not exist yet, document the gap.

Output mode: review-table

Freshness rule: current config/command contract wins over old feature-selection notes.

Done blocker: no Done if commands can expose disabled features or if profile docs imply runtime behavior not implemented.

---

## State ownership as a token filter

Before reading broad files, ask:

```text
Who owns the state touched by this prompt?
backend / local cache / display-only / config / file system / git / external service
```

If ownership is known, read only the owner path first.

Examples:

- coins/XP/reward settlement: backend-authoritative first;
- pending reward display: local/display UI first;
- feature profile flags: config/command contract first;
- generated reports: report writer + command contract first;
- evidence status: queue row + `.ai/runs` first.

This prevents common waste: reading UI files for backend-owned bugs or reading backend docs for display-only polish.

## Queue lifecycle token flow

Future CLI flow from previous planning:

```text
agentwatch next   -> choose smallest safe prompt and context pack
agentwatch run    -> execute with budget and pack
agentwatch finish -> write evidence, token report, negative cache
agentwatch report -> summarize cost, validation, waste, next prompt
```

Until the CLI exists, agents should simulate this in run logs:

```text
Selected pack: <pack>
Budget: XS/S/M/L
Files opened before expansion: <n>
Negative cache: <paths/commands not to repeat>
Token report: <known/unknown metrics>
```
