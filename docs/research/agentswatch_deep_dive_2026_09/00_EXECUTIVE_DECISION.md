# Executive Decision — AgentsWatch Deep Dive

Research cut-off: **2026-09-16**  
Status: analysis only

## Verdict

**CONTINUE, BUT NARROW WEDGE**

The analysis supports continued investment, but not the broad claim that AgentsWatch is differentiated simply because it is an "independent verifier". Qodo now offers independent local review inside coding-agent workflows, Claude Code and GitHub expose strong hooks/policy/audit surfaces, Cursor exposes AI-code attribution, and Sourcegraph can orchestrate and verify large agentic changes.

The surviving thesis is narrower:

> **Vendor-neutral run evidence and completion verification:** normalize the task into verification requirements, capture a repository baseline, compute the run-interval delta without stealing pre-existing dirty work, require validation evidence, verify narrow completion claims/scope, and emit a portable RunReceipt.

## Five kill questions

1. **Do native/vendor tools already solve enough?** Partly. They increasingly solve logs, policy, code review, audit and attribution. No reviewed product clearly provides the complete cross-vendor `contract -> pre-run baseline -> interval delta -> evidence sufficiency -> completion decision -> portable receipt` loop. **Confidence: MEDIUM.**
2. **Does independent verification change engineering decisions?** Plausible, not proven. This must be demonstrated by AW-VFY-011. **Confidence: LOW-MEDIUM.**
3. **Is vendor neutrality valuable enough to pay for?** Multi-tool use is real: Snyk reports 43% of nearly 10,000 observed developer environments used two or more AI coding environments, but that does not prove willingness to pay for normalization. **Confidence: MEDIUM on behavior, LOW on payment.**
4. **Can attribution/evidence be reliable?** Repository interval attribution is feasible in many cases; causal authorship is not always provable when humans, formatters or other agents modify the same worktree. `Ambiguous` must remain a valid result. **Confidence: MEDIUM-HIGH.**
5. **Can distribution work without enterprise-first sales?** OSS CLI + GitHub Check is plausible but unproven. **Confidence: LOW-MEDIUM.**

## Strongest gap

The best candidate is **run-scoped evidence attribution plus completion gating across vendors**, especially for dirty worktrees, required validation that was not actually run, owned/avoid path drift, and compact evidence that survives without retaining full chat.

## What is not a moat

Generic AI code review, session history, AI-line percentage, token dashboards, generic governance, generic Git diff, test execution, or another agent runtime.

## Product corrections recommended by this analysis

- `RunContract` should import/normalize an existing issue/prompt, not become a second Jira.
- Prefer the term **run-interval repository delta** unless executor instrumentation truly proves authorship.
- Keep `RunReceipt` compact: evidence, findings, decision, provenance. Move learning/next-prompt advice to human projections.
- Treat Qodo/CodeRabbit/Sonar/Semgrep/Snyk findings as possible future evidence inputs, not engines AgentsWatch should rebuild.
- Explore an attestation-like export after MVP; do not claim SLSA compliance.

## First ICP

AI-heavy engineering teams of roughly **5–30 developers** that use coding agents frequently, still perform meaningful human review, and have heterogeneous agent/tool usage or non-trivial local/pre-PR workflows.

## Updated score

Analytical weighted score: **7.80/10**.  
Previous portfolio score: **8.14/10**.

The reduction is driven mainly by stronger direct competition and platform risk, not by a weaker founder fit or smaller market.

A score above ~8.4 would only be defensible after 90-day evidence such as: reliable 30-run dogfood, 10+ external evaluations, 3+ decision-changing findings, 3+ continued-use requests and 3+ concrete paid/design-partner commitments.

## Decision tree

- **A — strong independent run-verification gap:** continue as primary.
- **B — useful but feature-level gap:** narrow to OSS/dev-tool integration.
- **C — governance demand but wrong product:** pivot toward policy/attestation/compliance.
- **D — native/Qodo baseline is sufficient:** stop the commercial project.

## Final questions — explicit answers

1. Real problem? **Yes, but commercial severity is unproven.**
2. Who feels it most? **Agent-heavy teams/reviewers.**
3. Who pays first? **Engineering manager/founder/DevEx owner.**
4. Why not Git + CI? **They do not by themselves encode the run boundary, required evidence and portable completion decision.**
5. Why not GitHub? **Strong platform threat; AgentsWatch only wins on local/cross-vendor independent evidence.**
6. Why not Cursor? **Cursor tracks AI contribution well; that is not the same as evidence-backed run completion.**
7. Why not Claude Code? **Hooks/policies can implement many controls; AgentsWatch must add portability and independent repository evidence.**
8. Why not Codex? **Codex has boundaries/telemetry; AgentsWatch must remain executor-independent.**
9. Why not Qodo? **Qodo is the closest competitor; AgentsWatch must focus on run provenance/completion, not generic review.**
10. Unique? **Potentially run-boundary attribution + portable evidence gate. Not yet proven.**
11. Dirty worktree important enough? **Useful differentiator, not sufficient as a company alone.**
12. RunContract necessary? **Yes as minimal normalization, not duplicate PM metadata.**
13. RunReceipt necessary? **Yes if compact and decision/audit useful.**
14. Vendor neutrality valuable? **Likely for heterogeneous teams; payment unproven.**
15. Local-first selling point? **Yes for trust/privacy/low COGS; secondary to value.**
16. Independence meaningful? **Yes conceptually; must prove decision impact.**
17. Can core be OSS? **Recommended.**
18. What should be paid? **Org policies, central retention/search, managed checks, RBAC/SSO/compliance/support.**
19. Never build first? **Runtime, orchestration platform, token dashboard, generic reviewer, broad SaaS admin.**
20. Strongest distribution surface? **OSS CLI followed by GitHub Check/Action.**
21. First ICP? **5–30 developer AI-heavy teams.**
22. Pricing? **Test ~€79–199/team/month after value proof; enterprise later.**
23. €1k MRR path? **~10 teams near €99/month, as a hypothesis.**
24. Creates moat? **Adopted schema, trustworthy attribution, policies, integrations, verified-run corpus.**
25. Destroys moat? **Qodo/GitHub/native tools absorbing the exact run-evidence loop.**
26. Next week? **Close Gate 0; then smallest `contract/start/finish/receipt/evidence` vertical slice.**
27. Stop when? **Findings do not change decisions, false positives remain high, or buyers prefer native tools.**
28. Roadmap correct? **Broadly yes; simplify receipt/contract semantics and keep commercial gates.**
29. Remain portfolio #1? **Conditionally yes, as a time-boxed primary bet.**
30. Score? **7.80/10 today.**

# IF THIS WERE MY MONEY

Would I spend the next 6 months on AgentsWatch? **ONLY IF SPECIFIC GATES PASS.**

**Strongest gap:** run-scoped attribution + evidence-backed completion across vendors  
**First ICP:** AI-heavy 5–30 developer teams  
**Reason to pay:** reduce review uncertainty/time and preserve executor-independent evidence  
**Main competitor:** Qodo; GitHub is the strongest platform threat  
**Main moat candidate:** adopted cross-vendor receipt/policy schema + trustworthy attribution + integrations  
**Main kill risk:** native/Qodo tooling makes the receipt redundant before distribution exists  
**Highest-value experiment next week:** close Gate 0 and build the smallest end-to-end evidence slice  
**Score today:** 7.80/10  
**Maximum defensible score after 90-day validation:** ~8.4–8.6/10

**AGENTSWATCH DECISION: CONTINUE, BUT NARROW WEDGE**
