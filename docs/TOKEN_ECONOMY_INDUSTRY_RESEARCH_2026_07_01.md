# Token Economy Industry Research — 2026-07-01

Status: research-backed strategy document  
Scope: AgentsWatch token/context efficiency  
Goal: reduce wasted coding-agent tokens without reducing correctness, validation, safety, or developer control.

## Executive summary

The strongest industry pattern is not "make prompts shorter". The winning pattern is a controlled context pipeline:

```text
classify task -> select context pack -> inspect repo map -> expand only needed files -> keep stable prefix cacheable -> validate -> compact evidence -> measure cost per solved task
```

Large context windows are useful, but they are not a license to load everything. Context can become stale, contradictory, costly, and distracting. AgentsWatch should optimize for "least sufficient fresh context" rather than "maximum context".

## Sources reviewed

- OpenAI prompt caching, compaction, latency and cost optimization docs.
- Anthropic Claude prompt caching and Claude Code memory docs.
- Google Gemini context caching docs.
- Aider repository-map documentation.
- Recent research on prompt caching for long-horizon agents, codebase indexes, AGENTS.md effectiveness, configuration smells, and context rot.
- Prior MathLearning / AgentsWatch planning conversations about skill packs, state ownership, feature profiles, prompt queues, and zero-waste review loops.

## Industry findings translated into AgentsWatch decisions

### 1. Stable-prefix prompt architecture

Industry finding: prompt caches reward exact shared prefixes. Static instructions, tools, schemas, and examples belong at the beginning. Dynamic user request, current diff, timestamps, and tool output belong at the end.

AgentsWatch decision:

- Add a cache-aware prompt skeleton.
- Split prompts into static prefix, selected context pack, validation contract, and variable suffix.
- Detect cache breakers:
  - timestamp in stable prefix;
  - branch SHA in stable prefix;
  - fresh tool output before reusable rules;
  - random ordering of files/rules;
  - generated summaries injected above static instructions.

### 2. Cache block control, not naive full-context caching

Industry finding: caching everything can be worse when volatile tool output is mixed into the cached section. Dynamic tool results should be late and often excluded from the cacheable block.

AgentsWatch decision:

- Define `cache-safe`, `cache-risky`, and `cache-forbidden` prompt sections.
- Build a prompt linter that warns when volatile content appears before the cache boundary.
- Track cache-hit metrics only when the provider exposes them.

### 3. Repo-map before full-file reads

Industry finding: Aider sends a concise map of key files, symbols, types, and call signatures. Recent structural-index research suggests codebase indexes can improve localization and lower cost per solved task for multi-file issues.

AgentsWatch decision:

- Add a repo-map contract based on signatures, imports, exports, public symbols, route names, commands, test names, and dependency edges.
- Do not use repo-map as proof. It is a file-selection layer.
- Full file reads still required before editing.

### 4. Path-scoped rules and skills

Industry finding: concise, path-scoped rules reduce always-loaded context and improve task fit. Prior planning also defined repo-specific skill docs for common task types.

AgentsWatch decision:

- Split agent rules into always-loaded core and path/task-scoped packs.
- Create path-scoped rule bundles for backend auth, Flutter UI tests, evidence repair, and cross-repo docs.
- Add a rule-bloat linter to catch repeated, conflicting, or global-only instructions.
- Use `docs/CONTEXT_PACKS.md` as the registry for skill-like context packs.

### 5. AGENTS.md minimalism

Research finding: repository-level context files can sometimes reduce task success and increase cost when they add unnecessary requirements. Another recent study finds context bloat and conflicting instructions are common smells in agent configuration files.

AgentsWatch decision:

- Keep `AGENTS.md` short and canonical.
- Move long procedures to packs/skills/queues.
- Add `AGENTS.md` smell checks:
  - context bloat;
  - conflicting instructions;
  - lint leakage;
  - skill leakage;
  - outdated path references;
  - runtime claims in docs-only rules.

### 6. Stale-context and context-rot detection

Research finding: AI configuration artifacts can become stale as code evolves. Stale context wastes tokens and can mislead agents.

AgentsWatch decision:

- Add `fresh`, `recent`, `stale`, and `unknown-freshness` labels.
- Require current code/tests to override stale docs.
- Add stale path/reference detector for AGENTS.md, DOCS_INDEX, routers, queues, and run logs.
- Block high-confidence Done based only on stale evidence.

### 7. Context packs instead of general discovery

Industry pattern: successful agent workflows give agents a narrow context contract per task type.

AgentsWatch decision:

Create packs such as:

- `pack.evidence.repair`
- `pack.agentwatch.gate0`
- `pack.backend.auth-risk`
- `pack.flutter.ui-test`
- `pack.cross-repo.standard-sync`
- `pack.review.diff-only`
- `pack.docs.index-sync`
- `pack.performance.hot-path`
- `pack.security.boundary-check`
- `pack.feature-profile.gating`

Each pack defines read-first files, avoid files, max files before expansion, validation defaults, stale-context rules, state ownership requirements, and output mode.

### 8. Progressive disclosure CLI

Industry pattern: do not dump all context by default. Tools should reveal more context step-by-step.

AgentsWatch decision:

Future CLI commands:

```text
agentswatch next
agentswatch run
agentswatch finish
agentswatch report
agentswatch context plan <prompt-id>
agentswatch context pack <pack-name>
agentswatch context map --budget 1000
agentswatch context expand <symbol-or-file>
agentswatch context stale-check
agentswatch context budget
agentswatch tokens report
agentswatch evidence compact
```

Default output must be small. Full details require explicit flags.

### 9. Context budget enforcement

Agents need budgets before they read, not after they waste tokens.

AgentsWatch decision:

Budgets:

| Budget | Max files before expansion | Typical task |
|---|---:|---|
| XS | 2 | queue/status/evidence typo |
| S | 5 | targeted doc/test fix |
| M | 10 | feature fix with tests |
| L | 15+ with reason | audit/refactor |

Crossing the budget requires a scope expansion note in the run log.

### 10. Output-token budget

Industry finding: output tokens often dominate latency. Shorter structured outputs matter.

AgentsWatch decision:

- Add response modes:
  - `brief-done`
  - `review-table`
  - `evidence-row`
  - `full-analysis`
- Default final response should be compact unless the prompt requires detail.
- Prefer tables and IDs over repeated prose.
- Use short schema field names only in machine-readable outputs, not in user-facing docs where clarity matters.

### 11. Predicted-output / patch-aware editing

Industry finding: when much of the output is known, predicted outputs or patch-focused editing can reduce latency and token generation.

AgentsWatch decision:

- Prefer patch/diff plans over regenerated full files.
- For docs, use section-level replacement, not full-file rewrite, when safe.
- For code, require anchors and local diffs.
- Track full-file rewrite events as token-risk events.

### 12. Small-model/static preflight

Industry pattern: not every step needs the largest model. Classification, prompt lint, stale-path checks, and context-pack selection can be static or small-model tasks.

AgentsWatch decision:

Future pipeline:

```text
static checks -> small-model preflight -> expensive coding model -> validator -> compact evidence
```

Use expensive model only for tasks that require reasoning over selected fresh context.

### 13. Retrieval quality over retrieval quantity

Research and practice both point to the same rule: more context is not always better.

AgentsWatch decision:

Track retrieval quality:

- selected files that were actually edited;
- selected files that influenced tests;
- selected files later marked irrelevant;
- stale files opened;
- missing files discovered late;
- time/token spent on broad search.

### 14. Negative context cache

Agents repeatedly reread irrelevant docs. That is avoidable.

AgentsWatch decision:

Run logs should record:

```text
Do not reread next time:
- path: reason
- command: too broad; use X
- doc: superseded by Y
```

Future CLI can warn when an agent tries to reopen known-irrelevant context for the same task class.

### 15. Evidence compaction with source pointers

Compaction is useful only if it preserves source pointers.

AgentsWatch decision:

After every 5-8 important run logs:

- create compact rollup;
- keep originals;
- mark originals as summarized by rollup;
- future agents read rollup first;
- originals are opened only for exact proof.

Each compacted fact needs a pointer back to file/commit/test/log.

### 16. Cost-per-solved-task, not raw token count

Raw token reduction can be harmful if it increases failures.

AgentsWatch decision:

Primary metric should become:

```text
cost per validated completed prompt
```

Secondary metrics:

- input tokens;
- output tokens;
- cached tokens;
- files inspected;
- files changed;
- tests run;
- validation pass rate;
- false Done rate;
- follow-up correction rate.

### 17. Agent configuration smell linter

Research identifies common smells such as context bloat, conflicting instructions, and skill leakage.

AgentsWatch decision:

Add `agentswatch rules lint` future command to detect:

- duplicate rules;
- contradictory rules;
- broad always-on rules that should be path-scoped;
- stale file paths;
- very long AGENTS/CLAUDE/GEMINI files;
- hidden runtime claims in docs;
- command examples that do not match OS/shell.

### 18. Provider-aware prompt profiles

Different providers expose different cache behavior and metrics.

AgentsWatch decision:

Define provider profiles:

- OpenAI: automatic prefix caching, cached token usage, prompt cache key where available, compaction.
- Anthropic: explicit and automatic cache breakpoints, 5-minute / 1-hour TTL options, path-scoped Claude memory/rules.
- Google Gemini: implicit context caching, usage metadata for cache hits, large common content early.
- Local/open-source: no billing cache, but still benefit from smaller context, repo-map, and fewer output tokens.

### 19. Token budgets in prompt queues

Every queue row should include:

```text
Token budget: XS/S/M/L
Context pack: <pack>
Max files before expansion: <n>
Expected output mode: brief-done/review-table/full-analysis
Cache profile: static-prefix yes/no
```

This makes token economy enforceable at prompt-selection time.

### 20. Human-visible cost warnings

AgentsWatch should make waste obvious to the developer before the run.

Example warning:

```text
This prompt will probably inspect 20+ files and break cache because dynamic logs are above stable rules.
Suggested split: evidence repair first, runtime tests second.
```

### 21. State ownership as token filter

Prior MathLearning planning showed that state authority determines which layer should be read first.

AgentsWatch decision:

Before broad discovery, classify the touched state owner:

```text
backend / local-cache / display-only / config / filesystem / git / external-service
```

Examples:

- backend-owned coins/XP/reward settlement -> backend owner path first;
- display-only reward animation -> Flutter/UI owner path first;
- feature flags -> config/command contract first;
- evidence status -> queue row and `.ai/runs` first.

### 22. Feature-profile gating

Prior AgentsWatch feature-selection planning introduced feature packages such as `core`, `reports`, `handoff`, `review`, `risk`, `validation`, `adapters`, `learning`, `lint`, `metrics`, `dogfood`, `dashboard`, `team`, and `cloud`.

AgentsWatch decision:

Do not load disabled/future feature package docs by default. A local/core MVP task should not read dashboard/team/cloud docs unless the prompt explicitly selects that profile.

### 23. Queue lifecycle as token discipline

Prior planning included `agentwatch next/run/finish/report`.

AgentsWatch decision:

Treat lifecycle steps as token controls:

- `next`: choose smallest safe prompt and context pack;
- `run`: execute within pack/budget;
- `finish`: write evidence, token report, negative cache;
- `report`: summarize cost, validation, waste, and next prompt.

### 24. Prompt anatomy plus token fields

Prior prompt quality work required goal, scope, files to inspect, non-goals, invariants, acceptance criteria, test plan, and output format.

AgentsWatch decision:

Add token-specific fields:

```text
Token budget
Context pack
Max files before expansion
Expected output mode
Cache profile
State owner
Feature profile
```

### 25. Batch review as compaction point

Prior batch-review policy after 3-5 important commits should also be used as a token compaction point.

AgentsWatch decision:

After a batch, create a compact rollup and mark older logs as summarized by the rollup. Future agents read rollup first and open originals only for exact proof.

### 26. Zero-waste domain playbook pattern

Prior Access/ERP cutoff planning used a reusable low-waste rule:

```text
clone/report layer first -> validate counts -> preserve base semantics -> change defaults only after proof
```

AgentsWatch decision:

For risky domains, create narrow playbooks that localize changes first. Do not inspect or rewrite base layers until counts/tests prove the need.

## Prioritized implementation roadmap

### Phase 1 — docs/spec, safe now

1. Context packs.
2. Cache-aware prompt skeleton.
3. Stale-context guard.
4. Queue row token-budget fields.
5. AGENTS/DOCS smell checklist.
6. State-ownership token filter.
7. Feature-profile gating rule.

### Phase 2 — CLI after Gate 0

1. Repo-map command.
2. Context planner command.
3. Token report command.
4. Stale-context checker.
5. Rules linter.
6. Feature-profile-aware context planner.
7. Negative-cache warning.

### Phase 3 — dogfood and measurement

1. 3 Flutter runs.
2. 3 backend runs.
3. 3 AgentsWatch runs.
4. Compare baseline vs token-economy flow.
5. Publish only measured claims.

## Practices to avoid

Do not reduce token use by:

- skipping current code inspection;
- skipping tests;
- trusting old docs;
- hiding residual risk;
- removing safety/security checks;
- using tiny prompts that cause uncontrolled tool exploration;
- collapsing all rules into one huge always-loaded file;
- claiming savings from cache without usage data;
- loading entire previous conversations into every prompt;
- using long context windows as a substitute for retrieval quality.

## Best next practical changes

1. Finish `docs/CONTEXT_PACKS.md` dogfood.
2. Create `docs/ai/prompts/CACHE_AWARE_PROMPT_SKELETON.md`.
3. Create `docs/STALE_CONTEXT_GUARD.md`.
4. Add token-budget fields to prompt queue templates.
5. Add `docs/AGENT_CONFIG_SMELL_CHECKLIST.md`.
6. Create dogfood measurement table before claims.
