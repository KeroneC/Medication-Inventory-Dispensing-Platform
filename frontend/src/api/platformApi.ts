import type {
  AuditRecord,
  DispenseRequest,
  DispenseResult,
  InventoryItem,
  NotificationMessage
} from "./types";
import { demoAuditRecords, demoInventory, demoNotifications } from "../data/demoData";

const inventoryBaseUrl = import.meta.env.VITE_INVENTORY_API_URL ?? "http://127.0.0.1:5101";
const dispensingBaseUrl = import.meta.env.VITE_DISPENSING_API_URL ?? "http://127.0.0.1:5102";
const auditBaseUrl = import.meta.env.VITE_AUDIT_API_URL ?? "http://127.0.0.1:5103";
const notificationBaseUrl = import.meta.env.VITE_NOTIFICATION_API_URL ?? "http://127.0.0.1:5104";

async function getJson<T>(url: string, fallback: T): Promise<T> {
  try {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`);
    }

    return await response.json() as T;
  } catch {
    // The UI remains useful even before the learner has started every service.
    return fallback;
  }
}

export async function getInventory(): Promise<InventoryItem[]> {
  return getJson<InventoryItem[]>(`${inventoryBaseUrl}/inventory`, demoInventory);
}

export async function getAuditRecords(): Promise<AuditRecord[]> {
  return getJson<AuditRecord[]>(`${auditBaseUrl}/audit-logs`, demoAuditRecords);
}

export async function getNotifications(): Promise<NotificationMessage[]> {
  return getJson<NotificationMessage[]>(`${notificationBaseUrl}/notifications`, demoNotifications);
}

export async function submitDispense(request: DispenseRequest): Promise<DispenseResult> {
  const response = await fetch(`${dispensingBaseUrl}/dispenses`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(request)
  });

  if (!response.ok) {
    const problem = await response.json().catch(() => ({ error: "Dispense request failed." }));
    throw new Error(problem.error ?? "Dispense request failed.");
  }

  return await response.json() as DispenseResult;
}
