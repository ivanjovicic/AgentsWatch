# Score Improvement Levers

Full matrix: `data/score_improvement_levers.csv`.

The score must improve through evidence, not through more documentation or features.

## Top five levers

### 1. Prove a high-severity gap against current baselines

Run the 20+ scenario comparison against Git + CI + PR review + native vendor evidence + Qodo/other relevant tooling.

Can legitimately raise:
- differentiation;
- competition score;
- moat potential.

### 2. 30-run adversarial dogfood

Prove:
- no material false attribution;
- ambiguity is handled safely;
- findings repeatedly expose real missing evidence/scope/claim issues;
- verification overhead is acceptable.

Can raise:
- technical-risk score;
- problem-severity confidence;
- retention confidence.

### 3. External decision-change study

At least 10 external real evaluations.

Strong signal:
- finding changes merge/rework/evidence decision;
- user asks to keep using the tool;
- user uses it on a second repository or with a teammate.

Can raise:
- differentiation;
- retention;
- market-size confidence;
- distribution confidence.

### 4. Concrete paid design-partner ask

At least three real commercial commitments.

Can raise:
- WTP;
- time-to-revenue;
- bootstrap score.

### 5. OSS/GitHub distribution proof

After verification quality is credible, demonstrate organic installation and second-repo/team adoption.

Can raise:
- distribution;
- moat;
- bootstrap feasibility.

## Maximum defensible near-term changes

- Competition: 3.5 → ~5 only if AgentsWatch clearly owns unsolved high-value scenarios rather than generic review.
- Differentiation: 6.5 → ~8.5 only with repeated external decision-changing findings.
- Distribution: 6.5 → ~8 only with organic installs/second-repo/team adoption.
- WTP: 7.5 → ~8.5 only with real paid commitments.
- Technical risk: 6 → ~8 only with trustworthy dogfood across dirty/ambiguous cases.
- Platform risk: 5 → ~7 only if the core remains useful across multiple vendors with thin adapters.
- Moat: 6 → ~8 only if the receipt/policy format becomes adopted and sticky.

## Highest information-value order

1. close Gate 0;
2. build minimal evidence spine;
3. adversarial 30-run dogfood;
4. external decision-change validation;
5. paid design-partner validation;
6. only then test OSS/GitHub distribution at scale.
