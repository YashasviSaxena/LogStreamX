using LogStreamX.Worker;
using LogStreamX.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// DI
builder.Services.AddSingleton<KafkaConsumerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();