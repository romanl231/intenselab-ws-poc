using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WS.Services;

namespace WS.Handlers
{
    public class ChatWebSocketHandler
    {
        private readonly WebSocketConnectionManager _manager;

        public ChatWebSocketHandler(WebSocketConnectionManager manager)
        {
            _manager = manager;
        }

        public async Task HandleAsync(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _manager.AddSocket(id, socket);

            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _manager.RemoveSocket(id);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                }
                else
                {
                    var data = MessagePackSerializer.Deserialize<string>(buffer[..result.Count]);
                    Console.WriteLine($"Received: {data}");

                    var payload = MessagePackSerializer.Serialize($"Echo: {data}");
                    foreach (var ws in _manager.GetAll())
                    {
                        if (ws.State == WebSocketState.Open)
                            await ws.SendAsync(payload, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                }
            }
        }
    }
}
