# AI-ONLY-EVAL-001 - Seal manifest (sidecar)

Template: `docs/evidence/aw-eval-kit/05-seal-manifest-template.md` at AgentsWatch main `fe1be515108437b948d34b3ac28fc5af36810db9`.
This file is NOT part of any hashed record. Classification: `AI_ONLY_EXPERIMENTAL` (not counted toward the human GO threshold; does not touch the 12/30 ledger).

## Row 1 - predregistracija

| Polje | Upis |
|---|---|
| Eval run id | `AI-ONLY-EVAL-001` |
| Vrsta pečata (`predregistracija` ili `baseline-pregled`) | `predregistracija` |
| Relativna putanja zamrznutog fajla | `docs/evidence/ai-only-eval-001/preregistration-v1.md` |
| Verzija zapisa (npr. v1; v2 samo ako je stari v1 sačuvan) | v1 |
| SHA-256 zamrznutih bajtova (hex) | `617ede30d6095f7d0f96d1d8ee99930e1cd7b6514f7a233823a348238ce21c66` |
| Alat / komanda heša (npr. `Get-FileHash -Algorithm SHA256`) | `Get-FileHash -Algorithm SHA256` over the committed blob bytes (UTF-8, no BOM, LF). Re-hash with `git show aae1f16c70ef103787af2cab09f29a879ef553d6:docs/evidence/ai-only-eval-001/preregistration-v1.md`; a Windows checkout with `core.autocrlf=true` yields CRLF bytes and a different hash. |
| Git commit SHA koji sadrži zamrznuti fajl, ako postoji | `aae1f16c70ef103787af2cab09f29a879ef553d6` |
| Da li je taj commit gurnut na remote? URL commit-a ili PR timeline | Yes. https://github.com/ivanjovicic/AgentsWatch/commit/aae1f16c70ef103787af2cab09f29a879ef553d6 on branch `eval/ai-only-001-prereg` (no PR). |
| Drugi proverljivi dokaz redosleda (potpis, remote zapis) | GitHub repository activity id `49639631760`: `branch_creation` of `refs/heads/eval/ai-only-001-prereg` at `aae1f16`, 2026-11-09T23:34:38Z (2026-11-10 00:34:38 Europe/Belgrade). |
| Lokalni timestamp (Europe/Belgrade) — **nije** nezavisni dokaz | 2026-11-10 00:33 |
| Šta ovaj pečat zatvara | Before `agentswatch start` and before any MathLearning code change. |

Ordering note: the remote push proves when the preregistration became remote-visible. It does not by itself prove that the later local `agentswatch start` happened after it; that rests on the operational order (operator-only start output, recorded start timestamp in the run baseline).

## Row 2 - baseline-pregled

Pending. Added as a new row after the blind ChatGPT baseline review is frozen, before `agentswatch finish` / receipt display.