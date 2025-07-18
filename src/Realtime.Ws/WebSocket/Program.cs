using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WS.Services;
using WS.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<ChatWebSocketHandler>();

var app = builder.Build();

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var handler = app.Services.GetRequiredService<ChatWebSocketHandler>();
        await handler.HandleAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

Console.WriteLine("WebSocket server started on ws://localhost:5000/ws");
await app.RunAsync();
