# Technical Audit — NeilGaietto.com

**Date:** 2026-06-11
**Scope:** Portfolio site core (layout, landing page, SEO/meta, CI, dependencies, deployment). The four AI-built demo pages (`/breakout`, `/nokia`, `/graphene`, `/shikaku`) are intentionally out of scope — they are for-fun experiments, not product code.

---

## Executive Summary

**Health grade: B**

This is a small, well-kept static Astro site. Structure is clean, conventions are consistent, and there is real engineering hygiene unusual for a personal site (SHA-pinned GitHub Actions, secret scanning, strict TypeScript config, design docs in `docs/plans/`). The issues found are small in absolute terms but two of them silently defeat things the repo *believes* it has: working analytics and a working CI type gate.

**Top 3 risks**

1. **The CI type-check is a silent no-op.** `npx astro check` errors out in CI because `@astrojs/check` and `typescript` are not project dependencies — but it exits 0, so the "Build & Type Check" job stays green while checking nothing. Verified empirically (see finding A1).
2. **Analytics is dead on every page.** `Layout.astro` ships the GA4 placeholder `G-XXXXXXXXXX`, so no traffic data has been collected since the Astro migration, and every visitor pays for a useless gtag.js request (A2).
3. **Outdated toolchain with known advisories.** `npm audit` reports 8 vulnerabilities (4 high, 4 moderate) across the Astro/Vite build chain (A3). Runtime exposure is low for a fully static site, but dev-server exposure is real and the fix is cheap.

**Top 3 opportunities**

1. Make the CI gate real (one `package.json` change) — everything merged afterward is actually type-checked.
2. Fix per-page SEO metadata: `og:url`, og/twitter descriptions, and canonical URLs are currently wrong on every page except the homepage (A4).
3. Automate dependency updates (Dependabot/Renovate) so the audit findings of class A3 don't recur.

---

## Repo Map

**Purpose:** Personal portfolio/landing page for Neil Gaietto, plus hidden AI-built demo pages. Early-stage; the core is still being built.

**Stack:** Astro 5 (static output), TypeScript in frontmatter, plain CSS. No UI framework — deliberate per the approved design doc (`docs/plans/2026-03-08-astro-migration-design.md:20`).

**Hosting:** Cloudflare Pages serving `dist/` (README.md:6–7). No deploy workflow in the repo; deployment is presumed to be Cloudflare's Git integration (not verifiable from the repo).

```
src/
  layouts/Layout.astro    # Shared <head>: meta, OG, JSON-LD, fonts, GA4 — the only shared module
  pages/                  # File-based routes: index, 404, demos + 4 demo pages (out of scope)
  styles/global.css       # Site-wide CSS (80 lines)
public/
  assets/bg.jpg, neil.jpg # 244 KB / 11 KB — reasonable
  sitemap.xml             # Hand-maintained, homepage only
.github/workflows/pr-checks.yml  # gitleaks secret scan + build + astro check
docs/plans/               # Migration design + implementation plan
```

**Architecture:** As simple as it should be. One layout, file-based pages, zero client-side framework. Each demo page is a self-contained Astro page with scoped styles and an inline `<script>`. No data layer, no server.

**Surprises (good):** Actions pinned to commit SHAs, gitleaks in CI, `astro/tsconfigs/strict`, dated design docs with status fields. **Surprises (bad):** the type-check that CI claims to run cannot actually run (A1).

---

## Audit Findings

Severity scale: Critical / High / Medium / Low. Each finding labels fact vs. judgment.

### A1 — CI type-check step is a silent no-op — **High**

- **Where:** `.github/workflows/pr-checks.yml:33-34`; `package.json:11-13`
- **Fact:** The workflow runs `npx astro check`, but `@astrojs/check` and `typescript` are not in `package.json` (the only dependency is `astro`). Reproduced locally with `CI=true npx astro check`: Astro prints `[ERROR] [check] The @astrojs/check and typescript packages are required…` **and exits 0**. The CI step passes without performing any check — all 21 historical workflow runs are green.
- **Why it matters:** The repo believes it has a type gate; it has a green checkbox. TypeScript errors in any page frontmatter or script will merge silently.
- **Fix:** `npm i -D @astrojs/check typescript`. Once installed, `astro check` runs for real and returns non-zero on errors.

### A2 — GA4 measurement ID is a placeholder; analytics dead — **High**

- **Where:** `src/layouts/Layout.astro:11` — `const GA4_ID = 'G-XXXXXXXXXX'; // Replace with your real GA4 Measurement ID`
- **Fact:** Every page loads `gtag.js` with this bogus ID (`Layout.astro:60-66`). No analytics data is collected; every visitor downloads ~80 KB of useless script. The migration design doc explicitly listed "Migrate from UA-29368003-1 to GA4" as part of the work (`docs/plans/2026-03-08-astro-migration-design.md:48`) — the migration removed the old ID but the new one was never inserted.
- **Why it matters:** For a portfolio site, traffic data is one of the few feedback signals you have. This has been silently broken since the migration (commit `1f81910`).
- **Fix:** Insert the real GA4 ID — or, if analytics isn't wanted, delete the gtag block entirely and save the page weight. Either is fine; the current state is the worst of both.

### A3 — 8 known vulnerabilities in the build toolchain — **Medium**

- **Where:** `package-lock.json` (astro 5.17.1 tree)
- **Fact:** `npm audit` (2026-06-11) reports 4 high (defu prototype pollution, devalue prototype pollution/DoS, picomatch ReDoS, vite path traversal / arbitrary file read via dev server) and 4 moderate (astro ×3 incl. XSS in `define:vars`, h3, postcss, smol-toml). All except the three astro advisories are fixable with a plain `npm audit fix`; the astro ones require the v6 major (`astro@6.4.6`).
- **Judgment:** Severity is Medium, not High, because the site is fully static — most of these only matter at dev/build time. The vite dev-server file-read issue is the most concrete (anyone on your LAN while `npm run dev` runs). Note the site does use `define:vars` (`Layout.astro:61`), the subject of one astro XSS advisory, though with a hardcoded constant, so it is not exploitable as written.
- **Fix:** `npm audit fix` now (non-breaking); schedule the Astro 6 upgrade separately; add Dependabot so this class of finding self-heals.

### A4 — Open Graph / Twitter metadata wrong on every non-home page — **Medium**

- **Where:** `src/layouts/Layout.astro:24, 26, 32-33`
- **Fact:** `og:url` is hardcoded to `https://neilgaietto.com` and `og:description`/`twitter:description` are hardcoded to the homepage blurb ("Software Architect and Developer based in Apex, NC."), even though the layout already receives a per-page `description` prop that is used for the plain meta description (`Layout.astro:20`). There is also no `<link rel="canonical">` on any page.
- **Why it matters:** Sharing `/demos` or any demo page on social/Slack shows the homepage description and links the preview card to the homepage. Search engines get no canonical signal.
- **Fix:** Use the `description` prop for OG/Twitter, and `new URL(Astro.url.pathname, Astro.site)` for `og:url` + canonical. `site` is already configured in `astro.config.mjs:5`.

### A5 — sitemap.xml is hand-maintained, stale, and homepage-only — **Low**

- **Where:** `public/sitemap.xml:8-11`
- **Fact:** Lists only `/` with `lastmod 2026-02-13`. `/demos` and the demo pages are absent — possibly intentional ("hidden" pages, per commit history), but `/demos` is publicly linked from the demo pages themselves, so the hiding is soft at best.
- **Fix:** Either adopt `@astrojs/sitemap` (auto-generates on build) or consciously keep the single-entry file and delete the stale `lastmod`. Decide whether demos should be indexed; if not, add `noindex` to those pages rather than relying on sitemap omission.

### A6 — No favicon, no robots.txt — **Low**

- **Where:** `public/` contains only `assets/` and `sitemap.xml`; no `<link rel="icon">` in `Layout.astro`.
- **Fact:** Every first visit 404s on `/favicon.ico`; browser tabs show a blank icon. No `robots.txt` (harmless — crawlers assume allow-all — but it's the natural place to reference the sitemap).
- **Fix:** Add a favicon (an SVG is one file) and a 2-line `robots.txt` pointing at the sitemap.

### A7 — Google Fonts loaded without preconnect — **Low**

- **Where:** `src/layouts/Layout.astro:58`
- **Fact:** The Maitree stylesheet is loaded render-blocking with no `<link rel="preconnect">` to `fonts.googleapis.com`/`fonts.gstatic.com`, and no `font-display` control beyond the default `display=swap` query param (which is present — good).
- **Judgment:** Costs ~100–300 ms of first-render latency on cold connections. Self-hosting the font (e.g. `@fontsource/maitree`) would also remove the third-party request entirely.

### A8 — No tests — **Low (accepted for current maturity)**

- **Fact:** Zero test files. The only automated verification is the CI build (which does catch Astro compile errors) and the broken type check (A1).
- **Judgment:** For a static portfolio this is acceptable. The highest-value "test" here is fixing A1 plus, optionally, a link-checker pass over `dist/` in CI. Unit tests are not recommended — there is no business logic on the portfolio side.

### Strengths (worth keeping)

- **SHA-pinned actions** (`pr-checks.yml:15,18,26,27`) — supply-chain discipline most companies don't have.
- **Secret scanning in CI** via gitleaks with full history (`fetch-depth: 0`).
- **Strict TS config** (`tsconfig.json:2`) and typed Layout props (`Layout.astro:4-8`).
- **Design docs** with dates and status (`docs/plans/`), and the implemented structure matches the approved design exactly.
- **Lockfile committed**, `npm ci` in CI, minimal dependency surface (one direct dependency).
- **README is accurate** — thin, but nothing in it is wrong.

---

## Improvement Strategy

### Theme 1 — Make the safety net real (explains A1, A3, A8)

**Target state:** CI fails when types are wrong or a build breaks; dependencies update themselves.
**Principle:** A green check that verifies nothing is worse than no check — it manufactures false confidence.
**Done when:** `astro check` demonstrably fails CI on an introduced type error; Dependabot PRs appear weekly; `npm audit` is clean at the non-breaking level.

### Theme 2 — Finish the migration's loose ends (explains A2, A4, A5, A6)

**Target state:** Analytics actually collects; every page emits correct, self-describing metadata; favicon and sitemap exist and are generated, not hand-edited.
**Principle:** The migration was declared done with placeholders still in place; close them out so "done" means done.
**Done when:** GA4 real-time view shows a visit; sharing `/demos` in Slack renders that page's own title/description/URL; no 404s in the browser network tab on a cold load.

### Theme 3 — Performance polish (explains A7)

**Target state:** Font delivery doesn't block first paint.
**Principle:** A portfolio's landing page *is* the product; first impressions are measured in milliseconds.
**Done when:** Lighthouse performance ≥ 95 on mobile for `/`.

### Explicitly not fixing

- **Demo page code quality** — out of scope by owner's direction; they're for-fun AI experiments.
- **Unit/E2E test suites** — no business logic to protect; effort outweighs payoff at this maturity.
- **Astro 6 major upgrade** — worth doing, but as its own task after CI is trustworthy (Theme 1), not before.
- **Self-hosting fonts** — optional stretch; preconnect captures most of the win for 2 lines.

---

## Task Plan

### Milestone 0 — Safety net

| # | Task | Files | Effort | Risk | Depends |
|---|------|-------|--------|------|---------|
| 0.1 | **Make `astro check` real**: `npm i -D @astrojs/check typescript`; verify CI fails on an injected type error, then revert the error | `package.json`, `package-lock.json` | S | Low — may surface pre-existing type errors in demo pages that must be fixed or excluded to get green | — |
| 0.2 | **Add Dependabot** for npm + github-actions ecosystems, weekly | `.github/dependabot.yml` | S | None | — |
| 0.3 | **`npm audit fix`** (non-breaking) | `package-lock.json` | S | Low — transitive bumps only; CI build validates | 0.1 |

### Milestone 1 — Critical fixes (correctness)

| # | Task | Files | Effort | Risk | Depends |
|---|------|-------|--------|------|---------|
| 1.1 | **Fix or remove GA4** — insert real measurement ID (owner must supply) or delete the gtag block | `src/layouts/Layout.astro` | S | None | — |
| 1.2 | **Per-page OG/Twitter/canonical metadata** — use `description` prop and `Astro.url`/`Astro.site` | `src/layouts/Layout.astro` | S | Low | 0.1 (so the change is type-checked) |

### Milestone 2 — High-leverage improvements

| # | Task | Files | Effort | Risk | Depends |
|---|------|-------|--------|------|---------|
| 2.1 | **Generated sitemap** via `@astrojs/sitemap`; decide indexability of demo pages (add `noindex` prop to Layout if hiding them) | `astro.config.mjs`, `package.json`, delete `public/sitemap.xml`, `Layout.astro` | S | Low | 0.1 |
| 2.2 | **Favicon + robots.txt** | `public/favicon.svg`, `public/robots.txt`, `Layout.astro` | S | None | — |
| 2.3 | **Astro 6 upgrade** — clears remaining astro advisories | `package.json`, lockfile; possible config/page tweaks | M | Medium — major version; CI build + manual smoke of all pages | 0.1, 0.3 |

### Milestone 3 — Quality & polish

| # | Task | Files | Effort | Risk | Depends |
|---|------|-------|--------|------|---------|
| 3.1 | **Font preconnect** (or self-host via `@fontsource/maitree`) | `src/layouts/Layout.astro` | S | None | — |
| 3.2 | **Link check in CI** — e.g. `lychee` over `dist/` after build | `.github/workflows/pr-checks.yml` | S | Low — external-link flakiness; restrict to internal links | — |
| 3.3 | **README upgrade** — dev commands, deploy notes, mention `/demos` and `docs/plans` convention | `README.md` | S | None | — |

**Quick wins (all S, high impact):** 0.1, 0.2, 0.3, 1.1, 1.2, 2.2, 3.1 — the entire audit minus the Astro 6 major is roughly one afternoon.

### Implementation sketches — top 3

**0.1 — Real type check**

```bash
npm i -D @astrojs/check typescript
npx astro check   # must run and exit 0
# sanity: introduce `const x: number = "a"` in index.astro frontmatter,
# confirm `npx astro check` exits non-zero, revert.
```

**1.2 — Correct per-page metadata** (`src/layouts/Layout.astro`)

```astro
---
const { title, description, includeSchema = false } = Astro.props;
const canonical = new URL(Astro.url.pathname, Astro.site);
const ogImage = new URL('/assets/neil.jpg', Astro.site);
---
<link rel="canonical" href={canonical}>
<meta property="og:title" content={title}>
<meta property="og:description" content={description}>
<meta property="og:url" content={canonical}>
<meta property="og:image" content={ogImage}>
<meta name="twitter:description" content={description}>
```

**2.1 — Generated sitemap**

```bash
npx astro add sitemap   # adds @astrojs/sitemap to astro.config.mjs
rm public/sitemap.xml
```

If demo pages should stay unindexed, filter them in the integration config (`sitemap({ filter: (p) => !['/breakout/','/nokia/','/graphene/','/shikaku/'].includes(new URL(p).pathname) })`) and add `<meta name="robots" content="noindex">` behind a Layout prop.

---

## Open Questions

1. **GA4:** Do you have a real GA4 property/measurement ID to insert, or should the analytics block be removed?
2. **Demo indexability:** Should `/demos` and the demo pages be discoverable by search engines, or stay soft-hidden (noindex + excluded from sitemap)?
3. **Deployment:** Is Cloudflare Pages building from the Git integration on `master`? Nothing in the repo confirms the deploy path — worth a line in the README either way.
4. **`main` vs `master`:** CI triggers on both (`pr-checks.yml:5`); the repo uses `master`. Intentional, or leftover hedging worth trimming?
