import type { InventoryItem, NotificationMessage } from "../api/types";

type MetricStripProps = {
  inventory: InventoryItem[];
  notifications: NotificationMessage[];
};

export function MetricStrip({ inventory, notifications }: MetricStripProps) {
  const lowStockCount = inventory.filter(item => item.isLowStock).length;
  const controlledCount = inventory.filter(item => item.isControlledSubstance).length;
  const openAlerts = notifications.filter(message => !message.acknowledged).length;

  return (
    <section className="metric-strip" aria-label="Operational summary">
      <div>
        <span className="metric-value">{inventory.length}</span>
        <span className="metric-label">Stock records</span>
      </div>
      <div>
        <span className="metric-value warning">{lowStockCount}</span>
        <span className="metric-label">Low-stock items</span>
      </div>
      <div>
        <span className="metric-value">{controlledCount}</span>
        <span className="metric-label">Controlled meds</span>
      </div>
      <div>
        <span className="metric-value alert">{openAlerts}</span>
        <span className="metric-label">Open alerts</span>
      </div>
    </section>
  );
}
