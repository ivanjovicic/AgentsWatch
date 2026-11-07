# Nezavisni orakl / adjudikator

Adjudikator proverava da li je tvrdnja o promeni odluke **tačna** i **pripisiva novom AgentsWatch dokazu**, a ne da li je alat tehnički ispravan.

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

1. predregistraciju i **zapečaćeni** baseline (heš / commit) — bez izmene pečata;
2. konačni comparison list;
3. AgentsWatch receipt / findings;
4. isti Git/CI/PR kontekst koji je recenzent imao.

Pitanja (odgovoriti da / ne / unknown uz razlog):

| Pitanje | Odgovor | Razlog |
|---|---|---|
| Da li se zapečaćena privremena odluka razlikuje od konačne? | | |
| Da li je promena obrazložena konkretnim AW nalazom (kod, atribucija, evidencija)? | | |
| Da li je taj nalaz već bio vidljiv u Git diff-u, testu ili CI pre AW-a? | | |
| Da li je recenzent video AW izlaz pre pečata (kontaminacija)? | | |
| Da li je ovo jedan od 12 istorijskih REAL runova ili isti njihov posao? | | |
| Da li je tvrdnja lažni alarm (AW je upozorio, recenzent i adjudikator odbacuju kao neosnovano)? | | |
| Da li postoji materijalni false Done, curenje privatnosti ili pogrešna atribucija? | | |

## Presuda adjudikatora

Jedna od:

- `CONFIRMED_DECISION_CHANGE` — promena je stvarna i pripisiva novom AW dokazu koji Git/CI/PR sami nisu dali recenzentu.
- `NO_DECISION_CHANGE` — konačna odluka je ista, ili promena nije zbog novog AW dokaza.
- `FALSE_ALARM` — AW nalaz je materijalno pogrešan ili ne bi smeo da menja odluku.
- `CONTAMINATED` — redosled ili pečat su prekršeni.
- `NOT_INDEPENDENT` — nema drugog čoveka; ne broji se kao nezavisna potvrda.
- `STOP_CONDITION` — false Done, ozbiljno curenje ili materijalna pogrešna atribucija.
- `UNKNOWN`

Obrazloženje:

```text

```
