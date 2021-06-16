using Communications.Messages;
using System;
using System.Collections.Generic;

namespace Serializations.Messages
{
    public static class RemoveGuestCommandSerializer
    {
        public static RemoveGuestCommand Deserialize(byte[] bytes)
        {
            var position = sizeof(int);
            var guestId = BitConverter.ToInt32(bytes, position);

            return new RemoveGuestCommand(guestId);
        }

        public static byte[] Serialize(RemoveGuestCommand command)
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.AddGuest));
            bytes.AddRange(BitConverter.GetBytes(command.GuestId));
            return bytes.ToArray();
        }
    }
}
