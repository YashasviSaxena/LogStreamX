using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LogStreamX.API.Services;

var builder = WebApplication.CreateBuilder(args);

// =======================
// CONTROLLERS
// =======================
builder.Services.AddControllers();

// =======================
// SWAGGER
// =======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================
// DATABASE (POSTGRES - RENDER SAFE)
// =======================
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql =>
        {
            npgsql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);

            npgsql.CommandTimeout(30);
        }));

// =======================
// BACKGROUND WORKER (KAFKA CONSUMER)
// =======================
builder.Services.AddHostedService<LogBackgroundWorker>();

var app = builder.Build();

// =======================
// MIDDLEWARE PIPELINE
// =======================

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX.API v1");
});

// Routing
app.UseRouting();

app.UseAuthorization();

// Controllers
app.MapControllers();

// Root redirect → Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();