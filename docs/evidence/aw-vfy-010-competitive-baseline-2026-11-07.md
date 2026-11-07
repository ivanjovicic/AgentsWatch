# AW-VFY-010 — konkurentska osnova (2026-11-07)

Datum istraživanja: 2026-11-07 (Europe/Belgrade)
Status: dodatak uz postojeću konkurentsku građu; nije tržišna studija i nije dokaz product-market fit-a.
Jezik: srpski (latinica), uz originalne nazive proizvoda i URL-ove.

Ovaj dokument **ne duplicira** postojeću osnovu vraćenu u PR #14 (`570bb2456d5a43d72c5c7e6fdb6d468260d09632`).
Čita se zajedno sa:

- [docs/COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md](../COMPETITIVE_LANDSCAPE_AND_DIFFERENTIATION_2026.md)
- [docs/research/COMPETITIVE_VALIDATION_ADDENDUM_2026_09_16.md](../research/COMPETITIVE_VALIDATION_ADDENDUM_2026_09_16.md)
- [docs/research/agentswatch_deep_dive_2026_09/02_COMPETITOR_CAPABILITY_MATRIX.md](../research/agentswatch_deep_dive_2026_09/02_COMPETITOR_CAPABILITY_MATRIX.md)

Pravilo evidencije isto kao u septembarskoj matrici: `YES` samo uz jasan zvanični izvor; sve ostalo je `PARTIAL` ili **Unknown**. Nema izmišljenih kupaca, tržišnih udela ni metrika konverzije.

## Poređenje sa običnim Git / CI / PR pregledom

Ovo je primarni baseline iz AW-VFY-010 dogfood-a, ne hipoteza.

| Pitanje | Šta Git + CI + PR već daju | Šta AgentsWatch tvrdi da dodaje | Šta je za sada provereno u ovom programu |
|---|---|---|---|
| Šta se promenilo? | `git diff` / `git status` / PR files | Run-interval vs pre-existing dirty atribucija | Atribucija je radila kako je očekivano u zabeleženim B3/B4 rundama; isti fajlovi su vidljivi i u `git status` |
| Da li testovi prolaze? | CI required checks, lokalni `dotnet test` | Validation evidence na receptu | CI i lokalni testovi su otkrili iste prolaze/padove; AgentsWatch je beležio izjavljene rezultate |
| Da li sme da se merge-uje? | Branch protection / required reviews / required status checks | Evidence Gate odluka (Done / NeedsReview / ...) | Ručni `Supported` ostaje `AWE009` / `NeedsReview`; to je zaštita, ne novi poslovni nalaz |
| Da li je zadatak ispunjen? | Ljudski PR review + ticketing | Contract + acceptance + claims naspram diff-a | Nema nezavisno verifikovanog Done puta |

Zvanični GitHub opis required status checks: [Managing a branch protection rule](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/managing-a-branch-protection-rule).

Zaključak ovog slučaja (12 korisnih REAL, ledger): običan Git/CI/PR pregled vidi iste izmene koje su bile predmet runova. AgentsWatch je dosledno blokirao neverifikovani Done. To **nije** dokaz da proizvod menja merge/close odluku recenzenta bolje od Git/CI/PR. Taj dokaz zahteva protokol u `aw-vfy-010-benefit-measurement-protocol.md`. Dvanaest postojećih runova se ne sme naknadno tretirati kao da su već dokazali novi cilj.

## Postojeće alternative (javni izvori, provereno 2026-11-07)

Ovo su kategorije koje je vlasnik tražio: agent observability / guardrail, eval ili srodni governance, CI policy, PR review botovi. Opisi su uski i vezani za link.

### Vendor observability i praćenje agenata

- **Cursor AI Code Tracking API** (Enterprise, alpha): prati AI doprinos po commitu i prihvaćene AI izmene (TAB / Composer). To je telemetrija atribucije koda, ne vendor-neutralni evidence receipt. Izvor: [cursor.com/docs/account/teams/ai-code-tracking-api](https://cursor.com/docs/account/teams/ai-code-tracking-api). (Septembarski addendum je citirao stariji `prod.cursor.com` URL; ovaj pregled koristi trenutno javni `cursor.com` dokument.)
- **GitHub Copilot agent sessions**: panel/session logovi, praćenje toka, token usage; commit poruke cloud agenta mogu da vode nazad na session log. Izvor: [Managing agent sessions](https://docs.github.com/en/copilot/how-tos/copilot-on-github/use-copilot-agents/manage-and-track-agents). Audit eventi za enterprise agente ostaju opisani u starijem addendumu; ova dopuna ih nije ponovo otvarala u celosti (**Unknown** da li je skup eventa proširen od 2026-09-16).

### Nezavisni review / governance sloj

- **Qodo Agentic Toolbox / Reviewer skill**: nezavisni review engine nad lokalnim committed i uncommitted izmenama, uključujući untracked fajlove, pre PR-a; vraća nalaze agentu, ne tvrdi da zamenjuje PR review. Izvori: [introducing Qodo’s Agentic Toolbox](https://www.qodo.ai/blog/introducing-qodos-agentic-toolbox/), [Reviewer skill](https://docs.qodo.ai/agentic-toolbox/agentic-toolbox-codebase-review-skill). Septembarska matrica već beleži da nije jasno evidencirano: persisted pre-run dirty baseline, `preExistingChangedFurther` semantika, i vendor-neutral completion receipt zasnovan na obaveznoj validaciji. Ta ćelija ostaje **nije jasno evidencirano / Unknown kao kompletan AW ekvivalent**.

### PR review botovi

- **CodeRabbit**: automatski AI pregled pull requesta, inkrementalni review posle novih commitova, komande u PR komentaru. Izvor: [Pull Request Reviews](https://docs.coderabbit.ai/overview/pull-request-review). To je forge-level review bot, ne start/end run contract sa lokalnim dirty-start baseline-om.

### Guardrail / policy unutar izvršioca

- **Claude Code hooks i permissions**: hookovi kao `PreToolUse` i `Stop` mogu da blokiraju alat ili spreče zaustavljanje; permissions se mogu evaluirati runtime. Izvori: [Hooks reference](https://code.claude.com/docs/en/hooks), [Configure permissions](https://code.claude.com/docs/en/permissions). To su vendor-native guardrails, ne prenosivi receipt nezavisan od izvršioca.

### CI policy checks

- **GitHub required status checks** u branch protection pravilima: merge može da zahteva da navedeni checkovi prođu. Izvor: [Managing a branch protection rule](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/managing-a-branch-protection-rule). AgentsWatch ne zamenjuje ovaj sloj.

## Šta ovo ne dokazuje

- Da bilo koji navedeni proizvod „pobeđuje“ ili „gubi“ od AgentsWatch-a na merenom skupu korisnika (**nema takvog merenja**).
- Veličinu tržišta, cene, win-rate ili broj kupaca (**Unknown**; dokument `11_UNIT_ECONOMICS_AND_MARKET_SIZE.md` nije ponovo verifikovan u ovom pregledu).
- Da session log, AI-code procenat ili PR bot komentar rešavaju AW tezu o machine-checkable Run Contract + dirty-start atribuciji + evidence-backed Done. Postojeći addendum to već kaže: izvori pokazuju pritisak, ne dokaz da neko u potpunosti rešava tu tezu.
- Ćelije septembarske matrice koje ovaj pregled nije ponovo otvorio ostaju onakve kakve jesu ili **Unknown**.

## Ključni zaključak za Gate 5 ADJUST / PAUSE

Postojeći competitiveni klin iz PR #14 i dalje stoji kao hipoteza: lokalni, vendor-neutralni sloj verifikacije (ugovor, atribucija, receipt, evidencija). Javni alati pokrivaju izvršenje, session logove, AI-code tracking, PR review i CI gate-ove. AW-VFY-010 do sada pokazuje tehničku ispravnost gating-a, ali **ne** izmerenu prednost u promeni odluke recenzenta u odnosu na Git/CI/PR. Zato se proizvod ne sme proglasiti korisnim samo zato što je tehnički ispravan. Sledeći korak je merenje prema protokolu, na stvarnom odobrenom poslu, bez automatskog starta B5.
