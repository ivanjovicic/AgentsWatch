# AgentsWatch Positioning and Pricing Hypotheses

Last aligned: 2026-07-03  
Status: hypotheses, not validated

## Positioning

AgentsWatch is a local-first AI coding-agent supervisor and context/token-waste optimizer.

Core promise:

```text
Spend fewer tokens. Merge safer AI code.
```

Evidence-safe category wording:

```text
AI coding-agent supervisor with local evidence, discovery, prompt-scope, and capability-proof workflows.
```

## Primary value hypotheses

AgentsWatch is designed to help developers:

- split broad prompts into safer runs;
- limit what agents inspect/edit;
- track actual changes and validation;
- catch claims-vs-actual mismatches;
- generate compact handoffs;
- preserve and route out-of-scope discoveries;
- prove which capabilities are actually implemented/verified.

Each item must use the maturity in `FEATURE_CAPABILITY_REGISTRY.md`. Planned/spec-only items are not advertised as shipped.

## Free local CLI hypothesis

Possible free/open features:

- init;
- status;
- prompt optimizer;
- task splitter;
- markdown run reports;
- handoff summaries;
- diff-only review prompts;
- basic risk scoring;
- local capability/evidence reports.

Goal: adoption and trust.

Availability is governed by capability maturity, not this list.

## Pro hypothesis

Possible paid local/pro features later:

- local dashboard;
- cross-repo history;
- advanced context/waste analytics;
- configurable policy/proof packs;
- exportable reports;
- saved prompt templates;
- advanced discovery/benchmark workflows.

Pricing hypothesis only:

```text
Solo Pro: $8-15/month or $79-149/year
```

Do not validate pricing before CLI dogfood, proof maturity, and repeat-use evidence exist.

## Team hypothesis

Possible later team features:

- GitHub PR integration;
- shared policy/proof rules;
- PR risk reports;
- CI annotations;
- team audit history.

Pricing hypothesis only:

```text
Team: $10-25/user/month
```

## Do not claim yet

- exact token/time/cost savings without measured paired evidence;
- production readiness;
- support for all languages;
- security certifications;
- stable PR integration;
- SaaS availability;
- runtime support for commands listed only in plans/specs;
- cross-platform support beyond executed CI/acceptance environments.

## Evidence needed before selling

Minimum trust evidence:

- Gate 0 restore/build/test/smoke proof;
- capability registry/traceability current;
- package checksum and isolated install;
- required safety/privacy scenarios;
- at least two repositories dogfooded;
- repeated use and at least one concrete risk/evidence issue caught;
- independent verification for the release candidate;
- known limitations visible.

For percentage efficiency claims, use the stricter sample and measurement rules in `BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`.

## Claim certification

Before updating website, pricing, README, or release messaging, run `PROOF-007-release-claim-certification.md` and record allowed wording.
