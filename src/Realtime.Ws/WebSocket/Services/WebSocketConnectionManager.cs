using System.Net.WebSockets;
using System.Collections.Concurrent;

namespace WS.Services
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new();

        public void AddSocket(string id, WebSocket socket)
        {
            _sockets.TryAdd(id, socket);
        }

        public WebSocket? GetSocketById(string id)
        {
            _sockets.TryGetValue(id, out var socket);
            return socket;
        }

        public IEnumerable<WebSocket> GetAll() => _sockets.Values;

        public void RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out _);
        }
    }
}
