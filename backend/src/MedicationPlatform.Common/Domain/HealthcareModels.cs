namespace MedicationPlatform.Common.Domain;

public sealed record Medication(
    Guid Id,
    string Ndc,
    string Name,
    string Strength,
    string Form,
    bool IsControlledSubstance,
    bool RequiresRefrigeration);

public sealed record InventoryLocation(
    Guid Id,
    string Name,
    string Type);

public sealed record StockItem(
    Guid MedicationId,
    Guid LocationId,
    int QuantityOnHand,
    int ReorderLevel,
    DateOnly ExpirationDate,
    string LotNumber);

public sealed record RestockRequest(
    Guid MedicationId,
    Guid LocationId,
    int Quantity,
    string PerformedBy,
    string Reason);

public sealed record DispenseRequest(
    Guid MedicationId,
    Guid LocationId,
    int Quantity,
    string PatientId,
    string RequestedBy,
    string CareArea);

public sealed record DispenseResult(
    Guid DispenseId,
    Guid MedicationId,
    Guid LocationId,
    int QuantityDispensed,
    int QuantityRemaining,
    bool LowStockDetected,
    string Message);

public sealed record AuditRecord(
    Guid Id,
    DateTimeOffset OccurredAt,
    string Actor,
    string Action,
    string EntityType,
    string EntityId,
    string Summary);

public sealed record NotificationMessage(
    Guid Id,
    DateTimeOffset CreatedAt,
    string Severity,
    string Title,
    string Body,
    bool Acknowledged);
