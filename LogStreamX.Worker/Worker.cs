using LogStreamX.Worker.Services;
using Microsoft.Extensions.Hosting;

namespace LogStreamX.Worker
{
    public class Worker : BackgroundService
    {
        private readonly KafkaConsumerService _service;

        public Worker(KafkaConsumerService service)
        {
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.StartConsuming(stoppingToken);
        }
    }
}