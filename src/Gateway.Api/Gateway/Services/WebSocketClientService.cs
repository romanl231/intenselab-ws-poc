using MessagePack;
using Shared;
using System.Net.WebSockets;

namespace Gateway.Services
{
    public class WebSocketClientService
    {
        private readonly ClientWebSocket _client;
        private readonly Uri _wsUri;

        public WebSocketClientService(IConfiguration config)
        {
            _client = new ClientWebSocket();
            _wsUri = new Uri(config["WsServerUrl"]);
        }

        public async Task ConnectAsync()
        {
            if (_client.State != WebSocketState.Open)
                await _client.ConnectAsync(_wsUri, CancellationToken.None);
        }

        public async Task SendMessageAsync(ChatMessage msg)
        {
            await ConnectAsync();

            var payload = MessagePackSerializer.Serialize(msg);
            await _client.SendAsync(
                payload, 
                WebSocketMessageType.Binary, 
                true, 
                CancellationToken.None);
        }
    }
}
