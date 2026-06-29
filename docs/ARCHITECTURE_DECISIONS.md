# AgentsWatch Architecture Decisions

Last aligned: 2026-06-29  
Status: active decisions

## ADR-001 — Local-first product

Decision: AgentsWatch starts as a local CLI and must be useful without cloud accounts.

Why:

- fastest useful MVP;
- highest trust for source code and prompts;
- no billing/auth/cloud complexity early;
- easier dogfood on private repos.

Consequences:

- markdown and local files first;
- no telemetry by default;
- cloud sync only after explicit opt-in later.

---

## ADR-002 — Modular monolith, not microservices

Decision: use a modular monolith with clear boundaries.

Why:

- the product is early;
- local CLI/dashboard need shared logic;
- microservices add operational cost without value.

Consequences:

- modules must have clear dependencies;
- future SaaS can extract services only after use cases stabilize.

---

## ADR-003 — Ports and adapters

Decision: domain/application logic must not depend directly on git CLI, file system, process runner, SQLite, HTTP, or GitHub APIs.

Why:

- testability;
- future dashboard/API reuse;
- safer integration expansion.

Consequences:

- introduce ports before deep feature growth;
- adapters wrap external systems;
- use cases depend on abstractions.

---

## ADR-004 — Markdown reports first

Decision: markdown reports are the first source of user-facing evidence.

Why:

- easy to read;
- git-friendly;
- no database needed;
- trustworthy for solo users.

Consequences:

- JSON/SQLite comes later;
- markdown format must remain stable enough to map to future data model.

---

## ADR-005 — Deterministic heuristics before LLM analysis

Decision: MVP risk scoring and prompt optimization should be deterministic and explainable.

Why:

- users need to trust why something is risky;
- no provider key required;
- works offline;
- easier to test.

Consequences:

- LLM integrations may later improve text generation, but should not be required for core value.

---

## ADR-006 — CLI is an interface, not the application core

Decision: CLI should parse commands and call application use cases.

Why:

- later local API/dashboard should reuse logic;
- avoids large `Program.cs` becoming untestable.

Consequences:

- extract use cases after Gate 0;
- keep console rendering separate from domain decisions.

---

## ADR-007 — Adapters suggest validation by default

Decision: language adapters suggest validation commands but should not execute them unless explicitly requested.

Why:

- avoids surprising user commands;
- safer across different stacks;
- works for read-only/report-only workflows.

Consequences:

- `validate` command should be explicit;
- reports can show suggested vs actually run validation.

---

## ADR-008 — Dashboard only after dogfood evidence

Decision: do not build dashboard until CLI reports are useful and reused.

Why:

- dashboard can distract from core value;
- markdown reports must prove value first;
- dogfood data should shape dashboard UX.

Consequences:

- dashboard is blocked by validation gates;
- local API/storage design should prepare for it without implementing it early.

---

## ADR-009 — SaaS only after local trust

Decision: SaaS/team features come after local CLI/dashboard proves regular usage.

Why:

- avoids premature billing/auth/cloud work;
- source/privacy trust matters;
- token-saving claim must be evidenced first.

Consequences:

- cloud architecture remains a future extension;
- local product must stand alone.
