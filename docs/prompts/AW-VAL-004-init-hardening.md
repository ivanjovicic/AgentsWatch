# AW-VAL-004 — Init command hardening

Run mode: implementation  
Token budget: low

## Prerequisite

Build validation is complete or remaining failures are documented.

## Task

Harden `agentswatch init` without adding unrelated CLI features.

## Owned paths

- `src/AgentsWatch.Cli/`
- `tests/AgentsWatch.Tests/`

## Required behavior

- creates expected local folders;
- does not overwrite existing user files;
- writes config/status/changelog/review checklist;
- works with temporary test directories;
- handles existing folders gracefully.

## Validation

- targeted init tests;
- solution build.

## Stop rules

Stop if this requires changing unrelated CLI commands, git logic, reports, or future dashboard scope.
