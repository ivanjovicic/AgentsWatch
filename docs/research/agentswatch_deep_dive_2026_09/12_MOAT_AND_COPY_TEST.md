# Moat and Competitive Copy Test

## Current moat

**None proven.**

## Potential moat

1. an adopted cross-vendor RunContract/RunReceipt schema;
2. unusually trustworthy run-boundary/dirty-state semantics;
3. organization verification policies tied to evidence;
4. a verified-run corpus and known error patterns;
5. integrations that make the evidence model part of team workflow;
6. execution-independent verifier reputation;
7. future signed/attestation-like interoperability.

## What is not moat

- Git diff;
- generic AI code review;
- session logs;
- AI-code percentage;
- token usage;
- generic policies;
- dashboard UI.

## Copy scenarios

### Future A — Cursor ships a full run receipt
AgentsWatch survives only if teams need one receipt/policy across Cursor and non-Cursor runs, including local repository evidence Cursor does not own.

### Future B — GitHub Copilot ships task contract + validation trace
This is the biggest platform threat because GitHub owns repository, PR and CI identity. AgentsWatch survives only as local/cross-vendor evidence tooling or as an implementation of a broader standard.

### Future C — Claude Code exposes structured run evidence
Claude already has strong hooks/permissions. AgentsWatch must remain valuable as normalization/independent evidence across tools.

### Future D — Qodo verifies agent work across tools
This is the hardest direct competitor case. AgentsWatch must stay distinct from generic review by focusing on:
- explicit run boundary/provenance;
- required-evidence sufficiency;
- deterministic completion claims;
- portable compact receipt;
- ambiguity/override semantics.

If Qodo adds these well and users prefer it, moat is weak.

### Future E — GitHub creates a vendor-neutral agent provenance standard
AgentsWatch should integrate with or implement that standard rather than invent a competing closed format.

## Structural platform risk

The project depends on a market whose largest platforms can add adjacent features rapidly. That is why time-to-proof matters more than feature breadth.

## Kill rule

If the normal combination of Git + CI + PR review + native agent evidence + Qodo/current review tooling solves the high-value scenario set with equal or lower friction, do not rescue the thesis by adding unrelated features.
