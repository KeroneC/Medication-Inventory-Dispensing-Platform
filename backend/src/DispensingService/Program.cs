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

var dispensingHistory = new List<DispenseResult>();

app.MapGet("/", () => Results.Ok(new
{
    service = "DispensingService",
    purpose = "Validates and records medication dispense workflows."
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", checkedAt = DateTimeOffset.UtcNow }));

app.MapGet("/dispenses", () =>
    Results.Ok(dispensingHistory.OrderByDescending(item => item.DispenseId)));

app.MapPost("/dispenses", (DispenseRequest request, InventoryStore store, EventCollector events) =>
{
    // This business rule is intentionally easy to find and test.
    if (string.IsNullOrWhiteSpace(request.PatientId))
    {
        return Results.BadRequest(new { error = "PatientId is required before medication can be dispensed." });
    }

    var (success, result, error) = store.Dispense(request);
    if (!success || result is null)
    {
        return Results.BadRequest(new { error });
    }

    dispensingHistory.Add(result);

    events.Publish(new MedicationDispensedEvent(
        Guid.NewGuid(),
        DateTimeOffset.UtcNow,
        result.DispenseId,
        request.MedicationId,
        request.LocationId,
        request.Quantity,
        request.PatientId,
        request.RequestedBy));

    return Results.Created($"/dispenses/{result.DispenseId}", result);
});

app.MapGet("/events", (EventCollector events) =>
    Results.Ok(events.AllEvents));

app.Run();
