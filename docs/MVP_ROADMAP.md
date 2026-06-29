# AgentsWatch MVP Roadmap

Last aligned: 2026-06-29  
Status: planning/specification

## Strategy

Do not start with SaaS. Start with a local CLI that works with any repo through git, markdown, shell commands, and config.

Recommended first product:

```text
AgentsWatch CLI — AI coding-agent supervisor and token optimizer.
```

## Prototype: 3-7 days

Goal: prove the workflow manually with markdown files.

Deliverables:

- `.ai/` folder shape;
- example optimized prompt set;
- example run report;
- example risk report;
- example handoff summary;
- example changelog entry.

No database, no dashboard, no SaaS.

## Phase 1: CLI MVP, 2-4 weeks

Goal: useful local tool.

Features:

1. `.ai` folder generator.
2. Basic `agentswatch.yml` config.
3. Prompt optimizer.
4. Prompt splitter.
5. Git diff tracker.
6. Basic risk score.
7. Markdown run report.
8. Changelog generator.

Definition of done:

- works on a Flutter repo;
- works on a .NET repo;
- reports changed files;
- generates a prompt split;
- creates run report and changelog;
- no cloud dependency.

## Phase 2: Useful solo tool, 4-8 weeks total

Add:

1. Acceptance criteria checker.
2. Claimed-vs-actual diff checker.
3. Validation runner.
4. Handoff summary generator.
5. Token waste report.
6. Diff-only review prompt generator.
7. Language adapters for Flutter, .NET, React/TypeScript, Python, Node.

## Phase 3: Local dashboard, 8-12 weeks total

Goal: visual run history and risk review.

Suggested stack:

- backend: .NET local API;
- frontend: React;
- storage: SQLite.

Dashboard pages:

- Runs;
- Tasks;
- Changed files;
- Risk report;
- Token waste;
- Validation results;
- Changelog;
- Settings.

## Phase 4: Team beta, 3-5 months

Add:

- GitHub PR diff ingestion;
- GitHub Actions status ingestion;
- PR review report;
- policy rules;
- export reports;
- team usage overview.

## Phase 5: Paid SaaS, 6-9 months

Only after local CLI/dashboard shows real usage.

Add:

- auth;
- billing;
- cloud sync;
- teams;
- hosted dashboards;
- historical analytics;
- organization policies.

## MVP priority order

1. Init `.ai` folder.
2. Git diff tracker.
3. Run report.
4. Risk scoring.
5. Prompt optimizer.
6. Prompt splitter.
7. Handoff summary.
8. Changelog generator.
9. Validation runner.
10. Claimed-vs-actual diff.

Reason: git/diff/report creates immediate value even before prompt optimization is smart.
