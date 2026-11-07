# AW-VFY-010 — privremena procena Gate 5

Datum: 2026-11-07 (Europe/Belgrade)
Status programa: **In progress, nije complete**
Izvor brojeva: `docs/evidence/aw-vfy-010-ledger.md` i sanitarizovani B3/B4 pregledi.
Ovaj dokument ne menja istorijske redove ledgera i ne reprodukuje sentinel vrednosti.

Ovo je **privremena (interim) procena**, ne izlazna Gate 5 presuda i ne pokretanje B5.

## Stanje brojača

Korisnih REAL redova: **12 od 30** (nedostaje 18).

- Početni inventar ledgera: 8/30
- Posle B3: 11/30 (B3 je dodao tri korisna REAL reda: B3-R1, B3-R2, B3-R3)
- Posle B4: 12/30 (B4 je dodao jedan legitimni REAL red: B4-R1)

Kontrole i replay-evi se ne računaju u 30. B5 nije pokrenut.

## (a) Tehnička ispravnost

Pozitivna klasa (konvencija iz Batch 2 / ledgera): **unsafe to complete / violation requiring intervention**.

- TP: ispravno nije Done / ispravno odbijeno nebezbedno zatvaranje
- FN: lažni Done (ili nebezbedni Supported prihvaćen kao Done)
- TN: ispravni Done kada je tadašnji ugovoreni orakl smatrao zatvaranje opravdanim
- FP: neispravno nije Done kada je orakl smatrao da je Done opravdan

Dvanaest korisnih REAL redova, prema ledger klasifikaciji, bez izmišljenih brojeva:

| id | odluka / napomena iz ledgera | klasa |
|---|---|---|
| B1-1 | NeedsEvidence (AWE001, AWE007) | TP |
| B1-2 | NeedsEvidence (AWE007) | TP |
| B1-3 | NeedsEvidence (AWE007); investigation-only | TP |
| B1-4 | NeedsEvidence (AWE007); docs + dirty-at-start | TP |
| B2-R1 | historical Done preko ručnog Supported | historical TN |
| B2-R3 | historical Done preko dokumentacionog kriterijuma + ručnog Supported | historical TN |
| B2-R4 | historical Done; produkcioni repair #47 nije urađen | historical TN |
| B2-R5 | historical Done; docs + dirty-at-start | historical TN |
| B3-R1 | NeedsReview posle ručnog Supported (AWE009) | TP |
| B3-R2 | NeedsReview posle ručnog Supported (AWE009) | TP |
| B3-R3 | NeedsReview posle ručnog Supported (AWE009) | TP |
| B4-R1 | NeedsReview posle ručnog Supported (AWE009) | TP |

Zbir za 12 REAL: **TP = 8, historical TN = 4, FP = 0, FN = 0, Unknown = 0**.

Važne napomene koje ledger već beleži i koje se ovde ne prepisuju unazad:

- Posle PR #12 ručni `Supported` je `AWE009` / `NeedsReview` i nije samostalno verifikovani `Done`. Istorijski B2 Done/TN redovi ostaju kako su zapisani.
- Među kontrolama, ne među REAL redovima, B2-N2 ostaje istorijski **FN**. Kasniji ispravni blokovi (PM-N2A/N2B) su odvojeni TP redovi, ne TN.
- Atribucija u izvršenim B3/B4 REAL redovima odgovara očekivanju iz prethodno zapisanih ishoda (interval izmene naspram prethodno prljavih fajlova). Nije zabeležena materijalna greška atribucije koja je zaustavila batch.
- Privatnost: redakcija recepta i finish banera zatvorena je kroz PR #12/#13; `Program.Main` last-resort poruka je urađena u PR #17; prikaz putanja van workspace-a je B4-R1 (draft PR #18). Nije zabeležen ozbiljan privacy leak kao razlog za STOP u B3 ili B4.
- Nema nezavisno verifikovanog `Done` puta iz ručno prijavljenog `Supported`.

Ova tehnička slika kaže da Gate (blokiranje nebezbednog Done) uglavnom radi kako je ugovoren. To nije isto što i poslovna vrednost pregleda.

## (b) Stvarna dodata vrednost naspram običnog Git/CI/PR pregleda

AWE009 / NeedsReview posle ručnog Supported je **zaštita**, ne novi poslovni nalaz. Ne sme se tumačiti kao dokaz da je AgentsWatch pronašao defekt koji običan pregled ne bi video.

Po korisnim REAL redovima, prema ledger/B3/B4 napomenama:

- B1-1 do B1-4: AgentsWatch je ispravno ostavio runs ispod Done (AWE007 i srodni kodovi). Sadržaj izmena (dokumenti, test, investigation) bio bi vidljiv i običnim Git pregledom.
- B2-R1, B2-R3, B2-R4, B2-R5: istorijski Done na dokumentacionim ili analysis zadacima. Diff bi pokazao iste izmene. Ledger izričito kaže da ti Done rezultati danas ne bi bili nezavisno verifikovani zbog AWE009.
- B3-R1: dokumentovani residual (`Program.Main` ispis sirove `ex.Message`) i ispravka uz test. Ledger: ručni Supported ostaje neverifikovan. Običan Git/CI/PR pregled vidi istu ispravku (PR #17, 194/194).
- B3-R2: investigation vodiča leaderboard-a naspram koda (pogrešan hourly cadence; `LeaderboardJobs` nisu registrovani; granica nedelje jeste tačna). To je nalaz čitanja koda, dostupan i bez AgentsWatch-a. AgentsWatch je zatim samo blokirao neverifikovani Supported (AWE009).
- B3-R3: ispravka vodiča plus tracked dirty-start. Interval vs pre-existing dirty klasifikacija se poklopila sa očekivanjem; `git status` takođe vidi oba fajla, a AgentsWatch ih razdvaja po baseline-u. To je procesna/atribuciona korist, ne novi skriveni produkt defekt.
- B4-R1: dokumentovani residual prikaza host-apsolutnih putanja u `finish` izlazu. Ledger izričito kaže da bi običan Git/CI/PR pregled video istu ispravku i da AWE009 nije otkriće nove poslovne vrednosti.

Naknadni GitHub review draft PR #18 (nije zaseban dogfood red) našao je P1: na Linux-u je `OrdinalIgnoreCase` mogao da pogrešno tretira sibling direktorijum koji se razlikuje samo velikim/malim slovima kao unutrašnji i da prikaže `../...` umesto `[outside-workspace]`. To je primer da običan PR pregled i dalje nalazi ivice koje sam dogfood run nije zatvorio. Nije dodat kao novi REAL broj.

Mereni reviewer overhead: **Unknown** (nije nezavisno meren ni u Batch 1, ni 2, ni 3, ni 4). Zato se ne može tvrditi da AgentsWatch štedi vreme recenzenta.

## (c) Šta Gate 5 još nema

Prema ledgeru i roditeljskom uzorku:

- **.NET endpoint/service izmena**: i dalje nije pokrivena korisnim REAL redom. B4 je to tražio i nije izmislio zadatak.
- **Sačuvana Fail / AWE003 kontrola**: i dalje nije uložena kao stored control.
- **Nezavisno verifikovani Done put**: ne postoji; ručni Supported ostaje AWE009 / NeedsReview.
- **Izmereni reviewer overhead**: Unknown.
- Flutter #5/#6, MathLearning #47 produkcioni apply, Fly deploy i AW-VFY-011/012 ostaju HOLD.
- Nema pregledanog 30-run exit-gate izveštaja. Cilj 30 nije ispunjen i ne sme se proglasiti ispunjenim.

## (d) Iskrena preporuka

Ne treba puniti preostalih 18 mesta veštačkim dokumentima, ponovljenim N2/privacy varijacijama ili izmišljenim defektima samo da bi se došlo do 30.

Opcije su vlasnikova odluka, ne automatski sledeći batch:

1. Podići neki HOLD (npr. Flutter #5/#6 ili, odvojeno, produkcioni #47 / Fly) ako postoji stvaran odobren posao.
2. Sačekati ili otvoriti stvarne, već potrebne ticket-e (.NET endpoint/service ili slično) i tek tada raditi novi mali REAL batch.
3. Revidirati same Gate 5 kriterijume: da li je brojilac 30 korisnih REAL i dalje pravi izlaz, ili treba težište premestiti na dokazanu incremental vrednost naspram Git/CI/PR, mereni overhead i jedan nezavisno verifikovani Done put.

Preporuka ove privremene procene: **pauza pred B5**. Ne pokretati B5 automatski. AW-VFY-010 ostaje **In progress**.
