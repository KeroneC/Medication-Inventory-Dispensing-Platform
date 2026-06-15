using MedicationPlatform.Common.Domain;
using MedicationPlatform.Common.Events;
using MedicationPlatform.Common.Testing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InventoryStore>();
builder.Services.AddSingleton<EventCollector>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => Results.Ok(new
{
    service = "InventoryService",
    purpose = "Tracks medication stock levels, restock actions, and low-stock detection."
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", checkedAt = DateTimeOffset.UtcNow }));

app.MapGet("/medications", (InventoryStore store) =>
    Results.Ok(store.Medications));

app.MapGet("/locations", (InventoryStore store) =>
    Results.Ok(store.Locations));

app.MapGet("/inventory", (InventoryStore store) =>
{
    var inventory =
        from stock in store.GetStock()
        join medication in store.Medications on stock.MedicationId equals medication.Id
        join location in store.Locations on stock.LocationId equals location.Id
        select new
        {
            stock.MedicationId,
            MedicationName = medication.Name,
            medication.Strength,
            medication.Form,
            medication.IsControlledSubstance,
            medication.RequiresRefrigeration,
            stock.LocationId,
            LocationName = location.Name,
            stock.QuantityOnHand,
            stock.ReorderLevel,
            IsLowStock = stock.QuantityOnHand <= stock.ReorderLevel,
            stock.ExpirationDate,
            stock.LotNumber
        };

    return Results.Ok(inventory);
});

app.MapGet("/inventory/low-stock", (InventoryStore store) =>
    Results.Ok(store.GetLowStock()));

app.MapPost("/inventory/restock", (RestockRequest request, InventoryStore store, EventCollector events) =>
{
    var (success, updatedItem, error) = store.Restock(request);
    if (!success || updatedItem is null)
    {
        return Results.BadRequest(new { error });
    }

    events.Publish(new RestockRequestedEvent(
        Guid.NewGuid(),
        DateTimeOffset.UtcNow,
        request.MedicationId,
        request.LocationId,
        request.Quantity,
        request.PerformedBy));

    return Results.Ok(updatedItem);
});

app.MapPost("/inventory/dispense", (DispenseRequest request, InventoryStore store, EventCollector events) =>
{
    var (success, result, error) = store.Dispense(request);
    if (!success || result is null)
    {
        return Results.BadRequest(new { error });
    }

    events.Publish(new MedicationDispensedEvent(
        Guid.NewGuid(),
        DateTimeOffset.UtcNow,
        result.DispenseId,
        request.MedicationId,
        request.LocationId,
        request.Quantity,
        request.PatientId,
        request.RequestedBy));

    if (result.LowStockDetected)
    {
        events.Publish(new LowStockDetectedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            request.MedicationId,
            request.LocationId,
            result.QuantityRemaining,
            store.GetStock().Single(item =>
                item.MedicationId == request.MedicationId &&
                item.LocationId == request.LocationId).ReorderLevel));
    }

    return Results.Ok(result);
});

app.MapGet("/events", (EventCollector events) =>
    Results.Ok(events.AllEvents));

app.Run();
