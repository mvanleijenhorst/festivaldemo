namespace Serializations
{
    public interface IWebSocketMessage
    {
        WebSocketCommandType MessageType { get; }
    }
}
