using Communications.Messages;
using Serializations.Messages;
using System;

namespace Serializations
{
    public static class WebSocketMessageSerializer
    {
        public static object Deserialize(byte[] bytes)
        {
            WebSocketCommandType messageType = (WebSocketCommandType)BitConverter.ToInt32(bytes, 0);
            if (messageType is WebSocketCommandType.OpenFestival)
            {
                return OpenFestivalCommandSerializer.Deserialize();
            }
            else if (messageType is WebSocketCommandType.CloseFestival)
            {
                return CloseFestivalCommandSerializer.Deserialize();
            }
            else if (messageType is WebSocketCommandType.AddGuest)
            {
                return AddGuestCommandSerializer.Deserialize(bytes);
            }
            else if (messageType is WebSocketCommandType.RemoveGuest)
            {
                return RemoveGuestCommandSerializer.Deserialize(bytes);
            }
            else if (messageType is WebSocketCommandType.UpdateGuest)
            {
                return UpdateGuestCommandSerializer.Deserialize(bytes);
            }
            else if (messageType is WebSocketCommandType.BuildingInfo)
            {
                return BuildingInfoCommandSerializer.Deserialize(bytes);
            }

            throw new NotSupportedException($"Message type '{messageType}' is not supported.");
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
