using Microsoft.EntityFrameworkCore;
using LogStreamX.Infrastructure.Data;
using LogStreamX.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<LogDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<KafkaConsumerService>();

builder.Build().Run();