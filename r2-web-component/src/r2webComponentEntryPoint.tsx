import React from 'react';
import ReactDOM from 'react-dom/client';
import type { ThemeMode } from './config/config';
import { DashboardPage } from './pages/DashboardPage/DashboardPage';

// ?inline tells Vite to return the compiled CSS as a plain string instead of
// injecting it into the document <head>. We then manually place that string
// inside the Shadow DOM so it never leaks out — and external styles never
// leak in either.
import dashboardStyles from './components/Dashboard/Dashboard.scss?inline';
import dashboardPageStyles from './pages/DashboardPage/DashboardPage.scss?inline';

const ALL_STYLES = dashboardStyles + '\n' + dashboardPageStyles;

class R2DashboardWidget extends HTMLElement {
  private root: ReactDOM.Root | null = null;
  private shadow: ShadowRoot;

  // The browser calls attributeChangedCallback only for attributes listed here.
  static get observedAttributes(): string[] {
    return ['theme'];
  }

  constructor() {
    super();
    // mode: 'open' lets DevTools inspect the shadow tree; use 'closed' to
    // prevent external JS from accessing this.shadowRoot.
    this.shadow = this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    // Inject styles. Creating a real <style> node (not adoptedStyleSheets) is
    // the most broadly compatible approach and works in all modern browsers.
    const styleEl = document.createElement('style');
    styleEl.textContent = ALL_STYLES;

    // Mount point for React — gives us a predictable DOM node to render into.
    const mountPoint = document.createElement('div');
    mountPoint.style.cssText = 'width:100%;min-height:100%;';

    this.shadow.appendChild(styleEl);
    this.shadow.appendChild(mountPoint);

    this.root = ReactDOM.createRoot(mountPoint);
    this.renderReact();
  }

  attributeChangedCallback(_name: string, oldVal: string | null, newVal: string | null) {
    if (oldVal !== newVal && this.root) {
      this.renderReact();
    }
  }

  disconnectedCallback() {
    // Clean up the React tree to avoid memory leaks.
    this.root?.unmount();
    this.root = null;
  }

  private resolveTheme(): ThemeMode {
    const attr = this.getAttribute('theme');
    if (attr === 'dark' || attr === 'light') return attr;
    // Fall back to runtime env, then to 'light'.
    return window.__env__?.THEME_MODE ?? 'light';
  }

  private renderReact() {
    this.root?.render(
      React.createElement(DashboardPage, { themeOverride: this.resolveTheme() }),
    );
  }
}

// Guard against double registration during HMR in development.
if (!customElements.get('r2-dashboard-widget')) {
  customElements.define('r2-dashboard-widget', R2DashboardWidget);
}
