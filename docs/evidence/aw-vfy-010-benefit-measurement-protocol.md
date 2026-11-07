# AW-VFY-010 — protokol merenja koristi

Datum: 2026-11-07 (Europe/Belgrade)
Status: normativni protokol za buduće merenje; ništa se ovde automatski ne pokreće.
Povezano: `aw-vfy-010-gate5-interim.md`, `aw-vfy-010-competitive-baseline-2026-11-07.md`, ledger.

Cilj nije da se dokaže da je alat tehnički ispravan (to je već uglavnom pokazano gating-om). Cilj je da se utvrdi da li AgentsWatch može da postane **koristan proizvod**.

Pravilo odluke, doslovno: `GO samo kada postoje proverljivi slučajevi promene odluke uz prihvatljiv dodatni trošak; inače NARROW ili STOP.`

Dvanaest postojećih korisnih REAL runova **ne sme** se naknadno preklasifikovati kako bi se ovaj protokol proglasio već ispunjenim.

## 1. Šta je „decision change case“

Decision change case postoji samo kada važi sve ovo:

1. Recenzent (čovek) prvo vidi običan Git diff i/ili CI i/ili PR pregled, bez čitanja AgentsWatch odluke, ili u unapred prijavljenom redosledu gde je Git/CI/PR sloj zabeležen pre AW sloja.
2. Recenzent zabeleži privremenu odluku: merge / ne merge, close / ne close, zahtevati izmenu / prihvatiti kako jeste, ili ekvivalentnu recenzentsku akciju.
3. Tek potom recenzent vidi AgentsWatch receipt / Evidence Gate izlaz (odluka, findings, atribucija, validacije).
4. Recenzent **promeni** tu privremenu odluku, i zabeleži zašto: koji AW nalaz, kod ili atribucija je promenila merge/close odluku.
5. Promena je proverljiva (zapis pre/posle, timestamp, identifikator runa i SHA). Nije dovoljno reći „korisno je bilo“.

Nije decision change case:

- isto tehničko blokiranje (`NeedsReview` / `AWE009`) koje recenzent i ovako ne bi tretirao kao Done;
- nalaz koji je već vidljiv u diff-u, testu ili CI i koji recenzent već namerava da koristi;
- lažni alarm koji recenzent odbaci;
- naknadno prepričavanje starog runa bez unapred prijavljenog očekivanja.

## 2. Pre-registracija očekivanog ishoda

Pre `agentswatch start` (ili bar pre čitanja AW izlaza) zapisati, van receipt-a ako treba, ali sačuvati uz evidenciju:

- svrha zadatka i zašto je legitimni odobreni posao (issue/PR/dokumentovani defekt; ne izmišljen);
- očekivano ponašanje proizvoda i prihvatni kriterijumi;
- očekivana privremena Git/CI/PR odluka ako AW ne postoji;
- očekivani AW izlaz (npr. NeedsReview + AWE009, atribucija određenih putanja);
- da li se očekuje decision change case (da/ne/unknown) i zašto.

Čitanje AW izlaza pre ovog zapisa diskvalifikuje run kao merenje koristi (i dalje sme ostati tehnički REAL red, ali ne ulazi u GO brojač decision change slučajeva).

## 3. Procedura poređenja (sekvencijalno / „slepo“ na AW sloj)

1. Uraditi stvarni odobreni posao u disposable worktree-u.
2. Recenzent pregleda Git diff i dostupni CI/PR kontekst i zabeleži privremenu odluku + vreme (Europe/Belgrade i UTC).
3. Pokrenuti ili prikazati već završen AgentsWatch receipt. Ne sme se paralelno mešati više runova nad istim worktree-om.
4. Recenzent zabeleži konačnu odluku + vreme i da li je odluka promenjena.
5. Klasifikovati run (`REAL` ako je stvaran odobren posao; kontrole ne ulaze u GO brojač koristi).
6. Uporediti sa pre-registracijom: da li je decision change predviđen, iznenađenje, lažni alarm ili potvrda.

„Slepo“ ovde znači: privremena Git/CI/PR odluka se donosi pre čitanja AW odluke. Ne znači sakrivanje koda od recenzenta.

## 4. Kako se meri dodatni trošak

Za svaki run zabeležiti, samo ako je stvarno mereno (inače **Unknown**):

- minuti recenzenta na Git/CI/PR sloj (start/stop timestamp);
- minuti recenzenta na AgentsWatch sloj (čitanje receipta, provera findings);
- minuti setup-a (contract, CLI, worktree) ako nisu deo redovnog posla;
- broj lažnih alarma koje je recenzent morao da odbaci, i vreme na njih;
- ukupan dodatni trošak = AW minuti + setup iznad uobičajenog Git/CI/PR toka.

Timestamp format: lokalno Europe/Belgrade uz navođenje zone, plus originalni izvor ako je u drugoj zoni.

Ako vreme nije mereno časovnikom u tom trenutku, upisati Unknown. Ne sme se pogađati naknadno.

## 5. Prihvatljiv dodatni trošak

**Prag prihvatljivog dodatnog troška: [OWNER-DEFINED — nije postavljen].**

Vlasnik treba da upiše konkretan prag pre GO odluke (npr. najviše N dodatnih recenzentskih minuta po decision change case, ili najviše M lažnih alarma po K runova). Dok prag nije upisan, nijedan skup merenja ne može sam sebe proglasiti „prihvatljivim“.

## 6. GO / NARROW / STOP

Primeniti vlasnikovo pravilo:

- **GO** samo kada postoje proverljivi decision change case-ovi **i** izmereni dodatni trošak je unutar vlasničkog praga.
- **NARROW** ako ima malo (ili usko lokalizovanih) decision change slučajeva, ili ako korist postoji samo u uskom režimu (npr. samo dirty-start atribucija) uz trošak koji vlasnik još može da prihvati u tom užem slučaju.
- **STOP** ako posle dogovorenog broja stvarnih odobrenih zadataka nema proverljivih decision change slučajeva, ili je dodatni trošak neprihvatljiv, ili lažni alarmi potiskuju korist.

Tehnički ispravno blokiranje neverifikovanog Done (`AWE009`) samo po sebi nije GO.

## 7. Klasifikacija runova

- Samo `REAL` runovi na stvarnom odobrenom poslu ulaze u brojač koristi.
- `FIXTURE`, `NEGATIVE_CONTROL`, `REPLAY`, `DUPLICATE` i `UNKNOWN` ne dokazuju korisnost proizvoda.
- Ne izmišljati defekte, dokumentacione izmene ni acceptance rezultate radi broja.
- HOLD stavke (Flutter #5/#6, produkcioni #47 SQL/apply, Fly deploy, cloud agents, SaaS, AW-VFY-011/012) ostaju HOLD dok vlasnik ne kaže drugačije.

## 8. Zabranjeni automatski startovi

Ovaj dokument ne pokreće B5, ne otvara ticket-e, ne merge-uje PR-ove i ne pokreće AW-VFY-011/012. Sledeći mereni run sme da počne samo uz novu, izričitu vlasničku dozvolu i uz već postojeći legitimni posao.
