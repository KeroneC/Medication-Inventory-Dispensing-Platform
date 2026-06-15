using MedicationPlatform.Common.Domain;
using MedicationPlatform.Common.Events;

namespace MedicationPlatform.Common.Testing;

public sealed class InventoryStore
{
    private readonly object syncRoot = new();
    private readonly List<StockItem> stockItems = SeedData.Stock.ToList();

    public IReadOnlyList<Medication> Medications => SeedData.Medications;
    public IReadOnlyList<InventoryLocation> Locations => SeedData.Locations;

    public IReadOnlyList<StockItem> GetStock()
    {
        lock (syncRoot)
        {
            return stockItems.ToList();
        }
    }

    public IReadOnlyList<StockItem> GetLowStock()
    {
        lock (syncRoot)
        {
            return stockItems
                .Where(item => item.QuantityOnHand <= item.ReorderLevel)
                .ToList();
        }
    }

    public (bool Success, StockItem? UpdatedItem, string Error) Restock(RestockRequest request)
    {
        if (request.Quantity <= 0)
        {
            return (false, null, "Restock quantity must be greater than zero.");
        }

        lock (syncRoot)
        {
            var index = stockItems.FindIndex(item =>
                item.MedicationId == request.MedicationId &&
                item.LocationId == request.LocationId);

            if (index < 0)
            {
                return (false, null, "No inventory record exists for that medication and location.");
            }

            var current = stockItems[index];
            var updated = current with { QuantityOnHand = current.QuantityOnHand + request.Quantity };
            stockItems[index] = updated;
            return (true, updated, string.Empty);
        }
    }

    public (bool Success, DispenseResult? Result, string Error) Dispense(DispenseRequest request)
    {
        if (request.Quantity <= 0)
        {
            return (false, null, "Dispense quantity must be greater than zero.");
        }

        lock (syncRoot)
        {
            var index = stockItems.FindIndex(item =>
                item.MedicationId == request.MedicationId &&
                item.LocationId == request.LocationId);

            if (index < 0)
            {
                return (false, null, "Medication is not stocked at that location.");
            }

            var current = stockItems[index];
            if (current.QuantityOnHand < request.Quantity)
            {
                return (false, null, $"Only {current.QuantityOnHand} units are available.");
            }

            var remaining = current.QuantityOnHand - request.Quantity;
            var updated = current with { QuantityOnHand = remaining };
            stockItems[index] = updated;

            var result = new DispenseResult(
                Guid.NewGuid(),
                request.MedicationId,
                request.LocationId,
                request.Quantity,
                remaining,
                remaining <= current.ReorderLevel,
                remaining <= current.ReorderLevel
                    ? "Medication dispensed and low-stock threshold was reached."
                    : "Medication dispensed successfully.");

            return (true, result, string.Empty);
        }
    }
}

public sealed class EventCollector
{
    private readonly List<IIntegrationEvent> events = [];

    public IReadOnlyList<IIntegrationEvent> AllEvents => events.ToList();

    public void Publish(IIntegrationEvent integrationEvent)
    {
        // A production implementation would publish to RabbitMQ, Azure Service Bus, or Kafka.
        events.Add(integrationEvent);
    }
}

public sealed class AuditLogStore
{
    private readonly List<AuditRecord> records =
    [
        new AuditRecord(Guid.NewGuid(), DateTimeOffset.UtcNow.AddMinutes(-35), "system", "Seeded", "Inventory", "demo",
            "Demo inventory data was loaded for local learning workflows.")
    ];

    public IReadOnlyList<AuditRecord> GetRecords() =>
        records.OrderByDescending(record => record.OccurredAt).ToList();

    public AuditRecord Add(string actor, string action, string entityType, string entityId, string summary)
    {
        var record = new AuditRecord(Guid.NewGuid(), DateTimeOffset.UtcNow, actor, action, entityType, entityId, summary);
        records.Add(record);
        return record;
    }
}

public sealed class NotificationStore
{
    private readonly List<NotificationMessage> messages =
    [
        new NotificationMessage(Guid.NewGuid(), DateTimeOffset.UtcNow.AddMinutes(-15), "Info",
            "Daily cabinet check ready", "Emergency Department cabinet counts are ready for review.", false)
    ];

    public IReadOnlyList<NotificationMessage> GetMessages() =>
        messages.OrderByDescending(message => message.CreatedAt).ToList();

    public NotificationMessage Add(string severity, string title, string body)
    {
        var message = new NotificationMessage(Guid.NewGuid(), DateTimeOffset.UtcNow, severity, title, body, false);
        messages.Add(message);
        return message;
    }

    public bool Acknowledge(Guid id)
    {
        var index = messages.FindIndex(message => message.Id == id);
        if (index < 0)
        {
            return false;
        }

        messages[index] = messages[index] with { Acknowledged = true };
        return true;
    }
}
