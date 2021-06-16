using Communications.Messages;
using System;
using System.Collections.Generic;

namespace Serializations.Messages
{
    public static class UpdateGuestCommandSerializer
    {
        public static UpdateGuestCommand Deserialize(byte[] bytes)
        {
            var position = sizeof(int);
            var guestId = BitConverter.ToInt32(bytes, position);

            position += sizeof(int);
            var logitude = BitConverter.ToSingle(bytes, position);

            position += sizeof(float);
            var latitude = BitConverter.ToSingle(bytes, position);

            return new UpdateGuestCommand(guestId, logitude, latitude);
        }

        public static byte[] Serialize(UpdateGuestCommand command)
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.UpdateGuest));
            bytes.AddRange(BitConverter.GetBytes(command.GuestId));
            bytes.AddRange(BitConverter.GetBytes(command.Longitude));
            bytes.AddRange(BitConverter.GetBytes(command.Latitude));
            return bytes.ToArray();
        }
    }
}
