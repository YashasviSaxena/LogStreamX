using LogStreamX.Infrastructure.Data;
using LogStreamX.Worker.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// DB
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Kafka Consumer
builder.Services.AddHostedService<KafkaConsumerService>();

var app = builder.Build();

app.Run();