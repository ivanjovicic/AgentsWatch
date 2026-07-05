# OPP-001 — Coding-agent problem interview synthesis

Repository: `ivanjovicic/AgentsWatch`  
Queue: `community_opportunity_validation.md`  
Run mode: research-only  
Token budget: medium  
Capabilities: AW-CAP-028 through AW-CAP-036

## Goal

Synthesize anonymized interviews with coding-agent users and decide which community-derived opportunities should Advance, Revise, Park, or Reject.

## Inputs

- interview notes only;
- user role and workflow category;
- real problem examples;
- current workaround;
- frequency and consequence;
- willingness to try/switch/pay;
- privacy and integration constraints.

Do not read repository implementation files.

## Questions to answer

1. Which problem happened in the last 30 days?
2. What did it cost in time, money, failed work, or review burden?
3. Which current workaround is used?
4. Why is the workaround insufficient?
5. What local artifact or event would make a solution possible?
6. Would the user install a CLI, add hooks, wrap an agent command, or generate rule files?
7. Which output would change a real decision?
8. What false-positive/noise level is acceptable?
9. Which data must never leave the machine?
10. Is this free utility, Pro solo value, or team value?

## Analysis

For each capability opportunity, report:

```text
Users interviewed
Users recognizing the problem
Recent concrete examples
Pain severity
Current substitutes
Adoption friction
Willingness signal
Evidence availability
Privacy constraints
Proposed MVP
Success metric
Kill condition
Decision: Advance | Revise | Park | Reject
```

## Rules

- Do not convert one enthusiastic interview into a market-size claim.
- Separate user statements from analyst inference.
- Preserve negative evidence and rejected ideas.
- Do not propose runtime implementation in this prompt.
- Update opportunity ranking only when the interview evidence warrants it.

## Validation

- every advancing opportunity has at least five relevant users or is clearly labeled an early exploratory exception;
- every decision cites real examples;
- willingness-to-pay is not inferred from pain alone;
- interview data is anonymized;
- capability maturity remains L1.

## Output

Interview corpus summary, opportunity-by-opportunity decision, revised score, recommended first wedge, research gaps, and next focused prompt.
