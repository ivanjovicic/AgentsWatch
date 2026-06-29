# AgentsWatch Release and Packaging Plan

Last aligned: 2026-06-29  
Status: draft, blocked until Gate 0 passes

## Purpose

Define how AgentsWatch becomes installable without starting SaaS or dashboard work too early.

## Prerequisite

Do not run release/packaging work until Gate 0 is complete:

- restore/build/test verified;
- CLI smoke verified;
- risk register updated.

## Release stages

### Stage 0 — Local dev run

Command:

```bash
dotnet run --project src/AgentsWatch.Cli -- --help
```

Purpose: prove command behavior during development.

### Stage 1 — Local pack

Command:

```bash
dotnet pack src/AgentsWatch.Cli/AgentsWatch.Cli.csproj --configuration Release
```

Purpose: prove the CLI can be packaged.

### Stage 2 — Local tool install

Example:

```bash
dotnet tool install --global --add-source ./artifacts AgentsWatch.Cli
```

Exact package output path must be confirmed after `dotnet pack` runs.

### Stage 3 — GitHub release draft

Only after local install works.

Release assets later:

- NuGet package;
- checksums;
- release notes;
- example reports;
- installation instructions.

### Stage 4 — NuGet publish

Only after dogfood evidence.

Do not publish until:

- CLI commands are stable;
- no-overwrite behavior is tested;
- privacy rules are documented;
- versioning policy exists.

## Versioning draft

Before public release:

```text
0.1.0 — bootstrap CLI MVP
0.2.0 — prompt optimizer and task split
0.3.0 — run reports and handoff
0.4.0 — diff-only review and claims-vs-actual
0.5.0 — dogfood-ready local CLI
1.0.0 — stable local CLI contract
```

## Release checklist

- [ ] build passes;
- [ ] tests pass;
- [ ] CLI smoke passes;
- [ ] package created;
- [ ] local tool install tested;
- [ ] docs updated;
- [ ] changelog updated;
- [ ] security/privacy reviewed;
- [ ] known risks documented.

## Release notes template

```markdown
# AgentsWatch <version>

## Highlights

- 

## Commands changed

- 

## Validation

- `dotnet build`: pass/fail
- `dotnet test`: pass/fail
- CLI smoke: pass/fail

## Known risks

- 

## Upgrade notes

- 
```
