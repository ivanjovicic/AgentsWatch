# AgentsWatch Positioning and Pricing Hypotheses

Last aligned: 2026-07-05  
Status: hypotheses, not validated

## Positioning decision

Initial public positioning should be concrete and evidence-first.

Preferred first message:

```text
Know what your coding agent changed, executed, tested, and missed.
```

Alternative:

```text
AgentsWatch verifies the work, not just the code.
```

Broader product direction:

```text
Spend fewer tokens. Merge safer AI code.
```

Internal long-term category hypothesis:

```text
A local control and evidence plane for coding agents.
```

Do not lead publicly with `control plane`, `firewall`, universal compatibility, or numerical savings before users and runtime proof validate those claims.

## Category boundary

AgentsWatch is not initially:

- another coding agent;
- a foundation model;
- a generic AI bug-review bot;
- a replacement for GitHub, CI, static analysis, or human review;
- a guaranteed security boundary.

AgentsWatch should initially be described as:

```text
A local-first evidence layer for AI coding-agent work.
```

## First product hypothesis

```text
AgentsWatch PR Evidence + Trust Ledger
```

It should answer:

- what task/scope was declared;
- what files actually changed;
- what commands and tests were observed;
- whether validation evidence matches the current commit;
- which claims are supported, stale, contradicted, missing, or not observable;
- what a reviewer should inspect next.

The differentiator is independent git/command/artifact/CI evidence, not a second model's opinion about the final diff.

## Primary value hypotheses

AgentsWatch is designed to help developers and teams:

- verify agent completion claims;
- bind build/test/CI evidence to a specific commit;
- detect changes outside declared scope;
- reduce time spent reconstructing what the agent actually did;
- resume work after context loss or provider switching;
- keep agent rules synchronized without hiding target limitations;
- identify repeated work and no-progress loops;
- understand runtime capability and permission blind spots;
- preserve and route out-of-scope discoveries;
- prove which product capabilities are actually implemented and verified.

Each item must use the maturity in `FEATURE_CAPABILITY_REGISTRY.md`. Planned/spec-only items are not advertised as shipped.

## Competitive differentiation hypothesis

| Existing AI review category | AgentsWatch hypothesis |
|---|---|
| Reviews code with another model | Verifies task/run evidence independently |
| Produces bug/style findings | Classifies claims by evidence status |
| Suggests tests | Shows which tests actually ran and for which commit |
| Focuses on final PR diff | Includes scope, commands, context, CI, and missing proof |
| Often tied to one workflow | Uses compatibility profiles and explicit fallback |

External code reviewers, static analyzers, and security scanners may later feed evidence into AgentsWatch. They are not the initial product identity.

## Free local CLI hypothesis

Possible free/open features:

- init/status;
- basic local PR/run evidence report;
- Rules Compiler and target-loss/drift report;
- basic Context Snapshot/export;
- prompt optimizer and task splitter;
- markdown run reports;
- handoff summaries;
- diff-only review prompts;
- basic risk scoring;
- local capability/compatibility reports;
- workspace doctor.

Goal:

- adoption;
- local trust;
- repeat use;
- public examples;
- low-friction evidence collection.

Availability is governed by capability maturity, not this list.

## Solo Pro hypothesis

Possible paid local/Pro features later:

- Trust Ledger history and comparisons;
- advanced Context Resume;
- offline Loop/Waste Analyzer;
- local dashboard after enough history exists;
- cross-repo history;
- configurable policy/proof packs;
- exportable evidence reports;
- supported premium adapters where justified;
- regression canaries later.

Pricing hypothesis only:

```text
Solo Pro: $8-15/month or $79-149/year
```

Annual pricing may be preferable for a local tool with low monthly interaction and payment-processing overhead.

Do not validate pricing before CLI dogfood, proof maturity, repeat-use evidence, and a concrete caught issue exist.

## Team hypothesis

Possible later team features:

- private-repository GitHub Action/PR Evidence automation;
- shared policy/rules packs;
- commit-bound PR evidence checks;
- organization compatibility reports;
- team audit/history;
- multi-agent workspace diagnostics;
- signed/tamper-evident evidence later;
- optional GitHub App only after Action/manual proof.

Pricing hypotheses:

```text
Founding pilot: $49-149/month for a small team
Team later: $10-25/active developer/month
Possible organization minimum: $99-149/month
```

The first paid offer should sell a concrete result and onboarding, not a broad platform promise.

## Enterprise hypothesis

Possible later value:

- SSO/RBAC;
- longer audit retention;
- managed organization policies;
- hybrid/self-hosted metadata service if validated;
- custom adapters;
- support/SLA;
- independently reviewed enforcement where claimed.

Do not build enterprise infrastructure before paid team demand and a customer requirement exist.

## First market validation experiment

Use 30 real AI-assisted PRs.

Measure:

- percentage of reports that change a reviewer action;
- missing or stale evidence discovered;
- review time saved or added;
- false-positive rate;
- repeat use;
- requests for automation;
- paid pilot acceptance.

Directional advance criteria:

```text
>= 30% of reports change a real review action
>= 50% of test users repeat or request repeat use
>= 3 teams request CI/GitHub automation
>= 2 teams accept a paid or explicitly budgeted pilot
```

These are internal product thresholds, not industry benchmarks.

## Buying roles hypothesis

### Solo developer

Buys:

- context recovery;
- evidence history;
- reduced repeated work;
- confidence before opening a PR.

### Tech lead/reviewer

Buys:

- faster evidence gathering;
- commit-bound validation;
- scope and completion-integrity checks;
- focused reviewer action list.

### Engineering manager

Buys:

- review debt visibility;
- policy consistency;
- risk/audit summaries;
- evidence that AI adoption is not merely increasing output volume.

### Platform/security team

Buys later:

- compatibility and effective-permission reports;
- shared policies;
- audit retention;
- declared enforcement classes;
- hybrid/self-hosted options.

## Do not claim yet

- exact token/time/cost savings without measured paired evidence;
- that AgentsWatch detects every agent action;
- that absence of an event proves an action did not happen;
- production readiness;
- support for all languages, tools, or environments;
- security certifications;
- stable PR/GitHub integration;
- SaaS availability;
- exact provider billing reconstruction;
- universal policy enforcement;
- superiority over AI code-review products;
- market size or willingness to pay;
- runtime support for commands listed only in plans/specs;
- cross-platform support beyond executed CI/acceptance environments.

## Evidence needed before selling

Minimum product trust evidence:

- Gate 0 restore/build/test/smoke proof;
- capability registry/traceability current;
- package checksum and isolated install;
- required safety/privacy scenarios;
- at least two repositories dogfooded;
- repeated use and at least one concrete risk/evidence issue caught;
- independent verification for the release candidate;
- known limitations visible.

Minimum market evidence before expanding infrastructure:

- real PR/problem examples;
- repeat use over more than one week;
- reports that change decisions or save measurable work;
- at least two budgeted/paid pilots before a Team Server;
- explicit customer request before enterprise/self-hosted work.

For percentage efficiency claims, use the stricter sample and measurement rules in `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`.

## Claim certification

Before updating website, pricing, README, release messaging, or Marketplace copy, run `PROOF-007-release-claim-certification.md` and record allowed wording.

Use `MARKET_PROBLEM_VALIDATION_SYNTHESIS_2026_07.md` for the current evidence and unresolved market questions.
