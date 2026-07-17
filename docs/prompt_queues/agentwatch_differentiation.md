# AgentsWatch Differentiation Queue

Last aligned: 2026-07-17  
Status: prioritized execution queue

## Rule

Do not implement integrations, dashboard, SaaS, generic scheduling, or autonomous execution before the internal contract/receipt/evidence spine is proven.

Use one prompt and one run mode per run.

## Gate 0 — current skeleton

Existing validation prompts remain first:

```text
AW-VAL-001 — restore/build/test validation
AW-VAL-002 — CLI smoke validation
```

No differentiated runtime implementation begins until Gate 0 has evidence.

## Queue

### DIF-001 — Agent Run Receipt contract

Run mode: investigation/design  
Token budget: low  
Permission mode: read_only  
Depends on: AW-VAL-001, AW-VAL-002

Define the smallest receipt that can normalize a manual Codex/Cursor/Claude run without full chat history.

Return:

- markdown contract;
- JSON sidecar contract for later;
- required vs optional fields;
- secret/redaction rules;
- status transitions;
- example Flutter and .NET receipts;
- implementation prompt.

Stop before code.

### DIF-002 — Run lifecycle and receipt implementation

Run mode: implementation  
Token budget: medium  
Permission mode: local_code  
Depends on: DIF-001

Implement the smallest slice:

```text
agentswatch start <task-id>
agentswatch finish <task-id>
agentswatch receipt create <run-id>
agentswatch receipt check <run-id>
```

Owned scope must be explicit in the generated implementation prompt.

Validation:

- targeted unit tests;
- CLI smoke scenario in a temporary repository;
- no writes outside expected local paths.

### DIF-003 — Evidence lint

Run mode: implementation/tests  
Token budget: low/medium  
Permission mode: local_code  
Depends on: DIF-002

Add deterministic checks for:

- missing changed-file evidence;
- runtime change without validation or blocked reason;
- missing learning note;
- missing next prompt/completion note;
- receipt status inconsistent with evidence.

Do not add scoring yet.

### DIF-004 — Roadmap Contract Compiler

Run mode: investigation/design, then separate implementation prompt  
Token budget: medium  
Permission mode: read_only for design  
Depends on: DIF-003

Define and then implement:

```text
agentswatch contract check <file>
agentswatch contract build <roadmap-item-or-prompt>
```

Required fields:

- intent;
- acceptance criteria;
- dependencies;
- owned/avoid paths;
- permission/run mode;
- budget;
- validation;
- stop rules;
- expected evidence.

Incomplete contracts must produce investigation/planning output, not implementation output.

### DIF-005 — Claims/diff/validation gate

Run mode: implementation/tests  
Token budget: medium  
Permission mode: local_code  
Depends on: DIF-003, DIF-004

Start with deterministic claims:

```text
tests added
backend changed
frontend changed
docs updated
validation passed
```

Compare claims with changed-file patterns and validation records.

Return findings before adding any score.

### DIF-006 — Scope and roadmap drift

Run mode: implementation/tests  
Token budget: medium  
Permission mode: local_code  
Depends on: DIF-004, DIF-005

Detect:

- changed files outside owned paths;
- required paths/deliverables missing;
- dependency gate skipped;
- roadmap item marked Done without acceptance evidence.

Add explainable Scope Drift findings.

### DIF-007 — Explainable Evidence Score

Run mode: investigation/design, then implementation  
Token budget: medium  
Permission mode: read_only for design  
Depends on: DIF-005, DIF-006

Design a score that summarizes deterministic findings without hiding them.

Rules:

- score is secondary to findings;
- every point change is explainable;
- user override requires a reason;
- no fake precision.

### DIF-008 — Validation Economy

Run mode: implementation/tests  
Token budget: medium  
Permission mode: local_code  
Depends on: trustworthy validation evidence

Implement the smallest command-profile and recommendation slice:

- duration/status/output-size proxy;
- repeated-command detection;
- targeted validation suggestion;
- compact error signature;
- avoidable command-time estimate.

Do not store full output by default.

### DIF-009 — Counterfactual learning

Run mode: investigation/design  
Token budget: medium  
Permission mode: read_only  
Depends on: at least 10 dogfood receipts

Design learning rules with:

- category;
- evidence count;
- repository/task scope;
- confidence;
- accepted/rejected status;
- expiry/deprecation;
- counterfactual prompt/context/validation route.

Do not implement opaque automatic rule creation.

### DIF-010 — Cross-agent empirical router

Run mode: research/design  
Token budget: high  
Permission mode: read_only  
Depends on: at least 30 comparable receipts

Design recommendations based on repository-local comparable runs.

Required result:

- task classification;
- minimum evidence threshold;
- quality/risk/cost inputs;
- confidence;
- explanation;
- fallback;
- `unknown` result when evidence is insufficient.

No hard-coded vendor winner.

### DIF-011 — Thin integration contracts

Run mode: investigation/design  
Token budget: medium  
Permission mode: read_only  
Depends on: stable receipt and contract schemas

Define adapters in this order:

1. MCP tools.
2. Codex skill/plugin.
3. Cursor preflight/postflight.
4. GitHub check/agent app.
5. Superplane component.
6. Devin/OpenHands receipt import.

External products execute. AgentsWatch contracts, verifies and learns.

## Explicitly blocked until later

```text
continuous autopilot
visual workflow canvas
cloud sandbox/runtime
generic scheduling
hosted dashboard
team SaaS
production deployment orchestration
automatic merge/release
integration marketplace
```

## Dogfood checkpoints

After 5 receipts:

- review receipt usefulness;
- remove unused fields.

After 10 receipts:

- review evidence/drift false positives;
- begin learning design.

After 30 comparable receipts:

- test empirical routing hypothesis;
- decide whether dashboard adds value.
