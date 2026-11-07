# Predregistracija evaluacionog runa

Popuniti **pre** `agentswatch start` i pre čitanja bilo kog AgentsWatch nalaza.
Ako je AW već završen, ovaj list i baseline list se i dalje popunjavaju i pečate pre otvaranja receipta.

Jezik zapisa: srpski (latinica) ili engleski, konzistentno unutar runa.
Vremenska zona zapisa: Europe/Belgrade, uz UTC ako izvor nije lokalni.

| Polje | Upis |
|---|---|
| Eval run id (novi, nije jedan od 12 istorijskih REAL id-jeva) | |
| Datum predregistracije | |
| Predložio / operator CLI | |
| Recenzent (čovek koji daje Git/CI/PR odluku) | |
| Planirani adjudikator (mora biti druga osoba od recenzenta) | |
| Repozitorijum i tačan URL stavke | |
| SHA / grana / issue ili PR broj | |
| Zašto je ovo legitimni već postojeći posao (ne izmišljen) | |
| Vlasnik je izričito odobrio ovaj run? (da/ne + id poruke) | |
| Status iz inventara (`READY_FOR_OWNER_REVIEW` itd.) | |
| Da li je stavka HOLD? Ako da, run je zabranjen dok se HOLD ne podigne | |
| Da li je isti diff već brojan među 12 istorijskih REAL runova? | |
| Svrha zadatka i prihvatni kriterijumi | |
| Očekivano ponašanje bez AgentsWatch-a (privremena Git/CI/PR odluka) | |
| Očekivani AgentsWatch izlaz (odluka, kodovi, atribucija) | |
| Očekuje li se decision change case? (da / ne / unknown) i zašto | |
| Klasifikacija runa (`REAL` jedino ulazi u GO brojač) | |

Potvrde pre starta (obavezno `da`):

- [ ] Nije jedan od 12 istorijskih korisnih REAL runova i ne koristi njihov isti posao za novi GO broj.
- [ ] Nije HOLD stavka.
- [ ] Nije preuzet tuđi claim / tuđi PR / tuđi queue lock.
- [ ] Adjudikator nije ista osoba kao recenzent. Ako druga osoba ne postoji, run se ne sme tretirati kao nezavisno potvrđen.
- [ ] Niko još nije otvorio AgentsWatch receipt / findings za ovaj run.

SHA-256 ovog popunjenog lista (posle čuvanja, pre AW pregleda):

```text
(upisati hex)
```
