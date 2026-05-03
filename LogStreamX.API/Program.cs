using Microsoft.OpenApi.Models;
using LogStreamX.API.Models;

var builder = WebApplication.CreateBuilder(args);

// ======================
// SERVICES
// ======================
builder.Services.AddControllers();

// IMPORTANT: Static files for index.html
builder.Services.AddDirectoryBrowser();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LogStreamX API",
        Version = "v1"
    });
});

// CORS FIX
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

var app = builder.Build();

// ======================
// PIPELINE ORDER (VERY IMPORTANT)
// ======================

// MUST serve wwwroot (fixes index.html 404)
app.UseDefaultFiles();   // 👈 IMPORTANT
app.UseStaticFiles();    // 👈 IMPORTANT

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
// IN-MEMORY STORAGE
// ======================
var logs = new List<LogEvent>();


// ======================
// API: PUSH LOG
// ======================
app.MapPost("/api/LogPush", (LogEvent log) =>
{
    logs.Add(new LogEvent
    {
        EventId = log.EventId,
        Message = log.Message,
        Source = log.Source,
        CreatedAt = DateTime.UtcNow
    });

    return Results.Ok(new
    {
        status = "Sent OK",
        eventId = log.EventId
    });
});


// ======================
// API: GET LOGS
// ======================
app.MapGet("/api/logs", () =>
{
    return Results.Ok(logs);
});


// ======================
// HEALTH CHECK
// ======================
app.MapGet("/health", () => new
{
    status = "OK",
    time = DateTime.UtcNow
});

app.Run();