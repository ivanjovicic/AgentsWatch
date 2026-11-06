# AW-VFY-007 evidence: validation evidence and Evidence Gate v1

- Prompt: AW-VFY-007; queue `docs/prompt_queues/verification_mvp_2026_08_25.md`; run mode: implementation
- Gate basis: AW-VFY-006 done locally (RunReceipt v1, 69/69). This commit not pushed at write time.
- Model/client: unknown-not-exposed; timing: unknown-not-recorded

## Validation evidence

`ValidationEvidence` (already on RunReceipt): status Pass|Fail|NotRun|BlockedByEnvironment|TimedOut|Killed|Unknown;
source AgentsWatchCommand|ImportedAgentResult|UserDeclared|CI|Unknown. Import via `agentswatch evidence record`
(requires --command --status --source; optional --id/--exit-code/--summary/--digest/--source-ref). User-declared stays labeled `UserDeclared`.
Agent prose / `ValidationPassed` claims are ignored (tested). Matching a contract requirement: same validationId or exact commandDisplay.

## Rule IDs

- AWE000: requirements satisfied (Done reason)
- AWE001: receipt/contract incomplete
- AWE002: mandatory validation missing, NotRun or Unknown
- AWE003: Fail, TimedOut or Killed -> decision Failed
- AWE004: BlockedByEnvironment -> decision Blocked (never Pass)
- AWE006: expectedEvidence not backed by a Pass with outputSummary/evidenceDigest/sourceRef
- AWE007: acceptance criterion Unknown -> NeedsEvidence; Unsupported -> NeedsReview
- AWE008: permissionMode approval-required without auditable override -> NeedsApproval
- AWR002 (retained from finish): ambiguous changes listed in reasons; would downgrade Done to NeedsReview

## Decision matrix (first match)

Failed > Blocked > NeedsApproval > NeedsReview (unsupported AC) > NeedsEvidence (missing/unknown mandatory, expected evidence, unknown AC, incomplete receipt) > Done.
Every non-Done decision lists `ruleId: message [evidence refs]`. No score. Finish now applies the same gate (no placeholder "gate not implemented").

## Files

new: `src/AgentsWatch.Core/Runs/EvidenceGate.cs`, `src/AgentsWatch.Cli/EvidenceCommand.cs`, `tests/AgentsWatch.Tests/EvidenceGateTests.cs`
changed: `RunReceipt.cs` (Replace + builder calls gate), `Program.cs` (route/help), queue/router/prompt

## Tests (10 new)

missing validation cannot Done + reasons in Markdown; UserDeclared Pass satisfies mandatory and stays labeled (overall NeedsEvidence due to unknown AC); unknown AC blocks Done; Pass + supported AC -> Done and Markdown reasons; Fail -> Failed; BlockedByEnvironment -> Blocked; Pass without backing -> AWE006; prose claim is not evidence; approval-required -> NeedsApproval; CLI check/record.

## Gate (executed)

restore exit 0; build Release 0 warnings 0 errors; test Release: Failed 0, Passed 79, Skipped 0, Total 79.

## Smoke (temp repo)

finish after edit -> Decision NeedsEvidence. `evidence check` lists AWE002 (CLI create assigned requirement id `v1`), AWE006, AWE007, exit 1.
`evidence record --status fail --source ci` -> Failed / AWE003, exit 1.
`evidence record --status pass --source user-declared --summary "Passed 1"` -> source `user-declared` in JSON; Decision NeedsEvidence (only AWE007 unknown AC); Markdown projects the Pass (source: UserDeclared) and AWE007.

## Deferred

No command runner/profiler (evidence is imported, not executed). No semantic AC evaluation (tests mark AC Supported only to prove Done is reachable). No override CLI (override field exists on RunDecision). Scope/claims still later prompts.

Commit SHA (change): b6372aa

