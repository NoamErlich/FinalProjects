import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'path';

export default defineConfig(({ mode }) => {
  const isWebComponent = mode === 'webcomponent';

  return {
    plugins: [react()],
    build: {
      outDir: isWebComponent ? 'wc-dist' : 'dist',
      emptyOutDir: true,
      ...(isWebComponent
        ? {
            lib: {
              entry: resolve(__dirname, 'src/r2webComponentEntryPoint.tsx'),
              name: 'R2DashboardWidget',
              formats: ['es'],
              fileName: () => 'r2-dashboard-widget.js',
            },
            rollupOptions: {
              // React is bundled INTO the web component so the host
              // app doesn't need React at all — true framework isolation.
              external: [],
              output: {
                inlineDynamicImports: true,
              },
            },
            cssCodeSplit: false,
          }
        : {}),
    },
  };
});
