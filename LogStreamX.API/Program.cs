using LogStreamX.Infrastructure.Data;
using LogStreamX.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Worker
builder.Services.AddHostedService<LogBackgroundWorker>();

var app = builder.Build();

// 🚨 ORDER IS CRITICAL
app.UseRouting();

// ✅ STATIC FILES MUST COME BEFORE MAPS
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// SPA fallback (VERY IMPORTANT)
app.MapFallbackToFile("index.html");

app.Run();