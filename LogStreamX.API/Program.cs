using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LogStreamX.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Worker
builder.Services.AddHostedService<LogBackgroundWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// 🔥 ENABLE STATIC FILES (THIS FIXES YOUR UI ISSUE)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// UI becomes default page
app.MapGet("/", () => Results.Redirect("/index.html"));

app.Run();