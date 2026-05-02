using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===================== CONTROLLERS =====================
builder.Services.AddControllers();

// ===================== DB CONTEXT =====================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ===================== SWAGGER (FIXED) =====================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "LogStreamX API",
        Version = "v1"
    });

    // 🔥 FIX: prevents Swagger crash (VERY IMPORTANT)
    options.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();

// ===================== PIPELINE =====================

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LogStreamX API v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// ===================== ROOT HEALTH =====================
app.MapGet("/", () => "LogStreamX API Running 🚀");

app.Run();