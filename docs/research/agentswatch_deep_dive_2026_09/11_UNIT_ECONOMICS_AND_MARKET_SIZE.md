# Unit Economics and Market Size

Full numeric scenarios: `data/unit_economics.csv`.

## Reviewer economics — illustrative only

Example team:
- 10 developers;
- 20 meaningful agent runs/week;
- 10 reviewer minutes saved/run;
- €70 loaded reviewer-hour cost.

Monthly value of time saved is roughly **€1,010**.

At 50 runs/week and 8 minutes saved/run, the same model yields roughly **€2,021/month**.

Therefore, before counting risk/audit value:
- €100/month requires about 1.4 reviewer-hours saved;
- €500/month requires about 7.1 hours;
- €2,000/month requires about 28.6 hours.

These are scenario estimates, not observed savings.

## Revenue paths — hypotheses

### ~€1k MRR
10 teams × €99 ≈ €990.

### ~€5k MRR
34 teams × €149 ≈ €5,066.

### ~€10k MRR
50 teams × €199 ≈ €9,950.

### ~€50k MRR
A mature illustrative mix such as 100 teams × €299 + 10 enterprise customers × €2,000 ≈ €49,900.

The €50k scenario is low-probability early and requires materially stronger enterprise packaging and distribution.

## Bottom-up market-size logic

Do not use 180M GitHub developers as TAM.

Funnel instead:

professional developers
→ frequent coding-agent users
→ teams with meaningful review/governance pain
→ teams willing to install independent tooling
→ teams with budget
→ teams not satisfied by native/Qodo/review alternatives.

There is no reliable public count for the final subset.

A **5,000–50,000 high-fit team** range can be used only as a low-confidence scenario model, not as measured TAM.

At €100–250 MRR that theoretical range corresponds to roughly €6M–€150M ARR, but reachability depends overwhelmingly on differentiation and distribution.

## Bootstrap path

Strong if demand exists:
- local-first infrastructure keeps COGS low;
- founder can build MVP;
- first hiring trigger should be driven by support/sales/integration load, not backend capacity.

## VC path

Potentially credible only if AgentsWatch becomes a broadly adopted cross-vendor verification/policy/attestation layer with significant enterprise ARPA and integration/standard effects.

A useful local CLI alone is not a venture-scale thesis.
