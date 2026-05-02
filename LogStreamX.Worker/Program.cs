using LogStreamX.Worker;
using LogStreamX.Worker.Services;
using LogStreamX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// DB registration (MANDATORY)
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// FIX: Singleton (NOT scoped)
builder.Services.AddSingleton<KafkaConsumerService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();