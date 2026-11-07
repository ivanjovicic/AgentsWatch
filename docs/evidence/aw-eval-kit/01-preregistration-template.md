# Predregistracija evaluacionog runa

Popuniti **pre** `agentswatch start` i pre bilo koje izmene koda.
`start` mora da uhvati početni worktree; zato se ne pokreće posle izmena. Posle zamrzavanja ove predregistracije i njenog remote dokaza operator sme da pokrene `start`, ali taj izlaz ostaje sakriven od baseline recenzenta.

Podrazumevano se `agentswatch finish` **ne** pokreće dok recenzent ne zapečati baseline Git/CI/PR odluku. Ne završavati run unapred da bi se „imao receipt na čekanju“, osim ako izolovani operator može da garantuje da recenzent nema pristup.

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
- [ ] `agentswatch start` još nije pokrenut; ova predregistracija se zamrzava pre starta i pre izmena koda.
- [ ] Niko još nije otvorio AgentsWatch receipt / findings za ovaj run.
- [ ] Ovaj list je potpun i **ne sadrži** sopstveni heš (heš ide samo u zaseban manifest).

## Zamrzavanje (pre heširanja)

1. Popuniti sva polja iznad. Ne ostavljati prazninu „za heš“ u ovom fajlu.
2. Sačuvati fajl i **prestati da ga menjate**. To su zamrznuti bajtovi.
3. Izračunati SHA-256 tih tačnih bajtova (UTF-8, kako je sačuvan).
4. Upisati putanju i heš u zaseban [05-seal-manifest-template.md](05-seal-manifest-template.md). **Ne vraćati heš u ovaj fajl** — to bi promenilo bajtove i oborilo pečat.
5. Ispravke idu u novu verziju lista; stara zamrznuta kopija ostaje sačuvana.

Lokalni datum u tabeli iznad **nije** nezavisni dokaz redosleda. Za predregistraciju je remote dokaz potreban **pre** `agentswatch start`. Posle ovog pečata ide `start` (sakriven od recenzenta), ne `finish`.
