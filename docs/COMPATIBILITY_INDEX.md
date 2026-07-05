# AgentsWatch Compatibility Index

Last aligned: 2026-07-03  
Status: entry point for cross-model, cross-tool, permission, and environment support

## Start here

| Document | Purpose |
|---|---|
| `MODEL_TOOL_PERMISSION_ENVIRONMENT_COMPATIBILITY_2026_07.md` | Research conclusion, compatibility dimensions, feature matrix, and environment-specific adaptations. |
| `RUNTIME_CAPABILITY_NEGOTIATION_AND_FALLBACKS.md` | Effective runtime profile, support decision modes, capability provenance, and fallback contract. |
| `COMPATIBILITY_ACCEPTANCE_SCENARIOS.md` | Fifty reproducible positive, degraded, conflicting, and negative compatibility scenarios. |
| `ADAPTER_SPEC.md` | Composable runtime, surface, model, event, permission, environment, VCS/CI, stack, rules, and usage adapters. |
| `COMPATIBILITY_IMPLEMENTATION_BACKLOG.md` | Issue-ready implementation order for AW-CAP-037. |
| `prompts/OPP-004-runtime-compatibility-audit.md` | Audit one real tool/surface/model/permission/environment combination. |
| `prompt_queues/community_opportunity_validation.md` | Gate opportunities and live integrations on compatibility proof. |

## Core answer

```text
Planned concepts are broadly reusable.
Their observation, verification, and enforcement depth is not equal.
```

Every advanced capability must use:

```text
Model profile
+ tool profile
+ surface profile
+ observation profile
+ permission profile
+ environment profile
+ VCS/delivery profile
= EffectiveRuntimeProfile
```

Then select:

```text
Full | Guarded | Advisory | PostHoc | Manual | Unavailable
```

## Capability truth

`AW-CAP-037 Runtime capability negotiation and fallback planning` is L1 Specified only.

No current AgentsWatch runtime automatically:

- discovers every coding tool or model;
- resolves effective permissions;
- detects every local/cloud/remote environment;
- installs or verifies provider hooks;
- enforces provider policies;
- guarantees equal feature support.

## Required proof order

1. Runtime profile schema.
2. Deterministic support-decision engine.
3. Generic/manual adapter.
4. Environment, permission, and VCS detectors.
5. Linux/Windows fixture matrix.
6. First local rich-event adapter.
7. Second materially different local adapter.
8. One cloud/PR evidence adapter.
9. Cross-surface dogfood.
10. Independent compatibility verification.

## Rule

When compatibility documentation conflicts with a provider's current official contract, update the provider adapter declaration and downgrade support. Never preserve a higher support claim merely for product consistency.
