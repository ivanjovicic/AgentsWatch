# AW-ORCH-RFC-001 — Codex/Cursor implementation with Claude/ChatGPT web reviews

Date: 2026-11-10 (Europe/Belgrade); refreshed 2026-11-13 onto main f3315267ce67c3c258453d597242b946c9359bcb (reference corrections only; proposal content unchanged)
Status: PROPOSAL / docs-only / NOT APPROVED FOR IMPLEMENTATION
Review status: a separate-agent AI text review was completed for this document version. No human review is requested for this documentation PR. The second-agent review is advisory only; it is not a GitHub human review/approval, authenticated provenance or proof of semantic acceptance.
Scope: optional companion workflow; no changes to AgentsWatch runtime, gate decisions or active queue
Sponsor intent: implement with Codex or Cursor; review with Claude web and/or ChatGPT web where supported.

## 1. Decision and current-state check

Recommended path: **review-first companion**, not a replacement for AgentsWatch's verification MVP, a proprietary agent runtime, or a new authoritative task queue.

Foundation on main at f3315267ce67c3c258453d597242b946c9359bcb (implemented capabilities as stated in AGENTS.md; the absence claims below were re-checked on 2026-11-13 by a source search for dispatcher, lease, execution-attempt, review-packet and orchestration artefacts, which found none; main will drift after this date):
- .NET 8 modular CLI: AgentsWatch.Cli/Core/Git/LanguageAdapters/Reports and Tests.
- Contract/start baseline/finish delta/receipt/evidence/scope/claims/acceptance checks are implemented.
- No production agent-dispatcher, lease service, provider execution adapters, GitHub webhook ingestion service, hosted backend, web reviewer control API, or review automation exists in the inspected runtime.
- Run interval evidence does not prove causal authorship. Manually declared executor and manually imported validation must not be treated as externally attested.
- The companion must consume RunContract and RunReceipt without changing the canonical schemas or verdict ordering merely to support automation.

Active governance:
- AW-VFY-010 Gate 5 = ADJUST / PAUSE, 12/30 useful REAL; do not run B5 or count rehearsal/synthetic flows as REAL.
- AW-VFY-011/012 and hosted/product expansion remain HOLD. This RFC does not lift them.
- No merge, deploy, production mutation, branch takeover, workflow activation, outreach or external API billing authorized by approval to WRITE THIS PLAN.
- An explicitly separate owner GO is required for implementation and each later activation. Preserve canonical router/verification queue.
- PR #24 (AW-FIX-DIGEST-001, platform-independent receipt and contract digests), PR #21 (AW-FIX-EXECUTOR-001, executor identity) and PR #25 (docs sync) are merged into main (merge commits dc7bf99, e0770dc and f331526). Merged is not independently reviewed: as of 2026-11-13 the GitHub reviews API shows no review submission on #21 or #24, so this RFC claims no independent human review for either. Executor identity stays declarative: `CONSISTENT_UNVERIFIED` is the strongest status, `VERIFIED` is never assigned (see `EXECUTOR_IDENTITY.md`), and neither a green CI run nor a merged PR proves authorship.

## 2. Provider capabilities and limitations (read from official docs on 2026-11-10; not re-checked in the 2026-11-13 refresh)

| Surface | Supported direction | MVP contract and hard limitation |
| --- | --- | --- |
| Codex CLI/SDK/app-server | Programmatic bounded coding runs and lifecycle control | Preferred local executor: scoped isolated worktree, non-interactive mode, captured process/job evidence. SDK/app-server later only if lifecycle use case requires it. |
| Cursor Cloud Agents API v1 | Programmatically launch/manage cloud agents | Optional second executor; API is public beta; per-run durable identifiers are not a source-code authorship attestation. v1 reference says API webhooks are coming soon (legacy v0 supports webhooks); use polling/callback only when validated against the exact installed version. |
| Cursor Automations | GitHub/CI/webhook triggers for cloud agents | Alternative native implementation trigger, not assumed to be an AgentsWatch-managed dispatch; avoid double triggering and implicit approvals. |
| Claude web GitHub integration | Select and sync repository files/branch for conversation/project context | Does NOT retrieve PRs, commit history or PR metadata. A precise PR review needs an independently exported patch + exact head SHA + CI data; web chat is human-assisted, not a controllable unattended worker via a documented generic web API. |
| ChatGPT web GitHub integration | Live read/search of authorized repos, including supported conversation/Work surfaces | Plain GitHub read connection is not an execution-control API. Eligible Work event-triggered tasks can react to supported PR activity; access, trigger and result export must be proved end-to-end before calling it unattended. |
| Claude Code Review service | PR-triggered automatic review for eligible Team/Enterprise organizations (separate billing) | A different product from Claude web chat; not assumed accessible or selected. Draft PR restrictions apply to this native service. |

Official references:
- https://developers.openai.com/blog/codex-as-a-platform
- https://cursor.com/docs/cloud-agent/automations
- https://prod.cursor.com/docs/cloud-agent/api/endpoints
- https://support.claude.com/en/articles/10167454-use-the-github-integration
- https://support.claude.com/en/articles/14233555-set-up-code-review-for-claude-code
- https://help.openai.com/en/articles/11145903-connecting-github-to-chatgpt
- https://help.openai.com/en/articles/10291617-scheduled-tasks-in-chatgpt

Do not automate desktop/browser clicking, scrape sessions/cookies, bypass human prompts, invent vendor callbacks or rely on unpublished APIs. Distinguish IDE desktop UI, provider CLI/SDK, provider cloud agent, and web chat: they are different integration surfaces.

## 3. Exact desired user journey

Phase A (usable with no new hosted service):
1. Owner selects an existing eligible issue/task. Gate/lease/branch/PR ownership is checked; HOLD remains HOLD.
2. AgentsWatch exports a verification RunContract and a bounded implementation Context Pack. Owner manually launches Codex or Cursor using a dedicated clean workspace/branch.
3. Run start is captured BEFORE edits; source/ref and worktree state are recorded. After implementation, agent prepares a draft PR and evidence. Do not declare tests passed merely from prose.
4. AgentsWatch exports a reviewer packet for the current exact PR head: issue intent, base/head SHAs, changed-file list, patch or safe repository links, relevant code context, actual CI run URLs/results, required acceptance, safety restrictions, receipt with caveats. Prevent secrets in the packet.
5. Owner opens Claude web or ChatGPT web, provides the packet, optionally uses their authorized GitHub connections, explicitly asks for a read-only advisory AI review (not a human review), and saves the response into a structured ReviewResult via manual import. Do not pretend this is unattended.
6. An agent review is advisory: Findings with file/line and evidence; uncertain claims stay Unknown. Only after triage are substantiated findings sent back to Codex/Cursor for a new implementation attempt.
7. A new head SHA invalidates every prior head-bound review/check for readiness; repeat CI, export and review. Final output is ReadyForOwnerReview or a conservative blocked state, never an automatic merge.

Phase B (conditional and opt-in):
- Codex CLI runner and Cursor Cloud Agents adapter can start, poll, retry/cancel only capabilities empirically supported in the selected runtime/API.
- Eligible ChatGPT Work GitHub-event tasks may serve as an event-based review *request* path, but require a proven structured result round-trip (for example, explicit owner-approved GitHub comment or dedicated intake). A successful trigger alone is not a completed review.
- Claude web remains assisted/manual unless a documented official integration for its web review workflow and safe callback becomes available. Switching to Claude Code/Anthropic API review requires a separate user/provider authorization and is NOT silently substituting Claude web.
- Normal user chats and model identities are not shared across providers automatically; only intentionally exported and appropriately classified context crosses the boundary.

## 4. Proposed architecture (future/optional)

GitHub Issue/PR (source of truth for task and code)
  -> Repo Intake + policy/gate preflight (read-only first)
  -> optional coordination sidecar [TaskLease, RunAttempt, Approval, EventJournal, Budget]
  -> ContextPackBuilder
  -> ExecutorAdapter [Codex CLI first | Cursor Cloud API later]
  -> isolated checkout/worktree; AgentsWatch start BEFORE edits
  -> draft PR and exact-head GitHub CI
  -> AgentsWatch finish + RunReceipt with evidence trust level
  -> ReviewPacket exporter
  -> [Claude web manual | ChatGPT web manual | optional ChatGPT Work event task, only after its round-trip is proven]
  -> ReviewResult importer (head-bound, advisory, provenance-labeled)
  -> policy/owner triage -> bounded correction loop -> owner final GO.

Boundary:
- Core verifier owns deterministic contract/receipt/evidence decisions; it MUST NOT depend on provider/browser orchestration.
- Companion orchestration owns execution attempts, lease/approval state, event journal and retry policy.
- GitHub owns task and code truth; companion MUST NOT create a competing project task system.
- Human-assisted reviewer UI is not an externally callable agent adapter. Implement ReviewPacket export and ReviewResult import first.
- Minimal modules only after proof: AgentsWatch.ReviewPackets and an optional AgentsWatch.Orchestration companion using ports/adapters. Avoid five new projects, API server, queue broker, PostgreSQL, dashboard or cloud hosting for the first experiment.

### Proposed data contracts (new, not current runtime)

TaskLease: leaseId, repo, sourceTaskRef, permissionMode, ownerActor, ownerAttemptId, scopeFingerprint, fencingToken, acquiredAt, expiresAt, lastHeartbeat, status.

ExecutionAttempt: attemptId, contractId, repo, provider, providerRunRef?, claimedExecutor, observedExecutionEvidence?, baseSha, branchRef, worktreeRef?, startedAt, finishedAt?, receiptRef?, state, budgetActual?, errorReason?.

ReviewPacket: schemaVersion, taskRef, repo, prNumber, baseSha, headSha, reviewerRole, contractDigest, evidenceDigest?, CI URLs/status/headSha, patchHash, relevantContextRefs, scope, redactionStatus, generatedAt.

ReviewResult: schemaVersion, packetDigest, reviewerSurface [ClaudeWeb/ChatGPTWeb/ChatGPTWork/Other], reviewerSessionRef? [declared unless provider-authenticated], reviewedHeadSha, findings [path,line,severity,evidence,confidence], recommendation [advisory], importedBy, source, sourceTrust, importedAt.

Approval: type, exact action, scope, current SHA/PR, grantedBy, issuedAt, expiration, disposition; merely quoting a chat statement is not a token enabling elevated execution.

EventJournal: eventId/sourceDeliveryId, source, repo, task/attempt, eventType, headSha?, receivedAt, dedupeKey, redactedPayloadDigest. Webhook signature validation and idempotency are mandatory before automatic execution.

Keep ReviewResult and workflow state OUTSIDE canonical RunReceipt v1 unless a separately reviewed schema extension and migration is approved. Hashes are integrity checksums, not independent signatures or attestations.

## 5. Ownership, review independence, security

- A single exclusive writer per repository task/scope. A reviewer works read-only with a separate context, never edits the implementer's branch. Parallel reviewers are allowed on the same frozen head.
- On lease timeout: UNKNOWN_PENDING_RECONCILIATION, not immediate task takeover. Check process/provider run, remote branch, PR and open claim. New fencing token only after atomic reconciliation; no force push.
- Local sidecar initially uses GitHub read-only reconciliation and an atomic file/SQLite single-host lease *only if the pilot needs a second concurrent executor*. For multi-host distributed execution, do not pretend a local lease is sufficient: require central transactional authority as a separately approved design.
- Review result from Claude web is a declarative assistant response imported by the owner; ChatGPT web likewise. Different model/vendor is AI cross-provider review, NOT a second human approval, GitHub APPROVED, certified independent provenance or a verified semantic acceptance check.
- If Codex implements, ChatGPT web uses the same OpenAI vendor; label SAME_VENDOR_CROSS_SURFACE. If Cursor implements, reviewer independence depends on the actual Cursor model/provider — do not assert different vendor from the Cursor brand alone. Claude web can be CROSS_PROVIDER_REVIEW if actual implementer provider evidence supports it.
- Never expose secrets, raw credentials, private chat history or unredacted sensitive source in packets without explicit access permission; least-privilege integration and budget caps.
- Prompt injection from issues, PR bodies and review prose is untrusted data; it cannot authorize tools, edit policy, modify acceptance, or bypass approval.
- No merge/rebase onto main, release, deploy, production DB mutation, billing, destructive command or force push from the companion. Reviewers may recommend but cannot grant owner GO.
- A review must refer to exact headSha and packetDigest; unknown SHA -> stale/unusable. CI checks must match exact head or be explicitly pending/unknown.
- A vendor job ID proves a job existed only to the extent provider evidence is authenticated. It does not prove per-line code authorship.

## 6. Incremental delivery, activation gates and acceptance tests

P0 / RFC ONLY (this document): align strategy, source checks, boundaries, cost/security tradeoff and experimental success criteria. NO runtime work.

P1 / AW-ORCH-001 — Read-only ReviewerPacket export prototype:
- Reuse current CLI evidence; GitHub diff/CI may be manually attached to avoid auth/network expansion.
- Require exact PR head SHA and secret-safe packet; output packet JSON/Markdown as derived, not receipt truth.
- Tests: missing/stale SHA, changed PR after packet, truncated diff, sensitive path, secret-redaction, unknown CI, no new source-of-truth records.
- Gate: separate owner approval for isolated experiment; do not register this as next canonical runtime task.

P2 / AW-ORCH-002 — Manual Claude/ChatGPT web review import:
- Strict schema/validation, advisory findings, quoted source as declared not attested, reviewer surface/provenance, stale-review invalidation.
- Tests: forged identity, unsupported finding, duplicate packet, wrong SHA, review importing a self-authorized merge request, prompt injection, same-vendor reviewer label.
- Gate: owner confirms the web workflow is useful on real authorized tasks.

P3 / AW-ORCH-003 — Codex supervised implementation adapter:
- Separate sandbox/worktree, start-before-write, bounded processes, actual exit codes, explicit permissions, cancellation/timeout reconciliation, draft PR only by owner's task authorization.
- Tests: concurrent writer refusal, crashed process, offline state, no validation => not Done, wrong branch/dirty start, duplicate event, no production permission.
- Gate: proof P1/P2 are decision-changing at acceptable overhead, independent human review of the implementation; no automatic task intake/loop initially.

P4 / AW-ORCH-004 — Cursor Cloud API supervised adapter:
- Validate API v1/version, auth and permitted actions, exact branch/PR correlation, managed cost and token exposure, polling and backoff where webhooks unsupported.
- Tests: delayed status, duplicate updates, changed head, API failure/rate limiting, unknown provider identity, budget exhaustion, no takeover.
- Gate: owner-authorized integration and successful P3 first.

P5 / AW-ORCH-005 — Conditional event/review handoff:
- Optional ChatGPT Work event-triggered review task ONLY if eligibility, allowed actions and structured completion callback are demonstrated in a small opt-in test. Claude web stays manual unless capabilities change.
- At most two automatic correction attempts, every fix invalidates CI+review on prior SHA; then NeedsReview/owner.
- Gate: proven value vs ordinary Git+CI+PR and acceptance of added costs; otherwise STOP/NARROW and keep manual packets.

P6 / Optional dashboard, centralized multi-host leases, hosted integration or commercial packaging:
- Explicitly blocked by AW-VFY-011/012 external-value/commercial gates and by separate architectural/security approvals. Not included in this RFC's implementation authority.

Success measures for an authorized experiment:
- zero observed material false attribution, false Done, unauthorized side effects and secret disclosure;
- 100% review results bound to the exact PR head, stale reviews rejected;
- no two live writers holding the same task/scope lease in enforced environments;
- actual review decision changes versus a sealed baseline Git/CI/PR judgment, with measured reviewer overhead and false alarms;
- cost and time measured from real runs, not estimated savings.
Use existing AW-VFY-010 usefulness protocol for comparability without silently altering its counters/GO criteria. The existing protocol's independent-human adjudication remains required to claim business value.

## 7. Trade-offs, stop rules and next owner action

Lowest-risk usable pilot: a manual Codex/Cursor coding run + generated exact-head reviewer packet + Claude/ChatGPT web read-only review + manually imported verdict. This meets the requested tool split NOW without pretending web chats are unattended agents.

Do NOT build all adapters simultaneously. If the manual handoff saves no work or creates misleading confidence, stop rather than adding workflow machinery.

Any implementation must pass current repository governance: explicit separate owner GO, scoped new branch/PR, no edit to active queue/hold ledger, independent human review and exact-head CI before any merge. The trust-boundary corrective work named in section 1 (#21, #24) has merged, but that approves nothing here: P1 would still need its own explicit owner GO while AW-VFY-010 is ADJUST / PAUSE.
