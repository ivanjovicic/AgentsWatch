# AgentsWatch Agent Rulebook

AgentsWatch is a local-first AI coding-agent supervisor and token optimizer.

## Source of truth

1. Current code and tests.
2. Commit-bound CI, acceptance, package, and release proof artifacts.
3. `README.md`.
4. `docs/AGENT_SHARED_OPERATING_STANDARD.md`.
5. `docs/AGENT_RUN_LOG_ENFORCEMENT.md`.
6. `docs/PROOF_AND_VERIFICATION_STRATEGY.md`.
7. `docs/FEATURE_CAPABILITY_REGISTRY.md` and `docs/FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`.
8. `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`.
9. `.ai/RUN_LOG_TEMPLATE.md`, `.ai/DISCOVERY_RECORD_TEMPLATE.md`, and `.ai/runs/README.md`.
10. `docs/ai/learning/MISTAKE_LEDGER.md`.
11. `docs/DOCS_INDEX.md`.
12. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
13. `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md` when the user asks for the next prompt.
14. `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` and `docs/PROMPT_LINT_CHECKLIST.md`.
15. `docs/ZERO_WASTE_EXECUTION_PROTOCOL.md`.
16. `docs/AGENT_RUN_EVIDENCE_STANDARD.md`.
17. `docs/WASTE_LEARNING_LOOP.md`.
18. `docs/PROMPT_BATCH_REVIEW_POLICY.md`.
19. `docs/AGENT_OPERATING_SYSTEM.md`.
20. `docs/CONTEXT_INDEX.md`.
21. Bootstrap validation docs while Gate 0 is incomplete.
22. Product contracts: `docs/CLI_SPEC.md`, `docs/COMMAND_CONTRACTS.md`, `docs/CLI_UX_OUTPUT_SPEC.md`, `docs/CONFIG_REFERENCE.md`, `docs/REPORT_FORMATS.md`, `docs/DATA_MODEL.md`, `docs/ADAPTER_SPEC.md`.
23. Prompt queues under `docs/prompt_queues/`.

If documents disagree, current code/tests and commit-matched proof artifacts win over planning notes, roadmap status, or chat history.

## Product rules

- Build local CLI first.
- Do not start with SaaS, billing, cloud sync, or dashboard before roadmap gates allow it.
- Universal git/markdown/file-system behavior comes before language adapters.
- Risk scoring must stay heuristic and explainable.
- Keep prompts small and split broad work.
- Markdown report contracts come before SQLite/dashboard work.
- No hidden telemetry or network calls in MVP.
- Capture unrelated findings instead of silently losing them or fixing them through scope creep.
- Register every advertised capability and never claim maturity above available evidence.

## Token economy rule

Every non-trivial prompt must pass `docs/PROMPT_LINT_CHECKLIST.md` before execution.

Use `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md` and `docs/AGENT_SHARED_OPERATING_STANDARD.md` as hard authority for:

- green/yellow/red prompt classification;
- token budgets;
- file read/edit/search limits;
- run mode enforcement;
- prompt split rules;
- stop rules;
- final evidence requirements.

Reject or rewrite prompts that fail lint.

## Run evidence and learning rule

Every non-trivial run must leave realistic `.ai/runs/<date>-<prompt-id>-evidence.md` evidence before it is considered complete.

The agent must record:

- model/client metadata or `unknown-not-exposed`;
- elapsed/phase timing or `unknown-not-recorded`;
- what was done;
- what was missed;
- files inspected;
- files changed;
- validation run or why it did not run;
- where time/tokens were wasted;
- why waste happened;
- relevant prior mistakes read;
- mistakes observed or `none`;
- docs/rules updated to prevent repeat;
- optimized prompt added or reason none was needed;
- out-of-scope discoveries observed or `none found`;
- discovery records created/updated or duplicates linked;
- primary owner and queue/prompt routing for actionable discoveries;
- affected capability IDs;
- proof/traceability rows updated or reason not applicable;
- unresolved discovery IDs or `none`;
- follow-up prompt;
- residual risk;
- commit SHA.

For every meaningful issue, waste item, blocker, stale reference, unclear rule, repeated failure, proof gap, or out-of-scope finding, the agent must do at least one of:

1. update an existing docs rule;
2. add a new rule to the relevant playbook;
3. update the prompt queue;
4. add a new optimized prompt;
5. update `docs/ai/learning/MISTAKE_LEDGER.md`;
6. add or update a lint/test prompt;
7. create or update a discovery record and assign a primary owner;
8. update the capability registry/traceability matrix and add missing proof work;
9. record why no rule, prompt, discovery, or proof update was needed.

A prompt cannot be marked high-confidence `Done` unless it references a run log or explicit fallback, and the score obeys `docs/AGENT_RUN_LOG_ENFORCEMENT.md`.

## Out-of-scope discovery rule

Use `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md` for every meaningful finding that should not be implemented inside the active prompt.

Hard rule:

```text
Do not fix unrelated work inside the current task.
Do not lose it either.
Capture -> deduplicate -> classify -> route -> generate follow-up -> verify closure.
```

Before learning-complete status:

1. extract findings from missed work, residual risk, mistakes, waste, blockers, and follow-up notes;
2. search `.ai/discoveries/`, mistake cards, risks, prompts, and queues for the same root issue;
3. create/update/link a discovery record;
4. assign one primary owner;
5. select a truthful status and gate;
6. create a focused prompt or record a no-op/rejection reason;
7. link the discovery and prompt from the run log.

Use DISC-001 through DISC-006 from the discovery queue. Do not describe docs/manual workflows as implemented CLI automation.

## Capability proof rule

Use `docs/PROOF_AND_VERIFICATION_STRATEGY.md` whenever a task implements, tests, documents, releases, or advertises a product capability.

Hard rule:

```text
No registry row = no supported feature claim.
No executed evidence = no verified claim.
No commit match = no proof for this version.
```

Maturity levels:

```text
L0 Idea
L1 Specified
L2 Implemented
L3 Test-backed
L4 CI-verified
L5 Dogfood-verified
L6 Release-verified
```

Before raising maturity:

1. identify capability IDs;
2. verify canonical contract and observable acceptance criteria;
3. link implementation paths;
4. add and execute targeted tests;
5. execute required black-box scenarios;
6. link CI evidence for the same commit;
7. update registry and traceability matrix;
8. retain failures, skips, blockers, and limitations;
9. require dogfood for usefulness/value claims;
10. require package checksum and clean-install evidence for release support.

Test source alone does not prove tests passed. A green build alone does not prove every capability. Percentage token/time/cost claims require measured paired evidence under `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`.

Use AW-PROOF-001 through AW-PROOF-007 from `docs/prompt_queues/agentwatch_proof_and_verification.md`.

## Prompt batch review rule

After 3-5 important prompt, queue, rule, evidence, discovery, proof, or agent-workflow commits, run `docs/PROMPT_BATCH_REVIEW_POLICY.md`.

Batch review must check:

- broken references;
- stale queue statuses;
- prompts marked Ready despite blocked gates;
- missing validation evidence;
- missing discovery reconciliation;
- unowned or duplicate discoveries;
- capability claims missing from registry/matrix;
- maturity levels exceeding evidence;
- proof artifacts referencing another commit;
- contradictions between rulebooks, router, queues, contracts, tests, and proof documents;
- missing follow-up prompts for discovered issues.

If review finds more than three unrelated issues, add/reconcile discoveries and follow-up prompts instead of fixing everything in one run.

## Bootstrap rule

Gate 0 is not complete until restore/build/test and CLI smoke evidence exists for the current commit.

Until then, work must prioritize:

1. `AW-VAL-001` build validation;
2. `AW-VAL-002` CLI smoke validation;
3. `AW-VAL-003` validation evidence review;
4. `AW-VAL-004` init hardening.

The CI proof workflow may produce Gate 0 evidence now. Do not add new CLI features before build/test/smoke evidence exists.

Docs/evidence discovery and proof workflows may run now. Runtime discovery/proof commands remain gated by their implementation queues.

## Required prompt fields

Every non-trivial task must include:

- repository;
- prompt id;
- queue;
- run mode;
- token budget;
- affected capability IDs when applicable;
- source discovery IDs when applicable;
- scope limiter;
- owned paths;
- avoid paths;
- stop rules;
- validation;
- expected evidence;
- expected proof/traceability updates;
- expected discovery reconciliation;
- relevant prior mistakes read;
- handoff summary when split or blocked.

Use investigation-only first when root cause is unknown. Use diff-only review after implementation commits.

## Working order

1. Read `docs/AGENT_SHARED_OPERATING_STANDARD.md`.
2. Read `docs/AGENT_RUN_LOG_ENFORCEMENT.md` and choose/create the `.ai/runs` path.
3. Read `docs/ai/learning/MISTAKE_LEDGER.md` and select relevant mistake IDs.
4. Read `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
5. Lint the prompt with `docs/PROMPT_LINT_CHECKLIST.md`.
6. Apply token/context limits.
7. Read the owning queue and smallest relevant context pack.
8. Identify affected capability IDs from `FEATURE_CAPABILITY_REGISTRY.md`.
9. Inspect only relevant docs/files.
10. Make the smallest safe change.
11. Add targeted tests when runtime behavior changes.
12. Run narrow validation and required acceptance scenarios when possible.
13. Record validation honestly.
14. Update capability registry/traceability only to the evidenced level.
15. Record run evidence using `.ai/RUN_LOG_TEMPLATE.md`.
16. Apply waste/mistake learning.
17. Reconcile discoveries.
18. Generate follow-up proof/prompts/queue rows for missing evidence.
19. Apply batch review when triggered.
20. Mark status with run-log and proof references.
21. Commit and push using the requested branch/PR flow; never claim main changed when work only exists on a branch.

## Validation defaults

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

For user-visible CLI changes, also run relevant black-box scenarios from `REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`.

Do not claim validation passed unless it was actually run or matching CI evidence is available.
