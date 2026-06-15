using MedicationPlatform.Common.Testing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<NotificationStore>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => Results.Ok(new
{
    service = "NotificationService",
    purpose = "Surfaces low-stock, restock, and workflow notifications."
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", checkedAt = DateTimeOffset.UtcNow }));

app.MapGet("/notifications", (NotificationStore store) =>
    Results.Ok(store.GetMessages()));

app.MapPost("/notifications", (CreateNotification request, NotificationStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new { error = "Title is required." });
    }

    var message = store.Add(request.Severity, request.Title, request.Body);
    return Results.Created($"/notifications/{message.Id}", message);
});

app.MapPost("/notifications/{id:guid}/acknowledge", (Guid id, NotificationStore store) =>
    store.Acknowledge(id)
        ? Results.NoContent()
        : Results.NotFound(new { error = "Notification was not found." }));

app.Run();

public sealed record CreateNotification(
    string Severity,
    string Title,
    string Body);
