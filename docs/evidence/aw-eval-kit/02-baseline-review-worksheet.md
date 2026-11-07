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

Da li bi recenzent i bez AgentsWatch-a već video isti nalaz u diff-u ili CI? (da/ne/unknown)

## 3. Pečat (pre bilo kog AgentsWatch nalaza)

Postupak:

1. Sačuvati ovaj fajl kao potpun (odluka + obrazloženje + timestampi).
2. Izračunati SHA-256 kanonskog sadržaja fajla (UTF-8, kako je sačuvan).
3. Poželjno: commitovati **samo** ovaj list (i predregistraciju) u evidencioni zapis, pre otvaranja receipta. Git commit daje nezavisni timestamp.
4. Upisati heš ispod. Tek tada sme da se otvori AgentsWatch izlaz.

| Polje | Upis |
|---|---|
| SHA-256 pečata | |
| Način pečata (fajl heš / git commit SHA) | |
| Git commit SHA pečata, ako postoji | |
| Potvrda: AgentsWatch findings još nisu čitani | da / ne |

Ako je AgentsWatch izlaz viđen pre pečata, run je **kontaminiran**. Može ostati tehnički zabeležen, ali **ne ulazi** u GO brojač decision change slučajeva.
