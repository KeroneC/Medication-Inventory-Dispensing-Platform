export type InventoryItem = {
  medicationId: string;
  medicationName: string;
  strength: string;
  form: string;
  isControlledSubstance: boolean;
  requiresRefrigeration: boolean;
  locationId: string;
  locationName: string;
  quantityOnHand: number;
  reorderLevel: number;
  isLowStock: boolean;
  expirationDate: string;
  lotNumber: string;
};

export type DispenseRequest = {
  medicationId: string;
  locationId: string;
  quantity: number;
  patientId: string;
  requestedBy: string;
  careArea: string;
};

export type DispenseResult = {
  dispenseId: string;
  medicationId: string;
  locationId: string;
  quantityDispensed: number;
  quantityRemaining: number;
  lowStockDetected: boolean;
  message: string;
};

export type AuditRecord = {
  id: string;
  occurredAt: string;
  actor: string;
  action: string;
  entityType: string;
  entityId: string;
  summary: string;
};

export type NotificationMessage = {
  id: string;
  createdAt: string;
  severity: string;
  title: string;
  body: string;
  acknowledged: boolean;
};
