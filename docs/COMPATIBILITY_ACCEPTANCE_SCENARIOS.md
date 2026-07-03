# AgentsWatch Compatibility Acceptance Scenarios

Last aligned: 2026-07-03  
Status: reproducible L1 acceptance contract; not executed

## Purpose

Prove that AgentsWatch selects honest support modes and fallbacks across models, tools, permissions, environments, and repository workflows.

## Global pass conditions

Every scenario must verify:

- detected/declared runtime profile;
- selected support mode per requested capability;
- known blind spots;
- confidence cap;
- fallback plan;
- no unsupported Full claim;
- no secret value persisted;
- no hidden network call;
- no unauthorized configuration change.

## Expected support-mode vocabulary

```text
Full
Guarded
Advisory
PostHoc
Manual
Unavailable
```

No synonyms such as `basically full`, `probably supported`, or `should work` are allowed in machine-readable outputs.

---

## COMP-001 — Chat-only model with no repository integration

Profile:

```text
Surface: chat-only
Observation: final response supplied manually
Permissions: no repository/shell/network control by AgentsWatch
Environment: none
VCS: user later applies a patch to git
```

Expected:

- Context Snapshot/Resume: Manual;
- Rules Compiler: Manual output/generic target;
- Trust Ledger: PostHoc after patch/CI evidence;
- Flight Recorder: Manual, not Full;
- Loop Guard live: Unavailable;
- Policy enforcement: Unavailable;
- Worktree Coordinator: Unavailable;
- PR Review: PostHoc after commit/PR.

Negative assertion:

- AgentsWatch must not claim to have observed file reads, commands, token use, or tool actions.

## COMP-002 — Local CLI with rich pre/post hooks

Profile:

```text
Surface: local interactive CLI
Observation: O6
Permissions: workspace write, restricted shell/network
Environment: local sandbox
VCS: clean git worktree
```

Expected:

- Flight Recorder: Full for exposed lifecycle;
- Trust Ledger: Full for git/command/test claims;
- Policy Firewall: Guarded or Full only according to declared interception coverage;
- Loop Detection: Full/Guarded;
- Live Stop: Guarded unless process ownership and checkpoint are verified;
- Context/Rules: Full;
- Worktree Coordinator: Full planner/integration if worktree controls exist.

Negative assertion:

- a rich hook set must not erase documented bypass paths.

## COMP-003 — Local CLI without hooks, launched by AgentsWatch wrapper

Profile:

```text
Observation: process stdout/stderr summary, exit, git/filesystem snapshots
Pre-tool events: unavailable
Process ownership: yes
```

Expected:

- Flight Recorder: Guarded;
- Trust Ledger: Full for outcomes supported by git/process evidence;
- Policy Firewall: wrapper enforcement for commands only, not hidden internal tool paths;
- Loop Guard: command-level Guarded;
- file-read history: Unknown;
- prompt/context internals: Unknown.

## COMP-004 — IDE extension using local workspace

Profile:

```text
Surface: IDE
Context: open files, selections, diagnostics may be available
Process ownership: no
VCS: local git
```

Expected:

- Flight Recorder: Guarded or PostHoc depending on extension events;
- Context Resume: Full target export with IDE context hints;
- live process stop: Unavailable unless host API exists;
- Trust Ledger: Full/PostHoc from git/tests;
- report records that open-file context is tool-supplied and may be incomplete.

## COMP-005 — Cloud agent in ephemeral container and PR delivery

Profile:

```text
Surface: cloud background agent
Environment: ephemeral/cached container
Network: agent phase disabled or allow-listed
VCS: provider branch/PR
Local process ownership: no
```

Expected:

- Flight Recorder: PostHoc/Guarded from provider logs;
- Trust Ledger: Full for commit/check-bound claims;
- Policy Firewall: Advisory/config audit, not local enforcement;
- Live Loop Stop: Unavailable unless provider cancellation API adapter exists;
- Worktree Coordinator: CloudBranchPR variant;
- environment setup/cache/commit identity included;
- missing private dependency classified EnvironmentBlocked.

## COMP-006 — GitHub/CI headless agent on fork pull request

Profile:

```text
Surface: CI
Interactive approvals: unavailable
Secrets: withheld
Token permissions: read or limited PR write
```

Expected:

- Policy decisions use workflow gates, not dialogs;
- unavailable secrets are not test failures;
- Trust Ledger binds checks to exact commit/attempt;
- PR Review reports fork/permission constraints;
- no automatic write/comment unless permission and explicit user configuration exist.

## COMP-007 — Read-only source access with shell disabled

Expected:

- prompt/context/rules lint: Full;
- Policy preflight: Full/Advisory;
- file modification claims: Unavailable/Contradicted if claimed;
- validation execution: Unavailable;
- test state: Missing, not failed;
- Loop Guard limited to reads/searches where observable;
- Worktree execution: planner only.

## COMP-008 — Workspace write enabled, shell disabled

Expected:

- scope/diff tracking: Full;
- edit oscillation detection: Guarded/Full;
- build/test claims require CI or remain Missing;
- command loop detection: Unavailable;
- Policy enforcement applies to write paths only;
- report must not recommend shell-dependent validation as executed evidence.

## COMP-009 — Shell enabled, network disabled

Fixture:

- dependencies missing locally;
- test command fails because package restore cannot reach registry.

Expected:

- failure classified EnvironmentBlocked/DependencyUnavailable;
- not automatically attributed to implementation defect;
- cost/loop system does not repeatedly retry without network-state change;
- checkpoint/handoff records required dependency.

## COMP-010 — Network enabled, private credentials unavailable

Fixture:

- public dependency works;
- private registry/integration service fails authentication.

Expected:

- public and private network capabilities separated;
- secret value never requested for storage in report;
- failure classified CredentialUnavailable;
- PR review notes incomplete protected integration validation.

## COMP-011 — Broad/YOLO/full-access mode

Expected:

- permission risk elevated;
- broad access does not imply full observability;
- policy enforcement class remains based on actual hooks/wrapper/sandbox;
- mandatory warning and recommended isolation/checkpoint;
- production-affecting actions remain blocked by AgentsWatch policy when configured;
- no automatic enabling of broad mode.

## COMP-012 — Managed enterprise policy overrides project policy

Fixture:

```text
Project allows network/write
Managed policy denies network and limits write paths
```

Expected:

- effective permissions reflect managed restriction;
- conflict and source precedence visible;
- Rules Compiler does not overwrite managed configuration;
- target output status: ConflictWithManagedPolicy or Weaker;
- no attempt to bypass policy.

## COMP-013 — Native Windows versus WSL2

Run same fixture in both environments.

Expected:

- distinct environment/profile IDs;
- path normalization does not merge `C:\repo` and `/mnt/c/repo` blindly;
- shell commands/adapters are platform-correct;
- symlink/junction/case behavior tested separately;
- canary comparison reports environment difference as confounder.

## COMP-014 — Dev container with host bind mount

Fixture:

- source mounted into container;
- toolchain inside container;
- host and container paths differ.

Expected:

- command evidence attributed to container;
- file evidence mapped to repository identity without leaking arbitrary host paths;
- image/devcontainer hash recorded;
- host-level policy not assumed to cover container subprocesses;
- Docker socket access, if present, elevates risk.

## COMP-015 — Nested/unprivileged container with weakened sandbox

Expected:

- environment reports weakened sandbox;
- Policy Firewall cannot claim PE5 Full;
- guarded/advisory mode selected according to actual controls;
- security limitation included in run report and proof bundle.

## COMP-016 — Remote SSH workspace

Fixture:

- IDE runs locally;
- shell/files run remotely;
- clocks and paths differ.

Expected:

- local IDE and remote execution sources retained separately;
- remote collector requirement surfaced;
- local process wrapper does not claim remote control;
- network/secrets attributed to remote host;
- timeline normalizes clock skew without hiding it.

## COMP-017 — Cloud environment cache changes

Fixture:

- baseline uses cached environment A;
- candidate uses invalidated setup script/environment B.

Expected:

- Regression Canary marks comparison confounded;
- setup/cache identity visible;
- cost/time changes are not attributed only to model;
- Trust Ledger binds results to environment revision.

## COMP-018 — Git worktree worker

Expected:

- repository ID shared with main checkout;
- workspace/worktree ID distinct;
- base commit and owned paths recorded;
- changes in another worktree not attributed to worker;
- cleanup dry-run protects uncommitted work;
- ignored setup-file absence is reported.

## COMP-019 — Cloud branch/PR worker

Expected:

- Multi-Agent Coordinator selects CloudBranchPR, not LocalWorktree;
- ownership uses repository paths and PR branch identity;
- integration check consumes PR diff/checks;
- no claim about local cwd or filesystem isolation.

## COMP-020 — Shared workspace with two agents

Fixture:

- both workers can write same checkout;
- ownership sets are initially disjoint;
- one worker writes into the other's path.

Expected:

- coordinator mode SharedWorkspaceOwnership;
- no isolation claim;
- ownership violation detected;
- event lineage identifies observed writer where available;
- unresolved attribution remains Unknown rather than guessed.

## COMP-021 — Multi-root monorepo

Fixture:

- .NET backend and React frontend roots;
- nested instruction files;
- task affects one API contract and one client.

Expected:

- multiple stack adapters compose;
- nearest instruction precedence retained;
- validation scoped to affected roots plus contract boundary;
- ownership and token/file budgets are per root;
- no whole-monorepo scan by default.

## COMP-022 — No-git workspace

Expected:

- Flight Recorder can use file manifests/hashes if available;
- Trust Ledger confidence lower than commit-bound proof;
- PR Review unavailable or manual diff import;
- Worktree Coordinator unavailable;
- Context/Rules/Policy/Loop analysis can still function according to other capabilities;
- no instruction to initialize git automatically without approval.

## COMP-023 — Dirty checkout with pre-existing changes

Expected:

- start snapshot identifies pre-existing files;
- run-owned changes separated where evidence allows;
- `no unrelated files changed` claim cannot be supported if ownership is ambiguous;
- restore/checkpoint operations protect pre-existing work;
- PR packet labels unbound/uncommitted evidence.

## COMP-024 — Generated and binary-heavy repository

Expected:

- changed files listed without trying to parse binary content as source;
- generated output linked to generator/input/command evidence;
- reviewer packet prioritizes source-of-generation changes;
- token/context system avoids loading large generated/binary files;
- trust result remains Missing when provenance cannot be established.

## COMP-025 — Planner/editor two-model workflow

Fixture:

- model A creates plan;
- model B edits files.

Expected:

- separate ModelProfile entries and roles;
- cost/usage attributed per role;
- plan and edit lineage linked;
- edit claim evidence bound to editor actions;
- Regression Canary compares equivalent role composition.

## COMP-026 — Automatic model routing during task

Expected:

- alias and resolved model recorded when available;
- model transition creates profile revision;
- cost/canary report does not claim a single-model run;
- hidden routing caps model-specific conclusions.

## COMP-027 — Model changed, tool/environment unchanged

Expected:

- canary may compare model variants if prompt/rules/permissions/environment are pinned;
- repeated runs required;
- difference report separates observed metrics from causal conclusion.

## COMP-028 — Tool version changed, model unchanged

Expected:

- tool version in comparison key;
- hook/event/permission capability handshake reruns;
- changed event coverage prevents naive model-regression claim;
- adapter incompatibility downgrades support mode.

## COMP-029 — Hook configured but not firing

Expected:

- handshake/health marks hook unavailable or unhealthy;
- Flight Recorder downgrades;
- Policy Firewall does not claim hook enforcement;
- fallback uses wrapper/git/post-hoc evidence;
- finding explains configuration-versus-effective mismatch.

## COMP-030 — Hook covers one tool path but an equivalent path exists

Expected:

- Policy mode Guarded, not Full;
- known bypass documented;
- post-tool/git verification enabled;
- user sees exact enforcement class.

## COMP-031 — Adapter fails mid-run

Expected:

- new profile revision created;
- later events marked partially observed;
- support modes downgrade from failure time;
- no fabricated continuity;
- run remains readable and marked incomplete.

## COMP-032 — Context export to weaker target format

Fixture:

- canonical policy contains path, network, approval, and subagent constraints;
- target supports only textual instructions.

Expected:

- output status Weaker/AdvisoryOnly;
- loss report lists unenforceable fields;
- generated text does not claim hard enforcement;
- original canonical policy remains unchanged.

## COMP-033 — Context budget smaller than snapshot

Expected:

- deterministic priority-based compaction;
- decisions, scope, validation, and residual risks retained before narrative history;
- dropped sections listed;
- no silent truncation;
- source snapshot/hash retained.

## COMP-034 — GUI/mobile behavior claim

Fixture:

- build passes;
- no UI test or manual visual evidence.

Expected:

- `build passed` Supported;
- `UI works correctly` Missing/NotVerifiable;
- suggested evidence includes targeted UI/widget/integration/manual check according to stack/environment;
- no screenshot fabrication.

## COMP-035 — Simulator/emulator is a singleton resource

Fixture:

- two workers request same simulator.

Expected:

- coordinator creates resource lease;
- validation is serialized or assigned to one worker;
- no parallel-completion claim;
- waiting time is not classified as agent loop without evidence.

## COMP-036 — Air-gapped local model

Expected:

- core local functions work without cloud account;
- usage report uses available local compute/time metrics only;
- no provider-price estimate;
- rules/context export has offline fallback;
- missing network is expected environment state;
- licensing or telemetry cannot block access to user-owned artifacts.

## COMP-037 — Corporate proxy and allowlist

Expected:

- allowed and denied destinations recorded as classes/domains without secrets;
- dependency failures correlated with policy;
- Policy Firewall does not widen allowlist automatically;
- setup-phase and agent-phase network permissions separated.

## COMP-038 — Test credentials versus production credentials

Expected:

- credential class and target environment recorded, never value;
- production profile forces approval-gated/advisory behavior;
- successful authenticated command is not deployment safety proof;
- external side effects require exact target/action approval.

## COMP-039 — Stacked pull request

Fixture:

- PR B targets feature branch from PR A.

Expected:

- review packet uses actual PR base/merge base;
- inherited changes from PR A are not reported as new PR B work;
- CI absence due to workflow target rules is reported accurately;
- later retargeting triggers profile/evidence refresh.

## COMP-040 — Provider checkpoint versus git commit

Fixture:

- tool exposes shadow snapshot/checkpoint;
- main git history unchanged.

Expected:

- checkpoint provenance ProviderCheckpoint;
- not presented as commit-bound proof;
- restore capability and storage limitations recorded;
- Trust Ledger may use it for file-state evidence with an appropriate confidence cap.

## COMP-041 — Missing provider usage telemetry

Expected:

- action/time metrics remain available;
- token count, price, and quota remain Unknown;
- no monetary savings claim;
- Loop Guard can still detect repeated observed actions.

## COMP-042 — Provider reports quota percentage but not tokens/cost

Expected:

- raw percentage/source stored;
- no conversion to tokens or currency;
- acceleration alert uses percentage units and confidence;
- cross-provider comparison marked non-equivalent.

## COMP-043 — Human deliberately reruns validation

Fixture:

- same test command executed twice after no code change because the user explicitly requested confirmation.

Expected:

- marked human-requested rerun;
- not automatically classified as waste/loop;
- report may note duplicate cost without stop recommendation.

## COMP-044 — Same failed validation after relevant change

Expected:

- action fingerprint includes relevant state revision;
- rerun considered a new justified attempt;
- repeated failure may create investigation finding, not immediate loop classification.

## COMP-045 — Production infrastructure repository

Expected:

- environment/permission profile elevated;
- Policy Firewall Advisory/approval-gated unless independent enforcement exists;
- no autonomous apply/deploy;
- plan/dry-run evidence required;
- human approval bound to exact environment/action;
- success means validated plan/change, not production rollout unless external evidence exists.

## COMP-046 — Unknown tool/version

Expected:

- universal git/file behavior used where safe;
- advanced features start Manual/PostHoc/Advisory;
- no guessed hooks, rule precedence, or permission semantics;
- compatibility discovery generated as a follow-up.

## COMP-047 — Tool capability removed after update

Expected:

- handshake detects loss;
- profile revision and downgrade;
- release/canary report identifies tool capability change;
- existing config does not preserve a false Full state.

## COMP-048 — Conflicting clocks and out-of-order events

Expected:

- raw timestamps retained;
- normalized order uses causal IDs where available;
- clock skew visible;
- no false loop based only on timestamp disorder;
- timeline confidence lowered.

## COMP-049 — Sensitive file read denied but file change visible in git

Expected:

- report may include path/risk and change metadata;
- content remains absent;
- Trust Ledger can prove changed/not changed without reading secret content;
- Policy result and git evidence remain separate.

## COMP-050 — Feature support report round trip

Expected:

- JSON and markdown support decisions agree;
- every requested capability has exactly one support mode;
- every non-Full mode has reason/fallback;
- every Full/Guarded mode lists capability sources;
- profile hash and schema version survive write/read.

## Cross-platform execution matrix

Minimum automated fixture execution:

| Layer | Linux | Windows | Additional/manual |
|---|---:|---:|---|
| Profile schema/serialization | Required | Required | macOS compatibility review |
| Path/worktree identity | Required | Required | WSL/native cross-path fixture |
| Permission conflict engine | Required | Required | managed-policy fixtures |
| Fallback planner | Required | Required | cloud/chat fixtures |
| Secret redaction | Required | Required | shell-specific quoting cases |
| CLI black-box compatibility report | Required | Required | container/remote fixture |

## Maturity gate

Compatibility capabilities cannot be advertised as production-ready until:

1. all 50 scenarios have executable fixtures or documented manual verification where external platforms are required;
2. no scenario incorrectly upgrades a capability;
3. downgrade behavior works mid-run;
4. Linux and Windows black-box tests pass;
5. at least two local tools, one cloud/PR flow, and one manual/chat flow are dogfooded;
6. independent verification confirms unsupported and blind-spot wording.
