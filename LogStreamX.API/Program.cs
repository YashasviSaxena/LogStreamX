using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LogStreamX.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================
// DATABASE (POSTGRES)
// =======================
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql =>
        {
            npgsql.EnableRetryOnFailure(5);
            npgsql.CommandTimeout(30);
        }));

// =======================
// BACKGROUND WORKER
// =======================
builder.Services.AddHostedService<LogBackgroundWorker>();

var app = builder.Build();

// =======================
// MIDDLEWARE
// =======================
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// Redirect root → Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();