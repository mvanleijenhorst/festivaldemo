namespace FestivalDemo.WebServer.Infrastructure.WebSockets
{
    public interface IWebSocketMessage
    {
        WebSocketCommandType MessageType { get; }
    }
}
