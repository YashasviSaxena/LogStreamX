using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===================== CONTROLLERS =====================
builder.Services.AddControllers();

// ===================== DB =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ===================== SWAGGER (IMPORTANT FIX) =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build app
var app = builder.Build();

// ===================== MIDDLEWARE ORDER (CRITICAL) =====================

// Swagger MUST come BEFORE auth/authorization
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX.API v1");

    // IMPORTANT: makes Swagger open at /swagger
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ===================== RENDER PORT FIX =====================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");