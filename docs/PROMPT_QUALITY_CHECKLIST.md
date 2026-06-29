# AgentsWatch Prompt Quality Checklist

Last aligned: 2026-06-29

Use before adding or running an AgentsWatch prompt.

## Must have

- One repository only.
- One prompt ID only.
- Assigned queue file.
- One clear task.
- Run mode.
- Token budget.
- Exact docs/files to inspect.
- Scope limiter.
- Owned paths.
- Avoid paths.
- Non-goals.
- Stop rules.
- Exact validation commands.
- Shell-neutral commands by default.
- Final response format.
- Completion percentage rule.
- Missed/follow-up/residual risk rule.

## AgentsWatch-specific checks

- Bootstrap validation status is known.
- Roadmap gate is named if feature work is requested.
- Config/report/data-model contract is referenced when relevant.
- Security/privacy guide is referenced when reports, config, or command output may include sensitive data.
- Test matrix is referenced when CLI behavior changes.
- Adapter spec is referenced when project detection or validation suggestions change.

## Good prompt questions

A good prompt answers:

1. Which repo is edited?
2. Which queue owns it?
3. Which gate allows it?
4. What is the run mode?
5. What is the token budget?
6. What is in scope?
7. What is out of scope?
8. Which tests or validation prove it?
9. What counts as Done?
10. What should become follow-up instead of hidden missed work?

## Red flags

Reject or split prompts that:

- ask to analyze everything;
- mix several run modes;
- skip validation;
- skip bootstrap gates;
- start dashboard/SaaS too early;
- do not name owned paths;
- do not name avoid paths;
- claim Done without evidence;
- ask for a whole-repo review when a diff-only review is enough;
- rely on long chat history instead of a handoff.

## Ready to run

A prompt is ready when it is short, scoped, testable, has clear ownership, names relevant docs, and includes exact validation/evidence rules.

A prompt is not Done until validation is reported honestly, skipped validation has residual risk, and the result is committed or the blocker is documented.
