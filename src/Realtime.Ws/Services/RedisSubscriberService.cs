using MessagePack;
using Microsoft.Extensions.Logging;
using Shared;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Handlers;

namespace WS.Services
{
    public class RedisSubscriberService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisSubscriberService> _logger;
        private readonly ChatWebSocketHandler _chatWebSocketHandler;

        public RedisSubscriberService(
            IConnectionMultiplexer redis, 
            ILogger<RedisSubscriberService> logger, 
            ChatWebSocketHandler chatWebSocketHandler)
        {
            _redis = redis;
            _logger = logger;
            _chatWebSocketHandler = chatWebSocketHandler;
        }

        public async Task SubscribeAsync(CancellationToken cancellationToken = default)
        {
            var subscriber = _redis.GetSubscriber();

            await subscriber.SubscribeAsync("realtime:chat", async (channel, message) =>
            {
                try
                {
                    var data = MessagePackSerializer.Deserialize<ChatMessage>((byte[])message);

                    _logger.LogInformation("Received message from Redis: {@Message}", data);

                    await _chatWebSocketHandler.HandleMessageEventAsync(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process Redis message");
                }
            });

            _logger.LogInformation("Subscribed to Redis channel: {ChannelName}", "realtime:chat");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
