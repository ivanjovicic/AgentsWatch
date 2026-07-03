# AgentsWatch MVP Proof Epic Addendum

Last aligned: 2026-07-03  
Status: mandatory addition to `MVP_EPICS_AND_ACCEPTANCE.md`

## Epic 9 — Capability proof and release trust

Goal: prove which AgentsWatch features work at a specific commit and prevent roadmap/spec claims from being mistaken for shipped behavior.

Prerequisites:

- Gate 0 workflow can execute;
- capability registry and traceability matrix exist;
- current command contracts are stable enough for scenarios.

Stories:

- maintain stable capability IDs and L0-L6 maturity;
- map acceptance criteria to tests/scenarios;
- run cross-platform restore/build/test/smoke;
- retain TRX and CLI transcripts;
- create package/checksum/clean-install proof;
- validate proof manifest and maturity;
- add safety/privacy negative tests;
- run paired dogfood benchmarks for value claims;
- perform independent verification before stable release;
- certify/downgrade public claims.

Acceptance criteria:

- every advertised feature has one registry row;
- every implemented feature has an implementation path and acceptance criteria;
- current commands have happy/failure black-box scenarios;
- Linux and Windows CI artifacts exist for the tested commit;
- package proof has checksum and isolated install smoke;
- proof manifest commit equals tested/package commit;
- failed/skipped stages remain visible;
- README/release claims do not exceed maturity;
- percentage savings claims are absent until benchmark gate passes;
- proof gaps create discovery/follow-up IDs.

Exit:

- Gate 0 evidence updated from actual CI;
- current skeleton capabilities have truthful maturity;
- proof bundle review passes or records exact blockers;
- stable release remains blocked until independent verification and required L6 evidence exist.
