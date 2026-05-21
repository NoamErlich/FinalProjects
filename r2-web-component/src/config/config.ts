declare global {
  interface Window {
    __env__?: {
      BACKEND_URL?: string;
      THEME_MODE?: 'light' | 'dark';
      APP_NAME?: string;
    };
  }
}

export const config = {
  backendUrl: window.__env__?.BACKEND_URL ?? 'http://localhost:3000',
  themeMode: (window.__env__?.THEME_MODE ?? 'light') as 'light' | 'dark',
  appName: window.__env__?.APP_NAME ?? 'R2 Dashboard',
} as const;

export type AppConfig = typeof config;
export type ThemeMode = AppConfig['themeMode'];
