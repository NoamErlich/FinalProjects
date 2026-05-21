import './HostPage.scss';
import '../../types/web-components.d.ts';

const nativeTableData = [
  { id: 1, product: 'Widget Alpha', category: 'Hardware', price: '$49.99', stock: 120 },
  { id: 2, product: 'Widget Beta', category: 'Software', price: '$29.99', stock: 340 },
  { id: 3, product: 'Widget Gamma', category: 'Services', price: '$199.00', stock: 8 },
  { id: 4, product: 'Widget Delta', category: 'Hardware', price: '$79.50', stock: 55 },
];

export const HostPage = () => {
  return (
    <div className="host-page">
      <header className="host-header">
        <h1>Web Component Isolation Test</h1>
        <p>
          This page intentionally applies <strong>aggressive global CSS</strong> to prove
          that the <code>&lt;r2-dashboard-widget&gt;</code> Web Component is fully
          style-isolated via Shadow DOM.
        </p>
      </header>

      <section className="host-section">
        <div className="host-section__header">
          <span className="host-section__badge host-section__badge--warning">AFFECTED</span>
          <h2>Native React Table — Corrupted by Host CSS</h2>
        </div>
        <p className="host-section__description">
          The global styles below target <code>table</code>, <code>th</code>, <code>td</code>,
          and <code>*</code> selectors. This table lives in the normal DOM so it gets hit by
          all of them — red borders, yellow background, Comic Sans, purple text.
        </p>
        <div className="native-table-wrapper">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Product</th>
                <th>Category</th>
                <th>Price</th>
                <th>Stock</th>
              </tr>
            </thead>
            <tbody>
              {nativeTableData.map((row) => (
                <tr key={row.id}>
                  <td>{row.id}</td>
                  <td>{row.product}</td>
                  <td>{row.category}</td>
                  <td>{row.price}</td>
                  <td>{row.stock}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      <div className="host-divider">
        <span>↓ Shadow DOM boundary — global CSS cannot cross this line ↓</span>
      </div>

      <section className="host-section">
        <div className="host-section__header">
          <span className="host-section__badge host-section__badge--success">ISOLATED</span>
          <h2>r2-dashboard-widget — Pristine Shadow DOM</h2>
        </div>
        <p className="host-section__description">
          The exact same global CSS rules are active. The web component below has its own
          Shadow DOM boundary — none of the host styles can pierce it. The component renders
          its own clean, dark-themed dashboard using styles injected directly into the shadow root.
        </p>

        {/*
          The custom element is used exactly like a native HTML element.
          The `theme` attribute is forwarded to the component at runtime.
          Try changing it to "light" — the host CSS still won't affect it.
        */}
        <div className="web-component-wrapper">
          <r2-dashboard-widget theme="dark" />
        </div>
      </section>

      <section className="host-section host-section--explanation">
        <h2>Why does this work?</h2>
        <div className="explanation-grid">
          <div className="explanation-card">
            <h3>Shadow DOM</h3>
            <p>
              The browser maintains a separate style scope inside each shadow root.
              External <code>&lt;style&gt;</code> and <code>&lt;link&gt;</code> tags cannot
              pierce the shadow boundary. Neither can the cascade from the document stylesheet.
            </p>
          </div>
          <div className="explanation-card">
            <h3>Injected Styles</h3>
            <p>
              Vite's <code>?inline</code> import query compiles the SCSS to a plain string.
              The web component appends a <code>&lt;style&gt;</code> node directly to its
              shadow root — styles live inside the boundary, never in <code>&lt;head&gt;</code>.
            </p>
          </div>
          <div className="explanation-card">
            <h3>Runtime Env</h3>
            <p>
              Both the host app and the web component read <code>window.__env__</code> which
              is populated by <code>/config.js</code> — a file swapped per environment
              by the deployment pipeline, no rebuild needed.
            </p>
          </div>
          <div className="explanation-card">
            <h3>Microfrontend Pattern</h3>
            <p>
              This web component is framework-agnostic. You could embed it in an Angular, Vue,
              or vanilla HTML page without any changes. It ships with its own React runtime,
              styles, and state — fully self-contained.
            </p>
          </div>
        </div>
      </section>
    </div>
  );
};
