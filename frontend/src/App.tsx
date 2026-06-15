import { useEffect, useMemo, useState } from "react";
import { getAuditRecords, getInventory, getNotifications } from "./api/platformApi";
import type { AuditRecord, DispenseResult, InventoryItem, NotificationMessage } from "./api/types";
import { AlertsPanel } from "./components/AlertsPanel";
import { AuditTimeline } from "./components/AuditTimeline";
import { DispenseWorkflow } from "./components/DispenseWorkflow";
import { InventoryTable } from "./components/InventoryTable";
import { MetricStrip } from "./components/MetricStrip";

export function App() {
  const [inventory, setInventory] = useState<InventoryItem[]>([]);
  const [auditRecords, setAuditRecords] = useState<AuditRecord[]>([]);
  const [notifications, setNotifications] = useState<NotificationMessage[]>([]);
  const [selectedItem, setSelectedItem] = useState<InventoryItem>();

  useEffect(() => {
    async function loadDashboard() {
      const [inventoryResponse, auditResponse, notificationResponse] = await Promise.all([
        getInventory(),
        getAuditRecords(),
        getNotifications()
      ]);

      setInventory(inventoryResponse);
      setAuditRecords(auditResponse);
      setNotifications(notificationResponse);
      setSelectedItem(inventoryResponse[0]);
    }

    loadDashboard();
  }, []);

  const selectedMedicationId = selectedItem?.medicationId ?? "";

  const lowStockNotifications = useMemo(
    () => notifications.filter(message => !message.acknowledged),
    [notifications]
  );

  function handleDispensed(result: DispenseResult) {
    setAuditRecords(current => [
      {
        id: result.dispenseId,
        occurredAt: new Date().toISOString(),
        actor: "frontend-demo",
        action: "MedicationDispensed",
        entityType: "Dispense",
        entityId: result.dispenseId,
        summary: result.message
      },
      ...current
    ]);
  }

  return (
    <main>
      <header className="app-header">
        <div>
          <p className="eyebrow">Healthcare test engineering reference app</p>
          <h1>Medication Inventory & Dispensing</h1>
        </div>
        <span className="environment-pill">React + .NET microservices</span>
      </header>

      <MetricStrip inventory={inventory} notifications={lowStockNotifications} />

      <div className="dashboard-grid">
        <InventoryTable
          inventory={inventory}
          selectedMedicationId={selectedMedicationId}
          onSelectMedication={setSelectedItem}
        />
        <DispenseWorkflow
          inventory={inventory}
          selectedItem={selectedItem}
          onDispensed={handleDispensed}
        />
        <AlertsPanel notifications={notifications} />
        <AuditTimeline records={auditRecords} />
      </div>
    </main>
  );
}
