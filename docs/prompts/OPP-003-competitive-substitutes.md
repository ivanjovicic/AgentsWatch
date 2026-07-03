# OPP-003 — Competitive substitute and differentiation review

Repository: `ivanjovicic/AgentsWatch`  
Queue: `community_opportunity_validation.md`  
Run mode: research-only  
Token budget: medium  
Capabilities: AW-CAP-028 through AW-CAP-036

## Goal

Identify whether users already solve each proposed problem with vendor-native features, open-source tools, scripts, CI checks, or manual conventions, and decide where AgentsWatch has a credible vendor-neutral advantage.

## Research categories

- agent session observability and replay;
- context memory and handoff;
- token, quota, and cost tracking;
- loop detection and stop policies;
- repository rule generation;
- command/path/network policy;
- multi-agent worktree coordination;
- AI pull-request review and contribution gating;
- model/client regression testing.

## For each substitute record

```text
Product/tool/workaround
Problem solved
Target user
Local or cloud
Provider-specific or neutral
Required data access
Strengths
Weaknesses
Adoption friction
Pricing where publicly available
Evidence/source date
What remains unsolved
```

## Decision test

For every AgentsWatch opportunity answer:

1. Is the problem already solved well enough?
2. Is cross-provider portability valuable or merely theoretical?
3. Can AgentsWatch remain simpler and local-first?
4. Does AgentsWatch possess a proof/evidence advantage?
5. Is the opportunity a core product, a profile, an integration, or a rejected idea?
6. What would make a user switch or add AgentsWatch?

## Rules

- Use current official product documentation and primary repositories where possible.
- Do not rank by feature count alone.
- Separate free adoption value from paid differentiation.
- Recommend rejection when a substitute fully solves the problem.
- Do not claim demand or willingness to pay without interview evidence.

## Output

Substitute matrix, differentiated wedge per advancing opportunity, crowded-market warnings, build/buy/integrate decision, rejected ideas, updated kill conditions, and next prompt.
