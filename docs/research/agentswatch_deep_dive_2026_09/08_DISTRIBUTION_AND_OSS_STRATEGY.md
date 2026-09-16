# Distribution and OSS Strategy

## Recommended sequence

### 1. Open-source local CLI
Best trust/adoption surface for deterministic verification. The verifier’s core rules should be inspectable.

### 2. GitHub Action / Check
Most plausible compounding team surface after receipt quality is trustworthy.

The check should expose only high-signal results such as:
- missing required evidence;
- scope drift;
- unsupported claim;
- ambiguity requiring review;
- receipt/evidence reference.

### 3. MCP
Useful integration/control surface, but not a primary acquisition channel by itself.

### 4. Thin agent adapters
Codex / Claude Code / Cursor metadata import should remain adapters around one canonical evidence model.

## De-prioritize initially
- VS Code/Cursor-specific extension;
- large integration marketplace;
- hosted dashboard;
- enterprise-first sales;
- agent runtime/orchestrator.

## OSS model

Recommended commercial hypothesis: **OPEN CORE**.

Open-source:
- receipt/contract schemas;
- local CLI;
- Git evidence primitives;
- deterministic verification rules;
- local receipt renderer/verifier;
- basic GitHub check/action when stable.

Potential paid layer:
- organization policy management;
- central evidence retention/search;
- managed checks;
- cross-repo controls;
- RBAC/SSO;
- compliance/audit exports;
- managed signatures/attestations;
- enterprise support.

## Why open the core?

Verification asks users to trust the verifier. Transparent deterministic rules increase inspectability and developer adoption.

## Cheapest path to milestones

### 100 real users
OSS CLI + credible public examples of decision-relevant catches.

### 10 design partners
Founder-led outreach to high-fit agent-heavy teams after strong dogfood evidence.

### 10 paying teams / ~€1k MRR
A flat pilot near €99/month is a reasonable experiment, not validated pricing.

## Distribution test

Distribution score can legitimately rise only if users:
- install without founder hand-holding;
- use the tool on a second repo;
- invite a teammate;
- request GitHub/CI integration;
- continue after the initial novelty period.
