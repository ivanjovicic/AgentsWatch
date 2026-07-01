# AgentsWatch Evidence Validation Follow-up Queue — 2026-07-01

Target repo: `ivanjovicic/AgentsWatch`  
Lane: evidence validator and manual workflow follow-ups  
Created from review of latest commits on 2026-07-01.

## Why this queue exists

Recent AgentsWatch commits added a validator script and manual workflow, but neither has been executed yet. Because AgentsWatch is supposed to productize this evidence loop, it should dogfood the validator before new feature work.

Important recent commits reviewed:

- `b237e16` — added `scripts/validate_agent_evidence.py`.
- `8960cd5` — required mechanical evidence validation in shared standard.
- `a70a6f3` — added manual Agent Evidence Validation workflow.

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-EVIDENCE-VAL-001 | Prompt-ready | Run the validator locally/CI-manually and fix only validator/evidence issues. |
| AW-EVIDENCE-VAL-002 | Prompt-ready after AW-EVIDENCE-VAL-001 | Record manual workflow result and decide if it can become a Gate 0 prerequisite. |
| AW-EVIDENCE-VAL-003 | Prompt-ready after AW-EVIDENCE-VAL-002 | Feed validator findings into AgentsWatch product requirements without adding runtime features. |

---

## AW-EVIDENCE-VAL-001 — Validator smoke and evidence repair

Run mode: validation-only / docs-evidence repair  
Token budget: low

### Goal

Prove `scripts/validate_agent_evidence.py` runs in AgentsWatch and repair only the evidence/docs issues it reports.

### Read first

- `AGENTS.md`
- `docs/AGENT_SHARED_OPERATING_STANDARD.md`
- `docs/AGENT_RUN_LOG_ENFORCEMENT.md`
- `docs/AGENT_RUN_EVIDENCE_STANDARD.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `.ai/runs/README.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`
- `scripts/validate_agent_evidence.py`

### Required work

1. Run:

```bash
python scripts/validate_agent_evidence.py --referenced-run-logs-only
```

2. If it fails due to validator runtime/syntax issue, fix only `scripts/validate_agent_evidence.py` and re-run.
3. If it fails due to missing evidence fields, repair only the referenced run logs/queue rows.
4. Do not edit CLI runtime/product features.
5. Record a run log and update this queue row.

### Owned paths

- `scripts/validate_agent_evidence.py` only if the validator itself fails
- referenced `.ai/runs/*.md` files only if required by validator output
- referenced queue rows only if required by validator output
- `.ai/runs/<yyyy-mm-dd>-AW-EVIDENCE-VAL-001-evidence.md`
- `docs/prompt_queues/agent_evidence_validation_followups_2026_07_01.md` status row only

### Avoid paths

- `src/**` or CLI runtime code
- product roadmap/spec expansion
- enabling automatic CI blocking

### Validation

```bash
python scripts/validate_agent_evidence.py --referenced-run-logs-only
```

If repaired and fast enough, also run:

```bash
python scripts/validate_agent_evidence.py
```

---

## AW-EVIDENCE-VAL-002 — Manual workflow result capture

Run mode: validation-only  
Token budget: low

### Goal

Run the manual GitHub Actions workflow and record whether it is safe enough to become part of Gate 0.

### Required work

1. Trigger `.github/workflows/agent-evidence-validation.yml` manually in `referenced` mode.
2. Record workflow run URL, status, and findings in `.ai/runs/<yyyy-mm-dd>-AW-EVIDENCE-VAL-002-evidence.md`.
3. If the workflow fails, classify failure:
   - validator runtime issue;
   - missing run-log field;
   - unknown mistake ID;
   - missing run-log path;
   - workflow configuration issue.
4. Update this queue row.
5. Do not make it automatic until referenced mode passes.

### Validation

Manual workflow: `Agent Evidence Validation`, mode `referenced`.

---

## AW-EVIDENCE-VAL-003 — Product requirement extraction from validator findings

Run mode: docs/product evidence  
Token budget: low

### Goal

Turn real validator findings into AgentsWatch product requirements without adding runtime features yet.

### Read first

- `.ai/runs/<yyyy-mm-dd>-AW-EVIDENCE-VAL-001-evidence.md`
- `.ai/runs/<yyyy-mm-dd>-AW-EVIDENCE-VAL-002-evidence.md`
- `docs/PRODUCT_SPEC.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_SPEC.md`
- `docs/WASTE_LEARNING_LOOP.md`

### Required output

Add or update a concise docs note describing which future CLI command should own the evidence validator behavior, for example:

```text
agentswatch lint evidence
agentswatch finish --retrospective
agentswatch mistakes check
```

### Stop rule

Do not implement the CLI command in this prompt. This is requirements extraction only.

### Validation

```bash
git diff --check
```
