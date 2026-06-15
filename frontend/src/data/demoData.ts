import type { AuditRecord, InventoryItem, NotificationMessage } from "../api/types";

export const demoInventory: InventoryItem[] = [
  {
    medicationId: "11111111-1111-1111-1111-111111111111",
    medicationName: "Acetaminophen",
    strength: "500 mg",
    form: "Tablet",
    isControlledSubstance: false,
    requiresRefrigeration: false,
    locationId: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    locationName: "Central Pharmacy",
    quantityOnHand: 240,
    reorderLevel: 80,
    isLowStock: false,
    expirationDate: "2027-10-15",
    lotNumber: "LOT-ACET-240"
  },
  {
    medicationId: "22222222-2222-2222-2222-222222222222",
    medicationName: "Morphine Sulfate",
    strength: "2 mg/mL",
    form: "Injection",
    isControlledSubstance: true,
    requiresRefrigeration: false,
    locationId: "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    locationName: "Emergency Department Cabinet",
    quantityOnHand: 12,
    reorderLevel: 10,
    isLowStock: false,
    expirationDate: "2026-12-15",
    lotNumber: "LOT-MOR-012"
  },
  {
    medicationId: "33333333-3333-3333-3333-333333333333",
    medicationName: "Insulin Glargine",
    strength: "100 units/mL",
    form: "Pen",
    isControlledSubstance: false,
    requiresRefrigeration: true,
    locationId: "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    locationName: "Emergency Department Cabinet",
    quantityOnHand: 7,
    reorderLevel: 8,
    isLowStock: true,
    expirationDate: "2026-10-15",
    lotNumber: "LOT-INS-007"
  }
];

export const demoAuditRecords: AuditRecord[] = [
  {
    id: "audit-1",
    occurredAt: new Date(Date.now() - 1000 * 60 * 25).toISOString(),
    actor: "system",
    action: "LowStockDetected",
    entityType: "Inventory",
    entityId: "33333333-3333-3333-3333-333333333333",
    summary: "Insulin Glargine fell below the reorder level in the Emergency Department Cabinet."
  }
];

export const demoNotifications: NotificationMessage[] = [
  {
    id: "notification-1",
    createdAt: new Date(Date.now() - 1000 * 60 * 15).toISOString(),
    severity: "Warning",
    title: "Insulin reorder needed",
    body: "Emergency Department Cabinet has 7 units on hand with a reorder level of 8.",
    acknowledged: false
  }
];
