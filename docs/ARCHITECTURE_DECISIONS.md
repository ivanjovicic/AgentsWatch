# AgentsWatch Architecture Decisions

Last aligned: 2026-08-25  
Status: active decisions

## ADR-001 — Local-first product

Decision: AgentsWatch starts as a local CLI and must be useful without cloud accounts.

Consequences:

- no telemetry/network calls by default;
- repository evidence stays local;
- cloud/team features come only after local verification proof.

## ADR-002 — Verification layer, not agent runtime

Decision: external coding agents execute work; AgentsWatch contracts and verifies it.

Consequences:

- no proprietary reasoning loop/cloud sandbox/session manager in MVP;
- integrations wrap stable contract/run/receipt use cases later;
- positioning centers on verified change, not orchestration breadth.

## ADR-003 — Modular monolith with ports/adapters

Decision: use one local modular application with clear boundaries.

Consequences:

- CLI/MCP can reuse application use cases;
- git/file/process/storage dependencies sit behind adapters where practical;
- no microservices/message bus/hosted database for MVP.

## ADR-004 — Structured JSON is canonical; Markdown is projection

Decision: RunContract, active-run baseline, RunDelta/RunReceipt are machine-readable JSON from the verification MVP start.

Why:

- evidence/drift/claims logic requires deterministic structured data;
- future MCP/GitHub integrations must not parse free-form Markdown;
- schema versioning is easier to validate and migrate.

Consequences:

- canonical paths live under `.agentwatch/`;
- Markdown under `.ai/` is generated for humans;
- SQLite is postponed until local-history queries justify it;
- reports must be regenerable from canonical JSON.

This supersedes the older `Markdown reports first, JSON later` interpretation.

## ADR-005 — Run attribution requires a start baseline

Decision: raw final `git status` is not sufficient to attribute changes to an agent run.

Consequences:

- `start` captures HEAD/branch and staged/unstaged/untracked state/fingerprints;
- `finish` computes start-to-end delta;
- pre-existing unchanged dirty files are excluded from attributable changes;
- pre-existing files changed further are surfaced with context;
- ambiguous attribution remains explicit.

## ADR-006 — Lossless git porcelain parsing

Decision: git status parsing must use a machine-safe porcelain contract, preferably NUL-delimited where appropriate.

Consequences:

- do not trim fixed-width prefixes before parsing;
- tests cover rename, spaces, staged/unstaged, delete, untracked;
- git parsing is an evidence primitive, not UI-string parsing.

## ADR-007 — Deterministic verification before LLM analysis

Decision: core Contract/Evidence/Scope/Claims checks work offline without provider keys.

Consequences:

- deterministic rule IDs and evidence references;
- `unknown` is valid;
- LLM semantic analysis may later be advisory, never the sole verification truth.

## ADR-008 — CLI is an interface, not application core

Decision: CLI parses/renders and calls reusable application use cases.

Consequences:

- avoid growing `Program.cs` with domain logic;
- future MCP/local API use the same RunContract/RunReceipt semantics;
- use-case tests can run without console parsing.

## ADR-009 — Adapters suggest validation by default

Decision: stack adapters suggest validation; execution is explicit.

Consequences:

- no surprising broad commands;
- structured imported/manual/CI validation evidence can exist without a command runner;
- command profiler/runner is a later optimization layer.

## ADR-010 — Universal git, .NET, Flutter first

Decision: focus initial verification support on universal git plus .NET and Flutter.

Consequences:

- existing React/Node/Python detection may remain but does not block MVP gates;
- adapter breadth is subordinate to correct attribution/receipts.

## ADR-011 — Dashboard and SaaS only after dogfood proof

Decision: do not build aggregate UI/team/cloud product before repeated local receipts prove value.

Gate:

- target at least 30 useful dogfood receipts;
- real unsupported claim catch;
- real scope-drift catch;
- real missing-evidence block;
- no known silent false-attribution issue in covered cases.

## ADR-012 — Learning/routing depend on trusted receipts

Decision: sophisticated mistake learning, token economy, and empirical model routing come after attribution/evidence are reliable.

Consequences:

- do not optimize noisy/untrusted data;
- route recommendations require comparable repository-local evidence;
- insufficient evidence returns `unknown`.
