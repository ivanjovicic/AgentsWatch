# AW-EVAL-PREP-001 — priprema evaluacije korisnosti

Datum: 2026-11-07 (Europe/Belgrade)
Status: **priprema**. Pilot se ne izvršava. B5, AW-VFY-011 i AW-VFY-012 se ne pokreću.
Ovaj dokument ne menja istorijske redove ledgera i ne prepravlja 12 postojećih REAL runova.
Kanonski red (router / verification queue / Gate 5 izlazni kriterijumi) nije menjan.

Provereni `origin/main` na početku rada: `038e2bf388ec19ed0ae9e15fbf6a5fd1c6da65dc`.

## Šta se ponovo koristi (metodologija se ne gradi iznova)

- [aw-vfy-010-benefit-measurement-protocol.md](aw-vfy-010-benefit-measurement-protocol.md) — definicija decision change case-a, predregistracija, sekvencijalno poređenje, merenje troška, GO/NARROW/STOP.
- [aw-vfy-010-gate5-interim.md](aw-vfy-010-gate5-interim.md) — Gate 5 ostaje **ADJUST / PAUSE**, 12/30, originalni cilj neispunjen.
- [aw-vfy-010-competitive-baseline-2026-11-07.md](aw-vfy-010-competitive-baseline-2026-11-07.md) — Git/CI/PR je primarni baseline; javne alternative su kvalifikovane.
- [aw-vfy-010-ledger.md](aw-vfy-010-ledger.md) — 12 korisnih REAL; te rundove ne smeju se naknadno koristiti kao dokaz novog cilja.
- `AGENTS.md`, `docs/prompt_queues/PROMPT_QUEUE_ROUTER.md`, `docs/prompt_queues/verification_mvp_2026_08_25.md`, `docs/prompt_queues/NEXT_PROMPT_FAST_PATH.md`.

Komplet radnih listova: [aw-eval-kit/README.md](aw-eval-kit/README.md).

Cilj buduće evaluacije (kad je vlasnik odobri i kad padnu blocker-i): utvrditi da li AgentsWatch daje **incremental** vrednost recenzentu iznad Git + CI + običnog PR pregleda. Pravilo proizvoda ostaje: `GO samo kada postoje proverljivi slučajevi promene odluke uz prihvatljiv dodatni trošak; inače NARROW ili STOP.`

## 1. Evaluacioni komplet

Četiri prazna šablona, bez izmišljenih brojeva:

| List | Ulaz |
|---|---|
| [aw-eval-kit/01-preregistration-template.md](aw-eval-kit/01-preregistration-template.md) | pre starta / pre čitanja AW izlaza |
| [aw-eval-kit/02-baseline-review-worksheet.md](aw-eval-kit/02-baseline-review-worksheet.md) | Git/CI/PR odluka i pečat |
| [aw-eval-kit/03-independent-oracle-worksheet.md](aw-eval-kit/03-independent-oracle-worksheet.md) | druga osoba potvrđuje pripisivost |
| [aw-eval-kit/04-final-comparison-worksheet.md](aw-eval-kit/04-final-comparison-worksheet.md) | posle pečata, posle AW pregleda |

## 2. Kako se beleži početna Git/CI/PR odluka pre AgentsWatch nalaza

Redosled je obavezan (protokol, odeljci 2–3):

1. Popuniti predregistraciju (očekivani ishod, zašto je posao legitiman, da/ne za očekivani decision change).
2. Recenzent pregleda **samo** Git diff, dostupni CI i/ili PR kontekst.
3. Recenzent upisuje privremenu odluku (merge / ne merge / izmena / close ili ekvivalent) i stvarne timestamp-e početka i kraja u baseline list.
4. **Pečat:** sačuvati list, izračunati SHA-256 sadržaja, i poželjno ga commitovati u evidencioni zapis *pre* otvaranja receipta. Git commit daje nezavisni timestamp. Heš se upisuje u sam list.
5. Tek posle pečata sme da se pokrene `agentswatch finish` pregled ili otvori već postojeći receipt.

Ako operator CLI-ja i recenzent nisu ista osoba, operator ne sme da prikaže nalaze recenzentu pre pečata. Ako je receipt već nastao, putanja i sadržaj ostaju zatvoreni do pečata.

Kršenje redosleda = `CONTAMINATED`. Run se ne broji kao decision change case.

## 3. Nezavisni adjudikator

Adjudikator proverava dva uslova zajedno:

- da je promena odluke stvarna (zapečaćena privremena ≠ konačna);
- da je pripisiva **novom** AgentsWatch dokazu, a ne nalazu koji je već bio u diff-u, testu ili CI.

To nije isto što i potvrda da je Evidence Gate tehnički ispravan (`AWE009` / `NeedsReview` samo po sebi nije decision change).

### Iskrena napomena o nezavisnosti (blokator)

Trenutno postoji **samo jedan čovek** koji pregleda ovaj program: vlasnik, na istom GitHub nalogu. Isto-nalog `COMMENTED` review već nije nezavisni `APPROVED`. Ista osoba ne može da bude i baseline recenzent i „nezavisni“ adjudikator. **Nezavisnost je blocker** dok druga osoba nije stvarno dostupna. Ovaj dokument ne izmišlja drugog recenzenta i ne tretira vlasnikov drugi prolaz kao nezavisnu potvrdu.

## 4. Merenje vremena i lažnih alarma

Meriti samo časovnikom u trenutku rada. Inače **Unknown**. Naknadna procena nije merenje.

| Veličina | Šta se beleži |
|---|---|
| Normalno (baseline) review vreme | minuti od početka do kraja Git/CI/PR pregleda |
| Incremental AW review vreme | minuti čitanja receipta / findings posle pečata |
| Setup trošak | minuti CLI/contract/worktree iznad uobičajenog toka |
| Incremental overhead po runu | setup + AW minuti |
| Materijalni lažni alarmi | broj AW nalaza koje recenzent i adjudikator odbace kao neosnovane, a koji su oduzeli pažnju |

Medijane se računaju samo nad runovima gde odgovarajuća veličina **nije** Unknown. Runovi sa Unknown vremenom ne smeju se tretirati kao da su prošli vremenski prag.

## 5. Pilot kriterijumi (unapred definisani, nisu izmereni rezultati)

Isti pragovi kao u protokolu (vlasnik, 2026-11-07). To **nisu** postignuti rezultati i **nisu** odobrenje za start AW-VFY-011 ili pilota.

| Metrika | Predloženi prag |
|---|---|
| Medijana dodatnog vremena po REAL runu (setup + AW sloj recenzenta) | Najviše 2 minuta |
| Relativni dodatni trošak | Najviše 25% medijane vremena običnog Git/CI/PR reviewa |
| Materijalni lažni alarmi | Najviše 1 na 10 legitimnih runova |
| Dokazane, nezavisno potvrđene promene odluke | Najmanje 3 u 10–15 relevantnih slučajeva (uslov za GO) |
| Materijalni false Done, ozbiljno curenje privatnosti, ili materijalna pogrešna atribucija | STOP (nije prihvatljivo) |

Prva dva vremenska ograničenja moraju da važe **zajedno**. Prekoračenje **bilo kog** od ta dva (medijana > 2 minuta **ili** medijana > 25% baseline medijane) **ruši GO uslov troška**.

GO zahteva i ≥ 3 nezavisno potvrđene decision change case-a u 10–15 podobnih slučajeva, i lažne alarme unutar praga, i odsustvo STOP uslova. NARROW ostaje opcija za near-miss ili usko lokalizovanu korist. Tehnički ispravno blokiranje neverifikovanog Done nije GO.

Samo `REAL` runovi na stvarnom, vlasnikom odobrenom poslu ulaze u brojač. Kontrole, replay-evi, kontaminirani runovi i istorijskih 12 ne ulaze.

## 6–7. Inventar kandidata (read-only, 2026-11-07)

Pregledani tačni repozitorijumi na GitHub-u: `ivanjovicic/AgentsWatch`, `ivanjovicic/MathLearning`, `ivanjovicic/Mathlearning-Mobile-App`, `ivanjovicic/Trendplus`. Pored njih postoje i `TrendplusProdavnica` i `Trendplus-access`; nisu bili u opsegu ovog zadatka i ovde se ne tretiraju kao kandidati.

Pravila oznaka:

- `READY_FOR_OWNER_REVIEW` — postoji legitimna stavka koja nije na postojećem HOLD-u, nije očigledno tuđi aktivan claim i nije već brojana; **postojanje nije odobrenje za izvršenje**.
- `HOLD` — već važeći HOLD ostaje HOLD.
- `ALREADY_DONE` — isti posao je već među 12 istorijskih REAL runova ili je inače već izvršen u tom smislu.
- `CONFLICTED` — tuđi/agent claim, zastareo/superseded PR, ili router/queue zabranjuje diranje bez posebnog OK.
- `UNKNOWN` — legitimna ili moguća stavka, ali claim/queue status ili podobnost nisu pouzdano utvrđeni u ovom read-only prolazu.

Nijedna stavka ispod nije odobrena za pilot run ovim dokumentom.

### AgentsWatch (`ivanjovicic/AgentsWatch`)

Nema otvorenih GitHub issue-a (provereno 2026-11-07). Nema slobodnog, već tiketiranog .NET endpoint/service posla u ovom repou.

| Stavka | Status | Razlog |
|---|---|---|
| [PR #4](https://github.com/ivanjovicic/AgentsWatch/pull/4) discovery/learning loop | CONFLICTED | Klasifikovan kao superseded / future-gated; router: ne merge/close/update bez posebnog vlasničkog zahteva. Iza AW-VFY-011. |
| [PR #5](https://github.com/ivanjovicic/AgentsWatch/pull/5) research/product direction | CONFLICTED | Salvage/future-gated; ne sme da pregazi kanonski red. |
| [PR #6](https://github.com/ivanjovicic/AgentsWatch/pull/6) early run-evidence foundation | CONFLICTED | Superseded živim start/finish/receipt ponašanjem na `main`. |
| [PR #8](https://github.com/ivanjovicic/AgentsWatch/pull/8) trust platform / SaaS | CONFLICTED / HOLD-srodno | Future-gated iza AW-VFY-012 (SaaS/auth/billing/team). |
| [PR #9](https://github.com/ivanjovicic/AgentsWatch/pull/9) queue precision | CONFLICTED | Salvage + superseded kao queue update; ostaje netaknut. |
| AW-VFY-011 / AW-VFY-012 | HOLD | Kanonski HOLD do završetka i pregleda 010 odnosno 011. |
| B5 predloženi slotovi u ledgeru | HOLD / nije startovan | Gate 5 = ADJUST / PAUSE; B5 se ne pokreće. |
| 12 postojećih korisnih REAL runova | ALREADY_DONE | Sačuvani su; ne smeju se reuse-ovati kao dokaz novog cilja. |

### MathLearning (`ivanjovicic/MathLearning`)

Repo ima sopstveni queue/ownership (`AGENTS.md` → `docs/prompt_queues/`). Ne preuzimati tuđi claim.

| Stavka | Status | Razlog |
|---|---|---|
| [Issue #47](https://github.com/ivanjovicic/MathLearning/issues/47) produkcioni quiz answer HTTP 500 | HOLD | Postojeći HOLD: produkcioni SQL / Fly apply / live settlement. Investigation-only je već bio B1-3; produkcioni zahvat ostaje HOLD. |
| [PR #49](https://github.com/ivanjovicic/MathLearning/pull/49) korekcija cadence-a u leaderboard vodiču | ALREADY_DONE | Isti posao kao B3-R3 (`LEADERBOARD_SYSTEM_GUIDE.md`). Korišćenje u pilotu bilo bi dvostruko brojanje. Draft; nije merge-ovan iz dogfood batch-a. |

Drugi otvoreni issue-i u ovom repou nisu nađeni.

### Mathlearning-Mobile-App (`ivanjovicic/Mathlearning-Mobile-App`)

Repo ima strogi claim/lease protokol (`AGENTS.md`, `docs/prompt_queues/PROMPT_LIFECYCLE.md`). Grane `agent/claim/*`, `cursor/*` i `codex/*` tretiraju se kao tuđi ili već započeti agentski rad — ne preuzimati.

| Stavka | Status | Razlog |
|---|---|---|
| [Issue #906](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/906) registracija na uređaju / API host vs Fly | HOLD | Postojeći HOLD (uređaj / Fly / APK host). Povezani draft PR-ovi #939 i #921 su agentski (`codex/`) rad na istoj temi — ne preuzimati. |
| Flutter dogfood #5 / GRX84, [PR #901](https://github.com/ivanjovicic/Mathlearning-Mobile-App/pull/901) | HOLD | Postojeći HOLD (GRX84 quiz step explanation). Grana `agent/claim/grx84-...`. |
| Flutter dogfood #6 / MOB80, [PR #865](https://github.com/ivanjovicic/Mathlearning-Mobile-App/pull/865) | HOLD | Postojeći HOLD (QuizProvider / quiz session). Grana `agent/claim/mob80-...`. |
| [Issue #154](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/154) consume_tx dokumentacija | ALREADY_DONE / UNKNOWN | B2-R5 je već koristio mobile consume_tx + planted dirty. Nije podobno za novi GO broj bez dokaza da je posao bitno drugi. |
| [Issue #879](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/879), [#877](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/877) CI / runner | UNKNOWN | Postojeći CI ticketi; queue/claim status nije pouzdano pročitan. Nisu odobreni. |
| [Issue #871](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/871), [#823](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/823), [#819](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/819), [#254](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/254), [#253](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/253), [#252](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/252) agent-system | CONFLICTED / UNKNOWN | Agent-system / lease / watchdog teme; formalni claim protokol. Ne preuzimati. |
| [Issue #463](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/463), [#16](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/16), [#15](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/15) rolling bot izveštaji | UNKNOWN | Periodični GitHub Actions ticketi, ne konkretan odobren produkt defekt. |
| [Issue #96](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/96), [#92](https://github.com/ivanjovicic/Mathlearning-Mobile-App/issues/92) | UNKNOWN | Stariji docs/agent-system ticketi; podobnost i claim nisu utvrđeni. |
| Otvoreni PR-ovi agentskog porekla (#939, #936, #926, #924, #921, #918–#914, #908–#902, #899, #898, #880–#866 i ostali na listi od 33) | CONFLICTED | Autori grana `codex/`, `cursor/`, `agent/claim/` ili integracioni/CI draftovi drugih agenata. Ne prisvajati. Zasebni linkovi su na GitHub listi otvorenih PR-ova. |
| [PR #883](https://github.com/ivanjovicic/Mathlearning-Mobile-App/pull/883), [#881](https://github.com/ivanjovicic/Mathlearning-Mobile-App/pull/881) Dependabot | UNKNOWN | Bump GitHub Actions; slaba osnova za merenje incremental reviewer vrednosti AgentsWatch-a. Nisu odobreni. |

### Trendplus (`ivanjovicic/Trendplus`)

Repo ima formalni queue/claim protokol (`docs/ai/PROMPT_QUEUE_PROTOCOL.md`). Aktivni claim se ne sme zaobići. Produkcijski SQL / import / deploy nisu odobreni ovom pripremom.

| Stavka | Status | Razlog |
|---|---|---|
| [Issue #208](https://github.com/ivanjovicic/Trendplus/issues/208) safe fresh Access import | UNKNOWN | Postojeći P0 tiket; queue claim nije pouzdano verifikovan. Obuhvata import podataka — nije auto-odobren i može dirati produkcijski rizik. |
| [Issue #174](https://github.com/ivanjovicic/Trendplus/issues/174) remap source-owned entities | UNKNOWN | Isto; P0 Access import, claim neverifikovan. |
| [Issue #173](https://github.com/ivanjovicic/Trendplus/issues/173) durable source namespace / batch #23 | UNKNOWN | Isto; P0 Access import, claim neverifikovan. |
| [Issue #312](https://github.com/ivanjovicic/Trendplus/issues/312) CI incident 2026-10-28 | UNKNOWN | CI/infra tiket; claim neverifikovan. |
| [Issue #293](https://github.com/ivanjovicic/Trendplus/issues/293) stabilizacija heartbeat testova | UNKNOWN | Test-timing tiket; claim neverifikovan. |
| [PR #317](https://github.com/ivanjovicic/Trendplus/pull/317) per-client access-control plan | UNKNOWN | Draft, telo PR-a kaže **DRAFT / NOT READY**, bez executable queue claim-ova. |

### Šta inventar ne sadrži

Nije nađen nijedan slobodan, već odobren, ne-HOLD, ne-istorijski .NET ili Flutter zadatak koji bi se smeo označiti `READY_FOR_OWNER_REVIEW` bez nagađanja o tuđim claim-ovima. Namerno nije izmišljen novi tiket da bi se popunio brojač 10–15.

## 8. Sprečavanje kontaminacije, naknadnog orakla i dvostrukog brojanja

- Pečat pre AW izlaza (odeljak 2). Viđen receipt pre pečata = kontaminacija.
- Očekivani ishod se piše u predregistraciji **pre** čitanja AW izlaza. Orakl se ne bira posle rezultata.
- Dvanaest istorijskih REAL runova ostaju kako jesu i ne ulaze u novi GO brojač. MathLearning #49 i B2-R5/consume_tx preklapanja su primer zabranjenog reuse-a.
- Isti diff, isti PR ili ista ispravka ne smeju se brojati dva puta (ni kao stari REAL, ni kao dva nova runa).
- Tuđi claim/PR se ne preuzima da bi se napravio „slobodan“ zadatak.
- HOLD se ne diže da bi se dobio broj.
- Ista osoba nije nezavisni adjudikator.
- Unknown vreme nije prolaz na vremenskom pragu.

## 9. Redosled malog pilota (ne izvršava se)

Ovo je redosled za **budućnost**, posle novog izričitog vlasničkog OK-a i posle uklanjanja blocker-a. Ovaj dokument ga ne pokreće.

1. Vlasnik imenuje drugog čoveka-adjudikatora (inače ostaje BLOCKED za nezavisne potvrde).
2. Vlasnik bira i izričito odobrava 10–15 podobnih `REAL` slučajeva sa inventara ili kasnije nastalog legitimnog posla. HOLD ostaje HOLD dok se posebno ne podigne.
3. Za svaki slučaj, u disposable worktree-u: predregistracija → Git/CI/PR pregled → pečat baseline lista → tek onda AgentsWatch start/finish ili otvaranje receipta → comparison list → adjudikator.
4. Zaustaviti čitav pilot odmah ako nastupi STOP uslov (materijalni false Done, ozbiljno curenje, materijalna pogrešna atribucija).
5. Posle kohorte izračunati medijane (samo nad izmerenim, ne nad Unknown) i primeniti GO/NARROW/STOP.
6. Ne puniti preostalih 18 istorijskih mesta do 30. Ne startovati B5 ni AW-VFY-011 iz ovog reda.

## 10. Blocker-i i presuda spremnosti

**Presuda: BLOCKED**

Konkretni blocker-i:

1. **Nema nezavisnog adjudikatora.** Dostupan je samo vlasnik na istom nalogu. Bez druge osobe nema *nezavisno potvrđenih* decision change slučajeva, a GO to zahteva.
2. **Nema vlasnikom odobrenog skupa od 10–15 podobnih zadataka.** Read-only inventar nije našao stavke `READY_FOR_OWNER_REVIEW`. Najkonkretniji stvarni defekti su HOLD (#47, #906, GRX84/#901, MOB80/#865). Trendplus P0/CI ticketi su `UNKNOWN` zbog neverifikovanog queue claim-a i mogućeg produkcijskog rizika. AgentsWatch nema otvorenih issue-a.
3. **Istorijskih 12 runova se ne sme reuse-ovati**, pa #49 i srodni već izvršeni poslovi ne popunjavaju kohortu.
4. **Postojeći HOLD-ovi ostaju aktivni** (uključujući AW-VFY-011/012, deploy, produkcioni SQL, cloud agents, outreach). Ovaj dokument ih ne diže.

Pilot se ne sme startovati dok vlasnik ne ukloni ove blocker-e (druga osoba + izričito odobreni legitimni zadaci, bez izmišljanja poslova). Čak i tada treba novo, odvojeno odobrenje za izvršenje — ova priprema to nije.
