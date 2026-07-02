# AgentsWatch Trial Licensing and IP Protection Plan

Last aligned: 2026-07-02  
Status: post-MVP commercial architecture plan  
Target product: AgentsWatch local CLI and future desktop/dashboard editions

## Executive decision

AgentsWatch can provide a time-limited or usage-limited trial, but it cannot perfectly hide every file or algorithm that must execute locally on the user's computer.

Core security truth:

```text
If the user's machine can decrypt, execute, or render proprietary content, a determined user can eventually inspect or copy it.
```

Therefore the product must not depend on “secret local files” as its main protection.

Recommended strategy:

```text
compiled and signed local client
+ signed feature entitlements
+ short-lived offline-capable license lease
+ secure local license storage
+ no raw proprietary templates shipped when avoidable
+ optional server-side execution only for the highest-value premium logic
+ user source code, diffs, prompts, and reports remain local by default
```

The goal is to prevent casual copying, perpetual trial use, clock rollback, and simple file replacement. It is not realistic to guarantee that a skilled reverse engineer can never inspect local code or generated output.

---

## 1. What can and cannot be hidden

### Can be reasonably protected

- Product source code, if distributed only as compiled binaries.
- License and entitlement state, when cryptographically signed.
- Premium feature availability, when checked at command boundaries.
- Trial expiration, when verified with a server-signed lease and rollback-resistant local state.
- Raw proprietary template libraries, when not copied as plaintext files into the installation.
- Premium rules that remain on a server and return only task-specific results.
- Update feeds, package downloads, and premium plugin access.

### Cannot be perfectly protected

- Any output intentionally written into the user's repository.
- Any prompt or report shown to the user.
- Any template decrypted and used locally at runtime.
- Compiled .NET logic from a determined reverse engineer.
- A machine fingerprint, because local machine values can be spoofed.
- Purely local expiration checks based only on the device clock.

### Product rule

```text
Do not promise that AgentsWatch makes local intellectual property impossible to copy.
Promise that licensing prevents normal unauthorized use and that proprietary implementation details are not distributed as editable plaintext files.
```

---

## 2. Threat model

AgentsWatch should defend against:

1. Copying an installation folder to another machine.
2. Resetting or deleting a local trial-state file.
3. Rolling the device clock backward.
4. Replacing a local config file to enable premium features.
5. Modifying a plaintext feature manifest.
6. Reusing an activation token on too many machines.
7. Extracting raw proprietary prompt/template files from the package.
8. Running an expired binary indefinitely while permanently offline.
9. Patching obvious client-side boolean checks.
10. Copying generated output that the user is legitimately shown.

The last item cannot be technically prevented. Generated output is intentionally delivered to the user and must be treated as copyable.

Out of scope:

- nation-state or highly funded reverse engineering;
- kernel-level anti-debugging;
- invasive device surveillance;
- hidden telemetry;
- DRM that encrypts or damages the user's repository;
- collecting source code to enforce licensing.

---

## 3. Recommended product editions

### Community or free local mode

Consider keeping a useful permanent local tier:

- project initialization;
- basic status;
- limited run reports;
- limited git diff summary;
- local data export/read access.

Benefits:

- fewer hostile trial-reset attempts;
- easier trust and adoption;
- users retain access to their own run history;
- premium value can focus on advanced optimization rather than artificial lockout.

### Trial mode

Recommended default trial:

```text
14 days or 25 supervised runs, whichever comes later or according to final product policy
```

Trial features can include most premium functionality so the user can evaluate real value.

Trial should require one visible activation call, but should work offline afterward through a short-lived signed lease.

### Paid local Pro mode

- full local CLI features;
- longer offline lease;
- premium adapters and advanced optimization;
- update access;
- optional support;
- no source-code upload required.

### Future team/SaaS mode

- team policy packs;
- organization entitlements;
- central billing;
- optional aggregate reporting;
- explicit data-sharing controls.

This remains post-MVP and must not be required for the local CLI to function.

---

## 4. Licensing architecture

### 4.1 License authority

Use a small licensing service as the source of truth for:

- license account/customer id;
- product edition;
- enabled features;
- trial start/end;
- activation count;
- device registrations;
- revoked licenses;
- lease issuance;
- minimum supported client version if needed.

The licensing service must never receive repository source code, prompts, diffs, validation output, or run logs for ordinary license checks.

### 4.2 Signed entitlement token

The client should receive a server-signed entitlement document, for example:

```json
{
  "licenseId": "lic_...",
  "subject": "customer-or-trial-id",
  "edition": "trial",
  "features": ["core", "reports", "handoff", "review", "learning"],
  "issuedAtUtc": "2026-07-02T10:00:00Z",
  "notBeforeUtc": "2026-07-02T10:00:00Z",
  "expiresAtUtc": "2026-07-09T10:00:00Z",
  "trialEndsAtUtc": "2026-07-16T10:00:00Z",
  "deviceBinding": "hashed-installation-id",
  "leaseVersion": 1,
  "maxOfflineDays": 7
}
```

Use asymmetric signatures:

- server signs with a private key;
- client contains only the public verification key;
- client must never contain a shared signing secret.

Suitable formats include a compact signed JSON structure, JWT with carefully restricted algorithms, PASETO public tokens, or a custom canonical JSON signature envelope.

Security requirements:

- fixed allowlisted signing algorithm;
- key id for rotation;
- issuer/audience validation;
- not-before and expiry validation;
- device/installation binding validation;
- feature claims validated centrally in one component;
- no client-generated entitlements.

### 4.3 Short-lived lease

Do not issue a license token that remains valid forever.

Recommended behavior:

- trial lease: 24–72 hours;
- paid local Pro lease: 7–30 days;
- offline grace: explicit and documented;
- refresh lease when online;
- after grace expires, premium actions stop but user-created data remains readable/exportable.

This makes revocation and trial expiration enforceable without requiring the app to be always online.

### 4.4 Device binding

Use a privacy-preserving installation identity rather than a raw hardware fingerprint whenever possible.

Recommended:

1. Generate a random installation id on first activation.
2. Store it in OS-protected storage.
3. Optionally mix in a coarse machine binding hash.
4. Send only the derived hash to the licensing service.
5. Allow users to deactivate old devices.

Avoid collecting invasive hardware inventories.

Device binding is a deterrent, not perfect security.

---

## 5. Local secure storage

Store license state through a platform abstraction:

- Windows: DPAPI, Windows Credential Manager, or protected local secret storage.
- macOS: Keychain.
- Linux: Secret Service/keyring where available, with a documented secure fallback.

Persist separately:

- signed lease token;
- installation id;
- last trusted server time;
- last successful lease refresh time;
- monotonic usage counters;
- trial run-count receipts or signed counter checkpoint;
- last seen client version/license schema version.

Do not rely on encryption with a static key embedded in the binary. A static embedded key can be extracted.

Local state must be treated as tamperable. Integrity must come from signatures, monotonic checks, server reconciliation, and conservative failure behavior.

---

## 6. Trial expiration and anti-rollback

Pure local clock checks are insufficient.

Recommended layered checks:

1. Validate the signed token expiration.
2. Save the latest trusted server UTC time.
3. Reject large backward movement relative to the latest trusted time.
4. Use monotonic process time during a running session.
5. Refresh the lease periodically when online.
6. Store usage counters with integrity checks and reconcile with the server.
7. Detect deletion/reset of local protected state and require reactivation.

Do not permanently lock a legitimate paid user merely because the clock changed or a keyring was unavailable. Provide a clear recovery path.

### Expired behavior

After trial/lease expiration:

Allowed:

- `agentswatch license status`;
- activation/login/deactivation;
- reading/exporting existing user-owned reports and run history;
- uninstall/reset commands;
- optional free-tier commands.

Blocked:

- premium prompt optimization;
- premium policy packs;
- advanced review/learning/adapters;
- new premium supervised runs.

Never encrypt, delete, corrupt, or hold user repository files hostage.

---

## 7. Protecting templates and proprietary rules

### Plaintext files to avoid shipping

Do not distribute premium assets as editable installation files such as:

```text
premium-prompts/*.md
premium-rules/*.json
premium-policies/*.yml
```

A user can trivially copy them.

### Better local packaging

For local-only logic:

- compile rules into assemblies;
- embed small assets as resources;
- publish as a .NET global tool or signed self-contained binary;
- consider single-file publish;
- consider trimming/AOT only after compatibility evidence;
- optionally use a reputable .NET obfuscator after legal and operational review;
- sign releases and verify update/package integrity.

Important limitation:

```text
Embedding, single-file publishing, AOT, and obfuscation raise reverse-engineering cost. They do not create perfect secrecy.
```

### Best protection for high-value logic

Keep only the highest-value premium algorithm or policy generation server-side.

Privacy-preserving server-side options:

- send feature request plus coarse repo metadata only;
- send hashes/counts/language/project type rather than source content;
- send user-selected sanitized snippets only with explicit consent;
- return a task-specific result rather than the complete policy/template library.

Server-side execution conflicts with strict offline/local-first goals, so it should be optional and limited to premium features that genuinely require stronger IP protection.

### Generated outputs

Anything AgentsWatch writes into `.ai/`, terminal output, reports, prompts, or handoffs is visible and copyable by design.

Do not attempt to hide or encrypt these user-facing artifacts. That would damage trust and usability.

---

## 8. Feature entitlement gates

Use one central entitlement service in the client:

```text
ILicenseService
IEntitlementEvaluator
ILicenseStorage
ILicenseLeaseClient
IInstallationIdentityProvider
ITrustedClock
```

Every premium command must declare required entitlements:

| Command/feature | Example entitlement |
|---|---|
| basic init/status | `core` |
| advanced report | `reports.pro` |
| prompt optimizer | `optimizer.pro` |
| task splitter | `splitter.pro` |
| diff-only review | `review.pro` |
| mistake learning | `learning.pro` |
| premium adapters | `adapters.pro` |
| team policy pack | `team.policy` |

Command order:

```text
parse command
-> resolve required entitlement
-> validate signed lease
-> check trial/usage allowance
-> show clear denial/recovery message if blocked
-> only then access premium assets or perform work
```

Do not scatter boolean license checks across commands. Centralized gates are easier to test and harder to bypass accidentally.

---

## 9. Usage-limited trial counters

A run-count trial can complement a time limit.

Rules:

- count only meaningful successful premium operations;
- do not count `--help`, failed validation, activation, status, or license recovery;
- identify operations with stable ids so retries are not double-counted;
- periodically reconcile counters with the licensing service;
- keep signed server receipts/checkpoints where feasible;
- show remaining allowance before an operation starts.

Local counters alone are resettable. Treat server reconciliation as the authority for commercial enforcement.

---

## 10. User experience and trust

Required CLI commands:

```bash
agentswatch license status
agentswatch license activate <key-or-device-code>
agentswatch license refresh
agentswatch license deactivate
agentswatch license diagnostics
```

Required messages:

- trial start and exact expiration date;
- remaining run allowance if usage-limited;
- last successful license refresh;
- offline grace end date;
- which feature is unavailable and why;
- how to activate, refresh, or recover;
- what data the licensing call sends.

No hidden network calls. The first activation and periodic refresh behavior must be disclosed.

Do not show frightening anti-piracy messages or accuse users of tampering when the cause might be a clock, keyring, proxy, or network problem.

---

## 11. Release/package protection

Recommended release controls:

- signed NuGet/global-tool package;
- checksums for release artifacts;
- signed update manifest;
- CI-generated reproducible release metadata;
- no private signing keys in repo or client;
- separate public verification keys from private licensing keys;
- SBOM and dependency scanning;
- package provenance where supported;
- optional single-file/self-contained editions after smoke tests.

Do not put production licensing API secrets into the client. The client should only contain public verification keys and public endpoint configuration.

---

## 12. What not to do

Do not:

- rely on hidden folders or file attributes;
- rely on renaming `.md` files to binary extensions;
- use a hardcoded AES key in the client;
- encrypt user repository files;
- delete user reports after expiration;
- require continuous internet access for every command;
- collect source code merely to enforce a license;
- store a plain `trialEndsAt` value in config and trust it;
- use only a machine fingerprint with no signed server authority;
- claim obfuscation makes copying impossible;
- create hidden telemetry for tamper detection.

---

## 13. Recommended implementation phases

### Phase 0 — Product and threat-model decision

- decide free tier vs trial-only;
- decide time limit, usage limit, or hybrid;
- define online/offline expectations;
- classify which premium logic stays local and which may be server-side;
- define privacy disclosure;
- define expired behavior.

### Phase 1 — Local licensing contracts

- license/entitlement models;
- signed token verification;
- trusted clock abstraction;
- secure storage abstraction;
- installation identity;
- central command entitlement gates;
- deterministic tests using a fake clock and test signing key.

### Phase 2 — Trial lifecycle

- activate/status/refresh/deactivate/diagnostics commands;
- trial time and run counters;
- offline lease/grace behavior;
- clock rollback detection;
- reset/reinstall/device-copy scenarios;
- clear expiry UX.

### Phase 3 — Licensing service

- activation endpoint;
- signed lease issuance;
- renewal/revocation;
- device activation limits;
- deactivation/recovery;
- audit events without source data;
- rate limiting and abuse controls.

### Phase 4 — Packaging/IP hardening

- remove plaintext premium asset files;
- embed/compile local rules;
- signed packages and update manifests;
- optional obfuscation/single-file/AOT evaluation;
- reverse-engineering review focused on casual bypasses.

### Phase 5 — Optional premium server-side features

- identify only the algorithms worth keeping remote;
- define minimal metadata request contracts;
- require explicit consent for any content upload;
- keep core local CLI functional without source upload.

---

## 14. Recommended MVP boundary

Licensing should **not** block the current CLI MVP validation and dogfood work.

Recommended order:

```text
validate and prove CLI MVP
-> dogfood and measure value
-> define commercial edition/free tier
-> implement local license contracts
-> add licensing service
-> harden packaging
```

Do not spend weeks on DRM before proving users want the product.

The licensing architecture can be designed now, but runtime implementation belongs after CLI MVP evidence and before a public paid beta.

---

## 15. Acceptance criteria

The commercial trial design is acceptable when:

1. Deleting a normal config file does not reset the trial.
2. Copying the installation folder to another machine does not copy the activation automatically.
3. Rolling the clock backward is detected conservatively.
4. An expired trial cannot run premium commands after offline grace.
5. Existing user reports remain readable/exportable after expiration.
6. Feature flags in local config cannot enable unlicensed features.
7. The client verifies server signatures with a public key only.
8. No source code, prompt, diff, or run log is sent during license activation/refresh.
9. Premium plaintext template libraries are not shipped as normal files.
10. Tests cover expiry, offline grace, clock rollback, storage loss, device mismatch, revoked license, key rotation, and command gating.
11. The product documentation states clearly that local anti-copy protection is deterrence, not perfect secrecy.

---

## Final recommendation

For AgentsWatch, the best balance is:

```text
local-first CLI with a permanent small free tier
+ 14-day/usage-limited Pro trial
+ server-signed short-lived offline lease
+ OS secure storage
+ central feature entitlement gates
+ compiled/embedded premium local logic
+ optional server-side execution only for the most valuable premium algorithms
```

This protects normal commercial use without violating the product's local-first privacy promise or pretending that user-visible local output can be made impossible to copy.
