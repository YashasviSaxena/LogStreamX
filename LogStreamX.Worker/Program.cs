using LogStreamX.Infrastructure.Data;
using LogStreamX.Worker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Kafka Worker
builder.Services.AddHostedService<KafkaConsumerService>();

var host = builder.Build();
host.Run();