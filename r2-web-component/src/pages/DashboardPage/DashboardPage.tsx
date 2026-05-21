import type { ThemeMode } from '../../config/config';
import { config } from '../../config/config';
import { Dashboard } from '../../components/Dashboard/Dashboard';
import './DashboardPage.scss';

interface DashboardPageProps {
  themeOverride?: ThemeMode;
}

export const DashboardPage = ({ themeOverride }: DashboardPageProps) => {
  const theme = themeOverride ?? config.themeMode;

  return (
    <div className="dashboard-page">
      <Dashboard
        theme={theme}
        backendUrl={config.backendUrl}
        appName={config.appName}
      />
    </div>
  );
};
