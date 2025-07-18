using Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatMessageController : ControllerBase
    {
        private readonly RedisPublisherService _redis;

        public ChatMessageController(RedisPublisherService redis) 
        {
            _redis = redis;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] ChatMessage message)
        {
            message.Event = "send";
            await _redis.PublishAsync(message);
            return Ok();
        }

        [HttpPost("join")]
        public async Task<IActionResult> Join([FromBody] ChatMessage message)
        {
            message.Event = "join";
            await _redis.PublishAsync(message);
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> Leave([FromBody] ChatMessage message)
        {
            message.Event = "leave";
            await _redis.PublishAsync(message);
            return Ok();
        }
    }
}
