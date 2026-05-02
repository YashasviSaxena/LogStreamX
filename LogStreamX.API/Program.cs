using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// IMPORTANT: SAFE CONNECTION HANDLING
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(conn);
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        p => p.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// SAFE PIPELINE
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ROOT ENDPOINT (SAFE)
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        status = "LogStreamX API Running 🚀",
        endpoints = new[] { "/api/logs" }
    });
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");