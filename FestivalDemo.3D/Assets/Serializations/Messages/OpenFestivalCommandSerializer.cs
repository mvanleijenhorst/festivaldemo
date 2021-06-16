using Communications.Messages;
using System;
using System.Collections.Generic;

namespace Serializations.Messages
{
    public static class OpenFestivalCommandSerializer
    {
        public static OpenFestivalCommand Deserialize()
        {
            return new OpenFestivalCommand();
        }

        public static byte[] Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.OpenFestival));
            return bytes.ToArray();
        }
    }
}
