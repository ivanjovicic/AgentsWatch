# Nezavisni orakl / adjudikator

Adjudikator proverava da li je tvrdnja o promeni odluke **tačna** i **pripisiva AgentsWatch dokazu koji recenzent nije već uočio i zapisao u zapečaćenom baseline-u**, a ne da li je alat tehnički ispravan.

AgentsWatch nalazi obično potiču iz Git/CI evidencije. Zato treba razlikovati:

- `ALREADY_NOTICED_IN_BASELINE` — recenzent je taj nalaz **stvarno uočio** i zapisao ga u zamrznutom baseline listu. To broji **protiv** incremental vrednosti AgentsWatch-a (ponavljanje već identifikovanog nalaza).
- `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED` — nalaz je teoretski mogao da se vidi pažljivim čitanjem diff-a/CI, ali **nije** zapisan kao uočen u baseline-u. To se **ne** broji automatski kao AW decision change i **ne** diskvalifikuje automatski AW korist. Treba potvrda adjudikatora da je AW taj nalaz stvarno izneo i da je to promenilo odluku.
- `NEWLY_SURFACED` — nije razumno tretirati kao nešto što je recenzent već koristio, i nije samo restatement već zabeleženog nalaza.

Samo `CONFIRMED_DECISION_CHANGE` posle ove klasifikacije ulazi u GO brojač. Hindsight („kasnije je očigledno iz diff-a“) nije dovoljan da se slučaj automatski odbaci ili prihvati.

## Zahtev nezavisnosti

Adjudikator mora biti **druga osoba** od recenzenta koji je dao baseline odluku.
Trenutno stanje programa (2026-11-07): postoji samo jedan čovek-pregledaoc (vlasnik, isti GitHub nalog). Isto-nalog `COMMENTED` review **nije** nezavisni `APPROVED`. Ista osoba koja nosi dve „kape“ **ne sme** da se predstavi kao nezavisni adjudikator. Dok druga osoba nije dostupna, nijedan run se ne može brojati kao *nezavisno potvrđena* promena odluke.

Ne izmišljati drugog recenzenta.

| Polje | Upis |
|---|---|
| Eval run id | |
| Adjudikator (ime; mora biti != recenzent) | |
| Da li je adjudikator stvarno druga osoba? | da / ne |
| Ako ne, run je nepodoban za GO brojač | |

## Šta se proverava

Adjudikator vidi, redom:

1. predregistraciju i **zamrznuti** baseline, plus sidecar manifest (heš originalnih bajtova) — bez izmene zamrznutih fajlova;
2. konačni comparison list;
3. AgentsWatch receipt / findings;
4. isti Git/CI/PR kontekst koji je recenzent imao;
5. proverljivi dokaz redosleda iz manifesta (remote commit / PR timeline / drugi zapis), ne lokalni sat.

Pitanja (odgovoriti da / ne / unknown uz razlog):

| Pitanje | Odgovor | Razlog |
|---|---|---|
| Da li se zapečaćena privremena odluka razlikuje od konačne? | | |
| Da li je promena obrazložena konkretnim AW nalazom (kod, atribucija, evidencija)? | | |
| Klasifikacija ključnog nalaza: `ALREADY_NOTICED_IN_BASELINE` / `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED` / `NEWLY_SURFACED` | | |
| Ako je `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED`, da li adjudikator potvrđuje da je AW to akciono izneo i da je to promenilo odluku? | | |
| Da li se heš u manifestu poklapa sa originalnim zamrznutim bajtovima (fajl nije menjan posle heša)? | | |
| Da li je `agentswatch start` pokrenut pre izmena koda (inače atribucija nije validna)? | | |
| Da li postoji odvojeni remote dokaz za predregistraciju (pre starta) i za baseline pregled (pre finish-a)? Lokalni timestamp nije dovoljan. | | |
| Da li je recenzent video start izlaz, dirty-at-start brojeve, baseline metapodatke ili receipt pre pečata baseline pregleda? | | |
| Da li je `finish` / prikaz receipta usledio tek posle remote dokaza baseline pečata (podrazumevano da)? | | |
| Da li je ovo jedan od 12 istorijskih REAL runova ili isti njihov posao? | | |
| Da li je tvrdnja lažni alarm (AW je upozorio, recenzent i adjudikator odbacuju kao neosnovano)? | | |
| Da li postoji materijalni false Done, curenje privatnosti ili pogrešna atribucija? | | |

## Presuda adjudikatora

Jedna od:

- `CONFIRMED_DECISION_CHANGE` — promena je stvarna i pripisiva AW nalazu koji **nije** bio `ALREADY_NOTICED_IN_BASELINE`. Ako je nalaz `DISCOVERABLE_IN_DIFF_BUT_NOT_NOTICED`, ova presuda sme da stoji samo uz izričitu potvrdu adjudikatora (nije automatska).
- `NO_DECISION_CHANGE` — konačna odluka je ista, ili je promena samo restatement nalaza već zapisanog u baseline-u (`ALREADY_NOTICED_IN_BASELINE`).
- `FALSE_ALARM` — AW nalaz je materijalno pogrešan ili ne bi smeo da menja odluku.
- `CONTAMINATED` — redosled ili pečat su prekršeni.
- `NOT_INDEPENDENT` — nema drugog čoveka; ne broji se kao nezavisna potvrda.
- `STOP_CONDITION` — false Done, ozbiljno curenje ili materijalna pogrešna atribucija.
- `UNKNOWN`

Obrazloženje:

```text

```
