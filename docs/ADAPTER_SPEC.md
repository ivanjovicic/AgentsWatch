# AgentsWatch Adapter Spec

Last aligned: 2026-06-29  
Status: draft contract

## Purpose

Adapters should make AgentsWatch useful across stacks without turning the MVP into a deep static analyzer.

Universal git/file behavior comes first. Stack adapters should only add detection, risk categories, and validation suggestions.

---

## Adapter responsibilities

Each adapter may provide:

- project detection;
- common validation command suggestions;
- high-risk file patterns;
- likely test folders;
- report labels;
- command safety notes.

Adapters should not execute commands by default unless the user explicitly requests validation.

---

## Universal adapter

Always available.

Detects:

- git repository;
- changed files;
- clean/dirty worktree;
- untracked files.

Suggested commands:

```bash
git status --short -uall
git diff --stat
git diff
```

High-risk patterns:

- many files changed;
- no tests changed with runtime changes;
- config/secrets changed;
- generated files changed;
- binary files changed.

---

## .NET adapter

Detects:

- `*.sln`;
- `*.csproj`;
- `*.cs`;
- `Directory.Build.props`;
- `Directory.Packages.props`.

Suggested validation:

```bash
dotnet restore
dotnet build
dotnet test
```

High-risk patterns:

- `Migrations/`;
- auth/security files;
- dependency injection composition;
- public API contracts;
- configuration files;
- project/solution references.

---

## Flutter adapter

Detects:

- `pubspec.yaml`;
- `lib/`;
- `test/`;
- `*.dart`.

Suggested validation:

```bash
flutter analyze
flutter test
```

High-risk patterns:

- providers/state;
- navigation/router;
- storage/offline;
- platform files;
- `pubspec.yaml`;
- localization files;
- widget tests missing for UI changes.

---

## React/TypeScript adapter

Detects:

- `package.json`;
- `src/`;
- `*.ts`;
- `*.tsx`;
- `vite.config.ts`;
- `tsconfig.json`.

Suggested validation:

```bash
npm run build
npm test
npm run lint
```

High-risk patterns:

- API clients;
- route guards;
- auth state;
- package changes;
- environment files;
- generated clients.

---

## Python adapter

Detects:

- `pyproject.toml`;
- `requirements.txt`;
- `*.py`;
- `tests/`.

Suggested validation:

```bash
pytest
ruff check .
mypy .
```

High-risk patterns:

- migrations;
- auth/security;
- dependency changes;
- background jobs;
- IO/network wrappers.

---

## Node adapter

Detects:

- `package.json`;
- `*.js`;
- `*.mjs`;
- `*.cjs`.

Suggested validation:

```bash
npm test
npm run build
npm run lint
```

---

## Adapter output shape

```text
Detected project types:
- dotnet
- react

Suggested validation:
- dotnet build
- dotnet test
- npm run build

Risk patterns matched:
- appsettings file changed
- API client changed without tests
```

---

## MVP rule

Do not block command output just because adapter detection is uncertain. Report `Unknown` and fall back to universal git behavior.
