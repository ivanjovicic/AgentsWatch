# Security, Compliance and Attestation

## Security/compliance as a later wedge

A plausible enterprise job is:

> Show exactly what an autonomous coding agent was allowed to change, what repository state changed inside its run, what validation executed, and why the result was considered reviewable/complete.

GitHub, Claude Code, OpenAI and other vendors increasingly expose hooks, permissions, audit or telemetry, which validates the need for controls but also increases competition.

## Do not start enterprise-first

Enterprise-first productization would prematurely require:
- SSO/RBAC;
- centralized retention;
- tenant isolation;
- deployment/security reviews;
- DPA/privacy controls;
- key management;
- SLA/support;
- possibly on-prem/private deployment.

First prove that the evidence artifact changes engineering decisions.

## Trust model

### Deterministic / directly evidenced
- repository identifiers and fingerprints;
- path policy checks;
- validation exit/status with provenance;
- schema validation;
- narrow claim classes;
- cryptographic/content digests where later added.

### Human-reviewable
- ambiguity;
- overrides;
- acceptance mappings;
- risk findings.

### AI-assisted only
- free-text claim extraction;
- semantic acceptance analysis;
- explanations;
- suggested follow-up.

LLM output must never silently upgrade weak evidence into fact.

## Standards/provenance assessment

### SLSA 1.2
SLSA defines approved source/build provenance and verification concepts. Its 1.2 Source Track is especially relevant as a conceptual reference for source-revision provenance and enforced controls. AgentsWatch should **not** claim SLSA compliance without satisfying SLSA’s actual model and requirements.

### in-toto Attestation Framework
in-toto provides a generic model for authenticated metadata about software artifacts and supports custom predicates. This is the strongest standards analogy for a future portable RunReceipt predicate or related evidence bundle.

### Sigstore / Cosign
Cosign can sign and verify in-toto attestations. It is a potential future mechanism for authenticating a receipt/evidence bundle, not an MVP dependency.

### OpenTelemetry GenAI conventions
OpenTelemetry is evolving semantic conventions for GenAI agents, workflows and tool execution. This is useful for telemetry interoperability, but telemetry spans are not a substitute for repository completion semantics.

### MCP
MCP is now a mature interoperability substrate for agent tools. AgentsWatch can expose verification operations over MCP later, but MCP itself does not define the verification truth model.

## Recommendation

After MVP/external proof, evaluate an **attestation-like, signed or content-addressed RunReceipt** and compatibility with existing standards instead of inventing a closed proprietary audit envelope.
