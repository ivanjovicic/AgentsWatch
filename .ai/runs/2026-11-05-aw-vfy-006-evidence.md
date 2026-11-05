# AW-VFY-006 evidence: RunReceipt v1

- Prompt: AW-VFY-006; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Gate basis: AW-VFY-005 done locally (run-interval delta, 61/61). Not pushed; CI not run on 005/006.
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## RunReceipt v1 (`src/AgentsWatch.Core/Runs/RunReceipt.cs`)

schemaVersion=1, runId, contractId, taskId, startedAtUtc, finishedAtUtc, executorMetadata? (omitted when unknown),
startRepositoryState{branch, headCommitSha, worktreeState}, endRepositoryState{...}, headTransition, runDelta (AW-VFY-005 shape),
validations[] (ValidationEvidence v1: status Pass|Fail|NotRun|BlockedByEnvironment|TimedOut|Killed|Unknown, source AgentsWatchCommand|ImportedAgentResult|UserDeclared|CI|Unknown),
claims[] (AgentClaim v1), acceptanceCriteria[] (AcceptanceCriterionResult: Supported|Unsupported|Unknown), findings[] (Finding v1 with DATA_MODEL categories/status),
decision{status Done|NeedsEvidence|NeedsReview|NeedsApproval|Blocked|Failed, reasons[], override?}, missedWork[], attributionPolicy, evidenceDigest (sha256 of canonical JSON without the digest).
No learningNote/nextPrompt/routeSuggestion/optimizationAdvice/verboseSessionNarrative. No source contents or logs (fingerprints only).

## Example (compact, from smoke)

`{schemaVersion:1, runId:"t-1-20261105t191545z-46384a", contractId:"t-1", taskId:"T-1", executorMetadata:{agent:"smoke"}, runDelta:{runIntervalChanges:[a.txt M, new.txt ??], preExistingUnchangedChanges:[pre.txt ??], ...}, validations:[], claims:[], acceptanceCriteria:[{ac1,"a edited",unknown}], findings:[], decision:{status:"needs-evidence", reasons:["no validation evidence recorded","Evidence Gate not implemented yet (AW-VFY-007)..."]}, missedWork:[], evidenceDigest:"sha256:b961...f463"}`

## Placeholder semantics (for AW-VFY-007/008/009)

- validations/claims always empty here; nothing synthesized from prose.
- acceptance criteria copied from contract with status Unknown.
- findings: AWR001 ContractIncomplete (contract unloadable), AWR002 AttributionAmbiguous/NeedsReview per ambiguity.
- decision is always NeedsEvidence (never Done) until the Evidence Gate exists.

## Outputs and canonical boundary

finish order: delta record (`.agentwatch/runs/<id>.delta.json`, intermediate) -> receipt `.agentwatch/runs/<id>.json` (temp+rename, never overwritten) -> remove active baseline -> projections `.ai/runs/<id>.md` + `.ai/handoffs/<id>.md`. Projection failure is reported as a warning; the receipt stays valid (tested). Report sections: header/digest, Decision, Run-interval changes (authorship not proven), Pre-existing changed further, Pre-existing unchanged, Ambiguous, Validations (`none recorded - no validation result is claimed`), Claims, Acceptance criteria, Findings, Missed work, Attribution policy. Handoff: decision, files by class, validation, open findings, residual risk, `Next step (non-canonical)` which never touches the receipt. `receipt show` regenerates Markdown from JSON and warns on digest mismatch; `handoff` regenerates both files.

## Files

new: `src/AgentsWatch.Core/Runs/RunReceipt.cs`, `src/AgentsWatch.Reports/ReceiptProjections.cs`, `src/AgentsWatch.Cli/ReceiptCommand.cs`, `tests/AgentsWatch.Tests/RunReceiptTests.cs`
changed: `src/AgentsWatch.Core/Runs/RunDelta.cs` (finish writes receipt/projections via optional ports), `src/AgentsWatch.Cli/FinishCommand.cs`, `src/AgentsWatch.Cli/Program.cs`, queue/router/prompt status

## Tests (8 new)

E2E finish -> receipt + projections + placeholder decision + no validation claim; interval/pre-existing/ambiguous projection (incl. commit of dirty file); JSON round trip + schema 2/missing rejected; unknown executor + no learning fields; handoff next-step does not change receipt; projection failure leaves receipt intact; existing receipt refused + baseline kept; tampered receipt digest mismatch + CLI show/handoff.

## Gate (executed)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 69, Skipped 0, Total 69.

## E2E smoke (temp repo)

contract create -> pre-existing untracked pre.txt -> start --agent smoke -> edit a.txt + add new.txt -> finish (Decision NeedsEvidence; 2 interval, 1 pre-existing unchanged) -> receipt show (all sections, digest) -> handoff file present; JSON 115 lines, schema 1, validations 0.

## Limitations / left for later

- Evidence Gate decision rules (AW-VFY-007), scope findings (AW-VFY-008), claims (AW-VFY-009).
- Legacy `MarkdownReportWriter`/`HandoffSummaryWriter` (RunReport) left untouched; unused by new flow.
- delta.json duplicates receipt data (intermediate, non-canonical).
- Receipt includes full start/end worktree entry lists; large dirty repos make big receipts.
- Projections overwrite on regeneration (by design: generated).

Commit SHA (change): 8fb0d62