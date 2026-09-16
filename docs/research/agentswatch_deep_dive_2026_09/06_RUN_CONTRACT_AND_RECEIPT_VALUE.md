# RunContract and RunReceipt Value

## RunContract

### Risk

Teams already use GitHub Issues, Jira, Linear, PR templates, prompts, AGENTS.md, Cursor rules, Claude/Codex instructions and repository policies. A second manually maintained task object would add friction.

### Recommendation

RunContract should be a **verification normalization layer**, not another project-management system.

Preferred flow:

`existing issue/prompt -> import/normalize -> ask only for missing verification fields`

Verification-specific fields should be minimal:
- acceptance criteria;
- owned paths;
- avoid paths;
- validation requirements;
- expected evidence;
- stop/approval rules.

Do not duplicate rich Jira/Linear metadata unless required for traceability.

## RunReceipt

### Why it can be valuable

A vendor session log and a run receipt solve different jobs.

Session log:
- detailed execution narrative;
- vendor-specific;
- potentially large/sensitive;
- may live only in vendor infrastructure.

RunReceipt:
- compact;
- repository/run scoped;
- explicit evidence provenance;
- portable across agent vendors;
- deterministic status reasons;
- designed for reviewer/CI/audit consumption.

## Recommended canonical receipt core

Keep:
- schema version / IDs / contract reference;
- executor metadata when known;
- start/end repository identity;
- run-interval delta and ambiguities;
- validations and provenance;
- structured claims/support result;
- scope/acceptance findings;
- decision + reasons;
- auditable override.

Move out of the canonical evidence artifact:
- `learningNote`;
- `nextPrompt`;
- optimization advice;
- verbose chat/session narrative.

Those can remain in Markdown handoffs/projections.

## Would anyone open it?

Developers may not manually open JSON receipts. That is acceptable if the receipt is consumed by:
- CLI summaries;
- GitHub checks;
- policy engines;
- reviewers;
- future audit/compliance tooling.

The receipt’s value is as a canonical evidence artifact, not as another dashboard page.

## Attestation direction

After MVP proof, evaluate:
- content-addressed receipt;
- evidence digests;
- signature/authentication;
- binding to repository revision/run identifiers;
- in-toto-compatible/custom predicate possibilities;
- GitHub Check/CI association.

Do not claim SLSA compliance: SLSA has its own source/build provenance semantics and trust requirements.
