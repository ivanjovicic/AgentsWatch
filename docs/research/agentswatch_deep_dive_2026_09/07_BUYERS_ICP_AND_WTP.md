# Buyers, ICP and Willingness to Pay

Full segment table: `data/buyer_wtp_matrix.csv`.

## Initial ICP

**AI-heavy engineering teams of roughly 5–30 developers.**

Prefer teams that:
- use coding agents frequently on real repositories;
- still perform meaningful human PR/review;
- use or experiment with more than one agent ecosystem;
- have non-trivial local/pre-PR workflows;
- care about scope/tests/evidence;
- can install a CLI/GitHub check without enterprise procurement.

User: developer/reviewer.  
Likely first payer: engineering manager, technical founder or DevEx owner.

## Other segments

### Solo power user
High-frequency user and good OSS/dogfood participant, but low budget. Not the primary payer.

### 20–100 developer engineering team
Potentially stronger budget and policy need, but more integration/support expectations.

### Platform / DevEx team
Strong expansion ICP if AgentsWatch becomes a reusable verification policy across repos and agent vendors.

### Security/compliance / regulated enterprise
Potentially high ARPA, but too much procurement, SSO/RBAC, retention, deployment and security burden for initial PMF discovery.

## Adjacent pricing anchors

Current adjacent prices show that engineering teams already pay meaningful amounts for AI coding/review/governance tools:
- GitHub Copilot Business: $19/user/month; Enterprise: $39;
- Cursor Teams Standard: $40/user/month monthly; Premium: $120;
- CodeRabbit Essentials: $24/developer/month annual ($30 monthly); Team: $48 annual ($60 monthly);
- Graphite Starter: $20/user/month annual; Team: $40;
- LangSmith Plus: $39/seat/month;
- Snyk Team starts at $25/contributing developer/month.

These prices prove category spend, **not AgentsWatch willingness to pay**.

## Pricing hypotheses to test

Only after AW-VFY-011 demonstrates decision-changing value:
- free/open local CLI core;
- ~€79–199/month small-team pilot;
- ~€199–999/month larger team/policy package;
- enterprise custom later.

Do not ask only “would you pay?”. Ask for paid pilot, invoice-ready commitment, signed design-partner agreement with budget or a real procurement step.

## What measurable value could justify payment?

1. reviewer time saved;
2. fewer false-Done/rework cycles;
3. policy/validation enforcement before merge;
4. portable audit evidence across heterogeneous agents;
5. lower risk without retaining full agent chat.

If buyer interviews cannot connect AgentsWatch to one of these measurable outcomes, the WTP score should fall.
