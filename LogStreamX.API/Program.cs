using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ✅ ROOT ENDPOINT FIX
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        status = "LogStreamX API Running 🚀",
        endpoints = new[]
        {
            "/api/logs"
        }
    });
});

// Render port fix
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");