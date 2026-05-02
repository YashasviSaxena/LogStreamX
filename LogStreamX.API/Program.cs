using Microsoft.OpenApi.Models;
using LogStreamX.API.Models;

var builder = WebApplication.CreateBuilder(args);

// ======================
// SERVICES
// ======================
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LogStreamX API",
        Version = "v1"
    });
});

// CORS (UI + Swagger FIX)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ======================
// PIPELINE
// ======================
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();


// ======================
// IN-MEMORY LOG STORAGE (NO DB)
// ======================
var logs = new List<LogEvent>();


// ======================
// PUSH LOG (Swagger uses this)
// ======================
app.MapPost("/api/LogPush", (LogEvent log) =>
{
    logs.Add(new LogEvent
    {
        EventId = log.EventId,
        Message = log.Message,
        Source = log.Source,
        CreatedAt = DateTime.UtcNow   // ALWAYS SERVER TIME
    });

    return Results.Ok(new
    {
        status = "Sent Successfully (Memory Mode)",
        eventId = log.EventId
    });
});


// ======================
// GET LOGS (UI uses this)
// ======================
app.MapGet("/api/logs", () =>
{
    return Results.Ok(logs);
});


// ======================
// HEALTH CHECK (Render safe)
// ======================
app.MapGet("/health", () => new
{
    status = "OK",
    service = "LogStreamX",
    time = DateTime.UtcNow
});

app.Run();