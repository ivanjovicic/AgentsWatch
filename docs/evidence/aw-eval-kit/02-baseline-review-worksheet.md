# Baseline Git/CI/PR pregled (pre prikaza AgentsWatch receipta)

Ovaj list je **zapečaćeni** zapis privremene odluke posle izvršenog zadatka. `agentswatch start` je smeo i mora da se desi **ranije**, pre izmena koda; recenzent ipak **ne sme** da vidi start izlaz, dirty-at-start brojeve, putanju ili sadržaj baseline fajla, findings ili receipt.

Posle pečata se list ne sme menjati. Ako treba ispravka slova, napraviti novi list sa novim pečatom i objasniti razlog; stari ostaje sačuvan.

## 1. Kontekst

| Polje | Upis |
|---|---|
| Eval run id (isti kao u predregistraciji) | |
| Recenzent | |
| Početak pregleda (Europe/Belgrade) | |
| Kraj pregleda (Europe/Belgrade) | |
| Trajanje baseline pregleda (minuti, samo ako su oba timestampa stvarna) | |
| Repozitorijum / PR ili diff | |
| SHA pregledanog koda | |
| CI run id i ishod, ako je viđen | |
| Šta je recenzent stvarno video (diff, testovi, PR komentari, CI) | |

Ako vreme nije mereno časovnikom u tom trenutku, upisati **Unknown**. Ne nagađati naknadno.

## 2. Privremena odluka (obavezna pre pečata)

Jedna od:

- merge / odobriti
- ne merge-ovati
- zahtevati izmenu
- zatvoriti / ne zatvoriti
- druga ekvivalentna recenzentska akcija: ________

Obrazloženje na osnovu **samo** Git/CI/PR (bez AW):

```text

```

Nalazi koje je recenzent **stvarno uočio i ovde zapisao** (konkretno; prazno = ništa nije zapisano kao uočeno). Samo ovo kasnije važi kao „već viđeno u baseline-u“. Nagađanje šta je recenzent *mogao* da primeti čitanjem diff-a ovde se ne upisuje.

```text

```

## 3. Zamrzavanje i pečat (pre `agentswatch finish` i pre prikaza receipta)

Ovaj list posle zamrzavanja se ne sme menjati. Ako treba ispravka, napraviti novu verziju; stara ostaje sačuvana.

Potvrde recenzenta (obavezno `da`):

- [ ] Nisam video izlaz `agentswatch start` (uključujući „Dirty at start“ / „Worktree clean at start“).
- [ ] Nisam video AW baseline metapodatke, findings ni receipt.

Postupak:

1. Popuniti odluku, obrazloženje, stvarno uočene nalaze i timestamp-e. **Ne upisivati heš u ovaj fajl.**
2. Sačuvati fajl i prestati da ga menjate (zamrznuti bajtovi).
3. Izračunati SHA-256 tih tačnih bajtova (UTF-8, kako je sačuvan).
4. Upisati putanju i heš u zaseban [05-seal-manifest-template.md](05-seal-manifest-template.md) kao pečat vrste `baseline-pregled`. Adjudikator kasnije hešira **originalne zamrznute bajtove**, nikad fajl izmenjen posle računanja heša.
5. Objaviti remote dokaz (git commit gurnut na remote / PR timeline ili drugi potpisan remote zapis). Tek posle toga operator sme da pokrene `agentswatch finish` i da prikaže receipt. Podrazumevano se `finish` čeka do ovog trenutka.

Lokalni timestamp u odeljku 1 i lokalni (nepohranjeni) git commit **nisu** nezavisni dokaz redosleda: oboje se mogu nazadovati. Remote push dokazuje kada je ovaj list postao remote-vidljiv; sam ne dokazuje kada je lokalni `finish` izvršen, osim uz pouzdan operativni redosled.

Ako je recenzent video start izlaz ili receipt pre ovog pečata, run je **kontaminiran**. Može ostati tehnički zabeležen, ali **ne ulazi** u GO brojač decision change slučajeva.
