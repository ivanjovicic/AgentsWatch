# AgentsWatch Agent Rulebook

AgentsWatch is a local-first, vendor-neutral verification and evidence layer for AI coding agents.

## Product truth

External agents execute coding work. AgentsWatch contracts, attributes, verifies, records evidence, and learns from trusted receipts.

Core promise:

```text
Turn roadmap intent into verified change — across any coding agent.
```

Do not turn AgentsWatch into another agent runtime, generic control plane, workflow engine, session viewer, or token dashboard.

## Canonical source of truth

Read the smallest relevant set. Default authority order:

1. current code and tests;
2. current CI/run evidence;
3. `README.md`;
4. `docs/PRODUCT_SPEC.md`;
5. `docs/MVP_ROADMAP.md`;
6. `docs/ARCHITECTURE.md`;
7. `docs/DATA_MODEL.md`;
8. `docs/COMMAND_CONTRACTS.md` / `docs/CLI_SPEC.md` for CLI work;
9. `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`;
10. selected prompt file from the canonical active queue.

Use `docs/DOCS_INDEX.md` only when you need a non-canonical specialist document.

Historical token-economy, productization, old roadmap, old queue, and research documents are supporting context only. They must not override the current verification-first product/roadmap.

## Current execution authority

Canonical active queue:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

Current next prompt is determined by:

```text
docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
```

Do not select an old `AW-VAL-*`, `AW-TOKEN-*`, productization, dashboard, or architecture-expansion prompt merely because its historical file says `Ready`.

## Current Gate 0 evidence

Latest known CI evidence:

```text
restore: pass
build: pass
tests: fail in GitStatusParserTests
```

Known parser bug is documented in `docs/BOOTSTRAP_NEXT_STEPS.md`.

Current priority:

1. `AW-VFY-001` — git parser/CI hardening;
2. `AW-VFY-002` — CLI smoke/Gate 0 closure;
3. then verification MVP prompts in strict dependency order.

Do not add new runtime features before Gate 0 is green.

## Verification-first product rules

- JSON is canonical for RunContract, active-run baseline, and RunReceipt.
- Markdown is a generated human-readable projection.
- Verification logic must not parse free-form Markdown as its source of truth.
- Raw final `git status` is not equivalent to run-attributable changes.
- `start` must preserve pre-existing staged/unstaged/untracked state.
- `finish` must compute attributable delta and surface ambiguity rather than guess.
- Scope and claim checks operate on attributable changes.
- Mandatory validation missing means the run cannot be `Done`.
- Agent prose is a claim, not evidence.
- Deterministic checks come before LLM interpretation.
- `unknown` and `ambiguous` are valid results.
- No opaque score may independently decide completion.
- Risky actions require explicit approval.
- No hidden telemetry or network calls in MVP.

## Architecture rules

- Keep a modular monolith.
- CLI is an interface, not the application core.
- After Gate 0, introduce reusable application use cases/ports before growing `Program.cs`.
- External dependencies such as git/process/file storage belong behind adapters/ports where practical.
- Start with universal git behavior, then .NET and Flutter.
- Do not introduce microservices, message buses, cloud storage, SaaS auth, or hosted infrastructure for the MVP.

## Prompt shape

Every non-trivial implementation prompt must include:

- repository;
- prompt ID;
- owning queue;
- dependency/gate;
- run mode;
- token/context budget guidance;
- exact task;
- owned paths;
- avoid paths;
- stop rules;
- tests/validation;
- expected evidence;
- final response shape.

Prefer one implementation slice per prompt.

Use investigation-only first when the root cause or contract is materially unknown.

## Scope discipline

Before reading many files:

1. read the selected prompt;
2. read only its canonical contract docs;
3. inspect exact relevant code/tests;
4. expand scope only when evidence requires it;
5. record why scope expanded.

Do not preload the full documentation tree.

Specialist context documents such as token-economy research, licensing, learning, productization, or historical audits should be read only when the selected task directly requires them.

## Implementation rules

- make the smallest coherent safe change;
- add targeted tests for runtime behavior;
- preserve public schemas/paths unless the prompt explicitly changes them;
- avoid opportunistic refactors;
- do not mix docs/product strategy changes into a runtime feature prompt unless required for truth synchronization;
- never claim tests/validation passed without executed evidence;
- if blocked by environment, record the block instead of altering product behavior to bypass it.

## Git evidence rules

Use a lossless machine-safe porcelain contract for status parsing, preferably NUL-delimited porcelain where appropriate.

Tests should cover as relevant:

- staged/unstaged modification;
- add/delete;
- rename;
- untracked;
- filenames with spaces;
- dirty-at-start attribution;
- changed-further-after-start attribution;
- ambiguous cases.

Do not trim fixed-width git porcelain prefixes before parsing them.

## Run evidence

Every non-trivial implementation/validation run should leave compact evidence under `.ai/runs/` or the repository's current evidence convention.

Record at minimum:

- prompt/run ID;
- what changed;
- files changed;
- tests/validation actually run;
- failures/blocks;
- missed work;
- follow-up prompt/status;
- commit SHA when committed.

Do not paste full chat history or full terminal logs.

## Learning rule

Learning work is downstream of trustworthy receipts.

Before the receipt/attribution/evidence spine is stable, do not spend implementation capacity on sophisticated model routing, generic token optimization, large mistake-learning engines, or dashboards.

After dogfood receipts exist, learning rules must be:

- scoped;
- evidence-backed;
- reviewable;
- confidence-labeled;
- expirable/deprecatable.

## Working order

1. Read this file.
2. Read `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.
3. Select exactly one currently claimable prompt from `verification_mvp_2026_08_25.md`.
4. Read the prompt file and only its required canonical docs.
5. Inspect relevant code/tests.
6. Make the smallest safe change.
7. Run targeted validation, then broader gate validation when required.
8. Record evidence honestly.
9. Update queue status/evidence references when the prompt is completed or blocked.
10. Commit/push according to the user's requested flow.

## Default validation for current .NET skeleton

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln --configuration Release
dotnet test AgentsWatch.sln --configuration Release
```

Use narrower test filters during iteration when useful, but final Gate 0 closure requires the full test gate.
