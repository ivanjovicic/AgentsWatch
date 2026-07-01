# Token Economy Hardening Queue — 2026-07-01

Target repo: `ivanjovicic/AgentsWatch`  
Lane: token/context efficiency without harming correctness  
Run after Gate 0 validation or as docs-only planning while Gate 0 is incomplete.

## Purpose

Turn token-economy ideas into small, safe prompts that reduce waste without reducing validation, safety, or current-code proof.

## Read first

- `../CONTEXT_TOKEN_ECONOMY_BLUEPRINT_2026_07_01.md`
- `../TOKEN_WASTE_METRICS.md`
- `../PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `../ZERO_WASTE_EXECUTION_PROTOCOL.md`
- `../AGENT_RUN_EVIDENCE_STANDARD.md`
- `../WASTE_LEARNING_LOOP.md`
- `../DOCS_INDEX.md`

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-TOKEN-ECON-001 | Prompt-ready | Define named context packs for common task types. |
| AW-TOKEN-ECON-002 | Prompt-ready after 001 | Define repo-map output contract and stale/fresh labels. |
| AW-TOKEN-ECON-003 | Prompt-ready after 001 | Add cache-aware prompt skeleton with stable prefix and variable suffix. |
| AW-TOKEN-ECON-004 | Prompt-ready after 002 | Add stale-context guard prompt and evidence row format. |
| AW-TOKEN-ECON-005 | Prompt-ready after 001-004 | Add dogfood measurement plan across Flutter/backend/AgentsWatch. |
| AW-TOKEN-ECON-006 | Prompt-ready after Gate 0 | Map token economy concepts to future CLI command contracts. |

---

## AW-TOKEN-ECON-001 — Context pack specification

Run mode: docs/spec  
Token budget: low

### Goal

Create a compact spec for named context packs so agents stop rereading broad documentation before every prompt.

### Required work

1. Add `docs/CONTEXT_PACKS.md`.
2. Define at least these packs:
   - `pack.evidence.repair`
   - `pack.agentwatch.gate0`
   - `pack.backend.auth-risk`
   - `pack.flutter.ui-test`
   - `pack.cross-repo.standard-sync`
3. For each pack, include:
   - max files before expansion;
   - read-first files;
   - avoid files;
   - validation defaults;
   - stale-context rule.
4. Update `docs/DOCS_INDEX.md` to include the spec.
5. Do not implement CLI code.

### Validation

```bash
git diff --check
```

---

## AW-TOKEN-ECON-002 — Repo-map output contract

Run mode: docs/spec  
Token budget: low/medium

### Goal

Define a repo-map contract inspired by concise symbol maps: enough codebase structure to select files, not enough to replace current-code inspection.

### Required work

1. Add `docs/REPO_MAP_CONTRACT.md`.
2. Define output shape:
   - path;
   - language;
   - public types/functions/commands;
   - signatures only;
   - risk tags;
   - last changed commit/date if available;
   - freshness label.
3. Define exclusions:
   - generated files;
   - build artifacts;
   - vendored dependencies;
   - large snapshots/logs.
4. Define when full file read is allowed after repo-map use.
5. Add one example for AgentsWatch, one for backend, one for Flutter.

### Validation

```bash
git diff --check
```

---

## AW-TOKEN-ECON-003 — Cache-aware prompt skeleton

Run mode: docs/prompt  
Token budget: low

### Goal

Create a reusable prompt skeleton that preserves cache-friendly static prefixes and keeps variable data at the end.

### Required work

1. Add `docs/ai/prompts/CACHE_AWARE_AGENT_PROMPT_SKELETON.md`.
2. Split prompt into:
   - stable prefix;
   - selected context pack;
   - owned paths;
   - validation contract;
   - variable suffix.
3. Add rules:
   - no timestamps in stable prefix;
   - no tool output in stable prefix;
   - no branch-specific diff in stable prefix;
   - dynamic values go last.
4. Include short examples for evidence repair and implementation/test prompts.
5. Update `docs/DOCS_INDEX.md` if safe.

### Validation

```bash
git diff --check
```

---

## AW-TOKEN-ECON-004 — Stale-context guard

Run mode: docs/evidence  
Token budget: low

### Goal

Prevent token waste and correctness bugs from old docs, old audits, and stale run logs being treated as current truth.

### Required work

1. Add `docs/STALE_CONTEXT_GUARD.md` or update an existing guardrail if one exists.
2. Define labels: `fresh`, `recent`, `stale`, `unknown-freshness`.
3. Require agents to record when stale docs are used only for history.
4. Add Done blocker: no runtime Done claim based only on stale evidence.
5. Add run-log fields:
   - stale context opened;
   - current source that overrode stale context;
   - stale-context risk remaining.

### Validation

```bash
git diff --check
```

---

## AW-TOKEN-ECON-005 — Dogfood measurement plan

Run mode: docs/measurement  
Token budget: low

### Goal

Create a real measurement plan before making public savings claims.

### Required work

1. Add `docs/TOKEN_ECONOMY_DOGFOOD_PLAN_2026_07_01.md`.
2. Define at least 9 dogfood runs:
   - 3 Flutter;
   - 3 backend;
   - 3 AgentsWatch.
3. For each run, record expected pack, baseline style, improved style, metrics, validation, and stop rules.
4. Require no public percentage claims until data exists.
5. Include failure cases where token reduction made output worse.

### Validation

```bash
git diff --check
```

---

## AW-TOKEN-ECON-006 — Future CLI command contracts

Run mode: docs/product contract  
Token budget: medium

### Goal

Map token-economy features to future AgentsWatch CLI commands without implementing runtime code prematurely.

### Required work

Update `docs/COMMAND_CONTRACTS.md` or add a small addendum covering:

```text
agentswatch context plan <prompt-id>
agentswatch context pack <pack-name>
agentswatch context map
agentswatch context stale-check
agentswatch tokens report
agentswatch evidence compact
```

For each command, define:

- inputs;
- output shape;
- safety rule;
- what it must not read by default;
- validation/test anchors for future implementation.

### Stop rule

Do not implement CLI behavior in this prompt.

### Validation

```bash
git diff --check
```
