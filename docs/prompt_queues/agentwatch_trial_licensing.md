# AgentsWatch Trial Licensing Prompt Queue

Last aligned: 2026-07-02  
Target repo: `ivanjovicic/AgentsWatch`  
Lane: post-MVP commercial trial, entitlement, secure storage, and IP-protection architecture  
Parent docs: `../TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`, `../SECURITY_AND_PRIVACY.md`, `agentwatch_mvp.md`, `agentwatch_foundation_followups.md`

Purpose: design and later implement a fair, privacy-preserving AgentsWatch trial that prevents normal perpetual use and casual copying without pretending local files can be perfectly hidden.

## Gate

Do not start runtime licensing work until:

```text
AW-VAL-001
AW-VAL-002
AW-VAL-003
CLI MVP evidence
AW-DOGFOOD-001 evidence
commercial free-tier/trial decision
```

Docs/threat-model prompts may run earlier.

## Hard rules

- Local files needed by the client cannot be perfectly hidden from a determined local user.
- Do not encrypt, delete, corrupt, or lock user repository files or user-owned reports.
- Do not send source code, prompts, diffs, validation output, command logs, or run history during license checks.
- No hidden telemetry or hidden network calls.
- Do not use a shared signing secret embedded in the client.
- Do not rely only on local clock or a plaintext `trialEndsAt` file.
- Do not ship premium prompt/template libraries as editable plaintext files when avoidable.
- Do not scatter boolean license checks across commands; use a central entitlement evaluator.
- Expired users retain read/export access to their own existing artifacts.
- Runtime implementation remains post-MVP; design must not block current CLI validation and dogfood.

---

## Active prompts

| ID | Status | Purpose |
|---|---|---|
| AW-LIC-001 | Ready docs-only | Decide threat model, free tier, trial shape, offline policy, and expired behavior. |
| AW-LIC-002 | Ready after AW-LIC-001 | Define signed license lease and feature entitlement contracts. |
| AW-LIC-003 | Ready after AW-LIC-002 and CLI MVP evidence | Implement local license models, signature verification, and central entitlement evaluation. |
| AW-LIC-004 | Ready after AW-LIC-002 and CLI MVP evidence | Implement platform secure storage and installation identity abstraction. |
| AW-LIC-005 | Ready after AW-LIC-003/004 | Implement trial lifecycle, trusted clock, offline grace, usage counters, and license CLI UX. |
| AW-LIC-006 | Ready after AW-LIC-003 | Add feature-gated command behavior and expired/read-only mode. |
| AW-LIC-007 | Backlog after paid-beta decision | Design and implement minimal licensing service for activation, lease renewal, revocation, and device management. |
| AW-LIC-008 | Backlog after CLI packaging evidence | Remove plaintext premium assets and evaluate signed packaging, single-file, AOT, and obfuscation. |
| AW-LIC-009 | Ready after AW-LIC-003..007 implementation | Add comprehensive trial, tamper, offline, key-rotation, and device-copy tests. |
| AW-LIC-010 | Backlog after premium value is proven | Design optional server-side premium algorithms using metadata-only/privacy-preserving contracts. |
| AW-LIC-011 | Ready docs-only after AW-LIC-001 | Write user-facing privacy, activation, offline-grace, trial, and data-retention disclosures. |

---

## AW-LIC-001 — Commercial threat model and trial decision

Run mode: docs-only  
Token budget: medium

Task: make the commercial/product decisions required before any licensing code exists.

Read first:

- `docs/TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- `docs/PRODUCT_SPEC.md`
- `docs/SECURITY_AND_PRIVACY.md`
- `docs/FEATURE_SELECTION_SPEC.md`
- `docs/MVP_ROADMAP.md`

Owned paths:

- `docs/TRIAL_LICENSING_PRODUCT_DECISION.md`
- this queue status row only

Required decisions:

1. Permanent free tier, trial-only, or hybrid.
2. Trial length.
3. Usage/run limit.
4. Whether expiration is whichever comes first or later.
5. Offline lease duration and grace period.
6. Number of allowed active devices.
7. Exact features in Free, Trial, Pro, and future Team.
8. What remains accessible after expiration.
9. Which proprietary logic remains local versus optional server-side.
10. What licensing requests send and explicitly never send.

Required conclusion:

```text
Do not claim that local content is impossible to copy.
Define protection as commercial enforcement plus raised reverse-engineering cost.
```

Validation:

```bash
git diff --check
```

---

## AW-LIC-002 — Signed lease and entitlement contract

Run mode: docs/spec first  
Token budget: medium

Task: define a versioned signed license lease and feature entitlement contract.

Read first:

- `docs/TRIAL_LICENSING_AND_IP_PROTECTION_PLAN.md`
- output from `AW-LIC-001`
- `docs/FEATURE_SELECTION_SPEC.md`
- `docs/SECURITY_AND_PRIVACY.md`

Owned paths:

- `docs/LICENSE_ENTITLEMENT_CONTRACT.md`
- this queue status row only

Required contract fields:

- license id;
- subject/customer/trial id;
- edition;
- feature entitlements;
- issued/not-before/expiry UTC;
- trial end UTC;
- installation/device binding;
- offline allowance;
- lease schema version;
- signing key id;
- optional usage allowance/checkpoint;
- revocation/version requirements.

Required security decisions:

1. Asymmetric signature only.
2. Fixed allowlisted algorithm.
3. Issuer/audience validation.
4. Public-key rotation.
5. Canonical serialization rules.
6. Device mismatch behavior.
7. Expiry/offline-grace behavior.
8. Clock rollback behavior.
9. Unknown entitlement behavior.
10. Future schema version behavior.

Validation:

```bash
git diff --check
```

---

## AW-LIC-003 — Local license verification and entitlement evaluator

Run mode: implementation/test  
Token budget: medium/high

Gate: CLI MVP and accepted `AW-LIC-002` contract.

Task: implement local signed-license validation and one central feature entitlement evaluator.

Suggested interfaces:

```csharp
public interface ILicenseService;
public interface IEntitlementEvaluator;
public interface ILicenseTokenVerifier;
public interface ITrustedClock;
```

Owned paths:

- new license/entitlement code in `AgentsWatch.Core`
- targeted tests
- no licensing HTTP client yet unless explicitly needed for fixtures

Required behavior:

1. Verify signature with embedded public verification key only.
2. Validate issuer, audience, schema, key id, not-before, expiry, and device binding.
3. Resolve edition and entitlements centrally.
4. Reject local config attempts to grant premium entitlements.
5. Return structured states: valid, expired, offline-grace, invalid-signature, revoked/unknown, device-mismatch, unsupported-version.
6. Use fake clock and test key fixtures.
7. Never log full license token or customer secret.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter "License|Entitlement|Signature"
```

---

## AW-LIC-004 — Secure storage and installation identity

Run mode: implementation/test  
Token budget: medium/high

Gate: accepted license contract and CLI MVP evidence.

Task: implement platform abstractions for protected license state and installation identity.

Suggested interfaces:

```csharp
public interface ILicenseStorage;
public interface IInstallationIdentityProvider;
public interface IProtectedLocalState;
```

Required behavior:

1. Generate random installation id on first activation.
2. Store installation id and lease in OS-protected storage where available.
3. Windows implementation uses a supported protected-store mechanism.
4. macOS/Linux behavior is explicitly supported or has a documented secure fallback.
5. Storage loss requires reactivation; it must not silently create a new trial.
6. Do not use a static AES key embedded in the binary.
7. Do not collect invasive hardware inventory.
8. Provide diagnostic status without exposing token contents.

Required tests:

- save/read/delete;
- corrupted protected state;
- unavailable keyring;
- copied storage on another installation id;
- migration/version handling;
- permission-denied storage behavior.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter "LicenseStorage|InstallationIdentity|ProtectedState"
```

---

## AW-LIC-005 — Trial lifecycle, trusted clock, offline grace, and CLI UX

Run mode: implementation/test  
Token budget: high

Task: implement a deterministic trial lifecycle and visible license commands.

Commands:

```bash
agentswatch license status
agentswatch license activate <key-or-device-code>
agentswatch license refresh
agentswatch license deactivate
agentswatch license diagnostics
```

Required behavior:

1. Exact expiration/grace dates shown in UTC and local time where useful.
2. Remaining run allowance shown before premium operation.
3. Latest trusted server time stored.
4. Large backward clock movement detected conservatively.
5. Running-session monotonic clock used where relevant.
6. Offline grace is explicit and testable.
7. Local state deletion does not grant a new trial.
8. Failed activation/refresh does not destroy a still-valid lease.
9. Network/keyring/clock failures use recoverable language, not piracy accusations.
10. No hidden network calls.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter "Trial|TrustedClock|OfflineGrace|LicenseCommand"
```

---

## AW-LIC-006 — Command entitlement gates and expired read-only mode

Run mode: implementation/test  
Token budget: medium

Task: add centralized entitlement gates to premium commands and preserve user-owned data after expiration.

Required behavior:

1. Every premium command declares required entitlement metadata.
2. Gate runs before premium assets or expensive work are accessed.
3. `--json` returns structured license error.
4. Local config cannot bypass entitlement.
5. Expired user can read/export existing `.ai` reports/history.
6. No repository file is encrypted, deleted, or changed because a license expired.
7. Free-tier commands continue according to product decision.
8. Retry does not double-count usage-limited operations.

Required tests:

- valid entitlement;
- missing entitlement;
- expired lease;
- offline grace;
- device mismatch;
- local feature flag bypass attempt;
- read/export after expiration;
- failed command does not consume a run.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter "FeatureGate|Entitlement|ExpiredMode|UsageCounter"
```

---

## AW-LIC-007 — Minimal licensing service

Run mode: architecture then implementation  
Token budget: high

Status: backlog after paid-beta decision.

Task: build the smallest service that can issue and refresh signed leases without receiving repository data.

Required endpoints:

```text
activate
refresh lease
deactivate device
license status/recovery
admin revoke
signing-key discovery or bundled key rotation metadata
```

Required server rules:

1. Private signing keys stay server-side/managed-secret storage.
2. Activation and refresh are rate-limited.
3. Device limits are enforced with recovery/deactivation flow.
4. Audit events contain license/account/device metadata only.
5. No source code, prompts, diffs, logs, reports, or command output accepted by licensing endpoints.
6. Trial creation is idempotent.
7. Lease renewal/revocation is testable.
8. Signing-key rotation has overlap period.

Validation must include service integration tests and client contract tests.

---

## AW-LIC-008 — Packaging and local IP hardening

Run mode: investigation followed by small implementation slices  
Token budget: medium

Task: remove trivial plaintext exposure of premium local assets and evaluate packaging protections honestly.

Required investigation:

1. List every shipped prompt/template/rule/policy asset.
2. Classify as public, user-editable, free, premium-local, or premium-remote.
3. Remove premium plaintext assets from normal install directories where feasible.
4. Evaluate embedded resources, compiled rule models, single-file publish, trimming, NativeAOT compatibility, and obfuscation.
5. Evaluate code signing and package verification.
6. Measure startup, package size, stack traces, plugin/reflection compatibility, and support impact.

Hard conclusion:

```text
Obfuscation and AOT are deterrents, not perfect secrecy.
```

Do not select a protection that breaks diagnostics or platform support without evidence.

---

## AW-LIC-009 — Trial/licensing security and regression suite

Run mode: test hardening  
Token budget: high

Task: add the release gate for licensing behavior.

Required scenarios:

1. Valid license.
2. Invalid signature.
3. Wrong issuer/audience.
4. Expired license.
5. Not-yet-valid license.
6. Offline grace active/expired.
7. Clock rollback.
8. Device/installation mismatch.
9. Protected state deleted/corrupted.
10. Key rotation.
11. Revoked license.
12. Unknown/future schema.
13. Local config entitlement bypass.
14. Copy installation folder to another machine/installation.
15. Failed operation does not consume usage allowance.
16. Retry is counted once.
17. Existing reports remain readable after expiry.
18. No license HTTP request contains source/prompt/diff/log/report data.

Validation:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter "License|Trial|Entitlement|ProtectedState|OfflineGrace|Tamper"
```

---

## AW-LIC-010 — Optional server-side premium algorithms

Run mode: docs/spec only first  
Token budget: medium

Status: backlog after premium value is proven.

Task: identify only the highest-value algorithms worth keeping remote for stronger IP protection.

Required decisions:

1. Which result cannot be delivered well through compiled local logic.
2. Minimum metadata needed.
3. Whether source/snippet upload is ever required.
4. Explicit opt-in and redaction.
5. Offline/local fallback.
6. Response contains task-specific result, not the full proprietary rule/template library.
7. Data retention and deletion.
8. Enterprise/local-only alternative.

Default:

```text
No source upload. Metadata-only where possible.
```

---

## AW-LIC-011 — User-facing trial, privacy, and activation documentation

Run mode: docs-only  
Token budget: low/medium

Task: write transparent user documentation before any public trial.

Required docs:

- exact trial duration/usage allowance;
- free-tier features;
- paid features;
- activation and device limits;
- offline lease/grace behavior;
- clock/keyring/network recovery;
- what happens after expiration;
- how to export/delete local data;
- exactly what licensing requests send;
- statement that no source code/prompts/diffs/run logs are sent for license verification;
- deactivation and device recovery;
- anti-copy protection is deterrence, not guaranteed secrecy.

Validation:

```bash
git diff --check
```
