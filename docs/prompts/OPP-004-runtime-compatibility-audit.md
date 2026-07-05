# OPP-004 — Runtime compatibility audit

Repository: `ivanjovicic/AgentsWatch`  
Queue: `community_opportunity_validation.md`  
Run mode: investigation and contract review only  
Token budget: medium-high  
Capability: AW-CAP-037

## Goal

Audit one coding-agent setup and determine the honest AgentsWatch support mode for AW-CAP-028 through AW-CAP-036.

## Required inputs

- tool/product and version;
- surface: chat, CLI, IDE, desktop, cloud, CI, SDK;
- configured and resolved model(s) where available;
- model roles: planner/editor/reviewer/coordinator/worker;
- hook/event/log/export documentation;
- effective permissions;
- environment and VCS/delivery workflow;
- network and credential classes;
- process ownership and checkpoint support.

Use official documentation and sanitized fixtures only. Do not inspect private user logs or credential values.

## Work

1. Build a draft `EffectiveRuntimeProfile`.
2. Separate static declarations from dynamic capabilities requiring handshake.
3. Map configured versus effective permissions.
4. Identify observation level O0-O6.
5. Classify environment and repository/delivery profile.
6. For each AW-CAP-028 through AW-CAP-036 choose:
   - Full;
   - Guarded;
   - Advisory;
   - PostHoc;
   - Manual;
   - Unavailable.
7. List blind spots and confidence cap.
8. Define the safest fallback.
9. Map applicable `COMP-*` scenarios.
10. Create discoveries for undocumented, unstable, or conflicting capabilities.

## Hard rules

- Never infer support from model/provider name alone.
- Never treat a configured hook as healthy without evidence.
- Never treat missing events as proof of no action.
- Never call textual instructions enforcement.
- Never convert quota percentage into tokens or money without documented mapping.
- Never claim local process control over an opaque cloud task.
- Never use local-worktree semantics for a branch/PR cloud flow.
- Never request or persist raw secrets.

## Output

```text
Tool/surface/version
Models and roles
Observation capabilities
Configured/effective permissions
Environment/VCS profile
Adapter stability
Per-capability support matrix
Blind spots
Fallbacks
Required fixtures
Decision: Supported for prototype | Research more | Park adapter
```

## Validation

- every capability has exactly one support mode;
- every Full/Guarded decision cites a capability source;
- every non-Full decision has fallback or explicit Unavailable reason;
- managed policy and environment constraints are included;
- no runtime capability maturity is raised by this docs-only audit.
