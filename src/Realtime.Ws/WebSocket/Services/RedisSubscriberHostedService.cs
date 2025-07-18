using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Services;

namespace WS.Services
{
    public class RedisSubscriberHostedService : IHostedService
    {
        private readonly RedisSubscriberService _subscriber;
        private readonly ILogger<RedisSubscriberHostedService> _logger;

        public RedisSubscriberHostedService(
            RedisSubscriberService subscriber, 
            ILogger<RedisSubscriberHostedService> logger)
        {
            _subscriber = subscriber;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting RedisSubscriberHostedService...");

            Task.Run(() => _subscriber.SubscribeAsync(cancellationToken), cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Stopping RedisSubscriberHostedService...");
            return Task.CompletedTask;
        }
    }
}
