# AgentsWatch Agent Long Task Playbook

Last aligned: 2026-06-29

## Purpose

Control long AI-agent runs so they do not turn into broad repo exploration.

Adapted from MathLearning long-task rules for AgentsWatch bootstrap, CLI, docs/evidence, and validation work.

---

## Use this when

- the task takes more than a few tool/terminal steps;
- the agent must choose from a queue;
- validation fails;
- multiple files are involved;
- a previous run may have partially completed the task;
- the agent wants to inspect the whole repo;
- build/test/CI state is unclear.

---

## Control loop

```text
1. State selected prompt and queue.
2. Check bootstrap gate status.
3. Check current git/status evidence.
4. Decide: validate, fix one failure, or stop with evidence.
5. Inspect only relevant docs/files.
6. Make one focused patch if needed.
7. Run narrow validation if possible.
8. Record evidence, missed work, follow-up, residual risk.
9. Commit/push only owned files.
```

---

## Bootstrap rule

If Gate 0 is incomplete, long tasks must start from:

- `docs/prompt_queues/bootstrap_validation.md`
- `docs/prompts/AW-VAL-001-build-validation.md`
- `docs/prompts/AW-VAL-002-cli-smoke.md`

Do not skip to feature work because roadmap prompts look more interesting.

---

## Existing work rule

Before editing:

```bash
git status --short -uall
git diff --stat
git diff -- <path>
```

If a file already contains the intended change, do not recreate it.

If the worktree is clean but work seems “missing”, check recent commits before rewriting.

---

## Validation strategy

Prefer:

```bash
dotnet restore AgentsWatch.sln
dotnet build AgentsWatch.sln
dotnet test AgentsWatch.sln
```

For targeted fixes:

```bash
dotnet test --filter <test-or-class-name>
```

If validation cannot be run in the current environment, record it as blocked. Do not claim pass.

---

## Environment failures

Classify before editing code.

| Symptom | Response |
|---|---|
| missing .NET SDK | record environment blocker; do not rewrite code blindly |
| NuGet network issue | retry once later or mark blocked; do not change runtime logic |
| CI run not visible | record evidence unavailable; do not mark Gate 0 complete |
| one test fails | inspect exact failure and fix only owned area |
| same failure twice | stop and write handoff |

---

## Repetition limits

Stop when:

- same command fails twice for the same reason;
- more than two unrelated files are needed for one fix;
- root cause is still unknown after initial scan;
- validation is unavailable and no CI evidence exists;
- a dashboard/SaaS idea appears before CLI gates pass.

Reassessment format:

```text
Selected prompt:
Changed files:
Last real failure:
Environment blocker:
What is already in main:
Smallest next step:
```

---

## Final response shape

```text
Selected prompt:
Why selected:
Changed files:
Validation run:
Validation not run:
Commit SHA:
Completion %:
Missed:
Follow-up:
Residual risk:
Token optimization applied:
```
