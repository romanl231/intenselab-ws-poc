using Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatMessageController : ControllerBase
    {
        private readonly WebSocketClientService _wsService;

        public ChatMessageController(WebSocketClientService wsService) 
        {
            _wsService = wsService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] ChatMessage message)
        {
            message.Event = "send";
            await _wsService.SendMessageAsync(message);
            return Ok();
        }

        [HttpPost("join")]
        public async Task<IActionResult> Join([FromBody] ChatMessage message)
        {
            message.Event = "join";
            await _wsService.SendMessageAsync(message);
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> Leave([FromBody] ChatMessage message)
        {
            message.Event = "leave";
            await _wsService.SendMessageAsync(message);
            return Ok();
        }
    }
}
