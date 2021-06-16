using System;
using System.Collections.Generic;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.Infrastructure.WebSockets.Messages
{
    public record AddGuestCommandSerializer 
    {
        public static AddGuestCommand Deserialize(byte[] bytes)
        {
            var position = sizeof(int);
            var guestId = BitConverter.ToInt32(bytes, position);

            position += sizeof(int);
            var isFollower = BitConverter.ToBoolean(bytes, position);

            return new AddGuestCommand(guestId, isFollower);
        }

        public static byte[] Serialize(AddGuestCommand command)
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.AddGuest));
            bytes.AddRange(BitConverter.GetBytes(command.GuestId));
            bytes.AddRange(BitConverter.GetBytes(command.IsFollower));
            return bytes.ToArray();
        }
    }
}
