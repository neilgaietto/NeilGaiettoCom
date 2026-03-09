# Astro Migration Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Convert NeilGaietto.com from a plain HTML/CSS static site to Astro with TypeScript, targeting Cloudflare Pages deployment, preserving pixel-perfect visual fidelity.

**Architecture:** Minimal Astro (no UI framework). Shared `Layout.astro` handles `<head>`, meta, fonts, and analytics. Two pages: `index.astro` and `404.astro`. All CSS extracted verbatim to `global.css`.

**Tech Stack:** Astro 5, TypeScript, Cloudflare Pages (static output, no adapter required)

---

## Before You Start

- The old source lives in `src/html/`. You will not delete it until the end.
- Images must go in `public/assets/` so the existing CSS path `url(assets/bg.jpg)` stays valid.
- Astro generates `404.html` from `src/pages/404.astro` automatically — Cloudflare Pages serves this for 404s.
- GA4 measurement ID: you need a `G-XXXXXXXXXX` ID from Neil's Google Analytics account. Replace the placeholder `G-XXXXXXXXXX` in Task 4 with the real ID before committing.

---

### Task 1: Scaffold Astro project

**Files:**
- Create: `astro.config.mjs`
- Create: `tsconfig.json`
- Create: `package.json`

**Step 1: Initialize Astro**

Run from `C:/Projects/NeilGaiettoCom`:

```bash
npm create astro@latest . -- --template minimal --typescript strict --no-install --no-git
```

When prompted:
- "Where should we create your new project?" → `.` (current directory)
- Accept all defaults

**Step 2: Install dependencies**

```bash
npm install
```

Expected: `node_modules/` created, no errors.

**Step 3: Verify the scaffold runs**

```bash
npm run dev
```

Expected: Dev server starts at `http://localhost:4321`. Open it — you'll see a blank Astro starter page. Stop with Ctrl+C.

**Step 4: Replace `astro.config.mjs` with minimal config**

```js
// astro.config.mjs
import { defineConfig } from 'astro/config';

export default defineConfig({
  site: 'https://neilgaietto.com',
});
```

**Step 5: Commit**

```bash
git add package.json package-lock.json astro.config.mjs tsconfig.json .gitignore
git commit -m "feat: scaffold Astro project"
```

---

### Task 2: Create global CSS

**Files:**
- Create: `src/styles/global.css`

**Step 1: Create the styles directory and file**

Create `src/styles/global.css` with the combined CSS from both HTML files. The index page has the full set; error.html is a subset. Combine them — all rules, no omissions:

```css
*, *::before, *::after {
    box-sizing: border-box;
}

body {
    width: 100%;
    min-height: 100vh;
    font-family: 'Maitree', serif;
    background-image: url(assets/bg.jpg);
    background-position: center;
    background-size: cover;
    background-attachment: fixed;
    color: white;
    margin: 0;
    padding: 0;
    display: flex;
    align-items: center;
    justify-content: center;
}

h1 {
    margin-top: 0;
    font-size: 2em;
}

h3 {
    font-size: 1.25em;
    font-weight: normal;
}

p {
    font-size: 1.1em;
}

a,
a:link,
a:visited {
    color: white;
    font-style: italic;
}

a:hover,
a:focus {
    opacity: 0.85;
}

main {
    text-align: center;
    background: rgba(0, 0, 0, 0.55);
    border-radius: 10px;
    padding: 2em 2.5em;
    backdrop-filter: blur(4px);
    -webkit-backdrop-filter: blur(4px);
}

img.neil {
    border-radius: 50%;
    width: 120px;
    height: 120px;
    object-fit: cover;
}

@media screen and (orientation: portrait) {
    body {
        font-size: 16px;
    }
    main {
        margin: 2em 1.5em;
        max-width: 90vw;
    }
}

@media screen and (orientation: landscape) {
    body {
        font-size: 18px;
    }
    main {
        max-width: 480px;
    }
}
```

**Step 2: Commit**

```bash
git add src/styles/global.css
git commit -m "feat: extract CSS to global.css"
```

---

### Task 3: Copy images and sitemap to public/

**Files:**
- Create: `public/assets/bg.jpg` (copy from `src/html/assets/bg.jpg`)
- Create: `public/assets/neil.jpg` (copy from `src/html/assets/neil.jpg`)
- Create: `public/sitemap.xml` (copy from `src/html/sitemap.xml`)

**Step 1: Copy files**

```bash
mkdir -p public/assets
cp src/html/assets/bg.jpg public/assets/bg.jpg
cp src/html/assets/neil.jpg public/assets/neil.jpg
cp src/html/sitemap.xml public/sitemap.xml
```

**Step 2: Commit**

```bash
git add public/
git commit -m "feat: copy images and sitemap to public/"
```

---

### Task 4: Create Layout.astro

**Files:**
- Create: `src/layouts/Layout.astro`

**Step 1: Get your GA4 measurement ID**

Go to [Google Analytics](https://analytics.google.com) → Admin → create a new GA4 property for neilgaietto.com if you haven't. Copy the Measurement ID (format: `G-XXXXXXXXXX`).

**Step 2: Create `src/layouts/Layout.astro`**

Replace `G-XXXXXXXXXX` with your real GA4 Measurement ID:

```astro
---
interface Props {
  title: string;
  description: string;
  includeSchema?: boolean;
}

const { title, description, includeSchema = false } = Astro.props;
const GA4_ID = 'G-XXXXXXXXXX'; // Replace with your real GA4 Measurement ID
---

<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{title}</title>
    <meta name="description" content={description}>

    <!-- Open Graph -->
    <meta property="og:title" content={title}>
    <meta property="og:description" content="Software Architect and Developer based in Apex, NC.">
    <meta property="og:image" content="https://neilgaietto.com/assets/neil.jpg">
    <meta property="og:url" content="https://neilgaietto.com">
    <meta property="og:type" content="website">

    <!-- Twitter Card -->
    <meta name="twitter:card" content="summary">
    <meta name="twitter:title" content={title}>
    <meta name="twitter:description" content="Software Architect and Developer based in Apex, NC.">
    <meta name="twitter:image" content="https://neilgaietto.com/assets/neil.jpg">

    {includeSchema && (
      <script type="application/ld+json" set:html={JSON.stringify({
        "@context": "https://schema.org",
        "@type": "Person",
        "address": {
          "@type": "PostalAddress",
          "addressLocality": "Apex",
          "addressRegion": "NC"
        },
        "email": "neil.gaietto@gmail.com",
        "jobTitle": "Software Architect/Developer",
        "name": "Neil Gaietto",
        "image": "https://neilgaietto.com/assets/neil.jpg",
        "gender": "male",
        "url": "https://neilgaietto.com",
        "sameAs": [
          "https://www.linkedin.com/in/neilgaietto/",
          "https://github.com/neilgaietto",
          "https://www.facebook.com/neil.gaietto"
        ]
      })} />
    )}

    <link href="https://fonts.googleapis.com/css2?family=Maitree&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="/src/styles/global.css">

    <!-- Google Analytics 4 -->
    <script async src={`https://www.googletagmanager.com/gtag/js?id=${GA4_ID}`}></script>
    <script define:vars={{ GA4_ID }}>
      window.dataLayer = window.dataLayer || [];
      function gtag() { dataLayer.push(arguments); }
      gtag('js', new Date());
      gtag('config', GA4_ID);
    </script>
</head>
<body>
    <slot />
</body>
</html>
```

**Step 3: Commit**

```bash
git add src/layouts/Layout.astro
git commit -m "feat: add Layout.astro with GA4 and typed props"
```

---

### Task 5: Create index.astro

**Files:**
- Create: `src/pages/index.astro`
- Delete: `src/pages/index.astro` from Astro scaffold (if it exists with placeholder content — replace it)

**Step 1: Create `src/pages/index.astro`**

```astro
---
import Layout from '../layouts/Layout.astro';
---

<Layout
  title="Neil Gaietto - Software Architect and Developer"
  description="Neil Gaietto is a Software Architect and Developer based in Apex, NC. Get in touch via email or LinkedIn."
  includeSchema={true}
>
  <main>
    <img class="neil" src="/assets/neil.jpg" alt="Photo of Neil Gaietto" title="Neil Gaietto" width="120" height="120" />
    <h1>Hi, I'm Neil Gaietto</h1>
    <h3>Software Architect and Developer</h3>
    <p>Get in touch via <a href="mailto:neil.gaietto@gmail.com?subject=Hello, from your site!" target="_blank" rel="noopener noreferrer">email</a> or <a href="https://www.linkedin.com/in/neilgaietto/" target="_blank" rel="noopener noreferrer">LinkedIn</a></p>
  </main>
  <!--Photo by Marita Kavelashvili on Unsplash-->
</Layout>
```

**Step 2: Start dev server and verify visually**

```bash
npm run dev
```

Open `http://localhost:4321`. Confirm:
- Background image fills the screen
- Profile photo is circular
- Name, title, and links appear in the semi-transparent card
- Font is Maitree serif
- Links are white and italic

Stop with Ctrl+C.

**Step 3: Commit**

```bash
git add src/pages/index.astro
git commit -m "feat: add index.astro landing page"
```

---

### Task 6: Create 404.astro

**Files:**
- Create: `src/pages/404.astro`

**Step 1: Create `src/pages/404.astro`**

```astro
---
import Layout from '../layouts/Layout.astro';
---

<Layout
  title="Page Not Found - Neil Gaietto"
  description="The page you're looking for doesn't exist."
>
  <main>
    <h1>Page Not Found</h1>
    <p>The page you're looking for doesn't exist.</p>
    <p><a href="/">Back to neilgaietto.com</a></p>
  </main>
</Layout>
```

**Step 2: Verify in dev server**

```bash
npm run dev
```

Navigate to `http://localhost:4321/anything-fake`. Confirm the 404 page renders with the same background and card style. Stop with Ctrl+C.

**Step 3: Commit**

```bash
git add src/pages/404.astro
git commit -m "feat: add 404.astro error page"
```

---

### Task 7: Fix CSS import for build

**Context:** In Task 4 we used `<link rel="stylesheet" href="/src/styles/global.css">`. This works in dev but Astro doesn't serve raw `src/` files in the build. The correct approach for Astro is to import the CSS in the frontmatter.

**Files:**
- Modify: `src/layouts/Layout.astro`

**Step 1: Replace the CSS link tag with a frontmatter import**

In `src/layouts/Layout.astro`, add this import to the frontmatter (inside the `---` block):

```ts
import '../styles/global.css';
```

And remove this line from `<head>`:

```html
<link rel="stylesheet" href="/src/styles/global.css">
```

**Step 2: Build and verify**

```bash
npm run build
```

Expected: `dist/` directory created, no errors.

```bash
npm run preview
```

Open `http://localhost:4321`. Visually verify the site looks identical to the original. Stop with Ctrl+C.

**Step 3: Commit**

```bash
git add src/layouts/Layout.astro
git commit -m "fix: import global.css in frontmatter for correct build output"
```

---

### Task 8: Remove old HTML source and update README

**Files:**
- Delete: `src/html/` (entire directory)
- Modify: `README.md`
- Modify: `TODO.md`

**Step 1: Remove old source**

```bash
rm -rf src/html/
```

**Step 2: Update README.md**

Replace contents with:

```markdown
# NeilGaietto.com

Personal landing page for Neil Gaietto.

**Stack:** Astro, TypeScript
**Hosting:** Cloudflare Pages
**Build:** `npm run build` → deploys `dist/`
```

**Step 3: Update TODO.md**

Remove the GA4 migration task (completed in Task 4). If there are no remaining items, you can delete the file:

```bash
rm TODO.md
```

**Step 4: Final build check**

```bash
npm run build
```

Expected: clean build, no errors or warnings about missing files.

**Step 5: Commit**

```bash
git add -A
git commit -m "chore: remove old HTML source, update README"
```

---

### Task 9: Cloudflare Pages deployment config

**Files:**
- No new files needed — Cloudflare Pages is configured in the dashboard

**Step 1: Push the branch**

```bash
git push origin claude/improve-landing-page-Mew5h
```

**Step 2: Configure Cloudflare Pages**

In the Cloudflare Pages dashboard:
- Build command: `npm run build`
- Build output directory: `dist`
- Node.js version: 20 (set in Environment Variables: `NODE_VERSION = 20`)

**Step 3: Create a PR to merge to master**

```bash
gh pr create --title "Convert site to Astro with TypeScript" --body "$(cat <<'EOF'
## Summary
- Converts plain HTML/CSS site to Astro with TypeScript
- Shared Layout.astro handles head, meta, fonts, GA4
- Pixel-perfect visual match to original design
- Migrates Google Analytics from UA to GA4
- Configured for Cloudflare Pages static deployment

## Test plan
- [ ] Visual check: landing page matches original design
- [ ] Visual check: 404 page works and matches design
- [ ] `npm run build` completes without errors
- [ ] Cloudflare Pages build succeeds
- [ ] GA4 events appear in Google Analytics dashboard after deploy
EOF
)"
```
