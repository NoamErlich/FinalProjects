# r2-web-component-demo

Two React + TypeScript + Vite + SCSS projects demonstrating:

- A React dashboard that builds as both a normal SPA **and** a Shadow DOM Web Component
- A host app that consumes the web component and proves full CSS isolation
- Runtime env injection via `window.__env__` (no rebuild needed per environment)
- Microfrontend architecture patterns

---

## Project 1 — `r2-web-component`

The dashboard widget. Can run as a standalone React app or be compiled into a self-contained custom element (`<r2-dashboard-widget>`).

### Run as normal app

```bash
cd r2-web-component
pnpm install
pnpm dev          # http://localhost:5173
```

### Build as Web Component

```bash
pnpm run build:web-component
# Output: wc-dist/r2-dashboard-widget.js
```

The output is a single ES module that self-registers `<r2-dashboard-widget>`. It bundles React, so the host page needs no React dependency.

### Build as standard SPA

```bash
pnpm run build
# Output: dist/
```

---

## Project 2 — `r2-host-app`

The consumer. Loads the web component bundle and renders it inside a page full of intentionally ugly global CSS.

### Setup

```bash
# 1. Build the web component first
cd r2-web-component
pnpm run build:web-component

# 2. Copy the bundle to the host app's public folder
cp wc-dist/r2-dashboard-widget.js ../r2-host-app/public/

# 3. Run the host app
cd ../r2-host-app
pnpm install
pnpm dev          # http://localhost:5174
```

---

## Runtime Environment

Both projects read `window.__env__` which is set by `public/config.js` **before** the app bundle loads. Edit this file to change env values without rebuilding:

```js
// public/config.js
window.__env__ = {
  BACKEND_URL: 'https://api.example.com',
  THEME_MODE: 'dark',   // 'light' | 'dark'
  APP_NAME: 'R2 Dashboard',
};
```

In production, your deployment pipeline (Docker, Kubernetes ConfigMap, CI/CD) replaces this single file per environment.

---

## Web Component API

```html
<!-- Uses window.__env__.THEME_MODE by default -->
<r2-dashboard-widget></r2-dashboard-widget>

<!-- Override theme via attribute -->
<r2-dashboard-widget theme="light"></r2-dashboard-widget>
<r2-dashboard-widget theme="dark"></r2-dashboard-widget>
```

The component reacts to `theme` attribute changes via `attributeChangedCallback`.

---

## Why CSS isolation works

The browser enforces a strict style boundary at each shadow root. External `<style>` tags, `<link>` stylesheets, and the document cascade **cannot pierce** into a shadow root. The web component's styles live inside the shadow root (injected via a `<style>` node created in `connectedCallback`) so they are equally invisible to the outside world.

The host app's aggressive global CSS (`table { border: 6px solid red !important }`, etc.) visually corrupts the native React table. The web component's tables are completely unaffected — you can verify this in the browser.

---

## Architecture — Single `vite.config.ts` with mode switching

```
pnpm run dev                    # mode = development → normal Vite dev server
pnpm run build                  # mode = production  → outDir: dist/
pnpm run build:web-component    # mode = webcomponent → lib build, outDir: wc-dist/
```

One config file handles all three cases. A separate config would only make sense if the build toolchain itself differed (e.g., SSR vs CSR).
