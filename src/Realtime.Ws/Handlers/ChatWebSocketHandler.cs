using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WS.Services;
using Shared;

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

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _manager.RemoveSocket(id);
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                        break;
                    }

                    var message = MessagePackSerializer.Deserialize<ChatMessage>(buffer[..result.Count]);
                    await HandleMessageEventAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Socket error: {ex.Message}");
                _manager.RemoveSocket(id);
            }
        }

        public async Task HandleMessageEventAsync(ChatMessage msg)
        {
            switch (msg.Event)
            {
                case "send":
                    Console.WriteLine($"[{msg.Sender}]: {msg.Text}");
                    await BroadcastAsync(msg);
                    break;

                case "join":
                    Console.WriteLine($"{msg.Sender} joined the chat");
                    await BroadcastAsync(msg);
                    break;

                case "leave":
                    Console.WriteLine($"{msg.Sender} left the chat");
                    await BroadcastAsync(msg);
                    break;

                default:
                    Console.WriteLine($"Unknown event: {msg.Event}");
                    break;
            }
        }

        private async Task BroadcastAsync(ChatMessage message)
        {
            var payload = MessagePackSerializer.Serialize(message);

            foreach (var ws in _manager.GetAll())
            {
                if (ws.State == WebSocketState.Open)
                {
                    try
                    {
                        await ws.SendAsync(payload, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send message: {ex.Message}");
                    }
                }
            }
        }
    }
}
