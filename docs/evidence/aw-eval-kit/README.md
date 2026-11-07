# AW-EVAL kit — indeks

Datum: 2026-11-07 (Europe/Belgrade)
Status: priprema; nijedan pilot run se ovde ne izvršava.

Ovaj komplet **ponovo koristi** metodologiju iz:

- [../aw-vfy-010-benefit-measurement-protocol.md](../aw-vfy-010-benefit-measurement-protocol.md)
- [../aw-vfy-010-gate5-interim.md](../aw-vfy-010-gate5-interim.md)
- [../aw-vfy-010-competitive-baseline-2026-11-07.md](../aw-vfy-010-competitive-baseline-2026-11-07.md)
- [../aw-vfy-010-ledger.md](../aw-vfy-010-ledger.md)

Glavni pripremni izveštaj, inventar kandidata i presuda spremnosti: [../aw-eval-prep-001.md](../aw-eval-prep-001.md)

Radni listovi (prazni šabloni, bez izmišljenih merenja):

1. [01-preregistration-template.md](01-preregistration-template.md)
2. [02-baseline-review-worksheet.md](02-baseline-review-worksheet.md)
3. [03-independent-oracle-worksheet.md](03-independent-oracle-worksheet.md)
4. [04-final-comparison-worksheet.md](04-final-comparison-worksheet.md)
5. [05-seal-manifest-template.md](05-seal-manifest-template.md) — sidecar za SHA-256; heš se ne upisuje u heširani fajl

Redosled (usklađen sa CLI-jem): odobren zadatak → predregistracija, zamrzavanje i remote dokaz → `agentswatch start` pre bilo koje izmene (operator sakriva izlaz od recenzenta) → agent radi zadatak → Git/CI/PR pregled, zamrzavanje baseline odluke i remote dokaz bez AW informacija → `agentswatch finish` i prikaz receipta tek posle tog pečata → poređenje, merenje vremena i nezavisni adjudikator. Lokalni timestamp nije nezavisni dokaz redosleda.
