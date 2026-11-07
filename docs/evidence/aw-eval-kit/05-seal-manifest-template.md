# Manifest pečata (sidecar)

Ovaj fajl **nije** deo heširanog zapisa. Hešira se samo zamrznuti list (predregistracija i/ili baseline) koji u sebi **ne sadrži** ovaj heš.

Jedan eval run može imati više redova (predregistracija, baseline, eventualna korekciona verzija). Posle upisa heša, zamrznuti list se ne dira. Ispravka = nova verzija lista + novi red ovde; stari red i stari fajl ostaju.

## Integritet naspram redosleda

- **Integritet:** SHA-256 se poklapa sa originalnim zamrznutim bajtovima. Adjudikator ponovo hešira taj fajl, nikad kopiju izmenjenu posle računanja.
- **Redosled (pre AgentsWatch izlaza):** potreban je proverljiv zapis van lokalne mašine, npr. git commit **gurnut na remote** i vidljiv u PR timeline-u, ili drugi potpisan / na remote-u zabeležen timestamp **pre** nego što je AW izlaz generisan ili otvoren.
- Lokalni sat (Europe/Belgrade timestamp na radnom listu) **nije** nezavisni dokaz redosleda.
- Lokalni git commit koji nije gurnut na remote **nije** nezavisni dokaz redosleda: autor, datum i poruka mogu se nazadovati.

| Polje | Upis |
|---|---|
| Eval run id | |
| Relativna putanja zamrznutog fajla | |
| Verzija zapisa (npr. v1; v2 samo ako je stari v1 sačuvan) | |
| SHA-256 zamrznutih bajtova (hex) | |
| Alat / komanda heša (npr. `Get-FileHash -Algorithm SHA256`) | |
| Git commit SHA koji sadrži zamrznuti fajl, ako postoji | |
| Da li je taj commit gurnut na remote? URL commit-a ili PR timeline | |
| Drugi proverljivi dokaz redosleda (potpis, remote zapis) | |
| Lokalni timestamp (Europe/Belgrade) — **nije** nezavisni dokaz | |
| Potvrda: AgentsWatch izlaz još nije generisan niti otvoren | da / ne |

Tek posle popunjenog manifesta i proverljivog remote dokaza redosleda sme da se generiše ili otvori AgentsWatch izlaz.
