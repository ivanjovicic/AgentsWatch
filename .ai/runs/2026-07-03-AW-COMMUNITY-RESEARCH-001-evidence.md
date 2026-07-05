# AW-COMMUNITY-RESEARCH-001 Evidence

Prompt ID: AW-COMMUNITY-RESEARCH-001  
Queue: `community_opportunity_validation.md`  
Agent/tool: ChatGPT with web research and GitHub connector  
Model: GPT-5.5 Thinking  
Run mode: research + product specification + documentation  
Token budget: high  
Date: 2026-07-03

## Goal

Identify underdeveloped AgentsWatch ideas that address repeated problems in AI coding-agent communities, then convert the strongest opportunities into truthful product, architecture, backlog, capability, and validation documentation.

## Sources inspected

### Research

- ContextBench;
- Overeager Coding Agents;
- Where Do AI Coding Agents Fail?;
- How Coding Agents Fail Their Users;
- An Endless Stream of AI Slop;
- Coding with “Enemy”;
- Stop Hand-Holding Your Coding Agent;
- Contextual Memory Virtualisation.

### Public community signals

Representative issues from:

- Anthropic Claude Code;
- OpenAI Codex;
- public reports referencing Reddit/Hacker News discussions;
- multi-agent, context, usage, permission, hooks, verification, and review workflows.

## Findings

Repeated problem clusters:

1. context loss and compaction damage;
2. unpredictable token/quota/cost consumption;
3. repeated no-progress actions and loops;
4. false completion and unverifiable claims;
5. scope expansion and overeager actions;
6. secret/path/network/command safety;
7. multi-agent worktree contamination;
8. fragmented cross-provider repository rules;
9. AI pull-request review debt;
10. missing lifecycle hooks and normalized observability;
11. model/client regressions;
12. environment and interface fragmentation.

## Opportunity ranking

1. Agent Flight Recorder and Trust Ledger;
2. Context and Memory Portability;
3. Cost and Loop Guard;
4. Agent Rules Compiler and Drift Detector;
5. Policy Firewall;
6. Multi-Agent Worktree Coordinator;
7. AI PR Review Debt Reducer;
8. Agent Regression Canary;
9. OSS AI Contribution Gatekeeper;
10. Workspace Health Monitor.

## Documentation added

- `docs/AI_CODING_AGENT_COMMUNITY_RESEARCH_2026_07.md`;
- `docs/POPULAR_FEATURE_OPPORTUNITY_MAP_2026_07.md`;
- `docs/COMMUNITY_OPPORTUNITY_EPICS_AND_ACCEPTANCE.md`;
- `docs/COMMUNITY_OPPORTUNITY_ARCHITECTURE_ADDENDUM.md`;
- `docs/COMMUNITY_OPPORTUNITY_BACKLOG.md`;
- `docs/prompt_queues/community_opportunity_validation.md`;
- `docs/prompts/OPP-001-user-interview-synthesis.md`;
- `docs/prompts/OPP-002-adapter-feasibility.md`;
- `docs/prompts/OPP-003-competitive-substitutes.md`;
- `.ai/discoveries/AW-DISC-MARKET-001.md`.

## Documentation updated

- `docs/PRODUCT_SPEC.md`;
- `docs/MVP_ROADMAP.md`;
- `docs/FEATURE_CAPABILITY_REGISTRY.md`;
- `docs/FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`;
- `docs/DOCS_INDEX.md`;
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`.

## Capability changes

Added as L1 Specified only:

- AW-CAP-028 Agent event flight recorder;
- AW-CAP-029 Claim-to-evidence trust ledger;
- AW-CAP-030 Portable context snapshot and session rescue;
- AW-CAP-031 Agent rules compiler and drift detector;
- AW-CAP-032 Cost and loop guard;
- AW-CAP-033 Policy firewall and safe execution broker;
- AW-CAP-034 Multi-agent worktree coordinator;
- AW-CAP-035 AI PR review debt reducer;
- AW-CAP-036 Agent/model regression canary.

No runtime implementation, test support, market validation, savings, popularity, or security effectiveness is claimed.

## Architecture decision

Use one shared normalized local agent-event journal and projections rather than separate pipelines for every opportunity.

Shared foundations:

- stable repository/workspace/worktree/task/run/agent/event identities;
- provider adapter capability declarations;
- append-only local event imports;
- redaction and path boundaries;
- git/process/validation evidence;
- rebuildable projections for timeline, trust, context, loops, policy, coordination, and review.

## Product strategy decision

Preserve the existing core MVP order. Run a separate opportunity incubator:

```text
interviews
-> adapter/source feasibility
-> competitive substitutes
-> one low-risk free wedge
-> shared event foundation
-> import-only/dry-run prototypes
-> dogfood
-> live/enforcing features only after evidence and review
```

Recommended initial wedges:

- free/open: Rules Compiler and Drift Detector;
- Pro candidate: Context Snapshot/Session Rescue or Flight Recorder/Trust Ledger.

## Validation performed

- cross-checked repeated themes across multiple primary studies and public repositories;
- separated problem evidence from market-demand claims;
- added capability and traceability rows at L1 only;
- added explicit kill criteria and non-goals;
- preserved local-first and no-hidden-telemetry requirements;
- routed follow-up work through AW-DISC-MARKET-001 and the community queue.

Runtime build/test was not required by the documentation-only changes. A pull-request comparison and repository CI status should still be reviewed before merge.

## Limitations

- no primary user interviews were conducted in this run;
- social-network access was represented partly through research analyzing Reddit/Hacker News and GitHub issues referencing those discussions, not a complete platform-wide sample;
- issue trackers over-represent failures and power users;
- competitor/substitute research remains a dedicated next prompt;
- provider adapter feasibility remains unverified;
- opportunity scores are directional.

## Discoveries

- `AW-DISC-MARKET-001`: strong opportunity signal, but user and adapter validation required.

## Next prompts

1. OPP-RSCH-001 — user interview synthesis;
2. OPP-RSCH-002 — adapter feasibility;
3. OPP-RSCH-003 — competitive substitute review;
4. re-score opportunities;
5. select one free wedge and one Pro prototype.

## Residual risk

The documentation may create enthusiasm for advanced capabilities. Registry, router, and proof rules explicitly prevent them from being described as implemented until code and evidence exist.
