# Agent Risk Boundaries

Last aligned: 2026-06-30  
Status: safety/governance policy

## Purpose

AgentsWatch must prevent coding agents from taking risky actions on their own.

Core rule:

```text
Agents may suggest risky actions. They must not execute them without an explicit approval gate.
```

This document defines what is allowed, what requires approval, and what is blocked by default.

## Default posture

AgentsWatch should default to:

- read-only first;
- local-first;
- minimal scope;
- no hidden network calls;
- no destructive operations;
- no secret exposure;
- no production changes;
- no autonomous merge/deploy/release;
- evidence before completion.

## Risk levels

### Safe by default

Allowed without human approval when scoped:

- read repository docs and source files within the prompt scope;
- generate prompts;
- generate markdown docs;
- generate local run reports;
- suggest validation commands;
- run read-only git commands;
- run targeted local build/test commands if explicitly requested by the prompt;
- create local-only summaries that do not include secrets.

### Approval required

Agent must stop and request explicit approval before:

- editing CI/CD workflow files;
- editing auth, security, permissions, or encryption code;
- editing database migrations or destructive data scripts;
- changing public API contracts;
- changing billing, payments, plans, quotas, or subscriptions;
- changing deployment, hosting, cloud, DNS, or infrastructure configuration;
- adding a new package, SDK, or external service;
- running commands that modify git history;
- running full test/CI/release workflows when targeted validation is enough;
- creating PR comments automatically;
- sending data to any external service;
- storing raw command output;
- storing command strings that may contain secrets;
- accessing files outside the repository root.

### Blocked by default

Agent must not do these by default:

- delete files or directories outside explicit owned paths;
- run destructive shell commands;
- run production deploys;
- run database drops/truncates/resets;
- rotate, create, or expose secrets;
- upload source code, prompts, diffs, logs, or run history to a cloud service;
- enable telemetry by default;
- install global tools or change machine-level configuration;
- change OS/user settings;
- rewrite git history;
- force push;
- merge PRs without explicit user instruction;
- auto-approve its own changes;
- hide validation failures;
- mark work complete without evidence.

## Dangerous command patterns

Commands matching these patterns require explicit approval or must be refused:

```text
rm -rf
rmdir /s
del /s
format
mkfs
dd if=
chmod -R 777
chown -R
git reset --hard
git clean -fd
git push --force
git rebase
DROP DATABASE
TRUNCATE TABLE
DELETE FROM without WHERE
terraform apply
pulumi up
kubectl delete
kubectl apply to production context
fly deploy
vercel --prod
netlify deploy --prod
docker system prune
npm publish
dotnet nuget push
```

This list is not complete. If a command can destroy data, change production, expose secrets, or change global state, it must be gated.

## Sensitive paths

Changes to these paths require approval or elevated review:

```text
.github/workflows/
Dockerfile
docker-compose*.yml
fly.toml
render.yaml
vercel.json
netlify.toml
k8s/
helm/
terraform/
pulumi/
infra/
Migrations/
*.sql
.env
*.env
*.pem
*.key
*.pfx
*.p12
secrets*.json
appsettings.Production.json
src/**/Auth*/
src/**/Security*/
src/**/Billing*/
src/**/Payments*/
```

## Approval contract

When approval is required, the agent must output:

```text
Approval required:
Action:
Why risky:
Files/commands involved:
Safer alternative:
Validation needed:
Rollback plan:
```

The agent must not proceed until the user explicitly approves that exact action.

## Safer alternatives

Prefer:

- plan instead of execute;
- dry-run instead of apply;
- diff/stat instead of full mutation;
- targeted validation instead of full suite;
- local report instead of external upload;
- markdown evidence instead of raw logs;
- PR draft instead of merge;
- migration review instead of migration execution.

## Roadmap-driven execution rule

Roadmap items must carry risk gates.

If a roadmap item touches a risky area, AgentsWatch should generate an investigation or approval prompt first, not implementation.

Example:

```text
Roadmap item: Add production deploy automation
Generated first prompt: investigate deployment safety and approval gates
Blocked implementation until: human approves deployment scope
```

## Self-review rule

After each agent run, the self-review must check:

- Did the agent touch sensitive paths?
- Did it run or suggest dangerous commands?
- Did it store or display secrets?
- Did it change production or external services?
- Did it exceed owned paths?
- Did it honestly record validation?
- Does the next prompt need approval?

If any answer is yes, roadmap status should become `NeedsApproval` or `Blocked`, not `Done`.

## Fail-closed rule

If AgentsWatch is unsure whether an action is safe, it must treat it as risky.

```text
When uncertain, stop and ask for approval.
```

## Non-goals

AgentsWatch should not become:

- a fully autonomous deployment bot;
- a secret manager;
- a cloud automation engine;
- an unreviewed PR merger;
- a replacement for human approval on risky changes.
