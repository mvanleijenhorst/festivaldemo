using System;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;
using FestivalDemo.WebServer.Infrastructure.WebSockets.Messages;

namespace FestivalDemo.WebServer.Infrastructure.WebSockets
{
    public static class WebSocketMessageSerializer
    {
        public static object Deserialize(byte[] bytes)
        {
            WebSocketCommandType messageType = (WebSocketCommandType)BitConverter.ToInt32(bytes, 0);
            return messageType switch
            {
                WebSocketCommandType.OpenFestival => OpenFestivalCommandSerializer.Deserialize(),
                WebSocketCommandType.CloseFestival => CloseFestivalCommandSerializer.Deserialize(),
                WebSocketCommandType.AddGuest => AddGuestCommandSerializer.Deserialize(bytes),
                WebSocketCommandType.RemoveGuest => RemoveGuestCommandSerializer.Deserialize(bytes),
                WebSocketCommandType.UpdateGuest => UpdateGuestCommandSerializer.Deserialize(bytes),
                WebSocketCommandType.BuildingInfo => BuildingInfoCommandSerializer.Deserialize(bytes),
                _ => throw new NotSupportedException($"Message type '{messageType}' is not supported."),
            };
        }

        public static byte[] Serialize(object message)
        {
            switch (message)
            {
                case OpenFestivalCommand cmd: 
                    return OpenFestivalCommandSerializer.Serialize();
                case CloseFestivalCommand cmd:
                    return CloseFestivalCommandSerializer.Serialize();
                case AddGuestCommand cmd:
                    return AddGuestCommandSerializer.Serialize(cmd);
                case RemoveGuestCommand cmd: 
                    return RemoveGuestCommandSerializer.Serialize(cmd);
                case UpdateGuestCommand cmd:
                    return UpdateGuestCommandSerializer.Serialize(cmd);
                case BuildingInfoCommand cmd:
                    return BuildingInfoCommandSerializer.Serialize(cmd);
                default:
                    throw new NotSupportedException($"Message type '{message.GetType()}' is not supported.");
            };
        }
    }
}
