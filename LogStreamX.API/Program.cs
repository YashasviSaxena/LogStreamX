using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LogStreamX.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

// Controllers
builder.Services.AddControllers();

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LogStreamX API",
        Version = "v1"
    });
});

// CORS (optional but useful for frontend later)
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

// -------------------- PIPELINE --------------------

// Swagger (IMPORTANT FOR RENDER)
app.UseSwagger();
app.UseSwaggerUI();

// Static files (fixes /index.html 404 if present)
app.UseStaticFiles();

// CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// fallback (ONLY if you actually want index.html SPA support)
app.MapFallbackToFile("index.html");

app.Run();