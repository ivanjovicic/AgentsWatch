# Baseline Git/CI/PR pregled (pre AgentsWatch nalaza)

Ovaj list je **zapečaćeni** zapis privremene odluke. Posle pečata se ne sme menjati.
Ako treba ispravka slova, napraviti novi list sa novim pečatom i objasniti razlog; stari ostaje sačuvan.

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

## 3. Zamrzavanje i pečat (pre bilo kog AgentsWatch nalaza)

Ovaj list posle zamrzavanja se ne sme menjati. Ako treba ispravka, napraviti novu verziju; stara ostaje sačuvana.

Postupak:

1. Popuniti odluku, obrazloženje, stvarno uočene nalaze i timestamp-e. **Ne upisivati heš u ovaj fajl.**
2. Sačuvati fajl i prestati da ga menjate (zamrznuti bajtovi).
3. Izračunati SHA-256 tih tačnih bajtova (UTF-8, kako je sačuvan).
4. Upisati putanju i heš u zaseban [05-seal-manifest-template.md](05-seal-manifest-template.md). Adjudikator kasnije hešira **originalne zamrznute bajtove**, nikad fajl izmenjen posle računanja heša.
5. Za dokaz da je pečat nastao **pre** AgentsWatch izlaza potreban je proverljiv redosled: git commit **gurnut na remote** (vidljiv u PR timeline-u) ili drugi potpisan / na remote-u zabeležen zapis. Tek posle toga sme da se generiše ili otvori AgentsWatch izlaz.

Lokalni timestamp u odeljku 1 i lokalni (nepohranjeni) git commit **nisu** nezavisni dokaz redosleda: oboje se mogu nazadovati.

Ako je AgentsWatch izlaz viđen ili generisan pre pečata i remote dokaza redosleda, run je **kontaminiran**. Može ostati tehnički zabeležen, ali **ne ulazi** u GO brojač decision change slučajeva.
