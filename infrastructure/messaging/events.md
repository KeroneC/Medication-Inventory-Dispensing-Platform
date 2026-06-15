# Message Contracts

These contracts are represented in C# under `MedicationPlatform.Common/Events`.

## MedicationDispensedEvent

Published when a dispense workflow succeeds.

```json
{
  "eventId": "7fe063c0-11af-4758-a5b4-952e134d8cf1",
  "occurredAt": "2026-06-15T13:30:00Z",
  "dispenseId": "d4bbafc5-85bd-4753-a134-611786ef07b5",
  "medicationId": "22222222-2222-2222-2222-222222222222",
  "locationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "quantity": 1,
  "patientId": "PT-1042",
  "requestedBy": "nurse.jordan"
}
```

## LowStockDetectedEvent

Published when a stock quantity reaches or falls below the reorder level.

## RestockRequestedEvent

Published when stock is replenished or restock is requested.
