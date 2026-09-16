# Claims and Evidence Verification

## Deterministic claim classes

| Claim | Verification strength | MVP value |
|---|---|---|
| `TestsAdded` | mostly deterministic from attributable test-file delta | medium |
| `DocsOnly` | deterministic with configured path/file classes | high |
| `BackendUnchanged` | deterministic with configured backend boundaries | high |
| `MigrationAdded` | mostly deterministic by path/pattern | medium |
| `ValidationPassed` | deterministic only with trusted validation evidence | **very high** |
| `NoUnrelatedChanges` | partial; depends on contract scope | high |
| `BugFixed` | not generally deterministic | low for deterministic MVP |
| `AcceptanceCriteriaCompleted` | partial/semantic | high value, but must allow `UNKNOWN` |

## Highest-value check

`ValidationPassed` is especially valuable because agent prose is not evidence.

Possible evidence provenance:
- command executed by AgentsWatch;
- CI result;
- imported structured result from a tool/vendor;
- user declaration, explicitly labeled lower-trust;
- unknown.

If the contract required integration tests and only unit tests ran, the run should not become `Done` merely because the executing agent said “tests pass”.

## Semantic claims

“Bug fixed” and broad acceptance criteria cannot generally be proven from a Git diff.

Do not turn another LLM’s opinion into a fact. Use:
- deterministic facts;
- evidence references;
- optional semantic advisory analysis;
- `Supported / Unsupported / Unknown`.

## Commercial test

A claim check matters if it changes a real decision, for example:

> “The run claims completion, but required validation evidence is missing, so do not merge yet.”

It matters much less if it merely restates something already obvious in Git.
