using MessagePack;
using Shared;
using StackExchange.Redis;

namespace Gateway.Services
{
    public class RedisPublisherService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPublisherService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task PublishAsync(ChatMessage msg)
        {
            var payload = MessagePackSerializer.Serialize(msg);
            var db = _redis.GetSubscriber();
            await db.PublishAsync("realtime:chat", payload);
        }
    }
}
