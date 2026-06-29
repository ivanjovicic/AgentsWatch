# AgentsWatch Project Readiness Checklist

Last aligned: 2026-06-29

Use this checklist before moving from bootstrap to feature expansion.

## Gate 0 — Bootstrap

- [ ] `dotnet restore AgentsWatch.sln` recorded.
- [ ] `dotnet build AgentsWatch.sln` recorded.
- [ ] `dotnet test AgentsWatch.sln` recorded.
- [ ] CLI `--help` smoke recorded.
- [ ] CLI `--version` smoke recorded.
- [ ] CLI `optimize` smoke recorded.
- [ ] CLI `status` smoke recorded.
- [ ] CI evidence recorded or explicitly unavailable.
- [ ] `docs/RISK_REGISTER.md` updated.

## Gate 1 — CLI foundation

- [ ] `agentswatch init` tested in temp directory.
- [ ] Existing files are not overwritten by default.
- [ ] `agentswatch status` handles clean and dirty git repos.
- [ ] `agentswatch optimize` handles broad and scoped prompts.
- [ ] Output folders and report paths are documented.

## Gate 2 — Product contracts

- [ ] Config schema documented.
- [ ] Report formats documented.
- [ ] Data model documented.
- [ ] Adapter responsibilities documented.
- [ ] Security/privacy rules documented.
- [ ] Test matrix documented.

## Gate 3 — Dogfood readiness

- [ ] At least one real run report exists.
- [ ] At least one handoff summary exists.
- [ ] At least one diff-only review prompt exists.
- [ ] At least one missed-test or scope risk was checked.
- [ ] Dogfood notes are saved.

## Stop rule

Do not start dashboard, SaaS, GitHub integration, or billing until the checklist proves CLI value.
