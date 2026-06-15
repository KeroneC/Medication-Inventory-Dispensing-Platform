using MedicationPlatform.Common.Testing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AuditLogStore>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => Results.Ok(new
{
    service = "AuditService",
    purpose = "Stores traceable audit records for regulated healthcare workflows."
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", checkedAt = DateTimeOffset.UtcNow }));

app.MapGet("/audit-logs", (AuditLogStore store) =>
    Results.Ok(store.GetRecords()));

app.MapPost("/audit-logs", (CreateAuditRecord request, AuditLogStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Actor) || string.IsNullOrWhiteSpace(request.Action))
    {
        return Results.BadRequest(new { error = "Actor and Action are required for an audit record." });
    }

    var record = store.Add(
        request.Actor,
        request.Action,
        request.EntityType,
        request.EntityId,
        request.Summary);

    return Results.Created($"/audit-logs/{record.Id}", record);
});

app.Run();

public sealed record CreateAuditRecord(
    string Actor,
    string Action,
    string EntityType,
    string EntityId,
    string Summary);
