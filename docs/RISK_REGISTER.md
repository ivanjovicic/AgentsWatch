# AgentsWatch Risk Register

Last aligned: 2026-08-25  
Status: active

## Current risk summary

| ID | Risk | Level | Why it matters | Mitigation / owner prompt |
|---|---|---|---|---|
| R-001 | Git parser corrupts porcelain paths | High / active | Current CI test fails and Git evidence is foundational. | `AW-VFY-001`: lossless porcelain parsing + edge-case tests. |
| R-002 | CLI smoke/local-write behavior not fully proven | High / active | Gate 0 cannot close without real CLI behavior evidence. | `AW-VFY-002`: temp-repo smoke and init no-overwrite tests. |
| R-003 | False attribution from pre-existing dirty worktree | Critical | If end-state dirtiness is mistaken for run changes, receipts, scope drift, and claims become untrustworthy. | `AW-VFY-004/005`: start baseline + attributable delta + dirty-at-start tests. |
| R-004 | Markdown becomes accidental source of truth | High | Free-form parsing will make verification brittle and integrations incompatible. | JSON-first RunContract/RunReceipt; Markdown projection only. |
| R-005 | Contract schema drifts between commands/integrations | High | CLI/MCP/GitHub adapters could disagree on task intent and evidence. | `AW-VFY-003`: schema version + deterministic lint + canonical storage. |
| R-006 | Receipt claims validation that was never evidenced | Critical | Product promise fails if agent prose is treated as proof. | `AW-VFY-007`: typed validation sources/statuses; mandatory evidence gate. |
| R-007 | Scope drift computed from raw git status | Critical | Pre-existing unrelated files would create false positives. | `AW-VFY-008` must consume RunDelta attributable changes only. |
| R-008 | Claims checker overstates certainty | High | `tests added` / `validation passed` checks could become another heuristic summary. | `AW-VFY-009`: deterministic supported/unsupported/unknown with evidence refs. |
| R-009 | Path/glob behavior differs by OS | Medium | Scope rules become unpredictable across Windows/Linux/macOS. | Central path normalization/matching tests in AW-VFY-008. |
| R-010 | Branch/HEAD changes during a run confuse attribution | High | Commit/reset/checkout during agent execution may make delta ambiguous. | Record transitions; handle deterministically or mark `Ambiguous`, never guess. |
| R-011 | Overengineering ports/storage before product proof | Medium | Architecture work can delay first verified receipt. | Introduce only minimum reusable use cases/ports needed by each prompt. |
| R-012 | Token optimizer/observability scope distracts from verification | High | Market already has broad agent tracking/control features; weakens differentiation. | Keep token/cost/command optimization post-receipt and post-dogfood. |
| R-013 | Dashboard/SaaS scope creep | High | UI/cloud work can mask an unproven core loop. | Block until AW-VFY-010 dogfood evidence. |
| R-014 | Learning/router trained on noisy receipts | High | Bad attribution/evidence would produce bad recommendations. | Learning/routing only after trustworthy dogfood receipts. |
| R-015 | Sensitive source/log data accidentally persisted | High | Local-first trust depends on minimal evidence capture. | Store fingerprints/summaries; redact secrets; no full source/chat/logs by default. |
| R-016 | Too many stale docs/queues redirect agents to old vision | Medium | Agents may implement token-first or old AW-VAL flow. | Canonical router/queue + legacy queue markers + reduced AGENTS/DOCS_INDEX. |

## Current gate

Before new verification runtime features:

1. complete `AW-VFY-001` and make full tests green;
2. complete `AW-VFY-002` and close CLI Gate 0.

Then follow only:

`docs/prompt_queues/verification_mvp_2026_08_25.md`

## Critical product invariant

```text
No trustworthy attribution -> no trustworthy receipt -> no trustworthy verification.
```

Therefore dirty-worktree attribution is a release-blocking correctness concern, not a nice-to-have enhancement.

## Evidence honesty rule

For any risk finding:

```text
Risk checked:
Evidence:
Attribution confidence:
Validation actually run:
Files changed:
Remaining ambiguity:
Follow-up prompt:
```

Do not convert unknown/ambiguous evidence into a passing result merely to close a prompt.

## High-risk implementation areas

Treat these as high risk during the MVP:

- git porcelain parsing and process execution;
- baseline/delta attribution;
- file-system writes under `.agentwatch` / `.ai`;
- schema serialization/versioning;
- path/glob normalization;
- validation evidence ingestion;
- run decision logic;
- command dispatch in CLI;
- future command execution/MCP/GitHub integration.
