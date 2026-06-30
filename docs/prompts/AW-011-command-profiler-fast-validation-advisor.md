# AW-011 — Command Profiler / Fast Validation Advisor

Run mode: investigation-first, then implementation in smaller follow-up prompts  
Token budget: medium  
Gate: after `AW-VAL-001`, `AW-VAL-002`, `AW-003`, and validation-runner groundwork

## Read first

- `AGENTS.md`
- `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`
- `docs/PROMPT_TOKEN_ECONOMY_RULEBOOK.md`
- `docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md`
- `docs/COMMAND_CONTRACTS.md`
- `docs/CLI_UX_OUTPUT_SPEC.md`
- `docs/ADAPTER_SPEC.md`
- `docs/REPORT_FORMATS.md`
- `docs/DATA_MODEL.md`
- `docs/SECURITY_AND_PRIVACY.md`

## Task

Design and later implement a local command profiler and fast validation advisor that helps agents avoid slow repeated validation commands and avoid sending large logs into model context.

Start with investigation/design unless a previous prompt has already produced accepted design evidence.

## Required behavior later

- add `agentswatch run -- <command>` as a local command wrapper;
- record command duration, exit code, output sizes, and compact error signature;
- add `agentswatch validate --suggest` recommendations;
- keep full stdout/stderr out of markdown reports by default;
- recommend language-specific fast validation commands;
- include compact command profile summaries in run reports and handoffs.

## Owned paths

```text
docs/COMMAND_PROFILER_FAST_VALIDATION_ADVISOR.md
docs/COMMAND_CONTRACTS.md
docs/CLI_UX_OUTPUT_SPEC.md
docs/ADAPTER_SPEC.md
docs/REPORT_FORMATS.md
docs/DATA_MODEL.md
src/AgentsWatch.Cli/
src/AgentsWatch.Core/
src/AgentsWatch.LanguageAdapters/
src/AgentsWatch.Reports/
tests/AgentsWatch.Tests/
```

## Avoid paths

```text
.github/workflows/
SaaS/dashboard code
cloud sync/auth/billing
unrelated prompt optimizer rewrites
unrelated git parser refactors
```

## Token and log rules

- Do not paste full terminal logs into the prompt or final answer.
- Do not write full stdout/stderr into markdown reports by default.
- Record command metrics and compact summaries only.
- Redact secret-looking values before writing summaries.
- Use targeted validation before broad validation when changed files allow it.

## Suggested split

```text
001-investigate-command-profiler-contracts.md
002-implement-command-history-model.md
003-implement-agentswatch-run-wrapper.md
004-implement-validate-suggest-fast-advisor.md
005-add-command-profile-report-section.md
006-add-security-redaction-tests.md
007-diff-only-review-command-profiler.md
```

## Validation

Use targeted validation first:

```bash
dotnet build AgentsWatch.sln
dotnet test --filter Command
dotnet test --filter Validation
```

Run broader validation only if runtime project references or shared contracts changed:

```bash
dotnet test AgentsWatch.sln
```

## Stop rules

Stop and report if:

- Gate 0 evidence is still missing;
- implementing the wrapper requires broad shell abstraction work;
- command output capture risks storing secrets without redaction;
- adapter detection cannot identify the project type;
- more than one runtime feature slice is needed;
- validation failures repeat twice.

## Return

1. files inspected;
2. files changed;
3. command contracts added or changed;
4. storage/report format decisions;
5. validation run or blocked reason;
6. command-log token waste avoided;
7. residual risk;
8. next minimal prompt.
