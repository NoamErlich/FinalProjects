import type { ThemeMode } from '../../config/config';
import './Dashboard.scss';

interface DashboardProps {
  theme: ThemeMode;
  backendUrl: string;
  appName: string;
}

const metrics = [
  { label: 'Total Users', value: '12,847', delta: '+8.3%', trend: 'up', icon: '👥' },
  { label: 'Monthly Revenue', value: '$248,391', delta: '+12.1%', trend: 'up', icon: '💰' },
  { label: 'API Uptime', value: '99.8%', delta: '-0.1%', trend: 'down', icon: '🔗' },
  { label: 'Deployments', value: '47', delta: '+3 this week', trend: 'up', icon: '🚀' },
];

const users = [
  { id: 'USR-001', name: 'Alice Chen', email: 'alice@example.com', role: 'Admin', status: 'active', joined: '2023-01-15' },
  { id: 'USR-002', name: 'Bob Martinez', email: 'bob@example.com', role: 'Developer', status: 'active', joined: '2023-03-22' },
  { id: 'USR-003', name: 'Carol Smith', email: 'carol@example.com', role: 'Viewer', status: 'inactive', joined: '2023-05-10' },
  { id: 'USR-004', name: 'Dan Okafor', email: 'dan@example.com', role: 'Developer', status: 'active', joined: '2023-07-01' },
  { id: 'USR-005', name: 'Eva Rossi', email: 'eva@example.com', role: 'Admin', status: 'active', joined: '2023-09-14' },
  { id: 'USR-006', name: 'Frank Liu', email: 'frank@example.com', role: 'Developer', status: 'pending', joined: '2024-01-03' },
];

const revenue = [
  { month: 'November', revenue: '$198,240', newUsers: 1_203, conversions: 847, growth: '+4.2%' },
  { month: 'December', revenue: '$212,880', newUsers: 1_421, conversions: 934, growth: '+7.4%' },
  { month: 'January', revenue: '$224,560', newUsers: 1_312, conversions: 902, growth: '+5.5%' },
  { month: 'February', revenue: '$230,110', newUsers: 1_489, conversions: 1_012, growth: '+2.5%' },
  { month: 'March', revenue: '$241,760', newUsers: 1_601, conversions: 1_134, growth: '+5.1%' },
  { month: 'April', revenue: '$248,391', newUsers: 1_748, conversions: 1_289, growth: '+2.7%' },
];

const deployments = [
  { id: 'DEP-047', service: 'api-gateway', status: 'success', version: 'v3.12.1', deployedAt: '2024-04-30 14:32' },
  { id: 'DEP-046', service: 'auth-service', status: 'success', version: 'v2.4.0', deployedAt: '2024-04-29 09:15' },
  { id: 'DEP-045', service: 'data-pipeline', status: 'failed', version: 'v1.8.3', deployedAt: '2024-04-28 22:48' },
  { id: 'DEP-044', service: 'frontend-app', status: 'success', version: 'v5.0.2', deployedAt: '2024-04-28 16:20' },
  { id: 'DEP-043', service: 'notification-svc', status: 'running', version: 'v1.3.7', deployedAt: '2024-04-28 11:05' },
];

const apiEndpoints = [
  { endpoint: '/api/v3/users', method: 'GET', latency: '42ms', requests: '284K/day', status: 'healthy' },
  { endpoint: '/api/v3/auth/token', method: 'POST', latency: '118ms', requests: '93K/day', status: 'healthy' },
  { endpoint: '/api/v3/payments', method: 'POST', latency: '340ms', requests: '47K/day', status: 'degraded' },
  { endpoint: '/api/v3/analytics', method: 'GET', latency: '67ms', requests: '121K/day', status: 'healthy' },
];

const statusClass: Record<string, string> = {
  active: 'badge badge--active',
  success: 'badge badge--active',
  healthy: 'badge badge--active',
  inactive: 'badge badge--inactive',
  failed: 'badge badge--failed',
  pending: 'badge badge--pending',
  running: 'badge badge--pending',
  degraded: 'badge badge--degraded',
};

export const Dashboard = ({ theme, backendUrl, appName }: DashboardProps) => {
  const themeClass = theme === 'dark' ? 'dashboard theme-dark' : 'dashboard theme-light';

  return (
    <div className={themeClass}>
      <header className="dashboard__header">
        <div className="dashboard__header-left">
          <h1 className="dashboard__title">{appName}</h1>
          <span className="dashboard__subtitle">Operations Overview</span>
        </div>
        <div className="dashboard__header-right">
          <span className="dashboard__env-badge">
            Backend: <code>{backendUrl}</code>
          </span>
          <span className="dashboard__theme-badge">{theme === 'dark' ? '🌙 Dark' : '☀️ Light'}</span>
        </div>
      </header>

      <section className="dashboard__metrics">
        {metrics.map((m) => (
          <div key={m.label} className="metric-card">
            <div className="metric-card__icon">{m.icon}</div>
            <div className="metric-card__body">
              <span className="metric-card__label">{m.label}</span>
              <span className="metric-card__value">{m.value}</span>
              <span className={`metric-card__delta metric-card__delta--${m.trend}`}>{m.delta}</span>
            </div>
          </div>
        ))}
      </section>

      <div className="dashboard__grid">
        <section className="dashboard__panel">
          <h2 className="dashboard__panel-title">Users</h2>
          <div className="table-wrapper">
            <table className="data-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Role</th>
                  <th>Status</th>
                  <th>Joined</th>
                </tr>
              </thead>
              <tbody>
                {users.map((u) => (
                  <tr key={u.id}>
                    <td className="data-table__id">{u.id}</td>
                    <td className="data-table__name">{u.name}</td>
                    <td className="data-table__muted">{u.email}</td>
                    <td>{u.role}</td>
                    <td><span className={statusClass[u.status] ?? 'badge'}>{u.status}</span></td>
                    <td className="data-table__muted">{u.joined}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <section className="dashboard__panel">
          <h2 className="dashboard__panel-title">API Health</h2>
          <div className="table-wrapper">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Endpoint</th>
                  <th>Method</th>
                  <th>Latency</th>
                  <th>Req/Day</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {apiEndpoints.map((e) => (
                  <tr key={e.endpoint}>
                    <td><code className="data-table__code">{e.endpoint}</code></td>
                    <td><span className="method-badge">{e.method}</span></td>
                    <td>{e.latency}</td>
                    <td className="data-table__muted">{e.requests}</td>
                    <td><span className={statusClass[e.status] ?? 'badge'}>{e.status}</span></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <section className="dashboard__panel">
          <h2 className="dashboard__panel-title">Revenue Analytics</h2>
          <div className="table-wrapper">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Month</th>
                  <th>Revenue</th>
                  <th>New Users</th>
                  <th>Conversions</th>
                  <th>Growth</th>
                </tr>
              </thead>
              <tbody>
                {revenue.map((r) => (
                  <tr key={r.month}>
                    <td>{r.month}</td>
                    <td className="data-table__value">{r.revenue}</td>
                    <td>{r.newUsers.toLocaleString()}</td>
                    <td>{r.conversions.toLocaleString()}</td>
                    <td className="data-table__growth">{r.growth}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <section className="dashboard__panel">
          <h2 className="dashboard__panel-title">Recent Deployments</h2>
          <div className="table-wrapper">
            <table className="data-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Service</th>
                  <th>Version</th>
                  <th>Status</th>
                  <th>Deployed At</th>
                </tr>
              </thead>
              <tbody>
                {deployments.map((d) => (
                  <tr key={d.id}>
                    <td className="data-table__id">{d.id}</td>
                    <td><code className="data-table__code">{d.service}</code></td>
                    <td className="data-table__muted">{d.version}</td>
                    <td><span className={statusClass[d.status] ?? 'badge'}>{d.status}</span></td>
                    <td className="data-table__muted">{d.deployedAt}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      </div>

      <footer className="dashboard__footer">
        <span>R2 Dashboard · Shadow DOM isolated · Runtime env via window.__env__</span>
      </footer>
    </div>
  );
};
