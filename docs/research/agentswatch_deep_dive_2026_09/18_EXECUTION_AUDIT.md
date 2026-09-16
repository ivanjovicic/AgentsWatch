# Execution Audit — Did the Deep-Dive Prompt Run Correctly?

Audit date: **2026-09-16**

## Short answer

The first generated deep-dive was **directionally useful but not fully compliant with the prompt**. This repository version corrects the material gaps.

## What was already good

- current `main` and latest CI were actually checked;
- Gate 0 failure/root cause was confirmed from source and GitHub Actions;
- 24 adversarial scenarios were modeled;
- dirty-worktree attribution, claims/evidence, RunContract/RunReceipt, ICP/WTP, distribution, unit economics, moat/copy risk, dogfood and kill gates were covered;
- the main business conclusion — narrow the wedge instead of expanding scope — remains supported.

## What was incomplete or too confident

### 1. Capability matrix evidence quality

The original matrix contained too many `YES/PARTIAL` cells without enough explicit primary evidence per competitor. That creates false precision.

Correction:
- the repo CSV now uses a more conservative matrix;
- unsupported cells are `UNKNOWN`, not guessed;
- competitor rows include evidence/source IDs.

### 2. Competitor breadth

Claude Code hooks/permissions, CodeRabbit local review, Sourcegraph Agentic Batch Changes, Devin audit/review and other adjacent surfaces were not researched deeply enough in the first pass.

Correction: current official sources are now included and reflected in the interpretation.

### 3. Vendor-neutrality evidence

The first pass treated multi-agent usage mostly as a thesis.

Correction: Snyk’s 2026 study reports 43% of nearly 10,000 observed developer environments using two or more AI coding environments. This supports multi-tool behavior, but **does not prove willingness to pay** for normalization.

### 4. Standards/provenance

The first pass mentioned SLSA but did not sufficiently distinguish:
- SLSA source/build provenance;
- generic in-toto attestations;
- Sigstore/Cosign authentication;
- OpenTelemetry agent telemetry;
- MCP tool interoperability.

Correction: `10_SECURITY_COMPLIANCE_AND_ATTESTATION.md` now treats these separately and explicitly states that AgentsWatch is not SLSA-compliant merely by producing a receipt.

### 5. Final 30 questions / decision tree

The original report answered many implicitly but not all explicitly.

Correction: `00_EXECUTIVE_DECISION.md` now contains the requested A–D decision tree and explicit answers to all 30 final questions.

### 6. Score precision

The prior portfolio score was 8.14/10. A competitor-level review justifies a lower current score because Qodo/Claude/GitHub/CodeRabbit/Sourcegraph reduce the amount of true whitespace.

Corrected analytical score: **7.80/10**.

This remains an analytical score, not a probability.

## Prompt coverage status

| Area | Status after correction |
|---|---|
| repository truth + CI | PASS |
| five kill questions | PASS |
| competitor research | PASS with explicit evidence limitations |
| capability matrix | PASS, conservative/UNKNOWN where unverified |
| 20+ scenario test | PASS (24 scenarios) |
| unique/useful/commodity/remove classification | PASS |
| dirty-worktree deep dive | PASS |
| claims/evidence deep dive | PASS |
| RunContract / RunReceipt | PASS |
| independent verifier / vendor neutrality | PASS |
| buyer / WTP / distribution / OSS | PASS |
| integration/copy tests | PASS |
| standards / provenance | PASS |
| security/compliance | PASS |
| reviewer economics / false-positive economics | PASS |
| trust model / product minimalism | PASS |
| technical feasibility / current CI | PASS |
| dogfood / external / commercial validation | PASS |
| revenue / unit economics / market size | PASS |
| bootstrap / VC | PASS |
| 2029 pre-mortem / kill criteria | PASS |
| updated score / improvement levers | PASS |
| explicit final questions / verdict | PASS |

## Accuracy conclusion

The corrected result is **fit to use as a research/decision artifact**, with three caveats kept explicit:

1. vendor docs prove advertised capabilities, not independent product quality;
2. `UNKNOWN` is intentionally used when a capability could not be verified;
3. market size, pricing and willingness-to-pay remain hypotheses until AW-VFY-011/012 produce real evidence.

## Canonical recommendation

Do not rewrite the product around this research automatically. Use it to sharpen validation:

`Gate 0 -> minimal evidence spine -> adversarial 30-run dogfood -> external decision-change gate -> commercial gate`

If the external gate does not demonstrate incremental value beyond Git/CI/PR/native tools/Qodo, narrow or stop rather than adding features.
