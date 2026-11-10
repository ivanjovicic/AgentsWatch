# AI-ONLY-EVAL-001 - Blind baseline review v1 (BACKEND-TEST-048)

Classification: `AI_ONLY_EXPERIMENTAL` (not counted toward the human GO threshold; does not touch the 12/30 ledger).
This file is frozen after hashing. Its SHA-256 lives only in the sidecar `seal-manifest.md`.

| Field | Value |
|---|---|
| Eval run id | `AI-ONLY-EVAL-001` |
| Reviewer | ChatGPT (AI), run manually by the owner (Ivan Jovicic) |
| Reviewed item | ivanjovicic/MathLearning draft PR #50 |
| Reviewed head SHA | `a7ec70f1f98df9b5add8e70bc1be9dc507d75ffe` |
| Base SHA | `b8acd2294d9677bc53a1a85acf3406595c1b49ef` |
| Review window (per reviewer) | 2026-11-10 01:04-01:10 Europe/Belgrade |
| Relayed by | owner, in messages `t53u` (sections 3-6) and `t54u` (sections 1-2) |
| Decision | `ACCEPT_AS_PARTIAL` |

## Operator note (disclosed caveats)

- The reviewer read `.ai/runs/2026-11-09-BACKEND-TEST-048-evidence.md`. That file is MathLearning's own `scripts/agent_run.py` run log and is part of the PR #50 diff; it is NOT AgentsWatch output. No AgentsWatch start output, contract, receipt or prior findings were exposed to the reviewer. Classification: `NOT_CONTAMINATED`, with this caveat disclosed.
- Reviewer's own disclosures: the CI test artifact (TRX) was downloaded through the public nightly.link mirror; raw CI run logs require sign-in and were not seen; the reviewer states it did not comment on, approve or change anything on GitHub.
- The review text below is verbatim as relayed by the owner, in order: sections 1-2 (t54u), then sections 3-6 (t53u).

## Verbatim review

1. Odluka: ACCEPT_AS_PARTIAL

Obim je poštovan, a Phase 1 je pošteno označen kao PARTIAL. Ipak postoje dva major nalaza koje treba rešiti pre nego što se ovo smatra završenim.

Prijava pre svega: diff sadrži fajl .ai/runs/2026-11-09-BACKEND-TEST-048-evidence.md. On izgleda kao agentski receipt: navodi agenta i alat, model, ID-jeve „prior mistakes" i Completion %. Pročitan je samo jednom, da se proveri obim, i nijedan nalaz ne počiva na njemu. PR komentari, review-i i transkripti nisu čitani.

2. Nalazi

major, tests/MathLearning.Tests/Infrastructure/IndexBloatPostgresTests.cs:59-84, 118-123. Test IndexMaintenance_AnalyzeStillRunsWithoutReindex ne bi uhvatio vraćanje REINDEX-a.

Prati Questions_pkey sa jednim redom, koji bi i stari kod preskočio (filter >1 MB), pa bi test prošao i na base-u.

Upoređuje i OID, a običan REINDEX INDEX/TABLE menja samo relfilenode. Provereno lokalno na PG 16.15: OID ostaje 16443, relfilenode ide 16443→16446.

Jedina prava zaštita je string provera DoesNotContain("REINDEX INDEX") u IndexMaintenanceSqlContractTests.cs:20. Ona ne hvata REINDEX TABLE ni drugačiji razmak.

major, src/MathLearning.Api/Endpoints/MaintenanceEndpoints.cs:31. bloatedIndexes = 0 je zakucano. Po Opciji B tu treba UNKNOWN ili null, a ne nula koja izgleda kao „zdravo". Test u MaintenanceEndpointContractTests.cs (~127) tu nulu još i zaključava.

minor, MaintenanceEndpoints.cs:29 i IndexMaintenanceService.cs:24-42. Errors se više ne puni, pa je success uvek true. Ako ANALYZE padne, ruta vraća 500. Test za granu success=false koristi lažni servis i pokriva put koji se u stvarnom radu ne dešava.

minor, MaintenanceEndpoints.cs:32, :68 i IndexMaintenanceService.cs:145. unknownBloatIndexes broji samo indekse veće od 1 MB, iako su i manji UNKNOWN. Lista se i dalje zove bloatedIndexes.

minor, IndexMaintenanceService.cs:24 i POST /rebuild-indexes. Imena „Rebuild…" su zbunjujuća. Autor je to naveo.

minor, IndexMaintenanceService.cs:30-35. Otvaraju se dve konekcije zaredom. Nije greška, samo nepotreban trošak.

minor, .ai/runs/...-evidence.md:52-55. Zapis je zastareo: „Needs validation", „draft PR pending", „Commit SHA: self".

Potvrđeno je i šta je u redu. Status je uvek "UNKNOWN", a BloatPercentage je null (IndexMaintenanceService.cs:89, 165, 243, 255). ReindexAsync je uklonjen. ANALYZE je ostao zaseban korak (UpdateStatisticsAsync, :199-221), nad fiksnom listom tabela.

3. Da li je obim poštovan: da. PR menja 7 fajlova: kod u Endpoints, Services i Maintenance, tri test fajla i jedan .ai/runs fajl. Ništa nije pod migracijama, .github, fly.toml ni queue routerom. CREATE EXTENSION i produkcioni connection stringovi se ne pojavljuju. PR je i dalje draft i nije mergovan. Head i base se slažu sa zadatim SHA-ovima, a PR ima jedan commit.

4. Da li su testovi i CI dovoljan dokaz: delimično.

Database Validation run 42723907114 je uspeo na a7ec70f, nad bazom postgres:16, sa POSTGRES_PROVIDER_TESTS_REQUIRED=1. TRX artefakt je lično proveren: 1285 od 1285 testova je prošlo, nijedan nije preskočen. Oba Postgres testa su se zaista izvršila (1,29 s i 0,42 s).

Backend Agent System Validation run 42723907180 je takođe uspeo, ali pokriva samo agent tooling i dokumentaciju, ne kod.

Lokalno ponavljanje na mom računaru, samo nad lokalnim PG 16.15: build je uspeo, a fokusiranih 10 od 10 testova je prošlo.

Šta CI pokriva: status UNKNOWN uz null procenat, da ANALYZE popunjava pg_stats i ugovore endpointa.

Šta ne pokriva: nema degradiranog fixture-a, nema testa koji bi pao na vraćenom REINDEX-u i nema red-rezultata pre izmene. Nema ni testova za CLI, nedostupnu ekstenziju ili privilegije, niti za partial, expression i particionisane indekse. Spec traži SQL dokaz u run logu, a toga nema.

Ograničenja provere: sirovi run logovi traže prijavu, pa nisu viđeni. Artefakt je preuzet preko javnog mirror-a nightly.link.

Upozorenje: bez POSTGRES_PROVIDER_TESTS_REQUIRED Postgres testovi „prolaze" naprazno za 1–4 ms i prijavljuju se kao Passed, a ne kao Skipped.

5. Reziduali

Naveo ih je autor: nema red-rezultata, nema tačnog merenja bloat-a, CLI (IndexMaintenanceTool/Program.cs:127-139) i dalje prikazuje HEALTHY za null, legacy imena i docs/API_ENDPOINT_INVENTORY.md.

Nije ih naveo:

IndexMaintenanceTool nije u MathLearning.slnx i ne builduje se ni na base-u ni na head-u (NU1605, downgrade Npgsql 8.0.6→8.0.3), pa ga CI uopšte ne pokriva.

openapi.yaml:2550-2556 i dalje kaže „Rebuild DB indexes".

Zakucano bloatedIndexes=0, success koji je uvek true i REINDEX test koji ništa ne razlikuje (nalazi iz tačke 2).

Rani return u Postgres testovima (:13, :61, :86-87) znači da van CI-ja tiho prolaze.

Iz speca nedostaju konfigurabilni pragovi, privilegije i ekstenzije po targetu (Render/Fly/Neon), isti prikaz u CLI-ju i endpointu, specijalni tipovi indeksa i retencija SQL dokaza.

Za tvoju potvrdu: pozadinski servis svakog dana u 03:00 UTC (04:00 po beogradskom vremenu) automatski pokreće ANALYZE nad 7 tabela kad je IndexMaintenanceEnabled uključen. Tako je bilo i na base-u, ali to znači da je „vođeni" ANALYZE zapravo zakazan, a ne ručan. Proveri da li je to namera.

6. Vreme pregleda: od 01:04 do 01:10, 10. 11. 2026, po beogradskom vremenu (UTC+1).

Na GitHub-u nisam ništa komentarisao, odobravao ni menjao.