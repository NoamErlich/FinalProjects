/**
 * Runtime environment configuration.
 *
 * This file is intentionally NOT bundled by Vite. It lives in /public
 * so it is served as-is. Replace its contents per environment:
 *
 *   Docker:      mount a different config.js via volume
 *   Kubernetes:  use a ConfigMap mounted at /usr/share/nginx/html/config.js
 *   CI/CD:       sed-replace values in this file during the deploy step
 *
 * The React app reads window.__env__ at runtime, so the same compiled
 * bundle works in dev, staging, and production — only this file changes.
 */
window.__env__ = {
  BACKEND_URL: 'https://api.example.com',
  THEME_MODE: 'dark',
  APP_NAME: 'R2 Dashboard',
};
