namespace MedicationPlatform.Common.Events;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAt { get; }
    string EventType { get; }
}

public sealed record MedicationDispensedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid DispenseId,
    Guid MedicationId,
    Guid LocationId,
    int Quantity,
    string PatientId,
    string RequestedBy) : IIntegrationEvent
{
    public string EventType => nameof(MedicationDispensedEvent);
}

public sealed record LowStockDetectedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid MedicationId,
    Guid LocationId,
    int QuantityOnHand,
    int ReorderLevel) : IIntegrationEvent
{
    public string EventType => nameof(LowStockDetectedEvent);
}

public sealed record RestockRequestedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid MedicationId,
    Guid LocationId,
    int Quantity,
    string PerformedBy) : IIntegrationEvent
{
    public string EventType => nameof(RestockRequestedEvent);
}
