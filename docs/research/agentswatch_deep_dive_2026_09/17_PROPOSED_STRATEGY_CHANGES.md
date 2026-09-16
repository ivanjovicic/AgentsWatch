# Proposed Strategy Changes — Analysis Only

These recommendations are **not automatically applied to the canonical roadmap/product spec** by this research commit.

## P1 — Narrow category wording

Prefer:

> `vendor-neutral run evidence and completion verification for coding agents`

Reason: Qodo already owns much of the “independent AI code review” message; GitHub/Cursor own growing traceability surfaces.

## P2 — Treat code review as an evidence input

Do not rebuild Qodo/CodeRabbit/Sonar/Semgrep/Snyk review engines.

Future receipt may reference external review/security findings as evidence.

## P3 — Make RunContract an import/normalization layer

Default should be:

`GitHub/Jira/Linear/prompt -> import -> normalize -> ask only for missing verification fields`

Avoid duplicate task management.

## P4 — Slim canonical RunReceipt

Keep evidence/provenance/findings/decision/override in canonical JSON.

Move workflow advice such as `learningNote` and `nextPrompt` to a generated handoff/projection unless later evidence shows they belong in the audit object.

## P5 — Tighten attribution terminology

Use `run-interval repository delta` for what start/end Git evidence proves.

Reserve `agent-authored` for cases with stronger executor instrumentation.

## P6 — Explore attestation-like interoperability later

After MVP/external proof, evaluate in-toto-style custom predicates, signing/content digests and CI/GitHub Check association.

Do not claim SLSA compliance without satisfying the actual standard.

## P7 — Default commercial hypothesis: open core

Open deterministic schemas/rules/local verifier for trust and distribution.

Monetize organization policy, managed evidence, central retention/search, cross-repo controls, RBAC/SSO/compliance and support.

## P8 — Keep existing external/commercial gates

The current AW-VFY-011 and AW-VFY-012 direction is correct.

Do not expand into dashboard/SaaS/team admin until decision-changing external value and concrete buyer intent exist.
