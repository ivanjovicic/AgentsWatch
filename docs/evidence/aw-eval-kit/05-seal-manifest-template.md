# Manifest pečata (sidecar)

Ovaj fajl **nije** deo heširanog zapisa. Hešira se samo zamrznuti list (predregistracija i/ili baseline) koji u sebi **ne sadrži** ovaj heš.

Jedan eval run ima **najmanje dva** reda: `predregistracija` (pre `agentswatch start` i pre izmena koda) i `baseline-pregled` (posle zadatka, pre `agentswatch finish` i pre prikaza receipta). Eventualna korekciona verzija dodaje novi red; stari red i stari fajl ostaju. Posle upisa heša, zamrznuti list se ne dira.

## Integritet naspram redosleda

- **Integritet:** SHA-256 se poklapa sa originalnim zamrznutim bajtovima. Adjudikator ponovo hešira taj fajl, nikad kopiju izmenjenu posle računanja. Heš se i dalje ne upisuje u heširani fajl.
- **Redosled:** remote push ili drugi potpisan / na remote-u zabeležen zapis dokazuje kada je zapečaćeni fajl postao remote-vidljiv. To **samo po sebi** ne dokazuje da je lokalna `agentswatch` komanda izvršena kasnije, osim ako je operativni redosled (odvojeni operator, sakriven start izlaz, `finish` tek posle pečata) ili drugi nezavisni zapis pouzdan. Ovaj komplet ne uvodi novu attestacijsku platformu.
- Lokalni sat (Europe/Belgrade timestamp na radnom listu) **nije** nezavisni dokaz redosleda.
- Lokalni git commit koji nije gurnut na remote **nije** nezavisni dokaz redosleda: autor, datum i poruka mogu se nazadovati.

| Polje | Upis |
|---|---|
| Eval run id | |
| Vrsta pečata (`predregistracija` ili `baseline-pregled`) | |
| Relativna putanja zamrznutog fajla | |
| Verzija zapisa (npr. v1; v2 samo ako je stari v1 sačuvan) | |
| SHA-256 zamrznutih bajtova (hex) | |
| Alat / komanda heša (npr. `Get-FileHash -Algorithm SHA256`) | |
| Git commit SHA koji sadrži zamrznuti fajl, ako postoji | |
| Da li je taj commit gurnut na remote? URL commit-a ili PR timeline | |
| Drugi proverljivi dokaz redosleda (potpis, remote zapis) | |
| Lokalni timestamp (Europe/Belgrade) — **nije** nezavisni dokaz | |
| Šta ovaj pečat zatvara | za `predregistracija`: pre `start`-a; za `baseline-pregled`: pre `finish`-a / prikaza receipta |

Posle pečata `predregistracija` operator pokreće `agentswatch start` (izlaz sakriven od recenzenta) pa tek onda idu izmene koda. Posle pečata `baseline-pregled` operator pokreće `agentswatch finish` i prikazuje receipt.
