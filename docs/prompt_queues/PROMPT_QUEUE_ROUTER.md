# AgentsWatch Prompt Queue Router

Last aligned: 2026-09-16

Use this file first when choosing the next AgentsWatch task.

## Canonical queue

Current active implementation + validation queue:

```text
docs/prompt_queues/verification_mvp_2026_08_25.md
```

Older bootstrap, MVP, token-economy, productization, roadmap, architecture, licensing, and evidence queues are historical/supporting context unless a task is explicitly re-promoted into the canonical queue.

## Current known state

Latest known GitHub CI evidence on `main`:

```text
restore: PASS
build: PASS
tests: FAIL
```

Known failing test/root cause:

```text
GitStatusParserTests.Parse_ParsesModifiedAndUntrackedFiles
```

The parser trims the fixed-width git porcelain prefix before slicing the path, corrupting `README.md` to `EADME.md`.

Therefore the current next prompt is not generic build discovery. It is the focused parser/CI hardening prompt.

## Current next prompt

```text
AW-VFY-001 — Git parser and CI hardening
```

Prompt file:

```text
docs/prompts/AW-VFY-001-git-parser-ci-hardening.md
```

## Strict decision tree

```text
Is AW-VFY-001 full test gate green?
  no  -> AW-VFY-001
  yes -> Is CLI smoke/Gate 0 closed?
          no  -> AW-VFY-002
          yes -> Is RunContract v1 proven?
                  no  -> AW-VFY-003
                  yes -> Is start baseline proven?
                          no  -> AW-VFY-004
                          yes -> Is attributable finish delta proven?
                                  no  -> AW-VFY-005
                                  yes -> Is RunReceipt v1 proven?
                                          no  -> AW-VFY-006
                                          yes -> Is Evidence Gate proven?
                                                  no  -> AW-VFY-007
                                                  yes -> Is Scope Drift proven?
                                                          no  -> AW-VFY-008
                                                          yes -> Are initial claims checks proven?
                                                                  no  -> AW-VFY-009
                                                                  yes -> Is 30-run dogfood proof complete?
                                                                          no  -> AW-VFY-010
                                                                          yes -> Is external value proven?
                                                                                  no  -> AW-VFY-011
                                                                                  yes -> Is commercial design-partner value proven?
                                                                                          no  -> AW-VFY-012
                                                                                          yes -> choose post-gate investment from evidence
```

## Non-negotiable ordering rules

Do not implement before their gate:

- Contract features before Gate 0 closes;
- `finish` attribution before start baseline is stable;
- receipt verification before attribution is proven;
- scope/claims checks on raw final git status;
- external/commercial packaging before trustworthy receipts/dogfood;
- learning/router/dashboard/broad integrations before AW-VFY-011 external-value evidence;
- SaaS/auth/billing/team administration before AW-VFY-012 commercial evidence.

## Product guardrails

Do not route current work toward:

- proprietary coding-agent execution;
- generic control-plane/session manager;
- cloud workspace/orchestration;
- generic token/cost dashboard as the primary product;
- generic AI-code tracking as the product wedge;
- SaaS/billing/OAuth before commercial gate;
- visual workflow builder;
- automatic merge/release/deploy.

Current competitive boundary:

```text
Cursor/GitHub/Qodo and adjacent vendors increasingly provide tracking, session/audit and governance surfaces.
AgentsWatch must prove independent cross-vendor verification value, not duplicate native tracking.
```

Canonical addendum:

```text
docs/research/COMPETITIVE_VALIDATION_ADDENDUM_2026_09_16.md
```

## Context rule

Normal prompt selection should require only:

```text
AGENTS.md
-> this router
-> selected prompt file
-> prompt's listed canonical docs
-> exact code/tests
```

Do not load all historical docs or queues.

For AW-VFY-011/012, load the competitive addendum and the reviewed AW-VFY-010 evidence summary in addition to the selected prompt.

## Historical queue rule

If an older queue says `Ready now` but the prompt is not present/promoted in `verification_mvp_2026_08_25.md`, treat that status as superseded.

The canonical queue wins until a newer dated queue explicitly replaces it.
