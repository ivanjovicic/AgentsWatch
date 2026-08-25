# AgentsWatch Bootstrap Validation Queue

Last aligned: 2026-08-25  
Status: **superseded historical queue**

This file is retained for audit/history only.

The previous `AW-VAL-001 -> AW-VAL-004` bootstrap sequence was written before current CI evidence existed. Current known evidence already shows restore/build pass and a focused GitStatusParser test failure.

## Current authority

Use:

```text
docs/prompt_queues/PROMPT_QUEUE_ROUTER.md
docs/prompt_queues/verification_mvp_2026_08_25.md
```

Current next work is:

```text
AW-VFY-001 — Git parser and CI hardening
AW-VFY-002 — CLI smoke and Gate 0 closure
```

Do not select the historical AW-VAL prompts merely because an older run/document references them as `Ready`.

## Historical mapping

| Old prompt | Current handling |
|---|---|
| AW-VAL-001 build validation | Superseded by focused AW-VFY-001 using existing CI evidence. |
| AW-VAL-002 CLI smoke | Replaced by AW-VFY-002. |
| AW-VAL-003 evidence review | Gate review is incorporated into AW-VFY-002 completion evidence. |
| AW-VAL-004 init hardening | Any still-missing init behavior belongs inside AW-VFY-002 smoke/hardening scope or a targeted follow-up if discovered. |

Historical prompt files may remain unchanged as evidence of earlier planning.
