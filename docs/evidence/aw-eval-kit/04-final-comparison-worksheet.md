# Konačno poređenje (posle pečata baseline pregleda i posle `finish`)

Otvoriti tek **posle** remote dokaza baseline pečata i tek pošto operator pokrene `agentswatch finish` i prikaže receipt. Incremental AW minuti se mere od tog trenutka, ne od skrivenog `start`-a.

| Polje | Upis |
|---|---|
| Eval run id | |
| Početak AW pregleda (Europe/Belgrade) | |
| Kraj AW pregleda (Europe/Belgrade) | |
| Incremental AW minuti recenzenta (samo ako su timestampi stvarni) | |
| Setup minuti (CLI, contract, worktree) iznad uobičajenog Git/CI/PR toka | |
| Setup + AW minuti (zbir; Unknown ako je bilo koji sabirak Unknown) | |
| AgentsWatch odluka / kodovi / ključni nalazi (bez reprodukcije sentinela) | |
| Receipt id, ako postoji | |
| Konačna recenzentska odluka | |
| Da li se razlikuje od zapečaćene privremene odluke? | da / ne |
| Koji tačan AW nalaz je promenio odluku? | |
| Klasifikacija tog nalaza (`ALREADY_NOTICED_IN_BASELINE` / `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED` / `NEWLY_SURFACED`) | |
| Ako je uočen u baseline-u, ne brojiti kao incremental AW vrednost | |
| Ako je samo otkriviv u diff-u a nije bio uočen, ne brojiti automatski kao decision change — čeka adjudikatora | |
| Materijalni lažni alarmi u ovom runu (broj; 0 ako nijedan) | |
| False Done / curenje / pogrešna atribucija? | ne / da (STOP) / unknown |

Ako vreme nije mereno časovnikom, polje je **Unknown**. Naknadno pogađanje nije dozvoljeno.

Privremena odluka (prepisati iz pečata, ne menjati pečat):

```text

```

Konačna odluka i zašto:

```text

```

Klasifikacija ovog runa za brojač koristi: `REAL` / nije podoban (kontrola, kontaminacija, HOLD, istorijski reuse, nije nezavisno).
