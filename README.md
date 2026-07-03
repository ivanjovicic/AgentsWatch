# AgentsWatch

AgentsWatch is a local-first AI coding-agent supervisor and context/token-waste optimizer.

It is designed to help developers run smaller, safer, more reviewable AI coding tasks by splitting broad prompts, limiting scope, tracking git evidence, preserving discoveries, learning from runs, and proving which capabilities actually work.

## Core promise

```text
Spend fewer tokens. Merge safer AI code.
```

Current evidence-safe positioning:

```text
Designed to reduce avoidable context, repeated work, scope creep, and evidence mistakes.
```

The earlier `30-50%` target is a product hypothesis, not a measured result. Numerical savings claims require the paired benchmark and quality guardrails in `docs/BENCHMARK_AND_DOGFOOD_METHODOLOGY.md`.

## Capability truth

AgentsWatch uses evidence maturity levels:

```text
L0 Idea
L1 Specified
L2 Implemented
L3 Test-backed
L4 CI-verified
L5 Dogfood-verified
L6 Release-verified
```

Authoritative status:

- `docs/FEATURE_CAPABILITY_REGISTRY.md`
- `docs/FEATURE_EVIDENCE_TRACEABILITY_MATRIX.md`
- commit-bound CI/proof artifacts

A roadmap or documentation page proves intent, not runtime support.

## Current implemented skeleton

Current source contains:

```bash
agentswatch init
agentswatch optimize <prompt-file-or-text>
agentswatch status
agentswatch --help
agentswatch --version
```

Prompt risk analysis, basic git-status parsing, and basic project-type/validation suggestions also have some unit-test source. The current commit still needs executed CI/acceptance evidence before these capabilities can be described as verified.

## Planned CLI

```bash
agentswatch task split <prompt-file>
agentswatch start <task-id>
agentswatch finish <task-id>
agentswatch report
agentswatch handoff
agentswatch review-diff <commit-or-range>
agentswatch validate
agentswatch run -- <command>
agentswatch mistakes list
agentswatch lint evidence
agentswatch discover reconcile <run-id|discovery-id>
agentswatch lint discoveries
agentswatch finish <task-id> --learn --reconcile
```

These commands are planned/specification items unless the capability registry says otherwise.

## Proof system

Every supported capability should have:

```text
claim
-> contract
-> acceptance criteria
-> implementation
-> targeted tests
-> black-box scenario
-> CI artifact
-> dogfood evidence when usefulness is claimed
-> release proof when support is claimed
```

See:

- `docs/PROOF_AND_VERIFICATION_STRATEGY.md`
- `docs/REPRODUCIBLE_ACCEPTANCE_SCENARIOS.md`
- `docs/PROOF_BUNDLE_SPEC.md`
- `docs/INDEPENDENT_VERIFICATION_RUNBOOK.md`
- `docs/prompt_queues/agentwatch_proof_and_verification.md`

CI is configured to produce Linux/Windows build-test-smoke artifacts and a package/checksum/isolated-install proof bundle. Actual maturity changes only after the workflow succeeds for the relevant commit.

## Discovery and self-improvement

AgentsWatch should not expand the current task to fix unrelated work, but it should not lose useful findings.

```text
Capture -> deduplicate -> classify -> route -> generate follow-up -> verify closure.
```

See:

- `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`
- `.ai/DISCOVERY_RECORD_TEMPLATE.md`
- `docs/prompt_queues/agentwatch_discovery_and_self_improvement.md`

The manual docs/evidence workflow exists now. Runtime discovery automation is still gated and planned.

## Run evidence and learning

Every meaningful agent run should leave:

- compact run evidence;
- validation result or blocked reason;
- one useful learning note;
- classified mistakes;
- reconciled out-of-scope findings;
- affected capability/proof updates;
- follow-up prompt and residual risk.

See:

- `docs/AGENT_RUN_LOGGING_AND_LEARNING.md`
- `docs/MISTAKE_LEARNING_SPEC.md`
- `.ai/RUN_LOG_TEMPLATE.md`
- `docs/ai/learning/MISTAKE_LEDGER.md`

## Supervised automation

AgentsWatch may sequence prompts, but MVP does not allow uncontrolled continuous autopilot.

Risky actions require explicit approval. No hidden UI automation, autonomous deploy/merge/release, or approval bypass.

See:

- `docs/SUPERVISED_AUTOPILOT_QUEUE.md`
- `docs/AGENT_RISK_BOUNDARIES.md`
- `docs/AGENT_PERMISSION_MODEL.md`

## Privacy

The local MVP must not upload repository source, prompts, diffs, run logs, discoveries, validation output, or proof artifacts containing private content by default.

See:

- `docs/SECURITY_AND_PRIVACY.md`
- `docs/PROOF_BUNDLE_SPEC.md`

## Bootstrap gate

The skeleton was initially created through GitHub file writes. Runtime feature work remains validation-first:

1. restore;
2. build;
3. tests;
4. CLI smoke;
5. proof artifact review;
6. only then continue gated runtime features.

See:

- `docs/BUILD_VALIDATION_PLAN.md`
- `docs/VALIDATION_EVIDENCE_2026_06_29.md`
- `docs/prompt_queues/bootstrap_validation.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`

## Repository layout

```text
src/
  AgentsWatch.Cli/
  AgentsWatch.Core/
  AgentsWatch.Git/
  AgentsWatch.LanguageAdapters/
  AgentsWatch.Reports/
tests/
  AgentsWatch.Tests/
docs/
.ai/
  runs/
  discoveries/
.github/workflows/
```

## Development principles

- Local-first CLI before dashboard/SaaS.
- Current code/tests/proof before planning claims.
- Universal git/markdown/file behavior before adapters.
- One primary run mode and bounded context per task.
- Investigation before uncertain implementation.
- Targeted validation before broad validation.
- Diff-only review after implementation.
- Compact evidence instead of long logs/chat history.
- Every meaningful discovery gets a disposition.
- Every advertised capability gets a registry row.
- No numerical efficiency claim without measured benchmark evidence.
- No release support claim without package, checksum, clean install, proof bundle, and independent verification.

## Documentation

Start with:

- `AGENTS.md`
- `docs/DOCS_INDEX.md`
- `docs/PRODUCT_SPEC.md`
- `docs/CLI_SPEC.md`
- `docs/PROOF_AND_VERIFICATION_STRATEGY.md`
- `docs/FEATURE_CAPABILITY_REGISTRY.md`
- `docs/DISCOVERY_AND_SELF_IMPROVEMENT_LOOP.md`
- `docs/TEST_STRATEGY.md`
- `docs/RELEASE_AND_PACKAGING_PLAN.md`
