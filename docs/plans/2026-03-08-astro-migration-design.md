# Astro Migration Design

**Date:** 2026-03-08
**Status:** Approved

## Goal

Convert NeilGaietto.com from a plain HTML/CSS static site to Astro, gaining a modern dev workflow (hot reload, TypeScript, build pipeline) while maintaining pixel-perfect visual fidelity. Deploy target: Cloudflare Pages.

## Current State

- `src/html/index.html` — landing page with inline CSS and embedded meta/structured data
- `src/html/error.html` — 404 page
- `src/html/assets/bg.jpg`, `neil.jpg` — images
- `src/html/sitemap.xml` — XML sitemap
- No build tooling, no dependencies

## Chosen Approach: Minimal Astro (Option A)

Straight conversion to `.astro` files. No UI framework. No Tailwind. CSS extracted verbatim to preserve visual fidelity. TypeScript in frontmatter.

## File Structure

```
NeilGaiettoCom/
├── src/
│   ├── layouts/
│   │   └── Layout.astro      # Shared <head>: meta, OG, JSON-LD, fonts, GA, styles
│   ├── pages/
│   │   ├── index.astro       # Landing page content
│   │   └── 404.astro         # Error page content
│   └── styles/
│       └── global.css        # All CSS extracted from current inline <style> tags
├── public/
│   ├── sitemap.xml           # Copied as-is
│   ├── bg.jpg
│   └── neil.jpg
├── astro.config.mjs
├── tsconfig.json
└── package.json
```

## Key Decisions

- **Output mode:** `static` (default). No `@astrojs/cloudflare` adapter needed — Cloudflare Pages serves the `dist/` folder directly.
- **CSS strategy:** Extract inline `<style>` blocks verbatim into `src/styles/global.css`. Import in `Layout.astro`. No rewriting.
- **TypeScript:** Used in Astro frontmatter for page metadata (title, description, etc.) as typed props passed to `Layout.astro`.
- **GA4 migration:** Migrate from Universal Analytics (UA-29368003-1) to GA4 as part of this work, since the analytics script lives in `Layout.astro`.
- **Images:** Moved to `public/` so Astro serves them at the root path unchanged.
- **Sitemap:** Copied to `public/` as-is.

## Deployment

Build command: `npm run build` (outputs to `dist/`)
Cloudflare Pages: point to `dist/` directory, no special adapter required.
