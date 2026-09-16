# Failure Modes and Kill Criteria

## September 2029 pre-mortem

| Failure | Class | Early warning |
|---|---|---|
| GitHub/Qodo/native vendors fully absorb the gap | STRUCTURAL | high-value scenario gap shrinks quarter by quarter |
| Single-vendor teams do not value neutrality | STRUCTURAL | interviews say native logs are enough |
| Dirty-worktree problem is too niche | STRUCTURAL | real runs rarely encounter it |
| RunReceipt becomes another unread log | AVOIDABLE | reviewers never inspect/use receipt output |
| RunContract duplicates Jira/Linear | AVOIDABLE | setup friction dominates feedback |
| False positives destroy trust | AVOIDABLE | findings routinely ignored/overridden |
| Causal attribution is overstated | AVOIDABLE | disputed actor/change findings |
| OSS usage never converts | STRUCTURAL/AVOIDABLE | installs but no team/commercial interest |
| GitHub Check does not distribute organically | STRUCTURAL/AVOIDABLE | no second-repo/team adoption |
| No clear budget owner | STRUCTURAL | users like it, managers will not fund it |
| Product drifts into generic code review | AVOIDABLE | roadmap duplicates Qodo/CodeRabbit |
| Product drifts into runtime/orchestration | AVOIDABLE | scope/cost explode before PMF |
| Enterprise asks dominate too early | AVOIDABLE | SSO/compliance before decision-value proof |
| Vendor adapters become maintenance treadmill | STRUCTURAL | frequent breaking compatibility work |
| Receipt schema never becomes interoperable | STRUCTURAL | no external consumers/integrations |
| Verification overhead exceeds savings | STRUCTURAL | negative reviewer time economics |
| Semantic acceptance checker overpromises certainty | AVOIDABLE | hallucinated `Done`/false blocks |
| Founder stays in dogfood too long | AVOIDABLE | months pass without external usage |

## 30-day gate

Continue if:
- Gate 0 closes;
- the smallest `contract/start/finish/receipt/evidence` vertical slice works;
- at least one real dogfood finding has incremental value beyond agent summary + Git/CI.

Narrow/stop if attribution semantics cannot be made trustworthy.

## 60-day gate

Continue if:
- ~30 useful structured receipts exist;
- no observed material false attribution;
- repeated evidence/scope/claim catches occur;
- deterministic finding noise is low;
- external recruitment has begun.

## 90-day gate

Commercial continuation requires:
- ≥10 external real evaluations;
- ≥3 decision-changing findings;
- ≥3 continued-use requests;
- at least two agent ecosystems represented where practical;
- ≥3 concrete paid/design-partner commitments, or an equally concrete commercial process already underway.

Otherwise narrow to OSS utility, pivot toward attestation/policy, or stop.

## Six-month gate

Must have:
- repeated external-team usage;
- meaningful recurring revenue or strong contracted pilots;
- an identifiable acquisition channel;
- defensible value versus Qodo/native features.

If not, stop treating AgentsWatch as the primary startup.
