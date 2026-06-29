# AgentsWatch Examples Catalog

Last aligned: 2026-06-29  
Status: examples plan and placeholders

## Purpose

Examples should prove the product workflow better than abstract docs.

Examples must not include private code, secrets, tokens, or client data.

## Planned examples

```text
examples/rough-prompts/broad-repo-fix.md
examples/optimized-prompts/broad-repo-fix-split.md
examples/run-reports/sample-run-report.md
examples/handoffs/sample-handoff.md
examples/diff-review-prompts/sample-diff-review.md
examples/token-waste-reports/sample-token-waste-report.md
```

## Example workflow

1. Start with a rough prompt.
2. Run `agentswatch optimize`.
3. Generate split task prompts.
4. Run an agent with the investigation prompt.
5. Save run report.
6. Generate handoff.
7. Generate diff-only review prompt.
8. Record what was saved/caught.

## Example quality rules

Each example should show:

- input;
- output;
- why the output is safer;
- validation status;
- residual risk;
- next prompt.

## Do not include

- real secrets;
- private code;
- full proprietary diffs;
- raw long chat transcripts;
- unsupported product claims.

## First examples to create

### Example 1 — Broad repo fix split

Shows how one unsafe prompt becomes four safe prompts.

### Example 2 — Validation evidence missing

Shows why `Done` is not allowed without build/test/smoke evidence.

### Example 3 — Tests claimed but not changed

Shows claims-vs-actual review catching a mismatch.

### Example 4 — Handoff reuse

Shows a compact handoff replacing long chat history.
