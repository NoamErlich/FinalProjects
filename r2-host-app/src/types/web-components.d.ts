import type React from 'react';

// Teach TypeScript about the custom element so JSX is type-safe.
declare global {
  namespace JSX {
    interface IntrinsicElements {
      'r2-dashboard-widget': React.DetailedHTMLProps<
        React.HTMLAttributes<HTMLElement>,
        HTMLElement
      > & {
        theme?: 'light' | 'dark';
      };
    }
  }
}
